# LoansPlatform API 💳

API RESTful desarrollada en **.NET 8 / C#** para la gestión de préstamos financieros.  
Permite registrar usuarios, crear préstamos, actualizar su estado y consultar las obligaciones financieras diferenciando entre activas y saldadas.  

Además, se integra con **PostgreSQL** como base de datos principal y **AWS DynamoDB** como sistema de caché distribuido.

## 🚀 Características principales

### Gestión de usuarios
- Registro con email y contraseña cifrada con **BCrypt**.  
- Validación de duplicados por correo.  
- Eliminación de usuarios (siempre que no tengan préstamos asociados).  

### Gestión de préstamos
- CRUD completo de préstamos.  
- Actualización de estado **Pending → Paid**.  
- Validaciones de negocio:  
  - El monto debe ser positivo.  
  - Un usuario no puede tener más de un préstamo pendiente.  
  - Los préstamos pagados son inmutables.  

### Persistencia
- Base de datos principal: **PostgreSQL (EF Core)**.  
- Caché distribuida: **AWS DynamoDB** para acelerar consultas.  

### Pruebas unitarias
- Capa de servicios cubierta con **xUnit + FluentAssertions + Moq**.  

## 🛠️ Tecnologías utilizadas

- **.NET 8 / C#**
- **Entity Framework Core** (PostgreSQL)  
- **AutoMapper** (mapeo entre entidades y DTOs)  
- **BCrypt.Net** (hashing de contraseñas)  
- **AWS SDK for .NET (DynamoDBv2)**  
- **xUnit, Moq, FluentAssertions** (pruebas unitarias)  


## 📂 Arquitectura aplicada

El proyecto sigue una **arquitectura en capas limpia**:

- **Domain**  
  Contiene las entidades (`User`, `Loan`), enumeraciones y contratos (interfaces de repositorios y cachés).  

- **Application**  
  Lógica de negocio (services), DTOs, validaciones y excepciones de negocio personalizadas.  

- **Infrastructure**  
  Implementaciones concretas (repositorios EF Core, integración con DynamoDB).  

- **WebApi**  
  Endpoints expuestos vía controladores (`UsersController`, `LoansController`) y configuración del proyecto.  

- **Tests**  
  Proyecto independiente con pruebas unitarias organizadas por servicios.  



## 📊 Flujo de operación

1. El cliente hace una request al **Controller**.  
2. El controller invoca al **Service** correspondiente.  
3. El service:  
   - Verifica en **DynamoDB (cache)** si el dato ya existe.  
   - Si hay **Cache Hit**, devuelve el valor.  
   - Si hay **Cache Miss**, consulta el **Repository** en PostgreSQL, aplica reglas de negocio y guarda el resultado en cache.  
4. Se devuelven los datos en formato **DTO** al cliente.  


## 🔑 Principios aplicados

- **Single Responsibility Principle (SRP):** cada capa y clase tiene una única responsabilidad (repositorio, servicio, DTOs, etc.).  
- **Dependency Injection (DI):** los servicios, repositorios y caches se inyectan en tiempo de ejecución.  
- **Separation of Concerns:** la lógica de negocio no depende de detalles de infraestructura.  
- **Validation & Business Rules:** centralizadas en la capa de servicios.
- 
## ⚡ Instalación y ejecución

### Clonar el repositorio
```bash
git clone https://github.com/tu-usuario/LoansPlatform.git
cd LoansPlatform


### Configurar la base de datos PostgreSQL
Editar el archivo appsettings.json con tus credenciales locales:
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=loansdb;Username=postgres;Password=yourpassword"
}

### Ejecutar las migraciones
dotnet ef database update --project LoansPlatform.Infrastructure --startup-project LoansPlatform.WebApi

### Configurar AWS DynamoDB

Crear las tablas en DynamoDB:

UsersCache → clave de partición Id (String)
LoansCache → clave de partición Id (String)

en appsettings.json
"AWS": {
  "Region": "us-east-2",
  "AccessKey": "YOUR_AWS_ACCESS_KEY",
  "SecretKey": "YOUR_AWS_SECRET_KEY"
}

###Ejecutar la API

dotnet run --project LoansPlatform.WebApi

###Pruebas Unitarias

dotnet test





