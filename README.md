# Sales & Users API with Gateway and RabbitMQ

A microservices-based architecture built with:

- **.NET 9** (Minimal APIs)  
- **YARP** (API Gateway)  
- **RabbitMQ** (messaging)  
- **PostgreSQL** (independent databases per service)  
- **Docker Compose** (orchestration)  

---

## 🚀 Services

- **Gateway** → [`http://localhost:5000`](http://localhost:5000)  
  Single entry point. Routes requests to backend services via YARP.  

- **Users API** → [`http://localhost:5012`](http://localhost:5012)  
  User management, authentication, and JWT generation.  

- **Sales API** → [`http://localhost:5013`](http://localhost:5013)  
  Sales management and event publishing to RabbitMQ.  

- **RabbitMQ**  
  - Broker: `amqp://localhost:5672`  
  - Dashboard: [`http://localhost:15672`](http://localhost:15672)  
  - Default credentials: `guest` / `guest`  

---

## 🛠️ Requirements

- [Docker](https://docs.docker.com/get-docker/)  
- [Docker Compose](https://docs.docker.com/compose/)  
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download) *(optional, to run outside containers)*  

---

## ▶️ Getting Started

Clone the repository and run:  

```bash
docker compose build
docker compose up -d
