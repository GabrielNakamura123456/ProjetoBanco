# ProjetoBanco API

## 1. Integrantes

- Gabriel Nakamura Ogata — RM560671
- Guilherme Costeira Braganholo — RM560628
- Julio Cesar Dias Vilella — RM560494

---

# 2. Produto bancário escolhido e justificativa

Os produtos bancários escolhidos foram:

- Empréstimo Pessoal
- Máquina de Cartão

A escolha foi feita porque esses produtos permitem simular regras reais de análise bancária, como validação de score de crédito, análise de faturamento e aprovação automática de contratações.

O projeto simula um fluxo bancário assíncrono utilizando mensageria com RabbitMQ.

---

# 3. Decisão de modelagem de filas

Foi utilizada uma única fila RabbitMQ:

```text
contratacao-solicitada
```

Fluxo da aplicação:

1. A API recebe a solicitação de contratação.
2. O Producer publica a mensagem na fila RabbitMQ.
3. O Consumer processa a contratação em background.
4. O status da contratação é atualizado automaticamente.
5. O Consumer realiza ACK manual da mensagem.

A escolha de apenas uma fila foi realizada devido à simplicidade do domínio e centralização do processamento das contratações.

---

# 4. Diagrama de Classes

Adicionar o diagrama na pasta `docs`.

Exemplo:

```md
![Diagrama](docs/diagrama-classes.png)
```

---

# 5. Como rodar localmente

## Pré-requisitos

- .NET 8 SDK
- Visual Studio 2022
- Docker Desktop
- Oracle Database

---

## Clonar o projeto

```bash
git clone https://github.com/GabrielNakamura123456/ProjetoBanco.git
```

---

## Executar RabbitMQ e Jaeger

```bash
docker compose up -d
```

RabbitMQ:

```text
http://localhost:15672
```

Usuário:

```text
guest
```

Senha:

```text
guest
```

Jaeger:

```text
http://localhost:16686
```

---

## Executar migrations Oracle

```bash
dotnet ef database update --project ProjetoBanco.Api
```

---

## Rodar aplicação

```bash
dotnet run --project ProjetoBanco.Api
```

Swagger:

```text
https://localhost:7170/swagger
```

Health:

```text
https://localhost:7170/health
```

---

# 6. Endpoints disponíveis

## Criar agência

```http
POST /api/agencias
```

Request:

```json
{
  "numero": "0001",
  "nome": "Agencia Central"
}
```

---

## Buscar agência

```http
GET /api/agencias/{id}
```

---

## Criar pessoa física

```http
POST /api/clientes/pf
```

Request:

```json
{
  "nome": "Gabriel Nakamura",
  "cpf": "99988877766",
  "dataNascimento": "2004-01-01T00:00:00",
  "agenciaId": 1
}
```

---

## Criar pessoa jurídica

```http
POST /api/clientes/pj
```

Request:

```json
{
  "nome": "Empresa XPTO",
  "cnpj": "12345678000199",
  "razaoSocial": "Empresa XPTO LTDA",
  "agenciaId": 1
}
```

---

## Buscar cliente

```http
GET /api/clientes/{id}
```

---

## Solicitar contratação

```http
POST /api/contratacoes
```

Request:

```json
{
  "clienteId": 1,
  "produtoId": 1,
  "valorSolicitado": 10000,
  "scoreCredito": 750,
  "faturamentoMensal": 5000
}
```

---

## Consultar contratação

```http
GET /api/contratacoes/{id}
```

Exemplo de resposta:

```json
{
  "id": 1,
  "status": 1,
  "valorSolicitado": 10000,
  "processadoEm": "2026-05-07T..."
}
```

---

## Health Check

```http
GET /health
```

Resposta:

```text
Healthy
```

---

# 7. Como executar os testes

Executar:

```bash
dotnet test
```

Adicionar print do terminal com os testes executados:

![Testes](docs/testes.png)

---

# 8. Print do painel RabbitMQ

Print do RabbitMQ mostrando a fila `contratacao-solicitada`:

![RabbitMQ](docs/rabbitmq.png)

---

# 9. Print da API rodando no Swagger

Print do Swagger mostrando uma contratação aprovada:

![Swagger](docs/swagger.png)

---

# Prints adicionais

## Health Check

![Health](docs/health.png)

---

## Jaeger

![Jaeger](docs/jaeger.png)

---

# Observabilidade

O projeto utiliza:

- Serilog
- OpenTelemetry
- Jaeger

O Jaeger recebe traces das chamadas HTTP realizadas na API, permitindo visualizar:

- POST /api/agencias
- POST /api/clientes/pf
- POST /api/contratacoes
- GET /api/contratacoes/{id}

---

# Tecnologias utilizadas

- ASP.NET Core 8
- Entity Framework Core
- Oracle Database
- RabbitMQ
- Docker
- Swagger
- Serilog
- OpenTelemetry
- Jaeger
- xUnit