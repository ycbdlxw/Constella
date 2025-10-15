# 📖 API Guide

This document provides a guide to using the API of the Enterprise Application Platform.

## Authentication

All API requests must be authenticated. The API uses token-based authentication. To authenticate, you must include an `Authorization` header in your request with the following format:

```
Authorization: Bearer <your-token>
```

## API Endpoints

The API provides a set of RESTful endpoints for interacting with the system's resources. The base URL for the API is `/api`.

### Generic API

The system provides a generic API for performing CRUD (Create, Read, Update, Delete) operations on any entity defined in the database. The generic API is available at `/api/data/{entityName}`.

**Example: Get all users**

```
GET /api/data/user
```

**Example: Get a single user**

```
GET /api/data/user/{id}
```

**Example: Create a new user**

```
POST /api/data/user
```

**Request Body:**

```json
{
  "username": "testuser",
  "password": "password123",
  "email": "testuser@example.com"
}
```

**Example: Update a user**

```
PUT /api/data/user/{id}
```

**Request Body:**

```json
{
  "email": "newemail@example.com"
}
```

**Example: Delete a user**

```
DELETE /api/data/user/{id}
```

### Custom API

In addition to the generic API, the system also supports custom API endpoints. Custom API endpoints are defined in the `ApiController` and can be used to implement more complex business logic.

## Request and Response Format

All API requests and responses are in JSON format.

### Success Response

```json
{
  "success": true,
  "data": {
    "id": 1,
    "username": "testuser",
    "email": "testuser@example.com"
  }
}
```

### Error Response

```json
{
  "success": false,
  "error": {
    "code": "INVALID_INPUT",
    "message": "The username is already taken."
  }
}
```

---
---

# 📖 API 指南

本指南旨在引导您如何使用企业应用平台的 API。

## 身份认证

所有 API 请求都必须经过身份认证。本 API 采用基于令牌（Token）的认证方式。您必须在请求头（Header）中包含 `Authorization` 字段，格式如下：

```
Authorization: Bearer <your-token>
```

## API 端点

本 API 提供了一套 RESTful 风格的端点，用于与系统资源进行交互。API 的基础 URL 为 `/api`。

### 通用 API

系统提供了一套通用 API，用于对数据库中定义的任何实体执行 CRUD（创建、读取、更新、删除）操作。通用 API 的访问地址为 `/api/data/{entityName}`。

**示例：获取所有用户**

```
GET /api/data/user
```

**示例：获取单个用户**

```
GET /api/data/user/{id}
```

**示例：创建新用户**

```
POST /api/data/user
```

**请求体：**

```json
{
  "username": "testuser",
  "password": "password123",
  "email": "testuser@example.com"
}
```

**示例：更新用户信息**

```
PUT /api/data/user/{id}
```

**请求体：**

```json
{
  "email": "newemail@example.com"
}
```

**示例：删除用户**

```
DELETE /api/data/user/{id}
```

### 自定义 API

除了通用 API，系统也支持自定义 API 端点。自定义 API 端点在 `ApiController` 中定义，可用于实现更复杂的业务逻辑。

## 请求和响应格式

所有 API 的请求和响应均采用 JSON 格式。

### 成功响应

```json
{
  "success": true,
  "data": {
    "id": 1,
    "username": "testuser",
    "email": "testuser@example.com"
  }
}
```

### 失败响应

```json
{
  "success": false,
  "error": {
    "code": "INVALID_INPUT",
    "message": "用户名已被占用。"
  }
}
```
