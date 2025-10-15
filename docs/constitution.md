# 🏛️ 系统核心宪章 (The Constitution for .NET Platform)

> **版本**：v2.0 企业服务版  
> **目标**：实现一个高扩展性、数据驱动、插件化、可部署、可观测的企业级应用平台。  
> **适用范围**：本宪章适用于平台级服务及其所有子模块、插件及外部集成组件。  

---

## 一、核心哲学 (Core Philosophy)

### 1. 数据驱动一切
任何业务逻辑、校验规则、接口行为，都应优先通过配置表驱动：
- `TableAttributeConfig`
- `ColumnAttributeConfig`
- `ColumnValidationRule`

> 🧠 在编写任何 C# 代码之前，请问自己：
> “这个功能能否通过数据库配置实现？”

---

### 2. 配置优于编码
如果一个功能可以通过数据库配置完成，**严禁硬编码**在 C# 中实现。  
配置应成为第一驱动力，代码仅作为执行容器。

---

### 3. 插件化一切
所有独立业务模块（文件服务、AI服务、OCR识别等）都应以插件方式存在。  
插件通过 `IPlugin` 接口与 `PluginEngine` 引擎通信，可独立加载、卸载。  

---

### 4. “三不三少”原则
| 原则 | 内容 |
|------|------|
| **三不** | 不过度开发、不搞复杂架构、不破坏现有功能 |
| **三少** | 少文件、少代码、少调用 |

> 🎯 追求简洁、高效、低耦合。

---

### 5. 服务化与可部署性
系统必须可运行于以下三种模式：
- ✅ **Windows Service 模式**
- ✅ **Console 调试模式**
- ✅ **Docker 容器模式**

在 `Program.cs` 中：
```csharp
Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((context, services) => {
        services.AddHostedService<CoreHostService>();
    })
    .Build()
    .Run();
````

`CoreHostService` 负责：

* 插件加载
* 日志初始化
* 配置加载
* 安全中间件注册

---

## 二、架构设计 (Architecture Design)

### 1. 双引擎体系

| 引擎                       | 说明                                                                 |
| ------------------------ | ------------------------------------------------------------------ |
| **数据引擎 (Data Engine)**   | 包含 `CommonService`, `BaseRepository`, `SystemDbContext` 等，用于通用数据操作 |
| **插件引擎 (Plugin Engine)** | 包含 `PluginEngine`, `IPlugin`, `PluginConfig` 等，用于插件生命周期与依赖注入管理     |

---

### 2. 三层架构

**Controller → Service → Repository**

* **Controller**：仅负责 HTTP 入口与响应封装。
* **Service**：业务逻辑、校验、数据聚合。
* **Repository**：通过 EF Core / Dapper 操作数据库。

---

### 3. 安全层 (Security Layer)

由 `AuthenticationMiddleware` 管理 JWT 验证，统一设置用户上下文。
禁止从参数获取用户信息，应使用：

```csharp
var userId = UserContext.Current.UserId;
```

白名单接口从数据库表 `SecurityConfig` 动态加载。

---

## 三、开发规范 (Development Rules)

### 1. API设计

* 遵循 **RESTful** 风格；
* 所有接口必须返回统一结构：

  ```csharp
  public class ApiResponse {
      public int Code { get; set; }
      public string Message { get; set; } = string.Empty;
      public object? Data { get; set; }
  }
  ```
* 通用接口前缀：`/api/common`
* 插件接口前缀：`/api/plugins/{pluginName}`

---

### 2. 数据库交互规范

* **禁止手写 SQL 拼接**；
* 所有数据库操作必须通过 Repository 层；
* **数据驱动配置表**：

  * `TableAttributeConfig`: 控制表级行为；
  * `ColumnAttributeConfig`: 控制字段显示、可编辑性；
  * `ColumnValidationRule`: 控制校验逻辑。

---

### 3. 插件开发规范

#### 插件结构

```
/plugins
 ├── UploadPlugin.dll
 ├── AiServicePlugin.dll
 └── ...
