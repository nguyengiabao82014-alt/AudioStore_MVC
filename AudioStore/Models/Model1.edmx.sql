
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/16/2026 07:42:24
-- Generated from EDMX file: D:\02_DuAnCode\TestVer1\AudioStore\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DB_AudioStore];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__CartItems__CartI__5BE2A6F2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CartItems] DROP CONSTRAINT [FK__CartItems__CartI__5BE2A6F2];
GO
IF OBJECT_ID(N'[dbo].[FK__CartItems__Produ__5CD6CB2B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CartItems] DROP CONSTRAINT [FK__CartItems__Produ__5CD6CB2B];
GO
IF OBJECT_ID(N'[dbo].[FK__Carts__UserID__5629CD9C]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Carts] DROP CONSTRAINT [FK__Carts__UserID__5629CD9C];
GO
IF OBJECT_ID(N'[dbo].[FK__OrderDeta__Order__5165187F]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderDetails] DROP CONSTRAINT [FK__OrderDeta__Order__5165187F];
GO
IF OBJECT_ID(N'[dbo].[FK__OrderDeta__Produ__52593CB8]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderDetails] DROP CONSTRAINT [FK__OrderDeta__Produ__52593CB8];
GO
IF OBJECT_ID(N'[dbo].[FK__Orders__Discount__4D94879B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK__Orders__Discount__4D94879B];
GO
IF OBJECT_ID(N'[dbo].[FK__Orders__UserID__4BAC3F29]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Orders] DROP CONSTRAINT [FK__Orders__UserID__4BAC3F29];
GO
IF OBJECT_ID(N'[dbo].[FK__Products__BrandI__44FF419A]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK__Products__BrandI__44FF419A];
GO
IF OBJECT_ID(N'[dbo].[FK__Products__Catego__440B1D61]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products] DROP CONSTRAINT [FK__Products__Catego__440B1D61];
GO
IF OBJECT_ID(N'[dbo].[FK__Users__RoleID__3B75D760]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK__Users__RoleID__3B75D760];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Brands]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Brands];
GO
IF OBJECT_ID(N'[dbo].[CartItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CartItems];
GO
IF OBJECT_ID(N'[dbo].[Carts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Carts];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[DiscountCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DiscountCodes];
GO
IF OBJECT_ID(N'[dbo].[OrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderDetails];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Brands'
CREATE TABLE [dbo].[Brands] (
    [BrandID] int IDENTITY(1,1) NOT NULL,
    [BrandName] nvarchar(100)  NOT NULL
);
GO

-- Creating table 'CartItems'
CREATE TABLE [dbo].[CartItems] (
    [CartItemID] int IDENTITY(1,1) NOT NULL,
    [CartID] int  NOT NULL,
    [ProductID] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [AddedAt] datetime  NULL
);
GO

-- Creating table 'Carts'
CREATE TABLE [dbo].[Carts] (
    [CartID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NOT NULL,
    [CreatedAt] datetime  NULL,
    [UpdatedAt] datetime  NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [CategoryID] int IDENTITY(1,1) NOT NULL,
    [CategoryName] nvarchar(100)  NOT NULL,
    [ParentType] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'DiscountCodes'
CREATE TABLE [dbo].[DiscountCodes] (
    [Code] varchar(50)  NOT NULL,
    [DiscountPercent] int  NOT NULL,
    [MaxDiscountAmount] decimal(18,0)  NULL,
    [MinOrderValue] decimal(18,0)  NULL,
    [ExpiryDate] datetime  NULL,
    [UsageLimit] int  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'OrderDetails'
CREATE TABLE [dbo].[OrderDetails] (
    [OrderID] int  NOT NULL,
    [ProductID] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [UnitPrice] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [OrderID] int IDENTITY(1,1) NOT NULL,
    [UserID] int  NULL,
    [OrderDate] datetime  NULL,
    [TotalAmount] decimal(18,0)  NOT NULL,
    [DiscountCode] varchar(50)  NULL,
    [FinalAmount] decimal(18,0)  NOT NULL,
    [PaymentMethod] nvarchar(50)  NULL,
    [ShippingAddress] nvarchar(max)  NULL,
    [Status] nvarchar(50)  NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [ProductID] int IDENTITY(1,1) NOT NULL,
    [ProductName] nvarchar(255)  NOT NULL,
    [Price] decimal(18,0)  NOT NULL,
    [StockQuantity] int  NULL,
    [CategoryID] int  NULL,
    [BrandID] int  NULL,
    [IsWireless] bit  NULL,
    [ImageUrl] nvarchar(max)  NULL,
    [VideoUrl] nvarchar(max)  NULL,
    [Specifications] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [RoleID] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [UserID] int IDENTITY(1,1) NOT NULL,
    [Username] varchar(50)  NOT NULL,
    [Password] varchar(255)  NOT NULL,
    [FullName] nvarchar(100)  NOT NULL,
    [Email] varchar(100)  NULL,
    [Phone] varchar(20)  NULL,
    [RoleID] int  NOT NULL,
    [CreatedAt] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [BrandID] in table 'Brands'
ALTER TABLE [dbo].[Brands]
ADD CONSTRAINT [PK_Brands]
    PRIMARY KEY CLUSTERED ([BrandID] ASC);
GO

-- Creating primary key on [CartItemID] in table 'CartItems'
ALTER TABLE [dbo].[CartItems]
ADD CONSTRAINT [PK_CartItems]
    PRIMARY KEY CLUSTERED ([CartItemID] ASC);
GO

-- Creating primary key on [CartID] in table 'Carts'
ALTER TABLE [dbo].[Carts]
ADD CONSTRAINT [PK_Carts]
    PRIMARY KEY CLUSTERED ([CartID] ASC);
GO

-- Creating primary key on [CategoryID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([CategoryID] ASC);
GO

-- Creating primary key on [Code] in table 'DiscountCodes'
ALTER TABLE [dbo].[DiscountCodes]
ADD CONSTRAINT [PK_DiscountCodes]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [OrderID], [ProductID] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [PK_OrderDetails]
    PRIMARY KEY CLUSTERED ([OrderID], [ProductID] ASC);
GO

-- Creating primary key on [OrderID] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([OrderID] ASC);
GO

-- Creating primary key on [ProductID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([ProductID] ASC);
GO

-- Creating primary key on [RoleID] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([RoleID] ASC);
GO

-- Creating primary key on [UserID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([UserID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [BrandID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK__Products__BrandI__44FF419A]
    FOREIGN KEY ([BrandID])
    REFERENCES [dbo].[Brands]
        ([BrandID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Products__BrandI__44FF419A'
CREATE INDEX [IX_FK__Products__BrandI__44FF419A]
ON [dbo].[Products]
    ([BrandID]);
GO

-- Creating foreign key on [CartID] in table 'CartItems'
ALTER TABLE [dbo].[CartItems]
ADD CONSTRAINT [FK__CartItems__CartI__5BE2A6F2]
    FOREIGN KEY ([CartID])
    REFERENCES [dbo].[Carts]
        ([CartID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__CartItems__CartI__5BE2A6F2'
CREATE INDEX [IX_FK__CartItems__CartI__5BE2A6F2]
ON [dbo].[CartItems]
    ([CartID]);
GO

-- Creating foreign key on [ProductID] in table 'CartItems'
ALTER TABLE [dbo].[CartItems]
ADD CONSTRAINT [FK__CartItems__Produ__5CD6CB2B]
    FOREIGN KEY ([ProductID])
    REFERENCES [dbo].[Products]
        ([ProductID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__CartItems__Produ__5CD6CB2B'
CREATE INDEX [IX_FK__CartItems__Produ__5CD6CB2B]
ON [dbo].[CartItems]
    ([ProductID]);
GO

-- Creating foreign key on [UserID] in table 'Carts'
ALTER TABLE [dbo].[Carts]
ADD CONSTRAINT [FK__Carts__UserID__5629CD9C]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Carts__UserID__5629CD9C'
CREATE INDEX [IX_FK__Carts__UserID__5629CD9C]
ON [dbo].[Carts]
    ([UserID]);
GO

-- Creating foreign key on [CategoryID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [FK__Products__Catego__440B1D61]
    FOREIGN KEY ([CategoryID])
    REFERENCES [dbo].[Categories]
        ([CategoryID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Products__Catego__440B1D61'
CREATE INDEX [IX_FK__Products__Catego__440B1D61]
ON [dbo].[Products]
    ([CategoryID]);
GO

-- Creating foreign key on [DiscountCode] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK__Orders__Discount__4D94879B]
    FOREIGN KEY ([DiscountCode])
    REFERENCES [dbo].[DiscountCodes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Orders__Discount__4D94879B'
CREATE INDEX [IX_FK__Orders__Discount__4D94879B]
ON [dbo].[Orders]
    ([DiscountCode]);
GO

-- Creating foreign key on [OrderID] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [FK__OrderDeta__Order__5165187F]
    FOREIGN KEY ([OrderID])
    REFERENCES [dbo].[Orders]
        ([OrderID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ProductID] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [FK__OrderDeta__Produ__52593CB8]
    FOREIGN KEY ([ProductID])
    REFERENCES [dbo].[Products]
        ([ProductID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__OrderDeta__Produ__52593CB8'
CREATE INDEX [IX_FK__OrderDeta__Produ__52593CB8]
ON [dbo].[OrderDetails]
    ([ProductID]);
GO

-- Creating foreign key on [UserID] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [FK__Orders__UserID__4BAC3F29]
    FOREIGN KEY ([UserID])
    REFERENCES [dbo].[Users]
        ([UserID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Orders__UserID__4BAC3F29'
CREATE INDEX [IX_FK__Orders__UserID__4BAC3F29]
ON [dbo].[Orders]
    ([UserID]);
GO

-- Creating foreign key on [RoleID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK__Users__RoleID__3B75D760]
    FOREIGN KEY ([RoleID])
    REFERENCES [dbo].[Roles]
        ([RoleID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Users__RoleID__3B75D760'
CREATE INDEX [IX_FK__Users__RoleID__3B75D760]
ON [dbo].[Users]
    ([RoleID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------