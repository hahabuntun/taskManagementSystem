using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Vkr.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class hihihaha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntityName = table.Column<string>(type: "text", nullable: false),
                    EntityKey = table.Column<string>(type: "text", nullable: true),
                    PropertyName = table.Column<string>(type: "text", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                    ChangeType = table.Column<string>(type: "text", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChangedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Checklists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerType = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checklists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sprint_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprint_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "task_filters",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OptionsJson = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_filters", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "task_priority",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_priority", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "task_relationship_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_relationship_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "task_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "task_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "worker_positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_worker_positions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChecklistId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistItems_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RelatedColorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_statuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_project_statuses_colors_RelatedColorId",
                        column: x => x.RelatedColorId,
                        principalTable: "colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TemplateName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TaskName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    TaskStatusId = table.Column<int>(type: "integer", nullable: true),
                    TaskPriorityId = table.Column<int>(type: "integer", nullable: true),
                    TaskTypeId = table.Column<int>(type: "integer", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Progress = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    StoryPoints = table.Column<int>(type: "integer", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskTemplates_task_priority_TaskPriorityId",
                        column: x => x.TaskPriorityId,
                        principalTable: "task_priority",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskTemplates_task_statuses_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "task_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskTemplates_task_types_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "task_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "worker_position_relations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkerPositionId = table.Column<int>(type: "integer", nullable: false),
                    SubordinateWorkerPositionId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_worker_position_relations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_worker_position_relations_worker_positions_SubordinateWorke~",
                        column: x => x.SubordinateWorkerPositionId,
                        principalTable: "worker_positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_worker_position_relations_worker_positions_WorkerPositionId",
                        column: x => x.WorkerPositionId,
                        principalTable: "worker_positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "taskTemplate_links",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    TaskTemplateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_taskTemplate_links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_taskTemplate_links_TaskTemplates_TaskTemplateId",
                        column: x => x.TaskTemplateId,
                        principalTable: "TaskTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTemplateTags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "integer", nullable: false),
                    TaskTemplateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTemplateTags", x => new { x.TagId, x.TaskTemplateId });
                    table.ForeignKey(
                        name: "FK_TaskTemplateTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTemplateTags_TaskTemplates_TaskTemplateId",
                        column: x => x.TaskTemplateId,
                        principalTable: "TaskTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "board_columns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BoardId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_board_columns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "board_tasks",
                columns: table => new
                {
                    BoardId = table.Column<int>(type: "integer", nullable: false),
                    TaskId = table.Column<int>(type: "integer", nullable: false),
                    CustomColumnName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BoardColumnsId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_board_tasks", x => new { x.BoardId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_board_tasks_board_columns_BoardColumnsId",
                        column: x => x.BoardColumnsId,
                        principalTable: "board_columns",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "boards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    OwnerId = table.Column<int>(type: "integer", nullable: true),
                    ProjectId = table.Column<int>(type: "integer", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    BoardTypeId = table.Column<int>(type: "integer", nullable: false),
                    Basis = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_boards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerType = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Key = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "history",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RelatedEntityType = table.Column<string>(type: "text", nullable: false),
                    RelatedEntityId = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RelatedEntityName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RelatedEntityId = table.Column<int>(type: "integer", nullable: false),
                    RelatedEntityType = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "organization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OwnerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "workers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SecondName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ThirdName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PasswordHash = table.Column<string>(type: "character varying(84)", maxLength: 84, nullable: true),
                    CanManageWorkers = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CanManageProjects = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    WorkerPositionId = table.Column<int>(type: "integer", nullable: false),
                    WorkerStatus = table.Column<int>(type: "integer", nullable: false),
                    OrganizationsId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workers_organization_OrganizationsId",
                        column: x => x.OrganizationsId,
                        principalTable: "organization",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_workers_worker_positions_WorkerPositionId",
                        column: x => x.WorkerPositionId,
                        principalTable: "worker_positions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: true),
                    Goal = table.Column<string>(type: "text", nullable: true),
                    ManagerId = table.Column<int>(type: "integer", nullable: false),
                    Progress = table.Column<int>(type: "integer", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrganizationId = table.Column<int>(type: "integer", nullable: false),
                    ProjectStatusId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_project_organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_project_statuses_ProjectStatusId",
                        column: x => x.ProjectStatusId,
                        principalTable: "project_statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_project_workers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkerId = table.Column<int>(type: "integer", nullable: false),
                    EntityId = table.Column<int>(type: "integer", nullable: false),
                    EntityType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_subscriptions_workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "worker_notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "integer", nullable: false),
                    WorkerId = table.Column<int>(type: "integer", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_worker_notifications", x => new { x.NotificationId, x.WorkerId });
                    table.ForeignKey(
                        name: "FK_worker_notifications_notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_worker_notifications_workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workers_management",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ManagerId = table.Column<int>(type: "integer", nullable: false),
                    SubordinateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workers_management", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workers_management_workers_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_workers_management_workers_SubordinateId",
                        column: x => x.SubordinateId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_checklists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    WorkerId = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_checklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_project_checklists_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_checklists_workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "project_links",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_project_links_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_member_managements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WorkerId = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    SubordinateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_member_managements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_project_member_managements_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_member_managements_workers_SubordinateId",
                        column: x => x.SubordinateId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_member_managements_workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTag",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTag", x => new { x.TagId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_ProjectTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTag_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sprints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Goal = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    StartsOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpireOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    SprintStatusId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sprints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sprints_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sprints_sprint_statuses_SprintStatusId",
                        column: x => x.SprintStatusId,
                        principalTable: "sprint_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerProgect",
                columns: table => new
                {
                    ProjectsListId = table.Column<int>(type: "integer", nullable: false),
                    WorkersListId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerProgect", x => new { x.ProjectsListId, x.WorkersListId });
                    table.ForeignKey(
                        name: "FK_WorkerProgect_project_ProjectsListId",
                        column: x => x.ProjectsListId,
                        principalTable: "project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerProgect_workers_WorkersListId",
                        column: x => x.WorkersListId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "project_checklist_checks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tittle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsChecked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ProjectChecklistId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_checklist_checks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_project_checklist_checks_project_checklists_ProjectChecklis~",
                        column: x => x.ProjectChecklistId,
                        principalTable: "project_checklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShortName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StoryPoints = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Progress = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    StartOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExpireOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ParentTaskId = table.Column<int>(type: "integer", nullable: true),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    TaskTypeId = table.Column<int>(type: "integer", nullable: false),
                    TaskStatusId = table.Column<int>(type: "integer", nullable: false),
                    ProjectId = table.Column<int>(type: "integer", nullable: false),
                    SprintId = table.Column<int>(type: "integer", nullable: true),
                    TaskPriorityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tasks_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tasks_sprints_SprintId",
                        column: x => x.SprintId,
                        principalTable: "sprints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_tasks_task_priority_TaskPriorityId",
                        column: x => x.TaskPriorityId,
                        principalTable: "task_priority",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tasks_task_statuses_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "task_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tasks_task_types_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "task_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tasks_tasks_ParentTaskId",
                        column: x => x.ParentTaskId,
                        principalTable: "tasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tasks_workers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_executors",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "integer", nullable: false),
                    WorkerId = table.Column<int>(type: "integer", nullable: false),
                    IsResponsible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_executors", x => new { x.TaskId, x.WorkerId });
                    table.ForeignKey(
                        name: "FK_task_executors_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_executors_workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_links",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Link = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    TaskId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_links_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageText = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    SenderId = table.Column<int>(type: "integer", nullable: false),
                    RelatedTaskId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_messages_tasks_RelatedTaskId",
                        column: x => x.RelatedTaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_messages_workers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "task_observers",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "integer", nullable: false),
                    WorkerId = table.Column<int>(type: "integer", nullable: false),
                    AssignedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_observers", x => new { x.TaskId, x.WorkerId });
                    table.ForeignKey(
                        name: "FK_task_observers_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_observers_workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "task_relationships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TaskId = table.Column<int>(type: "integer", nullable: false),
                    RelatedTaskId = table.Column<int>(type: "integer", nullable: false),
                    TaskRelationshipTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_relationships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_task_relationships_task_relationship_types_TaskRelationship~",
                        column: x => x.TaskRelationshipTypeId,
                        principalTable: "task_relationship_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_task_relationships_tasks_RelatedTaskId",
                        column: x => x.RelatedTaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_task_relationships_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskTag",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "integer", nullable: false),
                    TaskId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTag", x => new { x.TagId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_TaskTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskTag_tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Color", "Name" },
                values: new object[,]
                {
                    { 1, "#E5E7EB", "CRM" },
                    { 2, "#A7F3D0", "WebApp" },
                    { 3, "#BFDBFE", "MobileApp" },
                    { 4, "#FDE68A", "Delivery" },
                    { 5, "#DDD6FE", "Automation" },
                    { 6, "#D1D5DB", "Warehouse" },
                    { 7, "#FBCFE8", "Portal" },
                    { 8, "#99F6E4", "Marketing" },
                    { 9, "#F5D0FE", "Campaign" },
                    { 10, "#E2E8F0", "Analysis" },
                    { 11, "#BAE6FD", "Sprint1" },
                    { 12, "#F9A8D4", "Milestone" },
                    { 13, "#C7D2FE", "Design" },
                    { 14, "#BBF7D0", "Development" },
                    { 15, "#A5F3FC", "Sprint2" },
                    { 16, "#FED7AA", "Testing" },
                    { 17, "#D4D4D8", "Deployment" },
                    { 18, "#CCFBF1", "Documentation" },
                    { 19, "#CFFAFE", "Requirements" },
                    { 20, "#FECACA", "UI/UX" },
                    { 21, "#FEF3C7", "API" },
                    { 22, "#EDE9FE", "QA" },
                    { 23, "#FECDD3", "Release" }
                });

            migrationBuilder.InsertData(
                table: "colors",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "#FF5733", "Vibrant orange" },
                    { 2, "#32CD32", "Green" },
                    { 3, "#3357FF", "Strong blue" },
                    { 4, "#DC143C", "Pink-red" },
                    { 5, "#00BFFF", "Teal-blue" },
                    { 7, "#FFA500", "Dark orange" },
                    { 8, "#8A2BE2", "Purple" },
                    { 9, "#228B22", "Dark green" },
                    { 10, "#8B008B", "Dark purple-red" },
                    { 11, "#FFD700", "Gold" },
                    { 12, "#A0522D", "Brown" },
                    { 13, "#ADD8E6", "Light blue" },
                    { 14, "#808080", "Gray" },
                    { 15, "#C0C0C0", "Light gray" },
                    { 16, "#90EE90", "Medium Green" },
                    { 17, "#FF69B4", "Pink-red" },
                    { 18, "#FFA500", "Orange-red" },
                    { 19, "#000080", "Dark blue" },
                    { 20, "#008080", "Teal-Green" }
                });

            migrationBuilder.InsertData(
                table: "sprint_statuses",
                columns: new[] { "Id", "Color", "Name" },
                values: new object[,]
                {
                    { 1, "#ffd666", "PLANNED" },
                    { 2, "#95de64", "ACTIVE" },
                    { 3, "#69b1ff", "FINISHED" }
                });

            migrationBuilder.InsertData(
                table: "task_priority",
                columns: new[] { "Id", "Color", "Name" },
                values: new object[,]
                {
                    { 1, "blue", "Низкий" },
                    { 2, "blue", "Обычный" },
                    { 3, "blue", "Высокий" },
                    { 4, "blue", "Критичный" }
                });

            migrationBuilder.InsertData(
                table: "task_relationship_types",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Финиш-Старт" },
                    { 2, "Финиш-Финиш" },
                    { 3, "Старт-Старт" },
                    { 4, "Старт-Финиш" },
                    { 5, "Подзадача" }
                });

            migrationBuilder.InsertData(
                table: "task_statuses",
                columns: new[] { "Id", "Color", "Name" },
                values: new object[,]
                {
                    { 1, "blue", "В ожидании" },
                    { 2, "blue", "В работе" },
                    { 3, "blue", "На проверке" },
                    { 4, "blue", "Завершена" },
                    { 5, "blue", "Приостановлен" }
                });

            migrationBuilder.InsertData(
                table: "task_types",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Задача" },
                    { 2, "Веха" },
                    { 3, "Сводная задача" }
                });

            migrationBuilder.InsertData(
                table: "worker_positions",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Junior Developer" },
                    { 2, "Mid-Level Developer" },
                    { 3, "Senior Developer" },
                    { 4, "Team Lead" },
                    { 5, "Project Manager" },
                    { 6, "Product Owner" },
                    { 7, "Scrum Master" },
                    { 8, "QA Engineer" },
                    { 9, "DevOps Engineer" },
                    { 10, "System Architect" },
                    { 11, "UI/UX Designer" },
                    { 12, "Business Analyst" },
                    { 13, "Data Scientist" },
                    { 14, "Database Administrator" },
                    { 15, "Security Specialist" },
                    { 16, "Technical Writer" },
                    { 17, "Support Engineer" },
                    { 18, "Marketing Specialist" },
                    { 19, "HR Manager" },
                    { 20, "Finance Analyst" }
                });

            migrationBuilder.InsertData(
                table: "TaskTemplates",
                columns: new[] { "Id", "CreatedOn", "Description", "EndDate", "StartDate", "StoryPoints", "TaskName", "TaskPriorityId", "TaskStatusId", "TaskTypeId", "TemplateName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Сбор и анализ требований для проекта.", null, null, 8, "Анализ требований", 2, 1, 3, "Requirement Analysis Template" },
                    { 2, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Создание UI/UX дизайна для приложения.", null, null, 13, "Дизайн интерфейса", 2, 1, 3, "UI Design Template" },
                    { 3, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Создание REST API для приложения.", null, null, 5, "Разработка API", 2, 1, 1, "API Development Template" },
                    { 4, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Проведение тестирования приложения (unit, интеграционное, нагрузочное).", null, null, 8, "Тестирование", 2, 1, 3, "Testing Template" },
                    { 5, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Подготовка и выпуск приложения в продакшен.", null, null, 3, "Развертывание", 2, 1, 3, "Deployment Template" }
                });

            migrationBuilder.InsertData(
                table: "project_statuses",
                columns: new[] { "Id", "Name", "RelatedColorId" },
                values: new object[,]
                {
                    { 1, "Инициализируется", 15 },
                    { 2, "В работе", 20 },
                    { 3, "На проверке", 11 },
                    { 4, "Завершен", 14 },
                    { 5, "В архиве", 14 }
                });

            migrationBuilder.InsertData(
                table: "workers",
                columns: new[] { "Id", "CanManageProjects", "CanManageWorkers", "CreatedOn", "Email", "Name", "NormalizedEmail", "OrganizationsId", "PasswordHash", "Phone", "SecondName", "ThirdName", "WorkerPositionId", "WorkerStatus" },
                values: new object[] { 1, true, true, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "ivan.petrov@example.com", "Иван", "IVAN.PETROV@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Петров", "Александрович", 1, 0 });

            migrationBuilder.InsertData(
                table: "workers",
                columns: new[] { "Id", "CreatedOn", "Email", "Name", "NormalizedEmail", "OrganizationsId", "PasswordHash", "Phone", "SecondName", "ThirdName", "WorkerPositionId", "WorkerStatus" },
                values: new object[] { 2, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "anna.smirnova@example.com", "Анна", "ANNA.SMIRNOVA@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Смирнова", "Сергеевна", 2, 0 });

            migrationBuilder.InsertData(
                table: "workers",
                columns: new[] { "Id", "CanManageProjects", "CanManageWorkers", "CreatedOn", "Email", "Name", "NormalizedEmail", "OrganizationsId", "PasswordHash", "Phone", "SecondName", "ThirdName", "WorkerPositionId", "WorkerStatus" },
                values: new object[,]
                {
                    { 3, true, true, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "mikhail.ivanov@example.com", "Михаил", "MIKHAIL.IVANOV@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Иванов", "Владимирович", 3, 0 },
                    { 4, true, true, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "ekaterina.kozlova@example.com", "Екатерина", "EKATERINA.KOZLOVA@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Козлова", "Дмитриевна", 4, 0 },
                    { 5, true, true, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "alexandr.sokolov@example.com", "Александр", "ALEXANDR.SOKOLOV@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Соколов", "Игоревич", 5, 0 }
                });

            migrationBuilder.InsertData(
                table: "workers",
                columns: new[] { "Id", "CanManageProjects", "CreatedOn", "Email", "Name", "NormalizedEmail", "OrganizationsId", "PasswordHash", "Phone", "SecondName", "ThirdName", "WorkerPositionId", "WorkerStatus" },
                values: new object[] { 6, true, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "olga.novikova@example.com", "Ольга", "OLGA.NOVIKOVA@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Новикова", "Павловна", 6, 0 });

            migrationBuilder.InsertData(
                table: "workers",
                columns: new[] { "Id", "CanManageProjects", "CanManageWorkers", "CreatedOn", "Email", "Name", "NormalizedEmail", "OrganizationsId", "PasswordHash", "Phone", "SecondName", "ThirdName", "WorkerPositionId", "WorkerStatus" },
                values: new object[,]
                {
                    { 7, true, true, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "dmitry.morozov@example.com", "Дмитрий", "DMITRY.MOROZOV@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Морозов", "Алексеевич", 7, 0 },
                    { 8, true, true, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "elena.popova@example.com", "Елена", "ELENA.POPOVA@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Попова", "Николаевна", 8, 0 }
                });

            migrationBuilder.InsertData(
                table: "workers",
                columns: new[] { "Id", "CreatedOn", "Email", "Name", "NormalizedEmail", "OrganizationsId", "PasswordHash", "Phone", "SecondName", "ThirdName", "WorkerPositionId", "WorkerStatus" },
                values: new object[] { 9, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "sergey.fedorov@example.com", "Сергей", "SERGEY.FEDOROV@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Федоров", "Михайлович", 9, 0 });

            migrationBuilder.InsertData(
                table: "workers",
                columns: new[] { "Id", "CanManageProjects", "CanManageWorkers", "CreatedOn", "Email", "Name", "NormalizedEmail", "OrganizationsId", "PasswordHash", "Phone", "SecondName", "ThirdName", "WorkerPositionId", "WorkerStatus" },
                values: new object[] { 10, true, true, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "natalya.kuznetsova@example.com", "Наталья", "NATALYA.KUZNETSOVA@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Кузнецова", "Викторовна", 10, 0 });

            migrationBuilder.InsertData(
                table: "workers",
                columns: new[] { "Id", "CreatedOn", "Email", "Name", "NormalizedEmail", "OrganizationsId", "PasswordHash", "Phone", "SecondName", "ThirdName", "WorkerPositionId", "WorkerStatus" },
                values: new object[,]
                {
                    { 11, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "andrey.vorobiev@example.com", "Андрей", "ANDREY.VOROBIEV@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Воробьев", "Петрович", 11, 0 },
                    { 12, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "yuliya.grigorieva@example.com", "Юлия", "YULIYA.GRIGORIEVA@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Григорьева", "Андреевна", 12, 0 },
                    { 13, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "pavel.sidorov@example.com", "Павел", "PAVEL.SIDOROV@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Сидоров", "Евгеньевич", 13, 0 },
                    { 14, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "tatiana.lebedeva@example.com", "Татьяна", "TATIANA.LEBEDEVA@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Лебедева", "Игоревна", 14, 0 },
                    { 15, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "viktor.belov@example.com", "Виктор", "VIKTOR.BELOV@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Белов", "Анатольевич", 15, 0 },
                    { 16, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "maria.orlova@example.com", "Мария", "MARIA.ORLOVA@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Орлова", "Владимировна", 16, 0 },
                    { 17, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "roman.zaytsev@example.com", "Роман", "ROMAN.ZAYTSEV@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Зайцев", "Сергеевич", 17, 0 },
                    { 18, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "svetlana.egorova@example.com", "Светлана", "SVETLANA.EGOROVA@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Егорова", "Михайловна", 18, 0 }
                });

            migrationBuilder.InsertData(
                table: "workers",
                columns: new[] { "Id", "CanManageWorkers", "CreatedOn", "Email", "Name", "NormalizedEmail", "OrganizationsId", "PasswordHash", "Phone", "SecondName", "ThirdName", "WorkerPositionId", "WorkerStatus" },
                values: new object[] { 19, true, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "vladimir.krylov@example.com", "Владимир", "VLADIMIR.KRYLOV@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Крылов", "Дмитриевич", 19, 0 });

            migrationBuilder.InsertData(
                table: "workers",
                columns: new[] { "Id", "CreatedOn", "Email", "Name", "NormalizedEmail", "OrganizationsId", "PasswordHash", "Phone", "SecondName", "ThirdName", "WorkerPositionId", "WorkerStatus" },
                values: new object[] { 20, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "kseniya.vasilyeva@example.com", "Ксения", "KSENIYA.VASILYEVA@EXAMPLE.COM", null, "$2a$11$H9AgqJyZjbH6RAT46DTNQOyawjOXMVZoLyvI3bPxnnDwU/57ynpAi", null, "Васильева", "Егоровна", 20, 0 });

            migrationBuilder.InsertData(
                table: "TaskTemplateTags",
                columns: new[] { "TagId", "TaskTemplateId" },
                values: new object[,]
                {
                    { 10, 1 },
                    { 13, 2 },
                    { 14, 3 },
                    { 16, 4 },
                    { 17, 5 },
                    { 19, 1 },
                    { 20, 2 },
                    { 21, 3 },
                    { 22, 4 },
                    { 23, 5 }
                });

            migrationBuilder.InsertData(
                table: "organization",
                columns: new[] { "Id", "CreatedOn", "Name", "OwnerId" },
                values: new object[] { 1, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Инновации", 1 });

            migrationBuilder.InsertData(
                table: "taskTemplate_links",
                columns: new[] { "Id", "CreatedOn", "Description", "Link", "TaskTemplateId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Ссылка на документ с требованиями для анализа.", "https://docs.example.com/requirements", 1 },
                    { 2, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Инструмент для совместной работы над требованиями.", "https://collaboration.example.com/requirements-tool", 1 }
                });

            migrationBuilder.InsertData(
                table: "workers_management",
                columns: new[] { "Id", "ManagerId", "SubordinateId" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 2, 1, 9 },
                    { 3, 1, 11 },
                    { 4, 1, 12 },
                    { 5, 1, 13 },
                    { 6, 1, 14 },
                    { 7, 1, 15 },
                    { 8, 1, 16 },
                    { 9, 1, 17 },
                    { 10, 1, 18 },
                    { 11, 1, 20 },
                    { 12, 3, 2 },
                    { 13, 3, 9 },
                    { 14, 3, 11 },
                    { 15, 3, 12 },
                    { 16, 3, 13 },
                    { 17, 3, 14 },
                    { 18, 3, 15 },
                    { 19, 3, 16 },
                    { 20, 3, 17 },
                    { 21, 3, 18 },
                    { 22, 3, 20 },
                    { 23, 4, 2 },
                    { 24, 4, 9 },
                    { 25, 4, 11 },
                    { 26, 4, 12 },
                    { 27, 4, 13 },
                    { 28, 4, 14 },
                    { 29, 4, 15 },
                    { 30, 4, 16 },
                    { 31, 4, 17 },
                    { 32, 4, 18 },
                    { 33, 4, 20 },
                    { 34, 5, 2 },
                    { 35, 5, 9 },
                    { 36, 5, 11 },
                    { 37, 5, 12 },
                    { 38, 5, 13 },
                    { 39, 5, 14 },
                    { 40, 5, 15 },
                    { 41, 5, 16 },
                    { 42, 5, 17 },
                    { 43, 5, 18 },
                    { 44, 5, 20 },
                    { 45, 7, 2 },
                    { 46, 7, 9 },
                    { 47, 7, 11 },
                    { 48, 7, 12 },
                    { 49, 7, 13 },
                    { 50, 7, 14 },
                    { 51, 7, 15 },
                    { 52, 7, 16 },
                    { 53, 7, 17 },
                    { 54, 7, 18 },
                    { 55, 7, 20 },
                    { 56, 8, 2 },
                    { 57, 8, 9 },
                    { 58, 8, 11 },
                    { 59, 8, 12 },
                    { 60, 8, 13 },
                    { 61, 8, 14 },
                    { 62, 8, 15 },
                    { 63, 8, 16 },
                    { 64, 8, 17 },
                    { 65, 8, 18 },
                    { 66, 8, 20 },
                    { 67, 10, 2 },
                    { 68, 10, 9 },
                    { 69, 10, 11 },
                    { 70, 10, 12 },
                    { 71, 10, 13 },
                    { 72, 10, 14 },
                    { 73, 10, 15 },
                    { 74, 10, 16 },
                    { 75, 10, 17 },
                    { 76, 10, 18 },
                    { 77, 10, 20 },
                    { 78, 19, 2 },
                    { 79, 19, 9 },
                    { 80, 19, 11 },
                    { 81, 19, 12 },
                    { 82, 19, 13 },
                    { 83, 19, 14 },
                    { 84, 19, 15 },
                    { 85, 19, 16 },
                    { 86, 19, 17 },
                    { 87, 19, 18 },
                    { 88, 19, 20 }
                });

            migrationBuilder.InsertData(
                table: "project",
                columns: new[] { "Id", "CreatedOn", "Description", "EndDate", "Goal", "ManagerId", "Name", "OrganizationId", "Progress", "ProjectStatusId", "StartDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Создание CRM-системы для управления клиентами и продажами.", null, null, 1, "Разработка CRM-системы", 1, 10, 1, null },
                    { 2, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Разработка мобильного приложения для службы доставки еды.", null, null, 3, "Мобильное приложение для доставки", 1, 20, 1, null },
                    { 3, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Внедрение системы автоматизации учета на складе.", null, null, 4, "Автоматизация складского учета", 1, 30, 2, null },
                    { 4, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Создание внутреннего портала для сотрудников компании.", null, null, 5, "Корпоративный портал", 1, 15, 1, null },
                    { 5, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Планирование и запуск маркетинговой кампании для нового продукта.", null, null, 7, "Маркетинговая кампания", 1, 5, 3, null }
                });

            migrationBuilder.InsertData(
                table: "ProjectTag",
                columns: new[] { "ProjectId", "TagId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 4, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 5 },
                    { 3, 6 },
                    { 4, 7 },
                    { 5, 8 },
                    { 5, 9 }
                });

            migrationBuilder.InsertData(
                table: "WorkerProgect",
                columns: new[] { "ProjectsListId", "WorkersListId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 1, 9 },
                    { 1, 11 },
                    { 1, 12 },
                    { 2, 3 },
                    { 2, 13 },
                    { 2, 14 },
                    { 2, 15 },
                    { 3, 4 },
                    { 3, 16 },
                    { 3, 17 },
                    { 3, 18 },
                    { 3, 19 },
                    { 4, 5 },
                    { 4, 6 },
                    { 4, 8 },
                    { 4, 20 },
                    { 5, 7 },
                    { 5, 10 },
                    { 5, 11 },
                    { 5, 12 }
                });

            migrationBuilder.InsertData(
                table: "project_links",
                columns: new[] { "Id", "CreatedOn", "Description", "Link", "ProjectId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Jira for tracking CRM project tasks and sprints.", "https://www.atlassian.com/software/jira", 1 },
                    { 2, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Confluence for CRM project documentation and collaboration.", "https://www.confluence.com/", 1 },
                    { 3, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "GitHub repository for CRM project source code.", "https://github.com/example/crm-project", 1 },
                    { 4, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Figma for designing CRM UI/UX prototypes.", "https://www.figma.com/", 1 },
                    { 5, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Swagger for documenting CRM API endpoints.", "https://swagger.io/", 1 },
                    { 6, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Entity Framework Core docs for CRM backend development.", "https://docs.microsoft.com/en-us/ef/core/", 1 },
                    { 7, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "React documentation for CRM frontend development.", "https://reactjs.org/", 1 },
                    { 8, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Postman for testing CRM API endpoints.", "https://www.postman.com/", 1 },
                    { 9, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Salesforce guide on CRM systems for reference.", "https://www.salesforce.com/products/what-is-crm/", 1 },
                    { 10, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Trello for managing CRM project tasks and workflows.", "https://www.trello.com/", 1 },
                    { 11, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Notion for CRM project notes and planning.", "https://www.notion.so/", 1 },
                    { 12, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Zoho CRM for inspiration and feature benchmarking.", "https://www.zoho.com/crm/", 1 },
                    { 13, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "GitLab for CI/CD pipeline setup for CRM project.", "https://www.gitlab.com/", 1 },
                    { 14, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Cypress for end-to-end testing of CRM frontend.", "https://www.cypress.io/", 1 },
                    { 15, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Jest for unit testing CRM frontend components.", "https://jestjs.io/", 1 },
                    { 16, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Auth0 for CRM authentication and user management.", "https://www.auth0.com/", 1 },
                    { 17, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Docker for containerizing CRM application services.", "https://www.docker.com/", 1 },
                    { 18, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "MongoDB Atlas for potential NoSQL integration in CRM.", "https://www.mongodb.com/docs/atlas/", 1 },
                    { 19, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Slack for CRM project team communication.", "https://www.slack.com/", 1 },
                    { 20, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Pluralsight course on building CRM systems.", "https://www.pluralsight.com/courses/building-crm", 1 }
                });

            migrationBuilder.InsertData(
                table: "project_member_managements",
                columns: new[] { "Id", "ProjectId", "SubordinateId", "WorkerId" },
                values: new object[,]
                {
                    { 1, 1, 2, 1 },
                    { 2, 1, 9, 1 },
                    { 3, 2, 13, 3 },
                    { 4, 3, 16, 4 },
                    { 5, 3, 17, 4 },
                    { 6, 4, 6, 5 },
                    { 7, 5, 10, 7 }
                });

            migrationBuilder.InsertData(
                table: "sprints",
                columns: new[] { "Id", "CreatedOn", "ExpireOn", "Goal", "ProjectId", "SprintStatusId", "StartsOn", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 3, 23, 59, 59, 0, DateTimeKind.Utc), "Initial setup and core features", 1, 1, new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Sprint 1 - CRM" },
                    { 2, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), new DateTime(2025, 6, 18, 23, 59, 59, 0, DateTimeKind.Utc), "User authentication and dashboard", 1, 1, new DateTime(2025, 6, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Sprint 2 - CRM" }
                });

            migrationBuilder.InsertData(
                table: "tasks",
                columns: new[] { "Id", "CreatedOn", "CreatorId", "Description", "ExpireOn", "ParentTaskId", "ProjectId", "ShortName", "SprintId", "StartOn", "StoryPoints", "TaskPriorityId", "TaskStatusId", "TaskTypeId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Сбор и анализ требований для CRM", new DateTime(2025, 5, 15, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Анализ требований", 1, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 3 },
                    { 2, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Интервью с заказчиком", new DateTime(2025, 5, 5, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Сбор требований", 1, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 3, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Изучение аналогичных CRM систем", new DateTime(2025, 5, 7, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Анализ конкурентов", 1, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, 1 },
                    { 4, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Написание технического задания", new DateTime(2025, 5, 12, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Составление ТЗ", 1, new DateTime(2025, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 5, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Подтверждение ТЗ заказчиком", new DateTime(2025, 5, 13, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Утверждение ТЗ", 1, new DateTime(2025, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, 2 },
                    { 6, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "Создание UI/UX дизайна CRM", new DateTime(2025, 5, 31, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Дизайн приложения", 1, new DateTime(2025, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 3 },
                    { 7, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "Создание прототипов интерфейса", new DateTime(2025, 5, 20, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Прототипирование UI", 1, new DateTime(2025, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 8, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "Разработка логотипа CRM", new DateTime(2025, 5, 18, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Дизайн логотипа", 1, new DateTime(2025, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, 1 },
                    { 9, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "Окончательный дизайн интерфейса", new DateTime(2025, 5, 31, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Финальный дизайн UI", 1, new DateTime(2025, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 10, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "Подтверждение дизайна заказчиком", new DateTime(2025, 5, 31, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Утверждение дизайна", 1, new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, 2 },
                    { 11, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Complete Sprint 1 deliverables", new DateTime(2025, 5, 31, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Sprint 1 Completion", 1, new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, 2 },
                    { 12, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Кодинг основного функционала CRM", new DateTime(2025, 7, 15, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Разработка приложения", 2, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 3 },
                    { 13, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Подготовка dev-окружения", new DateTime(2025, 6, 5, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Настройка окружения", 2, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, 1 },
                    { 14, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Создание REST API для CRM", new DateTime(2025, 6, 20, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Разработка API", 2, new DateTime(2025, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 15, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, "Подключение UI к API", new DateTime(2025, 7, 5, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Интеграция фронтенда", 2, new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 16, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Настройка логина и регистрации", new DateTime(2025, 6, 25, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Реализация авторизации", 2, new DateTime(2025, 6, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 17, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Готовность базового функционала", new DateTime(2025, 7, 5, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Завершение разработки", 2, new DateTime(2025, 7, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, 2 },
                    { 18, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Проверка качества CRM", new DateTime(2025, 7, 20, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Тестирование приложения", 2, new DateTime(2025, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 3 },
                    { 19, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Написание модульных тестов", new DateTime(2025, 7, 12, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Unit-тесты", 2, new DateTime(2025, 7, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, 1 },
                    { 20, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Тестирование интеграции модулей", new DateTime(2025, 7, 18, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Интеграционное тестирование", 2, new DateTime(2025, 7, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 21, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Проверка производительности", new DateTime(2025, 7, 23, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Нагрузочное тестирование", 2, new DateTime(2025, 7, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 22, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Выпуск бета-версии CRM", new DateTime(2025, 7, 24, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Релиз бета-версии", 2, new DateTime(2025, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, 2 },
                    { 23, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Подготовка и выпуск финальной версии", new DateTime(2025, 7, 31, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Запуск приложения", 2, new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 3 },
                    { 24, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 12, "Создание пользовательской документации", new DateTime(2025, 7, 29, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Подготовка документации", 2, new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, 1, 1 },
                    { 25, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, "Запуск рекламы CRM", new DateTime(2025, 7, 30, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Маркетинговая кампания", 2, new DateTime(2025, 7, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 26, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, "Размещение CRM в продакшен", new DateTime(2025, 7, 31, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Публикация CRM", 2, new DateTime(2025, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 2, 1, 1 },
                    { 27, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Выпуск финальной версии CRM", new DateTime(2025, 7, 31, 23, 59, 59, 0, DateTimeKind.Utc), null, 1, "Официальный релиз", 2, new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 3, 1, 2 }
                });

            migrationBuilder.InsertData(
                table: "TaskTag",
                columns: new[] { "TagId", "TaskId" },
                values: new object[,]
                {
                    { 8, 25 },
                    { 10, 1 },
                    { 10, 2 },
                    { 10, 3 },
                    { 10, 4 },
                    { 11, 1 },
                    { 11, 2 },
                    { 11, 3 },
                    { 11, 4 },
                    { 11, 5 },
                    { 11, 6 },
                    { 11, 7 },
                    { 11, 8 },
                    { 11, 9 },
                    { 11, 10 },
                    { 11, 11 },
                    { 12, 5 },
                    { 12, 10 },
                    { 12, 11 },
                    { 12, 17 },
                    { 12, 22 },
                    { 12, 27 },
                    { 13, 6 },
                    { 13, 7 },
                    { 13, 8 },
                    { 13, 9 },
                    { 14, 12 },
                    { 14, 13 },
                    { 14, 14 },
                    { 14, 15 },
                    { 14, 16 },
                    { 15, 12 },
                    { 15, 13 },
                    { 15, 14 },
                    { 15, 15 },
                    { 15, 16 },
                    { 15, 17 },
                    { 15, 18 },
                    { 15, 19 },
                    { 15, 20 },
                    { 15, 21 },
                    { 15, 22 },
                    { 15, 23 },
                    { 15, 24 },
                    { 15, 25 },
                    { 15, 26 },
                    { 15, 27 },
                    { 16, 18 },
                    { 16, 19 },
                    { 16, 20 },
                    { 16, 21 },
                    { 17, 23 },
                    { 17, 26 },
                    { 18, 24 }
                });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId", "IsResponsible" },
                values: new object[,]
                {
                    { 1, 1, true },
                    { 2, 1, true }
                });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId" },
                values: new object[] { 2, 2 });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId", "IsResponsible" },
                values: new object[,]
                {
                    { 3, 2, true },
                    { 4, 1, true }
                });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId" },
                values: new object[] { 4, 2 });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId", "IsResponsible" },
                values: new object[,]
                {
                    { 5, 1, true },
                    { 6, 9, true },
                    { 7, 9, true },
                    { 8, 9, true },
                    { 9, 9, true },
                    { 10, 9, true },
                    { 11, 1, true },
                    { 12, 1, true },
                    { 13, 11, true },
                    { 14, 11, true }
                });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId" },
                values: new object[] { 15, 11 });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId", "IsResponsible" },
                values: new object[,]
                {
                    { 15, 12, true },
                    { 16, 11, true },
                    { 17, 1, true },
                    { 18, 2, true },
                    { 19, 2, true }
                });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId" },
                values: new object[] { 19, 12 });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId", "IsResponsible" },
                values: new object[] { 20, 2, true });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId" },
                values: new object[] { 20, 12 });

            migrationBuilder.InsertData(
                table: "task_executors",
                columns: new[] { "TaskId", "WorkerId", "IsResponsible" },
                values: new object[,]
                {
                    { 21, 2, true },
                    { 22, 1, true },
                    { 23, 1, true },
                    { 24, 12, true },
                    { 25, 12, true },
                    { 26, 11, true },
                    { 27, 1, true }
                });

            migrationBuilder.InsertData(
                table: "task_links",
                columns: new[] { "Id", "CreatedOn", "Description", "Link", "TaskId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "PostgreSQL documentation on DDL for designing database schemas.", "https://www.postgresql.org/docs/current/ddl.html", 1 },
                    { 2, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Online tool for creating ER diagrams for CRM database schema.", "https://dbdesigner.net/", 1 },
                    { 3, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Lucidchart guide for creating entity-relationship diagrams.", "https://www.lucidchart.com/pages/er-diagrams", 1 },
                    { 4, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Article on database normalization techniques for schema design.", "https://www.sqlshack.com/learn-sql-database-normalization/", 1 },
                    { 5, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "GitHub repository with sample CRM database schema scripts.", "https://github.com/example/crm-schema", 1 },
                    { 6, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Vertabelo tool for designing and visualizing database schemas.", "https://www.vertabelo.com/", 1 },
                    { 7, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "MySQL Workbench for designing and managing database schemas.", "https://www.mysql.com/products/workbench/", 1 },
                    { 8, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Tutorial on SQL indexes for optimizing database performance.", "https://www.tutorialspoint.com/sql/sql-indexes.htm", 1 },
                    { 9, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Guide on SQL joins for designing relational schemas.", "https://www.dofactory.com/sql/joins", 1 },
                    { 10, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "DBML language for defining database schemas in code.", "https://www.dbml.org/", 1 },
                    { 11, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Blog on best practices for database schema design.", "https://www.learnsql.com/blog/database-design/", 1 },
                    { 12, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "pgAdmin tool for managing PostgreSQL database schemas.", "https://www.pgadmin.org/", 1 },
                    { 13, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "SQL style guide for consistent schema naming conventions.", "https://www.sqlstyle.guide/", 1 },
                    { 14, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "DataCamp course on database design fundamentals.", "https://www.datacamp.com/courses/database-design", 1 },
                    { 15, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Draw.io for creating ER diagrams for CRM schema.", "https://www.draw.io/", 1 },
                    { 16, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Article on database design principles for relational databases.", "https://www.sqlservercentral.com/articles/database-design", 1 },
                    { 17, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Redgate SQL Toolbelt for schema management and comparison.", "https://www.red-gate.com/products/sql-toolbelt/", 1 },
                    { 18, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "SQLite tutorial on constraints for schema integrity.", "https://www.sqlitetutorial.net/sqlite-constraints/", 1 },
                    { 19, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "MongoDB data modeling guide for hybrid schema design.", "https://www.mongodb.com/docs/manual/core/data-modeling/", 1 },
                    { 20, new DateTime(2025, 5, 19, 12, 27, 0, 0, DateTimeKind.Utc), "Pluralsight course on designing efficient database schemas.", "https://www.pluralsight.com/courses/database-design", 1 }
                });

            migrationBuilder.InsertData(
                table: "task_relationships",
                columns: new[] { "Id", "RelatedTaskId", "TaskId", "TaskRelationshipTypeId" },
                values: new object[,]
                {
                    { 1, 2, 1, 5 },
                    { 2, 3, 1, 5 },
                    { 3, 4, 1, 5 },
                    { 4, 5, 1, 5 },
                    { 5, 7, 6, 5 },
                    { 6, 8, 6, 5 },
                    { 7, 9, 6, 5 },
                    { 8, 10, 6, 5 },
                    { 9, 13, 12, 5 },
                    { 10, 14, 12, 5 },
                    { 11, 15, 12, 5 },
                    { 12, 16, 12, 5 },
                    { 13, 17, 12, 5 },
                    { 14, 19, 18, 5 },
                    { 15, 20, 18, 5 },
                    { 16, 21, 18, 5 },
                    { 17, 22, 18, 5 },
                    { 18, 24, 23, 5 },
                    { 19, 25, 23, 5 },
                    { 20, 26, 23, 5 },
                    { 21, 27, 23, 5 },
                    { 22, 2, 3, 3 },
                    { 23, 2, 4, 1 },
                    { 24, 4, 5, 1 },
                    { 25, 5, 6, 1 },
                    { 26, 5, 7, 1 },
                    { 27, 5, 8, 1 },
                    { 28, 7, 9, 1 },
                    { 29, 9, 10, 1 },
                    { 30, 10, 11, 1 },
                    { 31, 11, 12, 1 },
                    { 32, 11, 13, 1 },
                    { 33, 13, 14, 1 },
                    { 34, 14, 15, 1 },
                    { 35, 14, 16, 3 },
                    { 36, 15, 17, 1 },
                    { 37, 16, 17, 1 },
                    { 38, 17, 18, 1 },
                    { 39, 17, 19, 1 },
                    { 40, 19, 20, 1 },
                    { 41, 20, 21, 1 },
                    { 42, 21, 22, 1 },
                    { 43, 22, 23, 1 },
                    { 44, 22, 24, 1 },
                    { 45, 22, 25, 1 },
                    { 46, 24, 26, 1 },
                    { 47, 26, 27, 1 },
                    { 48, 25, 27, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_board_columns_BoardId_Name",
                table: "board_columns",
                columns: new[] { "BoardId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_board_tasks_BoardColumnsId",
                table: "board_tasks",
                column: "BoardColumnsId");

            migrationBuilder.CreateIndex(
                name: "IX_board_tasks_BoardId_TaskId",
                table: "board_tasks",
                columns: new[] { "BoardId", "TaskId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_board_tasks_TaskId",
                table: "board_tasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_boards_Name",
                table: "boards",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_boards_OwnerId",
                table: "boards",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_boards_ProjectId",
                table: "boards",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_ChecklistId",
                table: "ChecklistItems",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreatorId",
                table: "Files",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_history_CreatorId",
                table: "history",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_history_RelatedEntityId",
                table: "history",
                column: "RelatedEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_history_RelatedEntityType",
                table: "history",
                column: "RelatedEntityType");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_CreatorId",
                table: "notifications",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_RelatedEntityId",
                table: "notifications",
                column: "RelatedEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_RelatedEntityType",
                table: "notifications",
                column: "RelatedEntityType");

            migrationBuilder.CreateIndex(
                name: "IX_organization_OwnerId",
                table: "organization",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_project_ManagerId",
                table: "project",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_project_Name",
                table: "project",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_project_OrganizationId",
                table: "project",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_project_ProjectStatusId",
                table: "project",
                column: "ProjectStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_project_checklist_checks_ProjectChecklistId",
                table: "project_checklist_checks",
                column: "ProjectChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_project_checklists_CreatedOn",
                table: "project_checklists",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_project_checklists_ProjectId",
                table: "project_checklists",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_project_checklists_WorkerId",
                table: "project_checklists",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_project_links_ProjectId",
                table: "project_links",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_project_member_managements_ProjectId",
                table: "project_member_managements",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_project_member_managements_SubordinateId",
                table: "project_member_managements",
                column: "SubordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_project_member_managements_WorkerId",
                table: "project_member_managements",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_project_statuses_RelatedColorId",
                table: "project_statuses",
                column: "RelatedColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTag_ProjectId",
                table: "ProjectTag",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_sprints_ProjectId",
                table: "sprints",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_sprints_SprintStatusId",
                table: "sprints",
                column: "SprintStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_sprints_Title",
                table: "sprints",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_subscriptions_WorkerId_EntityId_EntityType",
                table: "subscriptions",
                columns: new[] { "WorkerId", "EntityId", "EntityType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_executors_WorkerId",
                table: "task_executors",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_task_filters_Name",
                table: "task_filters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_links_TaskId",
                table: "task_links",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_task_messages_RelatedTaskId",
                table: "task_messages",
                column: "RelatedTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_task_messages_SenderId",
                table: "task_messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_task_observers_AssignedOn",
                table: "task_observers",
                column: "AssignedOn");

            migrationBuilder.CreateIndex(
                name: "IX_task_observers_WorkerId",
                table: "task_observers",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_task_relationship_types_Name",
                table: "task_relationship_types",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_task_relationships_RelatedTaskId",
                table: "task_relationships",
                column: "RelatedTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_task_relationships_TaskId_RelatedTaskId",
                table: "task_relationships",
                columns: new[] { "TaskId", "RelatedTaskId" });

            migrationBuilder.CreateIndex(
                name: "IX_task_relationships_TaskRelationshipTypeId",
                table: "task_relationships",
                column: "TaskRelationshipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_task_types_Name",
                table: "task_types",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tasks_CreatorId",
                table: "tasks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_ExpireOn",
                table: "tasks",
                column: "ExpireOn");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_ParentTaskId",
                table: "tasks",
                column: "ParentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_ProjectId",
                table: "tasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_ShortName",
                table: "tasks",
                column: "ShortName");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_SprintId",
                table: "tasks",
                column: "SprintId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_TaskPriorityId",
                table: "tasks",
                column: "TaskPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_TaskStatusId",
                table: "tasks",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tasks_TaskTypeId",
                table: "tasks",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTag_TaskId",
                table: "TaskTag",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_taskTemplate_links_TaskTemplateId",
                table: "taskTemplate_links",
                column: "TaskTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplates_TaskPriorityId",
                table: "TaskTemplates",
                column: "TaskPriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplates_TaskStatusId",
                table: "TaskTemplates",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplates_TaskTypeId",
                table: "TaskTemplates",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplateTags_TaskTemplateId",
                table: "TaskTemplateTags",
                column: "TaskTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_worker_notifications_WorkerId_NotificationId",
                table: "worker_notifications",
                columns: new[] { "WorkerId", "NotificationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_worker_position_relations_SubordinateWorkerPositionId",
                table: "worker_position_relations",
                column: "SubordinateWorkerPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_worker_position_relations_WorkerPositionId",
                table: "worker_position_relations",
                column: "WorkerPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerProgect_WorkersListId",
                table: "WorkerProgect",
                column: "WorkersListId");

            migrationBuilder.CreateIndex(
                name: "IX_workers_OrganizationsId",
                table: "workers",
                column: "OrganizationsId");

            migrationBuilder.CreateIndex(
                name: "IX_workers_WorkerPositionId",
                table: "workers",
                column: "WorkerPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_workers_management_ManagerId",
                table: "workers_management",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_workers_management_SubordinateId",
                table: "workers_management",
                column: "SubordinateId");

            migrationBuilder.AddForeignKey(
                name: "FK_board_columns_boards_BoardId",
                table: "board_columns",
                column: "BoardId",
                principalTable: "boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_board_tasks_boards_BoardId",
                table: "board_tasks",
                column: "BoardId",
                principalTable: "boards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_board_tasks_tasks_TaskId",
                table: "board_tasks",
                column: "TaskId",
                principalTable: "tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_boards_project_ProjectId",
                table: "boards",
                column: "ProjectId",
                principalTable: "project",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_boards_workers_OwnerId",
                table: "boards",
                column: "OwnerId",
                principalTable: "workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_workers_CreatorId",
                table: "Files",
                column: "CreatorId",
                principalTable: "workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_history_workers_CreatorId",
                table: "history",
                column: "CreatorId",
                principalTable: "workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_workers_CreatorId",
                table: "notifications",
                column: "CreatorId",
                principalTable: "workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_organization_workers_OwnerId",
                table: "organization",
                column: "OwnerId",
                principalTable: "workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_organization_workers_OwnerId",
                table: "organization");

            migrationBuilder.DropTable(
                name: "board_tasks");

            migrationBuilder.DropTable(
                name: "ChangeLogs");

            migrationBuilder.DropTable(
                name: "ChecklistItems");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "history");

            migrationBuilder.DropTable(
                name: "project_checklist_checks");

            migrationBuilder.DropTable(
                name: "project_links");

            migrationBuilder.DropTable(
                name: "project_member_managements");

            migrationBuilder.DropTable(
                name: "ProjectTag");

            migrationBuilder.DropTable(
                name: "subscriptions");

            migrationBuilder.DropTable(
                name: "task_executors");

            migrationBuilder.DropTable(
                name: "task_filters");

            migrationBuilder.DropTable(
                name: "task_links");

            migrationBuilder.DropTable(
                name: "task_messages");

            migrationBuilder.DropTable(
                name: "task_observers");

            migrationBuilder.DropTable(
                name: "task_relationships");

            migrationBuilder.DropTable(
                name: "TaskTag");

            migrationBuilder.DropTable(
                name: "taskTemplate_links");

            migrationBuilder.DropTable(
                name: "TaskTemplateTags");

            migrationBuilder.DropTable(
                name: "worker_notifications");

            migrationBuilder.DropTable(
                name: "worker_position_relations");

            migrationBuilder.DropTable(
                name: "WorkerProgect");

            migrationBuilder.DropTable(
                name: "workers_management");

            migrationBuilder.DropTable(
                name: "board_columns");

            migrationBuilder.DropTable(
                name: "Checklists");

            migrationBuilder.DropTable(
                name: "project_checklists");

            migrationBuilder.DropTable(
                name: "task_relationship_types");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "TaskTemplates");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "boards");

            migrationBuilder.DropTable(
                name: "sprints");

            migrationBuilder.DropTable(
                name: "task_priority");

            migrationBuilder.DropTable(
                name: "task_statuses");

            migrationBuilder.DropTable(
                name: "task_types");

            migrationBuilder.DropTable(
                name: "project");

            migrationBuilder.DropTable(
                name: "sprint_statuses");

            migrationBuilder.DropTable(
                name: "project_statuses");

            migrationBuilder.DropTable(
                name: "colors");

            migrationBuilder.DropTable(
                name: "workers");

            migrationBuilder.DropTable(
                name: "organization");

            migrationBuilder.DropTable(
                name: "worker_positions");
        }
    }
}
