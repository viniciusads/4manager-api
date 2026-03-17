# 4manager-api

Este projeto é uma API desenvolvida em .NET 9 utilizando as melhores práticas de arquitetura, incluindo:

- **CQRS com MediatR**
- **FluentValidation**
- **Entity Framework Core**
- **PostgreSQL**
- **Migrations**
- Testes com xUnit e NSubstitute _(em andamento)_

---

## Estrutura do Projeto

```
src/
├── 4Manager.API              # Projeto principal (camada de apresentação - controllers, middlewares)
├── 4Manager.Application      # Casos de uso (handlers, comandos, queries, validators)
├── 4Manager.Domain           # Entidades e interfaces
├── 4Manager.Infrastructure   # Serviços externos e integrações (ex: autenticação, email)
├── 4Manager.Persistence      # Acesso a dados (EF Core, Repositórios, DbContext, Migrations)
```

---

## Como rodar o projeto localmente

1. **Pré-requisitos**

   - [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
   - [PostgreSQL](https://www.postgresql.org/)
   - [Visual Studio 2022+ ou VS Code](https://code.visualstudio.com/)

2. **Configure o banco de dados**

Crie um banco no PostgreSQL com o nome desejado (ex: `4managerdb`).

Obs: Você pode criar o banco via docker com o seguinte comando:

```bash
docker run -d \
    --name postgres-4manager \
    -p 5432:5432 \
    -e POSTGRES_USER=postgres \
    -e POSTGRES_PASSWORD=123456 \
    -e POSTGRES_DB=4managerdb \
    postgres:latest
```

Atualize a `ConnectionStrings` no arquivo `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=4managerdb;Username=postgres;Password=123456"
}
```

3. **Rode a migration no banco**

Abra o terminal e execute:

```bash
dotnet ef database update --project 4Manager.Persistence --startup-project 4Manager.API
```

Isso criará as tabelas no seu banco com base nas entidades mapeadas no `DbContext`.

---

## Como rodar o projeto com Docker

1. **Executar o comando `docker compose up --build` na pasta `/4manager-api`**

---

## Padrões e Tecnologias

### CQRS com MediatR

- **Command**: utilizado para criar, atualizar ou deletar dados
- **Query**: utilizado para consultar dados
- Os comandos e queries são processados por `Handlers`.

> Os comandos ficam em `4Manager.Application.Features.[Nome]/Commands`
> As queries ficam em `4Manager.Application.Features.[Nome]/Queries`

### FluentValidation

Validações são feitas nos arquivos `RequestValidator` e são executadas antes dos comandos/queries serem processados.

```csharp
RuleFor(x => x.Nome).NotEmpty().WithMessage("Nome é obrigatório");
```

### 🐘 Entity Framework Core com PostgreSQL

Utilizamos EF Core como ORM e PostgreSQL como banco de dados. As migrations ficam em `4Manager.Persistence/Migrations`.

---

## Como adicionar uma nova funcionalidade

Vamos supor que queremos criar o módulo de `Usuários`.

1. Crie a entidade `Usuario` em `4Manager.Domain/Entities`.
2. Crie os comandos e queries em `4Manager.Application/Features/Usuarios/Commands` e `Queries`.
3. Crie os `Handlers` (que implementam `IRequestHandler<,>`) para processar a lógica.
4. Crie os `Validators` usando FluentValidation.
5. Crie o `Repository` que implementa a interface em `Domain`.
6. Crie um `Controller` na API e injete o `IMediator` para enviar comandos e queries.
7. Adicione novos testes se necessário.

---

## Testes

_(em breve)_ Testes unitários serão escritos usando:

- **NSubstitute**

---

## Comandos úteis

```bash
# Criar uma nova migration
dotnet ef migrations add NomeDaMigration --project 4Manager.Persistence --startup-project 4Manager.API

# Aplicar as migrations no banco
dotnet ef database update --project 4Manager.Persistence --startup-project 4Manager.API
```

---

## 👥 Contribuindo

Este projeto segue boas práticas e Clean Architecture. Siga os padrões existentes para adicionar novos módulos.

---

## Suporte

Se tiver dúvidas:

- Consulte a pasta `/Application/Features` para entender os padrões
