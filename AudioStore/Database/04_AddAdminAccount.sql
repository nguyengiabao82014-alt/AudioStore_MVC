USE DB_AudioStore;
GO

INSERT INTO Users (Username, Password, FullName, Email, Phone, RoleID)
VALUES ('admin', '123456', N'Quản Trị Viên Hệ Thống', 'admin@audiostore.com', '0999999999', 1);
GO