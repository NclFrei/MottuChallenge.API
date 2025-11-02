# MottuChallenge.API

API para o desafio técnico da Mottu, implementada em ASP.NET Core.  
Ela oferece endpoints para gerenciar usuários, pátios, motos e áreas de cobertura, com autenticação via JWT.

---


## Visão Geral

Esta API serve como backend para gerenciar recursos de um sistema de logística de motos e pátios, com cobertura geográfica (áreas). Ela permite:

- Cadastro e login de usuários  
- Registro e consulta de pátios  
- Registro e consulta de motos  
- Definição de áreas de cobertura  
- Autorização com token JWT  

O código está organizado em camadas: Controllers, Services, Repositories, Domain, Infrastructure, etc.

---

## Funcionalidades

- CRUD de usuários  
- CRUD de pátios  
- CRUD de motos  
- CRUD de áreas  
- Autenticação de usuário (login / JWT)  
- Validação de dados de entrada (usando FluentValidation ou similar)  
- Tratamento centralizado de exceções  
- Mapemento com AutoMapper  

---

## Tecnologias Utilizadas

- .NET / ASP.NET Core  
- Entity Framework Core para acesso a dados  
- AutoMapper  
- JWT para autenticação  
- FluentValidation para validações de DTOs  
- Injeção de dependência (DI)  
- Camadas de repositório / serviço / controller  
- Middleware para tratamento global de erros  

---

## Estrutura do Projeto

```text
MottuChallenge.API.sln
MottuChallenge.API/
├── Application/
│   ├── Mapper/
│   └── Service/
├── Controllers/
├── Domain/
│   ├── Dtos/
│   ├── Enums/
│   ├── Interfaces/
│   ├── Models/
│   └── Validator/
├── Erros/
├── Infrastructure/
│   ├── Configuration/
│   ├── Data/
│   └── Repository/
├── Middleware/
├── Program.cs
├── appsettings.json / appsettings.Development.json
└── MottuChallenge.API.http
```

---

## Pré-requisitos

- .NET SDK (versão usada no projeto)  
- SQL Server / banco de dados compatível (ou alterar para outro)  
- Ferramenta de migrações (ex: `dotnet ef`)  
- Ferramenta de execução (Visual Studio, VS Code, CLI)  

---

## Configuração & Execução

1. Clone o repositório:

   ```bash
   git clone https://github.com/NclFrei/MottuChallenge.API.git
   cd MottuChallenge.API
   ```

2. Ajuste o arquivo `appsettings.json` (ou `appsettings.Development.json`) para configurar a string de conexão e parâmetros JWT:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=...;Database=...;User Id=...;Password=...;"
     },
     "JwtSettings": {
       "SecretKey": "..."
     }
   }
   ```

3. Apply migrations e criar o banco:

   ```bash
   dotnet ef database update
   ```

4. Execute a API:

   ```bash
   dotnet run --project MottuChallenge.API
   ```

   Ou inicie via IDE configurando como startup project.

5. (Opcional) Use o arquivo `MottuChallenge.API.http` para testar chamadas HTTP diretamente no editor

---

## Endpoints & Rotas

| Método | Rota                     | Descrição |
|--------|---------------------------|------------|
| POST   | `/api/auth/login`         | Autenticação / login de usuário |
| POST   | `/api/users`               | Criação de usuário |
| GET    | `/api/users/{id}`         | Consulta de usuário |
| PUT    | `/api/users/{id}`         | Atualização de usuário |
| DELETE | `/api/users/{id}`         | Exclusão de usuário |
| GET    | `/api/patios`              | Listar pátios |
| POST   | `/api/patios`              | Criar pátio |
| ...    | ...                       | ... |

---

## Autenticação / Segurança

- O login retorna um **token JWT** que deve ser enviado em cada requisição autenticada no cabeçalho **Authorization: Bearer {token}**  
- Algumas rotas exigem que o usuário esteja autenticado  
- As permissões de rota devem ser verificadas no código de controller / serviço  

---

## Validações e Tratamento de Erros

- As requisições são validadas com DTOs que usam validadores (ex: `UserCreateRequestValidator`, `AreaCreateRequestValidator`)  
- Quando há erro de validação, retorna-se erro 400 com detalhes  
- Existe um **middleware global de exceção** que captura exceções não esperadas e retorna resposta padronizada  

---

## Testes / Cenários de Uso

- Registrar novo usuário  
- Fazer login  
- Criar pátios, motos e áreas  
- Atribuir motos aos pátios  
- Consultar áreas de cobertura  
- Tentar chamadas inválidas para testar tratamento de erro  

---