```

#### 插件接口定义

```csharp
public interface IPlugin {
    string Name { get; }
    void Initialize(IServiceCollection services, IConfiguration config);
    void Shutdown();
}
```

#### 插件约束

* 内部可拥有 Controller、Service；
* 不使用 `[ApiController]`、`[Service]`；
* 加载与卸载均由 `PluginEngine` 控制；
* 插件可以通过依赖注入访问平台公共服务（如 `CommonService`）。

---

## 四、运行与服务化 (Service Operation)

### 1. Windows Service 支持

系统必须支持以下运行方式：

| 模式                     | 描述                                 |
| ---------------------- | ---------------------------------- |
| 🪟 **Windows Service** | 通过 `UseWindowsService()` 自动注册为系统服务 |
| 🧩 **Console 模式**      | 调试、开发使用                            |
| 🐳 **Docker 模式**       | 云端部署，基于环境变量和配置文件自适应                |

---

### 2. 日志系统 (Logging System)

#### 技术栈

* **Serilog**：核心日志框架
* **Seq**：可视化日志平台
* **Microsoft.Extensions.Logging**：系统兼容接口

#### 配置示例

```json
"Serilog": {
  "MinimumLevel": "Information",
  "WriteTo": [
    { "Name": "Console" },
    { "Name": "File", "Args": { "path": "logs/log-.txt", "rollingInterval": "Day" } }
  ]
}
```

#### 日志封装

```csharp
public static class LogHelper {
    private static readonly ILogger _logger = Log.ForContext(typeof(LogHelper));

    public static void Info(string message, object? data = null)
        => _logger.Information("{Message} | {@Data}", message, data);

    public static void Error(Exception ex, string message)
        => _logger.Error(ex, message);
}
```

#### 日志查看方式

* 控制台实时输出；
* 每日日志文件自动切割；
* 支持集成 **Seq / Kibana** 可视化追踪。

---

## 五、统一参数传递机制 (Map Parameter Passing)

### 1. 原则

* 禁止使用强类型 DTO、ViewModel；
* 所有参数必须通过 `Dictionary<string, object>`（或别名 `Map`）传递；
* 通过键值解析增强通用性。

### 2. 示例

#### Controller 层

```csharp
[HttpPost("save")]
public async Task<ApiResponse> Save([FromBody] Dictionary<string, object> paramMap)
{
    return await _commonService.SaveAsync(paramMap);
}
```

#### Service 层

```csharp
public async Task<ApiResponse> SaveAsync(Dictionary<string, object> map)
{
    string table = map["table"].ToString()!;
    var data = map["data"] as Dictionary<string, object>;
    await _repository.SaveAsync(table, data!);
    return ApiResponse.Success();
}
```

> ⚙️ Map 模式能与配置表配合使用，实现无模型化、全动态业务处理。

---

## 六、异常与错误处理 (Exception Handling)

* 所有业务异常统一使用 `BusinessException`；
* 全局异常由 `GlobalExceptionMiddleware` 捕获；
* 响应格式：

  ```json
  { "code": 500, "message": "Internal Error", "data": null }
  ```

---

## 七、核心设计总结 (Summary Matrix)

| 核心点         | 描述                                 |
| ----------- | ---------------------------------- |
| 🧩 数据驱动     | 所有行为由配置表决定                         |
| ⚙️ 插件化设计    | 功能可加载、可卸载、独立生命周期                   |
| 🧠 Map 参数传递 | 消除模型依赖，通用数据结构                      |
| 🪟 服务可运行    | 支持 Windows / Console / Docker 三种模式 |
| 🪶 日志体系     | 支持 Serilog + Seq，统一日志查看            |
| 🔒 安全统一     | JWT + 数据库白名单                       |
| 🧱 架构规范     | Controller → Service → Repository  |
| 🚫 三不三少     | 不复杂、不重复、不破坏，少代码、少调用、少依赖            |

---

## 八、版本约定 (Versioning Policy)

* 主版本号变更（如 v2→v3）代表核心设计原则修改；
* 次版本号变更（如 v2.0→v2.1）代表插件或配置体系扩展；
* 修订号变更（如 v2.0.1）仅包含Bug修复与日志增强。

---

## 九、附录 (Appendix)

### 推荐目录结构

```
/src
 ├── CoreHostService.cs
 ├── CommonService.cs
 ├── PluginEngine.cs
 ├── GlobalExceptionMiddleware.cs
 ├── LogHelper.cs
 ├── UserContext.cs
 ├── Repository/
 ├── Services/
 ├── Controllers/
 ├── Plugins/
 └── appsettings.json
