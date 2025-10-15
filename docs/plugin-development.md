# 🔌 Plugin Development Guide

This guide describes how to develop plugins for the Enterprise Application Platform.

## What is a Plugin?

A plugin is a self-contained unit of functionality that can be added to the system to extend its capabilities. Plugins can be used to:

*   Add new features
*   Integrate with external systems
*   Customize the application's behavior

## Creating a Plugin

To create a plugin, you need to create a new C# project that references the `Microsoft.Extensions.DependencyInjection.Abstractions` NuGet package. Your plugin must implement the `IPlugin` interface:

```csharp
public interface IPlugin
{
    void Initialize(IServiceCollection services);
}
```

The `Initialize` method is called by the `PluginEngine` when the plugin is loaded. You can use this method to register your plugin's services with the dependency injection container.

## Plugin Discovery

The `PluginEngine` discovers plugins by scanning the `plugins` directory for assemblies that contain a class that implements the `IPlugin` interface. When the application starts, the `PluginEngine` loads all the discovered plugins and calls their `Initialize` methods.

## Example Plugin

Here is an example of a simple plugin that adds a new service to the system:

```csharp
using Microsoft.Extensions.DependencyInjection;

public class MyPlugin : IPlugin
{
    public void Initialize(IServiceCollection services)
    {
        services.AddTransient<IMyService, MyService>();
    }
}

public interface IMyService
{
    string GetMessage();
}

public class MyService : IMyService
{
    public string GetMessage()
    {
        return "Hello from my plugin!";
    }
}
```

To use this plugin, you would compile it into a DLL and place it in the `plugins` directory. The `PluginEngine` will then automatically discover and load the plugin, and you will be able to inject the `IMyService` into your controllers and other services.

## Best Practices

*   Plugins should be self-contained and have no dependencies on other plugins.
*   Plugins should not modify the core application code.
*   Plugins should be well-documented.

---
---

# 🔌 插件开发指南

本指南介绍了如何为企业应用平台开发插件。

## 什么是插件？

插件是一个独立的功能单元，可以添加到系统中以扩展其功能。插件可用于：

*   添加新功能
*   与外部系统集成
*   自定义应用程序的行为

## 创建插件

要创建插件，您需要创建一个新的 C# 项目，该项目引用 `Microsoft.Extensions.DependencyInjection.Abstractions` NuGet 包。您的插件必须实现 `IPlugin` 接口：

```csharp
public interface IPlugin
{
    void Initialize(IServiceCollection services);
}
```

`Initialize` 方法在加载插件时由 `PluginEngine` 调用。您可以使用此方法向依赖注入容器注册插件的服务。

## 插件发现

`PluginEngine` 通过扫描 `plugins` 目录中包含实现 `IPlugin` 接口的类的程序集来发现插件。当应用程序启动时，`PluginEngine` 会加载所有发现的插件并调用其 `Initialize` 方法。

## 示例插件

以下是一个向系统添加新服务的简单插件示例：

```csharp
using Microsoft.Extensions.DependencyInjection;

public class MyPlugin : IPlugin
{
    public void Initialize(IServiceCollection services)
    {
        services.AddTransient<IMyService, MyService>();
    }
}

public interface IMyService
{
    string GetMessage();
}

public class MyService : IMyService
{
    public string GetMessage()
    {
        return "来自我的插件的问候！";
    }
}
```

要使用此插件，您需要将其编译成 DLL 并将其放入 `plugins` 目录中。然后，`PluginEngine` 将自动发现并加载该插件，您将能够将 `IMyService` 注入到您的控制器和其他服务中。

## 最佳实践

*   插件应自成一体，不依赖于其他插件。
*   插件不应修改核心应用程序代码。
*   插件应有良好的文档记录。
