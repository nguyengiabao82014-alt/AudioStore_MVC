CREATE DATABASE DB_AudioStore;
GO

USE DB_AudioStore;
GO

CREATE TABLE Roles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL
);

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(50) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    Phone VARCHAR(20),
    RoleID INT NOT NULL FOREIGN KEY REFERENCES Roles(RoleID),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    ParentType NVARCHAR(50) NOT NULL
);

CREATE TABLE Brands (
    BrandID INT IDENTITY(1,1) PRIMARY KEY,
    BrandName NVARCHAR(100) NOT NULL
);

CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL,
    Price DECIMAL(18,0) NOT NULL,
    StockQuantity INT DEFAULT 0,
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    BrandID INT FOREIGN KEY REFERENCES Brands(BrandID),
    IsWireless BIT,
    ImageUrl NVARCHAR(MAX),
    VideoUrl NVARCHAR(MAX),
    Specifications NVARCHAR(MAX),
    Description NVARCHAR(MAX),
    IsActive BIT DEFAULT 1
);

CREATE TABLE DiscountCodes (
    Code VARCHAR(50) PRIMARY KEY,
    DiscountPercent INT NOT NULL,
    MaxDiscountAmount DECIMAL(18,0),
    MinOrderValue DECIMAL(18,0),
    ExpiryDate DATETIME,
    UsageLimit INT,
    IsActive BIT DEFAULT 1
);

CREATE TABLE Orders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,0) NOT NULL,
    DiscountCode VARCHAR(50) FOREIGN KEY REFERENCES DiscountCodes(Code),
    FinalAmount DECIMAL(18,0) NOT NULL,
    PaymentMethod NVARCHAR(50),
    ShippingAddress NVARCHAR(MAX),
    Status NVARCHAR(50) DEFAULT 'Pending'
);

CREATE TABLE OrderDetails (
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,0) NOT NULL,
    PRIMARY KEY (OrderID, ProductID)
);

INSERT INTO Roles (RoleName) VALUES ('Admin'), ('Customer');

INSERT INTO Categories (CategoryName, ParentType) VALUES 
('Trùm đầu (Over-ear)', 'Tai nghe'),
('Nhét tai (In-ear)', 'Tai nghe'),
('Loa to (PartyBox)', 'Loa'),
('Loa xách tay', 'Loa'),
('Soundbar', 'Loa'),
('Thu âm (Studio)', 'Mic'),
('Karaoke', 'Mic');

INSERT INTO Brands (BrandName) VALUES ('Sony'), ('JBL'), ('Harman Kardon'), ('Marshall');
GO