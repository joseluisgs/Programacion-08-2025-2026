# 🧙 Práctica 09: Moria - Proyecto Final Integrador

> *"En el profundo silencio de Moria, los ecos del pasado susurran secretos"* — Anónimo

### 📝 1. Enunciado

Proyecto final integrador que combina todos los conceptos aprendidos en la unidad: ficheos, streams, CSV, JSON, XML, DTOs, LINQ y arquitecturas completas.

> **Nota:** Consulta el enunciado completo en `Enunciado.pdf`

### 🏗️ 2. Requisitos del Proyecto

Este proyecto debe demostrar el dominio de:

- **Lectura de ficheos** en múltiples formatos (CSV, JSON, XML, texto)
- **Escritura de ficheos** con diferentes serializaciones
- **DTOs** para transferencia de datos
- **LINQ** para procesamiento y consultas
- **Patrón Repository** para acceso a datos
- **Patrón Servicio** para lógica de negocio
- **Arquitectura limpia** con separación de responsabilidades

### 🔄 3. Funcionalidades Esperadas

1. **Carga de datos** desde ficheos externos
2. **Procesamiento** con LINQ
3. **Exportación** a múltiples formatos
4. **Consultas** analíticas sobre los datos
5. **Gestión CRUD** completa

### 📁 4. Estructura de Proyecto

```
├── models/           # Entidades del dominio
├── dtos/            # Data Transfer Objects
├── mappers/         # Mapeadores modelo <-> DTO
├── repositories/    # Acceso a datos
├── services/        # Lógica de negocio
├── storage/         # Almacenamiento (CSV, JSON, XML, binario)
├── utils/           # Utilidades
└── Program.cs       # Punto de entrada
```

### ⚙️ 5. Requisitos Técnicos

- Usar **C# moderno** con sintaxis `using var`
- Usar **System.Text.Json** para JSON
- Usar **XmlSerializer** para XML
- Usar **LINQ** para todas las consultas
- Implementar **patrón Repository**
- Implementar **inyección de dependencias** si es posible
- **Gestión de errores** robusta
- **Documentación** del código

### 📊 6. Criterios de Evaluación

| Criterio | Ponderación |
|----------|-------------|
| Funcionamiento correcto | 40% |
| Calidad del código | 20% |
| Uso de patrones y arquitectura | 20% |
| Documentación | 10% |
| Git y control de versiones | 10% |

---

### 📤 Entrega

Sube el proyecto a tu repositorio GitHub con el nombre `09-Moria`.

**Fecha límite:** Consultar en Campus Virtual

**Nota:** El proyecto debe compilar y ejecutarse sin errores. Todo código debe ser original.
