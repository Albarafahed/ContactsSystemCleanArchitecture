/*
  Run this script against the existing ContactsDB database after taking a backup.
  It preserves all existing Contacts rows. Existing rows keep UserId = NULL.
  Do not assign legacy contacts to arbitrary users.
*/
IF OBJECT_ID(N'dbo.AspNetRoles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetRoles (
        Id nvarchar(450) NOT NULL CONSTRAINT PK_AspNetRoles PRIMARY KEY,
        Name nvarchar(256) NULL,
        NormalizedName nvarchar(256) NULL,
        ConcurrencyStamp nvarchar(max) NULL
    );
    CREATE UNIQUE INDEX RoleNameIndex ON dbo.AspNetRoles(NormalizedName)
        WHERE NormalizedName IS NOT NULL;
END;
GO
IF OBJECT_ID(N'dbo.AspNetUsers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetUsers (
        Id nvarchar(450) NOT NULL CONSTRAINT PK_AspNetUsers PRIMARY KEY,
        UserName nvarchar(256) NULL, NormalizedUserName nvarchar(256) NULL,
        Email nvarchar(256) NULL, NormalizedEmail nvarchar(256) NULL,
        EmailConfirmed bit NOT NULL, PasswordHash nvarchar(max) NULL,
        SecurityStamp nvarchar(max) NULL, ConcurrencyStamp nvarchar(max) NULL,
        PhoneNumber nvarchar(max) NULL, PhoneNumberConfirmed bit NOT NULL,
        TwoFactorEnabled bit NOT NULL, LockoutEnd datetimeoffset NULL,
        LockoutEnabled bit NOT NULL, AccessFailedCount int NOT NULL
    );
    CREATE UNIQUE INDEX UserNameIndex ON dbo.AspNetUsers(NormalizedUserName)
        WHERE NormalizedUserName IS NOT NULL;
    CREATE INDEX EmailIndex ON dbo.AspNetUsers(NormalizedEmail);
END;
GO
IF OBJECT_ID(N'dbo.AspNetRoleClaims', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetRoleClaims (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY,
        RoleId nvarchar(450) NOT NULL, ClaimType nvarchar(max) NULL, ClaimValue nvarchar(max) NULL,
        CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId
            FOREIGN KEY(RoleId) REFERENCES dbo.AspNetRoles(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_AspNetRoleClaims_RoleId ON dbo.AspNetRoleClaims(RoleId);
END;
GO
IF OBJECT_ID(N'dbo.AspNetUserClaims', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetUserClaims (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_AspNetUserClaims PRIMARY KEY,
        UserId nvarchar(450) NOT NULL, ClaimType nvarchar(max) NULL, ClaimValue nvarchar(max) NULL,
        CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId
            FOREIGN KEY(UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_AspNetUserClaims_UserId ON dbo.AspNetUserClaims(UserId);
END;
GO
IF OBJECT_ID(N'dbo.AspNetUserLogins', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetUserLogins (
        LoginProvider nvarchar(128) NOT NULL, ProviderKey nvarchar(128) NOT NULL,
        ProviderDisplayName nvarchar(max) NULL, UserId nvarchar(450) NOT NULL,
        CONSTRAINT PK_AspNetUserLogins PRIMARY KEY(LoginProvider, ProviderKey),
        CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId
            FOREIGN KEY(UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_AspNetUserLogins_UserId ON dbo.AspNetUserLogins(UserId);
END;
GO
IF OBJECT_ID(N'dbo.AspNetUserRoles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetUserRoles (
        UserId nvarchar(450) NOT NULL, RoleId nvarchar(450) NOT NULL,
        CONSTRAINT PK_AspNetUserRoles PRIMARY KEY(UserId, RoleId),
        CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId
            FOREIGN KEY(UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE,
        CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId
            FOREIGN KEY(RoleId) REFERENCES dbo.AspNetRoles(Id) ON DELETE CASCADE
    );
    CREATE INDEX IX_AspNetUserRoles_RoleId ON dbo.AspNetUserRoles(RoleId);
END;
GO
IF OBJECT_ID(N'dbo.AspNetUserTokens', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AspNetUserTokens (
        UserId nvarchar(450) NOT NULL, LoginProvider nvarchar(128) NOT NULL,
        Name nvarchar(128) NOT NULL, Value nvarchar(max) NULL,
        CONSTRAINT PK_AspNetUserTokens PRIMARY KEY(UserId, LoginProvider, Name),
        CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId
            FOREIGN KEY(UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
    );
END;
GO
IF COL_LENGTH(N'dbo.Contacts', N'UserId') IS NULL
    ALTER TABLE dbo.Contacts ADD UserId nvarchar(450) NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Contacts_AspNetUsers_UserId')
    ALTER TABLE dbo.Contacts WITH CHECK ADD CONSTRAINT FK_Contacts_AspNetUsers_UserId
        FOREIGN KEY(UserId) REFERENCES dbo.AspNetUsers(Id);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_Contacts_UserId' AND object_id = OBJECT_ID(N'dbo.Contacts'))
    CREATE INDEX IX_Contacts_UserId ON dbo.Contacts(UserId);
GO
IF OBJECT_ID(N'dbo.RefreshTokens', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RefreshTokens (
        Id int IDENTITY(1,1) NOT NULL CONSTRAINT PK_RefreshTokens PRIMARY KEY,
        TokenHash nvarchar(64) NOT NULL, UserId nvarchar(450) NOT NULL,
        CreatedAtUtc datetime2 NOT NULL, ExpiresAtUtc datetime2 NOT NULL,
        RevokedAtUtc datetime2 NULL, ReplacedByTokenHash nvarchar(64) NULL,
        RowVersion varbinary(8) NOT NULL,
        CONSTRAINT FK_RefreshTokens_AspNetUsers_UserId
            FOREIGN KEY(UserId) REFERENCES dbo.AspNetUsers(Id) ON DELETE CASCADE
    );
    CREATE UNIQUE INDEX IX_RefreshTokens_TokenHash ON dbo.RefreshTokens(TokenHash);
    CREATE INDEX IX_RefreshTokens_UserId_ExpiresAtUtc ON dbo.RefreshTokens(UserId, ExpiresAtUtc);
END;
GO
IF OBJECT_ID(N'dbo.RefreshTokens', N'U') IS NOT NULL
   AND EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.RefreshTokens')
               AND name = N'RowVersion' AND system_type_id = 189)
    ALTER TABLE dbo.RefreshTokens DROP COLUMN RowVersion;
GO
IF COL_LENGTH(N'dbo.RefreshTokens', N'RowVersion') IS NULL
    ALTER TABLE dbo.RefreshTokens ADD RowVersion varbinary(8) NOT NULL
        CONSTRAINT DF_RefreshTokens_RowVersion DEFAULT (CONVERT(varbinary(8), NEWID()));
GO
IF NOT EXISTS (SELECT 1 FROM dbo.AspNetRoles WHERE NormalizedName = N'USER')
    INSERT dbo.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (CONVERT(nvarchar(450), NEWID()), N'User', N'USER', CONVERT(nvarchar(36), NEWID()));
IF NOT EXISTS (SELECT 1 FROM dbo.AspNetRoles WHERE NormalizedName = N'ADMIN')
    INSERT dbo.AspNetRoles(Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (CONVERT(nvarchar(450), NEWID()), N'Admin', N'ADMIN', CONVERT(nvarchar(36), NEWID()));
GO
