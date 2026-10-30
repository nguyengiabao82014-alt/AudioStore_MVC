USE DB_AudioStore;
GO

-- 1. Tạo bảng Carts (Giỏ hàng)
-- Bảng này lưu thông tin giỏ hàng của từng User
CREATE TABLE Carts (
    CartID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL UNIQUE FOREIGN KEY REFERENCES Users(UserID), -- Mỗi User chỉ có 1 giỏ hàng duy nhất
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);
GO

-- 2. Tạo bảng CartItems (Chi tiết giỏ hàng)
-- Bảng này lưu các sản phẩm nằm trong giỏ hàng
CREATE TABLE CartItems (
    CartItemID INT IDENTITY(1,1) PRIMARY KEY,
    CartID INT NOT NULL FOREIGN KEY REFERENCES Carts(CartID),
    ProductID INT NOT NULL FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT NOT NULL DEFAULT 1,
    AddedAt DATETIME DEFAULT GETDATE(),
    -- Đảm bảo một sản phẩm không bị thêm nhiều lần vào cùng 1 giỏ hàng (mà sẽ tăng Quantity)
    CONSTRAINT UQ_Cart_Product UNIQUE (CartID, ProductID) 
);
GO