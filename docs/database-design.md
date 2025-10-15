# 数据库设计指南

本平台的核心思想是 **数据驱动**，这意味着大部分应用程序的行为都是由数据库中的数据定义的，而不是硬编码在应用程序逻辑中。本指南将详细介绍数据库的设计原则和实现“数据驱动”的方法。

## 设计原则

*   **配置优先于编码**：如果一个功能可以通过数据库配置来完成，那么就严格禁止在 C# 中硬编码。
*   **元数据驱动**：系统中的所有核心元素，如实体、属性、关系、验证规则等，都应该在元数据表中进行定义。
*   **Fat Model, Skinny Controller**：尽可能将业务逻辑保留在模型（或服务）中，保持控制器（Controller）的轻量，只负责请求的调度。
*   **约定优于配置**：在保证灵活性的前提下，尽可能使用通用的约定来减少不必要的配置。

## 核心表结构

为了实现“数据驱动”，我们设计了一系列核心的元数据表，用于定义系统的行为。

### `entities` - 实体表

此表用于定义系统中的所有业务实体。

| 字段名 | 类型 | 描述 |
| --- | --- | --- |
| `id` | `integer` | 主键，唯一标识一个实体。 |
| `name` | `string` | 实体的名称，例如 `user`, `product`。 |
| `display_name` | `string` | 实体的显示名称，用于在 UI 中展示。 |
| `description` | `string` | 实体的描述信息。 |

### `entity_attributes` - 实体属性表

此表用于定义每个实体的属性。

| 字段名 | 类型 | 描述 |
| --- | --- | --- |
| `id` | `integer` | 主键，唯一标识一个属性。 |
| `entity_id` | `integer` | 外键，关联到 `entities` 表，表示该属性属于哪个实体。 |
| `name` | `string` | 属性的名称，例如 `first_name`, `price`。 |
| `data_type` | `string` | 属性的数据类型，例如 `string`, `integer`, `datetime`。 |
| `is_required` | `boolean` | 是否为必填项。 |
| `default_value` | `string` | 默认值。 |

### `entity_relationships` - 实体关系表

此表用于定义实体之间的关系。

| 字段名 | 类型 | 描述 |
| --- | --- | --- |
| `id` | `integer` | 主键，唯一标识一个关系。 |
| `source_entity_id` | `integer` | 外键，源实体 ID。 |
| `target_entity_id` | `integer` | 外键，目标实体 ID。 |
| `relationship_type` | `string` | 关系类型，例如 `one-to-one`, `one-to-many`, `many-to-many`。 |

### `validation_rules` - 验证规则表

此表用于定义数据验证规则。

| 字段名 | 类型 | 描述 |
| --- | --- | --- |
| `id` | `integer` | 主键。 |
| `attribute_id` | `integer` | 外键，关联到 `entity_attributes` 表，表示该规则应用于哪个属性。 |
| `rule_type` | `string` | 规则类型，例如 `regex`, `min-length`, `max-length`。 |
| `rule_value` | `string` | 规则的值，例如正则表达式或长度限制。 |
| `error_message` | `string` | 验证失败时的错误信息。 |

## “数据驱动”的实现

通过上述的元数据表，我们可以在运行时动态地构建应用程序的行为。

1.  **动态生成实体**：`DatabaseService` 在启动时会读取 `entities` 和 `entity_attributes` 表，并使用这些元数据在内存中动态地创建 C# 的实体类。
2.  **动态数据验证**：在数据保存之前，`DatabaseService` 会查询 `validation_rules` 表，并根据定义的规则对数据进行验证。
3.  **动态 API**：`ApiController` 会根据 `entities` 表的定义，自动生成对应实体的 CRUD (Create, Read, Update, Delete) API 接口。

这种数据驱动的方法极大地提高了系统的灵活性和可扩展性。当需要添加新的实体或修改现有实体的行为时，我们只需要修改数据库中的元数据，而无需重新编译和部署应用程序。

---
---

# Database Design Guide

The core idea of this platform is **data-driven**, which means that most of the application's behavior is defined by data in the database, rather than being hard-coded in the application logic. This guide will detail the design principles of the database and the method of implementing "data-driven".

## Design Principles

*   **Configuration over coding**: If a function can be completed through database configuration, then hard coding in C# is strictly prohibited.
*   **Metadata-driven**: All core elements in the system, such as entities, attributes, relationships, validation rules, etc., should be defined in the metadata table.
*   **Fat Model, Skinny Controller**: Keep the business logic in the model (or service) as much as possible, and keep the controller (Controller) lightweight, only responsible for request scheduling.
*   **Convention over configuration**: On the premise of ensuring flexibility, use general conventions as much as possible to reduce unnecessary configuration.

## Core Table Structure

In order to achieve "data-driven", we have designed a series of core metadata tables to define the behavior of the system.

### `entities` - Entity Table

This table is used to define all business entities in the system.

| Field Name | Type | Description |
| --- | --- | --- |
| `id` | `integer` | Primary key, uniquely identifies an entity. |
| `name` | `string` | The name of the entity, for example `user`, `product`. |
| `display_name` | `string` | The display name of the entity, used for display in the UI. |
| `description` | `string` | The description of the entity. |

### `entity_attributes` - Entity Attribute Table

This table is used to define the attributes of each entity.

| Field Name | Type | Description |
| --- | --- | --- |
| `id` | `integer` | Primary key, uniquely identifies an attribute. |
| `entity_id` | `integer` | Foreign key, associated with the `entities` table, indicating which entity the attribute belongs to. |
| `name` | `string` | The name of the attribute, for example `first_name`, `price`. |
| `data_type` | `string` | The data type of the attribute, for example `string`, `integer`, `datetime`. |
| `is_required` | `boolean` | Whether it is a required item. |
| `default_value` | `string` | The default value. |

### `entity_relationships` - Entity Relationship Table

This table is used to define the relationships between entities.

| Field Name | Type | Description |
| --- | --- | --- |
| `id` | `integer` | Primary key, uniquely identifies a relationship. |
| `source_entity_id` | `integer` | Foreign key, source entity ID. |
| `target_entity_id` | `integer` | Foreign key, target entity ID. |
| `relationship_type` | `string` | The relationship type, for example `one-to-one`, `one-to-many`, `many-to-many`. |

### `validation_rules` - Validation Rule Table

This table is used to define data validation rules.

| Field Name | Type | Description |
| --- | --- | --- |
| `id` | `integer` | Primary key. |
| `attribute_id` | `integer` | Foreign key, associated with the `entity_attributes` table, indicating which attribute the rule applies to. |
| `rule_type` | `string` | The rule type, for example `regex`, `min-length`, `max-length`. |
| `rule_value` | `string` | The value of the rule, for example a regular expression or a length limit. |
| `error_message` | `string` | The error message when validation fails. |

## Implementation of "Data-Driven"

Through the above metadata tables, we can dynamically build the behavior of the application at runtime.

1.  **Dynamic Entity Generation**: `DatabaseService` reads the `entities` and `entity_attributes` tables at startup and uses this metadata to dynamically create C# entity classes in memory.
2.  **Dynamic Data Validation**: Before saving data, `DatabaseService` will query the `validation_rules` table and validate the data according to the defined rules.
3.  **Dynamic API**: `ApiController` will automatically generate the corresponding entity's CRUD (Create, Read, Update, Delete) API interface according to the definition of the `entities` table.

This data-driven approach greatly improves the flexibility and scalability of the system. When we need to add a new entity or modify the behavior of an existing entity, we only need to modify the metadata in the database without recompiling and deploying the application.
