# OpMetrics

API REST para registro e consulta de indicadores operacionais de produção industrial, como OEE, volume produzido e índice de qualidade.

Desenvolvida em **C# com ASP.NET Core 8** e **PostgreSQL**, com arquitetura em camadas, validações, mapeamento automático e documentação interativa via Swagger.

---

## Funcionalidades

- Registro e consulta de indicadores de **Produção**, **Qualidade** e **OEE** por linha e turno
- Cálculo automático de métricas derivadas (percentual atingido, disponibilidade, performance, índice de qualidade e OEE final)
- CRUD completo para cada indicador
- Validações de entrada com FluentValidation
- Documentação interativa acessível via Swagger UI

---

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Framework | ASP.NET Core 8 |
| Banco de dados | PostgreSQL |
| ORM | Entity Framework Core 8 + Npgsql |
| Mapeamento | AutoMapper 12 |
| Validação | FluentValidation 11 |
| Documentação | Swagger (Swashbuckle) |

---

## Arquitetura

O projeto segue uma arquitetura em camadas com responsabilidades bem definidas:

```
Controllers  →  Services  →  Repositories  →  Database
                    ↕
                 Entities / DTOs / Validators
```

- **Controllers** — recebem as requisições HTTP e devolvem as respostas
- **Services** — contêm a lógica de negócio
- **Repositories** — única camada que acessa o banco de dados
- **DTOs** — objetos separados para entrada (Request) e saída (Response)
- **Validators** — validação das requisições antes de chegar na lógica de negócio
- **Mappings** — configuração do AutoMapper entre entidades e DTOs

---

## Endpoints

### Produção — `/api/producao`

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/producao` | Lista todos os registros |
| GET | `/api/producao/{id}` | Busca por ID |
| GET | `/api/producao/linha/{linha}` | Busca por linha de produção |
| POST | `/api/producao` | Cria um novo registro |
| PUT | `/api/producao/{id}` | Atualiza um registro |
| DELETE | `/api/producao/{id}` | Remove um registro |

### Qualidade — `/api/qualidade`

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/qualidade` | Lista todos os registros |
| GET | `/api/qualidade/{id}` | Busca por ID |
| GET | `/api/qualidade/linha/{linha}` | Busca por linha de produção |
| POST | `/api/qualidade` | Cria um novo registro |
| PUT | `/api/qualidade/{id}` | Atualiza um registro |
| DELETE | `/api/qualidade/{id}` | Remove um registro |

### OEE — `/api/oee`

| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/oee` | Lista todos os registros |
| GET | `/api/oee/{id}` | Busca por ID |
| GET | `/api/oee/linha/{linha}` | Busca por linha de produção |
| POST | `/api/oee` | Cria um novo registro |
| PUT | `/api/oee/{id}` | Atualiza um registro |
| DELETE | `/api/oee/{id}` | Remove um registro |

---

## Métricas calculadas automaticamente

### Produção
- **Percentual Atingido** = (Peças Produzidas / Meta de Peças) × 100

### OEE
- **Disponibilidade** = (Tempo Rodando / Tempo Planejado) × 100
- **Performance** = (Peças Produzidas / Capacidade Ideal) × 100
- **Índice de Qualidade** = (Peças Boas / Peças Produzidas) × 100
- **OEE Final** = (Disponibilidade × Performance × Índice de Qualidade) / 100²

---

## Como rodar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)

### 1. Clone o repositório

```bash
git clone https://github.com/barbozaawill/OpMetrics.git
cd OpMetrics
```

### 2. Configure a string de conexão

No arquivo `appsettings.json`, ajuste os dados do seu PostgreSQL:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=OpMetricsDb;Username=postgres;Password=sua_senha"
}
```

### 3. Aplique as migrations

```bash
dotnet ef database update
```

### 4. Rode a aplicação

```bash
dotnet run
```

### 5. Acesse o Swagger

```
https://localhost:{porta}/swagger
```

A porta é exibida no terminal ao iniciar a aplicação.

---

## Estrutura do projeto

```
OpMetrics.Core/
├── Controllers/         # Endpoints da API
├── DTOs/
│   ├── Requests/        # Objetos de entrada
│   └── Responses/       # Objetos de saída
├── Data/                # DbContext
├── Entities/            # Modelos do banco de dados
├── Mappings/            # Perfis do AutoMapper
├── Migrations/          # Migrations do EF Core
├── Repositories/        # Acesso ao banco de dados
│   └── Interfaces/
├── Services/            # Lógica de negócio
│   └── Interfaces/
└── Validators/          # Validações de entrada
```
