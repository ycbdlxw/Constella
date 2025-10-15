# Project Blueprint

## Overview

This document outlines the plan for generating the project skeleton based on the `docs/constitution.md` file.

## Current Task: Initial Project Structure

### Plan

1.  Create the `src` directory as the main container for the source code.
2.  Create the following files with basic template content inside the `src` directory:
    *   `CoreHostService.cs`: The core service responsible for hosting the application.
    *   `CommonService.cs`: A service for common business logic.
    *   `PluginEngine.cs`: The engine for managing plugins.
    *   `GlobalExceptionMiddleware.cs`: Middleware for handling global exceptions.
    *   `LogHelper.cs`: A helper for logging.
    *   `UserContext.cs`: A class for managing user context.
3.  Create the following empty directories inside the `src` directory:
    *   `Repository`: For data access logic.
    *   `Services`: For business logic services.
    *   `Controllers`: For API controllers.
    *   `Plugins`: For hosting application plugins.
