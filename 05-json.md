- [5. Formatos de Intercambio (II): JSON](#5-formatos-de-intercambio-ii-json)
  - [5.1. Introducción: ¿Qué es JSON y Por Qué es el Rey?](#51-introducción-qué-es-json-y-por-qué-es-el-rey)
  - [5.2. Sintaxis Básica de JSON](#52-sintaxis-básica-de-json)
  - [5.3. El Duelo: Newtonsoft.Json vs System.Text.Json](#53-el-duelo-newtonsoftjson-vs-systemtextjson)
  - [5.4. Serialización: De Objeto a JSON](#54-serialización-de-objeto-a-json)
    - [5.4.1. Serialización Básica](#541-serialización-básica)
    - [5.4.2. Pretty Print (JSON Legible)](#542-pretty-print-json-legible)
    - [5.4.3. Guardar JSON en Fichero](#543-guardar-json-en-fichero)
    - [5.4.4. Serializar Listas](#544-serializar-listas)
  - [5.5. Deserialización: De JSON a Objeto](#55-deserialización-de-json-a-objeto)
    - [5.5.1. Deserialización Básica](#551-deserialización-básica)
    - [5.5.2. Leer JSON desde Fichero](#552-leer-json-desde-fichero)
    - [5.5.3. Deserializar Listas](#553-deserializar-listas)
  - [5.6. Personalización: Mapeo de Nombres](#56-personalización-mapeo-de-nombres)
  - [5.7. Objetos Anidados y Jerarquías](#57-objetos-anidados-y-jerarquías)
  - [5.8. LINQ + JSON: Procesamiento Avanzado](#58-linq--json-procesamiento-avanzado)
  - [5.9. Manejo de Errores en JSON](#59-manejo-de-errores-en-json)
  - [5.10. Ejemplo Integrador: Sistema de Configuración JSON](#510-ejemplo-integrador-sistema-de-configuración-json)

# 5. Formatos de Intercambio (II): JSON

## 5.1. Introducción: ¿Qué es JSON y Por Qué es el Rey?

**JSON** (JavaScript Object Notation) es el formato de intercambio de datos más popular del mundo moderno. Es el estándar para APIs REST, configuración de aplicaciones y almacenamiento de datos.

> 📝 **Nota del Profesor**: JSON ha conquistado el mundo. Si vas a trabajar con APIs web (algo inevitable hoy en día), necesitas dominar JSON. Es más estructurado que CSV y más ligero que XML.

**¿Por qué JSON es tan popular?**

✅ **Legible**: Fácil de entender para humanos  
✅ **Universal**: Todas las lenguajes lo soportan  
✅ **Estructurado**: Soporta objetos y arrays  
✅ **Ligero**: Menor tamaño que XML  
✅ **Integración nativa**: JavaScript lo entiende nativamente  

## 5.2. Sintaxis Básica de JSON

```json
{
    "nombre": "Ana García",
    "edad": 20,
    "estaActiva": true,
    "notas": [8.5, 7.0, 9.2],
    "direccion": {
        "calle": "Gran Vía",
        "ciudad": "Madrid"
    },
    "telefonos": ["612345678", "698765432"]
}
```

**Tipos de datos JSON:**

| Tipo JSON | Ejemplo | C# equivalente |
|-----------|---------|-----------------|
| String | `"Hola"` | `string` |
| Number | `25`, `8.5` | `int`, `double` |
| Boolean | `true`, `false` | `bool` |
| Array | `[1, 2, 3]` | `List<T>` |
| Object | `{"a": 1}` | `class`, `record` |
| Null | `null` | `null` |

## 5.3. El Duelo: Newtonsoft.Json vs System.Text.Json

En .NET tenemos dos librerías principales para JSON:

| Característica | **System.Text.Json** | **Newtonsoft.Json** |
|---------------|---------------------|-------------------|
| Rendimiento | ✅ Más rápido | Más lento |
| Funciones | Básico | ✅ Completísimo |
| Comunidad | Oficial (.NET) | ✅ Maduro |
| Predeterminado | .NET 5+ | .NET Framework |

> 💡 **Tip del Examinador**: Para .NET 6+ se recomienda **System.Text.Json** por ser el estándar oficial. Pero Newtonsoft sigue siendo popular por sus funciones avanzadas.

## 5.4. Serialización: De Objeto a JSON

### 5.4.1. Serialización Básica

```csharp
using System.Text.Json;

public record Alumno(int Id, string Nombre, int Edad, double Nota);

var alumno = new Alumno(1, "Ana García", 20, 8.5);

// Serializar a JSON string
string json = JsonSerializer.Serialize(alumno);
Console.WriteLine(json);

// Salida: {"Id":1,"Nombre":"Ana García","Edad":20,"Nota":8.5}
```

### 5.4.2. Pretty Print (JSON Legible)

```csharp
var opciones = new JsonSerializerOptions 
{ 
    WriteIndented = true 
};

string jsonFormateado = JsonSerializer.Serialize(alumno, opciones);
Console.WriteLine(jsonFormateado);

/* Salida:
{
  "Id": 1,
  "Nombre": "Ana García",
  "Edad": 20,
  "Nota": 8.5
}
*/
```

### 5.4.3. Guardar JSON en Fichero

```csharp
var alumno = new Alumno(1, "Ana García", 20, 8.5);

var opciones = new JsonSerializerOptions { WriteIndented = true };
string json = JsonSerializer.Serialize(alumno, opciones);

// Guardar en fichero
File.WriteAllText("alumno.json", json);

Console.WriteLine("✓ JSON guardado en alumno.json");
```

### 5.4.4. Serializar Listas

```csharp
var alumnos = new List<Alumno>
{
    new(1, "Ana García", 20, 8.5),
    new(2, "Juan Pérez", 22, 7.0),
    new(3, "María López", 21, 9.2)
};

var opciones = new JsonSerializerOptions { WriteIndented = true };
string json = JsonSerializer.Serialize(alumnos, opciones);

File.WriteAllText("alumnos.json", json);
Console.WriteLine(json);

/* Salida:
[
  {
    "Id": 1,
    "Nombre": "Ana García",
    "Edad": 20,
    "Nota": 8.5
  },
  ...
]
*/
```

## 5.5. Deserialización: De JSON a Objeto

### 5.5.1. Deserialización Básica

```csharp
string json = "{\"Id\":1,\"Nombre\":\"Ana García\",\"Edad\":20,\"Nota\":8.5}";

// Deserializar a objeto
Alumno? alumno = JsonSerializer.Deserialize<Alumno>(json);

Console.WriteLine($"ID: {alumno?.Id}");
Console.WriteLine($"Nombre: {alumno?.Nombre}");
Console.WriteLine($"Nota: {alumno?.Nota}");
```

### 5.5.2. Leer JSON desde Fichero

```csharp
// Leer fichero JSON
string json = File.ReadAllText("alumno.json");

// Deserializar
Alumno? alumno = JsonSerializer.Deserialize<Alumno>(json);

Console.WriteLine($"✓ Leído: {alumno?.Nombre}");
```

### 📝 Nota del Profesor: Lectura Eficiente con Streams

Para ficheros JSON grandes, usar `File.ReadAllText` es ineficiente porque carga **todo el texto** en un string gigante en la RAM antes de empezar a procesarlo.

**La forma profesional (Streaming):**
Usamos un `FileStream` para que el serializador lea directamente los bytes del fichero.

```csharp
// ✓ MEJOR: Uso de File.OpenRead()
using var stream = File.OpenRead("alumnos.json");
var alumnos = JsonSerializer.Deserialize<List<Alumno>>(stream);
```

**Beneficios:**
1.  **Menos RAM:** No se crea el objeto `string` intermedio.
2.  **Más rápido:** Empieza a deserializar conforme los bytes llegan del disco.
3.  **Escalabilidad:** Vital para aplicaciones que manejan miles de datos.

### 5.5.3. Deserializar Listas

```csharp
string json = File.ReadAllText("alumnos.json");

// Deserializar a lista
List<Alumno>? alumnos = JsonSerializer.Deserialize<List<Alumno>>(json);

Console.WriteLine($"✓ Leídos {alumnos?.Count} alumnos");

foreach (var a in alunos!)
{
    Console.WriteLine($"  {a.Nombre}: {a.Nota}");
}
```

## 5.6. Personalización: Mapeo de Nombres

### Problema: Nombres Diferentes

```csharp
// Tu clase C# usa PascalCase
public record Alumno(int IdAlumno, string NombreCompleto);

// JSON usa camelCase
// {"idAlumno": 1, "nombreCompleto": "Ana"}
```

### Solución: Atributo JsonPropertyName

```csharp
using System.Text.Json.Serialization;

public record Alumno(
    [property: JsonPropertyName("id")] 
    int Id,
    
    [property: JsonPropertyName("nombre")] 
    string Nombre,
    
    [property: JsonPropertyName("edad")] 
    int Edad,
    
    [property: JsonPropertyName("nota")] 
    double Nota
);

// Ahora funciona con:
string json = "{\"id\":1,\"nombre\":\"Ana\",\"edad\":20,\"nota\":8.5}";
var a = JsonSerializer.Deserialize<Alumno>(json);
```

### Política de Nombres Global (camelCase)

```csharp
var opciones = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

// Ahora serializa con camelCase automáticamente
string json = JsonSerializer.Serialize(alumno, opciones);
// {"id":1,"nombre":"Ana García","edad":20,"nota":8.5}
```

## 5.7. Objetos Anidados y Jerarquías

```csharp
public record Direccion(string Calle, string Ciudad, string CP);
public record Alumno(int Id, string Nombre, Direccion Direccion, List<double> Notas);

var alumno = new Alumno(
    1, 
    "Ana García", 
    new Direccion("Gran Vía", "Madrid", "28013"),
    new List<double> { 8.5, 7.0, 9.2 }
);

var opciones = new JsonSerializerOptions { WriteIndented = true };
string json = JsonSerializer.Serialize(alumno, opciones);

Console.WriteLine(json);

/* Salida:
{
  "Id": 1,
  "Nombre": "Ana García",
  "Direccion": {
    "Calle": "Gran Vía",
    "Ciudad": "Madrid",
    "CP": "28013"
  },
  "Notas": [
    8.5,
    7,
    9.2
  ]
}
*/
```

## 5.8. LINQ + JSON: Procesamiento Avanzado

```csharp
string json = File.ReadAllText("alumnos.json");
var alumnos = JsonSerializer.Deserialize<List<Alumno>>(json)!;

// Filtrar aprobados
var aprobados = alunos.Where(a => a.Nota >= 5).OrderByDescending(a => a.Nota);

// Nota media
double media = alunos.Average(a => a.Nota);

// Mejores 3
var top3 = alunos.OrderByDescending(a => a.Nota).Take(3);

// Agrupar por ciudad
var porCiudad = alunos.GroupBy(a => a.Direccion.Ciudad);

Console.WriteLine($"Aprobados: {aprobados.Count()}");
Console.WriteLine($"Nota media: {media:F2}");
```

## 5.9. Manejo de Errores en JSON

### JSON Inválido

```csharp
try
{
    string jsonInvalido = "{ esto no es json";
    var obj = JsonSerializer.Deserialize<Alumno>(jsonInvalido);
}
catch (JsonException ex)
{
    Console.WriteLine($"Error JSON: {ex.Message}");
}
```

### Propiedades Faltantes

```csharp
public record Alumno(
    int Id, 
    string Nombre, 
    int Edad = 0,  // Valor por defecto
    double Nota = 0
);

// JSON con menos propiedades funciona si hay valores por defecto
string json = "{\"Id\":1,\"Nombre\":\"Ana\"}";
var a = JsonSerializer.Deserialize<Alumno>(json);
// Resultado: Id=1, Nombre="Ana", Edad=0, Nota=0
```

## 5.10. Ejemplo Integrador: Sistema de Configuración JSON

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;

// Configuración de la aplicación
public record AppConfig(
    string NombreApp,
    string Version,
    ConfigServidor Servidor,
    bool ModoDebug,
    List<string> Modulos
);

public record ConfigServidor(
    string Host,
    int Puerto,
    string Usuario,
    string Password
);

// Crear configuración
var config = new AppConfig(
    "MiAplicación",
    "1.0.0",
    new ConfigServidor("localhost", 8080, "admin", "password123"),
    true,
    new List<string> { "usuarios", "productos", "reportes" }
);

// Guardar
var opciones = new JsonSerializerOptions 
{ 
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

string json = JsonSerializer.Serialize(config, opciones);
File.WriteAllText("appsettings.json", json);

// Leer
string jsonLeido = File.ReadAllText("appsettings.json");
var configLeido = JsonSerializer.Deserialize<AppConfig>(jsonLeido, opciones);

Console.WriteLine($"App: {configLeido.NombreApp}");
Console.WriteLine($"Servidor: {configLeido.Servidor.Host}:{configLeido.Servidor.Puerto}");
```

> 📝 **Nota del Profesor**: JSON es el formato rey en el desarrollo moderno. APIs REST, archivos de configuración, intercambio de datos... todo es JSON. Domínalo.

> 💡 **Tip del Examinador**: En el examen pueden preguntar cómo serializar/deserializar objetos anidados o cómo manejar errores. Recuerda usar `try-catch` con `JsonException`.
