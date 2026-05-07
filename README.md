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
│   └── MedgrupoChallenge
│       ├── Application
│       │   ├── DTOs
│       │   ├── Interfaces
│       │   └── Services
│       ├── Controllers
│       ├── Domain
│       │   ├── Entities
│       │   └── Enums
│       ├── Infrastructure
│       │   ├── Data
│       │   └── Repositories
│       ├── Migrations
│       ├── Program.cs
│       └── MedgrupoChallenge.csproj
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

## Endpoints

### Criar contato
```
POST /api/contacts
```
Exemplo de request:

```
{
  "name": "Luis Marcano",
  "birthDate": "2000-01-01",
  "gender": 1
}
```

### Listar contatos ativos
```
GET /api/contacts
```

### Visualizar contato por ID
```
GET /api/contacts/{id}
```
Apenas contatos ativos são retornados.

### Editar contato
```
PUT /api/contacts/{id}
```
Exemplo de request:

```
{
  "name": "Luis Marcano Atualizado",
  "birthDate": "2000-01-01",
  "gender": 1
}
```
Apenas contatos ativos podem ser editados.

### Desativar contato
```
PATCH /api/contacts/{id}/deactivate
```
Essa operação altera o contato para inativo.

### Excluir contato
```
DELETE /api/contacts/{id}
```
Essa operação remove o contato do banco de dados.

## Gêneros disponíveis

| Valor | Descrição |
| :--- | :--- |
| 1 | Male |
| 2 | Female |
| 3 | Other |

---

## Configuração do ambiente

A aplicação utiliza variáveis de ambiente gerenciadas através de um arquivo `.env`.

1. Crie um arquivo `.env` dentro da pasta: `src/MedgrupoChallenge/`
2. Utilize o seguinte modelo:

```env
DB_SERVER=localhost
DB_PORT=1433
DB_NAME=MedgrupoContactsDb
DB_USER=sa
DB_PASSWORD=YourStrong@Passw0rd
DB_TRUST_CERTIFICATE=True
```

Atenção: O arquivo .env contém credenciais sensíveis e não deve ser versionado no Git.

## Banco de dados com Docker

Para subir uma instância do SQL Server 2022 via Docker, execute:
```
docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
  -p 1433:1433 \
  --name sqlserver-medgrupo \
  -d [mcr.microsoft.com/mssql/server:2022-latest](https://mcr.microsoft.com/mssql/server:2022-latest)
```
Comandos úteis para o container:
```
Verifica status: docker ps
Iniciar container parado: docker start sqlserver-medgrupo
```

## Executando migrations
A partir da raiz da solution:
```
dotnet ef database update --project .\src\MedgrupoChallenge\MedgrupoChallenge.csproj
```
Caso precise criar uma nova migration:
```
dotnet ef migrations add NomeDaMigration --project .\src\MedgrupoChallenge\MedgrupoChallenge.csproj
```

## Executando a aplicação
A partir da raiz da solution:
```
cd .\src\MedgrupoChallenge
dotnet run
```
A API ficará disponível conforme a porta informada no terminal.

O Swagger pode ser acessado em:
```https://localhost:{porta}/swagger``` ou ```http://localhost:{porta}/swagger``` 

## Executando os testes
A partir da raiz da solution:
```
dotnet test .\MedgrupoChallenge.slnx
```
Os testes cobrem:

- regras de domínio da entidade Contact;
- criação de contatos válidos;
- validação de nome;
- validação de data futura;
- validação de maioridade;
- validação de idade igual a zero;
- validação de gênero inválido;
- cálculo de idade;
- atualização de contato;
- desativação de contato;
- comportamento do ContactService;
- chamadas esperadas ao repository usando mocks.

## Decisões técnicas
**Separação em camadas**

O projeto foi organizado separando responsabilidades em:

- Domain: entidades e regras centrais do domínio.
- Application: DTOs, interfaces e serviços de aplicação.
- Infrastructure: acesso a dados, DbContext e repositories.
- Controllers: exposição dos endpoints REST.

Essa organização evita que regras de negócio fiquem concentradas nos controllers.

## Idade calculada em tempo de execução

A idade do contato não é persistida no banco. Ela é calculada pela entidade Contact com base na data de nascimento.

Isso evita inconsistência de dados, já que a idade muda com o tempo.

## Contatos ativos

O contato possui um status ativo.

As operações de listagem, visualização e edição consideram apenas contatos ativos, conforme solicitado no desafio.

## Testes automatizados

Foram criados testes unitários para validar as regras de domínio e o comportamento da camada de serviço.

Os testes do ```ContactService``` utilizam mocks do repository para evitar dependência direta com banco de dados.

## Comandos úteis

Build da solution:

```
dotnet build .\MedgrupoChallenge.slnx
```
Executar testes:
```
dotnet test .\MedgrupoChallenge.slnx
```
Executar API:
```
cd .\src\MedgrupoChallenge
dotnet run
```
Aplicar migrations:
```
dotnet ef database update --project .\src\MedgrupoChallenge\MedgrupoChallenge.csproj
```

## Status do projeto
**Projeto funcional com:**

- API REST implementada
- Persistência em SQL Server
- Entity Framework Core configurado
- Migrations configuradas
- Regras de negócio implementadas
- Testes automatizados implementados
- Swagger disponível para validação dos endpoints