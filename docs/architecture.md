# 🏛️ Architecture Design

This document describes the architecture of the Enterprise Application Platform, which is designed to be a highly scalable, data-driven, and pluggable system.

## Core Philosophy

The platform is built on the principle of **"Configuration over Code."** This means that most of the application's behavior is defined by data in the database rather than being hard-coded into the application logic. This approach allows for greater flexibility and easier customization without requiring code changes.

## System Architecture

The system follows a classic layered architecture, with a clear separation of concerns between the different layers:

*   **Presentation Layer**: This layer is responsible for handling user interactions and presenting data to the user. It includes the API controllers that expose the system's functionality to the outside world.
*   **Application Layer**: This layer contains the core business logic of the application. It is responsible for orchestrating the various services and components to fulfill user requests.
*   **Domain Layer**: This layer represents the core business concepts and entities of the application. It is the heart of the system and is responsible for maintaining the integrity of the business data.
*   **Infrastructure Layer**: This layer is responsible for all the technical concerns of the application, such as database access, logging, and external service integrations.

## Key Components

*   **`CoreHostService`**: The entry point of the application, responsible for initializing and starting all the core services.
*   **`PluginEngine`**: The heart of the plug-in system, responsible for discovering, loading, and managing plug-ins.
*   **`DatabaseService`**: Provides an abstraction layer for all database operations, ensuring that the application is not tied to a specific database technology.
*   **`ApiController`**: The base controller for all API endpoints, providing common functionality such as authentication, authorization, and error handling.

## Data Flow

1.  A request comes into the `ApiController`.
2.  The controller authenticates and authorizes the request.
3.  The controller retrieves the necessary data from the database through the `DatabaseService`.
4.  The controller executes the business logic, which may involve calling other services or plug-ins.
5.  The controller returns a response to the client.

## Pluggable Architecture

The plug-in system is a key feature of the platform. It allows developers to extend the system's functionality without modifying the core application code. Plug-ins are discovered and loaded at runtime, and they can be used to add new features, integrate with external systems, or customize the application's behavior.

---
---

# 🏛️ 架构设计

本文档描述了企业应用平台的架构，该平台旨在成为一个高度可扩展、数据驱动和可插拔的系统。

## 核心理念

该平台建立在 **“配置优于编码”** 的原则之上。这意味着应用程序的大部分行为是由数据库中的数据定义的，而不是硬编码到应用程序逻辑中。这种方法可以在不需要更改代码的情况下，实现更大的灵活性和更轻松的自定义。

## 系统架构

该系统遵循经典的分层架构，在不同层之间明确分离了关注点：

*   **表示层**：该层负责处理用户交互并将数据呈现给用户。它包括向外界公开系统功能的 API 控制器。
*   **应用层**：该层包含应用程序的核心业务逻辑。它负责协调各种服务和组件以满足用户请求。
*   **领域层**：该层代表应用程序的核心业务概念和实体。它是系统的核心，负责维护业务数据的完整性。
*   **基础设施层**：该层负责应用程序的所有技术问题，例如数据库访问、日志记录和外部服务集成。

## 关键组件

*   **`CoreHostService`**：应用程序的入口点，负责初始化和启动所有核心服务。
*   **`PluginEngine`**：插件系统的核心，负责发现、加载和管理插件。
*   **`DatabaseService`**：为所有数据库操作提供抽象层，确保应用程序不与特定的数据库技术绑定。
*   **`ApiController`**：所有 API 端点的基本控制器，提供身份验证、授权和错误处理等通用功能。

## 数据流

1.  一个请求进入 `ApiController`。
2.  控制器对请求进行身份验证和授权。
3.  控制器通过 `DatabaseService` 从数据库中检索必要的数据。
4.  控制器执行业务逻辑，这可能涉及调用其他服务或插件。
5.  控制器向客户端返回响应。

## 可插拔架构

插件系统是该平台的一个关键特性。它允许开发人员在不修改核心应用程序代码的情况下扩展系统的功能。插件在运行时被发现和加载，它们可以用来添加新功能、与外部系统集成或自定义应用程序的行为。
