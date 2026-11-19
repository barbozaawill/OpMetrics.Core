# OpMetrics

API REST para registro e consulta de indicadores operacionais de produção industrial, como OEE, volume produzido e índice de qualidade.

Desenvolvida em **C# com ASP.NET Core 8 (Minimal APIs)** e **PostgreSQL**, organizada em **Vertical Slice Architecture**, com validações, testes automatizados e documentação interativa via Swagger.

---

## Funcionalidades

- Registro e consulta de indicadores de **Produção**, **Qualidade** e **OEE** por linha e turno
- Cálculo automático de métricas derivadas (percentual atingido, disponibilidade, performance, índice de qualidade e OEE final)
- CRUD completo para cada indicador
- Validações de entrada com FluentValidation, com erros por campo (`400 Bad Request`)
- Documentação interativa acessível via Swagger UI

---

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Framework | ASP.NET Core 8 (Minimal APIs) |
| Banco de dados | PostgreSQL |
| ORM | Entity Framework Core 8 + Npgsql |
| Validação | FluentValidation 11 |
| Documentação | Swagger (Swashbuckle) |
| Testes | xUnit + EF Core InMemory |

---

## Arquitetura: Vertical Slice

O código é organizado **por funcionalidade**, e não por camada técnica. Cada operação da API (registrar um OEE, listar produções, atualizar uma inspeção de qualidade...) é uma **fatia** (*slice*) que contém tudo o que ela precisa, num arquivo só:

```csharp
public static class RegistrarOee
{
    public record Request(...);                      // o que chega na requisição
    public class Validator : AbstractValidator<...>  // as regras de validação
    public class Handler(AppDbContext db) { ... }    // a lógica da operação
    public static void Map(RouteGroupBuilder grupo)  // a rota HTTP
}
```

Para entender ou alterar uma funcionalidade, basta abrir um arquivo. Uma mudança numa fatia não afeta as outras.

### Decisões de projeto

- **Um arquivo por operação** em `Features/<Entidade>/`. Cada entidade também tem:
  - `<Entidade>Regras.cs`: interface de entrada e regras de validação compartilhadas entre registrar e atualizar;
  - `<Entidade>Resposta.cs`: formato da resposta, com o mapeamento a partir da entidade;
  - `<Entidade>Endpoints.cs`: registro dos handlers (`AddOee()`) e das rotas (`MapOee()`).
- **Validação por filtro de endpoint** (`Common/ValidationFilter.cs`): roda antes do handler e responde `400` com os erros por campo. Cada endpoint ativa com `.ComValidacao<Request>()`.
- **`DbContext` direto nos handlers, sem repository**: o `DbContext` do EF Core já faz o papel de repositório (`DbSet`) e de unidade de trabalho (`SaveChangesAsync`). Cada handler faz só a consulta de que precisa. Consultas de leitura usam `AsNoTracking()`.
- **Mapeamento manual** (`OeeResposta.De(oee)`) no lugar do AutoMapper: o que sai na resposta fica explícito, e qualquer mudança de nome é pega pelo compilador.

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
git clone https://github.com/barbozaawill/OpMetrics.Core.git
cd OpMetrics.Core
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

## Testes

O projeto `OpMetrics.Tests` (xUnit) cobre:

- **Entidades:** Disponibilidade, Performance, Índice de Qualidade e OEE final, incluindo casos de divisão por zero.
- **Validators:** regras de entrada (turno, data futura, peças boas ≤ produzidas, defeitos + rejeitos ≤ inspecionado).
- **Handlers dos slices:** cada operação testada contra um banco em memória (EF Core InMemory), incluindo os casos de registro não encontrado.

```bash
dotnet test
```

---

## Estrutura do projeto

```
OpMetrics.Core/
├── Common/
│   └── ValidationFilter.cs      # Validação dos endpoints com FluentValidation
├── Data/                        # DbContext
├── Entities/                    # Modelos do banco de dados e cálculos das métricas
├── Features/                    # Vertical slices
│   ├── Oees/
│   │   ├── RegistrarOee.cs
│   │   ├── ObterOeePorId.cs
│   │   ├── ListarOees.cs
│   │   ├── ListarOeesPorLinha.cs
│   │   ├── AtualizarOee.cs
│   │   ├── RemoverOee.cs
│   │   ├── OeeRegras.cs
│   │   ├── OeeResposta.cs
│   │   └── OeeEndpoints.cs
│   ├── Producoes/               # Mesma estrutura
│   └── Qualidades/              # Mesma estrutura
├── Migrations/                  # Migrations do EF Core
├── Program.cs
└── OpMetrics.Tests/             # Testes (xUnit)
    ├── Entities/
    ├── Features/
    └── Validators/
```
