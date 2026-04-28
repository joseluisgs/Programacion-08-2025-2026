- [4. Formatos de Intercambio (I): DTOs y CSV](#4-formatos-de-intercambio-i-dtos-y-csv)
  - [4.1. Introducción: El Problema de la Persistencia de Objetos](#41-introducción-el-problema-de-la-persistencia-de-objetos)
  - [4.2. ¿Qué es CSV? Valores Separados por Comas](#42-qué-es-csv-valores-separados-por-comas)
  - [4.3. El Patrón DTO](#43-el-patrón-dto)
    - [4.3.1. ¿Qué es un DTO?](#431-qué-es-un-dto)
    - [4.3.2. ¿Por Qué Usar DTOs?](#432-por-qué-usar-dtos)
  - [4.4. Escritura de CSV](#44-escritura-de-csv)
    - [4.4.1. Escritura Manual Básica](#441-escritura-manual-básica)
    - [4.4.2. Añadir Método ToCsv al DTO](#442-añadir-método-tocsv-al-dto)
    - [4.4.3. Escritura con LINQ](#443-escritura-con-linq)
  - [4.5. Lectura de CSV](#45-lectura-de-csv)
    - [4.5.1. Lectura Básica con StreamReader](#451-lectura-básica-con-streamreader)
    - [4.5.2. Lectura con LINQ](#452-lectura-con-linq)
  - [4.6. Procesamiento Avanzado con LINQ](#46-procesamiento-avanzado-con-linq)
    - [4.6.1. Filtrado y Ordenación](#461-filtrado-y-ordenación)
    - [4.6.2. Estadísticas con LINQ](#462-estadísticas-con-linq)
    - [4.6.3. Proyección y Transformación](#463-proyección-y-transformación)
  - [4.7. Manejo de Casos Especiales en CSV](#47-manejo-de-casos-especiales-en-csv)
    - [4.7.1. Problema: Comas en los Datos](#471-problema-comas-en-los-datos)
    - [4.7.2. Diferentes Separadores](#472-diferentes-separadores)
  - [4.8. Ejemplo Integrador](#48-ejemplo-integrador)

# 4. Formatos de Intercambio (I): DTOs y CSV

## 4.1. Introducción: El Problema de la Persistencia de Objetos

Hasta ahora hemos trabajado con **texto plano** sin estructura. Pero en aplicaciones reales, necesitamos **guardar y recuperar objetos** con múltiples propiedades.

**Problema:**

```csharp
// Tengo un objeto en memoria
var alumno = new Alumno 
{ 
    Id = 1, 
    Nombre = "Ana García", 
    Edad = 20, 
    Nota = 8.5 
};

// ¿Cómo lo guardo en un fichero de texto?
// ¿Cómo lo recupero después?
```

**Soluciones:**

| Formato     | Legibilidad | Tamaño      | Uso                 | Ejemplo                          |
| ----------- | ----------- | ----------- | ------------------- | -------------------------------- |
| **CSV**     | Alta        | Pequeño     | Tablas, Excel       | `1,Ana García,20,8.5`            |
| **JSON**    | Alta        | Medio       | APIs, Web           | `{"id":1,"nombre":"Ana García"}` |
| **XML**     | Media       | Grande      | Configuración, SOAP | `<alumno><id>1</id></alumno>`    |
| **Binario** | Nula        | Muy pequeño | Alto rendimiento    | `[01][00][00][00][41][6E]... `   |

> 📝 **Nota del Profesor**: En esta unidad aprenderemos CSV y DTOs. Son fundamentales para el intercambio de datos con hojas de cálculo y sistemas legacy.

## 4.2. ¿Qué es CSV? Valores Separados por Comas

**CSV** (Comma-Separated Values) es un formato de texto donde:
- Cada **línea** representa un **registro** (fila)
- Los **valores** se separan por **comas** (o punto y coma `;` en Europa)
- La **primera línea** suele ser la **cabecera** (nombres de columnas)

**Ejemplo:**

```csv
Id,Nombre,Edad,Nota
1,Ana García,20,8.5
2,Juan Pérez,22,7.0
3,María López,21,9.2
```

**Visualización en Excel:**

| Id  | Nombre      | Edad | Nota |
| --- | ----------- | ---- | ---- |
| 1   | Ana García  | 20   | 8.5  |
| 2   | Juan Pérez  | 22   | 7.0  |
| 3   | María López | 21   | 9.2  |

**Características:**

✅ **Legible**: Cualquier editor de texto puede abrirlo  
✅ **Universal**: Excel, Google Sheets, LibreOffice lo leen  
✅ **Simple**: No requiere librerías externas  
✅ **Ligero**: Mucho más pequeño que XML  
❌ **Limitado**: Solo tablas planas (no jerarquías)  
❌ **Frágil**: Problemas con comas, saltos de línea en los datos  

## 4.3. El Patrón DTO

### 4.3.1. ¿Qué es un DTO?

Un **DTO** (Data Transfer Object) es una **clase simple** diseñada para **transportar datos** entre diferentes capas o sistemas. **No contiene lógica de negocio**, solo propiedades.

**Características de un DTO:**

✅ Solo propiedades públicas  
✅ Sin métodos de negocio (excepto conversión de formato)  
✅ Inmutable (preferiblemente)  
✅ Usa `record` en C# moderno  

**Ejemplo:**

```csharp
public record AlumnoDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public int Edad { get; init; }
    public double Nota { get; init; }
}
```

### 4.3.2. ¿Por Qué Usar DTOs?

**Sin DTO (acoplamiento directo):**

```csharp
// ❌ MAL: Modelo de negocio acoplado a CSV

public class Alumno
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public double Nota { get; set; }
    
    // Lógica de negocio
    public bool EstaAprobado() => Nota >= 5.0;
    
    // Persistencia CSV (¡acoplamiento!)
    public string ToCsv() => $"{Id},{Nombre},{Edad},{Nota}";
    
    // ¡La clase hace DEMASIADAS cosas!
}
```

**Con DTO (separación de responsabilidades):**

```csharp
// ✓ BIEN: Separar modelo de negocio y persistencia

// MODELO DE NEGOCIO (lógica)
public class Alumno
{
    public int Id { get; }
    public string Nombre { get; }
    public int Edad { get; }
    public double Nota { get; }
    
    public Alumno(int id, string nombre, int edad, double nota)
    {
        Id = id; Nombre = nombre; Edad = edad; Nota = nota;
    }
    
    public bool EstaAprobado() => Nota >= 5.0;
}

// DTO PARA CSV (persistencia)
public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

// MAPPER entre modelo y DTO
public static class AlumnoMapper
{
    public static AlumnoDto ToDto(Alumno alumno) => new(alumno.Id, alumno.Nombre, alumno.Edad, alumno.Nota);
    public static Alumno ToDomain(AlumnoDto dto) => new(dto.Id, dto.Nombre, dto.Edad, dto.Nota);
}
```

**Ventajas:**

✅ **Separación de responsabilidades**: Modelo de negocio vs Persistencia  
✅ **Cambio de formato fácil**: Cambiar CSV por JSON solo afecta al DTO  
✅ **Testeable**: Puedes testear el modelo sin ficheros  
✅ **Evolución independiente**: Modelo y formato evolucionan por separado  

## 4.4. Escritura de CSV

### 4.4.1. Escritura Manual Básica

```csharp
using System;
using System.IO;
using System.Collections.Generic;

// Definir DTO
public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

// Crear datos de prueba
var alumnos = new List<AlumnoDto>
{
    new(1, "Ana García", 20, 8.5),
    new(2, "Juan Pérez", 22, 7.0),
    new(3, "María López", 21, 9.2),
    new(4, "Pedro Martín", 23, 6.5),
    new(5, "Laura Ruiz", 20, 8.0)
};

string rutaCsv = "alumnos.csv";

Console.WriteLine(">>> Escribiendo CSV...\n");

// Escribir con StreamWriter
using var writer = new StreamWriter(rutaCsv);

// 1. Escribir CABECERA
writer.WriteLine("Id,Nombre,Edad,Nota");

// 2. Escribir DATOS
foreach (var alumno in alumnos)
{
    writer.WriteLine($"{alumno.Id},{alumno.Nombre},{alumno.Edad},{alumno.Nota}");
}

Console.WriteLine($"✓ CSV escrito: {rutaCsv}");
```

### 4.4.2. Añadir Método ToCsv al DTO

```csharp
public record AlumnoDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = "";
    public int Edad { get; init; }
    public double Nota { get; init; }
    
    // Método de conversión a CSV
    public string ToCsv() => $"{Id},{Nombre},{Edad},{Nota}";
    
    // Método estático para parsear desde CSV
    public static AlumnoDto FromCsv(string linea)
    {
        var partes = linea.Split(',');
        return new AlumnoDto(
            int.Parse(partes[0]),
            partes[1],
            int.Parse(partes[2]),
            double.Parse(partes[3])
        );
    }
}

// Uso
using var writer = new StreamWriter("alumnos.csv");
writer.WriteLine("Id,Nombre,Edad,Nota");
foreach (var a in alumnos)
{
    writer.WriteLine(a.ToCsv()); // Mucho más limpio
}
```

### 4.4.3. Escritura con LINQ

```csharp
// Escribir CSV con LINQ (más elegante)
using var writer = new StreamWriter("alumnos.csv");
writer.WriteLine("Id,Nombre,Edad,Nota");

alumnos
    .Select(a => $"{a.Id},{a.Nombre},{a.Edad},{a.Nota}")
    .ToList()
    .ForEach(linea => writer.WriteLine(linea));

// O más conciso:
File.WriteAllLines("alumnos.csv", 
    new[] { "Id,Nombre,Edad,Nota" }
    .Concat(alumnos.Select(a => $"{a.Id},{a.Nombre},{a.Edad},{a.Nota}"))
);
```

## 4.5. Lectura de CSV

### 4.5.1. Lectura Básica con StreamReader

```csharp
Console.WriteLine(">>> Leyendo CSV...\n");

var alumnosLeidos = new List<AlumnoDto>();

using var reader = new StreamReader(rutaCsv);

// Saltar cabecera
string? cabecera = reader.ReadLine();

string? linea;
while ((linea = reader.ReadLine()) != null)
{
    var partes = linea.Split(',');
    var alumno = new AlumnoDto(
        int.Parse(partes[0]),
        partes[1],
        int.Parse(partes[2]),
        double.Parse(partes[3])
    );
    alumnosLeidos.Add(alumno);
}

Console.WriteLine($"✓ Leídos {alumnosLeidos.Count} alumnos");

foreach (var a in alumnosLeidos)
{
    Console.WriteLine($"  {a.Id}: {a.Nombre} - Nota: {a.Nota}");
}
```

### 4.5.2. Lectura con LINQ

```csharp
// Lectura con LINQ (más elegante)
var csvData = File.ReadAllLines(rutaCsv)
    .Skip(1) // Saltar cabecera
    .Select(linea => linea.Split(','))
    .Select(partes => new AlumnoDto(
        int.Parse(partes[0]),
        partes[1],
        int.Parse(partes[2]),
        double.Parse(partes[3])
    ))
    .ToList();

// O como IEnumerable (lazy)
var csvDataLazy = File.ReadLines(rutaCsv)
    .Skip(1)
    .Select(linea => linea.Split(','))
    .Select(partes => new AlumnoDto(
        int.Parse(partes[0]),
        partes[1],
        int.Parse(partes[2]),
        double.Parse(partes[3])
    ));

### 📝 Nota del Profesor: El CSV como Cadena de Montaje

Cuando usas `File.ReadLines` junto con LINQ (Select, Where), estás creando una **cadena de montaje**.

1.  `ReadLines` abre el grifo de datos (línea 1).
2.  `Skip(1)` descarta la cabecera.
3.  `Select` transforma esa línea en un objeto DTO.
4.  Todo esto ocurre **bajo demanda**. No hay una lista gigante de objetos en memoria.

**⚠️ Evita el .ToList() prematuro:** 
Si haces `.ReadLines(...).Select(...).ToList()`, estás rompiendo la cadena de montaje y obligando a meter todos los objetos en un "parking" (RAM). Hazlo solo si necesitas el `.Count` o si vas a recorrer la misma lista varias veces. Para procesar datos una sola vez, ¡mantén el flujo Lazy!
```

## 4.6. Procesamiento Avanzado con LINQ

### 4.6.1. Filtrado y Ordenación

```csharp
// Alumnos aprobados (nota >= 5)
var aprobados = csvData
    .Where(a => a.Nota >= 5)
    .OrderByDescending(a => a.Nota);

Console.WriteLine("\n>>> Alumnos Aprobados:");
foreach (var a in aprobados)
{
    Console.WriteLine($"  {a.Nombre}: {a.Nota}");
}

// Alumnos mayores de 21 años
var mayores = csvData
    .Where(a => a.Edad > 21)
    .OrderBy(a => a.Nombre);
```

### 4.6.2. Estadísticas con LINQ

```csharp
Console.WriteLine("\n>>> ESTADÍSTICAS:");

// Nota media
double notaMedia = csvData.Average(a => a.Nota);
Console.WriteLine($"  Nota media: {notaMedia:F2}");

// Nota máxima y mínima
double notaMax = csvData.Max(a => a.Nota);
double notaMin = csvData.Min(a => a.Nota);
Console.WriteLine($"  Nota máxima: {notaMax}");
Console.WriteLine($"  Nota mínima: {notaMin}");

// Contar aprobados
int numAprobados = csvData.Count(a => a.Nota >= 5);
Console.WriteLine($"  Aprobados: {numAprobados}/{csvData.Count}");

// Suma de notas
double sumaNotas = csvData.Sum(a => a.Nota);
Console.WriteLine($"  Suma de notas: {sumaNotas:F2}");
```

### 4.6.3. Proyección y Transformación

```csharp
// Proyección: crear nuevos objetos con solo algunos campos
var nombresYNotas = csvData
    .Select(a => new { a.Nombre, a.Nota, Calificacion = a.Nota >= 9 ? "Sobresaliente" : a.Nota >= 7 ? "Notable" : a.Nota >= 5 ? "Aprobado" : "Suspenso" });

Console.WriteLine("\n>>> CALIFICACIONES:");
foreach (var n in nombresYNotas)
{
    Console.WriteLine($"  {n.Nombre}: {n.Nota} ({n.Calificacion})");
}

// Agrupación por edad
var porEdad = csvData.GroupBy(a => a.Edad);

Console.WriteLine("\n>>> ALUMNOS POR EDAD:");
foreach (var grupo in porEdad)
{
    Console.WriteLine($"  Edad {grupo.Key}: {grupo.Count()} alumnos");
}
```

## 4.7. Manejo de Casos Especiales en CSV

### 4.7.1. Problema: Comas en los Datos

```csharp
// ❌ PROBLEMA: Si el nombre contiene coma
var alumno = new AlumnoDto(1, "García, Ana", 20, 8.5);
writer.WriteLine($"{alumno.Id},{alumno.Nombre},{alumno.Edad},{alumno.Nota}");
// Resultado: 1,García, Ana,20,8.5  <- Mal parseado!

// ✓ SOLUCIÓN: Envolver en comillas
string FormatearCampo(string campo) => campo.Contains(',') ? $"\"{campo}\"" : campo;

writer.WriteLine($"{alumno.Id},{FormatearCampo(alumno.Nombre)},{alumno.Edad},{alumno.Nota}");
// Resultado: 1,"García, Ana",20,8.5  <- Correcto
```

### 4.7.2. Diferentes Separadores

```csharp
// En Europa es común usar punto y coma
char separador = ';';

var lineas = csvData
    .Select(a => string.Join((), a.Id, a.Nombreseparador.ToString, a.Edad, a.Nota));

// Escribir con punto y coma
File.WriteAllLines("alumnos_europa.csv", 
    new[] { $"Id{separador}Nombre{separador}Edad{separador}Nota" }
    .Concat(lineas)
);
```

## 4.8. Ejemplo Integrador

```csharp
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    public string ToCsv() => $"{Id},{Nombre},{Edad},{Nota}";
    
    public static AlumnoDto FromCsv(string linea)
    {
        var p = linea.Split(',');
        return new(int.Parse(p[0]), p[1], int.Parse(p[2]), double.Parse(p[3]));
    }
}

class ProgramaCSV
{
    static void Main()
    {
        var alumnos = new List<AlumnoDto>
        {
            new(1, "Ana García", 20, 8.5),
            new(2, "Juan Pérez", 22, 7.0),
            new(3, "María López", 21, 9.2),
            new(4, "Pedro Martín", 23, 6.5),
            new(5, "Laura Ruiz", 20, 8.0)
        };
        
        string ruta = "alumnos.csv";
        
        // Escribir
        using var w = new StreamWriter(ruta);
        w.WriteLine("Id,Nombre,Edad,Nota");
        foreach (var a in alumnos)
            w.WriteLine(a.ToCsv());
        
        // Leer y procesar
        var data = File.ReadAllLines(ruta)
            .Skip(1)
            .Select(AlumnoDto.FromCsv)
            .ToList();
        
        Console.WriteLine(">>> RESUMEN:");
        Console.WriteLine($"  Total: {data.Count}");
        Console.WriteLine($"  Nota media: {data.Average(a => a.Nota):F2}");
        Console.WriteLine($"  Mejor nota: {data.Max(a => a.Nota)}");
        
        // Mejores alumnos
        var mejores = data.Where(a => a.Nota >= 8).OrderByDescending(a => a.Nota);
        Console.WriteLine("\n>>> MEJORES ALUMNOS:");
        foreach (var m in mejores)
            Console.WriteLine($"  {m.Nombre}: {m.Nota}");
    }
}
```

> 📝 **Nota del Profesor**: El patrón DTO + CSV es fundamental. Te permitirá exportar datos a Excel y procesarlos con LINQ. Es una habilidad que usarás mucho en el mundo real.
