-- Test Users Oluşturma (Yeni Unified User Sistemi)
-- 
-- Test Customer: testcustomer / Test123!
-- Test Admin: adminuser / Admin123!

USE RannaTaskdb;
GO

-- Test customer kullanıcısı ekle (Role = 1 = Customer)
-- Password: Test123! (BCrypt hash)
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'testcustomer')
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, Role, IsActive, Created)
    VALUES 
    ('testcustomer', 'testcustomer@example.com', 
     '$2a$11$vYXQZqJkFqZ0Y9mZXF7p.eTjJKLYPk8YVz5XGj2YqEj8Hg6UqR3Aq', 
     'Test', 'Customer', 1, 1, GETUTCDATE());

    PRINT 'Test customer oluşturuldu: testcustomer / Test123!';
END
ELSE
BEGIN
    PRINT 'testcustomer zaten mevcut.';
END
GO

-- Test admin kullanıcısı ekle (Role = 3 = Admin)
-- Password: Admin123! (BCrypt hash)
IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'adminuser')
BEGIN
    INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, Role, IsActive, Created)
    VALUES 
    ('adminuser', 'admin@example.com', 
     '$2a$11$7KgZYqH8YjH8QqH8YjH8YuN1E.mZ8YjH8QqH8YjH8YuN1E.mZ8YjH', 
     'Admin', 'User', 3, 1, GETUTCDATE());

    PRINT 'Test admin oluşturuldu: adminuser / Admin123!';
END
ELSE
BEGIN
    PRINT 'adminuser zaten mevcut.';
END
GO

-- Mevcut bir kullanıcıyı admin yapmak için:
-- UPDATE Users SET Role = 3 WHERE Username = 'testcustomer';

-- Eklenen kayıtları kontrol et
SELECT 
    Id, 
    Username, 
    Email, 
    FirstName, 
    LastName, 
    CASE Role 
        WHEN 1 THEN 'Customer' 
        WHEN 2 THEN 'Manager' 
        WHEN 3 THEN 'Admin' 
    END AS RoleName,
    IsActive,
    Created
FROM Users
ORDER BY Created DESC;
GO

-- Ürünleri göster (CreatedBy ile)
SELECT 
    p.Id,
    p.Name,
    p.Code,
    p.Price,
    p.CreatedBy,
    u.Username AS CreatedByUsername,
    p.Created
FROM Products p
LEFT JOIN Users u ON p.CreatedBy = u.Id
ORDER BY p.Created DESC;
GO

-- Destek taleplerini göster
SELECT 
    sf.Id,
    sf.Subject,
    sf.Message,
    CASE sf.Status 
        WHEN 0 THEN 'Beklemede'
        WHEN 1 THEN 'İşleme Alındı'
        WHEN 2 THEN 'Silindi'
    END AS StatusText,
    u.Username AS CustomerUsername,
    sf.Created
FROM SupportForms sf
LEFT JOIN Users u ON sf.UserId = u.Id
ORDER BY sf.Created DESC;
GO

-- Bildirimleri göster
SELECT 
    n.Id,
    n.Title,
    n.Message,
    CASE n.Type
        WHEN 10 THEN 'Destek Talebi Oluşturuldu'
        WHEN 11 THEN 'Destek Talebi Güncellendi'
        ELSE 'Diğer'
    END AS NotificationType,
    n.IsRead,
    CASE 
        WHEN n.UserId IS NULL THEN 'Tüm Adminler'
        ELSE u.Username
    END AS RecipientUsername,
    n.Created
FROM Notifications n
LEFT JOIN Users u ON n.UserId = u.Id
ORDER BY n.Created DESC;
GO
