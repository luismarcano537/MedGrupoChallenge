# MedgrupoChallenge - Contacts API

API REST desenvolvida em .NET para gerenciamento de contatos, como parte de um desafio técnico para a vaga de Desenvolvedor Backend .NET Core Júnior.

O sistema permite criar, listar, visualizar, editar, desativar e excluir contatos, respeitando regras de negócio relacionadas à maioridade, data de nascimento e status ativo do contato.

---

## Tecnologias utilizadas

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Docker
- xUnit
- Moq
- FluentAssertions
- Swagger / OpenAPI

---

## Estrutura da solução

```txt
MedgrupoChallenge
├── src
│   ├── MedgrupoChallenge.Api
│   │   ├── Controllers
│   │   ├── Properties
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   │
│   ├── MedgrupoChallenge.Application
│   │   ├── DTOs
│   │   ├── Interfaces
│   │   └── Services
│   │
│   ├── MedgrupoChallenge.Domain
│   │   ├── Entities
│   │   └── Enums
│   │
│   └── MedgrupoChallenge.Infrastructure
│       ├── Data
│       │   └── Mappings
│       ├── Repositories
│       └── Migrations
│
├── tests
│   └── MedgrupoChallenge.Tests
│       ├── Domain
│       │   └── Entities
│       └── Application
│           └── Services
│
└── MedgrupoChallenge.slnx
```

---

## Organização em camadas

A solution foi separada em projetos para deixar as responsabilidades mais claras.

| Projeto | Responsabilidade |
|---|---|
| `MedgrupoChallenge.Api` | Controllers, configuração da aplicação, Swagger e injeção de dependência |
| `MedgrupoChallenge.Application` | DTOs, interfaces e serviços de aplicação |
| `MedgrupoChallenge.Domain` | Entidades, enums e regras centrais de negócio |
| `MedgrupoChallenge.Infrastructure` | DbContext, mapeamentos, migrations e repositories |
| `MedgrupoChallenge.Tests` | Testes automatizados de domínio e aplicação |

Essa estrutura evita concentrar regras de negócio nos controllers e deixa mais claro o limite entre API, aplicação, domínio e infraestrutura.

---

## Funcionalidades

- Criar contato
- Listar contatos ativos
- Visualizar detalhes de um contato ativo
- Editar contato ativo
- Desativar contato
- Excluir contato
- Calcular idade em tempo de execução
- Validar maioridade
- Validar data de nascimento
- Validar gênero
- Executar testes automatizados

---

## Regras de negócio

- O contato deve possuir nome, data de nascimento e gênero.
- A idade é calculada em tempo de execução.
- A idade não é armazenada no banco de dados.
- O contato deve ser maior de idade.
- A idade não pode ser igual a zero.
- A data de nascimento não pode ser maior que a data atual.
- Todo contato criado inicia como ativo.
- A listagem considera apenas contatos ativos.
- A visualização considera apenas contatos ativos.
- A edição considera apenas contatos ativos.
- A desativação altera o status do contato para inativo.
- A exclusão remove o contato do banco de dados.

---

## Endpoints

### Criar contato

```http
POST /api/contacts
```

Exemplo de request:

```json
{
  "name": "Luis Marcano",
  "birthDate": "2000-01-01",
  "gender": 1
}
```

Exemplo de response:

```json
{
  "id": "de9beea6-18b9-4a6f-8c5e-3f4e9790d888",
  "name": "Luis Marcano",
  "birthDate": "2000-01-01T00:00:00",
  "gender": 1,
  "age": 26,
  "isActive": true
}
```

---

### Listar contatos ativos

```http
GET /api/contacts
```

Essa operação retorna apenas contatos ativos.

---

### Visualizar contato por ID

```http
GET /api/contacts/{id}
```

Apenas contatos ativos são retornados.

---

### Editar contato

```http
PUT /api/contacts/{id}
```

Exemplo de request:

```json
{
  "name": "Luis Marcano Atualizado",
  "birthDate": "2000-01-01",
  "gender": 1
}
```

Apenas contatos ativos podem ser editados.

---

### Desativar contato

```http
PATCH /api/contacts/{id}/deactivate
```

Essa operação altera o contato para inativo.

---

### Excluir contato

```http
DELETE /api/contacts/{id}
```

Essa operação remove o contato do banco de dados.

---

## Gêneros disponíveis

| Valor | Descrição |
|------:|-----------|
| 1 | Male |
| 2 | Female |
| 3 | Other |

---

## Configuração do ambiente

A aplicação utiliza `appsettings.json` e `appsettings.Development.json` para configuração da connection string.

Exemplo de configuração:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1434;Database=MedgrupoContactsDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  }
}
```

A connection string é lida no `Program.cs` através de:

```csharp
builder.Configuration.GetConnectionString("DefaultConnection");
```

Observação: em ambiente real, credenciais sensíveis não devem ser versionadas diretamente. O ideal seria utilizar User Secrets, variáveis de ambiente ou um Secret Manager.

---

## Banco de dados com Docker

Para subir uma instância do SQL Server 2022 via Docker, execute:

```powershell
docker run -e "ACCEPT_EULA=Y" `
  -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" `
  -p 1434:1433 `
  --name sqlserver-medgrupo `
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Neste exemplo, a porta local `1434` aponta para a porta `1433` dentro do container.

