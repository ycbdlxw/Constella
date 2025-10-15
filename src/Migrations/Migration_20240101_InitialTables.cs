using FluentMigrator;

namespace myapp.Migrations
{
    [Migration(20240101000000, "Initial tables as per design guide")]
    public class Migration_20240101_InitialTables : Migration
    {
        public override void Up()
        {
            Create.Table("table_attribute")
                .WithColumn("tableName").AsString(100).Nullable()
                .WithColumn("dbtable").AsString(100).NotNullable().PrimaryKey()
                .WithColumn("sort").AsString(200).Nullable()
                .WithColumn("functions").AsString(100).Nullable()
                .WithColumn("groupby").AsString(200).Nullable()
                .WithColumn("isLoading").AsBoolean().WithDefaultValue(false)
                .WithColumn("isAllSelect").AsBoolean().WithDefaultValue(false)
                .WithColumn("isRowOpertionFlag").AsBoolean().WithDefaultValue(false)
                .WithColumn("isOpertionFlag").AsBoolean().WithDefaultValue(false)
                .WithColumn("tableProcedure").AsString(100).Nullable()
                .WithColumn("subTables").AsString(200).Nullable()
                .WithColumn("tableKey").AsString(100).Nullable()
                .WithColumn("ParameterType").AsInt16().Nullable()
                .WithColumn("alias").AsString(100).Nullable()
                .WithColumn("mainKey").AsString(100).Nullable()
                .WithColumn("pageTitle").AsString(100).Nullable()
                .WithColumn("roleFlag").AsBoolean().WithDefaultValue(false)
                .WithColumn("joinStr").AsString(500).Nullable()
                .WithColumn("definColumns").AsString(500).Nullable();

            Create.Table("column_attribute")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("dbTableName").AsString(100).NotNullable()
                .WithColumn("tableName").AsString(100).Nullable()
                .WithColumn("name").AsString(100).NotNullable()
                .WithColumn("pagename").AsString(100).Nullable()
                .WithColumn("IsShowInList").AsBoolean().WithDefaultValue(false)
                .WithColumn("searchFlag").AsBoolean().WithDefaultValue(false)
                .WithColumn("editFlag").AsBoolean().WithDefaultValue(false)
                .WithColumn("options").AsString(50).Nullable()
                .WithColumn("showType").AsString(50).Nullable()
                .WithColumn("queryType").AsString(50).Nullable()
                .WithColumn("checkMode").AsString(50).Nullable()
                .WithColumn("IsRequired").AsBoolean().WithDefaultValue(false)
                .WithColumn("autoSelectId").AsInt32().Nullable()
                .WithColumn("OrderNo").AsInt32().Nullable()
                .WithColumn("searchOrderNo").AsInt32().Nullable()
                .WithColumn("editOrderNo").AsInt32().Nullable()
                .WithColumn("defaultValue").AsString(200).Nullable()
                .WithColumn("len").AsInt32().Nullable()
                .WithColumn("fieldType").AsString(50).Nullable()
                .WithColumn("type").AsInt32().Nullable()
                .WithColumn("classcode").AsString(100).Nullable()
                .WithColumn("Other").AsString(200).Nullable()
                .WithColumn("isRead").AsBoolean().WithDefaultValue(false)
                .WithColumn("unionTable").AsString(100).Nullable()
                .WithColumn("IsPri").AsBoolean().WithDefaultValue(false)
                .WithColumn("IsForeignKey").AsBoolean().WithDefaultValue(false)
                .WithColumn("attrType").AsString(50).Nullable()
                .WithColumn("attrName").AsString(100).Nullable()
                .WithColumn("whereSql").AsString(500).Nullable()
                .WithColumn("templetResId").AsInt32().Nullable()
                .WithColumn("selectColumns").AsString(500).Nullable()
                .WithColumn("isExport").AsBoolean().WithDefaultValue(false)
                .WithColumn("showWidth").AsInt32().Nullable()
                .WithColumn("contentLen").AsInt32().Nullable()
                .WithColumn("editType").AsString(50).Nullable()
                .WithColumn("roles").AsString(200).Nullable()
                .WithColumn("importRequired").AsBoolean().WithDefaultValue(false)
                .WithColumn("searchWidth").AsInt32().Nullable()
                .WithColumn("listWidth").AsInt32().Nullable()
                .WithColumn("isBatch").AsBoolean().WithDefaultValue(false)
                .WithColumn("isMobile").AsBoolean().WithDefaultValue(false);
            Create.Index("idx_dbtable_name").OnTable("column_attribute").OnColumn("dbTableName").Ascending().OnColumn("name").Ascending();

            Create.Table("column_check_property")
                .WithColumn("id").AsInt64().Identity().PrimaryKey()
                .WithColumn("check_table").AsString(50).NotNullable()
                .WithColumn("target_table").AsString(50).NotNullable()
                .WithColumn("check_column").AsString(50).NotNullable()
                .WithColumn("check_mode").AsString(50).NotNullable()
                .WithColumn("check_order").AsInt32().NotNullable().WithDefaultValue(0)
                .WithColumn("status").AsByte().NotNullable().WithDefaultValue(1)
                .WithColumn("errorMsg").AsString(255).NotNullable()
                .WithColumn("whereStr").AsString(255).Nullable()
                .WithColumn("params").AsCustom("TEXT").Nullable(); // For JSON
            Create.Index("idx_check_table").OnTable("column_check_property").OnColumn("check_table");
            Create.Index("idx_check_column").OnTable("column_check_property").OnColumn("check_column");
            Create.Index("idx_check_order").OnTable("column_check_property").OnColumn("check_order");

            Create.Table("plugin_config")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("plugin_name").AsString(100).NotNullable().Unique()
                .WithColumn("class_name").AsString(200).NotNullable()
                .WithColumn("description").AsCustom("TEXT").Nullable()
                .WithColumn("is_active").AsBoolean().WithDefaultValue(true);

            Create.Table("sys_dictionary")
                .WithColumn("id").AsInt64().Identity().PrimaryKey()
                .WithColumn("dict_name").AsString(100).NotNullable()
                .WithColumn("keyword").AsString(255).NotNullable()
                .WithColumn("category").AsString(255).NotNullable()
                .WithColumn("extra").AsCustom("TEXT").Nullable() // For JSON
                .WithColumn("created_at").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("updated_at").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);
            Create.Index("idx_dict_name").OnTable("sys_dictionary").OnColumn("dict_name");
            Create.Index("idx_keyword").OnTable("sys_dictionary").OnColumn("keyword");

