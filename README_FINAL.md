# 🎉 RannaTask - Destek Talebi & Bildirim Sistemi

## ✅ TAMAMLANAN ÖZELLİKLER

### 🔐 Authentication Sistemi
- ✅ Tek kullanıcı tablosu (User)
- ✅ Role-based authorization (Customer, Manager, Admin)
- ✅ JWT Token authentication
- ✅ Session yönetimi
- ✅ Login/Register sayfaları

### 📋 Destek Talebi (SupportForm)
- ✅ Customer'lar destek talebi oluşturabilir
- ✅ Talepler listeleme sayfası
- ✅ Durum takibi (Beklemede, İşlemde, Silindi)
- ✅ Admin/Manager tüm talepleri görebilir

### 🔔 Bildirim Sistemi
- ✅ Database'de notification tablosu
- ✅ SignalR ile real-time bildirimler
- ✅ Destek talebi oluşturulunca Admin'e otomatik bildirim
- ✅ Notification API endpoints

---

## 🗄️ DATABASE YAPISI

```sql
Users
├── Id (PK)
├── Username (Unique)
├── Email (Unique)
├── PasswordHash
├── FirstName
├── LastName
├── Role (Customer=1, Manager=2, Admin=3)
├── IsActive
└── Created

Notifications
├── Id (PK)
├── Title
├── Message
├── Type (enum)
├── RelatedEntityId (nullable)
├── RelatedEntityType
├── IsRead
├── UserId (nullable, FK → Users)
└── Created

SupportForms
├── Id (PK)
├── UserId (FK → Users)
├── Subject
├── Message
├── Status (Pending, Processed, Deleted)
└── Created

Products
├── Id (PK)
├── Name
├── Code (Unique)
├── Price
└── Created
```

---

## 🚀 NASIL ÇALIŞTIRILIR?

### 1. API Projesini Başlat
```bash
cd RannaTask.API
dotnet run
```
**API URL:** `http://localhost:5094`
**Swagger:** `http://localhost:5094/swagger`

### 2. WEB Projesini Başlat (yeni terminal)
```bash
cd RannaTask.WEB
dotnet run
```

### 3. İlk Kullanıcıyı Oluştur

**Swagger'da:**
```
POST /api/auth/register
```
```json
{
  "firstName": "Test",
  "lastName": "Customer",
  "email": "test@example.com",
  "username": "testcustomer",
  "password": "Test123!"
}
```

### 4. Giriş Yap

**WEB:** `http://localhost:XXXX`
- Username: `testcustomer`
- Password: `Test123!`

---

## 📡 API ENDPOINTS

### Authentication
```
POST /api/auth/login        → Login (username veya email)
POST /api/auth/register     → Register (Customer olur)
```

### Support Forms
```
GET  /api/supportform              → Tüm talepler (Admin/Manager)
GET  /api/supportform/my-forms     → Benim taleplerim (Customer)
POST /api/supportform              → Yeni talep oluştur (Customer)
PUT  /api/supportform/{id}/status  → Durum güncelle (Admin/Manager)
```

### Notifications
```
GET  /api/notification/my-notifications  → Bildirimlerim
GET  /api/notification/unread-count      → Okunmamış sayısı
PUT  /api/notification/{id}/mark-read    → Okundu işaretle
PUT  /api/notification/mark-all-read     → Hepsini okundu işaretle
```

### User Management (Admin Only)
```
GET    /api/usermanagement           → Tüm kullanıcılar
GET    /api/usermanagement/{id}      → Kullanıcı detay
PUT    /api/usermanagement/{id}/role → Rol değiştir
PUT    /api/usermanagement/{id}/toggle-active → Aktif/Pasif
DELETE /api/usermanagement/{id}      → Sil
```

### SignalR Hub
```
ws://localhost:5094/notificationHub
```

---

## 🎯 TEST SENARYOSU

