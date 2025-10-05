## 📁 Estructura de Carpetas

```
Seeders/
├── BaseSeeder.cs          # 🏛️ Clase base abstracta
├── OwnerSeeder.cs         # 👥 Solo propietarios
├── PropertySeeder.cs      # 🏠 Solo propiedades
├── PropertyImageSeeder.cs # 📸 Solo imágenes
├── PropertyTraceSeeder.cs # 📊 Solo transacciones
└── DatabaseSeeder.cs      # 🎭 Orquestador principal
```

## 🛠️ **Cómo Extender el Sistema**

### Agregar Nueva Entidad (ej: Category)

1. **Crear CategorySeeder.cs:**
```csharp
public class CategorySeeder : BaseSeeder
{
    public override string SeederName => "Category";
    
    public override async Task<bool> ShouldSeedAsync()
    {
        return await _categories.CountDocumentsAsync(_ => true) == 0;
    }
    
    public override async Task SeedAsync()
    {
        // Tu lógica de seeding aquí
    }
    
    public override async Task ClearAsync()
    {
        await _categories.DeleteManyAsync(_ => true);
    }
}
```

2. **Agregar a DatabaseSeeder.cs:**
```csharp
private readonly CategorySeeder _categorySeeder;

public DatabaseSeeder(IConfiguration config)
{
    // ... otros seeders
    _categorySeeder = new CategorySeeder(config);
}

public async Task SeedAsync()
{
    // Ejecutar en el orden correcto
    await SeedEntityAsync(_categorySeeder);
    await SeedEntityAsync(_ownerSeeder);
    // ... resto
}
```

## 🎭 **BaseSeeder - Contrato Común**

Todos los seeders implementan:
- `ShouldSeedAsync()` - ¿Necesita seeding?
- `SeedAsync()` - Crear datos
- `ClearAsync()` - Limpiar datos
- `SeederName` - Nombre para logs

## 🔄 **Flujo de Ejecución**

### **Seeding (`dotnet run seed run`):**
1. OwnerSeeder → Crear propietarios
2. PropertySeeder → Crear propiedades (usa owners)
3. PropertyImageSeeder → Crear imágenes (usa properties)
4. PropertyTraceSeeder → Crear traces (usa properties)

### **Clearing (`dotnet run seed clear`):**
1. PropertyTraceSeeder → Limpiar traces
2. PropertyImageSeeder → Limpiar imágenes  
3. PropertySeeder → Limpiar propiedades
4. OwnerSeeder → Limpiar owners

## 🚀 **Comandos Disponibles**

```bash
# Todo funciona igual que antes
dotnet run seed run      # Seeding completo
dotnet run seed clear    # Limpieza completa
dotnet run seed reset    # Clear + Seed
```

## 🎉 **Resultado:**

- ✅ **Código más limpio** - Archivos pequeños, fáciles de leer
- ✅ **Mantenimiento simple** - Un cambio en Owner no afecta Property
- ✅ **Extensible** - Agregar nuevas entidades es súper fácil
- ✅ **Robusto** - Manejo de dependencias y errores
- ✅ **Profesional** - Arquitectura estilo Laravel/Django

¡Ahora tienes un sistema de seeding escalable y profesional! 🏗️✨