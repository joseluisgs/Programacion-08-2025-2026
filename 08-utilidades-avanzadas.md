- [8. Utilidades Avanzadas y Configuración](#8-utilidades-avanzadas-y-configuración)
  - [8.1. Ficheros Temporales](#81-ficheros-temporales)
    - [Directorio Temporal del Sistema](#directorio-temporal-del-sistema)
    - [Crear Fichero Temporal](#crear-fichero-temporal)
    - [Nombre Aleatorio sin Crear](#nombre-aleatorio-sin-crear)
  - [8.2. Compresión de Archivos (ZIP)](#82-compresión-de-archivos-zip)
    - [Comprimir Archivos](#comprimir-archivos)
    - [Extraer ZIP](#extraer-zip)
  - [8.3. Configuración de Aplicación con JSON](#83-configuración-de-aplicación-con-json)
    - [8.3.1. Método Básico: Parser Manual](#831-método-básico-parser-manual)
    - [8.3.2. Método Profesional: Microsoft.Extensions.Configuration](#832-método-profesional-microsoftextensionsconfiguration)
      - [1. Preparar el proyecto (.csproj)](#1-preparar-el-proyecto-csproj)
      - [2. Instalar las librerías (NuGet)](#2-instalar-las-librerías-nuget)
      - [3. Implementación del ConfigurationBuilder](#3-implementación-del-configurationbuilder)
    - [📝 Nota del Profesor: ¿Por qué usar la librería oficial vs Parser manual?](#-nota-del-profesor-por-qué-usar-la-librería-oficial-vs-parser-manual)
    - [Configuración por Entorno](#configuración-por-entorno)
  - [8.4. Directorios Especiales y Rutas Absolutas](#84-directorios-especiales-y-rutas-absolutas)

# 8. Utilidades Avanzadas y Configuración

## 8.1. Ficheros Temporales

Los ficheros temporales son esenciales para operaciones intermedias, descargas, procesamiento de datos, etc.

### Directorio Temporal del Sistema

```csharp
// Obtener directorio temporal
string tempPath = Path.GetTempPath();
Console.WriteLine($"Temporal: {tempPath}");
// Windows: C:\Users\Usuario\AppData\Local\Temp\
// Linux: /tmp/
```

### Crear Fichero Temporal

```csharp
// Crear nombre de fichero temporal único
string tempFile = Path.GetTempFileName();
Console.WriteLine($"Temp: {tempFile}");

// Escribir y usar
File.WriteAllText(tempFile, "Datos temporales");
var contenido = File.ReadAllText(tempFile);

// Eliminar
File.Delete(tempFile);
```

### Nombre Aleatorio sin Crear

```csharp
// Nombre aleatorio sin crear el fichero
string nombre = Path.Combine(
    Path.GetTempPath(),
    $"proceso_{Guid.NewGuid()}.tmp"
);
Console.WriteLine($"Nombre: {nombre}");
```

## 8.2. Compresión de Archivos (ZIP)

### Comprimir Archivos

```csharp
using System.IO.Compression;

string[] archivos = { "fichero1.txt", "fichero2.txt" };

// Crear ZIP
using var zip = ZipFile.Open("backup.zip", ZipArchiveMode.Create);

foreach (var archivo in archivos)
{
    zip.CreateEntryFromFile(archivo, Path.GetFileName(archivo));
}

Console.WriteLine("✓ ZIP creado");
```

### Extraer ZIP

```csharp
using System.IO.Compression;

// Extraer todo
ZipFile.ExtractToDirectory("backup.zip", "extraccion");

// Extraer uno específico
using var zip = ZipFile.OpenRead("backup.zip");
var entry = zip.GetEntry("fichero1.txt");
entry?.ExtractToFile("fichero1_extraido.txt");
```

## 8.3. Configuración de Aplicación con JSON

### 8.3.1. Método Básico: Parser Manual
Para aplicaciones muy simples, podemos leer el JSON como un string y deserializarlo a un objeto `record`.

```csharp
// Cargar
var json = File.ReadAllText("appsettings.json");
var config = JsonSerializer.Deserialize<Config>(json);
```

### 8.3.2. Método Profesional: Microsoft.Extensions.Configuration

En aplicaciones reales (ASP.NET Core, servicios, etc.), no se parsea el JSON a mano. Se utiliza el sistema de configuración oficial de .NET. Este método es más robusto, flexible y estándar en la industria. Con ello podemos leer configuraciones jerárquicas, mezclar fuentes (JSON, variables de entorno, argumentos de consola) y soportar recarga en caliente sin reiniciar la aplicación o tener que mapear manualmente cada sección o incluir ese código de lectura en cada parte del programa y tener que pasar ese objeto de configuración a cada parte del programa que lo necesite, lo que genera acoplamiento y código repetitivo o re compilar cada vez que se cambie la configuración.

#### 1. Preparar el proyecto (.csproj)
Para que el archivo `appsettings.json` esté disponible cuando ejecutemos el programa, debemos decirle a .NET que lo copie automáticamente al directorio de salida (`bin`). 

Editamos el fichero **.csproj** y añadimos:

```xml
<ItemGroup>
  <None Update="appsettings.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

Para parsearlo normalmente, el JSON debe tener una estructura clara, por ejemplo:

```json
{
    "AppName": "Mi Aplicación",
    "Version": "1.0.0",
    "Database": {
        "Host": "localhost",
        "Port": 5432,
        "User": "admin",
        "Password": "secret"
    }
}
```

Podríamos usar el serializador manual, pero es mucho más sencillo y profesional usar la librería oficial de configuración de .NET.

```csharp
public record DatabaseConfig(string Host, int Port, string User, string Password);
public record AppConfig(string AppName, string Version, DatabaseConfig Database);


var json = File.ReadAllText("appsettings.json");
var config = JsonSerializer.Deserialize<AppConfig>(json);
Console.WriteLine(config.AppName); // "Mi Aplicación"
Console.WriteLine(config.Database.Host); // "localhost"
```

Pero lo oficial es usar el `ConfigurationBuilder` que nos da muchas más funcionalidades y flexibilidad.

#### 2. Instalar las librerías (NuGet)
Necesitamos dos paquetes clave:
*   `Microsoft.Extensions.Configuration.Json`: Para leer ficheros JSON.
*   `Microsoft.Extensions.Configuration.Binder`: Para mapear el JSON a objetos C#.

#### 3. Implementación del ConfigurationBuilder
Este patrón permite leer la configuración de forma robusta y escalable.

```csharp
using Microsoft.Extensions.Configuration;

// 1. Construir la configuración
var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Directorio del ejecutable
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// 2. Leer valores individuales: Modo diccionario o jerárquico, tipando el resultado
string appName = config["AppName"]; // "Mi Aplicación", como si fuera un diccionario
int port = config.GetValue<int>("Database:Port"); // 5432 (acceso jerárquico con ":" para secciones anidadas)

// 3. Mapear a objetos fuertemente tipados
var appConfig = config.Get<AppConfig>();
Console.WriteLine(appConfig.AppName); // "Mi Aplicación"
Console.WriteLine(appConfig.Database.Host); // "localhost"

// 4. Mapear solo una sección a un objeto específico
var dbConfig = config.GetSection("Database").Get<DatabaseConfig>();
Console.WriteLine(dbConfig.Host); // "localhost"

// 5. Recarga en caliente (si el JSON cambia, se actualiza automáticamente sin reiniciar)
while (true)
{
    Console.WriteLine($"AppName: {config["AppName"]}, DB Host: {config["Database:Host"]}");
    Thread.Sleep(5000);
}


```

### 📝 Nota del Profesor: ¿Por qué usar la librería oficial vs Parser manual?

| Característica   | Parser Manual (JsonSerializer)               | Configuration Extensions                                                  |
| :--------------- | :------------------------------------------- | :------------------------------------------------------------------------ |
| **Complejidad**  | Tienes que gestionar la lectura del fichero. | El Builder lo gestiona todo.                                              |
| **Jerarquías**   | Difícil de navegar sin mapear todo.          | Acceso fácil por "Seccion:Clave".                                         |
| **Flexibilidad** | Solo lee JSON.                               | Puede mezclar JSON, Variables de Entorno, XML y Argumentos de consola.    |
| **Hot Reload**   | Tienes que reiniciar el programa.            | Puede detectar cambios en el JSON sin reiniciar (`reloadOnChange: true`). |
| **Estándar**     | No estándar.                                 | **Estándar de la industria en .NET.**                                     |

### Configuración por Entorno

Una de las grandes ventajas del sistema de configuración de .NET es que permite tener diferentes archivos de configuración para cada entorno (Desarrollo, Producción, etc.) y cargar el adecuado según la variable de entorno `ASPNETCORE_ENVIRONMENT` o `DOTNET_ENVIRONMENT`.

Por ejemplo, podríamos tener:

```csharp
// appsettings.Development.json
{
    "Logging": { "Level": "Debug" },
    "Database": { "Host": "localhost" }
}

// appsettings.Production.json
{
    "Logging": { "Level": "Warning" },
    "Database": { "Host": "production.db" }
}
```

De esta forma, al ejecutar en desarrollo se cargaría `appsettings.Development.json` y en producción `appsettings.Production.json`, permitiendo tener configuraciones específicas para cada entorno sin cambiar el código.

Para ello solo tenemos que añadir la línea `.AddJsonFile($"appsettings.{env}.json", optional: true)` al `ConfigurationBuilder` y asegurarnos de establecer la variable de entorno `ASPNETCORE_ENVIRONMENT` o `DOTNET_ENVIRONMENT` al ejecutar el programa.

```csharp
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .Build();
```

También, si queremos, podemos cargar ambos archivos (el general y el específico) y el sistema de configuración se encargará de mezclar las configuraciones, dando prioridad a las claves del archivo específico (por ejemplo, `appsettings.Production.json` sobre `appsettings.json`).

```csharp
var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
    .Build();
```

Finalmente podemos cambiar o elegir que configuración o perfil cargar al llamar a `dotnet run` con la variable de entorno:

```bash
# En Windows (PowerShell)
$env:ASPNETCORE_ENVIRONMENT="Development"; dotnet run
$env:ASPNETCORE_ENVIRONMENT="Production"; dotnet run
```

```bash
# En Linux/Mac
ASPNETCORE_ENVIRONMENT=Development dotnet run
ASPNETCORE_ENVIRONMENT=Production dotnet run
```

> 📝 **Nota del Profesor**: Estas utilidades son muy comunes en aplicaciones reales. Los ficheros temporales y ZIP se usan constantemente en operaciones de mantenimiento, y la configuración JSON es el estándar moderno.


## 8.4. Directorios Especiales y Rutas Absolutas

Con .NET, podemos acceder a directorios especiales del sistema de forma sencilla usando `Environment.GetFolderPath` junto con la enumeración `Environment.SpecialFolder`. Esto nos permite obtener rutas absolutas a carpetas como el escritorio, documentos, etc., sin preocuparnos por las diferencias entre sistemas operativos.

Gracias a esto, podemos escribir código que funcione tanto en Windows como en Linux o Mac sin tener que preocuparnos por las rutas específicas de cada sistema operativo. Además, también podemos obtener el directorio de ejecución o el directorio base de la aplicación para trabajar con archivos relativos a la ubicación del programa.

```csharp

// Directorio de Ejecución
string directorioEjecucion = Environment.CurrentDirectory;
Console.WriteLine($"Directorio de Ejecución: {directorioEjecucion}");

// Directorio Base
string directorioBase = AppDomain.CurrentDomain.BaseDirectory;
Console.WriteLine($"Directorio Base: {directorioBase}");

// Carpeta De Datos de Aplicación Local
string carpetaDatosLocal = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
Console.WriteLine($"Carpeta de Datos de Aplicación Local: {carpetaDatosLocal}");

// Carpeta De Datos de Aplicación Roaming
string carpetaDatosRoaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
Console.WriteLine($"Carpeta de Datos de Aplicación Roaming: {carpetaDatosRoaming}");

// Carpeta De Documentos
string carpetaDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
Console.WriteLine($"Carpeta de Documentos: {carpetaDocumentos}");

// Carpeta De Escritorio
string carpetaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
Console.WriteLine($"Carpeta de Escritorio: {carpetaEscritorio}");

// Carpeta del Usuario
string carpetaUsuario = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
Console.WriteLine($"Carpeta del Usuario: {carpetaUsuario}");

// Carpeta De Imágenes
string carpetaImagenes = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
Console.WriteLine($"Carpeta de Imágenes: {carpetaImagenes}");

// Carpeta De Descargas
string carpetaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads";
Console.WriteLine($"Carpeta de Descargas: {carpetaDescargas}");

// Carpeta De Música
string carpetaMusica = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
Console.WriteLine($"Carpeta de Música: {carpetaMusica}");

// Carpeta De Videos
string carpetaVideos = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
Console.WriteLine($"Carpeta de Videos: {carpetaVideos}");

// Carpeta De Programas (Archivos de Programa)
string carpetaProgramas = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
Console.WriteLine($"Carpeta de Programas: {carpetaProgramas}");

// Carpeta temporal
string carpetaTemporal = Path.GetTempPath();
Console.WriteLine($"Carpeta Temporal: {carpetaTemporal}");

// Menu Inicio
string carpetaMenuInicio = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
Console.WriteLine($"Carpeta de Menu Inicio: {carpetaMenuInicio}");
```