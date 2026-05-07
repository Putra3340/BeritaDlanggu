IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Slug] nvarchar(450) NOT NULL,
    [Description] nvarchar(max) NULL,
    [ParentId] int NULL,
    [SortOrder] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Categories_Categories_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Categories] ([Id])
);

CREATE TABLE [Settings] (
    [Id] int NOT NULL IDENTITY,
    [Key] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Settings] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(450) NOT NULL,
    [Email] nvarchar(450) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    [FullName] nvarchar(max) NOT NULL,
    [AvatarUrl] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [LastLoginAt] datetime2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [ActivityLogs] (
    [Id] int NOT NULL IDENTITY,
    [Action] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NULL,
    [UserId] int NULL,
    [IpAddress] nvarchar(max) NULL,
    [Timestamp] datetime2 NOT NULL,
    CONSTRAINT [PK_ActivityLogs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ActivityLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [Articles] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Slug] nvarchar(450) NOT NULL,
    [Content] nvarchar(max) NOT NULL,
    [Excerpt] nvarchar(max) NULL,
    [ThumbnailUrl] nvarchar(max) NULL,
    [Status] tinyint NOT NULL,
    [IsFeatured] bit NOT NULL,
    [Views] int NOT NULL,
    [MetaTitle] nvarchar(max) NULL,
    [MetaDescription] nvarchar(max) NULL,
    [AuthorId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NOT NULL,
    [PublishedAt] datetime2 NULL,
    CONSTRAINT [PK_Articles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Articles_Users_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Users] ([Id])
);

CREATE TABLE [ArticleCategories] (
    [ArticleId] int NOT NULL,
    [CategoryId] int NOT NULL,
    CONSTRAINT [PK_ArticleCategories] PRIMARY KEY ([ArticleId], [CategoryId]),
    CONSTRAINT [FK_ArticleCategories_Articles_ArticleId] FOREIGN KEY ([ArticleId]) REFERENCES [Articles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ArticleCategories_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Comments] (
    [Id] int NOT NULL IDENTITY,
    [ArticleId] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [Message] nvarchar(max) NOT NULL,
    [IsApproved] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_Articles_ArticleId] FOREIGN KEY ([ArticleId]) REFERENCES [Articles] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_ActivityLogs_UserId] ON [ActivityLogs] ([UserId]);

CREATE INDEX [IX_ArticleCategories_CategoryId] ON [ArticleCategories] ([CategoryId]);

CREATE INDEX [IX_Articles_AuthorId] ON [Articles] ([AuthorId]);

CREATE UNIQUE INDEX [IX_Articles_Slug] ON [Articles] ([Slug]);

CREATE INDEX [IX_Categories_ParentId] ON [Categories] ([ParentId]);

CREATE UNIQUE INDEX [IX_Categories_Slug] ON [Categories] ([Slug]);

CREATE INDEX [IX_Comments_ArticleId] ON [Comments] ([ArticleId]);

CREATE UNIQUE INDEX [IX_Settings_Key] ON [Settings] ([Key]);

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);

CREATE UNIQUE INDEX [IX_Users_Username] ON [Users] ([Username]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260420030716_InitialCreate', N'10.0.6');

COMMIT;
GO

BEGIN TRANSACTION;
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'

EXEC sp_MSforeachtable 'DELETE FROM ?'

EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'


DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql += 
'DBCC CHECKIDENT ([' + t.name + '], RESEED, 0);'
FROM sys.tables t
INNER JOIN sys.identity_columns ic ON t.object_id = ic.object_id;

EXEC sp_executesql @sql;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260422020440_BackupScheme', N'10.0.6');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [ActivityLogs] DROP CONSTRAINT [FK_ActivityLogs_Users_UserId];

ALTER TABLE [ArticleCategories] DROP CONSTRAINT [FK_ArticleCategories_Articles_ArticleId];

ALTER TABLE [ArticleCategories] DROP CONSTRAINT [FK_ArticleCategories_Categories_CategoryId];

ALTER TABLE [Articles] DROP CONSTRAINT [FK_Articles_Users_AuthorId];

ALTER TABLE [Categories] DROP CONSTRAINT [FK_Categories_Categories_ParentId];

DROP TABLE [Comments];

DROP INDEX [IX_Categories_ParentId] ON [Categories];

DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'ParentId');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [Categories] DROP COLUMN [ParentId];

DECLARE @var1 nvarchar(max);
SELECT @var1 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'SortOrder');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT ' + @var1 + ';');
ALTER TABLE [Categories] DROP COLUMN [SortOrder];

ALTER TABLE [Articles] ADD [CatId] int NOT NULL DEFAULT 0;

ALTER TABLE [Articles] ADD [SubCatId] int NULL;

ALTER TABLE [Articles] ADD [ThumbnailFull] nvarchar(max) NULL;

DROP INDEX [IX_ActivityLogs_UserId] ON [ActivityLogs];
DECLARE @var2 nvarchar(max);
SELECT @var2 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ActivityLogs]') AND [c].[name] = N'UserId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [ActivityLogs] DROP CONSTRAINT ' + @var2 + ';');
UPDATE [ActivityLogs] SET [UserId] = 0 WHERE [UserId] IS NULL;
ALTER TABLE [ActivityLogs] ALTER COLUMN [UserId] int NOT NULL;
ALTER TABLE [ActivityLogs] ADD DEFAULT 0 FOR [UserId];
CREATE INDEX [IX_ActivityLogs_UserId] ON [ActivityLogs] ([UserId]);

CREATE TABLE [NavSettings] (
    [Id] int NOT NULL,
    [CatId] int NULL,
    [Title] int NULL,
    [ArticleId] int NULL,
    CONSTRAINT [PK_NavSettings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_NavSettings_Articles] FOREIGN KEY ([ArticleId]) REFERENCES [Articles] ([Id]),
    CONSTRAINT [FK_NavSettings_Categories] FOREIGN KEY ([CatId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [SubCategories] (
    [Id] int NOT NULL IDENTITY,
    [ParentId] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Slug] nvarchar(450) NULL,
    [CreatedAt] datetime NOT NULL,
    CONSTRAINT [PK_SubCategories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SubCategories_Categories] FOREIGN KEY ([ParentId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Articles_CatId] ON [Articles] ([CatId]);

CREATE INDEX [IX_Articles_SubCatId] ON [Articles] ([SubCatId]);

CREATE INDEX [IX_NavSettings_ArticleId] ON [NavSettings] ([ArticleId]);

CREATE INDEX [IX_NavSettings_CatId] ON [NavSettings] ([CatId]);

CREATE INDEX [IX_SubCategories_ParentId] ON [SubCategories] ([ParentId]);

ALTER TABLE [ActivityLogs] ADD CONSTRAINT [FK_ActivityLogs_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;

ALTER TABLE [Articles] ADD CONSTRAINT [FK_Articles_Categories] FOREIGN KEY ([CatId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE;

ALTER TABLE [Articles] ADD CONSTRAINT [FK_Articles_SubCategories] FOREIGN KEY ([SubCatId]) REFERENCES [SubCategories] ([Id]);

ALTER TABLE [Articles] ADD CONSTRAINT [FK_Articles_Users_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260507025521_DynamicNav', N'10.0.6');

COMMIT;
GO

