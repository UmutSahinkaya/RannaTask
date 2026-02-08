# 🔍 LOCAL SQL SERVER BİLGİLERİNİ BULMA REHBERİ

## 1️⃣ SQL Server Instance'ını Bulma

### PowerShell ile:
```powershell
# SQL Server servislerini listele
Get-Service -Name *SQL* | Where-Object {$_.Status -eq 'Running'}

# Beklenen çıktı:
# MSSQL$MSSQLSERVER  (varsayılan instance)
# veya
# MSSQL$SQLEXPRESS    (SQL Server Express)
# veya  
# MSSQLFDLauncher$LOCALDB (LocalDB)
```

### SQL Server Configuration Manager ile:
```
1. Windows Search → "SQL Server Configuration Manager"
2. SQL Server Services
3. Çalışan servisleri gör:
   - SQL Server (MSSQLSERVER) → Varsayılan instance
   - SQL Server (SQLEXPRESS) → Express instance
   - SQL Server (LOCALDB) → LocalDB
```

---

## 2️⃣ Server Name / Host Bilgisi

### Senaryoya Göre Server Name:

**Senaryo 1: Varsayılan SQL Server Instance**
```
Server Name: localhost
veya
Server Name: .
veya  
Server Name: (local)
veya
Server Name: BILGISAYAR_ADINIZ
```

**Senaryo 2: SQL Server Express**
```
Server Name: localhost\SQLEXPRESS
veya
Server Name: .\SQLEXPRESS
veya
Server Name: BILGISAYAR_ADINIZ\SQLEXPRESS
```

**Senaryo 3: LocalDB**
```
Server Name: (localdb)\MSSQLLocalDB
veya
Server Name: (localdb)\v11.0
```

**Senaryo 4: Named Instance**
```
Server Name: localhost\INSTANCE_NAME
Server Name: BILGISAYAR_ADINIZ\INSTANCE_NAME
```

---

## 3️⃣ Bilgisayar Adını Bulma

### PowerShell:
```powershell
$env:COMPUTERNAME
# Örnek çıktı: DESKTOP-ABC1234
```

### CMD:
```cmd
hostname
```

### Windows Ayarları:
```
Windows Settings → System → About → Device name
```

---

## 4️⃣ Port Numarasını Bulma

### SQL Server Configuration Manager:
```
1. SQL Server Configuration Manager aç
2. SQL Server Network Configuration
3. Protocols for [INSTANCE_NAME]
4. TCP/IP → Sağ tık → Properties
5. IP Addresses sekmesi
6. IPAll → TCP Port: 1433 (varsayılan)
```

### PowerShell ile Registry'den:
```powershell
# Varsayılan instance için
Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\MSSQL*.*\MSSQLServer\SuperSocketNetLib\Tcp' | Select-Object TcpPort

# Express instance için
Get-ItemProperty -Path 'HKLM:\SOFTWARE\Microsoft\Microsoft SQL Server\MSSQL*.*\MSSQLServer\SuperSocketNetLib\Tcp' | Where-Object {$_.PSPath -like '*SQLEXPRESS*'} | Select-Object TcpPort
```

---

## 5️⃣ Authentication Mode (Kimlik Doğrulama)

### Windows Authentication (Önerilen - Local Development):
```
Server: localhost\SQLEXPRESS
Authentication: Windows Authentication
Username: (Otomatik - Windows kullanıcınız)
Password: (Gerekli değil)
```

**Connection String:**
```
Server=localhost\SQLEXPRESS;Database=RannaTaskdb;Integrated Security=True;TrustServerCertificate=True;
```

### SQL Server Authentication:
```
Server: localhost\SQLEXPRESS
Authentication: SQL Server Authentication
Username: sa
Password: [sa kullanıcısının şifresi]
```

**Connection String:**
```
Server=localhost\SQLEXPRESS;Database=RannaTaskdb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;
```

---

## 6️⃣ SQL Server'a Bağlantıyı Test Etme