Comandos úteis:

```powershell
docker ps
```

```powershell
docker start sqlserver-medgrupo
```

```powershell
docker stop sqlserver-medgrupo
```

---

## Executando migrations

Como o `DbContext` está no projeto `Infrastructure` e a aplicação inicializável está no projeto `Api`, utilize o comando abaixo a partir da raiz da solution:

```powershell
dotnet ef database update `
  --project .\src\MedgrupoChallenge.Infrastructure\MedgrupoChallenge.Infrastructure.csproj `
  --startup-project .\src\MedgrupoChallenge.Api\MedgrupoChallenge.Api.csproj
```

Para criar uma nova migration:

```powershell
dotnet ef migrations add NomeDaMigration `
  --project .\src\MedgrupoChallenge.Infrastructure\MedgrupoChallenge.Infrastructure.csproj `
  --startup-project .\src\MedgrupoChallenge.Api\MedgrupoChallenge.Api.csproj
```

---

## Executando a aplicação

A partir da raiz da solution:

```powershell
cd .\src\MedgrupoChallenge.Api
dotnet run
```

A API ficará disponível conforme a porta informada no terminal.

O Swagger pode ser acessado em:

```txt
https://localhost:{porta}/swagger
```

ou:

```txt
http://localhost:{porta}/swagger
```

---

## Executando os testes

A partir da raiz da solution:

```powershell
dotnet test .\MedgrupoChallenge.slnx
```

Os testes cobrem:

- regras de domínio da entidade `Contact`;
- criação de contatos válidos;
- validação de nome;
- validação de data futura;
- validação de maioridade;
- validação de idade igual a zero;
- validação de gênero inválido;
- cálculo de idade;
- atualização de contato;
- desativação de contato;
- comportamento do `ContactService`;
- chamadas esperadas ao repository usando mocks.

---

## Decisões técnicas

### Separação em projetos

Inicialmente, as camadas poderiam ser organizadas apenas por pastas dentro de um único projeto. Nesta versão, a solution foi separada em projetos para deixar mais claros os limites entre as responsabilidades.

A API depende de Application e Infrastructure. A Application depende do Domain. A Infrastructure implementa os contratos definidos na Application e utiliza o Domain para persistência das entidades.

Essa organização não tem a intenção de ser uma Clean Architecture completa, mas sim uma separação em camadas por projetos, adequada ao escopo do desafio.

---

### Regras de domínio na entidade

A entidade `Contact` concentra regras importantes do domínio, como:

- nome obrigatório;
- data de nascimento válida;
- maioridade;
- idade diferente de zero;
- controle de status ativo/inativo.

Isso evita que um contato seja criado em estado inválido por diferentes pontos da aplicação.

---

### Idade calculada em tempo de execução

A idade do contato não é persistida no banco. Ela é calculada pela entidade `Contact` com base na data de nascimento.

Isso evita inconsistência de dados, já que a idade muda com o tempo.

---

### Mapeamento separado

O mapeamento da entidade `Contact` foi separado em uma classe própria usando `IEntityTypeConfiguration<Contact>`.

Isso evita que o `AppDbContext` cresça demais conforme novas entidades sejam adicionadas.

O `AppDbContext` aplica os mappings através de:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
```

Dessa forma, novos mappings criados no mesmo assembly são aplicados automaticamente.

---

### Contatos ativos

O contato possui um status ativo.

As operações de listagem, visualização e edição consideram apenas contatos ativos, conforme solicitado no desafio.

A desativação altera o status do contato para inativo. A exclusão remove o registro do banco de dados.

---

### Testes automatizados

Foram criados testes unitários para validar as regras de domínio e o comportamento da camada de aplicação.

Os testes de domínio validam a entidade `Contact` isoladamente, sem dependência de banco de dados, API ou repository.

Os testes do `ContactService` utilizam mocks do repository para evitar dependência direta com banco de dados e validar o fluxo da aplicação.

---

## Comandos úteis

Build da solution:

```powershell
dotnet build .\MedgrupoChallenge.slnx
```

Executar testes:

```powershell
dotnet test .\MedgrupoChallenge.slnx
```

Executar API:

```powershell
cd .\src\MedgrupoChallenge.Api
dotnet run
```

Aplicar migrations:

```powershell
dotnet ef database update `
  --project .\src\MedgrupoChallenge.Infrastructure\MedgrupoChallenge.Infrastructure.csproj `
  --startup-project .\src\MedgrupoChallenge.Api\MedgrupoChallenge.Api.csproj
```

Criar migration:

```powershell
dotnet ef migrations add NomeDaMigration `
  --project .\src\MedgrupoChallenge.Infrastructure\MedgrupoChallenge.Infrastructure.csproj `
  --startup-project .\src\MedgrupoChallenge.Api\MedgrupoChallenge.Api.csproj
```

---

## Status do projeto

Projeto funcional com:

- API REST implementada
- Persistência em SQL Server
- Entity Framework Core configurado
- Migrations configuradas
- Mapeamento separado por entidade
- Regras de negócio implementadas no domínio
- Solution organizada em projetos por camada
- Testes automatizados implementados
- Swagger disponível para validação dos endpoints