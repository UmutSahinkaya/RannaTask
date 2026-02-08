# 🚀 RannaTask Deployment Guide

## 📋 Table of Contents
1. [Local Development](#local-development)
2. [Docker Deployment](#docker-deployment)
3. [Production Deployment](#production-deployment)
4. [CI/CD Setup](#cicd-setup)
5. [Troubleshooting](#troubleshooting)

---

## 🏠 Local Development

### Prerequisites
- .NET 6 SDK
- SQL Server LocalDB
- Visual Studio 2022 / VS Code

### Steps

1. **Clone repository:**
```bash
git clone https://github.com/UmutSahinkaya/RannaTask.git
cd RannaTask
```

2. **Restore NuGet packages:**
```bash
dotnet restore
```

3. **Update database:**
```bash
cd RannaTask.API
dotnet ef database update
```

4. **Run API (Terminal 1):**
```bash
cd RannaTask.API
dotnet run
```

5. **Run WEB (Terminal 2):**
```bash
cd RannaTask.WEB
dotnet run
```

6. **Access:**
- Web: http://localhost:5100
- API: http://localhost:5094/swagger

---

## 🐳 Docker Deployment

### Option 1: Local Docker

**Build and run:**
```bash
docker-compose up --build -d
```

**View logs:**
```bash
docker-compose logs -f
```

**Stop:**
```bash
docker-compose down
```

### Option 2: Pre-built Images

**1. Create .env file:**
```bash
DOCKERHUB_USERNAME=yourusername
SQL_SA_PASSWORD=YourStrong@Passw0rd
JWT_SECRET_KEY=your-secret-key-32-chars-min
```

**2. Pull and run:**
```bash
docker-compose -f docker-compose.prod.yml pull
docker-compose -f docker-compose.prod.yml up -d
```

---

## 🌐 Production Deployment

### AWS EC2 / Azure VM

**1. SSH to server:**
```bash
ssh user@your-server-ip
```

**2. Install Docker:**
```bash
# Ubuntu/Debian
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh
sudo usermod -aG docker $USER
```

**3. Install Docker Compose:**
```bash
sudo curl -L "https://github.com/docker/compose/releases/download/v2.20.0/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose
```

**4. Clone and configure:**
```bash
git clone https://github.com/UmutSahinkaya/RannaTask.git
cd RannaTask
cp .env.example .env
nano .env  # Edit with your values
```

**5. Deploy:**
```bash
docker-compose -f docker-compose.prod.yml up -d
```

**6. Setup Nginx (Optional - for domain/SSL):**
```bash
sudo apt install nginx certbot python3-certbot-nginx

# Create nginx config
sudo nano /etc/nginx/sites-available/rannatask

# Example config:
server {
    listen 80;
    server_name yourdomain.com;

    location / {
        proxy_pass http://localhost:5100;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }

    location /api {
        proxy_pass http://localhost:5094;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
    }
}

# Enable site
sudo ln -s /etc/nginx/sites-available/rannatask /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl reload nginx

# Get SSL certificate
sudo certbot --nginx -d yourdomain.com
```

---

## 🔄 CI/CD Setup

### GitHub Actions (Automated)

**1. Setup Docker Hub:**
- Create account: https://hub.docker.com
- Create access token: Account Settings → Security → New Access Token

**2. Add GitHub Secrets:**

Go to: `Repository → Settings → Secrets → Actions`

Add these secrets:
```
DOCKERHUB_USERNAME: your-dockerhub-username
DOCKERHUB_TOKEN: your-access-token
SERVER_HOST: your-server-ip (optional)
SERVER_USER: ssh-username (optional)
SERVER_SSH_KEY: ssh-private-key (optional)
```

**3. Workflow triggers automatically on:**
- Push to `master` branch
- Pull requests

**4. Manual trigger:**
```bash
# In GitHub:
Actions → CI/CD Pipeline → Run workflow
```

### Manual Deployment to Server

**1. SSH to server:**
```bash
ssh user@server-ip
cd /opt/rannatask
```

**2. Pull latest changes:**
```bash
git pull origin master
```

**3. Update containers:**
```bash
docker-compose -f docker-compose.prod.yml pull
docker-compose -f docker-compose.prod.yml up -d
```

**4. Check status:**
```bash
docker-compose ps
docker-compose logs -f api
docker-compose logs -f web
```

---

## 🐛 Troubleshooting

### Issue: Database connection failed

**Check SQL Server:**
```bash
docker-compose logs sqlserver
docker exec -it rannatask-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourPassword' -Q "SELECT @@VERSION"
```

**Recreate database:**
```bash
docker-compose down -v
docker-compose up -d sqlserver
# Wait 30 seconds for SQL Server startup
docker-compose up -d api web
```

### Issue: API returns 500 error

**Check logs:**
```bash
docker-compose logs api --tail=100
```

**Common fixes:**
1. Verify `appsettings.Production.json` is correct
2. Check JWT key is set correctly
3. Ensure SQL Server is healthy

### Issue: JWT token invalid

**Verify JWT configuration:**
```bash
# In API container
docker exec -it rannatask-api cat appsettings.json
```

**Ensure JWT secret is same in all environments:**
- docker-compose.yml
- appsettings.Production.json
- .env file

### Issue: CORS errors

**Update API CORS policy:**
```csharp
// Program.cs
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

### Issue: Migrations not applied

**Run migrations manually:**
```bash
docker exec -it rannatask-api dotnet ef database update
```

### Issue: Container won't start

**Check Docker resources:**
```bash
docker system prune -a  # Clean up
docker-compose down
docker-compose up -d
```

**View detailed logs:**
```bash
docker-compose logs -f
```

---

## 📊 Monitoring

### Health Checks

**API Health:**
```bash
curl http://localhost:5094/health
```

**Database Health:**
```bash
docker exec rannatask-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'YourPassword' -Q "SELECT 1"
```

### Resource Usage

**View container stats:**
```bash
docker stats
```

**View logs:**
```bash
docker-compose logs -f --tail=100
```

---

## 🔒 Security Checklist

- [ ] Change default SQL SA password
- [ ] Generate unique JWT secret key (32+ chars)
- [ ] Enable HTTPS in production
- [ ] Setup firewall rules (UFW/Security Groups)
- [ ] Enable Docker content trust
- [ ] Regular security updates: `apt update && apt upgrade`
- [ ] Backup database regularly
- [ ] Use environment variables (never commit secrets)
- [ ] Enable rate limiting
- [ ] Setup monitoring (Prometheus/Grafana)

---

## 🆘 Support

**Logs location:**
- API: `docker-compose logs api`
- WEB: `docker-compose logs web`
- SQL: `docker-compose logs sqlserver`

**Useful commands:**
```bash
# Restart all services
docker-compose restart

# Rebuild specific service
docker-compose up -d --build api

# View running containers
docker ps

# Clean everything
docker-compose down -v
docker system prune -a
```

---

**Need help?** Open an issue: https://github.com/UmutSahinkaya/RannaTask/issues