            Create.Table("sys_user")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("username").AsString(50).NotNullable().Unique()
                .WithColumn("password").AsString(100).NotNullable()
                .WithColumn("real_name").AsString(50).Nullable()
                .WithColumn("status").AsByte().WithDefaultValue(1)
                .WithColumn("org_id").AsInt32().Nullable()
                .WithColumn("default_remote_id").AsInt32().Nullable()
                .WithColumn("created_at").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("last_login_time").AsInt64().Nullable();

            Create.Table("sys_user_role")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("user_id").AsInt32().NotNullable().ForeignKey("sys_user", "id").OnDelete(System.Data.Rule.Cascade)
                .WithColumn("role_code").AsString(50).NotNullable();
            Create.UniqueConstraint("uk_user_role").OnTable("sys_user_role").Columns("user_id", "role_code");

            Create.Table("sys_organization")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("org_name").AsString(100).NotNullable()
                .WithColumn("parent_id").AsInt32().Nullable().ForeignKey("sys_organization", "id").OnDelete(System.Data.Rule.SetNull)
                .WithColumn("org_code").AsString(50).Nullable()
                .WithColumn("status").AsByte().WithDefaultValue(1)
                .WithColumn("created_at").AsDateTime().WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        public override void Down()
        {
            Delete.Table("sys_organization");
            Delete.Table("sys_user_role");
            Delete.Table("sys_user");
            Delete.Table("sys_dictionary");
            Delete.Table("plugin_config");
            Delete.Table("column_check_property");
            Delete.Table("column_attribute");
            Delete.Table("table_attribute");
        }
    }
}
