# 👥 Práctica 08: Herencia, DTOs y Ficheros

> *"La herencia es la羡慕 de la programación orientada a objetos"* — Anónimo

### 📝 1. Enunciado

Dado un ficheo CSV con personas (profesores y alumnos), procesa los datos usando herencia y DTOs, y realiza consultas LINQ.

### 🏗️ 2. Estructura de Datos (Herencia)

```
Persona (clase base)
├── String nombre

Profesor (hereda de Persona)
├── String modulo

Alumno (hereda de Persona)
└── int edad
```

### 📂 2.1. Fichero de Entrada

El ficheo `personas.csv` contiene personas con estructura mixta:
- Puede estar en carpeta `resources` o `data`
- Formato CSV con campos variables

### 🔄 3. Consultas Requeridas

Implementa las siguientes consultas LINQ:

1. **Profesor más mayor** - El de mayor edad
2. **Alumno más joven** - El de menor edad
3. **Media de edad de alumnos**
4. **Media de longitud de nombre** - Longitud media de los nombres
5. **Listado agrupado por tipo** - Profesores vs Alumnos
6. **Listado de profesores de programación** - Los que enseñan "Programación"
7. **Agrupados por edad, número de alumnos**
8. **Agrupados por módulo, número de profesores**
9. **Agrupados por edad, obtener la longitud de nombre**
10. **Agrupados por edad, obtener el nombre más largo**

### 📤 4. Exportación

Escribir en un ficheo llamado `profesores.csv` en la carpeta `output`:
- Solo los datos de los profesores
- Con formato CSV apropiado

### 🏗️ 5. Arquitectura Sugerida

```
├── models/
│   ├── Persona.cs
│   ├── Profesor.cs
│   └── Alumno.cs
├── dtos/
│   ├── PersonaDto.cs
│   ├── ProfesorDto.cs
│   └── AlumnoDto.cs
├── mappers/
│   └── PersonaMapper.cs
├── repositories/
│   └── PersonaRepository.cs
├── services/
│   └── PersonaService.cs
└── Program.cs
```

### ⚙️ 6. Requisitos Técnicos

- Implementar **herencia** correctamente (Profesor : Persona, Alumno : Persona)
- Usar **DTOs** para el intercambio con ficheos
- Usar **mapeadores** entre modelo y DTO
- Usar **LINQ** para todas las consultas
- Usar sintaxis moderna `using var`
- Usar **polimorfismo** donde corresponda
- Manejar la lectura de tipos mixtos (profesores y alumnos)

---

### 📤 Entrega

Sube el proyecto a tu repositorio GitHub con el nombre `08-Herencia-DTO`.
