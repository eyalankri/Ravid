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
GO

CREATE TABLE [Roles] (
    [RoleId] int NOT NULL IDENTITY,
    [RoleName] nvarchar(max) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([RoleId])
);
GO

CREATE TABLE [Users] (
    [UserId] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Email] nvarchar(50) NOT NULL,
    [Password] char(64) NOT NULL,
    [Phone1] varchar(20) NOT NULL,
    [Phone2] varchar(20) NULL,
    [DateRegistered] datetime2 NOT NULL DEFAULT (getdate()),
    [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit),
    [Company] nvarchar(300) NULL,
    [Comment] ntext NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

CREATE TABLE [TrasportRequests] (
    [TrasportRequestId] uniqueidentifier NOT NULL,
    [ForDate] datetime2 NOT NULL,
    [Comment] ntext NULL,
    [NumberOfPlates] int NOT NULL,
    [DeliveryFor] nvarchar(400) NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (getdate()),
    [TrasportRequestStatus] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_TrasportRequests] PRIMARY KEY ([TrasportRequestId]),
    CONSTRAINT [FK_TrasportRequests_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserInRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_UserInRoles] PRIMARY KEY ([RoleId], [UserId]),
    CONSTRAINT [FK_UserInRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([RoleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserInRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'RoleName') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([RoleId], [RoleName])
VALUES (1, N'Administrator');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'RoleName') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'RoleName') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([RoleId], [RoleName])
VALUES (2, N'Client');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'RoleName') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserId', N'Comment', N'Company', N'DateRegistered', N'Email', N'FirstName', N'LastName', N'Password', N'Phone1', N'Phone2') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([UserId], [Comment], [Company], [DateRegistered], [Email], [FirstName], [LastName], [Password], [Phone1], [Phone2])
VALUES ('482f8b0e-1fe7-4ea6-9c10-4726a859b627', NULL, N'ET Internet Services', '2021-01-28T12:03:04.9819353+02:00', N'eyal.ank@gmail.com', N'Eyal', N'Ankri', '744fd6f1e1f3bc2d2a023c27f4bcc1a12523767d55de7508c0b21a160ab1fdbf', '054-6680240', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'UserId', N'Comment', N'Company', N'DateRegistered', N'Email', N'FirstName', N'LastName', N'Password', N'Phone1', N'Phone2') AND [object_id] = OBJECT_ID(N'[Users]'))
    SET IDENTITY_INSERT [Users] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[UserInRoles]'))
    SET IDENTITY_INSERT [UserInRoles] ON;
INSERT INTO [UserInRoles] ([RoleId], [UserId])
VALUES (1, '482f8b0e-1fe7-4ea6-9c10-4726a859b627');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'RoleId', N'UserId') AND [object_id] = OBJECT_ID(N'[UserInRoles]'))
    SET IDENTITY_INSERT [UserInRoles] OFF;
GO

CREATE INDEX [IX_TrasportRequests_UserId] ON [TrasportRequests] ([UserId]);
GO

CREATE INDEX [IX_UserInRoles_UserId] ON [UserInRoles] ([UserId]);
GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20210128100305_Initial', N'5.0.2');
GO

COMMIT;
GO

