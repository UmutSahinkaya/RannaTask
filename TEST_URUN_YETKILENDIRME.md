# 🔧 ÜRÜN YETKİLENDİRME TEST KILAVUZU

## ✅ SORUN ÇÖZÜLDİ!

**Yapılan Düzeltmeler:**
1. ✅ ProductManager.UpdateAsync → Mevcut ürünü al, CreatedBy'ı koru
2. ✅ ProductManager.GetByIdAsync → CreatedBy'ı da döndür
3. ✅ ProductsController → 403 Forbidden response düzeltildi

---

## 🚀 TEST ADIMLARI:

### 1️⃣ Veritabanı Hazırlığı

**SQL Management Studio'da CreateTestUser.sql'i çalıştır:**
```sql
-- Çalıştır: CreateTestUser.sql
-- 2 kullanıcı oluşturulacak:
-- testcustomer (Customer) / Test123!
-- adminuser (Admin) / Admin123!
```

### 2️⃣ Projeleri Başlat

**Terminal 1 - API:**
```bash
cd RannaTask.API
dotnet run
```

**Terminal 2 - WEB:**
```bash
cd RannaTask.WEB
dotnet run
```

### 3️⃣ Test Senaryosu 1: Customer - Kendi Ürününü Güncelleme ✅

```
1. Browser → http://localhost:XXXX
2. Register: customer1 / Test123! / customer1@test.com
3. Login: customer1 / Test123!
4. Navbar → "Ürünler"
5. "Yeni Ürün Ekle"
   - İsim: "Laptop Dell"
   - Kod: "LP001"
   - Fiyat: 15000
   - Kaydet ✅

6. Ürünler sayfasında "Düzenle" tıkla
7. İsmi değiştir: "Laptop HP"
8. Kaydet ✅
9. SONUÇ: Başarıyla güncellendi! ✅
```

### 4️⃣ Test Senaryosu 2: Customer - Başkasının Ürününü Güncelleme ❌

```
1. Yeni pencere aç (Incognito)
2. Register: customer2 / Test123! / customer2@test.com
3. Login: customer2 / Test123!
4. Navbar → "Ürünler"
5. "Yeni Ürün Ekle"
   - İsim: "Mouse Logitech"
   - Kod: "MS001"
   - Fiyat: 150
   - Kaydet ✅

6. İlk pencereye dön (customer1)
7. Ürünler sayfasını yenile (F5)
8. "Mouse Logitech" görünecek
9. "Düzenle" butonuna tıkla
10. Değişiklik yap, Kaydet
11. SONUÇ: Hata! "Bu ürünü güncelleme yetkiniz yok" ❌
```

### 5️⃣ Test Senaryosu 3: Admin - Her Şeyi Güncelleme ✅

```sql
-- SQL'de customer1'i admin yap
UPDATE Users SET Role = 3 WHERE Username = 'customer1';
```
```
1. customer1 Logout → Login
2. Navbar → "Ürünler"
3. Hem "Laptop HP" hem "Mouse Logitech" var
4. "Mouse Logitech" düzenle (customer2'nin ürünü)
5. Değişiklik yap, Kaydet
6. SONUÇ: Admin olduğu için başarıyla güncellendi! ✅
```

---

## 🔍 SWAGGER İLE TEST:

**1. Swagger aç:** `http://localhost:5094/swagger`

**2. Login (Customer):**
```json
POST /api/auth/login
{
  "username": "customer1",
  "password": "Test123!"
}
```
Response'tan **token**'ı kopyala.

**3. Authorize butonuna tıkla:**
```
Bearer {token}
```

**4. Ürün ekle:**
```json
POST /api/products
{
  "name": "Test Ürün",
  "code": "TST001",
  "price": 100.00,
  "image": "test.jpg"
}
```
Response'tan **Id**'yi not al (örn: 5)

**5. Ürün detayını kontrol et:**
```json
GET /api/products/5
```
Response:
```json
{
  "id": 5,
  "name": "Test Ürün",
  "code": "TST001",
  "price": 100.00,
  "image": "test.jpg",
  "createdBy": 1  ← Senin user ID'n!
}
```

**6. Ürünü güncelle (kendi ürünün):**
```json
PUT /api/products/5
{
  "id": 5,
  "name": "Test Ürün Güncellenmiş",
  "code": "TST001",
  "price": 150.00,
  "image": "test.jpg",
  "createdBy": 1
}
```
SONUÇ: ✅ 200 OK - Başarılı!

**7. Başka birinin ürününü güncellemeyi dene:**
```json
PUT /api/products/1
{
  "id": 1,
  "name": "Başkasının Ürünü",
  ...
}
```
SONUÇ: ❌ 403 Forbidden - "Bu ürünü güncelleme yetkiniz yok"

---

## 🐛 SORUN GİDERME:

### Ürün güncellenmiyor:
```
1. F12 (Developer Tools) → Network
2. Güncelleme butonuna tıkla
3. Hatayı kontrol et:
   - 401 Unauthorized → Token süresi dolmuş, logout → login
   - 403 Forbidden → Yetkiniz yok
   - 400 Bad Request → Validation hatası
```

### CreatedBy NULL:
```sql
-- Eski ürünlere sahip ata
UPDATE Products SET CreatedBy = 1 WHERE CreatedBy IS NULL;
```

### Token almıyorum:
```
1. API çalışıyor mu kontrol et
2. Swagger'dan login test et
3. Response'ta "token" field'ı var mı?
```

---

## 📊 DOĞRULAMA SORULARI:

**SQL ile kontrol et:**
```sql
-- Ürün sahibini kontrol et
SELECT 
    p.Id,
    p.Name,
    p.CreatedBy,
    u.Username AS Owner
FROM Products p
LEFT JOIN Users u ON p.CreatedBy = u.Id;

-- Beklenen sonuç:
-- Id | Name           | CreatedBy | Owner
-- 1  | Laptop HP      | 1         | customer1
-- 2  | Mouse Logitech | 2         | customer2
```

**Beklenen Davranışlar:**
- ✅ Customer kendi ürününü güncelleyebilir
- ✅ Customer kendi ürününü silebilir
- ❌ Customer başkasının ürününü güncelleyemez
- ❌ Customer başkasının ürününü silemez
- ✅ Admin tüm ürünleri güncelleyebilir
- ✅ Admin tüm ürünleri silebilir

---

## ✅ TEST BAŞARILI OLUNCA:

```
🎉 Tebrikler! Ürün yetkilendirme sistemi çalışıyor!

Şimdi yapabilecekleriniz:
1. ✅ Kendi ürünlerini yönetebilirsin
2. ✅ Admin olarak tüm ürünleri yönetebilirsin
3. ✅ Güvenlik ihlali yok (403 Forbidden)
4. ✅ CreatedBy tracking çalışıyor
```

---

**Sorun devam ediyorsa, şu bilgileri paylaş:**
1. Hata mesajı (tam metin)
2. Browser console log'ları (F12)
3. API response (Swagger veya Network tab)
4. SQL sorgu sonuçları (CreatedBy kontrol)
