-- Test Customer Oluşturma
-- Username: testuser
-- Password: test123

USE RannaTaskdb;
GO

-- Test customer ekle
INSERT INTO Customers (FirstName, LastName, Email, Username, PasswordHash, Created)
VALUES 
('Test', 'User', 'testuser@example.com', 'testuser', '$2a$11$vYXQZqJkFqZ0Y9mZXF7p.eTjJKLYPk8YVz5XGj2YqEj8Hg6UqR3Aq', GETUTCDATE());

GO

-- Eklenen kayıtları kontrol et
SELECT * FROM Customers;
SELECT * FROM Users;

GO
