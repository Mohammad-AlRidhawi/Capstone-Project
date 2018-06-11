
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/01/2015 13:13:56
-- Generated from EDMX file: E:\Downloads\AntiCorruptionSeachEngine - April 1 - Duncan\AntiCorruptionSeachEngine\AntiCorruptionSeachEngine\SearchModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Duncan];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__link_indu__count__7C4F7684]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[link_industry_country] DROP CONSTRAINT [FK__link_indu__count__7C4F7684];
GO
IF OBJECT_ID(N'[dbo].[FK__link_indu__indus__5BE2A6F2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[link_industry_website] DROP CONSTRAINT [FK__link_indu__indus__5BE2A6F2];
GO
IF OBJECT_ID(N'[dbo].[FK__link_indu__websi__5AEE82B9]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[link_industry_website] DROP CONSTRAINT [FK__link_indu__websi__5AEE82B9];
GO
IF OBJECT_ID(N'[dbo].[FK__link_indu__websi__7B5B524B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[link_industry_country] DROP CONSTRAINT [FK__link_indu__websi__7B5B524B];
GO
IF OBJECT_ID(N'[dbo].[FK__link_webs__websi__5EBF139D]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[link_website_words] DROP CONSTRAINT [FK__link_webs__websi__5EBF139D];
GO
IF OBJECT_ID(N'[dbo].[FK__link_webs__word___5FB337D6]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[link_website_words] DROP CONSTRAINT [FK__link_webs__word___5FB337D6];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[admin_users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[admin_users];
GO
IF OBJECT_ID(N'[dbo].[country]', 'U') IS NOT NULL
    DROP TABLE [dbo].[country];
GO
IF OBJECT_ID(N'[dbo].[did_you_know]', 'U') IS NOT NULL
    DROP TABLE [dbo].[did_you_know];
GO
IF OBJECT_ID(N'[dbo].[histories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[histories];
GO
IF OBJECT_ID(N'[dbo].[industries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[industries];
GO
IF OBJECT_ID(N'[dbo].[link_industry_country]', 'U') IS NOT NULL
    DROP TABLE [dbo].[link_industry_country];
GO
IF OBJECT_ID(N'[dbo].[link_industry_website]', 'U') IS NOT NULL
    DROP TABLE [dbo].[link_industry_website];
GO
IF OBJECT_ID(N'[dbo].[link_website_words]', 'U') IS NOT NULL
    DROP TABLE [dbo].[link_website_words];
GO
IF OBJECT_ID(N'[dbo].[websites]', 'U') IS NOT NULL
    DROP TABLE [dbo].[websites];
GO
IF OBJECT_ID(N'[dbo].[words]', 'U') IS NOT NULL
    DROP TABLE [dbo].[words];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'admin_users'
CREATE TABLE [dbo].[admin_users] (
    [id] int IDENTITY(1,1) NOT NULL,
    [username] varchar(500)  NULL,
    [password] varchar(500)  NULL,
    [lastlogin] datetime  NULL,
    [previouslogin] datetime  NULL
);
GO

-- Creating table 'countries'
CREATE TABLE [dbo].[countries] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] varchar(50)  NULL
);
GO

-- Creating table 'did_you_know'
CREATE TABLE [dbo].[did_you_know] (
    [id] int IDENTITY(1,1) NOT NULL,
    [info] varchar(1000)  NULL
);
GO

-- Creating table 'histories'
CREATE TABLE [dbo].[histories] (
    [id] int IDENTITY(1,1) NOT NULL,
    [phrase] varchar(50)  NULL,
    [frequency] int  NULL
);
GO

-- Creating table 'industries'
CREATE TABLE [dbo].[industries] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] varchar(30)  NULL,
    [keywords] varchar(50)  NULL
);
GO

-- Creating table 'link_country_website'
CREATE TABLE [dbo].[link_country_website] (
    [id] int IDENTITY(1,1) NOT NULL,
    [website_id] int  NULL,
    [country_id] int  NULL
);
GO

-- Creating table 'link_industry_website'
CREATE TABLE [dbo].[link_industry_website] (
    [id] int IDENTITY(1,1) NOT NULL,
    [website_id] int  NULL,
    [industry_id] int  NULL
);
GO

-- Creating table 'link_website_words'
CREATE TABLE [dbo].[link_website_words] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [website_id] int  NULL,
    [word_id] int  NULL,
    [rank] int  NULL
);
GO

-- Creating table 'websites'
CREATE TABLE [dbo].[websites] (
    [id] int IDENTITY(1,1) NOT NULL,
    [anchor] varchar(250)  NULL,
    [country] varchar(50)  NULL,
    [info] varchar(max)  NULL,
    [title] varchar(100)  NULL,
    [cost] bit  NULL,
    [rank] int  NULL,
    [word_rank] int  NULL
);
GO

-- Creating table 'words'
CREATE TABLE [dbo].[words] (
    [id] int IDENTITY(1,1) NOT NULL,
    [phrase] varchar(100)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'admin_users'
ALTER TABLE [dbo].[admin_users]
ADD CONSTRAINT [PK_admin_users]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'countries'
ALTER TABLE [dbo].[countries]
ADD CONSTRAINT [PK_countries]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'did_you_know'
ALTER TABLE [dbo].[did_you_know]
ADD CONSTRAINT [PK_did_you_know]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'histories'
ALTER TABLE [dbo].[histories]
ADD CONSTRAINT [PK_histories]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'industries'
ALTER TABLE [dbo].[industries]
ADD CONSTRAINT [PK_industries]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'link_country_website'
ALTER TABLE [dbo].[link_country_website]
ADD CONSTRAINT [PK_link_country_website]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'link_industry_website'
ALTER TABLE [dbo].[link_industry_website]
ADD CONSTRAINT [PK_link_industry_website]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'link_website_words'
ALTER TABLE [dbo].[link_website_words]
ADD CONSTRAINT [PK_link_website_words]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id] in table 'websites'
ALTER TABLE [dbo].[websites]
ADD CONSTRAINT [PK_websites]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'words'
ALTER TABLE [dbo].[words]
ADD CONSTRAINT [PK_words]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [country_id] in table 'link_country_website'
ALTER TABLE [dbo].[link_country_website]
ADD CONSTRAINT [FK__link_indu__count__7C4F7684]
    FOREIGN KEY ([country_id])
    REFERENCES [dbo].[countries]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__link_indu__count__7C4F7684'
CREATE INDEX [IX_FK__link_indu__count__7C4F7684]
ON [dbo].[link_country_website]
    ([country_id]);
GO

-- Creating foreign key on [industry_id] in table 'link_industry_website'
ALTER TABLE [dbo].[link_industry_website]
ADD CONSTRAINT [FK__link_indu__indus__5BE2A6F2]
    FOREIGN KEY ([industry_id])
    REFERENCES [dbo].[industries]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__link_indu__indus__5BE2A6F2'
CREATE INDEX [IX_FK__link_indu__indus__5BE2A6F2]
ON [dbo].[link_industry_website]
    ([industry_id]);
GO

-- Creating foreign key on [website_id] in table 'link_country_website'
ALTER TABLE [dbo].[link_country_website]
ADD CONSTRAINT [FK__link_indu__websi__7B5B524B]
    FOREIGN KEY ([website_id])
    REFERENCES [dbo].[websites]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__link_indu__websi__7B5B524B'
CREATE INDEX [IX_FK__link_indu__websi__7B5B524B]
ON [dbo].[link_country_website]
    ([website_id]);
GO

-- Creating foreign key on [website_id] in table 'link_industry_website'
ALTER TABLE [dbo].[link_industry_website]
ADD CONSTRAINT [FK__link_indu__websi__5AEE82B9]
    FOREIGN KEY ([website_id])
    REFERENCES [dbo].[websites]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__link_indu__websi__5AEE82B9'
CREATE INDEX [IX_FK__link_indu__websi__5AEE82B9]
ON [dbo].[link_industry_website]
    ([website_id]);
GO

-- Creating foreign key on [website_id] in table 'link_website_words'
ALTER TABLE [dbo].[link_website_words]
ADD CONSTRAINT [FK__link_webs__websi__5EBF139D]
    FOREIGN KEY ([website_id])
    REFERENCES [dbo].[websites]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__link_webs__websi__5EBF139D'
CREATE INDEX [IX_FK__link_webs__websi__5EBF139D]
ON [dbo].[link_website_words]
    ([website_id]);
GO

-- Creating foreign key on [word_id] in table 'link_website_words'
ALTER TABLE [dbo].[link_website_words]
ADD CONSTRAINT [FK__link_webs__word___5FB337D6]
    FOREIGN KEY ([word_id])
    REFERENCES [dbo].[words]
        ([id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK__link_webs__word___5FB337D6'
CREATE INDEX [IX_FK__link_webs__word___5FB337D6]
ON [dbo].[link_website_words]
    ([word_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------