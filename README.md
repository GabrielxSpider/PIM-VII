# ConectaLar - Backend

Backend da plataforma **ConectaLar**, desenvolvido para o projeto PIM VII.

A aplicação fornece uma API REST para gerenciamento de usuários, serviços, profissionais, agendamentos, avaliações e funcionalidades relacionadas à diversidade e responsabilidade social.

## Tecnologias

- C#
- ASP.NET Core
- Entity Framework Core
- SQLite
- JWT Authentication
- REST API

## Estrutura

- `Controllers/` — endpoints da API
- `DTOs/` — objetos utilizados nas requisições
- `Data/` — contexto do banco de dados
- `Migrations/` — migrations do Entity Framework
- `Models/` — entidades e enums
- `Program.cs` — configuração da aplicação

## Como executar

1. Clone o repositório.
2. Abra `BackendPIM.slnx` no Visual Studio.
3. Restaure as dependências do projeto.
4. No Package Manager Console, execute:

```powershell
Update-Database
