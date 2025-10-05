# 🌱 Sistema de Seeding - Guía de Uso
El sistema crea:
- **5 Propietarios** con datos completos
- **8 Propiedades** en diferentes ciudades colombianas
- **~20 Imágenes** distribuidas entre las propiedades
- **~23 Traces** con historial de transacciones

### **Comando Principal**
```bash
# Ejecutar seeding
dotnet run seed run

# Limpiar base de datos
dotnet run seed clear

# Reiniciar (limpiar + seed nuevo)
dotnet run seed reset

# Ver ayuda
dotnet run seed help
```

### 👥 **Propietarios (5)**
- Juan Carlos Pérez (Bogotá)
- María Elena González (Medellín)  
- Carlos Alberto Rodríguez (Cali)
- Ana Sofía Martínez (Barranquilla)
- Diego Fernando López (Bucaramanga)

### 🏠 **Propiedades (8)**
- Casa Centro Histórico - $450M (Bogotá)
- Apartamento Zona Rosa - $320M (Bogotá)
- Casa Campestre El Poblado - $680M (Medellín)
- Apartamento Laureles - $280M (Medellín)
- Casa Granada Norte - $420M (Cali)
- Penthouse Ciudad Jardín - $850M (Cali)
- Casa El Prado - $380M (Barranquilla)
- Apartamento Cabecera - $250M (Bucaramanga)

### 📸 **Imágenes**
- **2-3 imágenes por propiedad**
- **URLs de Unsplash** (imágenes reales de casas)
- **Habilitadas por defecto**

### 📊 **PropertyTraces**
- **2-4 traces por propiedad**
- **Historial de 2 años**
- **Valores realistas** (5M - 50M por transacción)
- **Nombres variados**: Venta Inicial, Remodelación, Reparaciones, etc.

## 🎯 Ejemplos de Prueba Después del Seeding

```bash
# Ver todas las propiedades con owner y traces
GET http://localhost:5189/api/property

# Filtrar por ciudad
GET http://localhost:5189/api/property?address=bogotá

# Filtrar por rango de precios
GET http://localhost:5189/api/property?minPrice=300000000&maxPrice=500000000

# Ver propietarios
GET http://localhost:5189/api/owner

# Ver traces de una propiedad específica
GET http://localhost:5189/api/propertytrace/byproperty/{propertyId}
```