### SSMS (SQL Server Management Studio):
```
1. SSMS'i aç
2. Connect to Server:
   - Server type: Database Engine
   - Server name: localhost\SQLEXPRESS
   - Authentication: Windows Authentication
3. Connect tıkla
4. Başarılı! → Bağlantı bilgileri doğru
```

### Visual Studio:
```
1. View → SQL Server Object Explorer
2. Add SQL Server
3. Server Name: localhost\SQLEXPRESS
4. Connect
```

### sqlcmd (Command Line):
```cmd
# Windows Authentication
sqlcmd -S localhost\SQLEXPRESS -E

# SQL Authentication
sqlcmd -S localhost\SQLEXPRESS -U sa -P YourPassword

# Test sorgusu
1> SELECT @@SERVERNAME, @@VERSION;
2> GO
```

---

## 7️⃣ appsettings.json İçin Connection String

### Senin Durumun İçin Önerilen:

**LocalDB (Visual Studio ile gelir):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=RannaTaskdb;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

**SQL Server Express:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=RannaTaskdb;Integrated Security=True;TrustServerCertificate=True;"
  }
}
```

**SQL Server (Full - sa kullanıcısı ile):**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RannaTaskdb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

---

## 8️⃣ Hızlı Test PowerShell Script

Dosyayı kaydet: `test-sql-connection.ps1`

```powershell
# Test SQL Server Connection
Write-Host "=== SQL Server Bağlantı Testi ===" -ForegroundColor Cyan

# 1. Çalışan SQL Server servislerini bul
Write-Host "`n1. Çalışan SQL Server Servisleri:" -ForegroundColor Yellow
Get-Service -Name *SQL* | Where-Object {$_.Status -eq 'Running'} | Format-Table -AutoSize

# 2. Bilgisayar adı
Write-Host "`n2. Bilgisayar Adı:" -ForegroundColor Yellow
Write-Host $env:COMPUTERNAME

# 3. Test bağlantıları
Write-Host "`n3. Bağlantı Testleri:" -ForegroundColor Yellow

$connections = @(
    "(localdb)\MSSQLLocalDB",
    "localhost",
    "localhost\SQLEXPRESS",
    ".\SQLEXPRESS"
)

foreach ($server in $connections) {
    Write-Host "`nTest ediliyor: $server" -ForegroundColor Gray
    try {
        $conn = New-Object System.Data.SqlClient.SqlConnection
        $conn.ConnectionString = "Server=$server;Integrated Security=True;Connection Timeout=3;"
        $conn.Open()
        Write-Host "✅ BAŞARILI: $server" -ForegroundColor Green
        
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT @@SERVERNAME AS ServerName, @@VERSION AS Version"
        $reader = $cmd.ExecuteReader()
        if ($reader.Read()) {
            Write-Host "   Server: $($reader["ServerName"])" -ForegroundColor Cyan
        }
        $reader.Close()
        $conn.Close()
    }
    catch {
        Write-Host "❌ BAŞARISIZ: $server" -ForegroundColor Red
    }
}

Write-Host "`n=== Test Tamamlandı ===" -ForegroundColor Cyan
```

Çalıştır:
```powershell
.\test-sql-connection.ps1
```

---

## 9️⃣ Şu Anda Projenin Kullandığı Connection String

Kontrol et:
```powershell
# RannaTask.API dizininde
Get-Content appsettings.json | Select-String "ConnectionStrings" -Context 0,2
```

---

## 🎯 ÖZET: Senin İçin Gerekli Bilgiler

PowerShell'de şunu çalıştır:
```powershell
# 1. SQL Server çalışıyor mu?
Get-Service -Name *SQL* | Where-Object {$_.Status -eq 'Running'}

# 2. Bilgisayar adı
$env:COMPUTERNAME

# 3. Test bağlantı (LocalDB)
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q "SELECT @@SERVERNAME"

# 4. Test bağlantı (Express)
sqlcmd -S "localhost\SQLEXPRESS" -E -Q "SELECT @@SERVERNAME"
```

Bu komutların çıktılarını bana gönder, connection string'ini hazırlayayım! 🚀