```

### 责任声明

所有开发者必须遵守本宪章进行开发。
任何违背本宪章原则的实现，必须经架构委员会批准后方可合并至主干分支。

---

**📜 本文件为平台级系统宪章，是一切架构、开发、部署、维护的最高准则。**

---
---

# 🏛️ The Constitution for .NET Platform

> **Version**: v2.0 Enterprise Service Edition  
> **Objective**: To implement a highly extensible, data-driven, plugin-based, deployable, and observable enterprise-grade application platform.  
> **Scope**: This constitution applies to the platform-level service and all its sub-modules, plugins, and external integration components.  

---

## I. Core Philosophy

### 1. Data Drives Everything
Any business logic, validation rules, or API behavior should be primarily driven by configuration tables:
- `TableAttributeConfig`
- `ColumnAttributeConfig`
- `ColumnValidationRule`

> 🧠 Before writing any C# code, ask yourself:
> "Can this feature be implemented through database configuration?"

---

### 2. Configuration Over Code
If a feature can be accomplished through database configuration, **it is strictly forbidden to hard-code it in C#**.  
Configuration should be the primary driving force; code is merely the execution container.

---

### 3. Pluginize Everything
All independent business modules (e.g., File Service, AI Service, OCR Recognition) should exist as plugins.  
Plugins communicate with the `PluginEngine` through the `IPlugin` interface and can be loaded and unloaded independently.  

---

### 4. The "Three Nos & Three Lows" Principle
| Principle | Content |
|---|---|
| **Three Nos** | No over-engineering, no complex architecture, no breaking existing functionality |
| **Three Lows** | Fewer files, less code, fewer calls |

> 🎯 Strive for simplicity, efficiency, and low coupling.

---

### 5. Servitization and Deployability
The system must be runnable in the following three modes:
- ✅ **Windows Service Mode**
- ✅ **Console Debug Mode**
- ✅ **Docker Container Mode**

In `Program.cs`:
```csharp
Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((context, services) => {
        services.AddHostedService<CoreHostService>();
    })
    .Build()
    .Run();
````

`CoreHostService` is responsible for:

*   Plugin loading
*   Log initialization
*   Configuration loading
*   Security middleware registration

---

## II. Architecture Design

### 1. Dual-Engine System

| Engine | Description |
|---|---|
| **Data Engine** | Contains `CommonService`, `BaseRepository`, `SystemDbContext`, etc., for generic data operations |
| **Plugin Engine** | Contains `PluginEngine`, `IPlugin`, `PluginConfig`, etc., for managing plugin lifecycle and dependency injection |

---

### 2. Three-Layer Architecture

**Controller → Service → Repository**

*   **Controller**: Solely responsible for HTTP entry points and response wrapping.
*   **Service**: Business logic, validation, data aggregation.
*   **Repository**: Operates the database via EF Core / Dapper.

---

### 3. Security Layer

JWT validation is managed by `AuthenticationMiddleware`, which uniformly sets the user context.
Fetching user information from parameters is forbidden. Use this instead:

```csharp
var userId = UserContext.Current.UserId;
```

Whitelisted APIs are dynamically loaded from the `SecurityConfig` database table.

---

## III. Development Rules

### 1. API Design

*   Follow **RESTful** style.
*   All APIs must return a unified structure:

  ```csharp
  public class ApiResponse {
      public int Code { get; set; }
      public string Message { get; set; } = string.Empty;
      public object? Data { get; set; }
  }
  ```
*   Generic API prefix: `/api/common`
*   Plugin API prefix: `/api/plugins/{pluginName}`

---

### 2. Database Interaction Rules

*   **Manual SQL string concatenation is forbidden**.
*   All database operations must go through the Repository layer.
*   **Data-Driven Configuration Tables**:

    *   `TableAttributeConfig`: Controls table-level behavior.
    *   `ColumnAttributeConfig`: Controls field display and editability.
    *   `ColumnValidationRule`: Controls validation logic.

---

### 3. Plugin Development Rules

#### Plugin Structure

```
/plugins
 ├── UploadPlugin.dll
 ├── AiServicePlugin.dll
 └── ...
```

#### Plugin Interface Definition

```csharp
public interface IPlugin {
    string Name { get; }
    void Initialize(IServiceCollection services, IConfiguration config);
    void Shutdown();
}
```

#### Plugin Constraints

*   Can have internal Controllers and Services.
*   Do not use `[ApiController]` or `[Service]` attributes.
*   Loading and unloading are controlled by the `PluginEngine`.
*   Plugins can access platform-common services (like `CommonService`) via dependency injection.

