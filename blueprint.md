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
