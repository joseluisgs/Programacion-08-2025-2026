# Ejercicios: Ficheros, Streams y Formatos de Intercambio

- [Ejercicios: Ficheros, Streams y Formatos de Intercambio](#ejercicios-ficheros-streams-y-formatos-de-intercambio)
  - [Ejercicio 1: Gestor de Ficheros de Texto](#ejercicio-1-gestor-de-ficheros-de-texto)
  - [Ejercicio 2: Procesador de CSV con LINQ](#ejercicio-2-procesador-de-csv-con-linq)
  - [Ejercicio 3: Sistema de Configuración JSON](#ejercicio-3-sistema-de-configuración-json)
  - [Ejercicio 4: Conversor de Formatos](#ejercicio-4-conversor-de-formatos)
  - [Ejercicio 5: Logger con Rotación](#ejercicio-5-logger-con-rotación)
  - [Ejercicio 6: Procesador de Logs](#ejercicio-6-procesador-de-logs)

## Ejercicio 1: Gestor de Ficheros de Texto

Crea una aplicación que gestione ficheos de texto con las siguientes funcionalidades:

1. Crear un ficheo de texto con contenido.
2. Leer el contenido de un ficheo.
3. Añadir contenido al final de un ficheo existente.
4. Copiar un ficheo a otra ubicación.
5. Mostrar información del ficheo (tamaño, fechas).

**Requisitos:**
- Usar la sintaxis moderna `using var`.
- Manejar excepciones apropiadamente.
- Crear un menú interactivo.

## Ejercicio 2: Procesador de CSV con LINQ

Crea un procesador de CSV que:

1. Lea un ficheo CSV con datos de estudiantes (id, nombre, edad, nota).
2. Filtre estudiantes por nota mínima.
3. Ordene por nombre o por nota.
4. Calcule estadísticas (media, máximo, mínimo).
5. Agrupe por rango de edad.

**Datos de prueba:**
```csv
id,nombre,edad,nota
1,Ana García,20,8.5
2,Juan Pérez,22,7.0
3,María López,21,9.2
4,Pedro Martín,23,6.5
5,Laura Ruiz,20,8.0
```

**Requisitos:**
- Usar DTOs para representar los datos.
- Usar LINQ para todas las operaciones de procesamiento.
- Mostrar resultados de forma clara.

## Ejercicio 3: Sistema de Configuración JSON

Implementa un sistema de configuración:

1. Crea una clase `Configuracion` con propiedades para una aplicación (nombre, versión, servidor BD, puerto, etc.).
2. Serializa la configuración a JSON.
3. Deserializa desde JSON.
4. Permite modificar valores y guardar.

**Requisitos:**
- Usar `JsonSerializer` de `System.Text.Json`.
- Usar `JsonPropertyName` para el mapeo.
- Manejar errores si el ficheo no existe o está malformado.

## Ejercicio 4: Conversor de Formatos

Crea un conversor que:

1. Lea un ficheo CSV.
2. Convierta los datos a JSON.
3. Convierta los datos a XML.
4. Guarde en el formato seleccionado.

**Requisitos:**
- Crear DTOs para los datos.
- Implementar serialización a los tres formatos.
- Crear un menú para seleccionar el formato de salida.

## Ejercicio 5: Logger con Rotación

Implementa un sistema de logging que:

1. Escriba logs con timestamp y nivel (INFO, WARNING, ERROR).
2. Cree un nuevo ficheo cada día (rotación diaria).
3. Mantenga solo los últimos 7 días de logs.
4. Permita filtrar por nivel.

**Requisitos:**
- Usar `StreamWriter` con `using var`.
- Usar `File` para gestión de ficheos.
- Implementar rotación por fecha.

## Ejercicio 6: Procesador de Logs

Crea un procesador de logs que:

1. Lea un log con formato: `[TIMESTAMP] [NIVEL] Mensaje`
2. Cuente errores, warnings e infos.
3. Filtre por rango de fechas.
4. Genere un informe con estadísticas.

**Log de ejemplo:**
```
[2025-01-15 10:30:15] INFO Aplicación iniciada
[2025-01-15 10:30:20] ERROR Error de conexión a BD
[2025-01-15 10:30:25] WARNING Reintentando conexión
[2025-01-15 10:30:30] INFO Conexión restaurada
```

**Requisitos:**
- Usar expresiones regulares o string parsing.
- Usar LINQ para procesamiento.
- Generar informe en texto claro.

---

## Ejercicios Bonus

### Bonus 1: Compresor de Ficheros
Implementa un sistema que comprima ficheos en ZIP y los descomprima.

### Bonus 2: Buscador de Ficheros
Crea un buscador que busque ficheos por nombre, extensión, tamaño y fecha en un directorio y subdirectorios.

### Bonus 3: Monitor de Directorio
Implementa un monitor que detecte cambios en un directorio (creación, modificación, eliminación de ficheos).
