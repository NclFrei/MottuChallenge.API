# Mottu Challenge - CP2 FIAP

Este projeto faz parte da disciplina **Advanced Business Development with .NET** e tem como objetivo criar uma API RESTful que soluciona um desafio real da empresa Mottu. A aplicação permite que usuários registrem pátios, associem áreas a esses pátios e façam a gestão desses recursos com segurança e boas práticas.

---

## Descrição do Projeto

Uma API RESTful desenvolvida em **.NET 8** com banco de dados **Oracle** utilizando **Clean Architecture**, **Domain-Driven Design (DDD)** e **Entity Framework Core**. O sistema permite:

- Cadastro de usuários
- Criação e gerenciamento de pátios
- Associação de áreas a pátios
- Autenticação via JWT

---

## Tecnologias Utilizadas

- [.NET 8]
- ASP.NET Core 
- Entity Framework Core
- Oracle Database
- Swagger
- JWT Authentication

---


## 👥 GRUPO

- RM557647 - Nicollas Frei
- RM554921 - Eduardo Eiki
- RM558208 - Heitor Pereira Duarte
  
---

## Estrutura do Projeto
MottuChallenge/

  ├── MottuChallenge.API (Presentation)

  ├── MottuChallenge.Application (Services e DTOs)

  ├── MottuChallenge.Domain (Entidades, Models)

  ├── MottuChallenge.Infrastructure (Contexto, Migrations)


---

### AuthController
- `POST /api/auth/register` → Criação de usuário
- `POST /api/auth/login` → Login e retorno do JWT

### PatioController
- `POST /api/patio` → Criação de pátio
- `GET /api/patio` → Listagem de todos os pátios
- `GET /api/patio/{id}` → Obter pátio por ID
- `PUT /api/patio/{id}` → Atualizar pátio
- `DELETE /api/patio/{id}` → Deletar pátio

###  AreaController
- `POST /api/area` → Criar área
- `GET /api/area` → Listar todas as áreas
- `GET /api/area/{id}` → Obter área por ID
- `PUT /api/area/{id}` → Atualizar área
- `DELETE /api/area/{id}` → Deletar área

---

## ✅ Funcionalidades Implementadas

- [x] CRUD completo para `User`, `Patio` e `Area`
- [x] Autenticação com JWT
- [x] Relacionamento entre Patio → Endereco → Area → User
- [x] Validações com Data Annotations
- [x] Documentação com Swagger
- [x] Clean Architecture aplicada
- [x] Versionamento de banco via EF Migrations

---

## 🧪 Como Executar Localmente

1. Clone o repositório:
```bash
git clone https://github.com/NclFrei/MottuChallenge.API
