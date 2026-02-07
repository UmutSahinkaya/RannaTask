# RannaTask Projesi - Yapılan Düzeltmeler ve Test Kullanıcıları

## 📋 Yapılan Düzeltmeler

### 1. **Authentication Sistemi Eklendi**
- ✅ Login/Register sayfaları oluşturuldu
- ✅ Session tabanlı JWT token yönetimi eklendi
- ✅ Tüm controller'lara authentication kontrolü eklendi
- ✅ API isteklerine otomatik Bearer token ekleme mekanizması
- ✅ Navbar'a Login/Logout butonları eklendi

### 2. **Veritabanı Yapısı Düzeltildi**
- ✅ Kullanılmayan Manager ve UserRole entity'leri kaldırıldı
- ✅ Customer tablosuna unique constraint'ler eklendi (Email, Username)
- ✅ User tablosuna unique constraint eklendi (Username)
- ✅ Product tablosuna unique constraint eklendi (Code)
- ✅ SupportForm - Customer ilişkisi düzeltildi (Navigation property eklendi)
- ✅ Gereksiz seed data'lar temizlendi
- ✅ Yeni migration'lar oluşturuldu ve database güncellendi

### 3. **Configuration Dosyaları Düzenlendi**
- ✅ CustomerConfiguration: Unique index'ler ve validation kuralları eklendi
- ✅ UserConfiguration: Unique index ve admin seed data eklendi
- ✅ ProductConfiguration: Unique index ve validation kuralları eklendi
- ✅ SupportFormConfiguration: Foreign key ilişkisi düzenlendi
- ✅ Gereksiz ManagerConfiguration ve UserRoleConfiguration silindi

### 4. **Model Sınıfları Eklendi**
- ✅ LoginViewModel
- ✅ RegisterViewModel
- ✅ TokenResponse

### 5. **Controller'lar**
- ✅ AccountController oluşturuldu (Login, Register, Logout)
- ✅ ProductController güncellendi (Authentication kontrolü eklendi)
- ✅ HomeController güncellendi (Authentication kontrolü eklendi)

### 6. **Program.cs Güncellendi**
- ✅ Session desteği eklendi
- ✅ Default route Account/Login olarak değiştirildi

## 👥 Test Kullanıcıları

### Customer (Müşteri) Hesabı
```
Kullanıcı Adı: testuser
Şifre: test123
Email: testuser@example.com
Ad: Test
Soyad: User
```

### Admin/User (Panel) Hesabı
```
Kullanıcı Adı: admin
Şifre: admin123
Role: Admin
```

## 🚀 Projeyi Çalıştırma

### 1. Database Güncellemesi (İlk Kurulum)
Database zaten güncellenmiştir, ancak tekrar çalıştırmak isterseniz:

```bash
dotnet ef database update --project RannaTask.DAL --startup-project RannaTask.API
```

### 2. API Projesini Çalıştırma
```bash
cd RannaTask.API
dotnet run
```
API varsayılan olarak `http://localhost:5094` adresinde çalışacaktır.

### 3. WEB Projesini Çalıştırma
Yeni bir terminal açın:
```bash
cd RannaTask.WEB
dotnet run
```

### 4. İlk Giriş
1. Web tarayıcınızda WEB projesinin URL'sine gidin
2. Otomatik olarak Login sayfasına yönlendirileceksiniz
3. Yukarıdaki test kullanıcı bilgileri ile giriş yapın

## 📁 Proje Yapısı

```
RannaTask/
├── RannaTask.API/          # Web API (Backend)
│   ├── Controllers/
│   │   ├── AuthController.cs      # Login/Register endpoints
│   │   ├── ProductsController.cs  # Product CRUD
│   │   └── SupportFormController.cs
│   └── Program.cs
│
├── RannaTask.WEB/          # MVC Web App (Frontend)
│   ├── Controllers/
│   │   ├── AccountController.cs   # Login/Register/Logout
│   │   ├── HomeController.cs
│   │   └── ProductController.cs
│   ├── Models/
│   │   ├── LoginViewModel.cs
│   │   ├── RegisterViewModel.cs
│   │   └── TokenResponse.cs
│   ├── Views/
│   │   ├── Account/
│   │   │   ├── Login.cshtml
│   │   │   └── Register.cshtml
│   │   ├── Product/
│   │   └── Shared/
│   │       └── _Layout.cshtml
│   └── Program.cs
│
├── RannaTask.Business/      # Business Logic Layer
│   ├── Customers/
│   ├── Users/
│   ├── Products/
│   └── Helpers/
│       ├── TokenManager.cs
│       └── PasswordHasher.cs
│
├── RannaTask.DAL/           # Data Access Layer
│   ├── Contexts/
│   │   └── AppDbContext.cs
│   ├── Configurations/
│   │   ├── CustomerConfiguration.cs
│   │   ├── UserConfiguration.cs
│   │   ├── ProductConfiguration.cs
│   │   └── SupportFormConfiguration.cs
│   ├── Repositories/
│   ├── Migrations/
│   └── UnitOfWorks/
│
└── RannaTask.Entities/      # Domain Entities
    └── Entities/
        ├── Customer.cs
        ├── User.cs
        ├── Product.cs
        └── SupportForm.cs
```

## 🔐 Güvenlik Özellikleri

- ✅ BCrypt ile şifre hashleme
- ✅ JWT token tabanlı authentication
- ✅ Session ile token yönetimi
- ✅ Protected controller actions
- ✅ Unique constraint'ler ile veri bütünlüğü

## 🎯 Önemli Endpoint'ler

### API Endpoints
- `POST /api/auth/customer/login` - Customer girişi
- `POST /api/auth/customer/register` - Customer kaydı
- `POST /api/auth/user/login` - Admin/User girişi
- `GET/POST/PUT/DELETE /api/products` - Product CRUD

### Web Routes
- `/Account/Login` - Giriş sayfası
- `/Account/Register` - Kayıt sayfası
- `/Account/Logout` - Çıkış
- `/Product/Index` - Ürün listesi (Authentication gerekli)
- `/Product/CreateProduct` - Ürün ekleme (Authentication gerekli)

## 📝 Notlar

1. **API ve WEB projelerinin ikisi de çalışıyor olmalıdır**
2. **API varsayılan port: 5094**
3. **Tüm şifreler BCrypt ile hashlenmiştir**
4. **Session timeout: 30 dakika**
5. **JWT token expiry: 60 dakika**

## 🐛 Sorun Giderme

### Giriş Yapamıyorum
- API projesinin çalıştığından emin olun
- Doğru kullanıcı adı/şifre kullandığınızdan emin olun
- Browser console'da hata kontrolü yapın

### Database Hatası
```bash
dotnet ef database drop --project RannaTask.DAL --startup-project RannaTask.API
dotnet ef database update --project RannaTask.DAL --startup-project RannaTask.API
```

### Token Hatası
- Session'ı temizleyin (Logout yapın)
- Browser cache'ini temizleyin
- Tekrar login olun

## ✅ Tamamlanan İşler

- [x] Authentication sistemi
- [x] Database yapısı düzeltildi
- [x] Unique constraint'ler eklendi
- [x] Gereksiz entity'ler kaldırıldı
- [x] Migration'lar yeniden oluşturuldu
- [x] Test kullanıcıları eklendi
- [x] Login/Register sayfaları
- [x] Session yönetimi
- [x] JWT token entegrasyonu
- [x] Protected routes

