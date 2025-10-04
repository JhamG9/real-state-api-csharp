# RealEstate API

API REST para gestión de propiedades inmobiliarias desarrollada con ASP.NET Core y MongoDB.

## Características

- 🏠 Gestión de propiedades inmobiliarias
- 👥 Gestión de propietarios
- 📸 Gestión de imágenes de propiedades
- 📁 Servicio de archivos estáticos para imágenes
- 🗄️ Base de datos MongoDB
- 🚀 ASP.NET Core 9.0

## Endpoints Principales

### Propiedades
- `GET /api/Property` - Obtener todas las propiedades
- `GET /api/Property/{id}` - Obtener propiedad por ID
- `POST /api/Property` - Crear nueva propiedad
- `PUT /api/Property/{id}` - Actualizar propiedad
- `DELETE /api/Property/{id}` - Eliminar propiedad

### Propietarios
- `GET /api/Owner` - Obtener todos los propietarios
- `GET /api/Owner/{id}` - Obtener propietario por ID
- `POST /api/Owner` - Crear nuevo propietario
- `PUT /api/Owner/{id}` - Actualizar propietario
- `DELETE /api/Owner/{id}` - Eliminar propietario

### Imágenes de Propiedades
- `GET /api/PropertyImage` - Obtener todas las imágenes
- `GET /api/PropertyImage/{id}` - Obtener imagen por ID
- `GET /api/PropertyImage/byProperty/{idProperty}` - Obtener imágenes por propiedad
- `POST /api/PropertyImage` - Subir nueva imagen
- `DELETE /api/PropertyImage/{id}` - Eliminar imagen

## Configuración

### Prerrequisitos
- .NET 9.0 SDK
- MongoDB

### Configuración de MongoDB

Actualiza el archivo `appsettings.json` con tu cadena de conexión de MongoDB:

```json
{
  "MongoSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "RealEstateDB"
  }
}
```

### Instalación y Ejecución

1. Clona el repositorio:
```bash
git clone <url-del-repositorio>
cd RealEstate.API
```

2. Restaura las dependencias:
```bash
dotnet restore
```

3. Ejecuta la aplicación:
```bash
dotnet run
```

La API estará disponible en `http://localhost:5189`

## Estructura del Proyecto

```
RealEstate.API/
├── Controllers/           # Controladores de la API
├── Models/               # Modelos de datos
├── Services/             # Servicios de negocio
├── uploads/              # Directorio para archivos subidos
├── Program.cs            # Punto de entrada de la aplicación
└── appsettings.json      # Configuración de la aplicación
```

## Tecnologías Utilizadas

- **ASP.NET Core 9.0** - Framework web
- **MongoDB.Driver** - Driver para MongoDB
- **C#** - Lenguaje de programación

## Funcionalidades

### Gestión de Imágenes
- Subida de imágenes para propiedades
- Servicio de archivos estáticos
- URLs públicas para acceso directo a imágenes
- Almacenamiento en el directorio `uploads/`

### Base de Datos
- Almacenamiento en MongoDB
- Modelos con ObjectId de MongoDB
- Operaciones CRUD completas

## Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit tus cambios (`git commit -am 'Agrega nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Abre un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT.