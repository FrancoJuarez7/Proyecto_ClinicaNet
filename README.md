# ClinicaNet

**ClinicaNet** es una aplicación web desarrollada en C# utilizando ASP.NET Core MVC, orientada a la gestión de afiliados y tickets médicos. Provee una solución integral para el registro, seguimiento y resolución de pedidos clínicos, mediante un sistema de control de acceso basado en roles para usuarios asistentes y doctores.

---

## 📑 Índice

1. [Descripción del Proyecto](#descripcion-del-proyecto)  
2. [Modelo de Datos](#modelo-de-datos)  
3. [Gestión de Roles y Permisos](#gestion-de-roles-y-permisos)  
   - [Asistente](#usuario-asistente)  
   - [Doctor](#usuario-doctor)  
4. [Funcionalidades Principales](#funcionalidades-principales)  
5. [Seguridad y Autenticacion](#seguridad-y-autenticacion)  
6. [Interfaz y Experiencia de Usuario](#interfaz-y-experiencia-de-usuario)  
7. [Importacion de Datos](#importacion-de-datos)  
8. [Tecnologias Utilizadas](#tecnologias-utilizadas)  
9. [Contribuciones](#contribuciones)

---

## Descripcion del Proyecto

ClinicaNet permite la gestión eficiente de afiliados, estados y tickets clínicos. Los asistentes pueden registrar y mantener los datos de los afiliados, mientras que los doctores se encargan de gestionar los tickets y sus detalles. La aplicación ofrece control de acceso, vistas personalizadas, filtros y búsqueda avanzada, e importación de datos desde archivos Excel.

---

## Modelo de Datos

- **Afiliado**
  - `Id`  
  - `Apellido`  
  - `Nombres`  
  - `DNI`  
  - `FechaNacimiento`  
  - `Foto`  

- **Estado**
  - `Id`  
  - `Descripción`

- **Ticket**
  - `Id`  
  - `AfiliadoId` (relación con Afiliado)  
  - `FechaSolicitud`  
  - `Observación`  

- **TicketDetalle**
  - `Id`  
  - `TicketId` (relación con Ticket)  
  - `DescripciónPedido`  
  - `EstadoId` (relación con Estado)  
  - `FechaEstado`  

---

## Gestion de Roles y Permisos

### Usuario Asistente

- ✅ Crear, editar, eliminar y visualizar **Afiliados**  
- 👁️ Ver detalles de **Tickets**  
- ✅ Crear, editar, eliminar y visualizar **Estados**  
- 👁️ Ver detalles de **TicketDetalle**

### Usuario Doctor

- 👁️ Ver detalles de **Afiliados** y **Estados**  
- ✅ Crear, editar, eliminar y visualizar **Tickets**  
- ✅ Crear, editar, eliminar y visualizar **TicketDetalle**

---

## Funcionalidades Principales

- Sistema de autenticación y autorización por roles  
- Vista principal personalizada para doctores con tickets pendientes de resolución  
- CRUD completo de entidades según permisos del rol  
- Búsqueda avanzada y filtros dinámicos  
- Importación de afiliados desde archivos Excel (`.xlsx`)  
- Página de acceso denegado para operaciones no autorizadas  

---

## Seguridad y Autenticacion

- Implementación de ASP.NET Core Identity  
- Control de acceso por roles  
- Protección de rutas y acciones no autorizadas  
- Páginas personalizadas para acceso denegado

---

## Interfaz y Experiencia de Usuario

- Interfaz limpia y responsiva  
- Vistas adaptadas según el tipo de usuario  
- Feedback visual para operaciones (mensajes, validaciones)  
- Navegación intuitiva con estructura clara

---

## Importacion de Datos

La aplicación permite la carga masiva de afiliados mediante archivos Excel. Se utiliza una librería como **ClosedXML** o **EPPlus** para procesar los archivos `.xlsx`.

- Validación de formato y campos requeridos  
- Rechazo de filas con errores  
- Confirmación de importación exitosa

---

## Tecnologias Utilizadas

- **Lenguaje:** C#  
- **Framework:** ASP.NET Core MVC  
- **Base de Datos:** SQL Server  
- **ORM:** Entity Framework Core  
- **Autenticación:** ASP.NET Core Identity  
- **Excel:** ClosedXML
- **Frontend:** Razor Views, Bootstrap

---

## Contribuciones

Si tenés ideas para mejorar ClinicaNet, encontraste un error o querés sumar funcionalidades, toda contribución es bienvenida.  
No dudes en participar y ayudar a que este proyecto siga creciendo.

---

## Contribuciones

Si tenés ideas para mejorar ClinicaNet, encontraste un error o querés sumar funcionalidades, toda contribución es bienvenida.  
No dudes en participar y ayudar a que este proyecto siga creciendo.
