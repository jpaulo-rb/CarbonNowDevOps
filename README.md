# API REST – Projeto CarbonNowAPI

## Visão Geral
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

---

## Pipeline CI/CD

A automação do projeto é realizada com **GitHub Actions**, permitindo integração e entrega contínua (CI/CD) de forma confiável.  

### Ferramentas utilizadas
- **GitHub Actions**: orquestra os jobs de build, teste e deploy.  
- **Docker Hub**: armazena as imagens containerizadas da API.  
- **Azure App Service**: hospeda a aplicação em produção.

### Etapas do pipeline

1. **Trigger**  
   O pipeline é disparado em push ou pull request nas branches `main` ou `develop`.

2. **Build**  
   Compila a API e valida dependências.

3. **Testes**  
   Executa testes unitários e verifica a cobertura de código.

4. **Docker Build & Push**  
   Gera a imagem Docker da API e envia para o **Docker Hub**.

5. **Deploy na Azure**  
   Realiza o deploy automático da imagem mais recente no **Azure App Service**.

### Exemplo de workflow (GitHub Actions)
```yaml
jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 8.0.x
        
    - name: Restore dependencies
      run: dotnet restore ./CarbonNowAPI/CarbonNowAPI.csproj
      
    - name: Build
      run: dotnet build ./CarbonNowAPI/CarbonNowAPI.csproj --no-restore
      
    - name: Test
      run: dotnet test ./CarbonNowAPI-Teste/CarbonNowAPI-Teste.csproj --verbosity normal

    - name: Set up Docker Buildx
      uses: docker/setup-buildx-action@v3

    - name: Log into registry
      uses: docker/login-action@v3
      with:
        registry: https://index.docker.io/v1/
        username: ${{ secrets.DOCKERHUB_USERNAME }}
        password: ${{ secrets.DOCKERHUB_TOKEN }}

    - name: Build and push Docker image
      uses: docker/build-push-action@v5
      with:
        push: true
        tags: ${{ secrets.DOCKERHUB_USERNAME }}/carbonnow-api:${{ github.sha }}
        context: .
        file: ./CarbonNowAPI/Dockerfile


  deploy:
      runs-on: ubuntu-latest
      needs: build
      environment:
        name: 'production'
        url: ${{ steps.deploy-to-webapp.outputs.webapp-url }}
 
      steps:
      - name: Deploy to Azure Web App
        id: deploy-to-webapp
        uses: azure/webapps-deploy@v2
        with:
          app-name: 'carbonnowapi'
          slot-name: 'production'
          publish-profile: ${{ secrets.AZURE_PROFILE }}
          images: '${{ secrets.DOCKERHUB_USERNAME }}/carbonnow-api:${{ github.sha }}'
```
