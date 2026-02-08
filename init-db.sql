-- RannaTask Database Initialization Script
-- Manuel olarak çalıştırılabilir

USE master;
GO

-- Database oluştur (yoksa)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'RannaTaskdb')
BEGIN
    CREATE DATABASE RannaTaskdb;
    PRINT 'Database RannaTaskdb created.';
END
ELSE
BEGIN
    PRINT 'Database RannaTaskdb already exists.';
END
GO

USE RannaTaskdb;
GO

-- Migration tablosu oluştur
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
    PRINT '__EFMigrationsHistory table created.';
END
GO

PRINT 'Initialization complete. Run migrations from API container.';