---

## IV. Service Operation

### 1. Windows Service Support

The system must support the following execution modes:

| Mode | Description |
|---|---|
| 🪟 **Windows Service** | Automatically registers as a system service via `UseWindowsService()` |
| 🧩 **Console Mode** | Used for debugging and development |
| 🐳 **Docker Mode** | For cloud deployment, adapts based on environment variables and configuration files |

---

### 2. Logging System

#### Tech Stack

*   **Serilog**: Core logging framework
*   **Seq**: Visual logging platform
*   **Microsoft.Extensions.Logging**: System compatibility interface

#### Configuration Example

```json
"Serilog": {
  "MinimumLevel": "Information",
  "WriteTo": [
    { "Name": "Console" },
    { "Name": "File", "Args": { "path": "logs/log-.txt", "rollingInterval": "Day" } }
  ]
}
```

#### Logging Wrapper

```csharp
public static class LogHelper {
    private static readonly ILogger _logger = Log.ForContext(typeof(LogHelper));

    public static void Info(string message, object? data = null)
        => _logger.Information("{Message} | {@Data}", message, data);

    public static void Error(Exception ex, string message)
        => _logger.Error(ex, message);
}
```

#### Log Viewing Methods

*   Real-time console output.
*   Automatic daily log file rotation.
*   Supports integration with **Seq / Kibana** for visual tracking.

---

## V. Unified Parameter Passing Mechanism (Map Parameter Passing)

### 1. Principle

*   The use of strongly-typed DTOs or ViewModels is forbidden.
*   All parameters must be passed via `Dictionary<string, object>` (or its alias, `Map`).
*   Enhance generality through key-value parsing.

### 2. Example

#### Controller Layer

```csharp
[HttpPost("save")]
public async Task<ApiResponse> Save([FromBody] Dictionary<string, object> paramMap)
{
    return await _commonService.SaveAsync(paramMap);
}
```

#### Service Layer

```csharp
public async Task<ApiResponse> SaveAsync(Dictionary<string, object> map)
{
    string table = map["table"].ToString()!;
    var data = map["data"] as Dictionary<string, object>;
    await _repository.SaveAsync(table, data!);
    return ApiResponse.Success();
}
```

> ⚙️ The Map pattern, when used with configuration tables, enables model-less, fully dynamic business processing.

---

## VI. Exception and Error Handling

*   All business exceptions should uniformly use `BusinessException`.
*   Global exceptions are caught by `GlobalExceptionMiddleware`.
*   Response format:

  ```json
  { "code": 500, "message": "Internal Error", "data": null }
  ```

---

## VII. Core Design Summary Matrix

| Core Point | Description |
|---|---|
| 🧩 Data-Driven | All behavior is determined by configuration tables |
| ⚙️ Plugin-Based Design | Features are loadable, unloadable, with an independent lifecycle |
| 🧠 Map Parameter Passing | Eliminates model dependency with a generic data structure |
| 🪟 Service Runnable | Supports Windows / Console / Docker modes |
| 🪶 Logging System | Supports Serilog + Seq for unified log viewing |
| 🔒 Unified Security | JWT + database whitelist |
| 🧱 Architectural Standard | Controller → Service → Repository |
| 🚫 Three Nos & Three Lows | Not complex, not repetitive, not destructive; less code, fewer calls, fewer dependencies |

---

## VIII. Versioning Policy

*   A major version change (e.g., v2→v3) represents a modification to core design principles.
*   A minor version change (e.g., v2.0→v2.1) represents an extension to the plugin or configuration system.
*   A revision number change (e.g., v2.0.1) includes only bug fixes and logging enhancements.

---

## IX. Appendix

### Recommended Directory Structure

```
/src
 ├── CoreHostService.cs
 ├── CommonService.cs
 ├── PluginEngine.cs
 ├── GlobalExceptionMiddleware.cs
 ├── LogHelper.cs
 ├── UserContext.cs
 ├── Repository/
 ├── Services/
 ├── Controllers/
 ├── Plugins/
 └── appsettings.json
```

### Disclaimer

All developers must adhere to this constitution during development.
Any implementation that violates the principles of this constitution must be approved by the architecture committee before being merged into the main branch.

---

**📜 This document is the platform-level system constitution and serves as the supreme guideline for all architecture, development, deployment, and maintenance.**
