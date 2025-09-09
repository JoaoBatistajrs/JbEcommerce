# Sales & Users API com Gateway e RabbitMQ

Este projeto implementa uma arquitetura baseada em **microservices** utilizando:

- **.NET 9** (Minimal APIs)  
- **YARP (Gateway)**  
- **RabbitMQ** (mensageria)  
- **PostgreSQL** (bancos independentes para cada serviço)  
- **Docker Compose** (orquestração dos serviços)

---
## 🚀 Serviços

- **Gateway** (`http://localhost:5000`)  
  O Gateway é a única porta de entrada (porta 5000).

  API Gateway com YARP para rotear requisições para as APIs.  

- **UsersApi** (`http://localhost:5012`)  
  O UsersApi gerencia usuários e autenticação (porta 5012).

  Gerenciamento de usuários, autenticação e geração de JWT.  

- **SalesAPI** (`http://localhost:5013`)  
  O SalesAPI gerencia vendas e publica eventos no RabbitMQ (porta 5013).

  Gerenciamento de vendas, publica eventos no RabbitMQ.  

- **Postgres**  
  Cada API tem seu próprio banco no Postgres.

  - `usersdb` (porta 5432 interna, exposta em 5432 no host)  
  - `salesdb` (porta 5432 interna, exposta em 5433 no host)  

- **RabbitMQ**  

  RabbitMQ gerencia a comunicação assíncrona entre serviços.

  - Broker de mensagens (`amqp://localhost:5672`)  
  - Painel de administração (`http://localhost:15672`)  
  - Usuário padrão: `guest` / Senha: `guest`  
  
---

## 🛠️ Pré-requisitos

- [Docker](https://docs.docker.com/get-docker/)  
- [Docker Compose](https://docs.docker.com/compose/)  
- .NET 9 SDK (se quiser rodar fora do container)

---

## ▶️ Como rodar

Na raiz do repositório:

```bash
docker compose build
docker compose up -d