# 🏥 Clínica San Salud - API RESTful (.NET 9)

Bienvenido al repositorio del proyecto **Clínica San Salud API**. Esta es una aplicación de referencia desarrollada en **.NET 9 (C#)** pensada con fines educativos y profesionales para estudiantes de **Prácticas Profesionalizantes I** y materias afines a la Ingeniería de Software.

El sistema implementa una **API RESTful** para la gestión integral de turnos médicos, médicos y pacientes, aplicando buenas prácticas de arquitectura, desacoplamiento y diseño de software.

---

## 📑 Tabla de Contenidos
1. [¿Qué hace la aplicación?](#-1-qué-hace-la-aplicación)
2. [Arquitectura y Estructura del Proyecto](#-2-arquitectura-y-estructura-del-proyecto)
3. [Requisitos Previos y Cómo Ejecutar](#-3-requisitos-previos-y-cómo-ejecutar)
4. [Documentación de API y Contratos (Scalar & OpenAPI)](#-4-documentación-de-api-y-contratos-scalar--openapi)
5. [Colección de Bruno para Pruebas (QA / Estudiantes)](#-5-colección-de-bruno-para-pruebas-qa--estudiantes)
6. [Conceptos Clave para el Aprendizaje](#-6-conceptos-clave-para-el-aprendizaje)

---

## 🩺 1. ¿Qué hace la aplicación?

La **API de Clínica San Salud** permite realizar operaciones CRUD (Crear, Leer, Actualizar y Eliminar) sobre tres recursos clave del dominio clínico:

- **Médicos**: Registro de profesionales de la salud con su nombre, especialidad, matrícula, email y teléfono.
- **Pacientes**: Registro de pacientes con su nombre, DNI, obra social/cobertura y fecha de nacimiento.
- **Turnos Médicos**: Reserva y gestión de turnos asignados a un paciente y a un médico específico en una fecha/hora dada, validando reglas de negocio complejas (como evitar solapamientos de horarios).

---

## 🏗️ 2. Arquitectura y Estructura del Proyecto

El proyecto está organizado siguiendo una **Arquitectura en Capas (N-Tier Architecture)** para mantener una estricta separación de responsabilidades:

```text
Clinica-San-Salud/
├── Clinica-San-Salud.slnx           # Archivo de solución de la aplicación (.NET)
└── SanSaludAPI/                     # Proyecto principal Web API
    ├── API/                         # 🟢 Capa de Presentación (Controladores HTTP)
    │   ├── MedicosController.cs
    │   ├── PacientesController.cs
    │   └── TurnosController.cs
    ├── BusinessLogic/               # 🔵 Capa de Lógica de Negocio (Servicios y Reglas)
    │   ├── IMedicoService.cs / MedicoService.cs
    │   ├── IPacienteService.cs / PacienteService.cs
    │   └── ITurnoService.cs / TurnoService.cs
    ├── DataAccess/                  # 🔴 Capa de Acceso a Datos (EF Core DbContext & Repositorios)
    │   ├── SanSaludDbContext.cs
    │   ├── Medico.cs / Paciente.cs / Turno.cs
    │   └── Repositories (IMedicoRepository, IPacienteRepository, ITurnoRepository)
    ├── Shared/                      # 🟡 DTOs y Excepciones Personalizadas
    │   ├── DTOs (MedicoCreateDTO, TurnoResponseDTO, etc.)
    │   └── Exceptions (OverlappingScheduleException, BusinessExceptions, etc.)
    ├── Migrations/                  # Migraciones de Entity Framework Core
    └── bruno/                       # 🧪 Colección de peticiones HTTP para el cliente Bruno
```

### Descripción de cada capa:
1. **API Layer (`API/`)**: Expone los endpoints REST. No contiene lógica de negocio ni consultas directas a la base de datos; delega el trabajo a la capa de servicios y transforma los resultados en respuestas HTTP (`200 OK`, `201 Created`, `400 Bad Request`, `404 Not Found`, `409 Conflict`).
2. **Business Logic Layer (`BusinessLogic/`)**: Aplica las reglas del negocio (ejemplo: comprobar que la fecha del turno sea futura, verificar que el médico exista y que no tenga otro turno que se solape en ese horario).
3. **Data Access Layer (`DataAccess/`)**: Gestiona la interacción con la base de datos a través de **Entity Framework Core (SQLite)** mediante el patrón Repositorio.
4. **Shared Layer (`Shared/`)**: Contiene los **DTOs (Data Transfer Objects)** para que las entidades de base de datos nunca se expongan directamente al cliente, y las **Excepciones de Negocio** para comunicar errores específicos.

---

## 🚀 3. Requisitos Previos y Cómo Ejecutar

### Requisitos:
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) o superior.
- Un editor de código como **VS Code**, **Visual Studio 2022+** o **JetBrains Rider**.
- (Opcional) Cliente de API **Bruno** o navegador web.

### Pasos para iniciar la aplicación:

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/PracticasProfesionalizantes-I-2026/demo-repository.git
   cd demo-repository
   ```

2. **Restaurar dependencias y ejecutar la API:**
   ```bash
   cd SanSaludAPI
   dotnet restore
   dotnet run
   ```

3. La aplicación se ejecutará en las siguientes direcciones por defecto:
   - **HTTP**: `http://localhost:5175`
   - **HTTPS**: `https://localhost:7088`

4. **Base de Datos (SQLite):**
   - La base de datos `SanSalud.db` se crea/actualiza automáticamente mediante migraciones de Entity Framework Core. Si deseas aplicar las migraciones manualmente:
     ```bash
     dotnet ef database update
     ```

---

## 📖 4. Documentación de API y Contratos (Scalar & OpenAPI)

Esta aplicación utiliza **OpenAPI 3.0** y **Scalar API Reference** para generar la documentación interactiva de la API de forma automática.

### ¿Cómo ver los contratos de la API?
Cuando la aplicación se ejecuta en ambiente de desarrollo (`ASPNETCORE_ENVIRONMENT=Development`), ingresa a las siguientes URLs desde tu navegador:

- **Scalar API Reference (Interfaz interactiva recomendada):**
  [http://localhost:5175/scalar/v1](http://localhost:5175/scalar/v1)

- **Especificación OpenAPI en formato JSON:**
  [http://localhost:5175/openapi/v1.json](http://localhost:5175/openapi/v1.json)

Desde la interfaz de Scalar podrás:
- Inspeccionar todos los **Endpoints disponibles** (`GET`, `POST`, `PUT`, `DELETE`).
- Ver los esquemas de **solicitud (Request Body)** y **respuesta (Response Body)**.
- Probar llamadas directamente desde el navegador (*Try it out*).

---

## 🧪 5. Colección de Bruno para Pruebas (QA / Estudiantes)

En la carpeta `SanSaludAPI/bruno` se incluye una colección completa de peticiones HTTP para el cliente de API **[Bruno](https://www.usebruno.com/)** (una alternativa liviana, de código abierto y sin almacenamiento en la nube a Postman/Insomnia).

### Estructura de la Colección de Bruno:
- `Medicos/`
  - `GET GetAll Medicos`: Obtener el listado de médicos.
  - `GET Get Medico by Id`: Buscar un médico por su UUID.
  - `POST Create Medico`: Dar de alta un nuevo médico.
- `Pacientes/`
  - `GET GetAll Pacientes`: Lista de pacientes.
  - `GET Get Paciente by Id`: Buscar un paciente.
  - `POST Create Paciente`: Registrar un nuevo paciente.
- `Turnos/`
  - `GET GetAll Turnos`: Listar todos los turnos.
  - `GET Get Turnos by Medico`: Filtrar turnos por ID de médico.
  - `GET Get Turnos by Paciente`: Filtrar turnos por ID de paciente.
  - `POST Create Turno`: Crear un turno (con verificación de horario).
  - `PUT Update Turno`: Modificar fecha/hora o integrantes del turno.
  - `DELETE Delete Turno`: Cancelar/Eliminar un turno.

### Pasos para usar Bruno:
1. Descarga e instala **Bruno** desde [usebruno.com](https://www.usebruno.com/).
2. Abre Bruno y selecciona **Open Collection**.
3. Selecciona la carpeta `SanSaludAPI/bruno` que está dentro de este proyecto.
4. ¡Listo! Tendrás todos los endpoints organizados y listos para ejecutar contra `http://localhost:5175`.

---

## 🎓 6. Conceptos Clave para el Aprendizaje

Esta aplicación fue construida como un ejemplo pedagógico. A continuación se resumen los principales conceptos y patrones que los estudiantes pueden aprender analizando el código fuente:

### 💡 A. Inyección de Dependencias (DI)
En `Program.cs` se configuran las dependencias del contenedor con el tiempo de vida `Scoped` (una instancia por solicitud HTTP):
```csharp
// Registrar Repositorios
builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();

// Registrar Servicios de Negocio
builder.Services.AddScoped<ITurnoService, TurnoService>();
```
*Aprendizaje:* Esto permite desacoplar los componentes y facilita la creación de pruebas unitarias mediante Mocks.

### 💡 B. Data Transfer Objects (DTOs)
Las entidades de base de datos como `Medico`, `Paciente` y `Turno` no se envían directamente al cliente REST. Se utilizan DTOs como `MedicoCreateDTO` y `TurnoResponseDTO`.
*Aprendizaje:* Previene problemas de sobreexposición de datos (*Over-posting*), evita referencias circulares en JSON y protege el modelo interno de la base de datos.

### 💡 C. Validación de Reglas de Negocio Complejas
En `TurnoService.cs` se implementa la lógica para detectar solapamiento de horarios entre turnos del mismo médico:
```csharp
if (isOverlapping)
{
    throw new OverlappingScheduleException("El turno solicitado se solapa en horario con otro existente para el mismo médico.");
}
```
*Aprendizaje:* Separar la lógica de validación en la capa de servicios evita ensuciar los controladores y garantiza la integridad de los datos.

### 💡 D. Cambio Transparente de Motor de Base de Datos
Entity Framework Core abstrae las consultas. Actualmente se utiliza SQLite:
```csharp
builder.Services.AddDbContext<SanSaludDbContext>(options =>
    options.UseSqlite(connectionString));
```
*Aprendizaje:* Para migrar a **PostgreSQL** o **SQL Server**, solo se debe instalar el paquete NuGet correspondiente y cambiar el proveedor en `Program.cs` sin modificar una sola línea de lógica SQL manual.

---

¡Esperamos que este proyecto sirva como una guía práctica y clara para dominar el desarrollo de Web APIs modernas en .NET! 🚀
