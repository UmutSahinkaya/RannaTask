-- Mevcut ürünlere CreatedBy ekle (migration'dan sonra eksik kalmış olabilir)

USE RannaTaskdb;
GO

-- Önce kullanıcıları kontrol et
SELECT Id, Username, Role FROM Users;
GO

-- Mevcut ürünleri kontrol et
SELECT Id, Name, Code, CreatedBy FROM Products;
GO

-- SEÇENEK 1: Tüm mevcut ürünleri ilk kullanıcıya ata
-- UPDATE Products 
-- SET CreatedBy = (SELECT TOP 1 Id FROM Users ORDER BY Id)
-- WHERE CreatedBy IS NULL;

-- SEÇENEK 2: Her ürünü farklı kullanıcılara ata (test için)
-- DECLARE @User1Id INT = (SELECT TOP 1 Id FROM Users WHERE Role = 1 ORDER BY Id);
-- DECLARE @User2Id INT = (SELECT Id FROM Users WHERE Id != @User1Id AND Role = 1);

-- UPDATE Products SET CreatedBy = @User1Id WHERE Id % 2 = 1 AND CreatedBy IS NULL;
-- UPDATE Products SET CreatedBy = @User2Id WHERE Id % 2 = 0 AND CreatedBy IS NULL;

-- SEÇENEK 3: Manuel atama (Önerilen - test senaryosu için)
-- İlk ürünü customer1'e ata
-- UPDATE Products SET CreatedBy = 1 WHERE Id = 1;

-- İkinci ürünü customer2'ye ata  
-- UPDATE Products SET CreatedBy = 2 WHERE Id = 2;

PRINT 'Ürün sahipliği güncellemesi için yukarıdaki SQL komutlarından birini uncomment edin.';
PRINT 'Veya yeni eklenen ürünler otomatik olarak token sahibine atanacaktır.';
GO

-- Güncellenmiş durumu kontrol et
SELECT 
    p.Id,
    p.Name,
    p.Code,
    p.CreatedBy,
    u.Username AS CreatedByUsername,
    CASE 
        WHEN p.CreatedBy IS NULL THEN 'Sahipsiz (Eski ürün)'
        ELSE 'Sahibi var'
    END AS OwnershipStatus
FROM Products p
LEFT JOIN Users u ON p.CreatedBy = u.Id;
GO
