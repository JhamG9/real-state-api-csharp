# RealEstate API
API REST completa para gestión de propiedades inmobiliarias desarrollada con ASP.NET Core 9.0 y MongoDB.

### Instalación y Ejecución
1. Clona el repositorio:
```bash
git clone https://github.com/JhamG9/real-state-api-csharp.git
cd real-state-api-csharp
```

2. Restaura las dependencias:
```bash
dotnet restore
```

3. Ejecuta los tests:
```bash
dotnet test
```

4. Ejecuta la aplicación:
```bash
dotnet run
```

# Rutas
### 🏠 Propiedades (`/api/property`) ⭐ **CON FILTROS**
- `GET /api/property` - Obtener todas las propiedades (con filtros opcionales)
- `GET /api/property/{id}` - Obtener propiedad por ID
- `POST /api/property` - Crear nueva propiedad
- `PUT /api/property/{id}` - **Actualización parcial** de propiedad
- `DELETE /api/property/{id}` - Eliminar propiedad

#### � **Filtros de Búsqueda Disponibles:**
```
GET /api/property?name={texto}&address={texto}&minPrice={numero}&maxPrice={numero}
```

**Parámetros de Query:**
- `name` - Busca coincidencias en el nombre (LIKE, case-insensitive)
- `address` - Busca coincidencias en la dirección (LIKE, case-insensitive)  
- `minPrice` - Precio mínimo (mayor o igual)
- `maxPrice` - Precio máximo (menor o igual)

**Ejemplos de Uso:**
```bash
# Buscar por nombre
GET /api/property?name=casa

# Buscar por dirección
GET /api/property?address=calle+75

# Buscar por rango de precios
GET /api/property?minPrice=100000&maxPrice=500000

# Combinación de filtros
GET /api/property?name=casa&address=centro&minPrice=200000&maxPrice=800000

# Sin filtros (todas las propiedades)
GET /api/property
```

### �👥 Propietarios (`/api/owner`)
- `GET /api/owner` - Obtener todos los propietarios
- `GET /api/owner/{id}` - Obtener propietario por ID
- `POST /api/owner` - Crear nuevo propietario
- `PUT /api/owner/{id}` - **Actualización parcial** de propietario
- `DELETE /api/owner/{id}` - Eliminar propietario

### 📸 Imágenes de Propiedades (`/api/propertyimage`)
- `GET /api/propertyimage` - Obtener todas las imágenes
- `GET /api/propertyimage/{id}` - Obtener imagen por ID
- `GET /api/propertyimage/byproperty/{idProperty}` - Obtener imágenes por propiedad
- `POST /api/propertyimage` - Subir nueva imagen (multipart/form-data)
- `DELETE /api/propertyimage/{id}` - Eliminar imagen
- `GET /public/uploads/{filename}` - **Acceso directo a archivos de imagen**

### 📊 Trazabilidad de Propiedades (`/api/propertytrace`)
- `GET /api/propertytrace` - Obtener todos los registros
- `GET /api/propertytrace/{id}` - Obtener registro por ID
- `GET /api/propertytrace/byproperty/{propertyId}` - Obtener por propiedad
- `POST /api/propertytrace` - Crear nuevo registro
- `PUT /api/propertytrace/{id}` - **Actualización parcial** ⚡
- `PATCH /api/propertytrace/{id}` - Reemplazo completo
- `DELETE /api/propertytrace/{id}` - Eliminar registro

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

La API estará disponible en `http://localhost:5189`

## 🧪 Testing

### Ejecutar Tests Unitarios
```bash
# Ejecutar todos los tests
dotnet test

# Ejecutar con verbosidad detallada
dotnet test --verbosity normal

# Ejecutar solo tests de un archivo específico
dotnet test --filter "ClassName=PropertyTraceServiceTests"
```

## Estructura del Proyecto

```
RealEstate.API/
├── Controllers/           # Controladores de la API REST
├── Models/               # Modelos de datos y DTOs
│   ├── Owner/           # Modelos de propietarios
│   ├── Property/        # Modelos de propiedades
│   ├── PropertyImage/   # Modelos de imágenes
│   ├── PropertyTrace/   # Modelos de trazabilidad ⭐
│   └── Common/          # Modelos compartidos (paginación)
├── Services/             # Servicios de negocio (lógica)
├── Tests/                # Tests unitarios con NUnit 🧪
├── Middleware/           # Middleware personalizado
├── uploads/              # Directorio para archivos subidos
├── Program.cs            # Punto de entrada de la aplicación
└── appsettings.json      # Configuración de la aplicación
```