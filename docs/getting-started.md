# 🚀 Getting Started Guide

This guide will walk you through the process of setting up your development environment and running the Enterprise Application Platform for the first time.

## Prerequisites

Before you begin, you will need to have the following software installed on your machine:

*   **.NET 8 SDK**: The platform is built on .NET 8, so you will need to have the SDK installed to build and run the code.
*   **Visual Studio 2022** (or another code editor of your choice): You can use Visual Studio to open, edit, and debug the project.

## Setting up the Database

The platform uses a local SQLite database to store its data. The database file is located at `Data/myapp.db`. When you first run the application, the database will be automatically created and seeded with the initial data.

## Running the Application

There are two ways to run the application:

### 1. Using the .NET CLI

To run the application from the command line, open a terminal window in the project's root directory and run the following command:

```bash
. run.sh
```

This will start the application and listen for requests on port 5000.

### 2. Using Visual Studio

To run the application from Visual Studio, open the project in Visual Studio and press `F5`. This will build and run the application in debug mode.

## Accessing the API

Once the application is running, you can access the API at `http://localhost:5000/api`. For more information on how to use the API, see the [API Guide](./api-guide.md).

## What's Next?

Now that you have the application running, you can start exploring the code and making changes. Here are a few things you might want to try:

*   Add a new entity to the database and see how the API automatically updates to include it.
*   Create a new plugin to add a new feature to the system.
*   Customize the application's behavior by changing the configuration in the database.

---
---

# 🚀 快速入门指南

本指南将引导您完成设置开发环境和首次运行企业应用平台的过程。

## 环境要求

在开始之前，您需要在您的机器上安装以下软件：

*   **.NET 8 SDK**：本平台基于 .NET 8 构建，因此您需要安装该 SDK 以便构建和运行代码。
*   **Visual Studio 2022**（或您选择的其他代码编辑器）：您可以使用 Visual Studio 来打开、编辑和调试项目。

## 设置数据库

本平台使用本地 SQLite 数据库来存储数据。数据库文件位于 `Data/myapp.db`。当您首次运行应用程序时，数据库将被自动创建并填充初始数据。

## 运行应用程序

有两种方式可以运行本应用程序：

### 1. 使用 .NET CLI

要从命令行运行本应用程序，请在项目根目录中打开一个终端窗口，然后运行以下命令：

```bash
. run.sh
```

这将会启动应用程序并开始在 5000 端口监听请求。

### 2. 使用 Visual Studio

要从 Visual Studio 运行本应用程序，请在 Visual Studio 中打开项目，然后按 `F5`。这将在调试模式下构建并运行应用程序。

## 访问 API

应用程序运行后，您可以在 `http://localhost:5000/api` 访问 API。有关如何使用 API 的更多信息，请参阅 [API 指南](./api-guide.md)。

## 下一步

既然您已经成功运行了应用程序，就可以开始探索代码并进行修改了。您可以尝试以下几件事：

*   向数据库中添加一个新的实体，看看 API 是如何自动更新以包含它的。
*   创建一个新的插件来为系统添加新功能。
*   通过更改数据库中的配置来自定义应用程序的行为。
