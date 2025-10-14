# API REST – Projeto CarbonNowAPI

## 📖 Visão Geral
Esta API foi desenvolvida em **C# (.NET 8)** para fornecer endpoints REST de forma escalável e containerizada.  
O projeto utiliza **Oracle Database** como persistência, e é totalmente automatizado via **GitHub Actions** para build, testes, publicação no **Docker Hub** e deploy contínuo na **Azure**.

---

## Como executar localmente com Docker

### Pré-requisitos
- Docker  
- Docker Compose  

### Passos

**1. Clone o repositório:**
```bash
git clone https://github.com/jpaulo-rb/CarbonNowDevOps.git
cd CarbonNowAPI
```

**2. Configure as variáveis de ambiente (appsettings.json):**
```json
{
  "ConnectionString:Oracle": "User Id=SeuUsuario;Password=SuaSenha;Data Source=SeuDataSource",
  "Jwt:Key": "SuaJwtKey",
  "Jwt:Issuer": "SeuJwtIssuer",
  "Jwt:Audience": "SeuJwtAudience"
}
```

**3. Build e execução do container:**
```bash
docker-compose up --build
```

**4. Acesse a aplicação:**
- Swagger: [http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)
