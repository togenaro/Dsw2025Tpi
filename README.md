# Trabajo Práctico Integrador - Desarrollo de Software
## Backend - ASP.NET Core API

---

## 📌 Integrante del Grupo
- **Nombre:** Genaro Toledo
- **Legajo:** 53477
- **Correo:** genaro.toledo@alu.frt.utn.edu.ar

---

## 📜 Introducción
Este proyecto corresponde a la **primera parte** del Trabajo Práctico Integrador de la materia **Desarrollo de Software**.  
El objetivo es desarrollar el módulo de **Órdenes** para una plataforma de comercio electrónico (*E-commerce*), permitiendo la gestión completa de las mismas.

---

## 🎯 Visión General del Producto
A partir del relevamiento de requisitos, se definieron las siguientes funcionalidades:

- Los visitantes pueden consultar los productos sin necesidad de registrarse o iniciar sesión.
- Para realizar un pedido es necesario iniciar sesión.
- Una orden debe incluir información de cliente, envío y facturación.
- Antes de registrar una orden se valida el stock de los productos.
- Si la orden es exitosa, se descuenta el stock correspondiente.
- Es posible consultar órdenes individuales o listarlas con filtros.
- Las órdenes pueden cambiar de estado según su ciclo de vida.
- **Administradores:** pueden gestionar productos (alta, modificación y baja) y actualizar estados de órdenes.
- **Clientes:** pueden crear y consultar sus propias órdenes.

---

## 📦 Tecnologías Utilizadas
- **Lenguaje:** C# 12.0
- **Framework:** .NET 8
- **Base de datos:** SQL Server
- **ORM:** Entity Framework Core
- **Autenticación y Autorización:** ASP.NET Identity + JWT
- **Documentación y pruebas de API:** Swagger
- **Patrón de diseño:** Separación en capas (API, Application, Domain, Data)

---

## ⚙️ Instrucciones para Ejecutar el Proyecto Localmente

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/togenaro/Dsw2025Tpi.git
   ```

2. **Configurar la base de datos**
   - En el archivo `appsettings.Development.json` (o `appsettings.json`), modificar la cadena de conexión:
     ```json
     "ConnectionStrings": {
         "Dsw2025TpiEntities": "Server=localhost;Database=Dsw2025Tpi;Trusted_Connection=True;TrustServerCertificate=True;"
     }
     ```

3. **Aplicar migraciones**
   ```bash
   dotnet ef database update
   ```

4. **Ejecutar la aplicación**
   ```bash
   dotnet run --project Dsw2025Tpi.Api
   ```

5. **Acceder a la documentación Swagger**
   - URL por defecto:
     ```
     https://localhost:7138/swagger
     ```

---

## 🔗 Endpoints Implementados

### Autenticación (`/api/authenticate`)
- **POST** `/login` → Inicia sesión y devuelve un token JWT.
- **POST** `/register` → Registra un nuevo usuario.

### Administración de Roles (`/api/admin`)
- **POST** `/assign-role` → Asigna un rol a un usuario (solo administradores).

### Órdenes (`/api/orders`)
- **GET** `/` → Lista órdenes con filtros opcionales.
- **GET** `/{id}` → Obtiene una orden por su ID.
- **POST** `/` → Crea una nueva orden.
- **PUT** `/{id}/status` → Actualiza el estado de una orden.

### Productos (`/api/products`)
- **GET** `/` → Lista de productos.
- **GET** `/{id}` → Obtiene un producto por ID.
- **POST** `/` → Agrega un nuevo producto.
- **PUT** `/{id}` → Modifica un producto existente.
- **DELETE** `/{id}` → Elimina un producto.
