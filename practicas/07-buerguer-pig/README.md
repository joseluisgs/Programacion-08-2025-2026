# 🍔 Práctica 07: Burguer Pig - Sistema de Gestión de Hamburguesas

> *"No hay problema que una buena hamburguesa no pueda resolver"* — Anónimo

### 📝 1. Enunciado

Crea un sistema completo de gestión de hamburguesas con múltiples formatos de almacenamiento.

### 🏗️ 2. Estructura de Datos

#### Ingrediente
- `int id` - Identificador auto-numérico
- `String nombre` - Nombre del ingrediente
- `double precio` - Precio del ingrediente

#### Hamburguesa
- `UUID id` - Identificador único
- `String nombre` - Nombre de la hamburguesa
- `List<Ingrediente> ingredientes` - Lista de ingredientes
- `double precio` - Precio calculado desde ingredientes

### 🔄 3. Almacenamiento Requerido

Implementa almacenamiento y repositorio para **Ingredientes** y **Hamburguesas** en los siguientes formatos:

1. **Ficheros de texto** - Lectura y escritura básica
2. **Ficheros binarios** - BinaryWriter/BinaryReader
3. **Ficheros serializados** - Serialización binaria de .NET
4. **Ficheros CSV** - Formato CSV con DTOs
5. **Ficheros JSON** - System.Text.Json
6. **Ficheros XML** - XmlSerializer

### 📊 4. Consultas LINQ

Implementa las siguientes consultas sobre los datos:

1. **Hamburguesa más cara** - La de mayor precio
2. **Hamburguesa con más ingredientes** - Mayor número de ingredientes
3. **Número de hamburguesas por ingrediente** - ¿Cuántas hamburguesas usan cada ingrediente?
4. **Hamburguesas agrupadas por total de ingredientes**
5. **Precio medio de las hamburguesas**
6. **Precio medio de los ingredientes**

### 🏗️ 5. Arquitectura

```
├── models/
│   ├── Ingrediente.cs
│   └── Hamburguesa.cs
├── dtos/
│   ├── IngredienteDto.cs
│   └── HamburguesaDto.cs
├── storage/
│   ├── TextStorage.cs
│   ├── BinaryStorage.cs
│   ├── SerializedStorage.cs
│   ├── CsvStorage.cs
│   ├── JsonStorage.cs
│   └── XmlStorage.cs
├── repositories/
│   ├── IngredienteRepository.cs
│   └── HamburguesaRepository.cs
└── Program.cs
```

### ⚙️ 6. Requisitos Técnicos

- Usar **patrón Repository** para separación de responsabilidades
- Usar **DTOs** para el intercambio con ficheos
- Usar **mapeadores** entre modelo y DTO
- Usar **interfaces** para los repositorios
- Usar sintaxis moderna `using var`
- Implementar **CRUD** completo (Create, Read, Update, Delete)

---

### 📤 Entrega

Sube el proyecto a tu repositorio GitHub con el nombre `07-BurguerPig`.