### Customer İşlemleri:
1. ✅ Kayıt ol (`testcustomer` / `Test123!`)
2. ✅ Login ol
3. ✅ Navbar'dan **"Destek Taleplerim"** tıkla
4. ✅ **"Yeni Destek Talebi"** oluştur
5. ✅ Liste sayfasında talebini gör

### Admin İşlemleri:
1. ✅ SQL ile bir kullanıcıyı Admin yap:
```sql
UPDATE Users SET Role = 3 WHERE Username = 'testcustomer';
```
2. ✅ Swagger'dan **GET /api/notification/my-notifications** çağır
3. ✅ Destek talebinin bildirimini gör!

---

## 🔔 BİLDİRİM AKIŞI

```
Customer → Destek Talebi Oluşturur
    ↓
API → SupportForm kaydedilir
    ↓
API → Notification oluşturulur (UserId = null, Admin'lere)
    ↓
SignalR → "Admins" grubuna real-time gönderilir
    ↓
Admin → Bildirim alır (DB + Real-time)
```

---

## 📋 WEB SAYFALARI

### Public (Giriş Gerektirmez)
- `/Account/Login` - Giriş
- `/Account/Register` - Kayıt

### Customer
- `/Home/Index` - Ana sayfa
- `/SupportForm/Index` - Destek taleplerim
- `/SupportForm/Create` - Yeni destek talebi
- `/Product/Index` - Ürünler

### Admin (Henüz WEB'de yok, API'den erişilebilir)
- Kullanıcı yönetimi
- Tüm destek talepleri
- Bildirimler

---

## 🎨 KULLANILAN TEKNOLOJİLER

**Backend:**
- ASP.NET Core 6.0 Web API
- Entity Framework Core 6
- SQL Server (LocalDB)
- JWT Authentication
- SignalR (Real-time)
- AutoMapper
- BCrypt.NET

**Frontend:**
- ASP.NET Core 6.0 MVC
- Razor Views
- Bootstrap 5
- Bootstrap Icons
- jQuery

---

## 🔧 GELİŞTİRME NOTLARI

### Rol Sistemi:
- **Customer (1):** Destek talebi oluşturabilir, kendi taleplerini görebilir
- **Manager (2):** Tüm talepleri görebilir, durum güncelleyebilir
- **Admin (3):** Tüm yetkilere sahip, kullanıcı yönetimi yapabilir

### Güvenlik:
- Tüm API endpoint'leri `[Authorize]` ile korumalı
- Role-based access control
- JWT token ile authentication
- BCrypt ile şifre hashleme

### Database:
- Unique constraints (Email, Username, Product Code)
- Cascade delete (User silinince SupportForm ve Notifications silinir)
- Nullable foreign keys (Notification.UserId)

---

## 📝 YAPILACAKLAR (İleride)

- [ ] WEB'de Admin paneli
- [ ] Real-time notification badge (SignalR client)
- [ ] Notification dropdown menü
- [ ] Email bildirimleri
- [ ] SupportForm için mesajlaşma sistemi
- [ ] File upload (Destek talebine dosya ekleme)

---

## 🐛 SORUN GİDERME

**Database bağlantı hatası:**
```bash
dotnet ef database drop --force --project RannaTask.DAL --startup-project RannaTask.API
dotnet ef database update --project RannaTask.DAL --startup-project RannaTask.API
```

**Login yapamıyorum:**
- API çalışıyor mu kontrol edin
- Swagger'dan `/api/auth/login` test edin
- Browser console'da hata var mı bakın

**Token hatası:**
- Session'ı temizleyin (Logout)
- Tekrar login olun

---

## ✅ PROJE DURUMU

**Tamamlanan:** %95
- ✅ Authentication
- ✅ User Management
- ✅ Support Form (Customer)
- ✅ Notification System
- ✅ SignalR Hub
- ✅ Database Migration
- ⏳ Admin Panel (WEB tarafı eksik)
- ⏳ Real-time notification UI

---

**Geliştirici:** RannaTask Team
**Tarih:** 2025-02-07
**Version:** 2.0
