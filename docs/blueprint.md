# Project Blueprint

## Overview

This document outlines the project's architecture and implemented features. It serves as a single source of truth for the application's design and capabilities, from its initial version to the current one.

### Core Features

*   **Modular Architecture**: The application is built with a clear separation of concerns, using directories like `src`, `Repository`, `Services`, `Controllers`, and `Plugins` to organize code.
*   **Plugin Engine**: A plugin engine (`PluginEngine.cs`) allows for extending the application's functionality by loading external assemblies.
*   **Database Migrations**: Utilizes FluentMigrator for managing database schema changes, ensuring a consistent database state across different environments.
*   **Centralized Logging**: Implements Serilog for structured logging throughout the application.
*   **Global Exception Handling**: A custom middleware (`GlobalExceptionMiddleware.cs`) is in place to catch and handle unhandled exceptions gracefully.
*   **API Documentation**: Integrated Swagger UI for interactive API documentation and testing.

---

## Current Task: Incremental Update Feature

### Objective

To implement a versioning and incremental update mechanism. This will allow for creating lightweight update packages containing only new or modified files, instead of requiring a full application redeployment for every change.

### Plan

1.  **Add Project Versioning**: Introduce a `<Version>` tag in the `myapp.csproj` file to manage the application's version number.
2.  **Create Build Scripts Directory**: A new `scripts` directory will be created to house the build and packaging scripts.
3.  **Implement Manifest Generation (`generate-manifest.csx`**):
    *   Create a C# script that runs after a `publish` command.
    *   This script will scan the `publish` directory, calculate an MD5 hash for each file, and save this information into a `manifest.json` file.
    *   The manifest will also store the application's version number.
4.  **Implement Update Package Creation (`create-update.csx`**):
    *   Create a C# script to generate the update package.
    *   The script will compare the `manifest.json` from the latest build with the one from the previous release.
    *   It will identify all new and modified files based on the hash comparison.
    *   These files will be copied to an `update` directory, preserving their original folder structure.
5.  **Integrate with VS Code Tasks**:
    *   Update `tasks.json` to define two distinct publishing tasks:
        *   **`publish-full`**: Performs a clean build, publishes the entire application, and generates a new `manifest.json`.
        *   **`publish-update`**: Publishes the application and then runs the `create-update.csx` script to generate the differential update package.

---
---

# 项目蓝图

## 概述

本文档概述了项目的架构和已实现的功能。它作为应用程序从初始版本到当前版本的设计和功能的单一事实来源。

### 核心功能

*   **模块化架构**：应用程序在构建时明确分离了关注点，使用 `src`、`Repository`、`Services`、`Controllers` 和 `Plugins` 等目录来组织代码。
*   **插件引擎**：插件引擎 (`PluginEngine.cs`) 允许通过加载外部程序集来扩展应用程序的功能。
*   **数据库迁移**：利用 FluentMigrator 管理数据库模式的变更，确保在不同环境下数据库状态的一致性。
*   **集中式日志记录**：实现 Serilog 以在整个应用程序中进行结构化日志记录。
*   **全局异常处理**：一个自定义中间件 (`GlobalExceptionMiddleware.cs`) 用于优雅地捕获和处理未处理的异常。
*   **API 文档**：集成了 Swagger UI，用于交互式 API 文档和测试。

---

## 当前任务：增量更新功能

### 目标

实现版本控制和增量更新机制。这将允许创建仅包含新增或已修改文件的轻量级更新包，而无需为每次更改都重新部署整个应用程序。

### 计划

1.  **添加项目版本控制**：在 `myapp.csproj` 文件中引入 `<Version>` 标签来管理应用程序的版本号。
2.  **创建构建脚本目录**：将创建一个新的 `scripts` 目录，用于存放构建和打包脚本。
3.  **实现清单生成 (`generate-manifest.csx`)**：
    *   创建一个在 `publish` 命令后运行的 C# 脚本。
    *   此脚本将扫描 `publish` 目录，为每个文件计算 MD5 哈希值，并将此信息保存到 `manifest.json` 文件中。
    *   该清单还将存储应用程序的版本号。
4.  **实现更新包创建 (`create-update.csx`)**：
    *   创建一个 C# 脚本来生成更新包。
    *   该脚本会将最新构建的 `manifest.json` 与上一版本的 `manifest.json` 进行比较。
    *   它将根据哈希值比较，识别所有新增和已修改的文件。
    *   这些文件将被复制到 `update` 目录，并保留其原始文件夹结构。
5.  **与 VS Code 任务集成**：
    *   更新 `tasks.json` 以定义两个不同的发布任务：
        *   **`publish-full`**：执行干净的构建，发布整个应用程序，并生成一个新的 `manifest.json`。
        *   **`publish-update`**：发布应用程序，然后运行 `create-update.csx` 脚本以生成差异更新包。
