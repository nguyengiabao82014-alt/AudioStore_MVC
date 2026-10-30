USE DB_AudioStore;
GO

-- 1. Sửa lỗi font chữ cho bảng Categories (Danh mục)
UPDATE Categories SET CategoryName = N'Trùm đầu (Over-ear)', ParentType = N'Tai nghe' WHERE CategoryID = 1;
UPDATE Categories SET CategoryName = N'Nhét tai (In-ear)', ParentType = N'Tai nghe' WHERE CategoryID = 2;
UPDATE Categories SET CategoryName = N'Loa to (PartyBox)', ParentType = N'Loa' WHERE CategoryID = 3;
UPDATE Categories SET CategoryName = N'Loa xách tay', ParentType = N'Loa' WHERE CategoryID = 4;
UPDATE Categories SET CategoryName = N'Soundbar', ParentType = N'Loa' WHERE CategoryID = 5;
UPDATE Categories SET CategoryName = N'Thu âm (Studio)', ParentType = N'Mic' WHERE CategoryID = 6;
UPDATE Categories SET CategoryName = N'Karaoke', ParentType = N'Mic' WHERE CategoryID = 7;

-- 2. Sửa lỗi font chữ cho bảng Brands (Hãng)
UPDATE Brands SET BrandName = N'Harman Kardon' WHERE BrandName LIKE '%Harman%';
GO