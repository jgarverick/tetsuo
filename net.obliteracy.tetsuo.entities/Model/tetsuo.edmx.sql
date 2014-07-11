
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 07/15/2011 06:58:52
-- Generated from EDMX file: C:\Users\Josh\Dropbox\Code\tetsuo\FrameworkV4\net.obliteracy.tetsuo.entities\Model\tetsuo.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [tetsuo];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Hubs_Gateways]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Hub] DROP CONSTRAINT [FK_Hubs_Gateways];
GO
IF OBJECT_ID(N'[dbo].[FK_Spokes_Hubs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Spoke] DROP CONSTRAINT [FK_Spokes_Hubs];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Gateway]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Gateway];
GO
IF OBJECT_ID(N'[dbo].[Hub]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Hub];
GO
IF OBJECT_ID(N'[dbo].[Spoke]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Spoke];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Gateway'
CREATE TABLE [dbo].[Gateway] (
    [GatewayId] int IDENTITY(1,1) NOT NULL,
    [GatewayName] varchar(50)  NULL,
    [GatewayBaseUri] varchar(200)  NULL,
    [GatewayDefaultBinding] varchar(50)  NULL,
    [GatewayIsActive] bit  NULL
);
GO

-- Creating table 'Hub'
CREATE TABLE [dbo].[Hub] (
    [HubId] int IDENTITY(1,1) NOT NULL,
    [HubName] varchar(50)  NULL,
    [HubEndpoint] varchar(50)  NULL,
    [Active] bit  NULL,
    [GatewayId] int  NOT NULL
);
GO

-- Creating table 'Spoke'
CREATE TABLE [dbo].[Spoke] (
    [SpokeId] int IDENTITY(1,1) NOT NULL,
    [SpokeName] varchar(50)  NULL,
    [SpokeContract] varchar(50)  NULL,
    [SpokeBinding] varchar(50)  NULL,
    [SpokeEndpoint] varchar(50)  NULL,
    [SpokeAssembly] varchar(255)  NULL,
    [SpokeClientClass] varchar(255)  NULL,
    [Active] bit  NULL,
    [HubId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [GatewayId] in table 'Gateway'
ALTER TABLE [dbo].[Gateway]
ADD CONSTRAINT [PK_Gateway]
    PRIMARY KEY CLUSTERED ([GatewayId] ASC);
GO

-- Creating primary key on [HubId] in table 'Hub'
ALTER TABLE [dbo].[Hub]
ADD CONSTRAINT [PK_Hub]
    PRIMARY KEY CLUSTERED ([HubId] ASC);
GO

-- Creating primary key on [SpokeId] in table 'Spoke'
ALTER TABLE [dbo].[Spoke]
ADD CONSTRAINT [PK_Spoke]
    PRIMARY KEY CLUSTERED ([SpokeId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [GatewayId] in table 'Hub'
ALTER TABLE [dbo].[Hub]
ADD CONSTRAINT [FK_Hubs_Gateways]
    FOREIGN KEY ([GatewayId])
    REFERENCES [dbo].[Gateway]
        ([GatewayId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Hubs_Gateways'
CREATE INDEX [IX_FK_Hubs_Gateways]
ON [dbo].[Hub]
    ([GatewayId]);
GO

-- Creating foreign key on [HubId] in table 'Spoke'
ALTER TABLE [dbo].[Spoke]
ADD CONSTRAINT [FK_Spokes_Hubs]
    FOREIGN KEY ([HubId])
    REFERENCES [dbo].[Hub]
        ([HubId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Spokes_Hubs'
CREATE INDEX [IX_FK_Spokes_Hubs]
ON [dbo].[Spoke]
    ([HubId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------