# 🚀 RannaTask - Enterprise Product Management System

[![CI/CD Pipeline](https://github.com/UmutSahinkaya/RannaTask/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/UmutSahinkaya/RannaTask/actions/workflows/ci-cd.yml)
[![.NET](https://img.shields.io/badge/.NET-6.0-512BD4)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

> A full-stack enterprise web application built with **ASP.NET Core 6.0**, featuring JWT authentication, role-based authorization, real-time notifications, and comprehensive product & user management.

## 🎯 Live Demo

- **[Live Demo](http://your-deployed-url.com)** (Coming Soon)
- **[API Documentation](http://your-deployed-url.com/swagger)** (Swagger UI)

## ✨ Key Features

- ✅ **Authentication & Authorization** (JWT + Role-based)
- ✅ **User Management** (Admin Panel)
- ✅ **Product CRUD** (Owner-based permissions)
- ✅ **Support Form System** (Customer → Admin)
- ✅ **Real-time Notifications** (SignalR)
- ✅ **Password Reset** (Email code verification)
- ✅ **Activity Logging** (Audit trail)
- ✅ **Dark Mode Support**
- ✅ **Responsive Design** (Bootstrap 5)
- ✅ **DataTables** (Sorting, filtering, pagination)

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                        │
│  ┌──────────────────┐          ┌─────────────────────────┐  │
│  │  RannaTask.API   │          │   RannaTask.WEB         │  │
│  │  (REST API)      │          │   (MVC/Razor Pages)     │  │
│  └────────┬─────────┘          └───────────┬─────────────┘  │
└───────────┼────────────────────────────────┼────────────────┘
            │                                │
┌───────────┼────────────────────────────────┼────────────────┐
│           │     Business Logic Layer       │                │
│           │    ┌───────────────────────────▼─────────┐      │
│           └────►   RannaTask.Business                │      │
│                │   • Services & Managers              │      │
│                │   • DTOs (Data Transfer Objects)     │      │
│                │   • Business Rules & Validation      │      │
│                └───────────────┬──────────────────────┘      │
└────────────────────────────────┼───────────────────────────┘
                                 │
┌────────────────────────────────┼───────────────────────────┐
│           Data Access Layer    │                            │
│                ┌───────────────▼──────────────────────┐     │
│                │   RannaTask.DAL                      │     │
│                │   • EF Core DbContext                │     │
│                │   • Repositories (Generic)           │     │
│                │   • Unit of Work Pattern             │     │
│                │   • Database Configurations          │     │
│                └───────────────┬──────────────────────┘     │
└────────────────────────────────┼───────────────────────────┘
                                 │
┌────────────────────────────────┼───────────────────────────┐
│             Domain Layer       │                            │
│                ┌───────────────▼──────────────────────┐     │
│                │   RannaTask.Entities                 │     │
│                │   • Entity Models                    │     │
│                │   • Enums                            │     │
│                │   • Base Entities                    │     │
│                └──────────────────────────────────────┘     │
└────────────────────────────────────────────────────────────┘
```

### Design Patterns Used:
- **Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management
- **Dependency Injection** - Loose coupling
- **DTO Pattern** - Data transfer between layers
- **Factory Pattern** - Object creation
- **Base Controller Pattern** - Code reusability

---

## 🏛️ Project Structure

```
RannaTask/
├── RannaTask.API/              # REST API Layer
│   ├── Controllers/            # API Endpoints
│   ├── Hubs/                   # SignalR Hubs
│   ├── Data/                   # Seed Data
│   └── Dockerfile              # API Container
│
├── RannaTask.WEB/              # Web Application Layer
│   ├── Controllers/            # MVC Controllers
│   ├── Views/                  # Razor Views
│   ├── Models/                 # View Models
│   ├── Common/                 # Shared Components
│   └── Dockerfile              # Web Container
│
├── RannaTask.Business/         # Business Logic Layer
│   ├── Products/               # Product Services
│   ├── Users/                  # User Services
│   ├── SupportForms/           # Support Services
│   ├── Notifications/          # Notification Services
│   └── Extensions/             # Extension Methods
│
├── RannaTask.DAL/              # Data Access Layer
│   ├── Contexts/               # EF Core DbContext
│   ├── Configurations/         # Entity Configurations
│   ├── Repositories/           # Repository Implementations
│   └── UnitOfWork/             # Unit of Work
│
└── RannaTask.Entities/         # Domain Layer
    ├── Entities/               # Domain Models
    ├── Common/                 # Base Entities & Enums
    └── DTOs/                   # Data Transfer Objects
```

## 🛠️ Tech Stack

**Backend:**
- ASP.NET Core 6.0 Web API
- Entity Framework Core 6
- SQL Server
- JWT Authentication
- SignalR (Real-time)
- Swagger/OpenAPI

**Frontend:**
- ASP.NET Core MVC
- Bootstrap 5
- jQuery
- DataTables
- SweetAlert2
- SignalR Client

## 🐳 Quick Start with Docker

### Prerequisites
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Run with Docker Compose

1. **Clone the repository:**
```bash
git clone https://github.com/UmutSahinkaya/RannaTask.git
cd RannaTask
```

2. **Create .env file:**
```bash
cp .env.example .env
# Edit .env and set your passwords
```

3. **Start the application:**
```bash
docker-compose up -d
```

4. **Access the application:**
- **Web UI:** http://localhost:5100
- **API:** http://localhost:5094
- **Swagger:** http://localhost:5094/swagger

5. **Default credentials:**
```
Admin:
Username: admin
Password: Admin123!

Customer:
Username: testcustomer
Password: Test123!
```

6. **Stop the application:**
```bash
docker-compose down
```

## 💻 Local Development (Without Docker)

### Prerequisites
- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb)

### Setup

1. **Clone and restore:**
```bash
git clone https://github.com/UmutSahinkaya/RannaTask.git
cd RannaTask
dotnet restore
```

2. **Update database:**
```bash
cd RannaTask.API
dotnet ef database update
```

3. **Run API:**
```bash
cd RannaTask.API
dotnet run
# API: http://localhost:5094
# Swagger: http://localhost:5094/swagger
```

4. **Run WEB (new terminal):**
```bash
cd RannaTask.WEB
dotnet run
# WEB: http://localhost:5100
```

## 🔧 Configuration

### API (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=RannaTaskdb;..."
  },
  "JwtTokenOptions": {
    "Key": "your-secret-key-here",
    "Issuer": "RannaTaskAPI",
    "Audience": "RannaTaskClients",
    "DurationInMinutes": 60
  }
}
```

## 🚀 CI/CD Pipeline

GitHub Actions workflow automatically:
1. ✅ Runs tests on every push/PR
2. ✅ Builds Docker images on master push
3. ✅ Pushes images to Docker Hub
4. ✅ Deploys to production server (optional)

### GitHub Secrets Required

Set these in **Settings → Secrets → Actions:**

```
DOCKERHUB_USERNAME     # Docker Hub username
DOCKERHUB_TOKEN        # Docker Hub access token
SERVER_HOST            # Production server IP (optional)
SERVER_USER            # SSH username (optional)
SERVER_SSH_KEY         # SSH private key (optional)
```

## 📦 Docker Images

Pull pre-built images from Docker Hub:

```bash
docker pull yourusername/rannatask-api:latest
docker pull yourusername/rannatask-web:latest
```

## 🧪 Testing

Run unit tests:
```bash
dotnet test
```

## 📚 API Documentation

Once running, visit **Swagger UI:**
- Local: http://localhost:5094/swagger
- Docker: http://localhost:5094/swagger

### Authentication Flow
1. **POST** `/api/auth/login` → Get JWT token
2. Click **Authorize** button in Swagger
3. Enter: `Bearer {your-token}`
4. Test protected endpoints ✅

## 🗂️ Database Schema

- **Users** - User accounts with roles
- **Products** - Product catalog
- **SupportForms** - Customer support tickets
- **Notifications** - Real-time notifications
- **ActivityLogs** - Audit trail (coming soon)

## 🔐 Security Features

- ✅ JWT token authentication
- ✅ Role-based authorization (Admin, Manager, Customer)
- ✅ Password hashing (BCrypt)
- ✅ Password reset with verification code
- ✅ Login attempt limiting (3 tries)
- ✅ Owner-based resource access control
- ✅ HTTPS support
- ✅ CORS configuration

## 🌟 Upcoming Features

- [ ] Email service (SMTP)
- [ ] 2FA (Two-factor authentication)
- [ ] Activity logging
- [ ] Dashboard analytics
- [ ] Excel/PDF export
- [ ] Image optimization
- [ ] Caching (Redis)
- [ ] Rate limiting

## 🤝 Contributing

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License.

## 👨‍💻 Author

**Umut Sahinkaya**
- GitHub: [@UmutSahinkaya](https://github.com/UmutSahinkaya)

## 📞 Support

For issues and questions:
- Open an [Issue](https://github.com/UmutSahinkaya/RannaTask/issues)
- Check [Documentation](https://github.com/UmutSahinkaya/RannaTask/wiki)

---

⭐ **Star this repo if you find it useful!**
