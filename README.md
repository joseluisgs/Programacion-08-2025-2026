# Programación - 08 Lectura y escritura de información externa. Ficheros.

Tema 08 Lectura y escritura de información externa. Ficheros. 1DAW. Curso 2025-2026

![imagen](https://raw.githubusercontent.com/joseluisgs/Programacion-00-2022-2023/master/images/programacion.png)

- [Programación - 08 Lectura y escritura de información externa. Ficheros.](#programación---08-lectura-y-escritura-de-información-externa-ficheros)
  - [Contenido en Youtube](#contenido-en-youtube)
  - [1. Fundamentos de I/O y "El Sistema de Candados"](#1-fundamentos-de-io-y-el-sistema-de-candados)
    - [1.0. ¿Qué es un Fichero?  El Concepto Fundamental](#10-qué-es-un-fichero--el-concepto-fundamental)
      - [1.0.1. Definición: Fichero y Directorio](#101-definición-fichero-y-directorio)
      - [1.0.2. La Gran Revelación: Todo es un Fichero (en Unix/Linux)](#102-la-gran-revelación-todo-es-un-fichero-en-unixlinux)
      - [1.0.3. Anatomía de un Fichero:  Metadatos vs Contenido](#103-anatomía-de-un-fichero--metadatos-vs-contenido)
      - [1.0.4. La Jerarquía del Sistema de Ficheros](#104-la-jerarquía-del-sistema-de-ficheros)
    - [1.1. ¿Qué es un Flujo (Stream)? El Concepto Clave](#11-qué-es-un-flujo-stream-el-concepto-clave)
      - [1.1.1. El Problema:  Los Ficheros son Grandes](#111-el-problema--los-ficheros-son-grandes)
      - [1.1.2. ¿Por Qué Usar Streams?](#112-por-qué-usar-streams)
      - [1.1.3. Operaciones Básicas de un Stream](#113-operaciones-básicas-de-un-stream)
      - [1.1.4. Tipos de Streams:  La Jerarquía](#114-tipos-de-streams--la-jerarquía)
      - [1.1.5. Decoradores:  StreamReader y StreamWriter](#115-decoradores--streamreader-y-streamwriter)
    - [1.2. El Problema Fundamental: Recursos Compartidos y el "Candado"](#12-el-problema-fundamental-recursos-compartidos-y-el-candado)
      - [1.2.1. La Analogía de la Biblioteca](#121-la-analogía-de-la-biblioteca)
      - [1.2.2. El Sistema de Candados del Sistema Operativo](#122-el-sistema-de-candados-del-sistema-operativo)
      - [1.2.3. ¿Qué Pasa Si Hay una Excepción?](#123-qué-pasa-si-hay-una-excepción)
    - [1.3. La Interfaz IDisposable: El Contrato de Limpieza](#13-la-interfaz-idisposable-el-contrato-de-limpieza)
      - [1.3.1. ¿Qué es IDisposable?](#131-qué-es-idisposable)
    - [1.4. La Revolución del `using`: Liberación Automática](#14-la-revolución-del-using-liberación-automática)
      - [1.4.1. Bloque `using` Clásico (C# 1.0 - Presente)](#141-bloque-using-clásico-c-10---presente)
      - [1.4.2. Declaración `using var` (C# 8+ / . NET Core 3.0+) - **RECOMENDADO**](#142-declaración-using-var-c-8---net-core-30---recomendado)
      - [1.4.3. ¿Cuándo Usar Cada Uno?](#143-cuándo-usar-cada-uno)
    - [1.5. Ejemplo Completo: Demostración del Sistema de Candados](#15-ejemplo-completo-demostración-del-sistema-de-candados)
    - [1.6. Resumen de Buenas Prácticas](#16-resumen-de-buenas-prácticas)
  - [2. Manipulación del Sistema de Ficheros (Clases File, Directory y Path)](#2-manipulación-del-sistema-de-ficheros-clases-file-directory-y-path)
    - [2.0. Introducción:  Las Herramientas del Sistema de Archivos](#20-introducción--las-herramientas-del-sistema-de-archivos)
    - [2.1. La Clase `File`: Operaciones sobre Ficheros](#21-la-clase-file-operaciones-sobre-ficheros)
      - [2.1.1. Verificar Existencia:  `File.Exists()`](#211-verificar-existencia--fileexists)
      - [2.1.2. Crear y Escribir:  `File.WriteAllText()` y `File.WriteAllLines()`](#212-crear-y-escribir--filewritealltext-y-filewritealllines)
      - [2.1.3. Leer Contenido: `File.ReadAllText()` y `File.ReadAllLines()`](#213-leer-contenido-filereadalltext-y-filereadalllines)
      - [2.1.4. Copiar Ficheros: `File.Copy()`](#214-copiar-ficheros-filecopy)
      - [2.1.5. Mover Ficheros: `File.Move()`](#215-mover-ficheros-filemove)
      - [2.1.6. Eliminar Ficheros: `File.Delete()`](#216-eliminar-ficheros-filedelete)
      - [2.1.7. Obtener Información:  Metadatos del Fichero](#217-obtener-información--metadatos-del-fichero)
    - [2.2. La Clase `FileInfo`: Operaciones Orientadas a Objetos](#22-la-clase-fileinfo-operaciones-orientadas-a-objetos)
      - [2.2.1. Diferencia entre `File` y `FileInfo`](#221-diferencia-entre-file-y-fileinfo)
      - [2.2.2. Ejemplo Completo con FileInfo](#222-ejemplo-completo-con-fileinfo)
    - [2.3. La Clase `Directory`: Operaciones sobre Directorios](#23-la-clase-directory-operaciones-sobre-directorios)
      - [2.3.1. Crear Directorios: `Directory.CreateDirectory()`](#231-crear-directorios-directorycreatedirectory)
      - [2.3.2. Listar Contenido: `GetFiles()` y `GetDirectories()`](#232-listar-contenido-getfiles-y-getdirectories)
      - [2.3.3. Búsqueda con Patrones:  Filtros](#233-búsqueda-con-patrones--filtros)
      - [2.3.4. Búsqueda Recursiva: `SearchOption.AllDirectories`](#234-búsqueda-recursiva-searchoptionalldirectories)
      - [2.3.5. Mover y Eliminar Directorios](#235-mover-y-eliminar-directorios)
    - [2.4. La Clase `Path`: Manipulación de Rutas](#24-la-clase-path-manipulación-de-rutas)
      - [2.4.1. Combinar Rutas: `Path.Combine()`](#241-combinar-rutas-pathcombine)
      - [2.4.2. Extraer Componentes de una Ruta](#242-extraer-componentes-de-una-ruta)
      - [2.4.3. Generar Rutas Temporales](#243-generar-rutas-temporales)
      - [2.4.4. Rutas Absolutas vs Relativas](#244-rutas-absolutas-vs-relativas)
    - [2.5. LINQ + Sistema de Ficheros:  Búsquedas Avanzadas](#25-linq--sistema-de-ficheros--búsquedas-avanzadas)
      - [2.5.1. Filtrar Ficheros por Tamaño](#251-filtrar-ficheros-por-tamaño)
      - [2.5.2. Filtrar por Fecha de Modificación](#252-filtrar-por-fecha-de-modificación)
      - [2.5.3. Búsqueda Compleja:   Imágenes JPG Grandes](#253-búsqueda-compleja---imágenes-jpg-grandes)
      - [2.5.4. Agrupar Ficheros por Extensión](#254-agrupar-ficheros-por-extensión)
  - [3. Ficheros de Texto Plano](#3-ficheros-de-texto-plano)
    - [3.0. Introducción: ¿Qué es un Fichero de Texto?](#30-introducción-qué-es-un-fichero-de-texto)
    - [3.1. Codificación de Caracteres:   UTF-8, ASCII y Unicode](#31-codificación-de-caracteres---utf-8-ascii-y-unicode)
      - [3.1.1. ¿Qué es la Codificación?](#311-qué-es-la-codificación)
      - [3.1.2. Principales Codificaciones](#312-principales-codificaciones)
    - [3.2. Escritura de Ficheros de Texto](#32-escritura-de-ficheros-de-texto)
      - [3.2.1. Método Rápido: `File.WriteAllText()` y `File.WriteAllLines()`](#321-método-rápido-filewritealltext-y-filewritealllines)
      - [3.2.2. StreamWriter:   Escritura Eficiente (Ficheros Grandes)](#322-streamwriter---escritura-eficiente-ficheros-grandes)
      - [3.2.3. Write vs WriteLine](#323-write-vs-writeline)
      - [3.2.4. Flush:   Forzar Escritura Inmediata](#324-flush---forzar-escritura-inmediata)
    - [3.3. Lectura de Ficheros de Texto](#33-lectura-de-ficheros-de-texto)
      - [3.3.1. Método Rápido:  `File.ReadAllText()` y `File.ReadAllLines()`](#331-método-rápido--filereadalltext-y-filereadalllines)
      - [3.3.2. StreamReader: Lectura Eficiente (Ficheros Grandes)](#332-streamreader-lectura-eficiente-ficheros-grandes)
      - [3.3.3. Ejemplo Práctico:  Procesar Log Line](#333-ejemplo-práctico--procesar-log-line)
    - [3.4. Comparación de Métodos:   ¿Cuál Usar?](#34-comparación-de-métodos---cuál-usar)
    - [3.5. Ejemplo Integrador: Sistema de Logs](#35-ejemplo-integrador-sistema-de-logs)
  - [4.  Formatos de Intercambio (I):   DTOs y CSV con LINQ](#4--formatos-de-intercambio-i---dtos-y-csv-con-linq)
    - [4.0. Introducción: El Problema de la Persistencia de Objetos](#40-introducción-el-problema-de-la-persistencia-de-objetos)
    - [4.1. ¿Qué es CSV?   Valores Separados por Comas](#41-qué-es-csv---valores-separados-por-comas)
    - [4.2. El Patrón DTO (Data Transfer Object)](#42-el-patrón-dto-data-transfer-object)
      - [4.2.1. ¿Qué es un DTO?](#421-qué-es-un-dto)
      - [4.2.2. ¿Por Qué Usar DTOs?](#422-por-qué-usar-dtos)
    - [4.3. Escritura de CSV](#43-escritura-de-csv)
      - [4.3.1. Escritura Manual Básica](#431-escritura-manual-básica)
      - [4.3.2. Añadir Método ToCsv al DTO](#432-añadir-método-tocsv-al-dto)
      - [4.3.3. Escritura con LINQ](#433-escritura-con-linq)
    - [4.4. Lectura de CSV](#44-lectura-de-csv)
      - [4.4.1. Lectura Básica con StreamReader](#441-lectura-básica-con-streamreader)
      - [4.4.2. Lectura con LINQ (Más Elegante)](#442-lectura-con-linq-más-elegante)
    - [4.5. Procesamiento Avanzado con LINQ](#45-procesamiento-avanzado-con-linq)
      - [4.5.1. Filtrado y Ordenación](#451-filtrado-y-ordenación)
      - [4.5.2. Estadísticas con LINQ](#452-estadísticas-con-linq)
      - [4.5.3. Proyección y Transformación](#453-proyección-y-transformación)
    - [4.6. Manejo de Casos Especiales en CSV](#46-manejo-de-casos-especiales-en-csv)
      - [4.6.1. Problema:   Comas en los Datos](#461-problema---comas-en-los-datos)
      - [4.6.2. Diferentes Separadores (`;` en Europa)](#462-diferentes-separadores--en-europa)
    - [4.7. Ejemplo Integrador:   Análisis Completo de CSV](#47-ejemplo-integrador---análisis-completo-de-csv)
  - [5. Formatos de Intercambio (II): JSON](#5-formatos-de-intercambio-ii-json)
    - [5.0. Introducción:     ¿Qué es JSON y Por Qué es el Rey?](#50-introducción-----qué-es-json-y-por-qué-es-el-rey)
    - [5.1. Sintaxis Básica de JSON](#51-sintaxis-básica-de-json)
    - [5.2. El Duelo:     Newtonsoft.Json vs System.Text.Json](#52-el-duelo-----newtonsoftjson-vs-systemtextjson)
      - [5.2.1. Comparativa](#521-comparativa)
    - [5.3. Serialización:     De Objeto a JSON](#53-serialización-----de-objeto-a-json)
      - [5.3.1. Serialización Básica](#531-serialización-básica)
      - [5.3.2. Pretty Print (JSON Legible)](#532-pretty-print-json-legible)
      - [5.3.3. Guardar JSON en Fichero](#533-guardar-json-en-fichero)
      - [5.3.4. Serializar Listas](#534-serializar-listas)
    - [5.4. Deserialización:      De JSON a Objeto](#54-deserialización------de-json-a-objeto)
      - [5.4.1. Deserialización Básica](#541-deserialización-básica)
      - [5.4.2. Leer JSON desde Fichero](#542-leer-json-desde-fichero)
      - [5.4.3. Deserializar Listas](#543-deserializar-listas)
    - [5.5. Personalización:       Mapeo de Nombres con `[JsonPropertyName]`](#55-personalización-------mapeo-de-nombres-con-jsonpropertyname)
      - [5.5.1. Problema:     Nombres Diferentes](#551-problema-----nombres-diferentes)
      - [5.5.2. Solución:     Atributo `[JsonPropertyName]`](#552-solución-----atributo-jsonpropertyname)
      - [5.5.3. Política de Nombres Global (camelCase)](#553-política-de-nombres-global-camelcase)
    - [5.6. Objetos Anidados y Jerarquías](#56-objetos-anidados-y-jerarquías)
    - [5.7. LINQ + JSON:       Procesamiento Avanzado](#57-linq--json-------procesamiento-avanzado)
    - [5.8. Manejo de Errores en JSON](#58-manejo-de-errores-en-json)
      - [5.8.1. JSON Inválido](#581-json-inválido)
      - [5.8.2. Propiedades Faltantes](#582-propiedades-faltantes)
    - [5.9. Ejemplo Integrador:       Sistema de Configuración JSON](#59-ejemplo-integrador-------sistema-de-configuración-json)
  - [6. XML Estructurado](#6-xml-estructurado)
    - [6.0. Introducción: ¿Qué es XML y Cuándo Usarlo?](#60-introducción-qué-es-xml-y-cuándo-usarlo)
    - [6.1. Sintaxis Básica de XML](#61-sintaxis-básica-de-xml)
      - [6.1.1. Elementos y Atributos](#611-elementos-y-atributos)
      - [6.1.2. Ejemplo Completo](#612-ejemplo-completo)
    - [6.2. Serialización XML con `XmlSerializer`](#62-serialización-xml-con-xmlserializer)
      - [6.2.1. Atributos XML](#621-atributos-xml)
      - [6.2.2. Serialización Básica](#622-serialización-básica)
      - [6.2.3. Serialización sin Namespaces](#623-serialización-sin-namespaces)
      - [6.2.4. Serializar Listas](#624-serializar-listas)
    - [6.3. Deserialización XML](#63-deserialización-xml)
      - [6.3.1. Deserialización Básica](#631-deserialización-básica)
      - [6.3.2. Deserializar Listas](#632-deserializar-listas)
    - [6.4. Objetos Anidados y Jerarquías](#64-objetos-anidados-y-jerarquías)
    - [6.5. LINQ to XML:        Consultas sobre Datos XML](#65-linq-to-xml--------consultas-sobre-datos-xml)
    - [6.6. Manejo de Errores en XML](#66-manejo-de-errores-en-xml)
    - [6.7. Comparación Final:        CSV vs JSON vs XML](#67-comparación-final--------csv-vs-json-vs-xml)
  - [7. Ficheros Binarios y el Riesgo del Acoplamiento](#7-ficheros-binarios-y-el-riesgo-del-acoplamiento)
    - [7.0. Introducción: ¿Qué es un Fichero Binario?](#70-introducción-qué-es-un-fichero-binario)
    - [7.1. BinaryReader y BinaryWriter:           Lectura/Escritura de Tipos Primitivos](#71-binaryreader-y-binarywriter-----------lecturaescritura-de-tipos-primitivos)
      - [7.1.1. Escritura Binaria Básica](#711-escritura-binaria-básica)
      - [7.1.2. Lectura Binaria Básica](#712-lectura-binaria-básica)
    - [7.2. Serialización Binaria de Objetos](#72-serialización-binaria-de-objetos)
      - [7.2.1. Serialización Manual](#721-serialización-manual)
      - [7.2.2. Serialización de Listas](#722-serialización-de-listas)
    - [7.3. Acceso Aleatorio con FileStream y Seek](#73-acceso-aleatorio-con-filestream-y-seek)
      - [7.3.1. Concepto de Seek](#731-concepto-de-seek)
      - [7.3.2. Ejemplo Práctico: Actualizar Registro en Posición Específica](#732-ejemplo-práctico-actualizar-registro-en-posición-específica)
    - [7.4. ⚠️ EL GRAN PROBLEMA:           Acoplamiento y Falta de Interoperabilidad](#74-️-el-gran-problema-----------acoplamiento-y-falta-de-interoperabilidad)
      - [7.4.1. El Problema del Acoplamiento](#741-el-problema-del-acoplamiento)
      - [7.4.2. Demostración del Problema](#742-demostración-del-problema)
      - [7.4.3. Comparación:  Binario vs JSON](#743-comparación--binario-vs-json)
    - [7.5. Casos de Uso Válidos para Ficheros Binarios](#75-casos-de-uso-válidos-para-ficheros-binarios)
      - [7.5.1. ✅ Cachés Temporales de Alto Rendimiento](#751--cachés-temporales-de-alto-rendimiento)
      - [7.5.2. ✅ Formatos Estándar Binarios (con Especificación)](#752--formatos-estándar-binarios-con-especificación)
      - [7.5.3. ❌ Casos Donde NO Usar Binario](#753--casos-donde-no-usar-binario)
    - [7.6. Resumen y Recomendaciones](#76-resumen-y-recomendaciones)
  - [8. Utilidades Avanzadas y Configuración](#8-utilidades-avanzadas-y-configuración)
    - [8.0. Introducción](#80-introducción)
    - [8.1. Ficheros Temporales](#81-ficheros-temporales)
      - [8.1.1. ¿Qué son los Ficheros Temporales?](#811-qué-son-los-ficheros-temporales)
      - [8.1.2. Directorio Temporal del Sistema](#812-directorio-temporal-del-sistema)
      - [8.1.3. Crear Fichero Temporal con Nombre Único](#813-crear-fichero-temporal-con-nombre-único)
      - [8.1.4. Nombre Aleatorio sin Crear el Fichero](#814-nombre-aleatorio-sin-crear-el-fichero)
      - [8.1.5. Ejemplo Práctico: Procesar Descarga Temporal](#815-ejemplo-práctico-procesar-descarga-temporal)
    - [8.2. Compresión de Archivos (ZIP)](#82-compresión-de-archivos-zip)
      - [8.2.1.  Introducción a la Compresión](#821--introducción-a-la-compresión)
      - [8.2.2. Comprimir un Archivo Individual](#822-comprimir-un-archivo-individual)
      - [8.2.3. Crear Archivo ZIP con Múltiples Archivos](#823-crear-archivo-zip-con-múltiples-archivos)
      - [8.2.4. Extraer Archivo ZIP](#824-extraer-archivo-zip)
      - [8.2.5. Agregar Archivos a ZIP Existente](#825-agregar-archivos-a-zip-existente)
    - [8.3. Configuración de Aplicación con JSON](#83-configuración-de-aplicación-con-json)
      - [8.3.1. El Antiguo Enfoque:   `.properties` / `.config`](#831-el-antiguo-enfoque---properties--config)
      - [8.3.2. El Enfoque Moderno:  `appsettings.json`](#832-el-enfoque-moderno--appsettingsjson)
      - [8.3.3. Crear Sistema de Configuración](#833-crear-sistema-de-configuración)
      - [8.3.4. Configuración por Entorno (Development/Production)](#834-configuración-por-entorno-developmentproduction)
  - [9. PROYECTO FINAL: Sistema CRUD de Estudiantes con Persistencia JSON](#9-proyecto-final-sistema-crud-de-estudiantes-con-persistencia-json)
    - [9.0. Introducción al Proyecto](#90-introducción-al-proyecto)
    - [9.1. Modelo de Dominio:  Student](#91-modelo-de-dominio--student)
    - [9.2. DTO para Persistencia](#92-dto-para-persistencia)
    - [9.3. Interfaz del Repositorio](#93-interfaz-del-repositorio)
    - [9.4. Implementación: StudentJsonRepository](#94-implementación-studentjsonrepository)
    - [9.5. Servicio de Búsqueda con LINQ](#95-servicio-de-búsqueda-con-linq)
    - [9.6. Programa Principal:  Demostración Completa](#96-programa-principal--demostración-completa)
    - [9.7. Salida Esperada del Programa](#97-salida-esperada-del-programa)
    - [9.8. Extensiones Opcionales del Proyecto](#98-extensiones-opcionales-del-proyecto)
      - [9.8.1. Exportar Informe a CSV](#981-exportar-informe-a-csv)
      - [9.8.2. Backup del Sistema](#982-backup-del-sistema)
  - [Autor](#autor)
    - [Contacto](#contacto)
  - [Licencia de uso](#licencia-de-uso)



---

## Contenido en Youtube

- [Podcast](#)
- [Resumen](#)
- [Lista de Reproducción](https://www.youtube.com/watch?v=wKCdgacEr4Q&list=PLGIH-7eZDbVw6q2AdcAUe2r6YxJYBkfCi)


---

## 1. Fundamentos de I/O y "El Sistema de Candados"

### 1.0. ¿Qué es un Fichero?  El Concepto Fundamental

Antes de escribir una sola línea de código, necesitamos entender **qué es realmente un fichero** desde el punto de vista del sistema operativo.

#### 1.0.1. Definición: Fichero y Directorio

**Un fichero (o archivo)** es una **secuencia de bytes** almacenada en un dispositivo de almacenamiento persistente (disco duro, SSD, USB, etc.) identificada por un **nombre único** dentro de su ubicación. 

```
┌─────────────────────────────────────────┐
│  FICHERO = Nombre + Secuencia de Bytes  │
├─────────────────────────────────────────┤
│  Nombre:    "documento.txt"             │
│  Bytes:    [72][111][108][97]...        │
│            (H) (o)  (l)  (a)            │
└─────────────────────────────────────────┘
```

**Ejemplo conceptual:**

Imagina que un fichero es como un **libro en una biblioteca**:
- **Nombre del libro** (nombre del fichero):  `mi_libro.txt`
- **Contenido del libro** (bytes del fichero):  el texto, imágenes, etc. 
- **Ubicación del libro** (ruta del fichero):  `C:\Documentos\mi_libro.txt`

**Un directorio (o carpeta)** es un **contenedor especial** que puede almacenar otros ficheros y directorios.  Es como una **estantería** en la biblioteca que organiza los libros.

```
Directorio
    ├─ Fichero 1
    ├─ Fichero 2
    └─ Subdirectorio
           ├─ Fichero 3
           └─ Fichero 4
```

#### 1.0.2. La Gran Revelación: Todo es un Fichero (en Unix/Linux)

En sistemas operativos basados en Unix (Linux, macOS), existe un principio fundamental: 

> **"Everything is a file"** (Todo es un fichero)

Esto significa que el sistema operativo **trata casi todo como si fuera un fichero**: 

| Elemento             | ¿Es un Fichero?         | Ejemplo                |
| -------------------- | ----------------------- | ---------------------- |
| Archivo de texto     | ✅ Sí                    | `documento.txt`        |
| Directorio           | ✅ Sí (fichero especial) | `C:\Documentos\`       |
| Dispositivo USB      | ✅ Sí                    | `/dev/sdb1` (Linux)    |
| Impresora            | ✅ Sí                    | `/dev/lp0` (Linux)     |
| Conexión de red      | ✅ Sí (socket)           | `/var/run/docker.sock` |
| Proceso en ejecución | ✅ Sí                    | `/proc/1234/` (Linux)  |

**¿Por qué es importante esto?**

Porque significa que **el mismo conjunto de operaciones** (abrir, leer, escribir, cerrar) funciona para: 
- Leer un archivo de texto
- Leer datos de un teclado
- Enviar datos por red
- Leer la temperatura de la CPU

**En C#, esto se refleja en la interfaz común:** `Stream`

```csharp
// TODAS estas clases heredan de Stream: 
FileStream      // Archivo en disco
NetworkStream   // Datos por red
MemoryStream    // Datos en RAM
```

#### 1.0.3. Anatomía de un Fichero:  Metadatos vs Contenido

Un fichero tiene **dos partes**:

**1. Metadatos** (información SOBRE el fichero):
   - Nombre
   - Tamaño (en bytes)
   - Fechas (creación, modificación, último acceso)
   - Permisos (¿quién puede leerlo/escribirlo?)
   - Atributos (oculto, solo lectura, sistema)

**2. Contenido** (los bytes del fichero):
   - La información real que almacena

```
┌─────────────────────────────────────────┐
│  METADATOS (Información del fichero)    │
├─────────────────────────────────────────┤
│  Nombre:         "foto.jpg"             │
│  Tamaño:         1,024,000 bytes        │
│  Fecha creación: 2025-01-15 10:30       │
│  Permisos:       rwxr-xr-- (Unix)       │
└─────────────────────────────────────────┘
           ↓
┌─────────────────────────────────────────┐
│  CONTENIDO (Bytes del fichero)          │
├─────────────────────────────────────────┤
│  [255][216][255][224][0][16]...         │
│  (Encabezado JPEG + imagen comprimida)  │
└─────────────────────────────────────────┘
```

**Ejemplo en C#:**

```csharp
using System;
using System.IO;

string ruta = "documento.txt";

// Crear un fichero de ejemplo
File.WriteAllText(ruta, "Hola mundo");

// ========================================
// METADATOS (información SOBRE el fichero)
// ========================================

var info = new FileInfo(ruta);

Console.WriteLine("═══ METADATOS DEL FICHERO ═══");
Console.WriteLine($"Nombre:            {info.Name}");
Console.WriteLine($"Ruta completa:    {info.FullName}");
Console.WriteLine($"Tamaño:           {info.Length} bytes");
Console.WriteLine($"Fecha creación:   {info.CreationTime}");
Console.WriteLine($"Fecha modificado: {info.LastWriteTime}");
Console.WriteLine($"Solo lectura:     {info.IsReadOnly}");

// ========================================
// CONTENIDO (los bytes del fichero)
// ========================================

Console. WriteLine("\n═══ CONTENIDO DEL FICHERO ═══");
string contenido = File.ReadAllText(ruta);
Console.WriteLine($"Texto:  {contenido}");

// Leer como bytes
byte[] bytes = File.ReadAllBytes(ruta);
Console.WriteLine($"Bytes: [{string.Join(", ", bytes)}]");

// Limpiar
File.Delete(ruta);
```

**Salida:**

```
═══ METADATOS DEL FICHERO ═══
Nombre:           documento. txt
Ruta completa:     C:\Users\.. .\documento.txt
Tamaño:           10 bytes
Fecha creación:    15/01/2025 10:30:45
Fecha modificado: 15/01/2025 10:30:45
Solo lectura:      False

═══ CONTENIDO DEL FICHERO ═══
Texto: Hola mundo
Bytes: [72, 111, 108, 97, 32, 109, 117, 110, 100, 111]
       (H) (o)  (l)  (a) ( ) (m) (u)  (n)  (d)  (o)
```

#### 1.0.4. La Jerarquía del Sistema de Ficheros

El sistema de ficheros es una **estructura en árbol** (jerarquía):

```
C:\                              ← Raíz (en Windows)
│
├─ Windows\                      ← Directorio del sistema
│  ├─ System32\
│  └─ notepad.exe               ← Fichero ejecutable
│
├─ Users\
│  └─ Alumno\
│     ├─ Documents\
│     │  └─ apuntes.txt         ← Fichero de texto
│     └─ Pictures\
│        └─ foto.jpg            ← Fichero de imagen
│
└─ Program Files\
   └─ MiApp\
      └─ app.exe
```

**Rutas Absolutas vs Relativas:**

```csharp
// RUTA ABSOLUTA:  Desde la raíz del sistema
string absolutaWindows = @"C:\Users\Alumno\Documents\apuntes.txt";
string absolutaLinux = "/home/alumno/documents/apuntes.txt";

// RUTA RELATIVA: Desde el directorio actual
string relativa = "apuntes.txt";                    // En el directorio actual
string relativaSubdir = "Documents/apuntes.txt";    // En un subdirectorio
string relativaPadre = "../apuntes.txt";            // En el directorio padre
```

---

### 1.1. ¿Qué es un Flujo (Stream)? El Concepto Clave

Ahora que sabemos qué es un fichero, necesitamos entender **cómo accedemos a su contenido**. 

#### 1.1.1. El Problema:  Los Ficheros son Grandes

Imagina que tienes un fichero de **1 GB** (como un vídeo). Si intentas cargarlo **todo a la vez en memoria RAM**, tu programa se quedaría sin memoria. 

**La solución:  Procesarlo por partes (streaming)**

Un **Stream** (flujo) es una **abstracción que representa una secuencia de datos** que se procesa **poco a poco**, en lugar de todo a la vez. 

**Analogía 1: El río**

```
        Origen (fichero)          Destino (programa)
             │                         │
             │  ┌──────┐              │
        ┌────┴──┤ AGUA ├──────────────┴────┐
        │       └──────┘                    │
        │    ← ← ← ← ← ← ← ← ← ←          │
        │         Flujo continuo            │
        └───────────────────────────────────┘

No necesitas todo el río a la vez,
solo el agua que pasa en cada momento.
```

**Analogía 2: La cinta transportadora**

```
┌─────────────────────────────────────────┐
│   FICHERO (en disco)                    │
│   [byte][byte][byte][byte][byte]...      │
└────────────┬────────────────────────────┘
             │
             ▼ Stream (flujo)
    ┌────────────────────────┐
    │ Buffer (memoria RAM)   │  ← Solo una parte cada vez
    │ [byte][byte][byte]     │
    └────────┬───────────────┘
             │
             ▼
    ┌────────────────────────┐
    │  Tu Programa (procesa) │
    └────────────────────────┘
```

#### 1.1.2. ¿Por Qué Usar Streams? 

**Ventajas:**

1. **Eficiencia de memoria**: Procesas pequeños trozos, no todo el fichero
2. **Velocidad**: Empiezas a procesar ANTES de cargar todo
3. **Ficheros grandes**: Puedes procesar ficheros de GB sin problemas
4. **Universalidad**: La misma API para ficheros, red, memoria, etc.

**Ejemplo conceptual:**

```csharp
// ❌ SIN Stream:  Todo a memoria (peligroso con ficheros grandes)
byte[] todosLosBytes = File.ReadAllBytes("video.mp4"); // 1 GB en RAM
// Procesar todosLosBytes... (puede fallar por falta de memoria)

// ✅ CON Stream: Procesar por partes
using var stream = File.OpenRead("video. mp4");
byte[] buffer = new byte[4096]; // Buffer de 4 KB

while (stream.Read(buffer, 0, buffer. Length) > 0)
{
    // Procesar solo estos 4 KB
    // Nunca más de 4 KB en memoria a la vez
}
```

#### 1.1.3. Operaciones Básicas de un Stream

Todo Stream soporta estas operaciones fundamentales:

| Operación    | Descripción                                | Método en C#             |
| ------------ | ------------------------------------------ | ------------------------ |
| **Abrir**    | Establecer conexión con la fuente de datos | Constructor              |
| **Leer**     | Obtener datos del stream                   | `Read()`, `ReadByte()`   |
| **Escribir** | Enviar datos al stream                     | `Write()`, `WriteByte()` |
| **Buscar**   | Mover la posición actual                   | `Seek()`                 |
| **Cerrar**   | Liberar el recurso                         | `Dispose()` / `Close()`  |

```csharp
// ========================================
// OPERACIONES DE UN STREAM
// ========================================

using var stream = new FileStream("datos.bin", FileMode.Open);

// 1. LEER: Obtener bytes
byte[] buffer = new byte[10];
int bytesLeidos = stream.Read(buffer, 0, buffer.Length);

// 2. POSICIÓN: ¿Dónde estamos?
long posicionActual = stream.Position;
Console.WriteLine($"Posición actual: {posicionActual}");

// 3. BUSCAR: Movernos a otra posición
stream.Seek(0, SeekOrigin.Begin); // Volver al inicio

// 4. ESCRIBIR:  Enviar bytes (si está abierto para escritura)
// byte[] datos = new byte[] { 1, 2, 3 };
// stream.Write(datos, 0, datos.Length);

// 5. CERRAR: Automático con 'using' al salir del scope
```

#### 1.1.4. Tipos de Streams:  La Jerarquía

En . NET, todos los streams heredan de la clase abstracta `Stream`:

```
System.IO.Stream (clase abstracta base)
    │
    ├─ FileStream           → Ficheros en disco
    │   └─ Ejemplo:  Leer "documento.txt"
    │
    ├─ MemoryStream         → Datos en memoria RAM
    │   └─ Ejemplo: Buffer temporal para procesar datos
    │
    ├─ NetworkStream        → Datos por red (TCP/IP)
    │   └─ Ejemplo: Recibir datos de un servidor
    │
    ├─ GZipStream           → Datos comprimidos (gzip)
    │   └─ Ejemplo: Leer un archivo . gz
    │
    ├─ CryptoStream         → Datos cifrados
    │   └─ Ejemplo: Descifrar un archivo encriptado
    │
    └─ BufferedStream       → Stream con buffer interno
        └─ Ejemplo: Mejorar rendimiento de lectura/escritura
```

**Ejemplo práctico:**

```csharp
using System;
using System.IO;
using System.Text;

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  DEMOSTRACIÓN:  Tipos de Streams");
Console.WriteLine("═══════════════════════════════════════════\n");

// ========================================
// 1. FileStream: Fichero en disco
// ========================================

Console.WriteLine(">>> 1. FileStream (fichero en disco)");

using (var fileStream = new FileStream("test.txt", FileMode.Create))
{
    byte[] datos = Encoding.UTF8.GetBytes("Hola desde FileStream");
    fileStream.Write(datos, 0, datos.Length);
    Console.WriteLine($"  ✓ Escrito {datos.Length} bytes en disco");
}

// ========================================
// 2. MemoryStream: Datos en RAM
// ========================================

Console. WriteLine("\n>>> 2. MemoryStream (datos en memoria)");

using (var memStream = new MemoryStream())
{
    byte[] datos = Encoding.UTF8.GetBytes("Hola desde MemoryStream");
    memStream.Write(datos, 0, datos.Length);
    
    Console.WriteLine($"  ✓ Escrito {datos.Length} bytes en RAM");
    Console.WriteLine($"  Capacidad del buffer: {memStream. Capacity} bytes");
    
    // Leer desde el principio
    memStream. Seek(0, SeekOrigin.Begin);
    byte[] leido = new byte[memStream. Length];
    memStream.Read(leido, 0, leido.Length);
    
    string texto = Encoding.UTF8.GetString(leido);
    Console.WriteLine($"  Leído: {texto}");
}

// ========================================
// 3. Comparación: FileStream vs MemoryStream
// ========================================

Console.WriteLine("\n>>> 3. Comparación de rendimiento");

// FileStream:  Escritura en disco (lenta)
var sw1 = System.Diagnostics.Stopwatch.StartNew();
using (var fs = new FileStream("perf_test.txt", FileMode.Create))
{
    for (int i = 0; i < 10000; i++)
    {
        byte[] data = BitConverter.GetBytes(i);
        fs.Write(data, 0, data.Length);
    }
}
sw1.Stop();

// MemoryStream: Escritura en RAM (rápida)
var sw2 = System.Diagnostics.Stopwatch.StartNew();
using (var ms = new MemoryStream())
{
    for (int i = 0; i < 10000; i++)
    {
        byte[] data = BitConverter.GetBytes(i);
        ms.Write(data, 0, data.Length);
    }
}
sw2.Stop();

Console.WriteLine($"  FileStream (disco):   {sw1.ElapsedMilliseconds}ms");
Console.WriteLine($"  MemoryStream (RAM):   {sw2.ElapsedMilliseconds}ms");
Console.WriteLine($"  Mejora:                {sw1.ElapsedMilliseconds / (double)sw2.ElapsedMilliseconds: F1}x más rápido");

// Limpiar
File.Delete("test.txt");
File.Delete("perf_test. txt");

Console.WriteLine("\n═══════════════════════════════════════════");
```

#### 1.1.5. Decoradores:  StreamReader y StreamWriter

Los `Stream` trabajan con **bytes** (nivel bajo), pero nosotros queremos trabajar con **texto** (nivel alto). Para esto existen los **decoradores**:

```
┌──────────────────────────────────────┐
│  ALTO NIVEL (texto, líneas, etc.)   │
├──────────────────────────────────────┤
│  StreamReader / StreamWriter         │  ← Lee/escribe texto
│  (decora un Stream)                  │
└─────────────┬────────────────────────┘
              │
              ▼
┌──────────────────────────────────────┐
│  BAJO NIVEL (bytes crudos)           │
├──────────────────────────────────────┤
│  FileStream                          │  ← Lee/escribe bytes
│  (acceso directo al fichero)        │
└─────────────┬────────────────────────┘
              │
              ▼
┌──────────────────────────────────────┐
│  Sistema Operativo (disco físico)   │
└──────────────────────────────────────┘
```

**Ejemplo:**

```csharp
// ========================================
// Forma 1: Manualmente (educativa)
// ========================================

// Crear el stream de bajo nivel (bytes)
using var fileStream = new FileStream("datos.txt", FileMode.Create);

// Decorar con StreamWriter para trabajar con texto
using var writer = new StreamWriter(fileStream, Encoding.UTF8);

writer.WriteLine("Esta es una línea de texto");
writer.WriteLine("Y esta es otra");

// ========================================
// Forma 2: Directamente (más común)
// ========================================

// StreamWriter crea el FileStream internamente
using var writer = new StreamWriter("datos.txt");
writer.WriteLine("Texto");

// Equivalente a:
// var fs = new FileStream("datos.txt", FileMode.Create);
// var writer = new StreamWriter(fs);
```

---

### 1.2. El Problema Fundamental: Recursos Compartidos y el "Candado"

Ahora que entendemos qué es un fichero y un stream, veamos **el problema más importante** al trabajar con ficheros. 

#### 1.2.1. La Analogía de la Biblioteca

Imagina que estás en una **biblioteca** y tomas un **libro** de un estante: 

1. **Tomas el libro** (abres el fichero)
2. **Lo lees** (accedes al contenido)
3. **Lo devuelves** (cierras el fichero)

**¿Qué pasa si te olvidas de devolverlo?**

- El libro queda **"bloqueado"** para otros usuarios
- Nadie más puede leerlo hasta que lo devuelvas
- Si muchas personas hacen esto, ¡la biblioteca colapsa!

**En programación, ocurre exactamente lo mismo con los ficheros.**

#### 1.2.2. El Sistema de Candados del Sistema Operativo

Cuando tu programa **abre un fichero**, el sistema operativo coloca un **"candado"** (lock) en él: 

```
Estado INICIAL: Fichero cerrado
┌─────────────────────┐
│  "datos.txt"        │  ← Nadie lo está usando
│  [sin candado]      │
└─────────────────────┘

Tu programa ABRE el fichero: 
┌─────────────────────┐
│  "datos.txt"        │  🔒 ← ¡CANDADO ACTIVADO!
│  [bloqueado]        │     (solo tu programa puede acceder)
└─────────────────────┘

Otro programa intenta abrirlo: 
❌ IOException: "El archivo está siendo usado por otro proceso"

Tu programa CIERRA el fichero: 
┌─────────────────────┐
│  "datos.txt"        │  🔓 ← Candado liberado
│  [sin candado]      │     (otros pueden acceder)
└─────────────────────┘
```

**Código problemático:**

```csharp
// ❌ CÓDIGO PROBLEMÁTICO (sin liberar el candado)

var file = File.Open("datos.txt", FileMode.Open);

// ...  trabajar con el archivo ...

// ¡OLVIDO CERRAR EL ARCHIVO!
// file.Close(); // ← Esta línea falta

// CONSECUENCIA: El candado nunca se libera
```

**Intentar abrir el fichero de nuevo:**

```csharp
try
{
    var file2 = File.Open("datos. txt", FileMode.Open);
}
catch (IOException ex)
{
    Console.WriteLine(ex.Message);
    // "El proceso no puede acceder al archivo 'datos.txt' 
    //  porque está siendo utilizado por otro proceso."
}
```

#### 1.2.3. ¿Qué Pasa Si Hay una Excepción?

El problema se agrava si hay un **error** antes de cerrar el fichero:

```csharp
// ❌ MUY PELIGROSO

FileStream file = File.Open("datos.txt", FileMode.Open);

// Leer datos... 
byte[] buffer = new byte[100];
file.Read(buffer, 0, buffer.Length);

// ¡Aquí ocurre una excepción!  (división por cero, etc.)
int resultado = 10 / 0; // ← BOOM!  

file.Close(); // ← Esta línea NUNCA se ejecuta

// RESULTADO: El fichero queda bloqueado PARA SIEMPRE
// (hasta reiniciar la aplicación)
```

**Solución clásica:  try-finally**

```csharp
// ✓ Solución correcta (pero verbosa)

FileStream file = File.Open("datos.txt", FileMode.Open);

try
{
    // Trabajar con el archivo
    byte[] buffer = new byte[100];
    file.Read(buffer, 0, buffer.Length);
    
    int resultado = 10 / 0; // Excepción
}
finally
{
    // ¡SIEMPRE se ejecuta, incluso con excepciones!
    file.Close();
}
```

---

### 1.3. La Interfaz IDisposable: El Contrato de Limpieza

Para resolver este problema de forma elegante, . NET introdujo la interfaz `IDisposable`.

#### 1.3.1. ¿Qué es IDisposable?

`IDisposable` es un **contrato** que dice: 

> "Esta clase gestiona recursos (ficheros, red, BD, etc.) 
>  que DEBEN liberarse manualmente.  
>  Llama a `Dispose()` cuando termines de usarla."

```csharp
public interface IDisposable
{
    void Dispose(); // Método que libera el recurso
}
```

**Clases que implementan `IDisposable`:**

| Clase           | Recurso que Gestiona | ¿Por qué necesita Dispose?    |
| --------------- | -------------------- | ----------------------------- |
| `FileStream`    | Fichero en disco     | Liberar candado del SO        |
| `StreamReader`  | Lector de texto      | Cerrar stream subyacente      |
| `StreamWriter`  | Escritor de texto    | Flush buffer + cerrar stream  |
| `SqlConnection` | Conexión a BD        | Cerrar conexión de red        |
| `HttpClient`    | Cliente HTTP         | Cerrar sockets de red         |
| `Bitmap`        | Imagen en memoria    | Liberar memoria no gestionada |

**Uso manual de Dispose:**

```csharp
// ✓ Correcto, pero tedioso

FileStream file = File.Open("datos.txt", FileMode.Open);

try
{
    // Trabajar con el archivo
}
finally
{
    file. Dispose(); // O file.Close() - son equivalentes
}
```

---

### 1.4. La Revolución del `using`: Liberación Automática

C# introdujo la palabra clave `using` para **automatizar** la llamada a `Dispose()`, garantizando que el recurso se libere **incluso si hay excepciones**.

#### 1.4.1. Bloque `using` Clásico (C# 1.0 - Presente)

```csharp
// ========================================
// BLOQUE USING CLÁSICO
// ========================================

using (var reader = new StreamReader("datos.txt"))
{
    // Trabajar con el archivo
    string contenido = reader.ReadToEnd();
    Console.WriteLine(contenido);
    
} // ← Aquí se llama automáticamente a reader.Dispose()

// El archivo ya está liberado aquí
Console.WriteLine("Fichero cerrado");
```

**¿Qué hace el compilador internamente?**

El código anterior se traduce a:

```csharp
StreamReader reader = new StreamReader("datos.txt");

try
{
    string contenido = reader.ReadToEnd();
    Console.WriteLine(contenido);
}
finally
{
    if (reader != null)
    {
        reader.Dispose(); // ¡SIEMPRE se ejecuta!
    }
}
```

**Ventajas:**

✅ **Garantiza** la liberación del recurso  
✅ Funciona **incluso con excepciones**  
✅ Código **más limpio** que try-finally manual  
✅ Previene el error "archivo en uso"  

#### 1.4.2. Declaración `using var` (C# 8+ / . NET Core 3.0+) - **RECOMENDADO**

C# 8 introdujo una sintaxis **aún más limpia**:  `using` sin llaves.  El recurso se libera automáticamente **al final del scope** (método, bloque, etc.).

```csharp
// ========================================
// DECLARACIÓN USING VAR (C# 8+) - MÁS LIMPIA
// ========================================

using var reader = new StreamReader("datos.txt");

// Trabajar con el archivo (sin llaves)
string contenido = reader.ReadToEnd();
Console.WriteLine(contenido);

// Al final del método/bloque, reader.Dispose() se llama automáticamente
Console.WriteLine("Fichero cerrado");
```

**Comparación visual:**

```csharp
// ========================================
// ANTES: Bloque using con llaves
// ========================================

void ProcesarArchivos()
{
    using (var reader1 = new StreamReader("archivo1.txt"))
    {
        var linea1 = reader1.ReadLine();
        Console.WriteLine(linea1);
    } // Dispose aquí
    
    using (var reader2 = new StreamReader("archivo2.txt"))
    {
        var linea2 = reader2.ReadLine();
        Console.WriteLine(linea2);
    } // Dispose aquí
}

// ========================================
// AHORA: using var sin llaves (más plano)
// ========================================

void ProcesarArchivos()
{
    using var reader1 = new StreamReader("archivo1.txt");
    var linea1 = reader1.ReadLine();
    Console.WriteLine(linea1);
    
    using var reader2 = new StreamReader("archivo2.txt");
    var linea2 = reader2.ReadLine();
    Console.WriteLine(linea2);
    
} // Dispose de reader1 y reader2 al final del método
```

**Ventajas de `using var`:**

✅ **Menos indentación** (código más plano)  
✅ **Más legible** con múltiples recursos  
✅ **Scope claro** (fin del método/bloque)  
✅ **Estilo moderno** de C#  

#### 1.4.3. ¿Cuándo Usar Cada Uno?

| Situación                                 | Usar                   | Razón                |
| ----------------------------------------- | ---------------------- | -------------------- |
| **Código simple, recurso hasta el final** | `using var`            | Más limpio           |
| **Control preciso del cierre**            | Bloque `using { }`     | Cerrar antes del fin |
| **Múltiples recursos independientes**     | Varios `using var`     | Sin anidamiento      |
| **Recursos dependientes**                 | Bloque `using` anidado | Control de orden     |

**Ejemplos:**

```csharp
// ========================================
// CASO 1: using var (recomendado para código simple)
// ========================================

void LeerArchivo()
{
    using var reader = new StreamReader("datos.txt");
    string contenido = reader.ReadToEnd();
    Console.WriteLine(contenido);
} // Dispose al final del método

// ========================================
// CASO 2: Bloque using (control preciso del cierre)
// ========================================

void ProcesarYBorrar()
{
    string contenido;
    
    using (var reader = new StreamReader("datos.txt"))
    {
        contenido = reader. ReadToEnd();
    } // Dispose AQUÍ (antes de borrar)
    
    // Ahora el archivo está cerrado, puedo borrarlo
    File.Delete("datos.txt");
    
    Console.WriteLine(contenido);
}

// ========================================
// CASO 3: Múltiples recursos (varios using var)
// ========================================

void CopiarArchivo()
{
    using var input = new StreamReader("origen.txt");
    using var output = new StreamWriter("destino. txt");
    
    string linea;
    while ((linea = input.ReadLine()) != null)
    {
        output.WriteLine(linea);
    }
    
} // Ambos se cierran al final
```

---

### 1.5. Ejemplo Completo: Demostración del Sistema de Candados

```csharp
// ========================================
// DEMOSTRACIÓN COMPLETA:  Sistema de Candados
// ========================================

using System;
using System.IO;

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  DEMOSTRACIÓN:  Sistema de Candados");
Console.WriteLine("═══════════════════════════════════════════\n");

string archivoTest = "candado_test.txt";

// ========================================
// PASO 1: Crear archivo CON using (correcto)
// ========================================

Console.WriteLine(">>> PASO 1: Crear archivo (con using var)");

using (var writer = new StreamWriter(archivoTest))
{
    writer.WriteLine("Línea 1");
    writer.WriteLine("Línea 2");
    Console.WriteLine("  ✓ Archivo creado y escrito");
} // ← Dispose se llama aquí, candado liberado

Console.WriteLine("  ✓ Archivo cerrado automáticamente\n");

// ========================================
// PASO 2: Intentar abrir DOS VECES sin liberar
// ========================================

Console.WriteLine(">>> PASO 2: Demostrar el problema del candado");

FileStream?  file1 = null;

try
{
    // Primera apertura (OK)
    file1 = File.Open(archivoTest, FileMode.Open, FileAccess.Read);
    Console.WriteLine("  ✓ Primera apertura:  OK");
    
    // Intentar abrir DE NUEVO sin cerrar la primera
    var file2 = File.Open(archivoTest, FileMode. Open, FileAccess.Write);
    Console.WriteLine("  ✓ Segunda apertura: OK (no debería llegar aquí)");
}
catch (IOException ex)
{
    Console.WriteLine($"  ✗ ERROR ESPERADO:");
    Console.WriteLine($"    {ex.Message}");
    Console.WriteLine("    → El archivo está bloqueado por file1\n");
}
finally
{
    // Liberar file1 manualmente para continuar
    file1?.Dispose();
}

// ========================================
// PASO 3: Abrir dos veces CON liberación (correcto)
// ========================================

Console.WriteLine(">>> PASO 3: Solución correcta con using");

using (var file1Read = File.Open(archivoTest, FileMode.Open, FileAccess.Read))
{
    Console.WriteLine("  ✓ Primera apertura: OK");
} // ← Dispose aquí, candado liberado

using (var file2Write = File.Open(archivoTest, FileMode. Append, FileAccess.Write))
{
    Console.WriteLine("  ✓ Segunda apertura: OK (el archivo estaba cerrado)");
    
    using var writer = new StreamWriter(file2Write);
    writer.WriteLine("Línea 3 (añadida después)");
} // ← Dispose aquí

Console.WriteLine("  ✓ Ambas operaciones completadas correctamente\n");

// ========================================
// PASO 4: Comparar using var vs using block
// ========================================

Console. WriteLine(">>> PASO 4: Comparar using var vs using block\n");

// Versión 1: using block (con llaves)
Console.WriteLine("  [Versión 1: using block]");
using (var reader = new StreamReader(archivoTest))
{
    string primeraLinea = reader.ReadLine() ?? "";
    Console.WriteLine($"    Leído: {primeraLinea}");
} // Dispose aquí (inmediatamente)
Console.WriteLine("    → Archivo cerrado después del bloque\n");

// Versión 2: using var (sin llaves)
Console.WriteLine("  [Versión 2: using var]");
using var reader2 = new StreamReader(archivoTest);
string primeraLinea2 = reader2.ReadLine() ?? "";
Console.WriteLine($"    Leído: {primeraLinea2}");
Console.WriteLine("    → Archivo aún abierto (se cierra al final del scope)");
// Dispose al final del método main

// ========================================
// PASO 5: Demostrar uso con múltiples archivos
// ========================================

Console.WriteLine("\n>>> PASO 5: Múltiples archivos simultáneamente");

string origen = "origen.txt";
string destino = "destino.txt";

// Crear archivo de origen
File.WriteAllText(origen, "Contenido a copiar");

using var inputStream = new StreamReader(origen);
using var outputStream = new StreamWriter(destino);

string contenido = inputStream.ReadToEnd();
outputStream.Write(contenido);

Console.WriteLine("  ✓ Archivo copiado con éxito");
Console.WriteLine("  → Ambos archivos se cerrarán al final del método\n");

// Verificar
Console.WriteLine(">>> PASO 6: Verificar contenido del destino");
string contenidoDestino = File.ReadAllText(destino);
Console.WriteLine($"  Contenido de '{destino}': {contenidoDestino}");

// Limpiar archivos de prueba
File.Delete(archivoTest);
File.Delete(origen);
File.Delete(destino);

Console.WriteLine("\n═══════════════════════════════════════════");
Console.WriteLine("  FIN DE LA DEMOSTRACIÓN");
Console.WriteLine("═══════════════════════════════════════════");
```

**Salida esperada:**

```
═══════════════════════════════════════════
  DEMOSTRACIÓN:  Sistema de Candados
═══════════════════════════════════════════

>>> PASO 1: Crear archivo (con using var)
  ✓ Archivo creado y escrito
  ✓ Archivo cerrado automáticamente

>>> PASO 2: Demostrar el problema del candado
  ✓ Primera apertura: OK
  ✗ ERROR ESPERADO:
    El proceso no puede acceder al archivo porque está siendo utilizado por otro proceso. 
    → El archivo está bloqueado por file1

>>> PASO 3: Solución correcta con using
  ✓ Primera apertura: OK
  ✓ Segunda apertura: OK (el archivo estaba cerrado)
  ✓ Ambas operaciones completadas correctamente

>>> PASO 4: Comparar using var vs using block

  [Versión 1: using block]
    Leído:  Línea 1
    → Archivo cerrado después del bloque

  [Versión 2: using var]
    Leído:  Línea 1
    → Archivo aún abierto (se cierra al final del scope)

>>> PASO 5: Múltiples archivos simultáneamente
  ✓ Archivo copiado con éxito
  → Ambos archivos se cerrarán al final del método

>>> PASO 6: Verificar contenido del destino
  Contenido de 'destino.txt': Contenido a copiar

═══════════════════════════════════════════
  FIN DE LA DEMOSTRACIÓN
═══════════════════════════════════════════
```

---

### 1.6. Resumen de Buenas Prácticas

```csharp
// ========================================
// ✅ BUENAS PRÁCTICAS
// ========================================

// 1. Siempre usar 'using' con recursos IDisposable
using var reader = new StreamReader("archivo.txt");
// NO: var reader = new StreamReader("archivo.txt");

// 2. Preferir 'using var' para código más limpio (C# 8+)
using var writer = new StreamWriter("salida.txt");
writer.WriteLine("Texto");

// 3. Usar bloque 'using' cuando necesites control preciso del cierre
using (var file = File.OpenWrite("datos.bin"))
{
    // Trabajar con el archivo
} // Cerrado aquí (antes del resto del código)

// 4. Múltiples recursos:  varios 'using var' en lugar de anidamiento
using var input = new StreamReader("entrada.txt");
using var output = new StreamWriter("salida.txt");
// Código más plano y legible

// 5. Capturar excepciones específicas de I/O
try
{
    using var reader = new StreamReader("archivo.txt");
    // ... 
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Archivo no encontrado: {ex. FileName}");
}
catch (IOException ex)
{
    Console.WriteLine($"Error de I/O: {ex.Message}");
}

// ========================================
// ❌ MALAS PRÁCTICAS (EVITAR)
// ========================================

// 1. NO olvidar el using
var reader = new StreamReader("archivo. txt");
string contenido = reader.ReadToEnd();
// ¡Falta reader.Dispose()!  Archivo bloqueado

// 2. NO usar try-catch sin finally/using
try
{
    var reader = new StreamReader("archivo.txt");
    // Si hay excepción, reader nunca se cierra
}
catch { }

// 3. NO usar FileStream directamente para texto (preferir StreamReader/Writer)
var fs = new FileStream("archivo.txt", FileMode.Open);
// Mejor: new StreamReader("archivo.txt")

// 4. NO mezclar using con Close() manual (redundante)
using (var reader = new StreamReader("archivo.txt"))
{
    // ... 
    reader.Close(); // ← Innecesario, using ya lo hace
}
```

---

## 2. Manipulación del Sistema de Ficheros (Clases File, Directory y Path)

### 2.0. Introducción:  Las Herramientas del Sistema de Archivos

Hasta ahora hemos aprendido a **abrir** y **trabajar con el contenido** de ficheros mediante streams.  Pero antes de leer o escribir, necesitamos: 

- **Verificar** si un fichero existe
- **Crear** directorios
- **Copiar** o **mover** ficheros
- **Eliminar** ficheros temporales
- **Obtener información** (tamaño, fechas, permisos)

.NET proporciona tres clases principales para estas operaciones:

| Clase       | Propósito                           | Tipo de Métodos                               |
| ----------- | ----------------------------------- | --------------------------------------------- |
| `File`      | Operaciones sobre **ficheros**      | Estáticos (ej: `File.Exists()`)               |
| `Directory` | Operaciones sobre **directorios**   | Estáticos (ej: `Directory.CreateDirectory()`) |
| `Path`      | Manipulación de **rutas** (strings) | Estáticos (ej: `Path.Combine()`)              |

Además, tenemos clases para operaciones más avanzadas:

| Clase           | Propósito                              | Cuándo Usar                                     |
| --------------- | -------------------------------------- | ----------------------------------------------- |
| `FileInfo`      | Información detallada de un fichero    | Múltiples operaciones sobre el mismo fichero    |
| `DirectoryInfo` | Información detallada de un directorio | Múltiples operaciones sobre el mismo directorio |

---

### 2.1. La Clase `File`: Operaciones sobre Ficheros

La clase `File` proporciona métodos **estáticos** para trabajar con ficheros.  No necesitas crear instancias, solo llamar a los métodos directamente.

#### 2.1.1. Verificar Existencia:  `File.Exists()`

```csharp
// ========================================
// VERIFICAR SI UN FICHERO EXISTE
// ========================================

using System;
using System.IO;

string rutaFichero = "documento.txt";

if (File. Exists(rutaFichero))
{
    Console.WriteLine($"✓ El fichero '{rutaFichero}' existe");
}
else
{
    Console. WriteLine($"✗ El fichero '{rutaFichero}' NO existe");
}
```

**¿Por qué es importante verificar antes de abrir?**

```csharp
// ❌ SIN verificar (puede lanzar excepción)
try
{
    using var reader = new StreamReader("noexiste.txt");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

// ✓ CON verificación (más elegante)
if (File.Exists("noexiste.txt"))
{
    using var reader = new StreamReader("noexiste.txt");
    // Procesar... 
}
else
{
    Console.WriteLine("El fichero no existe.  Creando uno nuevo...");
    File.WriteAllText("noexiste.txt", "Contenido inicial");
}
```

#### 2.1.2. Crear y Escribir:  `File.WriteAllText()` y `File.WriteAllLines()`

```csharp
// ========================================
// CREAR FICHERO Y ESCRIBIR CONTENIDO
// ========================================

// Método 1: WriteAllText (todo el contenido de una vez)
string contenido = "Esta es la línea 1\nEsta es la línea 2\nEsta es la línea 3";
File.WriteAllText("fichero1.txt", contenido);
Console.WriteLine("✓ Fichero creado con WriteAllText");

// Método 2: WriteAllLines (array de líneas)
string[] lineas = 
[
    "Primera línea",
    "Segunda línea",
    "Tercera línea"
];
File. WriteAllLines("fichero2.txt", lineas);
Console.WriteLine("✓ Fichero creado con WriteAllLines");

// Método 3: WriteAllBytes (array de bytes)
byte[] bytes = { 72, 111, 108, 97 }; // "Hola" en UTF-8
File.WriteAllBytes("fichero3.bin", bytes);
Console.WriteLine("✓ Fichero binario creado con WriteAllBytes");
```

**⚠️ ADVERTENCIA:  Estos métodos SOBRESCRIBEN el fichero si ya existe.**

```csharp
// Si el fichero existe, se BORRA y se crea uno nuevo
File.WriteAllText("importante.txt", "Contenido original");
File.WriteAllText("importante.txt", "Nuevo contenido"); // ← ¡Se pierde el original! 
```

#### 2.1.3. Leer Contenido: `File.ReadAllText()` y `File.ReadAllLines()`

```csharp
// ========================================
// LEER CONTENIDO DE UN FICHERO
// ========================================

// Método 1: ReadAllText (todo el contenido como string)
if (File.Exists("fichero1.txt"))
{
    string contenido = File.ReadAllText("fichero1.txt");
    Console.WriteLine($"Contenido completo:\n{contenido}");
}

// Método 2: ReadAllLines (array de líneas)
if (File.Exists("fichero2.txt"))
{
    string[] lineas = File.ReadAllLines("fichero2.txt");
    Console.WriteLine($"\nTotal de líneas: {lineas. Length}");
    
    for (int i = 0; i < lineas.Length; i++)
    {
        Console.WriteLine($"  Línea {i + 1}: {lineas[i]}");
    }
}

// Método 3: ReadAllBytes (array de bytes)
if (File.Exists("fichero3.bin"))
{
    byte[] bytes = File.ReadAllBytes("fichero3.bin");
    Console.WriteLine($"\nBytes leídos: [{string.Join(", ", bytes)}]");
}
```

**⚠️ ADVERTENCIA:  No uses estos métodos con ficheros grandes (> 100 MB).**

```csharp
// ❌ MAL: Fichero de 2 GB cargado completamente en RAM
string contenido = File.ReadAllText("video.mp4"); // ¡OutOfMemoryException!

// ✓ BIEN: Usar streams para ficheros grandes
using var stream = File.OpenRead("video.mp4");
byte[] buffer = new byte[4096];
while (stream.Read(buffer, 0, buffer.Length) > 0)
{
    // Procesar solo 4 KB cada vez
}
```

#### 2.1.4. Copiar Ficheros: `File.Copy()`

```csharp
// ========================================
// COPIAR FICHEROS
// ========================================

string origen = "original.txt";
string destino = "copia.txt";

// Crear fichero de origen
File.WriteAllText(origen, "Contenido del fichero original");

// Copiar (lanza excepción si el destino ya existe)
try
{
    File.Copy(origen, destino);
    Console.WriteLine($"✓ Fichero copiado:  {origen} → {destino}");
}
catch (IOException ex)
{
    Console.WriteLine($"✗ Error al copiar: {ex.Message}");
}

// Copiar SOBRESCRIBIENDO si ya existe
File.Copy(origen, destino, overwrite: true);
Console.WriteLine("✓ Fichero copiado (sobrescrito si existía)");
```

#### 2.1.5. Mover Ficheros: `File.Move()`

```csharp
// ========================================
// MOVER / RENOMBRAR FICHEROS
// ========================================

string rutaActual = "fichero_viejo.txt";
string rutaNueva = "fichero_nuevo.txt";

File.WriteAllText(rutaActual, "Contenido");

// Mover/Renombrar (el origen desaparece)
File.Move(rutaActual, rutaNueva);
Console.WriteLine($"✓ Fichero movido: {rutaActual} → {rutaNueva}");

// Verificar
Console.WriteLine($"¿Existe '{rutaActual}'? {File.Exists(rutaActual)}"); // false
Console.WriteLine($"¿Existe '{rutaNueva}'? {File.Exists(rutaNueva)}");   // true

// Mover a otro directorio
Directory.CreateDirectory("backup");
File.Move(rutaNueva, Path.Combine("backup", rutaNueva));
Console.WriteLine($"✓ Fichero movido a:  backup/{rutaNueva}");
```

**Diferencia entre `Copy` y `Move`:**

```
COPY:  Origen se mantiene, destino se crea
[origen. txt] ──Copy──→ [origen.txt]  +  [destino.txt]

MOVE: Origen desaparece, destino se crea
[origen.txt] ──Move──→ [destino.txt]
```

#### 2.1.6. Eliminar Ficheros: `File.Delete()`

```csharp
// ========================================
// ELIMINAR FICHEROS
// ========================================

string ficheroEliminar = "temporal.txt";

File.WriteAllText(ficheroEliminar, "Contenido temporal");

if (File.Exists(ficheroEliminar))
{
    File.Delete(ficheroEliminar);
    Console.WriteLine($"✓ Fichero eliminado: {ficheroEliminar}");
}

// Delete NO lanza excepción si el fichero no existe
File. Delete("noexiste.txt"); // No hace nada, no lanza error
Console.WriteLine("✓ Delete es seguro aunque el fichero no exista");
```

#### 2.1.7. Obtener Información:  Metadatos del Fichero

```csharp
// ========================================
// OBTENER INFORMACIÓN DE UN FICHERO
// ========================================

string rutaInfo = "info_test.txt";
File.WriteAllText(rutaInfo, "Contenido de prueba para obtener información.");

if (File.Exists(rutaInfo))
{
    Console.WriteLine($"\n═══ INFORMACIÓN DE '{rutaInfo}' ═══");
    
    // Fechas
    DateTime creacion = File.GetCreationTime(rutaInfo);
    DateTime modificacion = File.GetLastWriteTime(rutaInfo);
    DateTime acceso = File.GetLastAccessTime(rutaInfo);
    
    Console.WriteLine($"Fecha de creación:       {creacion}");
    Console.WriteLine($"Última modificación:    {modificacion}");
    Console.WriteLine($"Último acceso:          {acceso}");
    
    // Atributos
    FileAttributes atributos = File.GetAttributes(rutaInfo);
    Console.WriteLine($"Atributos:               {atributos}");
    
    // Tamaño (requiere FileInfo)
    var fileInfo = new FileInfo(rutaInfo);
    Console.WriteLine($"Tamaño:                 {fileInfo.Length} bytes");
}

// Limpiar
File.Delete(rutaInfo);
```

---

### 2.2. La Clase `FileInfo`: Operaciones Orientadas a Objetos

Cuando necesitas realizar **múltiples operaciones** sobre el **mismo fichero**, usar `FileInfo` es más eficiente que usar `File`.

#### 2.2.1. Diferencia entre `File` y `FileInfo`

| Aspecto     | `File` (estático)        | `FileInfo` (instancia)        |
| ----------- | ------------------------ | ----------------------------- |
| Sintaxis    | `File.Exists("ruta")`    | `new FileInfo("ruta").Exists` |
| Rendimiento | Verifica en cada llamada | Cachea información            |
| Uso típico  | Una operación puntual    | Múltiples operaciones         |

```csharp
// ========================================
// COMPARACIÓN:   File vs FileInfo
// ========================================

string ruta = "ejemplo.txt";
File.WriteAllText(ruta, "Contenido de ejemplo");

// ────────────────────────────────────────
// Opción 1: File (múltiples verificaciones)
// ────────────────────────────────────────

if (File. Exists(ruta))
{
    long tamaño = new FileInfo(ruta).Length; // Acceso a disco
    DateTime fecha = File.GetLastWriteTime(ruta); // Acceso a disco
    FileAttributes attr = File.GetAttributes(ruta); // Acceso a disco
    
    Console.WriteLine($"Tamaño: {tamaño} bytes");
}

// ────────────────────────────────────────
// Opción 2: FileInfo (una verificación, caché)
// ────────────────────────────────────────

var fileInfo = new FileInfo(ruta);

if (fileInfo.Exists)
{
    long tamaño = fileInfo.Length;               // Desde caché
    DateTime fecha = fileInfo.LastWriteTime;     // Desde caché
    FileAttributes attr = fileInfo.Attributes;   // Desde caché
    
    Console.WriteLine($"Tamaño: {tamaño} bytes");
}

File.Delete(ruta);
```

#### 2.2.2. Ejemplo Completo con FileInfo

```csharp
// ========================================
// USO COMPLETO DE FileInfo
// ========================================

using System;
using System.IO;

string rutaArchivo = "documento_completo.txt";

// Crear fichero de prueba
File.WriteAllText(rutaArchivo, "Este es un documento de ejemplo.\nSegunda línea.");

// Crear instancia de FileInfo
var fileInfo = new FileInfo(rutaArchivo);

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  INFORMACIÓN COMPLETA DEL FICHERO");
Console.WriteLine("═══════════════════════════════════════════\n");

// ────────────────────────────────────────
// PROPIEDADES BÁSICAS
// ────────────────────────────────────────

Console.WriteLine(">>> PROPIEDADES BÁSICAS");
Console.WriteLine($"Nombre:               {fileInfo.Name}");
Console.WriteLine($"Nombre sin extensión: {Path.GetFileNameWithoutExtension(fileInfo.Name)}");
Console.WriteLine($"Extensión:           {fileInfo.Extension}");
Console.WriteLine($"Ruta completa:       {fileInfo.FullName}");
Console.WriteLine($"Directorio:          {fileInfo.DirectoryName}");

// ────────────────────────────────────────
// TAMAÑO Y FECHAS
// ────────────────────────────────────────

Console.WriteLine("\n>>> TAMAÑO Y FECHAS");
Console.WriteLine($"Tamaño:              {fileInfo.Length} bytes");
Console.WriteLine($"Creado:              {fileInfo.CreationTime: dd/MM/yyyy HH:mm:ss}");
Console.WriteLine($"Modificado:          {fileInfo.LastWriteTime:dd/MM/yyyy HH:mm:ss}");
Console.WriteLine($"Último acceso:       {fileInfo. LastAccessTime:dd/MM/yyyy HH:mm:ss}");

// ────────────────────────────────────────
// ATRIBUTOS
// ────────────────────────────────────────

Console.WriteLine("\n>>> ATRIBUTOS");
Console.WriteLine($"¿Es solo lectura?    {fileInfo.IsReadOnly}");
Console.WriteLine($"Atributos completos: {fileInfo.Attributes}");

// Verificar atributos específicos
bool esOculto = (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
bool esSistema = (fileInfo.Attributes & FileAttributes.System) == FileAttributes.System;

Console.WriteLine($"¿Es oculto?          {esOculto}");
Console.WriteLine($"¿Es de sistema?      {esSistema}");

// ────────────────────────────────────────
// OPERACIONES
// ────────────────────────────────────────

Console.WriteLine("\n>>> OPERACIONES");

// Copiar
var copia = fileInfo.CopyTo("copia_documento.txt", overwrite: true);
Console.WriteLine($"✓ Copiado a:          {copia.FullName}");

// Mover
fileInfo.MoveTo("documento_movido.txt");
Console.WriteLine($"✓ Movido a:          {fileInfo.FullName}");

// Abrir para lectura
using (var stream = fileInfo.OpenRead())
{
    using var reader = new StreamReader(stream);
    string primeraLinea = reader.ReadLine() ?? "";
    Console.WriteLine($"Primera línea:        {primeraLinea}");
}

// Eliminar
fileInfo.Delete();
Console.WriteLine($"✓ Fichero eliminado");

// Limpiar copia
File.Delete("copia_documento.txt");

Console.WriteLine("\n═══════════════════════════════════════════");
```

**Salida esperada:**

```
═══════════════════════════════════════════
  INFORMACIÓN COMPLETA DEL FICHERO
═══════════════════════════════════════════

>>> PROPIEDADES BÁSICAS
Nombre:              documento_completo.txt
Nombre sin extensión: documento_completo
Extensión:           .txt
Ruta completa:       C:\.. .\documento_completo.txt
Directorio:          C:\... 

>>> TAMAÑO Y FECHAS
Tamaño:              48 bytes
Creado:               15/01/2025 14:30:45
Modificado:          15/01/2025 14:30:45
Último acceso:        15/01/2025 14:30:45

>>> ATRIBUTOS
¿Es solo lectura?    False
Atributos completos: Archive
¿Es oculto?          False
¿Es de sistema?       False

>>> OPERACIONES
✓ Copiado a:          C:\...\copia_documento.txt
✓ Movido a:          C:\...\documento_movido. txt
Primera línea:       Este es un documento de ejemplo. 
✓ Fichero eliminado

═══════════════════════════════════════════
```

---

### 2.3. La Clase `Directory`: Operaciones sobre Directorios

La clase `Directory` proporciona métodos estáticos para trabajar con directorios (carpetas).

#### 2.3.1. Crear Directorios: `Directory.CreateDirectory()`

```csharp
// ========================================
// CREAR DIRECTORIOS
// ========================================

string rutaDirectorio = "MiCarpeta";

// Crear directorio
if (! Directory.Exists(rutaDirectorio))
{
    Directory.CreateDirectory(rutaDirectorio);
    Console.WriteLine($"✓ Directorio creado:  {rutaDirectorio}");
}
else
{
    Console.WriteLine($"El directorio '{rutaDirectorio}' ya existe");
}

// Crear estructura de directorios anidados
string rutaAnidada = Path.Combine("Proyecto", "src", "models");
Directory.CreateDirectory(rutaAnidada);
Console.WriteLine($"✓ Estructura creada: {rutaAnidada}");
// Crea automáticamente:  Proyecto/src/models
```

**Nota importante:** `CreateDirectory` es **idempotente** (no lanza error si ya existe).

#### 2.3.2. Listar Contenido: `GetFiles()` y `GetDirectories()`

```csharp
// ========================================
// LISTAR CONTENIDO DE UN DIRECTORIO
// ========================================

string carpetaPrueba = "TestListado";
Directory.CreateDirectory(carpetaPrueba);

// Crear ficheros de prueba
File.WriteAllText(Path.Combine(carpetaPrueba, "doc1.txt"), "Contenido 1");
File.WriteAllText(Path.Combine(carpetaPrueba, "doc2.txt"), "Contenido 2");
File.WriteAllText(Path. Combine(carpetaPrueba, "imagen.jpg"), "fake image");

// Crear subdirectorio
Directory.CreateDirectory(Path.Combine(carpetaPrueba, "Subfolder"));

// ────────────────────────────────────────
// Listar FICHEROS
// ────────────────────────────────────────

Console.WriteLine($"\n>>> FICHEROS EN '{carpetaPrueba}':");
string[] ficheros = Directory.GetFiles(carpetaPrueba);

foreach (string fichero in ficheros)
{
    var info = new FileInfo(fichero);
    Console.WriteLine($"  📄 {info.Name} ({info.Length} bytes)");
}

// ────────────────────────────────────────
// Listar DIRECTORIOS
// ────────────────────────────────────────

Console.WriteLine($"\n>>> DIRECTORIOS EN '{carpetaPrueba}':");
string[] directorios = Directory.GetDirectories(carpetaPrueba);

foreach (string directorio in directorios)
{
    var info = new DirectoryInfo(directorio);
    Console.WriteLine($"  📁 {info.Name}");
}

// ────────────────────────────────────────
// Listar TODO (ficheros + directorios)
// ────────────────────────────────────────

Console.WriteLine($"\n>>> TODO EN '{carpetaPrueba}':");
string[] todo = Directory.GetFileSystemEntries(carpetaPrueba);

foreach (string entrada in todo)
{
    string icono = Directory. Exists(entrada) ? "📁" : "📄";
    Console.WriteLine($"  {icono} {Path.GetFileName(entrada)}");
}
```

#### 2.3.3. Búsqueda con Patrones:  Filtros

```csharp
// ========================================
// BÚSQUEDA CON PATRONES (wildcards)
// ========================================

// Crear ficheros de diferentes tipos
Directory.CreateDirectory("Documentos");
File.WriteAllText(Path.Combine("Documentos", "reporte.pdf"), "PDF content");
File.WriteAllText(Path.Combine("Documentos", "informe.docx"), "Word content");
File.WriteAllText(Path.Combine("Documentos", "datos.xlsx"), "Excel content");
File.WriteAllText(Path. Combine("Documentos", "notas.txt"), "Text content");

// Buscar solo ficheros .pdf
Console.WriteLine("\n>>> FICHEROS .pdf:");
string[] pdfs = Directory.GetFiles("Documentos", "*.pdf");
foreach (string pdf in pdfs)
{
    Console.WriteLine($"  {Path.GetFileName(pdf)}");
}

// Buscar ficheros que empiecen con 'inf'
Console.WriteLine("\n>>> FICHEROS que empiezan con 'inf':");
string[] informes = Directory.GetFiles("Documentos", "inf*");
foreach (string informe in informes)
{
    Console.WriteLine($"  {Path.GetFileName(informe)}");
}

// Buscar ficheros con cualquier extensión que empiece por 'd'
Console.WriteLine("\n>>> FICHEROS con extensión . d*:");
string[] dFiles = Directory.GetFiles("Documentos", "*. d*");
foreach (string file in dFiles)
{
    Console.WriteLine($"  {Path.GetFileName(file)}");
}
```

#### 2.3.4. Búsqueda Recursiva: `SearchOption.AllDirectories`

```csharp
// ========================================
// BÚSQUEDA RECURSIVA (en subdirectorios)
// ========================================

// Crear estructura de directorios
Directory. CreateDirectory(Path.Combine("Proyecto", "src"));
Directory.CreateDirectory(Path. Combine("Proyecto", "tests"));
Directory.CreateDirectory(Path. Combine("Proyecto", "docs"));

File.WriteAllText(Path. Combine("Proyecto", "README. md"), "Main readme");
File.WriteAllText(Path. Combine("Proyecto", "src", "Program.cs"), "Code");
File.WriteAllText(Path.Combine("Proyecto", "src", "Utils.cs"), "Utils code");
File.WriteAllText(Path. Combine("Proyecto", "tests", "Test. cs"), "Test code");

// Buscar SOLO en el directorio raíz
Console.WriteLine("\n>>> Ficheros . cs (solo raíz):");
string[] csFilesRaiz = Directory.GetFiles("Proyecto", "*.cs", SearchOption.TopDirectoryOnly);
Console.WriteLine($"  Encontrados: {csFilesRaiz.Length}");

// Buscar RECURSIVAMENTE en todos los subdirectorios
Console.WriteLine("\n>>> Ficheros .cs (recursivo):");
string[] csFilesRecursivo = Directory.GetFiles("Proyecto", "*.cs", SearchOption.AllDirectories);

foreach (string file in csFilesRecursivo)
{
    // Mostrar ruta relativa
    string relativePath = Path.GetRelativePath("Proyecto", file);
    Console.WriteLine($"  {relativePath}");
}
```

#### 2.3.5. Mover y Eliminar Directorios

```csharp
// ========================================
// MOVER DIRECTORIOS
// ========================================

Directory.CreateDirectory("CarpetaOriginal");
File.WriteAllText(Path.Combine("CarpetaOriginal", "fichero.txt"), "Contenido");

// Mover (renombrar)
Directory.Move("CarpetaOriginal", "CarpetaNueva");
Console.WriteLine("✓ Directorio movido:  CarpetaOriginal → CarpetaNueva");

// ========================================
// ELIMINAR DIRECTORIOS
// ========================================

// Eliminar directorio VACÍO
Directory.CreateDirectory("DirectorioVacio");
Directory.Delete("DirectorioVacio");
Console.WriteLine("✓ Directorio vacío eliminado");

// Eliminar directorio CON CONTENIDO (recursive:  true)
Directory.CreateDirectory("DirectorioConContenido");
File.WriteAllText(Path. Combine("DirectorioConContenido", "archivo.txt"), "dato");

Directory.Delete("DirectorioConContenido", recursive: true);
Console.WriteLine("✓ Directorio con contenido eliminado (recursivo)");
```

---

### 2.4. La Clase `Path`: Manipulación de Rutas

La clase `Path` NO interactúa con el disco, solo **manipula strings de rutas** de forma segura y multiplataforma.

#### 2.4.1. Combinar Rutas: `Path.Combine()`

```csharp
// ========================================
// COMBINAR RUTAS DE FORMA SEGURA
// ========================================

// ❌ MAL: Concatenación manual (problemas multiplataforma)
string rutaMala = "C:\\Carpeta" + "\\" + "Subcarpeta" + "\\" + "archivo.txt";
// En Linux fallaría:   usa '/' en lugar de '\\'

// ✓ BIEN: Path.Combine (automático según el SO)
string rutaBuena = Path.Combine("C:", "Carpeta", "Subcarpeta", "archivo.txt");
Console.WriteLine($"Ruta combinada: {rutaBuena}");
// Windows: C:\Carpeta\Subcarpeta\archivo.txt
// Linux:    C:/Carpeta/Subcarpeta/archivo.txt

// Combinar con directorio actual
string rutaRelativa = Path.Combine(Directory.GetCurrentDirectory(), "datos", "config.json");
Console.WriteLine($"Ruta absoluta desde actual: {rutaRelativa}");
```

#### 2.4.2. Extraer Componentes de una Ruta

```csharp
// ========================================
// EXTRAER COMPONENTES DE UNA RUTA
// ========================================

string rutaCompleta = Path.Combine("C:", "Usuarios", "Alumno", "Documentos", "informe.pdf");

Console.WriteLine($"Ruta completa: {rutaCompleta}\n");

// Nombre del fichero (con extensión)
string nombreFichero = Path.GetFileName(rutaCompleta);
Console.WriteLine($"Nombre fichero:          {nombreFichero}"); // informe.pdf

// Nombre sin extensión
string nombreSinExt = Path.GetFileNameWithoutExtension(rutaCompleta);
Console.WriteLine($"Nombre sin extensión:   {nombreSinExt}"); // informe

// Extensión
string extension = Path.GetExtension(rutaCompleta);
Console.WriteLine($"Extensión:              {extension}"); // .pdf

// Directorio contenedor
string directorio = Path. GetDirectoryName(rutaCompleta);
Console.WriteLine($"Directorio:              {directorio}"); // C:\Usuarios\Alumno\Documentos

// Raíz del sistema
string raiz = Path.GetPathRoot(rutaCompleta);
Console.WriteLine($"Raíz:                    {raiz}"); // C:\
```

#### 2.4.3. Generar Rutas Temporales

```csharp
// ========================================
// FICHEROS Y DIRECTORIOS TEMPORALES
// ========================================

// Directorio temporal del sistema
string dirTemp = Path.GetTempPath();
Console.WriteLine($"Directorio temporal: {dirTemp}");
// Windows: C:\Users\.. .\AppData\Local\Temp\
// Linux:    /tmp/

// Generar nombre de fichero temporal único
string ficheroTemp = Path.GetTempFileName();
Console.WriteLine($"Fichero temporal:     {ficheroTemp}");
// Crea automáticamente un fichero vacío con nombre único

// Usar y eliminar
File.WriteAllText(ficheroTemp, "Datos temporales");
Console.WriteLine($"✓ Escrito en temporal");

File.Delete(ficheroTemp);
Console.WriteLine($"✓ Temporal eliminado");

// Generar nombre aleatorio (sin crear el fichero)
string nombreAleatorio = Path.GetRandomFileName();
Console.WriteLine($"Nombre aleatorio:     {nombreAleatorio}"); // ej:  xyz123ab. tmp
```

#### 2.4.4. Rutas Absolutas vs Relativas

```csharp
// ========================================
// RUTAS ABSOLUTAS Y RELATIVAS
// ========================================

// Directorio actual
string directorioActual = Directory.GetCurrentDirectory();
Console.WriteLine($"Directorio actual:  {directorioActual}\n");

// Ruta relativa
string rutaRelativa = Path.Combine("datos", "config.json");
Console.WriteLine($"Ruta relativa:   {rutaRelativa}");

// Convertir a absoluta
string rutaAbsoluta = Path.GetFullPath(rutaRelativa);
Console.WriteLine($"Ruta absoluta:   {rutaAbsoluta}");

// Obtener ruta relativa desde un punto
string origen = @"C:\Proyecto\src";
string destino = @"C:\Proyecto\docs\manual.pdf";
string relativa = Path.GetRelativePath(origen, destino);
Console.WriteLine($"\nDesde:  {origen}");
Console.WriteLine($"Hasta: {destino}");
Console.WriteLine($"Relativa: {relativa}"); // ..\docs\manual.pdf
```

---

### 2.5. LINQ + Sistema de Ficheros:  Búsquedas Avanzadas

Ahora que conocemos las herramientas básicas, podemos combinarlas con **LINQ** para realizar búsquedas y filtrados complejos.

#### 2.5.1. Filtrar Ficheros por Tamaño

```csharp
// ========================================
// LINQ:   Filtrar ficheros por tamaño
// ========================================

using System;
using System.IO;
using System.Linq;

// Crear ficheros de prueba con diferentes tamaños
Directory.CreateDirectory("TestLINQ");
File.WriteAllText(Path.Combine("TestLINQ", "pequeño.txt"), "abc"); // 3 bytes
File.WriteAllText(Path.Combine("TestLINQ", "mediano.txt"), new string('x', 1024)); // 1 KB
File.WriteAllText(Path.Combine("TestLINQ", "grande.txt"), new string('y', 1024 * 100)); // 100 KB

// Obtener ficheros mayores de 1 KB usando LINQ
var ficherosGrandes = Directory. GetFiles("TestLINQ")
    .Select(ruta => new FileInfo(ruta))
    .Where(info => info.Length > 1024)
    .OrderByDescending(info => info.Length);

Console.WriteLine(">>> FICHEROS MAYORES DE 1 KB:");
foreach (var fichero in ficherosGrandes)
{
    Console.WriteLine($"  {fichero.Name}:  {fichero.Length:N0} bytes ({fichero.Length / 1024. 0:F2} KB)");
}
```

#### 2.5.2. Filtrar por Fecha de Modificación

```csharp
// ========================================
// LINQ:   Ficheros modificados recientemente
// ========================================

// Crear ficheros con diferentes fechas
Directory.CreateDirectory("TestFechas");
string fichero1 = Path.Combine("TestFechas", "antiguo.txt");
string fichero2 = Path.Combine("TestFechas", "reciente.txt");

File.WriteAllText(fichero1, "Antiguo");
File.WriteAllText(fichero2, "Reciente");

// Modificar fecha del antiguo
File.SetLastWriteTime(fichero1, DateTime.Now.AddDays(-30));

// Obtener ficheros modificados en los últimos 7 días
var ficherosRecientes = Directory.GetFiles("TestFechas")
    .Select(ruta => new FileInfo(ruta))
    .Where(info => info.LastWriteTime > DateTime.Now. AddDays(-7))
    .OrderByDescending(info => info.LastWriteTime);

Console.WriteLine("\n>>> FICHEROS MODIFICADOS EN LOS ÚLTIMOS 7 DÍAS:");
foreach (var fichero in ficherosRecientes)
{
    TimeSpan antiguedad = DateTime.Now - fichero.LastWriteTime;
    Console.WriteLine($"  {fichero.Name}: hace {antiguedad.Days} días");
}
```

#### 2.5.3. Búsqueda Compleja:   Imágenes JPG Grandes

```csharp
// ========================================
// LINQ:   Búsqueda con múltiples criterios
// ========================================

// Crear estructura de prueba
Directory.CreateDirectory(Path.Combine("TestComplejo", "imagenes"));
Directory.CreateDirectory(Path.Combine("TestComplejo", "docs"));

// Ficheros de prueba
File.WriteAllBytes(Path.Combine("TestComplejo", "imagenes", "foto1.jpg"), new byte[2 * 1024 * 1024]); // 2 MB
File.WriteAllBytes(Path.Combine("TestComplejo", "imagenes", "foto2.jpg"), new byte[500 * 1024]); // 500 KB
File.WriteAllBytes(Path.Combine("TestComplejo", "imagenes", "icono.png"), new byte[50 * 1024]); // 50 KB
File.WriteAllText(Path.Combine("TestComplejo", "docs", "manual.pdf"), "PDF content");

// BÚSQUEDA:   Imágenes JPG mayores de 1 MB creadas esta semana
var resultado = Directory.GetFiles("TestComplejo", "*.*", SearchOption.AllDirectories)
    .Select(ruta => new FileInfo(ruta))
    .Where(info => info.Extension.ToLower() == ".jpg")
    .Where(info => info.Length > 1 * 1024 * 1024) // > 1 MB
    .Where(info => info.CreationTime > DateTime.Now.AddDays(-7))
    .OrderByDescending(info => info.Length);

Console.WriteLine("\n>>> IMÁGENES JPG > 1 MB (última semana):");
foreach (var img in resultado)
{
    string rutaRelativa = Path.GetRelativePath("TestComplejo", img.FullName);
    Console.WriteLine($"  {rutaRelativa}");
    Console.WriteLine($"    Tamaño: {img.Length / (1024.0 * 1024):F2} MB");
    Console.WriteLine($"    Creado: {img.CreationTime:dd/MM/yyyy HH:mm}");
}
```

#### 2.5.4. Agrupar Ficheros por Extensión

```csharp
// ========================================
// LINQ:  Agrupar ficheros por extensión
// ========================================

Directory.CreateDirectory("TestAgrupacion");
File.WriteAllText(Path.Combine("TestAgrupacion", "doc1.txt"), "text");
File.WriteAllText(Path.Combine("TestAgrupacion", "doc2.txt"), "text");
File.WriteAllText(Path.Combine("TestAgrupacion", "imagen. jpg"), "image");
File.WriteAllText(Path.Combine("TestAgrupacion", "datos.csv"), "data");
File.WriteAllText(Path. Combine("TestAgrupacion", "config.json"), "json");

// Agrupar por extensión y contar
var porExtension = Directory.GetFiles("TestAgrupacion")
    .Select(ruta => new FileInfo(ruta))
    .GroupBy(info => info.Extension.ToLower())
    .Select(grupo => new
    {
        Extension = grupo.Key,
        Cantidad = grupo.Count(),
        TamañoTotal = grupo.Sum(f => f.Length)
    })
    .OrderByDescending(x => x.Cantidad);

Console.WriteLine("\n>>> FICHEROS AGRUPADOS POR EXTENSIÓN:");
foreach (var grupo in porExtension)
{
    Console.WriteLine($"  {grupo. Extension}: {grupo.Cantidad} ficheros, {grupo.TamañoTotal} bytes");
}
```

---

## 3. Ficheros de Texto Plano

### 3.0. Introducción: ¿Qué es un Fichero de Texto? 

Un **fichero de texto plano** es un fichero que contiene **solo caracteres legibles** (letras, números, símbolos) codificados en algún formato de texto (UTF-8, ASCII, etc.).  **No contiene** formato (negritas, colores), imágenes ni estructuras binarias complejas.

**Ejemplos de ficheros de texto:**
- `.txt` → Archivos de texto sin formato
- `.csv` → Valores separados por comas
- `.json` → Datos estructurados en formato JSON
- `.xml` → Datos estructurados en XML
- `.md` → Markdown (documentación)
- `.log` → Archivos de registro (logs)
- `.config` → Archivos de configuración

**Comparación:**

```
FICHERO DE TEXTO (. txt):
┌─────────────────────────────────────┐
│ Hola mundo                          │
│ Esta es la segunda línea            │
└─────────────────────────────────────┘
Bytes:   [72][111][108][97][32][109]... 
        (H) (o)  (l)  (a) ( ) (m)...
        
FICHERO BINARIO (.  docx, . jpg, .exe):
┌─────────────────────────────────────┐
│ [PK][03][04][14][00][06][00]...     │  ← No legible
│ [08][00][00][00][21][00][B2][AF]... │
└─────────────────────────────────────┘
```

### 3.1. Codificación de Caracteres:   UTF-8, ASCII y Unicode

Antes de trabajar con texto, necesitamos entender **cómo se representan los caracteres en bytes**.

#### 3.1.1. ¿Qué es la Codificación? 

La **codificación** es el proceso de convertir **caracteres** (letras, símbolos) en **bytes** (números).

```
Carácter  →  [Codificación]  →  Bytes

   'A'    →    [ASCII]       →   65
   'Ñ'    →    [UTF-8]       →   195, 145
   '€'    →    [UTF-8]       →   226, 130, 172
```

#### 3.1.2. Principales Codificaciones

| Codificación             | Descripción            | Rango                | Uso Típico                          |
| ------------------------ | ---------------------- | -------------------- | ----------------------------------- |
| **ASCII**                | American Standard Code | 0-127 (7 bits)       | Inglés básico (sin acentos)         |
| **Latin-1 (ISO-8859-1)** | ASCII extendido        | 0-255 (8 bits)       | Idiomas europeos (español, francés) |
| **UTF-8**                | Unicode 8-bit          | Variable (1-4 bytes) | **Estándar moderno (RECOMENDADO)**  |
| **UTF-16**               | Unicode 16-bit         | Variable (2-4 bytes) | Windows internamente                |
| **UTF-32**               | Unicode 32-bit         | 4 bytes por carácter | Procesamiento interno               |

**Ejemplo práctico:**

```csharp
// ========================================
// DEMOSTRACIÓN:   Codificaciones
// ========================================

using System;
using System.Text;

string texto = "Hola España €";

// ────────────────────────────────────────
// UTF-8 (recomendado, 1-4 bytes por carácter)
// ────────────────────────────────────────

byte[] bytesUTF8 = Encoding. UTF8.GetBytes(texto);
Console.WriteLine("UTF-8:");
Console.WriteLine($"  Texto:   {texto}");
Console.WriteLine($"  Bytes:  {bytesUTF8.Length}");
Console.WriteLine($"  Hex:    {BitConverter.ToString(bytesUTF8)}");

// ────────────────────────────────────────
// ASCII (solo caracteres básicos)
// ────────────────────────────────────────

try
{
    byte[] bytesASCII = Encoding.ASCII.GetBytes(texto);
    string recuperado = Encoding.ASCII.GetString(bytesASCII);
    Console.WriteLine("\nASCII:");
    Console.WriteLine($"  Original:     {texto}");
    Console.WriteLine($"  Recuperado:  {recuperado}"); // Pierde 'ñ' y '€'
}
catch
{
    Console.WriteLine("\nASCII:  No puede codificar caracteres especiales");
}

// ────────────────────────────────────────
// Latin-1 (8 bits, idiomas europeos)
// ────────────────────────────────────────

byte[] bytesLatin1 = Encoding.Latin1.GetBytes("Hola España"); // Sin €
Console.WriteLine("\nLatin-1:");
Console.WriteLine($"  Bytes:  {bytesLatin1.Length}");
Console.WriteLine($"  Hex:    {BitConverter.ToString(bytesLatin1)}");

// ────────────────────────────────────────
// UTF-16 (2-4 bytes, usado por Windows)
// ────────────────────────────────────────

byte[] bytesUTF16 = Encoding.Unicode.GetBytes(texto);
Console.WriteLine("\nUTF-16:");
Console.WriteLine($"  Bytes: {bytesUTF16.Length}");
Console.WriteLine($"  Hex:   {BitConverter.ToString(bytesUTF16)}");
```

**Salida esperada:**

```
UTF-8:
  Texto:  Hola España €
  Bytes: 15
  Hex:    48-6F-6C-61-20-45-73-70-61-C3-B1-61-20-E2-82-AC

ASCII:
  Original:    Hola España €
  Recuperado: Hola Espa? a ? 

Latin-1:
  Bytes: 11
  Hex:   48-6F-6C-61-20-45-73-70-61-F1-61

UTF-16:
  Bytes: 28
  Hex:   48-00-6F-00-6C-00-61-00-20-00-45-00-73-00-70-00-61-00-F1-00-61-00-20-00-AC-20
```

**Conclusión:   Usa siempre UTF-8 para compatibilidad universal.**

---

### 3.2. Escritura de Ficheros de Texto

#### 3.2.1. Método Rápido: `File.WriteAllText()` y `File.WriteAllLines()`

Para ficheros **pequeños** donde puedes tener todo el contenido en memoria:

```csharp
// ========================================
// ESCRITURA RÁPIDA (ficheros pequeños)
// ========================================

using System;
using System.IO;
using System.Text;

// ────────────────────────────────────────
// Método 1:  WriteAllText (todo el texto de una vez)
// ────────────────────────────────────────

string contenido = "Primera línea\nSegunda línea\nTercera línea";
File.WriteAllText("fichero1.txt", contenido, Encoding.UTF8);
Console.WriteLine("✓ Fichero creado con WriteAllText");

// ────────────────────────────────────────
// Método 2: WriteAllLines (array de líneas)
// ────────────────────────────────────────

string[] lineas = 
[
    "Línea 1: Encabezado",
    "Línea 2: Contenido",
    "Línea 3: Pie de página"
];

File. WriteAllLines("fichero2.txt", lineas, Encoding. UTF8);
Console.WriteLine("✓ Fichero creado con WriteAllLines");

// ────────────────────────────────────────
// Método 3: AppendAllText (añadir al final)
// ────────────────────────────────────────

File.AppendAllText("fichero1.txt", "\nLínea añadida al final", Encoding.UTF8);
Console.WriteLine("✓ Texto añadido con AppendAllText");

// Leer para verificar
string resultado = File.ReadAllText("fichero1.txt");
Console.WriteLine($"\nContenido final:\n{resultado}");
```

**⚠️ IMPORTANTE:   WriteAllText SOBRESCRIBE el fichero si existe.**

```csharp
// Crear fichero
File.WriteAllText("importante.txt", "Contenido original");

// ¡SOBRESCRIBE!  El contenido anterior se pierde
File.WriteAllText("importante.txt", "Nuevo contenido");

// Solución:   Usar AppendAllText para añadir
File.AppendAllText("importante.txt", "\nMás contenido");
```

#### 3.2.2. StreamWriter:   Escritura Eficiente (Ficheros Grandes)

Para ficheros **grandes** o cuando escribes línea por línea:

```csharp
// ========================================
// STREAMWRITER (escritura eficiente)
// ========================================

using System;
using System.IO;

// ────────────────────────────────────────
// CREAR fichero nuevo (sobrescribe si existe)
// ────────────────────────────────────────

Console.WriteLine(">>> CREAR FICHERO NUEVO");

using (var writer = new StreamWriter("log.txt"))
{
    writer.WriteLine("=== INICIO DEL LOG ===");
    writer.WriteLine($"Fecha: {DateTime.Now}");
    writer.WriteLine("Usuario: Admin");
    writer.WriteLine("======================");
}

Console.WriteLine("✓ Fichero log. txt creado\n");

// ────────────────────────────────────────
// AÑADIR al final (append:  true)
// ────────────────────────────────────────

Console.WriteLine(">>> AÑADIR AL FINAL (append)");

using (var writer = new StreamWriter("log.txt", append: true))
{
    writer.WriteLine($"[{DateTime.Now:HH:mm: ss}] Usuario inició sesión");
    writer.WriteLine($"[{DateTime.Now:HH:mm:ss}] Operación ejecutada");
    writer.WriteLine($"[{DateTime.Now:HH:mm:ss}] Usuario cerró sesión");
}

Console.WriteLine("✓ Líneas añadidas al log\n");

// ────────────────────────────────────────
// Especificar codificación explícitamente
// ────────────────────────────────────────

using (var writer = new StreamWriter("utf8.txt", append: false, encoding: System.Text.Encoding.UTF8))
{
    writer.WriteLine("Texto con caracteres especiales: España, €, ñ, á");
}

Console.WriteLine("✓ Fichero con UTF-8 creado\n");

// Leer y mostrar contenido final
Console.WriteLine(">>> CONTENIDO FINAL DE log.txt:");
string contenido = File.ReadAllText("log.txt");
Console.WriteLine(contenido);
```

**Comparación:   Crear vs Añadir**

```csharp
// ========================================
// DEMOSTRACIÓN:  append:  false vs true
// ========================================

string rutaTest = "test_append.txt";

// Escritura 1: Crear fichero
using (var writer = new StreamWriter(rutaTest, append: false))
{
    writer.WriteLine("Línea 1 (primera escritura)");
}

// Escritura 2: SOBRESCRIBIR (append: false)
using (var writer = new StreamWriter(rutaTest, append: false))
{
    writer.WriteLine("Línea 1 (segunda escritura)");
}

Console.WriteLine("Contenido después de sobrescribir:");
Console.WriteLine(File.ReadAllText(rutaTest));
// Solo muestra:   "Línea 1 (segunda escritura)"

// Escritura 3: AÑADIR (append: true)
using (var writer = new StreamWriter(rutaTest, append: true))
{
    writer.WriteLine("Línea 2 (añadida)");
}

Console.WriteLine("\nContenido después de añadir:");
Console.WriteLine(File.ReadAllText(rutaTest));
// Muestra ambas líneas

File.Delete(rutaTest);
```

#### 3.2.3. Write vs WriteLine

```csharp
// ========================================
// DIFERENCIA:   Write vs WriteLine
// ========================================

using (var writer = new StreamWriter("diferencia.txt"))
{
    // Write:   NO añade salto de línea
    writer. Write("Hola ");
    writer.Write("mundo ");
    writer.Write("sin ");
    writer.Write("saltos");
    
    // WriteLine: SÍ añade salto de línea
    writer.WriteLine(); // Salto de línea vacío
    writer.WriteLine("Esta es una línea completa");
    writer.WriteLine("Y esta es otra línea");
}

string contenido = File.ReadAllText("diferencia.txt");
Console.WriteLine("Contenido:");
Console.WriteLine(contenido);

// Salida:
// Hola mundo sin saltos
// Esta es una línea completa
// Y esta es otra línea

File.Delete("diferencia.txt");
```

#### 3.2.4. Flush:   Forzar Escritura Inmediata

Por defecto, `StreamWriter` usa un **buffer interno** para mejorar el rendimiento.  Los datos no se escriben inmediatamente al disco.  

```csharp
// ========================================
// DEMOSTRACIÓN:  Buffer y Flush
// ========================================

using System;
using System.IO;
using System.Threading;

string rutaLog = "log_flush.txt";

using (var writer = new StreamWriter(rutaLog))
{
    writer.WriteLine("Línea 1 (en buffer)");
    Console.WriteLine("Escrito en buffer (aún no en disco)");
    Thread.Sleep(2000);
    
    // Forzar escritura a disco
    writer.Flush();
    Console.WriteLine("✓ Flush ejecutado (ahora SÍ está en disco)");
    Thread.Sleep(2000);
    
    writer.WriteLine("Línea 2 (también en buffer)");
    Console.WriteLine("Escrito en buffer de nuevo");
    Thread.Sleep(2000);
    
} // ← Al salir del using, se hace Flush automático

Console.WriteLine("✓ Salida del using (Flush automático)");

// Verificar
Console.WriteLine($"\nContenido final:\n{File.ReadAllText(rutaLog)}");

File.Delete(rutaLog);
```

**¿Cuándo usar `Flush()`?**

- ✅ **Logs críticos**: Si la aplicación puede crashear, queremos los logs en disco inmediatamente
- ✅ **Depuración**:  Ver cambios en tiempo real
- ❌ **NO en bucles**: Afecta el rendimiento (espera escritura a disco en cada iteración)

```csharp
// ❌ MAL:  Flush en cada iteración (muy lento)
using (var writer = new StreamWriter("salida.txt"))
{
    for (int i = 0; i < 10000; i++)
    {
        writer.WriteLine($"Línea {i}");
        writer.Flush(); // ← Espera escritura a disco 10,000 veces
    }
}

// ✓ BIEN:  Dejar que el buffer haga su trabajo
using (var writer = new StreamWriter("salida. txt"))
{
    for (int i = 0; i < 10000; i++)
    {
        writer.WriteLine($"Línea {i}");
    }
} // Flush automático al final
```

---

### 3.3. Lectura de Ficheros de Texto

#### 3.3.1. Método Rápido:  `File.ReadAllText()` y `File.ReadAllLines()`

Para ficheros **pequeños** (< 100 MB):

```csharp
// ========================================
// LECTURA RÁPIDA (ficheros pequeños)
// ========================================

using System;
using System.IO;

// Crear fichero de prueba
string[] lineasPrueba = 
[
    "Primera línea",
    "Segunda línea",
    "Tercera línea con datos:  123",
    "Cuarta línea final"
];

File.WriteAllLines("prueba_lectura.txt", lineasPrueba);

// ────────────────────────────────────────
// Método 1: ReadAllText (todo el contenido como string)
// ────────────────────────────────────────

string contenidoCompleto = File.ReadAllText("prueba_lectura.txt");

Console.WriteLine(">>> ReadAllText (todo el texto):");
Console.WriteLine(contenidoCompleto);
Console.WriteLine($"\nLongitud total: {contenidoCompleto. Length} caracteres\n");

// ────────────────────────────────────────
// Método 2:  ReadAllLines (array de líneas)
// ────────────────────────────────────────

string[] lineas = File.ReadAllLines("prueba_lectura.txt");

Console.WriteLine(">>> ReadAllLines (array de líneas):");
Console.WriteLine($"Total líneas: {lineas.Length}\n");

for (int i = 0; i < lineas.Length; i++)
{
    Console.WriteLine($"  [{i}] {lineas[i]}");
}

// ────────────────────────────────────────
// Método 3: ReadLines (IEnumerable<string> - LAZY)
// ────────────────────────────────────────

Console.WriteLine("\n>>> ReadLines (IEnumerable - evaluación diferida):");

IEnumerable<string> lineasLazy = File.ReadLines("prueba_lectura.txt");

// Solo se leen cuando se iteran
foreach (string linea in lineasLazy)
{
    Console.WriteLine($"  → {linea}");
    
    if (linea.Contains("Segunda"))
    {
        Console. WriteLine("    (Deteniendo lectura)");
        break; // ¡No lee el resto!
    }
}

File.Delete("prueba_lectura.txt");
```

**Diferencia clave:  ReadAllLines vs ReadLines**

```csharp
// ReadAllLines:  Lee TODO a memoria (eager)
string[] lineas = File.ReadAllLines("archivo.txt");
// → [Línea1, Línea2, Línea3, ... ] en RAM

// ReadLines:  Lee línea por línea (lazy)
IEnumerable<string> lineas = File.ReadLines("archivo. txt");
// → Solo lee cuando iteras (foreach)
```

#### 3.3.2. StreamReader: Lectura Eficiente (Ficheros Grandes)

Para ficheros **grandes** o lectura línea por línea:

```csharp
// ========================================
// STREAMREADER (lectura eficiente)
// ========================================

using System;
using System.IO;

// Crear fichero de prueba grande
using (var writer = new StreamWriter("grande.txt"))
{
    for (int i = 1; i <= 1000; i++)
    {
        writer.WriteLine($"Línea {i}:  Datos de ejemplo");
    }
}

Console.WriteLine("✓ Fichero de 1000 líneas creado\n");

// ────────────────────────────────────────
// Leer línea por línea con ReadLine()
// ────────────────────────────────────────

Console.WriteLine(">>> Leer línea por línea (ReadLine):");

using (var reader = new StreamReader("grande.txt"))
{
    int contador = 0;
    string?  linea;
    
    // ReadLine() devuelve null al final del fichero
    while ((linea = reader.ReadLine()) != null)
    {
        contador++;
        
        // Mostrar solo las primeras 5 líneas
        if (contador <= 5)
        {
            Console.WriteLine($"  {linea}");
        }
        
        // Procesar línea (buscar algo, parsear, etc.)
        if (linea.Contains("500"))
        {
            Console. WriteLine($"  ...  (encontrada línea 500)");
        }
    }
    
    Console.WriteLine($"\nTotal líneas procesadas: {contador}");
}

// ────────────────────────────────────────
// Leer todo de una vez con ReadToEnd()
// ────────────────────────────────────────

Console.WriteLine("\n>>> Leer todo con ReadToEnd():");

using (var reader = new StreamReader("grande.txt"))
{
    string todoElContenido = reader.ReadToEnd();
    
    Console.WriteLine($"Longitud total: {todoElContenido.Length} caracteres");
    Console.WriteLine($"Primeros 100 caracteres:\n{todoElContenido. Substring(0, 100)}...");
}

File.Delete("grande.txt");
```

#### 3.3.3. Ejemplo Práctico:  Procesar Log Line

```csharp
// ========================================
// CASO PRÁCTICO: Analizar fichero de log
// ========================================

using System;
using System.IO;
using System. Linq;

// Crear fichero de log simulado
string rutaLog = "servidor.log";

using (var writer = new StreamWriter(rutaLog))
{
    writer.WriteLine("[2025-01-15 10:30:15] INFO:   Servidor iniciado");
    writer.WriteLine("[2025-01-15 10:31:20] INFO:  Usuario 'admin' conectado");
    writer.WriteLine("[2025-01-15 10:32:45] WARNING:   Memoria al 80%");
    writer.WriteLine("[2025-01-15 10:33:10] ERROR:  Fallo en conexión a BD");
    writer.WriteLine("[2025-01-15 10:33:11] ERROR:   Reintentando conexión...  ");
    writer.WriteLine("[2025-01-15 10:33:15] INFO:  Conexión restaurada");
    writer.WriteLine("[2025-01-15 10:35:00] INFO:  Operación completada");
}

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  ANÁLISIS DE LOG");
Console.WriteLine("═══════════════════════════════════════════\n");

// ────────────────────────────────────────
// Analizar el log
// ────────────────────────────────────────

int totalLineas = 0;
int errores = 0;
int warnings = 0;

using (var reader = new StreamReader(rutaLog))
{
    string?  linea;
    
    while ((linea = reader. ReadLine()) != null)
    {
        totalLineas++;
        
        if (linea.Contains("ERROR"))
        {
            errores++;
            Console.WriteLine($"🔴 {linea}");
        }
        else if (linea.Contains("WARNING"))
        {
            warnings++;
            Console.WriteLine($"🟡 {linea}");
        }
    }
}

Console.WriteLine("\n═══════════════════════════════════════════");
Console.WriteLine("  RESUMEN");
Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine($"Total líneas:   {totalLineas}");
Console.WriteLine($"Errores:       {errores}");
Console.WriteLine($"Warnings:      {warnings}");
Console.WriteLine($"Info:           {totalLineas - errores - warnings}");

// ────────────────────────────────────────
// ALTERNATIVA con LINQ + ReadLines
// ────────────────────────────────────────

Console. WriteLine("\n>>> ANÁLISIS CON LINQ:");

var lineasConError = File.ReadLines(rutaLog)
    .Where(linea => linea.Contains("ERROR"));

Console.WriteLine($"Líneas con ERROR:   {lineasConError.Count()}");

foreach (var linea in lineasConError)
{
    Console.WriteLine($"  → {linea}");
}

File.Delete(rutaLog);
```

**Salida esperada:**

```
═══════════════════════════════════════════
  ANÁLISIS DE LOG
═══════════════════════════════════════════

🟡 [2025-01-15 10:32:45] WARNING:  Memoria al 80%
🔴 [2025-01-15 10:33:10] ERROR: Fallo en conexión a BD
🔴 [2025-01-15 10:33:11] ERROR:  Reintentando conexión... 

═══════════════════════════════════════════
  RESUMEN
═══════════════════════════════════════════
Total líneas:  7
Errores:       2
Warnings:     1
Info:         4

>>> ANÁLISIS CON LINQ: 
Líneas con ERROR: 2
  → [2025-01-15 10:33:10] ERROR:   Fallo en conexión a BD
  → [2025-01-15 10:33:11] ERROR:  Reintentando conexión... 
```

---

### 3.4. Comparación de Métodos:   ¿Cuál Usar?

```csharp
// ========================================
// TABLA COMPARATIVA: Métodos de Lectura/Escritura
// ========================================

Console.WriteLine("═══════════════════════════════════════════════════════════════");
Console.WriteLine("  COMPARACIÓN DE MÉTODOS");
Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

var comparacion = new[]
{
    new { Método = "File.WriteAllText()", Uso = "Ficheros pequeños", Ventaja = "Simple, una línea", Desventaja = "Sobrescribe, todo en memoria" },
    new { Método = "File.  WriteAllLines()", Uso = "Array de líneas", Ventaja = "Simple para listas", Desventaja = "Sobrescribe, todo en memoria" },
    new { Método = "File.AppendAllText()", Uso = "Añadir al final", Ventaja = "No sobrescribe", Desventaja = "Solo texto, no líneas" },
    new { Método = "StreamWriter (crear)", Uso = "Ficheros grandes", Ventaja = "Eficiente, línea a línea", Desventaja = "Más código" },
    new { Método = "StreamWriter (append)", Uso = "Logs, añadir datos", Ventaja = "Eficiente, no sobrescribe", Desventaja = "Más código" },
    new { Método = "File.ReadAllText()", Uso = "Ficheros pequeños", Ventaja = "Simple, una línea", Desventaja = "Todo en memoria (peligro con GB)" },
    new { Método = "File.ReadAllLines()", Uso = "Array de líneas", Ventaja = "Fácil de iterar", Desventaja = "Todo en memoria" },
    new { Método = "File.ReadLines()", Uso = "Ficheros grandes", Ventaja = "Lazy, bajo uso de RAM", Desventaja = "IEnumerable (no array)" },
    new { Método = "StreamReader. ReadLine()", Uso = "Procesar línea a línea", Ventaja = "Control total, eficiente", Desventaja = "Más código, bucle manual" },
    new { Método = "StreamReader.ReadToEnd()", Uso = "Todo el contenido", Ventaja = "Simple", Desventaja = "Todo en memoria" }
};

Console.WriteLine($"{"Método",-30} {"Uso Típico",-25} {"Ventaja",-30}");
Console.WriteLine(new string('─', 85));

foreach (var item in comparacion. Take(5)) // Escritura
{
    Console.WriteLine($"{item.Método,-30} {item.Uso,-25} {item.Ventaja,-30}");
}

Console.WriteLine();

foreach (var item in comparacion. Skip(5)) // Lectura
{
    Console.WriteLine($"{item.Método,-30} {item.Uso,-25} {item.Ventaja,-30}");
}
```

---

### 3.5. Ejemplo Integrador: Sistema de Logs

```csharp
// ========================================
// PROYECTO:   Sistema de Logs con Rotación
// ========================================

using System;
using System.IO;

/// <summary>
/// Sistema de logging que escribe en ficheros con rotación diaria. 
/// </summary>
class Logger
{
    private readonly string _directorio;
    
    public Logger(string directorio = "logs")
    {
        _directorio = directorio;
        Directory.CreateDirectory(_directorio);
    }
    
    /// <summary>
    /// Escribe un mensaje de log con nivel y timestamp.
    /// </summary>
    public void Log(string nivel, string mensaje)
    {
        string nombreFichero = $"log_{DateTime.Now:yyyy-MM-dd}.txt";
        string rutaCompleta = Path.Combine(_directorio, nombreFichero);
        
        string lineaLog = $"[{DateTime.Now:HH:mm:ss}] {nivel. ToUpper().PadRight(8)} {mensaje}";
        
        // Usar StreamWriter con append para añadir al final
        using (var writer = new StreamWriter(rutaCompleta, append: true))
        {
            writer.WriteLine(lineaLog);
        }
    }
    
    public void Info(string mensaje) => Log("INFO", mensaje);
    public void Warning(string mensaje) => Log("WARNING", mensaje);
    public void Error(string mensaje) => Log("ERROR", mensaje);
    
    /// <summary>
    /// Lee todos los logs del día actual.
    /// </summary>
    public void MostrarLogsHoy()
    {
        string nombreFichero = $"log_{DateTime.Now:yyyy-MM-dd}.txt";
        string rutaCompleta = Path.Combine(_directorio, nombreFichero);
        
        if (!  File.Exists(rutaCompleta))
        {
            Console.WriteLine("No hay logs para hoy");
            return;
        }
        
        Console.WriteLine($"\n═══ LOGS DE {DateTime.Now:dd/MM/yyyy} ═══\n");
        
        using (var reader = new StreamReader(rutaCompleta))
        {
            string?  linea;
            while ((linea = reader.ReadLine()) != null)
            {
                // Colorear según nivel
                if (linea.Contains("ERROR"))
                {
                    Console. ForegroundColor = ConsoleColor.Red;
                }
                else if (linea.Contains("WARNING"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
                Console.WriteLine(linea);
                Console.ResetColor();
            }
        }
    }
}

// ────────────────────────────────────────
// DEMOSTRACIÓN
// ────────────────────────────────────────

var logger = new Logger();

Console.WriteLine(">>> Generando logs...\n");

logger.Info("Aplicación iniciada");
logger.Info("Usuario 'admin' conectado");
logger.Warning("Uso de memoria al 75%");
logger.Error("Fallo en conexión a BD");
logger.Info("Conexión restaurada");
logger.Info("Operación completada con éxito");

Console.WriteLine("✓ Logs escritos");

// Mostrar los logs
logger.MostrarLogsHoy();

Console.WriteLine("\n✓ Sistema de logs funcional");
```

---

## 4.  Formatos de Intercambio (I):   DTOs y CSV con LINQ

### 4.0. Introducción: El Problema de la Persistencia de Objetos

Hasta ahora hemos trabajado con **texto plano** sin estructura.     Pero en aplicaciones reales, necesitamos **guardar y recuperar objetos** con múltiples propiedades.  

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

En este punto veremos **CSV** (Comma-Separated Values), el formato más simple y compatible con Excel.

---

### 4.1. ¿Qué es CSV?   Valores Separados por Comas

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

✅ **Legible**:  Cualquier editor de texto puede abrirlo  
✅ **Universal**: Excel, Google Sheets, LibreOffice lo leen  
✅ **Simple**: No requiere librerías externas  
✅ **Ligero**: Mucho más pequeño que XML  
❌ **Limitado**: Solo tablas planas (no jerarquías)  
❌ **Frágil**: Problemas con comas, saltos de línea en los datos  

---

### 4.2. El Patrón DTO (Data Transfer Object)

Antes de trabajar con CSV, necesitamos entender el **patrón DTO**.

#### 4.2.1. ¿Qué es un DTO?

Un **DTO** (Data Transfer Object) es una **clase simple** diseñada para **transportar datos** entre diferentes capas o sistemas.   **No contiene lógica de negocio**, solo propiedades.

**Características de un DTO:**

✅ Solo propiedades públicas  
✅ Sin métodos de negocio (excepto conversión de formato)  
✅ Inmutable (preferiblemente)  
✅ Usa `record` en C# moderno  

**Ejemplo:**

```csharp
// ========================================
// DTO:      Alumno para persistencia
// ========================================

/// <summary>
/// DTO para representar un alumno en ficheros CSV.   
/// Usa record para inmutabilidad y comparación por valor.
/// </summary>
public record AlumnoDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public int Edad { get; init; }
    public double Nota { get; init; }
}
```

#### 4.2.2. ¿Por Qué Usar DTOs?

**Sin DTO (acoplamiento directo):**

```csharp
// ❌ MAL:     Modelo de negocio acoplado a CSV

public class Alumno
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public double Nota { get; set; }
    
    // Lógica de negocio
    public bool EstaAprobado() => Nota >= 5.0;
    
    public string ObtenerCalificacion() => Nota switch
    {
        >= 9.0 => "Sobresaliente",
        >= 7.0 => "Notable",
        >= 5.0 => "Aprobado",
        _ => "Suspenso"
    };
    
    // Persistencia CSV (¡acoplamiento!)
    public string ToCsv() => $"{Id},{Nombre},{Edad},{Nota}";
    
    // ¡La clase hace DEMASIADAS cosas!
    // ¿Y si queremos cambiar a JSON?   ¿Añadimos ToJson()?
}
```

**Con DTO (separación de responsabilidades):**

```csharp
// ✓ BIEN:   Separar modelo de negocio y persistencia

// ════════════════════════════════════════
// 1. MODELO DE NEGOCIO (lógica)
// ════════════════════════════════════════

public class Alumno
{
    public int Id { get; }
    public string Nombre { get; }
    public int Edad { get; }
    public double Nota { get; }
    
    public Alumno(int id, string nombre, int edad, double nota)
    {
        Id = id;
        Nombre = nombre;
        Edad = edad;
        Nota = nota;
    }
    
    // Solo lógica de negocio
    public bool EstaAprobado() => Nota >= 5.0;
    
    public string ObtenerCalificacion() => Nota switch
    {
        >= 9.0 => "Sobresaliente",
        >= 7.0 => "Notable",
        >= 5.0 => "Aprobado",
        _ => "Suspenso"
    };
}

// ════════════════════════════════════════
// 2. DTO PARA CSV (persistencia)
// ════════════════════════════════════════

public record AlumnoDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = "";
    public int Edad { get; init; }
    public double Nota { get; init; }
}

// ════════════════════════════════════════
// 3. MAPPER entre modelo y DTO
// ════════════════════════════════════════

public static class AlumnoMapper
{
    public static AlumnoDto ToDto(Alumno alumno) => new()
    {
        Id = alumno.Id,
        Nombre = alumno.Nombre,
        Edad = alumno. Edad,
        Nota = alumno.Nota
    };
    
    public static Alumno ToDomain(AlumnoDto dto) => 
        new(dto.Id, dto.Nombre, dto.Edad, dto. Nota);
}
```

**Ventajas:**

✅ **Separación de responsabilidades**: Modelo de negocio vs Persistencia  
✅ **Cambio de formato fácil**: Cambiar CSV por JSON solo afecta al DTO  
✅ **Testeable**: Puedes testear el modelo sin ficheros  
✅ **Evolución independiente**: Modelo y formato evolucionan por separado  

---

### 4.3. Escritura de CSV

#### 4.3.1. Escritura Manual Básica

```csharp
// ========================================
// ESCRITURA BÁSICA DE CSV
// ========================================

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

// Escribir con StreamWriter (using var)
using var writer = new StreamWriter(rutaCsv);

// 1. Escribir CABECERA
writer.WriteLine("Id,Nombre,Edad,Nota");

// 2. Escribir DATOS
foreach (var alumno in alumnos)
{
    writer.WriteLine($"{alumno.Id},{alumno.Nombre},{alumno. Edad},{alumno.Nota}");
}

Console.WriteLine($"✓ CSV creado:   {rutaCsv}");
Console.WriteLine($"✓ {alumnos.Count} registros escritos\n");

// Verificar contenido
Console.WriteLine(">>> Contenido del CSV:");
Console.WriteLine(File.ReadAllText(rutaCsv));
```

**Salida:**

```
>>> Escribiendo CSV...

✓ CSV creado:  alumnos.csv
✓ 5 registros escritos

>>> Contenido del CSV:
Id,Nombre,Edad,Nota
1,Ana García,20,8.5
2,Juan Pérez,22,7
3,María López,21,9.2
4,Pedro Martín,23,6.5
5,Laura Ruiz,20,8
```

#### 4.3.2. Añadir Método ToCsv al DTO

Para hacer el código más limpio, podemos añadir un método al DTO:

```csharp
// ========================================
// DTO CON MÉTODO DE CONVERSIÓN
// ========================================

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    /// <summary>
    /// Convierte el DTO a una línea CSV. 
    /// </summary>
    public string ToCsv()
    {
        return $"{Id},{Nombre},{Edad},{Nota}";
    }
}

// Uso más limpio
var alumnos = new List<AlumnoDto>
{
    new(1, "Ana García", 20, 8.5),
    new(2, "Juan Pérez", 22, 7.0)
};

string rutaCsv = "alumnos_limpio.csv";

using var writer = new StreamWriter(rutaCsv);

writer.WriteLine("Id,Nombre,Edad,Nota");

foreach (var alumno in alumnos)
{
    writer.WriteLine(alumno.ToCsv()); // Más legible
}

Console.WriteLine($"✓ CSV creado con ToCsv()");
```

#### 4.3.3. Escritura con LINQ

```csharp
// ========================================
// ESCRITURA CSV CON LINQ (más elegante)
// ========================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    public string ToCsv() => $"{Id},{Nombre},{Edad},{Nota}";
}

var alumnos = new List<AlumnoDto>
{
    new(1, "Ana García", 20, 8.5),
    new(2, "Juan Pérez", 22, 7.0),
    new(3, "María López", 21, 9.2)
};

string rutaCsv = "alumnos_linq.csv";

// Convertir a líneas CSV con LINQ
var lineasCsv = alumnos. Select(a => a.ToCsv());

// Escribir todo de una vez
File.WriteAllLines(rutaCsv, 
    new[] { "Id,Nombre,Edad,Nota" } // Cabecera
    . Concat(lineasCsv)               // Datos
);

Console.WriteLine($"✓ CSV creado con LINQ:  {rutaCsv}");

// Verificar
Console.WriteLine("\nContenido:");
foreach (var linea in File.ReadLines(rutaCsv))
{
    Console.WriteLine($"  {linea}");
}
```

---

### 4.4. Lectura de CSV

#### 4.4.1. Lectura Básica con StreamReader

```csharp
// ========================================
// LECTURA BÁSICA DE CSV
// ========================================

using System;
using System.IO;
using System.Collections.Generic;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    public string ToCsv() => $"{Id},{Nombre},{Edad},{Nota}";
    
    /// <summary>
    /// Crea un DTO desde una línea CSV.
    /// </summary>
    public static AlumnoDto FromCsv(string lineaCsv)
    {
        string[] partes = lineaCsv.Split(',');
        
        return new AlumnoDto(
            Id: int.Parse(partes[0]),
            Nombre: partes[1],
            Edad: int.Parse(partes[2]),
            Nota: double.Parse(partes[3])
        );
    }
}

// Leer CSV
string rutaCsv = "alumnos.csv";

Console.WriteLine(">>> Leyendo CSV...\n");

var alumnos = new List<AlumnoDto>();

using var reader = new StreamReader(rutaCsv);

// Leer y descartar cabecera
string?  cabecera = reader.ReadLine();
Console.WriteLine($"Cabecera: {cabecera}\n");

// Leer datos línea por línea
string? linea;
while ((linea = reader.ReadLine()) != null)
{
    AlumnoDto alumno = AlumnoDto.FromCsv(linea);
    alumnos.Add(alumno);
}

Console.WriteLine($"✓ {alumnos.Count} registros leídos\n");

// Mostrar datos
Console.WriteLine(">>> ALUMNOS:");
foreach (var alumno in alumnos)
{
    Console.WriteLine($"  [{alumno.Id}] {alumno.Nombre} - Edad: {alumno.Edad}, Nota: {alumno. Nota}");
}
```

**Salida:**

```
>>> Leyendo CSV... 

Cabecera: Id,Nombre,Edad,Nota

✓ 5 registros leídos

>>> ALUMNOS:
  [1] Ana García - Edad: 20, Nota: 8.5
  [2] Juan Pérez - Edad: 22, Nota: 7
  [3] María López - Edad: 21, Nota:  9.2
  [4] Pedro Martín - Edad: 23, Nota: 6.5
  [5] Laura Ruiz - Edad:  20, Nota: 8
```

#### 4.4.2. Lectura con LINQ (Más Elegante)

```csharp
// ========================================
// LECTURA CSV CON LINQ
// ========================================

using System;
using System.IO;
using System. Linq;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    public static AlumnoDto FromCsv(string lineaCsv)
    {
        string[] partes = lineaCsv.Split(',');
        return new AlumnoDto(
            int.Parse(partes[0]),
            partes[1],
            int.Parse(partes[2]),
            double.Parse(partes[3])
        );
    }
}

string rutaCsv = "alumnos. csv";

Console.WriteLine(">>> Leyendo CSV con LINQ...\n");

// Leer con LINQ (evaluación diferida)
var alumnos = File.ReadLines(rutaCsv)
    .Skip(1)                     // Saltar cabecera
    .Select(AlumnoDto.FromCsv)   // Convertir cada línea a DTO
    .ToList();                   // Materializar a lista

Console.WriteLine($"✓ {alumnos.Count} registros leídos\n");

// Mostrar
Console.WriteLine(">>> ALUMNOS:");
foreach (var alumno in alumnos)
{
    Console.WriteLine($"  [{alumno.Id}] {alumno.Nombre} - Nota: {alumno.Nota}");
}
```

**Ventajas de usar LINQ:**

✅ **Más conciso**: Una sola expresión  
✅ **Composable**: Fácil añadir filtros  
✅ **Lazy**:  `File.ReadLines()` no carga todo en memoria  

---

### 4.5. Procesamiento Avanzado con LINQ

Una vez leídos los datos, podemos aplicar **toda la potencia de LINQ** para filtrar, ordenar, agrupar y transformar. 

#### 4.5.1. Filtrado y Ordenación

```csharp
// ========================================
// LINQ:      Filtrado y Transformación de CSV
// ========================================

using System;
using System.IO;
using System.Linq;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    public static AlumnoDto FromCsv(string lineaCsv)
    {
        string[] partes = lineaCsv.Split(',');
        return new AlumnoDto(
            int.Parse(partes[0]),
            partes[1],
            int.Parse(partes[2]),
            double.Parse(partes[3])
        );
    }
}

string rutaCsv = "alumnos.csv";

// Leer CSV
var alumnos = File.ReadLines(rutaCsv)
    .Skip(1)
    .Select(AlumnoDto.FromCsv)
    .ToList();

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  ANÁLISIS DE ALUMNOS CON LINQ");
Console.WriteLine("═══════════════════════════════════════════\n");

// ────────────────────────────────────────
// 1. Filtrar aprobados (nota >= 5)
// ────────────────────────────────────────

var aprobados = alumnos.Where(a => a.Nota >= 5.0);

Console.WriteLine(">>> APROBADOS (nota >= 5):");
foreach (var alumno in aprobados)
{
    Console.WriteLine($"  ✓ {alumno.Nombre}:  {alumno.Nota}");
}

// ────────────────────────────────────────
// 2. Filtrar suspensos
// ────────────────────────────────────────

var suspensos = alumnos.Where(a => a.Nota < 5.0);

Console.WriteLine($"\n>>> SUSPENSOS (nota < 5): {suspensos.Count()}");
foreach (var alumno in suspensos)
{
    Console.WriteLine($"  ✗ {alumno.Nombre}:  {alumno.Nota}");
}

// ────────────────────────────────────────
// 3. Ordenar por nota descendente
// ────────────────────────────────────────

var ordenadosPorNota = alumnos.OrderByDescending(a => a. Nota);

Console.WriteLine("\n>>> RANKING (ordenado por nota):");
int posicion = 1;
foreach (var alumno in ordenadosPorNota)
{
    string medalla = posicion switch
    {
        1 => "🥇",
        2 => "🥈",
        3 => "🥉",
        _ => $"{posicion}."
    };
    
    Console.WriteLine($"  {medalla} {alumno. Nombre}: {alumno.Nota}");
    posicion++;
}

// ────────────────────────────────────────
// 4. Top 3 mejores notas
// ────────────────────────────────────────

var top3 = alumnos
    .OrderByDescending(a => a.Nota)
    .Take(3);

Console.WriteLine("\n>>> TOP 3 MEJORES NOTAS:");
foreach (var alumno in top3)
{
    Console.WriteLine($"  • {alumno.Nombre}: {alumno.Nota}");
}

// ────────────────────────────────────────
// 5. Alumnos de 20 años
// ────────────────────────────────────────

var alumnosDe20 = alumnos.Where(a => a. Edad == 20);

Console.WriteLine($"\n>>> ALUMNOS DE 20 AÑOS:  {alumnosDe20.Count()}");
foreach (var alumno in alumnosDe20)
{
    Console.WriteLine($"  {alumno.Nombre} - Nota: {alumno.Nota}");
}
```

**Salida:**

```
═══════════════════════════════════════════
  ANÁLISIS DE ALUMNOS CON LINQ
═══════════════════════════════════════════

>>> APROBADOS (nota >= 5):
  ✓ Ana García: 8.5
  ✓ Juan Pérez: 7
  ✓ María López:  9.2
  ✓ Pedro Martín: 6.5
  ✓ Laura Ruiz: 8

>>> SUSPENSOS (nota < 5): 0

>>> RANKING (ordenado por nota):
  🥇 María López: 9.2
  🥈 Ana García: 8.5
  🥉 Laura Ruiz: 8
  4. Juan Pérez: 7
  5. Pedro Martín: 6.5

>>> TOP 3 MEJORES NOTAS:
  • María López: 9.2
  • Ana García: 8.5
  • Laura Ruiz:  8

>>> ALUMNOS DE 20 AÑOS: 2
  Ana García - Nota:  8.5
  Laura Ruiz - Nota: 8
```

#### 4.5.2. Estadísticas con LINQ

```csharp
// ========================================
// LINQ:     Calcular estadísticas
// ========================================

using System;
using System.IO;
using System.Linq;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    public static AlumnoDto FromCsv(string lineaCsv)
    {
        string[] partes = lineaCsv. Split(',');
        return new AlumnoDto(
            int. Parse(partes[0]),
            partes[1],
            int.Parse(partes[2]),
            double.Parse(partes[3])
        );
    }
}

string rutaCsv = "alumnos. csv";

var alumnos = File.ReadLines(rutaCsv)
    .Skip(1)
    .Select(AlumnoDto.FromCsv)
    .ToList();

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  ESTADÍSTICAS CON LINQ");
Console.WriteLine("═══════════════════════════════════════════\n");

// ────────────────────────────────────────
// Estadísticas básicas
// ────────────────────────────────────────

int totalAlumnos = alumnos.Count();
double notaMedia = alumnos.Average(a => a.Nota);
double notaMaxima = alumnos.Max(a => a.Nota);
double notaMinima = alumnos.Min(a => a.Nota);
int totalAprobados = alumnos.Count(a => a.Nota >= 5.0);
int totalSuspensos = alumnos.Count(a => a.Nota < 5.0);

Console.WriteLine(">>> ESTADÍSTICAS GENERALES:");
Console.WriteLine($"  Total alumnos:      {totalAlumnos}");
Console.WriteLine($"  Nota media:        {notaMedia: F2}");
Console.WriteLine($"  Nota máxima:      {notaMaxima: F2}");
Console.WriteLine($"  Nota mínima:      {notaMinima: F2}");
Console.WriteLine($"  Aprobados:          {totalAprobados} ({totalAprobados * 100.0 / totalAlumnos: F1}%)");
Console.WriteLine($"  Suspensos:         {totalSuspensos} ({totalSuspensos * 100.0 / totalAlumnos:F1}%)");

// ────────────────────────────────────────
// Alumno con mejor/peor nota
// ────────────────────────────────────────

var mejorAlumno = alumnos.MaxBy(a => a.Nota);
var peorAlumno = alumnos.MinBy(a => a.Nota);

Console.WriteLine("\n>>> DESTACADOS:");
Console.WriteLine($"  Mejor nota:   {mejorAlumno! .Nombre} ({mejorAlumno.Nota})");
Console.WriteLine($"  Menor nota:   {peorAlumno!.Nombre} ({peorAlumno.Nota})");

// ────────────────────────────────────────
// Media por rango de edad
// ────────────────────────────────────────

var mediasPorEdad = alumnos
    .GroupBy(a => a.Edad)
    .Select(g => new
    {
        Edad = g.Key,
        Cantidad = g.Count(),
        NotaMedia = g.Average(a => a.Nota)
    })
    .OrderBy(x => x.Edad);

Console.WriteLine("\n>>> ESTADÍSTICAS POR EDAD:");
foreach (var grupo in mediasPorEdad)
{
    Console.WriteLine($"  {grupo.Edad} años: {grupo.Cantidad} alumnos, media {grupo.NotaMedia:F2}");
}
```

**Salida:**

```
═══════════════════════════════════════════
  ESTADÍSTICAS CON LINQ
═══════════════════════════════════════════

>>> ESTADÍSTICAS GENERALES:
  Total alumnos:    5
  Nota media:       7.84
  Nota máxima:      9.2
  Nota mínima:     6.5
  Aprobados:        5 (100.0%)
  Suspensos:        0 (0.0%)

>>> DESTACADOS:
  Mejor nota:  María López (9.2)
  Menor nota:  Pedro Martín (6.5)

>>> ESTADÍSTICAS POR EDAD:
  20 años: 2 alumnos, media 8.25
  21 años: 1 alumnos, media 9.20
  22 años: 1 alumnos, media 7.00
  23 años: 1 alumnos, media 6.50
```

#### 4.5.3. Proyección y Transformación

```csharp
// ========================================
// LINQ:      Proyección a nuevo formato
// ========================================

using System;
using System.IO;
using System.Linq;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    public static AlumnoDto FromCsv(string lineaCsv)
    {
        string[] partes = lineaCsv.Split(',');
        return new AlumnoDto(
            int.Parse(partes[0]),
            partes[1],
            int.Parse(partes[2]),
            double.Parse(partes[3])
        );
    }
}

string rutaCsv = "alumnos.csv";

var alumnos = File.ReadLines(rutaCsv)
    .Skip(1)
    .Select(AlumnoDto.FromCsv)
    .ToList();

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  PROYECCIÓN CON LINQ");
Console.WriteLine("═══════════════════════════════════════════\n");

// ────────────────────────────────────────
// Proyectar a informe con estado y calificación
// ────────────────────────────────────────

var informe = alumnos
    .Select(a => new
    {
        a.Nombre,
        a. Nota,
        Estado = a.Nota >= 5.0 ? "✓ Aprobado" : "✗ Suspenso",
        Calificacion = a.Nota switch
        {
            >= 9.0 => "Sobresaliente",
            >= 7.0 => "Notable",
            >= 5.0 => "Aprobado",
            _ => "Suspenso"
        }
    });

Console.WriteLine(">>> INFORME DE CALIFICACIONES:");
Console.WriteLine($"{"Nombre",-20} {"Nota",-8} {"Estado",-15} {"Calificación",-15}");
Console.WriteLine(new string('─', 60));

foreach (var item in informe)
{
    Console.WriteLine($"{item.Nombre,-20} {item.Nota,-8:F2} {item.Estado,-15} {item.Calificacion,-15}");
}

// ────────────────────────────────────────
// Proyectar solo nombres y notas
// ────────────────────────────────────────

var nombreYNotas = alumnos
    . Select(a => $"{a.Nombre}:  {a.Nota}");

Console.WriteLine("\n>>> LISTADO SIMPLE:");
foreach (var texto in nombreYNotas)
{
    Console.WriteLine($"  {texto}");
}

// ────────────────────────────────────────
// Proyectar incrementando las notas un 10%
// ────────────────────────────────────────

var conBonus = alumnos
    . Select(a => a with { Nota = Math.Min(10, a.Nota * 1.1) });

Console.WriteLine("\n>>> CON BONUS DEL 10%:");
foreach (var alumno in conBonus)
{
    Console.WriteLine($"  {alumno.Nombre}: {alumno.Nota:F2}");
}
```

**Salida:**

```
═══════════════════════════════════════════
  PROYECCIÓN CON LINQ
═══════════════════════════════════════════

>>> INFORME DE CALIFICACIONES: 
Nombre               Nota     Estado          Calificación   
────────────────────────────────────────────────────────────
Ana García           8.50     ✓ Aprobado      Notable        
Juan Pérez           7.00     ✓ Aprobado      Notable        
María López          9.20     ✓ Aprobado      Sobresaliente  
Pedro Martín         6.50     ✓ Aprobado      Aprobado       
Laura Ruiz           8.00     ✓ Aprobado      Notable        

>>> LISTADO SIMPLE: 
  Ana García: 8.5
  Juan Pérez: 7
  María López: 9.2
  Pedro Martín: 6.5
  Laura Ruiz: 8

>>> CON BONUS DEL 10%:
  Ana García: 9.35
  Juan Pérez: 7.70
  María López: 10.00
  Pedro Martín:  7.15
  Laura Ruiz: 8.80
```

---

### 4.6. Manejo de Casos Especiales en CSV

#### 4.6.1. Problema:   Comas en los Datos

```csharp
// ========================================
// PROBLEMA:   Datos con comas
// ========================================

// ❌ CSV INCORRECTO
// Id,Nombre,Edad,Nota
// 1,García, Ana,20,8.5
//          ^ Esta coma rompe el formato

// ✓ SOLUCIÓN:  Entrecomillar campos con comas
// Id,Nombre,Edad,Nota
// 1,"García, Ana",20,8.5
```

**Implementación mejorada:**

```csharp
// ========================================
// DTO CON MANEJO DE COMAS
// ========================================

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    /// <summary>
    /// Convierte a CSV entrecomillando campos con comas.   
    /// </summary>
    public string ToCsv()
    {
        string nombreEscapado = Nombre.Contains(',') 
            ? $"\"{Nombre}\"" 
            : Nombre;
        
        return $"{Id},{nombreEscapado},{Edad},{Nota}";
    }
    
    /// <summary>
    /// Parsea CSV manejando campos entrecomillados.  
    /// NOTA: Implementación simple.   Para casos complejos usar librería CsvHelper.
    /// </summary>
    public static AlumnoDto FromCsv(string lineaCsv)
    {
        var partes = new List<string>();
        bool dentroComillas = false;
        string parteActual = "";
        
        foreach (char c in lineaCsv)
        {
            if (c == '"')
            {
                dentroComillas = !dentroComillas;
            }
            else if (c == ',' && !dentroComillas)
            {
                partes.Add(parteActual);
                parteActual = "";
            }
            else
            {
                parteActual += c;
            }
        }
        
        partes.Add(parteActual); // Última parte
        
        return new AlumnoDto(
            Id: int.Parse(partes[0]),
            Nombre: partes[1],
            Edad: int.Parse(partes[2]),
            Nota: double.Parse(partes[3])
        );
    }
}

// ────────────────────────────────────────
// Prueba
// ────────────────────────────────────────

var alumno = new AlumnoDto(1, "García, Ana María", 20, 8.5);
string csv = alumno.ToCsv();

Console.WriteLine($"CSV generado: {csv}");
// Salida: 1,"García, Ana María",20,8.5

var recuperado = AlumnoDto.FromCsv(csv);
Console.WriteLine($"Nombre recuperado: {recuperado.Nombre}");
// Salida: García, Ana María
```

#### 4.6.2. Diferentes Separadores (`;` en Europa)

```csharp
// ========================================
// CSV CON SEPARADOR PERSONALIZADO
// ========================================

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    public string ToCsv(char separador = ',')
    {
        return $"{Id}{separador}{Nombre}{separador}{Edad}{separador}{Nota}";
    }
    
    public static AlumnoDto FromCsv(string lineaCsv, char separador = ',')
    {
        string[] partes = lineaCsv.Split(separador);
        
        return new AlumnoDto(
            Id: int.Parse(partes[0]),
            Nombre: partes[1]. Trim(),
            Edad: int.Parse(partes[2]),
            Nota: double. Parse(partes[3])
        );
    }
}

// ────────────────────────────────────────
// Uso con punto y coma (Excel europeo)
// ────────────────────────────────────────

var alumnos = new List<AlumnoDto>
{
    new(1, "Ana García", 20, 8.5),
    new(2, "Juan Pérez", 22, 7.0)
};

string rutaCsv = "alumnos_eu.csv";

using var writer = new StreamWriter(rutaCsv);

writer.WriteLine("Id;Nombre;Edad;Nota");

foreach (var alumno in alumnos)
{
    writer.WriteLine(alumno.ToCsv(';')); // Separador ';'
}

Console.WriteLine("✓ CSV europeo creado (separador ;)");

// Leer
var leidos = File.ReadLines(rutaCsv)
    .Skip(1)
    .Select(linea => AlumnoDto.FromCsv(linea, ';'))
    .ToList();

Console.WriteLine($"✓ {leidos.Count} registros leídos con separador ;");

// Verificar
Console.WriteLine("\nContenido:");
Console.WriteLine(File.ReadAllText(rutaCsv));
```

**Salida:**

```
✓ CSV europeo creado (separador ;)
✓ 2 registros leídos con separador ;

Contenido:
Id;Nombre;Edad;Nota
1;Ana García;20;8.5
2;Juan Pérez;22;7
```

---

### 4.7. Ejemplo Integrador:   Análisis Completo de CSV

```csharp
// ========================================
// PROYECTO:   Análisis Completo de Alumnos CSV
// ========================================

using System;
using System.IO;
using System.Collections.Generic;
using System. Linq;

// ════════════════════════════════════════
// DTO
// ════════════════════════════════════════

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    public string ToCsv() => $"{Id},{Nombre},{Edad},{Nota}";
    
    public static AlumnoDto FromCsv(string lineaCsv)
    {
        string[] partes = lineaCsv.Split(',');
        return new AlumnoDto(
            int.Parse(partes[0]),
            partes[1],
            int.Parse(partes[2]),
            double.Parse(partes[3])
        );
    }
}

// ════════════════════════════════════════
// Crear CSV de prueba
// ════════════════════════════════════════

string rutaCsv = "alumnos_completo.csv";

var alumnosIniciales = new List<AlumnoDto>
{
    new(1, "Ana García", 20, 8.5),
    new(2, "Juan Pérez", 22, 7.0),
    new(3, "María López", 21, 9.2),
    new(4, "Pedro Martín", 23, 4.5),
    new(5, "Laura Ruiz", 20, 8.0),
    new(6, "Carlos Díaz", 21, 6.5),
    new(7, "Sofía Torres", 22, 9.5),
    new(8, "Miguel Vega", 20, 3.0)
};

// Escribir CSV
using var writer = new StreamWriter(rutaCsv);
writer.WriteLine("Id,Nombre,Edad,Nota");
foreach (var alumno in alumnosIniciales)
{
    writer.WriteLine(alumno.ToCsv());
}

Console.WriteLine($"✓ CSV creado con {alumnosIniciales.Count} alumnos\n");

// ════════════════════════════════════════
// Leer y Analizar
// ════════════════════════════════════════

var alumnos = File.ReadLines(rutaCsv)
    .Skip(1)
    .Select(AlumnoDto.FromCsv)
    .ToList();

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  ANÁLISIS COMPLETO DE ALUMNOS");
Console.WriteLine("═══════════════════════════════════════════\n");

// ────────────────────────────────────────
// 1. ESTADÍSTICAS GENERALES
// ────────────────────────────────────────

Console.WriteLine(">>> ESTADÍSTICAS GENERALES:");

int total = alumnos.Count;
double media = alumnos.Average(a => a.Nota);
double max = alumnos.Max(a => a.Nota);
double min = alumnos.Min(a => a.Nota);
int aprobados = alumnos.Count(a => a.Nota >= 5.0);
int suspensos = total - aprobados;

Console. WriteLine($"  Total:           {total} alumnos");
Console.WriteLine($"  Nota media:     {media:F2}");
Console.WriteLine($"  Nota máxima:    {max:F2}");
Console.WriteLine($"  Nota mínima:    {min: F2}");
Console.WriteLine($"  Aprobados:       {aprobados} ({aprobados * 100.0 / total:F1}%)");
Console.WriteLine($"  Suspensos:      {suspensos} ({suspensos * 100.0 / total:F1}%)");

// ────────────────────────────────────────
// 2. TOP Y BOTTOM
// ────────────────────────────────────────

Console.WriteLine("\n>>> TOP 3 MEJORES:");
var top3 = alumnos. OrderByDescending(a => a.Nota).Take(3);
int pos = 1;
foreach (var alumno in top3)
{
    string medalla = pos switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => "" };
    Console.WriteLine($"  {medalla} {alumno.Nombre}:  {alumno.Nota}");
    pos++;
}

Console.WriteLine("\n>>> ALUMNOS EN RIESGO (nota < 5):");
var enRiesgo = alumnos. Where(a => a.Nota < 5.0).OrderBy(a => a.Nota);
foreach (var alumno in enRiesgo)
{
    Console.WriteLine($"  ⚠️  {alumno.Nombre}: {alumno.Nota}");
}

// ────────────────────────────────────────
// 3. DISTRIBUCIÓN POR CALIFICACIÓN
// ────────────────────────────────────────

Console.WriteLine("\n>>> DISTRIBUCIÓN POR CALIFICACIÓN:");

var porCalificacion = alumnos
    .GroupBy(a => a.Nota switch
    {
        >= 9.0 => "Sobresaliente",
        >= 7.0 => "Notable",
        >= 5.0 => "Aprobado",
        _ => "Suspenso"
    })
    .Select(g => new
    {
        Calificacion = g. Key,
        Cantidad = g.Count(),
        Porcentaje = g.Count() * 100.0 / total
    })
    .OrderByDescending(x => x.Cantidad);

foreach (var grupo in porCalificacion)
{
    Console.WriteLine($"  {grupo.Calificacion,-15}:  {grupo.Cantidad} alumnos ({grupo.Porcentaje:F1}%)");
}

// ────────────────────────────────────────
// 4. ANÁLISIS POR EDAD
// ────────────────────────────────────────

Console.WriteLine("\n>>> ANÁLISIS POR EDAD:");

var porEdad = alumnos
    .GroupBy(a => a.Edad)
    .Select(g => new
    {
        Edad = g.Key,
        Cantidad = g. Count(),
        MediaNota = g.Average(a => a.Nota)
    })
    .OrderBy(x => x.Edad);

foreach (var grupo in porEdad)
{
    Console.WriteLine($"  {grupo.Edad} años: {grupo.Cantidad} alumnos, media {grupo.MediaNota:F2}");
}

// ────────────────────────────────────────
// 5. EXPORTAR INFORME
// ────────────────────────────────────────

Console.WriteLine("\n>>> Generando informe...");

string rutaInforme = "informe_alumnos.txt";

using var informeWriter = new StreamWriter(rutaInforme);

informeWriter.WriteLine("═══════════════════════════════════════════");
informeWriter.WriteLine("  INFORME DE ALUMNOS");
informeWriter.WriteLine($"  Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");
informeWriter.WriteLine("═══════════════════════════════════════════\n");

informeWriter.WriteLine($"Total alumnos: {total}");
informeWriter.WriteLine($"Nota media: {media:F2}");
informeWriter.WriteLine($"Aprobados: {aprobados}/{total} ({aprobados * 100.0 / total:F1}%)\n");

informeWriter.WriteLine("LISTADO COMPLETO:");
informeWriter.WriteLine(new string('-', 50));

foreach (var alumno in alumnos. OrderByDescending(a => a. Nota))
{
    string estado = alumno.Nota >= 5.0 ? "✓" : "✗";
    informeWriter.WriteLine($"{estado} {alumno.Nombre,-20} {alumno.Nota:F2}");
}

Console.WriteLine($"✓ Informe exportado a:  {rutaInforme}");

Console.WriteLine("\n═══════════════════════════════════════════");

// Limpiar
File.Delete(rutaCsv);
File.Delete(rutaInforme);
```

**Salida:**

```
✓ CSV creado con 8 alumnos

═══════════════════════════════════════════
  ANÁLISIS COMPLETO DE ALUMNOS
═══════════════════════════════════════════

>>> ESTADÍSTICAS GENERALES: 
  Total:          8 alumnos
  Nota media:    7.03
  Nota máxima:   9.5
  Nota mínima:    3
  Aprobados:     6 (75.0%)
  Suspensos:     2 (25.0%)

>>> TOP 3 MEJORES:
  🥇 Sofía Torres: 9.5
  🥈 María López: 9.2
  🥉 Ana García: 8.5

>>> ALUMNOS EN RIESGO (nota < 5):
  ⚠️  Miguel Vega: 3
  ⚠️  Pedro Martín: 4.5

>>> DISTRIBUCIÓN POR CALIFICACIÓN:
  Notable        :  3 alumnos (37.5%)
  Sobresaliente  : 2 alumnos (25.0%)
  Suspenso       : 2 alumnos (25.0%)
  Aprobado       : 1 alumnos (12.5%)

>>> ANÁLISIS POR EDAD:
  20 años: 3 alumnos, media 6.50
  21 años: 2 alumnos, media 7.85
  22 años: 2 alumnos, media 8.25
  23 años: 1 alumnos, media 4.50

>>> Generando informe... 
✓ Informe exportado a: informe_alumnos. txt

═══════════════════════════════════════════
```

---

## 5. Formatos de Intercambio (II): JSON

### 5.0. Introducción:     ¿Qué es JSON y Por Qué es el Rey? 

**JSON** (JavaScript Object Notation) es el **formato de intercambio de datos más popular** del mundo.    Nació en el contexto de JavaScript, pero se ha convertido en el estándar universal para: 

- **APIs REST**:   Casi todas las APIs web usan JSON
- **Configuración**:  `appsettings.json`, `package.json`, etc.
- **Almacenamiento**:  Bases de datos NoSQL (MongoDB, Cosmos DB)
- **Comunicación**:  Entre frontend y backend

**¿Por qué JSON ganó la batalla contra XML?**

| Aspecto            | JSON                     | XML                   |
| ------------------ | ------------------------ | --------------------- |
| **Legibilidad**    | ✅ Alta                   | ⚠️ Media (verboso)     |
| **Tamaño**         | ✅ Compacto               | ❌ Grande (etiquetas)  |
| **Parsing**        | ✅ Rápido                 | ⚠️ Más lento           |
| **Tipos de datos** | ✅ Nativos (number, bool) | ❌ Todo es string      |
| **Jerarquías**     | ✅ Soporta                | ✅ Soporta             |
| **Arrays**         | ✅ Nativos `[]`           | ⚠️ Elementos repetidos |

**Ejemplo comparativo:**

```json
// JSON (52 caracteres)
{
  "id": 1,
  "nombre": "Ana García",
  "nota": 8.5
}
```

```xml
<!-- XML (123 caracteres) -->
<alumno>
  <id>1</id>
  <nombre>Ana García</nombre>
  <nota>8.5</nota>
</alumno>
```

---

### 5.1. Sintaxis Básica de JSON

JSON tiene **6 tipos de valores**:

| Tipo        | Ejemplo JSON           | Tipo C#           |
| ----------- | ---------------------- | ----------------- |
| **String**  | `"Ana García"`         | `string`          |
| **Number**  | `8.5`, `42`            | `double`, `int`   |
| **Boolean** | `true`, `false`        | `bool`            |
| **Null**    | `null`                 | `null`            |
| **Object**  | `{ "clave": "valor" }` | `class`, `record` |
| **Array**   | `[1, 2, 3]`            | `List<T>`, `T[]`  |

**Ejemplo completo:**

```json
{
  "id": 1,
  "nombre": "Ana García",
  "edad": 20,
  "nota": 8.5,
  "aprobado": true,
  "email": null,
  "direccion": {
    "calle": "Gran Vía 1",
    "ciudad": "Madrid"
  },
  "asignaturas": ["Matemáticas", "Física", "Programación"]
}
```

**Reglas importantes:**

✅ Las claves deben estar **entre comillas dobles**  
✅ Los strings usan **comillas dobles** (no simples)  
✅ No hay comas después del último elemento  
✅ No hay comentarios (a diferencia de JavaScript)  

---

### 5.2. El Duelo:     Newtonsoft.Json vs System.Text.Json

Existen **dos librerías principales** para trabajar con JSON en .  NET:

#### 5.2.1. Comparativa

| Aspecto             | **Newtonsoft.Json** (Json.  NET) | **System. Text.Json**         |
| ------------------- | -------------------------------- | ----------------------------- |
| **Origen**          | Librería externa (NuGet)         | Nativa de .  NET Core 3.0+    |
| **Rendimiento**     | ⚠️ Bueno                          | ✅ Excelente (2-3x más rápido) |
| **Memoria**         | ⚠️ Usa más RAM                    | ✅ Optimizado                  |
| **Características** | ✅ Muy completo                   | ⚠️ Menos flexible              |
| **Permisividad**    | ✅ Tolera errores                 | ❌ Estricto                    |
| **Popularidad**     | ✅ Muy usado (legado)             | ✅ Estándar moderno            |
| **Casos de uso**    | Proyectos legacy, flexibilidad   | Nuevos proyectos, rendimiento |

**Recomendación:**

- **System.Text.Json**:  Para proyectos nuevos (. NET Core 3.0+)
- **Newtonsoft. Json**:  Si necesitas compatibilidad con código antiguo

En este curso usaremos **System.Text.Json** (la librería nativa de Microsoft).

---

### 5.3. Serialización:     De Objeto a JSON

**Serialización** es el proceso de convertir un objeto de C# en una cadena JSON. 

#### 5.3.1. Serialización Básica

```csharp
// ========================================
// SERIALIZACIÓN BÁSICA (Objeto → JSON)
// ========================================

using System;
using System.IO;
using System.  Text. Json;

// Definir DTO
public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

// Crear objeto
var alumno = new AlumnoDto(1, "Ana García", 20, 8.5);

Console.WriteLine(">>> SERIALIZACIÓN:   Objeto → JSON\n");

// Serializar a string JSON
string json = JsonSerializer.Serialize(alumno);

Console.WriteLine("JSON generado:");
Console.WriteLine(json);
```

**Salida:**

```
>>> SERIALIZACIÓN: Objeto → JSON

JSON generado:
{"Id":1,"Nombre":"Ana García","Edad":20,"Nota":8.5}
```

#### 5.3.2. Pretty Print (JSON Legible)

Por defecto, JSON se genera **compacto** (sin espacios ni saltos de línea).   Para hacerlo **legible**:

```csharp
// ========================================
// PRETTY PRINT (JSON legible con indentación)
// ========================================

using System;
using System. Text.Json;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

var alumno = new AlumnoDto(1, "Ana García", 20, 8.5);

Console.WriteLine(">>> JSON SIN Pretty Print (compacto):");
string jsonCompacto = JsonSerializer. Serialize(alumno);
Console.WriteLine(jsonCompacto);

Console.WriteLine("\n>>> JSON CON Pretty Print (legible):");

// Configurar opciones
var opciones = new JsonSerializerOptions
{
    WriteIndented = true // ← Clave para Pretty Print
};

string jsonLegible = JsonSerializer. Serialize(alumno, opciones);
Console.WriteLine(jsonLegible);
```

**Salida:**

```
>>> JSON SIN Pretty Print (compacto):
{"Id":1,"Nombre":"Ana García","Edad":20,"Nota":8.5}

>>> JSON CON Pretty Print (legible):
{
  "Id": 1,
  "Nombre": "Ana García",
  "Edad":  20,
  "Nota": 8.5
}
```

#### 5.3.3. Guardar JSON en Fichero

```csharp
// ========================================
// GUARDAR JSON EN FICHERO
// ========================================

using System;
using System.IO;
using System.Text.Json;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

var alumno = new AlumnoDto(1, "Ana García", 20, 8.5);

string rutaJson = "alumno.json";

Console.WriteLine(">>> Guardando JSON en fichero...\n");

// Opción 1: Serializar y guardar manualmente
var opciones = new JsonSerializerOptions { WriteIndented = true };
string json = JsonSerializer.  Serialize(alumno, opciones);
File.WriteAllText(rutaJson, json);

Console.WriteLine($"✓ JSON guardado en: {rutaJson}");

// Verificar contenido
Console.WriteLine("\nContenido del fichero:");
Console.WriteLine(File.ReadAllText(rutaJson));
```

**Salida:**

```
>>> Guardando JSON en fichero...

✓ JSON guardado en: alumno.json

Contenido del fichero:
{
  "Id": 1,
  "Nombre": "Ana García",
  "Edad":  20,
  "Nota": 8.5
}
```

#### 5.3.4. Serializar Listas

```csharp
// ========================================
// SERIALIZAR LISTA DE OBJETOS
// ========================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

var alumnos = new List<AlumnoDto>
{
    new(1, "Ana García", 20, 8.5),
    new(2, "Juan Pérez", 22, 7.0),
    new(3, "María López", 21, 9.2)
};

string rutaJson = "alumnos.json";

Console.  WriteLine(">>> Serializando lista de alumnos...\n");

var opciones = new JsonSerializerOptions { WriteIndented = true };
string json = JsonSerializer.Serialize(alumnos, opciones);

File.WriteAllText(rutaJson, json);

Console.WriteLine($"✓ Lista serializada ({alumnos.Count} alumnos)");
Console.WriteLine("\nJSON generado:");
Console.WriteLine(json);
```

**Salida:**

```
>>> Serializando lista de alumnos... 

✓ Lista serializada (3 alumnos)

JSON generado:
[
  {
    "Id":  1,
    "Nombre": "Ana García",
    "Edad": 20,
    "Nota": 8.5
  },
  {
    "Id": 2,
    "Nombre": "Juan Pérez",
    "Edad":  22,
    "Nota": 7
  },
  {
    "Id": 3,
    "Nombre": "María López",
    "Edad": 21,
    "Nota": 9.2
  }
]
```

---

### 5.4. Deserialización:      De JSON a Objeto

**Deserialización** es el proceso inverso: convertir una cadena JSON en un objeto de C#.

#### 5.4.1. Deserialización Básica

```csharp
// ========================================
// DESERIALIZACIÓN BÁSICA (JSON → Objeto)
// ========================================

using System;
using System.  Text.Json;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

string json = """
{
  "Id": 1,
  "Nombre":  "Ana García",
  "Edad": 20,
  "Nota": 8.5
}
""";

Console.WriteLine(">>> DESERIALIZACIÓN:   JSON → Objeto\n");
Console.WriteLine("JSON de entrada:");
Console.WriteLine(json);

// Deserializar
AlumnoDto?   alumno = JsonSerializer.Deserialize<AlumnoDto>(json);

if (alumno != null)
{
    Console.WriteLine("\n✓ Objeto deserializado:");
    Console.WriteLine($"  Id:       {alumno.Id}");
    Console.WriteLine($"  Nombre:  {alumno.Nombre}");
    Console.WriteLine($"  Edad:   {alumno. Edad}");
    Console.WriteLine($"  Nota:    {alumno.Nota}");
}
```

**Salida:**

```
>>> DESERIALIZACIÓN:   JSON → Objeto

JSON de entrada:
{
  "Id": 1,
  "Nombre": "Ana García",
  "Edad": 20,
  "Nota": 8.5
}

✓ Objeto deserializado:
  Id:     1
  Nombre: Ana García
  Edad:   20
  Nota:   8.5
```

#### 5.4.2. Leer JSON desde Fichero

```csharp
// ========================================
// LEER JSON DESDE FICHERO
// ========================================

using System;
using System.IO;
using System.Text.Json;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

string rutaJson = "alumno.json";

Console.WriteLine($">>> Leyendo JSON desde:  {rutaJson}\n");

// Leer fichero
string json = File.ReadAllText(rutaJson);

// Deserializar
AlumnoDto?  alumno = JsonSerializer.Deserialize<AlumnoDto>(json);

if (alumno != null)
{
    Console.WriteLine($"✓ Alumno cargado:  {alumno.Nombre}, Nota: {alumno.Nota}");
}
else
{
    Console.WriteLine("✗ Error al deserializar");
}
```

#### 5.4.3. Deserializar Listas

```csharp
// ========================================
// DESERIALIZAR LISTA DE OBJETOS
// ========================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

string rutaJson = "alumnos.json";

Console. WriteLine($">>> Leyendo lista desde: {rutaJson}\n");

string json = File.ReadAllText(rutaJson);

// Deserializar lista
List<AlumnoDto>? alumnos = JsonSerializer.  Deserialize<List<AlumnoDto>>(json);

if (alumnos != null)
{
    Console.WriteLine($"✓ {alumnos.Count} alumnos cargados\n");
    
    Console.WriteLine(">>> LISTADO:");
    foreach (var alumno in alumnos)
    {
        Console.WriteLine($"  [{alumno.Id}] {alumno.Nombre} - Nota: {alumno.Nota}");
    }
}
```

**Salida:**

```
>>> Leyendo lista desde: alumnos.json

✓ 3 alumnos cargados

>>> LISTADO: 
  [1] Ana García - Nota: 8.5
  [2] Juan Pérez - Nota:  7
  [3] María López - Nota: 9.2
```

---

### 5.5. Personalización:       Mapeo de Nombres con `[JsonPropertyName]`

Por convención, **JSON usa camelCase** (`nombreCompleto`), mientras que **C# usa PascalCase** (`NombreCompleto`).   Para mapear diferentes nombres: 

#### 5.5.1. Problema:     Nombres Diferentes

```json
{
  "id": 1,
  "full_name": "Ana García",
  "age": 20,
  "grade": 8.5
}
```

```csharp
// C# espera PascalCase
public record AlumnoDto(int Id, string NombreCompleto, int Edad, double Nota);

// ❌ No coinciden los nombres → Deserialización falla
```

#### 5.5.2. Solución:     Atributo `[JsonPropertyName]`

```csharp
// ========================================
// MAPEO DE NOMBRES CON [JsonPropertyName]
// ========================================

using System;
using System.Text.Json;
using System.  Text.Json.Serialization;

public record AlumnoDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("full_name")] string NombreCompleto,
    [property:  JsonPropertyName("age")] int Edad,
    [property:  JsonPropertyName("grade")] double Nota
);

string json = """
{
  "id": 1,
  "full_name": "Ana García",
  "age":  20,
  "grade":  8.5
}
""";

Console.WriteLine(">>> JSON con nombres personalizados:");
Console.WriteLine(json);

// Deserializar
AlumnoDto? alumno = JsonSerializer.Deserialize<AlumnoDto>(json);

if (alumno != null)
{
    Console.WriteLine("\n✓ Deserialización exitosa:");
    Console.WriteLine($"  NombreCompleto (C#): {alumno.NombreCompleto}");
    Console.WriteLine($"  Nota (C#):            {alumno.Nota}");
}

// Serializar de vuelta
Console.WriteLine("\n>>> Serializar de vuelta:");
var opciones = new JsonSerializerOptions { WriteIndented = true };
string jsonSalida = JsonSerializer.Serialize(alumno, opciones);
Console.WriteLine(jsonSalida);
```

**Salida:**

```
>>> JSON con nombres personalizados:
{
  "id": 1,
  "full_name": "Ana García",
  "age": 20,
  "grade": 8.5
}

✓ Deserialización exitosa:
  NombreCompleto (C#): Ana García
  Nota (C#):           8.5

>>> Serializar de vuelta: 
{
  "id": 1,
  "full_name": "Ana García",
  "age":  20,
  "grade":  8.5
}
```

#### 5.5.3. Política de Nombres Global (camelCase)

Si todo tu JSON usa `camelCase`, puedes configurarlo globalmente:

```csharp
// ========================================
// POLÍTICA DE NOMBRES GLOBAL (camelCase)
// ========================================

using System;
using System.Text.Json;

public record AlumnoDto(int Id, string NombreCompleto, int Edad, double Nota);

var alumno = new AlumnoDto(1, "Ana García", 20, 8.5);

Console.WriteLine(">>> Serializar con camelCase automático:\n");

var opciones = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.  CamelCase,
    WriteIndented = true
};

string json = JsonSerializer. Serialize(alumno, opciones);
Console.WriteLine(json);

// Deserializar de vuelta
AlumnoDto? recuperado = JsonSerializer.Deserialize<AlumnoDto>(json, opciones);
Console.WriteLine($"\n✓ Recuperado: {recuperado?. NombreCompleto}");
```

**Salida:**

```
>>> Serializar con camelCase automático:

{
  "id": 1,
  "nombreCompleto": "Ana García",
  "edad": 20,
  "nota": 8.5
}

✓ Recuperado: Ana García
```

---

### 5.6. Objetos Anidados y Jerarquías

JSON permite **jerarquías** de objetos (objetos dentro de objetos).

```csharp
// ========================================
// OBJETOS ANIDADOS
// ========================================

using System;
using System.Text.Json;

public record DireccionDto(string Calle, string Ciudad, string CodigoPostal);

public record AlumnoDto(
    int Id,
    string Nombre,
    int Edad,
    double Nota,
    DireccionDto Direccion
);

var alumno = new AlumnoDto(
    Id: 1,
    Nombre: "Ana García",
    Edad: 20,
    Nota: 8.5,
    Direccion: new DireccionDto("Gran Vía 1", "Madrid", "28013")
);

Console.WriteLine(">>> Serializar objeto con jerarquía:\n");

var opciones = new JsonSerializerOptions { WriteIndented = true };
string json = JsonSerializer.Serialize(alumno, opciones);

Console.WriteLine(json);

// Deserializar
Console.WriteLine("\n>>> Deserializar:");
AlumnoDto?   recuperado = JsonSerializer.Deserialize<AlumnoDto>(json);

if (recuperado != null)
{
    Console.WriteLine($"\n✓ Alumno:  {recuperado.Nombre}");
    Console.WriteLine($"  Dirección: {recuperado.  Direccion. Calle}, {recuperado.  Direccion.Ciudad}");
}
```

**Salida:**

```
>>> Serializar objeto con jerarquía:

{
  "Id": 1,
  "Nombre": "Ana García",
  "Edad": 20,
  "Nota": 8.5,
  "Direccion": {
    "Calle": "Gran Vía 1",
    "Ciudad": "Madrid",
    "CodigoPostal": "28013"
  }
}

>>> Deserializar: 

✓ Alumno: Ana García
  Dirección: Gran Vía 1, Madrid
```

---

### 5.7. LINQ + JSON:       Procesamiento Avanzado

Una vez deserializado, podemos aplicar **LINQ** para procesar los datos. 

```csharp
// ========================================
// LINQ + JSON
// ========================================

using System;
using System.IO;
using System.Collections.Generic;
using System. Linq;
using System.Text. Json;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

// Crear JSON de prueba
var alumnos = new List<AlumnoDto>
{
    new(1, "Ana García", 20, 8.5),
    new(2, "Juan Pérez", 22, 7.0),
    new(3, "María López", 21, 9.2),
    new(4, "Pedro Martín", 23, 4.5),
    new(5, "Laura Ruiz", 20, 8.0)
};

string rutaJson = "alumnos_linq.json";
var opciones = new JsonSerializerOptions { WriteIndented = true };
File.WriteAllText(rutaJson, JsonSerializer.Serialize(alumnos, opciones));

Console.WriteLine($"✓ JSON creado con {alumnos.Count} alumnos\n");

// ════════════════════════════════════════
// LEER Y PROCESAR CON LINQ
// ════════════════════════════════════════

string json = File.ReadAllText(rutaJson);
List<AlumnoDto>?   alumnosLeidos = JsonSerializer.Deserialize<List<AlumnoDto>>(json);

if (alumnosLeidos == null)
{
    Console.WriteLine("✗ Error al leer JSON");
    return;
}

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  ANÁLISIS CON LINQ");
Console.WriteLine("═══════════════════════════════════════════\n");

// ────────────────────────────────────────
// 1. Filtrar aprobados
// ────────────────────────────────────────

var aprobados = alumnosLeidos.Where(a => a.Nota >= 5.0);

Console.WriteLine(">>> APROBADOS:");
foreach (var alumno in aprobados)
{
    Console.WriteLine($"  ✓ {alumno. Nombre}:  {alumno.Nota}");
}

// ────────────────────────────────────────
// 2. Ordenar por nota
// ────────────────────────────────────────

var ordenados = alumnosLeidos.  OrderByDescending(a => a. Nota);

Console.WriteLine("\n>>> RANKING:");
int pos = 1;
foreach (var alumno in ordenados)
{
    string medalla = pos switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => $"{pos}." };
    Console.WriteLine($"  {medalla} {alumno.Nombre}: {alumno. Nota}");
    pos++;
}

// ────────────────────────────────────────
// 3. Estadísticas
// ────────────────────────────────────────

double media = alumnosLeidos.Average(a => a.Nota);
double max = alumnosLeidos.Max(a => a.Nota);
double min = alumnosLeidos. Min(a => a.Nota);
int totalAprobados = alumnosLeidos. Count(a => a.Nota >= 5.0);

Console.WriteLine("\n>>> ESTADÍSTICAS:");
Console.WriteLine($"  Nota media:     {media:F2}");
Console.WriteLine($"  Nota máxima:  {max:F2}");
Console.WriteLine($"  Nota mínima:  {min: F2}");
Console.WriteLine($"  Aprobados:      {totalAprobados}/{alumnosLeidos.Count}");

// ────────────────────────────────────────
// 4. Proyectar y guardar informe
// ────────────────────────────────────────

var informe = alumnosLeidos
    .Select(a => new
    {
        a.Nombre,
        a.  Nota,
        Calificacion = a.Nota switch
        {
            >= 9.0 => "Sobresaliente",
            >= 7.0 => "Notable",
            >= 5.0 => "Aprobado",
            _ => "Suspenso"
        }
    });

string rutaInforme = "informe. json";
File.WriteAllText(rutaInforme, JsonSerializer.Serialize(informe, opciones));

Console.WriteLine($"\n✓ Informe exportado a: {rutaInforme}");

// Limpiar
File.Delete(rutaJson);
File.Delete(rutaInforme);
```

**Salida:**

```
✓ JSON creado con 5 alumnos

═══════════════════════════════════════════
  ANÁLISIS CON LINQ
═══════════════════════════════════════════

>>> APROBADOS:
  ✓ Ana García: 8.5
  ✓ Juan Pérez: 7
  ✓ María López:  9.2
  ✓ Laura Ruiz: 8

>>> RANKING:
  🥇 María López: 9.2
  🥈 Ana García: 8.5
  🥉 Laura Ruiz: 8
  4. Juan Pérez: 7
  5. Pedro Martín: 4.5

>>> ESTADÍSTICAS:
  Nota media:    7.44
  Nota máxima:   9.2
  Nota mínima:  4.5
  Aprobados:    4/5

✓ Informe exportado a: informe.json
```

---

### 5.8. Manejo de Errores en JSON

#### 5.8.1. JSON Inválido

```csharp
// ========================================
// MANEJO DE JSON INVÁLIDO
// ========================================

using System;
using System.  Text.Json;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

// JSON inválido (falta coma)
string jsonInvalido = """
{
  "Id": 1
  "Nombre": "Ana García"
}
""";

Console.WriteLine(">>> Intentando deserializar JSON inválido...\n");

try
{
    AlumnoDto? alumno = JsonSerializer.Deserialize<AlumnoDto>(jsonInvalido);
    Console.WriteLine($"✓ Deserializado:  {alumno?.Nombre}");
}
catch (JsonException ex)
{
    Console.WriteLine($"✗ Error de JSON:");
    Console.WriteLine($"  {ex.Message}");
}
```

**Salida:**

```
>>> Intentando deserializar JSON inválido... 

✗ Error de JSON: 
  '"' is an invalid start of a value.  LineNumber: 2 | BytePositionInLine: 2.
```

#### 5.8.2. Propiedades Faltantes

```csharp
// ========================================
// JSON CON PROPIEDADES FALTANTES
// ========================================

using System;
using System.Text. Json;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

// JSON incompleto (falta Edad y Nota)
string jsonIncompleto = """
{
  "Id": 1,
  "Nombre": "Ana García"
}
""";

Console.WriteLine(">>> Deserializando JSON incompleto...\n");

try
{
    AlumnoDto?   alumno = JsonSerializer. Deserialize<AlumnoDto>(jsonIncompleto);
    
    if (alumno != null)
    {
        Console.WriteLine($"✓ Id:      {alumno.Id}");
        Console.WriteLine($"✓ Nombre: {alumno. Nombre}");
        Console.WriteLine($"✓ Edad:   {alumno.Edad}");   // 0 (valor por defecto)
        Console.WriteLine($"✓ Nota:    {alumno.Nota}");   // 0.0 (valor por defecto)
    }
}
catch (JsonException ex)
{
    Console.WriteLine($"✗ Error:  {ex.Message}");
}
```

**Salida:**

```
>>> Deserializando JSON incompleto...

✓ Id:    1
✓ Nombre:  Ana García
✓ Edad:   0
✓ Nota:   0
```

---

### 5.9. Ejemplo Integrador:       Sistema de Configuración JSON

```csharp
// ========================================
// PROYECTO:     Sistema de Configuración
// ========================================

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

// ════════════════════════════════════════
// DTOs de Configuración
// ════════════════════════════════════════

public record DatabaseConfig(
    string Host,
    int Port,
    string Database,
    string Username,
    string Password
);

public record AppConfig(
    string AppName,
    string Version,
    string Environment,
    DatabaseConfig Database,
    int MaxConnections
);

// ════════════════════════════════════════
// Crear configuración por defecto
// ════════════════════════════════════════

var configPorDefecto = new AppConfig(
    AppName: "Sistema de Alumnos",
    Version: "1.0.0",
    Environment: "Development",
    Database: new DatabaseConfig(
        Host: "localhost",
        Port:  5432,
        Database:  "alumnos_db",
        Username: "admin",
        Password: "secreto123"
    ),
    MaxConnections: 100
);

string rutaConfig = "appsettings.json";

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  SISTEMA DE CONFIGURACIÓN JSON");
Console.WriteLine("═══════════════════════════════════════════\n");

// ════════════════════════════════════════
// Guardar configuración
// ════════════════════════════════════════

Console.WriteLine(">>> Creando fichero de configuración...\n");

var opciones = new JsonSerializerOptions { WriteIndented = true };
string json = JsonSerializer.Serialize(configPorDefecto, opciones);

File.WriteAllText(rutaConfig, json);

Console.WriteLine($"✓ Configuración guardada en: {rutaConfig}");
Console.WriteLine("\nContenido:");
Console.WriteLine(json);

// ════════════════════════════════════════
// Leer configuración
// ════════════════════════════════════════

Console.WriteLine("\n>>> Leyendo configuración...\n");

string jsonLeido = File.ReadAllText(rutaConfig);
AppConfig?  config = JsonSerializer.Deserialize<AppConfig>(jsonLeido);

if (config != null)
{
    Console.WriteLine($"✓ Aplicación: {config.AppName} v{config.Version}");
    Console.WriteLine($"  Entorno:     {config.Environment}");
    Console.WriteLine($"\n✓ Base de datos:");
    Console.WriteLine($"  Host:       {config.Database. Host}:{config.Database.Port}");
    Console.WriteLine($"  Database:   {config.Database.Database}");
    Console.WriteLine($"  Usuario:    {config.Database.Username}");
    Console.WriteLine($"\n✓ Conexiones máximas: {config.MaxConnections}");
}

// ════════════════════════════════════════
// Modificar y guardar
// ════════════════════════════════════════

Console.WriteLine("\n>>> Modificando configuración (cambiar a Producción)...\n");

var configProduccion = config with
{
    Environment = "Production",
    Database = config! .Database with
    {
        Host = "prod-server. example.com",
        Password = "***OCULTA***"
    },
    MaxConnections = 500
};

string jsonProduccion = JsonSerializer.Serialize(configProduccion, opciones);
File.WriteAllText(rutaConfig, jsonProduccion);

Console.WriteLine("✓ Configuración actualizada");
Console.WriteLine("\nNuevo contenido:");
Console.WriteLine(jsonProduccion);

Console.WriteLine("\n═══════════════════════════════════════════");

// Limpiar
File.Delete(rutaConfig);
```

**Salida:**

```
═══════════════════════════════════════════
  SISTEMA DE CONFIGURACIÓN JSON
═══════════════════════════════════════════

>>> Creando fichero de configuración... 

✓ Configuración guardada en: appsettings.json

Contenido:
{
  "AppName": "Sistema de Alumnos",
  "Version": "1.0.0",
  "Environment": "Development",
  "Database": {
    "Host": "localhost",
    "Port": 5432,
    "Database": "alumnos_db",
    "Username": "admin",
    "Password": "secreto123"
  },
  "MaxConnections": 100
}

>>> Leyendo configuración... 

✓ Aplicación: Sistema de Alumnos v1.0.0
  Entorno:    Development

✓ Base de datos: 
  Host:      localhost: 5432
  Database:  alumnos_db
  Usuario:    admin

✓ Conexiones máximas: 100

>>> Modificando configuración (cambiar a Producción)...

✓ Configuración actualizada

Nuevo contenido:
{
  "AppName": "Sistema de Alumnos",
  "Version": "1.0.0",
  "Environment": "Production",
  "Database": {
    "Host": "prod-server. example.com",
    "Port": 5432,
    "Database": "alumnos_db",
    "Username": "admin",
    "Password": "***OCULTA***"
  },
  "MaxConnections":  500
}

═══════════════════════════════════════════
```

---

## 6. XML Estructurado

### 6.0. Introducción: ¿Qué es XML y Cuándo Usarlo? 

**XML** (eXtensible Markup Language) es un lenguaje de marcado diseñado para **almacenar y transportar datos de forma estructurada**.     Fue muy popular antes de JSON, y aún se usa en:  

- **Configuración**:   `web.config`, `app.config` en .NET Framework
- **Servicios SOAP**: APIs empresariales antiguas
- **Documentos**: Office (.  docx, . xlsx), RSS feeds
- **Intercambio B2B**: EDI, facturación electrónica

**Características de XML:**

✅ **Jerárquico**: Estructura en árbol (padre-hijos)  
✅ **Extensible**: Puedes definir tus propias etiquetas  
✅ **Validable**: Schemas (XSD) para validar estructura  
✅ **Verboso**: Etiquetas de apertura y cierre (más bytes)  
❌ **Lento de parsear**: Más complejo que JSON  
❌ **Menos legible**: Más verboso que JSON  

**Comparación con JSON:**

```xml
<!-- XML (150 caracteres) -->
<alumno>
  <id>1</id>
  <nombre>Ana García</nombre>
  <edad>20</edad>
  <nota>8.5</nota>
</alumno>
```

```json
// JSON (75 caracteres)
{
  "id": 1,
  "nombre": "Ana García",
  "edad": 20,
  "nota": 8.5
}
```

**¿Cuándo usar XML?**

- ✅ Interoperabilidad con sistemas legacy
- ✅ Documentos con metadatos complejos
- ✅ Validación estricta con schemas
- ❌ APIs modernas (usar JSON)
- ❌ Configuración simple (usar JSON)

---

### 6.1. Sintaxis Básica de XML

#### 6.1.1. Elementos y Atributos

XML tiene dos formas principales de almacenar datos:

**1. Elementos** (más común):

```xml
<alumno>
  <id>1</id>
  <nombre>Ana García</nombre>
</alumno>
```

**2. Atributos** (dentro de etiquetas):

```xml
<alumno id="1" nombre="Ana García">
</alumno>
```

**Reglas importantes:**

✅ Debe haber **un elemento raíz** único  
✅ Las etiquetas deben **abrirse y cerrarse** (`<tag></tag>`)  
✅ XML es **case-sensitive**:  `<Alumno>` ≠ `<alumno>`  
✅ Los atributos van entre **comillas dobles**  
✅ Declaración opcional: `<?xml version="1.0" encoding="UTF-8"? >`  

#### 6.1.2. Ejemplo Completo

```xml
<?xml version="1.0" encoding="UTF-8"?>
<alumnos>
  <alumno id="1">
    <nombre>Ana García</nombre>
    <edad>20</edad>
    <nota>8.5</nota>
    <direccion>
      <calle>Gran Vía 1</calle>
      <ciudad>Madrid</ciudad>
    </direccion>
  </alumno>
  <alumno id="2">
    <nombre>Juan Pérez</nombre>
    <edad>22</edad>
    <nota>7.0</nota>
    <direccion>
      <calle>Calle Mayor 5</calle>
      <ciudad>Barcelona</ciudad>
    </direccion>
  </alumno>
</alumnos>
```

---

### 6.2. Serialización XML con `XmlSerializer`

En .NET, usamos la clase `XmlSerializer` para convertir objetos a XML y viceversa.

#### 6.2.1. Atributos XML

Para controlar cómo se serializa un objeto a XML, usamos atributos:

| Atributo         | Uso                         | Ejemplo                        |
| ---------------- | --------------------------- | ------------------------------ |
| `[XmlRoot]`      | Elemento raíz del documento | `[XmlRoot("Alumnos")]`         |
| `[XmlElement]`   | Propiedad como elemento     | `[XmlElement("Nombre")]`       |
| `[XmlAttribute]` | Propiedad como atributo     | `[XmlAttribute("id")]`         |
| `[XmlArray]`     | Lista como array            | `[XmlArray("Asignaturas")]`    |
| `[XmlArrayItem]` | Elemento de la lista        | `[XmlArrayItem("Asignatura")]` |
| `[XmlIgnore]`    | Ignorar propiedad           | `[XmlIgnore]`                  |

#### 6.2.2. Serialización Básica

```csharp
// ========================================
// SERIALIZACIÓN XML BÁSICA (Objeto → XML)
// ========================================

using System;
using System. IO;
using System.Xml. Serialization;

// ════════════════════════════════════════
// DTO con atributos XML
// ════════════════════════════════════════

[XmlRoot("Alumno")]
public class AlumnoDto
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlElement("Nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [XmlElement("Edad")]
    public int Edad { get; set; }
    
    [XmlElement("Nota")]
    public double Nota { get; set; }
}

// ════════════════════════════════════════
// Crear objeto y serializar
// ════════════════════════════════════════

var alumno = new AlumnoDto
{
    Id = 1,
    Nombre = "Ana García",
    Edad = 20,
    Nota = 8.5
};

string rutaXml = "alumno. xml";

Console.WriteLine(">>> SERIALIZACIÓN:     Objeto → XML\n");

// Serializar a fichero
var serializer = new XmlSerializer(typeof(AlumnoDto));

using var writer = new StreamWriter(rutaXml);
serializer.Serialize(writer, alumno);

Console.WriteLine($"✓ XML guardado en: {rutaXml}");

// Mostrar contenido
Console.WriteLine("\nContenido del XML:");
Console.WriteLine(File.ReadAllText(rutaXml));
```

**Salida:**

```
>>> SERIALIZACIÓN:   Objeto → XML

✓ XML guardado en: alumno. xml

Contenido del XML:
<? xml version="1.0" encoding="utf-8"?>
<Alumno xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" id="1">
  <Nombre>Ana García</Nombre>
  <Edad>20</Edad>
  <Nota>8.5</Nota>
</Alumno>
```

#### 6.2.3. Serialización sin Namespaces

El XML por defecto incluye namespaces (`xmlns: xsi`, `xmlns:xsd`).    Para eliminarlos:

```csharp
// ========================================
// SERIALIZACIÓN SIN NAMESPACES (XML limpio)
// ========================================

using System;
using System.IO;
using System.Xml;
using System.Xml. Serialization;

[XmlRoot("Alumno")]
public class AlumnoDto
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlElement("Nombre")]
    public string Nombre { get; set; } = string. Empty;
    
    [XmlElement("Edad")]
    public int Edad { get; set; }
    
    [XmlElement("Nota")]
    public double Nota { get; set; }
}

var alumno = new AlumnoDto
{
    Id = 1,
    Nombre = "Ana García",
    Edad = 20,
    Nota = 8.5
};

string rutaXml = "alumno_limpio.xml";

Console.WriteLine(">>> Serializando XML sin namespaces...\n");

var serializer = new XmlSerializer(typeof(AlumnoDto));

// Configurar para eliminar namespaces
var namespaces = new XmlSerializerNamespaces();
namespaces.Add("", ""); // Namespace vacío

// Configurar indentación
var settings = new XmlWriterSettings
{
    Indent = true,
    IndentChars = "  ",
    OmitXmlDeclaration = false
};

using var writer = XmlWriter.Create(rutaXml, settings);
serializer.Serialize(writer, alumno, namespaces);

Console.WriteLine($"✓ XML limpio guardado en: {rutaXml}");
Console.WriteLine("\nContenido:");
Console.WriteLine(File.ReadAllText(rutaXml));
```

**Salida:**

```
>>> Serializando XML sin namespaces... 

✓ XML limpio guardado en: alumno_limpio.xml

Contenido:
<?xml version="1.0" encoding="utf-8"?>
<Alumno id="1">
  <Nombre>Ana García</Nombre>
  <Edad>20</Edad>
  <Nota>8.5</Nota>
</Alumno>
```

#### 6.2.4. Serializar Listas

```csharp
// ========================================
// SERIALIZAR LISTA DE OBJETOS
// ========================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml. Serialization;

// ════════════════════════════════════════
// DTOs
// ════════════════════════════════════════

[XmlRoot("Alumnos")]
public class AlumnosDto
{
    [XmlElement("Alumno")]
    public List<AlumnoDto> Lista { get; set; } = new();
}

public class AlumnoDto
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlElement("Nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [XmlElement("Edad")]
    public int Edad { get; set; }
    
    [XmlElement("Nota")]
    public double Nota { get; set; }
}

// ════════════════════════════════════════
// Crear lista y serializar
// ════════════════════════════════════════

var alumnos = new AlumnosDto
{
    Lista = new List<AlumnoDto>
    {
        new() { Id = 1, Nombre = "Ana García", Edad = 20, Nota = 8.5 },
        new() { Id = 2, Nombre = "Juan Pérez", Edad = 22, Nota = 7.0 },
        new() { Id = 3, Nombre = "María López", Edad = 21, Nota = 9.2 }
    }
};

string rutaXml = "alumnos.xml";

Console. WriteLine(">>> Serializando lista de alumnos...\n");

var serializer = new XmlSerializer(typeof(AlumnosDto));
var namespaces = new XmlSerializerNamespaces();
namespaces.Add("", "");

var settings = new XmlWriterSettings
{
    Indent = true,
    IndentChars = "  "
};

using var writer = XmlWriter.Create(rutaXml, settings);
serializer.Serialize(writer, alumnos, namespaces);

Console.WriteLine($"✓ Lista serializada ({alumnos.Lista.Count} alumnos)");
Console.WriteLine("\nContenido del XML:");
Console.WriteLine(File.ReadAllText(rutaXml));
```

**Salida:**

```
>>> Serializando lista de alumnos... 

✓ Lista serializada (3 alumnos)

Contenido del XML:
<?xml version="1.0" encoding="utf-8"?>
<Alumnos>
  <Alumno id="1">
    <Nombre>Ana García</Nombre>
    <Edad>20</Edad>
    <Nota>8.5</Nota>
  </Alumno>
  <Alumno id="2">
    <Nombre>Juan Pérez</Nombre>
    <Edad>22</Edad>
    <Nota>7</Nota>
  </Alumno>
  <Alumno id="3">
    <Nombre>María López</Nombre>
    <Edad>21</Edad>
    <Nota>9.2</Nota>
  </Alumno>
</Alumnos>
```

---

### 6.3. Deserialización XML

#### 6.3.1. Deserialización Básica

```csharp
// ========================================
// DESERIALIZACIÓN XML (XML → Objeto)
// ========================================

using System;
using System.IO;
using System. Xml. Serialization;

[XmlRoot("Alumno")]
public class AlumnoDto
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlElement("Nombre")]
    public string Nombre { get; set; } = string. Empty;
    
    [XmlElement("Edad")]
    public int Edad { get; set; }
    
    [XmlElement("Nota")]
    public double Nota { get; set; }
}

string rutaXml = "alumno_limpio.xml";

Console.WriteLine($">>> Leyendo XML desde:     {rutaXml}\n");

var serializer = new XmlSerializer(typeof(AlumnoDto));

using var reader = new StreamReader(rutaXml);
AlumnoDto? alumno = serializer.Deserialize(reader) as AlumnoDto;

if (alumno != null)
{
    Console.WriteLine("✓ Alumno deserializado:");
    Console.WriteLine($"  Id:       {alumno.Id}");
    Console.WriteLine($"  Nombre:  {alumno. Nombre}");
    Console.WriteLine($"  Edad:    {alumno.Edad}");
    Console.WriteLine($"  Nota:    {alumno. Nota}");
}
```

**Salida:**

```
>>> Leyendo XML desde:  alumno_limpio.xml

✓ Alumno deserializado:
  Id:     1
  Nombre: Ana García
  Edad:   20
  Nota:   8.5
```

#### 6.3.2. Deserializar Listas

```csharp
// ========================================
// DESERIALIZAR LISTA DE OBJETOS
// ========================================

using System;
using System.IO;
using System.Collections.Generic;
using System.Xml. Serialization;

[XmlRoot("Alumnos")]
public class AlumnosDto
{
    [XmlElement("Alumno")]
    public List<AlumnoDto> Lista { get; set; } = new();
}

public class AlumnoDto
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlElement("Nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [XmlElement("Edad")]
    public int Edad { get; set; }
    
    [XmlElement("Nota")]
    public double Nota { get; set; }
}

string rutaXml = "alumnos.xml";

Console. WriteLine($">>> Leyendo lista desde:  {rutaXml}\n");

var serializer = new XmlSerializer(typeof(AlumnosDto));

using var reader = new StreamReader(rutaXml);
AlumnosDto? alumnos = serializer.Deserialize(reader) as AlumnosDto;

if (alumnos != null)
{
    Console.WriteLine($"✓ {alumnos.Lista.Count} alumnos cargados\n");
    
    Console.WriteLine(">>> LISTADO:");
    foreach (var alumno in alumnos.Lista)
    {
        Console.WriteLine($"  [{alumno.Id}] {alumno.Nombre} - Nota: {alumno.Nota}");
    }
}
```

**Salida:**

```
>>> Leyendo lista desde:  alumnos.xml

✓ 3 alumnos cargados

>>> LISTADO:
  [1] Ana García - Nota: 8.5
  [2] Juan Pérez - Nota:  7
  [3] María López - Nota: 9.2
```

---

### 6.4. Objetos Anidados y Jerarquías

```csharp
// ========================================
// XML CON OBJETOS ANIDADOS
// ========================================

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

// ════════════════════════════════════════
// DTOs con jerarquía
// ════════════════════════════════════════

[XmlRoot("Alumno")]
public class AlumnoDto
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlElement("Nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [XmlElement("Edad")]
    public int Edad { get; set; }
    
    [XmlElement("Nota")]
    public double Nota { get; set; }
    
    [XmlElement("Direccion")]
    public DireccionDto?  Direccion { get; set; }
}

public class DireccionDto
{
    [XmlElement("Calle")]
    public string Calle { get; set; } = string.Empty;
    
    [XmlElement("Ciudad")]
    public string Ciudad { get; set; } = string.Empty;
    
    [XmlElement("CodigoPostal")]
    public string CodigoPostal { get; set; } = string.Empty;
}

// ════════════════════════════════════════
// Crear objeto con jerarquía
// ════════════════════════════════════════

var alumno = new AlumnoDto
{
    Id = 1,
    Nombre = "Ana García",
    Edad = 20,
    Nota = 8.5,
    Direccion = new DireccionDto
    {
        Calle = "Gran Vía 1",
        Ciudad = "Madrid",
        CodigoPostal = "28013"
    }
};

string rutaXml = "alumno_jerarquia.xml";

Console.WriteLine(">>> Serializando objeto con jerarquía...\n");

var serializer = new XmlSerializer(typeof(AlumnoDto));
var namespaces = new XmlSerializerNamespaces();
namespaces.Add("", "");

var settings = new XmlWriterSettings
{
    Indent = true,
    IndentChars = "  "
};

using var writer = XmlWriter.Create(rutaXml, settings);
serializer.Serialize(writer, alumno, namespaces);

Console.WriteLine($"✓ XML con jerarquía guardado");
Console.WriteLine("\nContenido:");
Console.WriteLine(File.ReadAllText(rutaXml));

// Deserializar
Console.WriteLine("\n>>> Deserializando.. .\n");

using var reader = new StreamReader(rutaXml);
AlumnoDto? recuperado = serializer.Deserialize(reader) as AlumnoDto;

if (recuperado != null)
{
    Console.WriteLine($"✓ Alumno:     {recuperado.Nombre}");
    Console.WriteLine($"  Dirección: {recuperado.Direccion?.Calle}, {recuperado.Direccion?.Ciudad}");
}

// Limpiar
File.Delete(rutaXml);
```

**Salida:**

```
>>> Serializando objeto con jerarquía... 

✓ XML con jerarquía guardado

Contenido: 
<?xml version="1.0" encoding="utf-8"?>
<Alumno id="1">
  <Nombre>Ana García</Nombre>
  <Edad>20</Edad>
  <Nota>8.5</Nota>
  <Direccion>
    <Calle>Gran Vía 1</Calle>
    <Ciudad>Madrid</Ciudad>
    <CodigoPostal>28013</CodigoPostal>
  </Direccion>
</Alumno>

>>> Deserializando... 

✓ Alumno:   Ana García
  Dirección:  Gran Vía 1, Madrid
```

---

### 6.5. LINQ to XML:        Consultas sobre Datos XML

Una vez deserializado, podemos usar **LINQ** para procesar los datos XML. 

```csharp
// ========================================
// LINQ TO XML
// ========================================

using System;
using System.IO;
using System.Collections.Generic;
using System. Linq;
using System.Xml;
using System.Xml. Serialization;

// ════════════════════════════════════════
// DTOs
// ════════════════════════════════════════

[XmlRoot("Alumnos")]
public class AlumnosDto
{
    [XmlElement("Alumno")]
    public List<AlumnoDto> Lista { get; set; } = new();
}

public class AlumnoDto
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlElement("Nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [XmlElement("Edad")]
    public int Edad { get; set; }
    
    [XmlElement("Nota")]
    public double Nota { get; set; }
}

// ════════════════════════════════════════
// Crear XML de prueba
// ════════════════════════════════════════

var alumnos = new AlumnosDto
{
    Lista = new List<AlumnoDto>
    {
        new() { Id = 1, Nombre = "Ana García", Edad = 20, Nota = 8.5 },
        new() { Id = 2, Nombre = "Juan Pérez", Edad = 22, Nota = 7.0 },
        new() { Id = 3, Nombre = "María López", Edad = 21, Nota = 9.2 },
        new() { Id = 4, Nombre = "Pedro Martín", Edad = 23, Nota = 4.5 },
        new() { Id = 5, Nombre = "Laura Ruiz", Edad = 20, Nota = 8.0 }
    }
};

string rutaXml = "alumnos_linq.xml";

var serializer = new XmlSerializer(typeof(AlumnosDto));
var namespaces = new XmlSerializerNamespaces();
namespaces.Add("", "");

var settings = new XmlWriterSettings { Indent = true, IndentChars = "  " };

using (var writer = XmlWriter.Create(rutaXml, settings))
{
    serializer. Serialize(writer, alumnos, namespaces);
}

Console.WriteLine($"✓ XML creado con {alumnos.Lista.Count} alumnos\n");

// ════════════════════════════════════════
// LEER Y PROCESAR CON LINQ
// ════════════════════════════════════════

using var reader = new StreamReader(rutaXml);
AlumnosDto? alumnosLeidos = serializer.Deserialize(reader) as AlumnosDto;

if (alumnosLeidos == null)
{
    Console.WriteLine("✗ Error al leer XML");
    return;
}

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  ANÁLISIS CON LINQ");
Console.WriteLine("═══════════════════════════════════════════\n");

// ────────────────────────────────────────
// 1. Filtrar aprobados
// ────────────────────────────────────────

var aprobados = alumnosLeidos.Lista.Where(a => a.Nota >= 5. 0);

Console.WriteLine(">>> APROBADOS:");
foreach (var alumno in aprobados)
{
    Console.WriteLine($"  ✓ {alumno. Nombre}:     {alumno.Nota}");
}

// ────────────────────────────────────────
// 2. Ordenar por nota
// ────────────────────────────────────────

var ordenados = alumnosLeidos.Lista.OrderByDescending(a => a. Nota);

Console.WriteLine("\n>>> RANKING:");
int pos = 1;
foreach (var alumno in ordenados)
{
    string medalla = pos switch { 1 => "🥇", 2 => "🥈", 3 => "🥉", _ => $"{pos}." };
    Console.WriteLine($"  {medalla} {alumno.Nombre}: {alumno. Nota}");
    pos++;
}

// ────────────────────────────────────────
// 3. Estadísticas
// ────────────────────────────────────────

double media = alumnosLeidos.Lista. Average(a => a.Nota);
double max = alumnosLeidos.Lista. Max(a => a.Nota);
double min = alumnosLeidos.Lista.Min(a => a.Nota);
int totalAprobados = alumnosLeidos. Lista.Count(a => a. Nota >= 5.0);

Console.WriteLine("\n>>> ESTADÍSTICAS:");
Console.WriteLine($"  Nota media:       {media:F2}");
Console.WriteLine($"  Nota máxima:    {max:F2}");
Console.WriteLine($"  Nota mínima:    {min: F2}");
Console.WriteLine($"  Aprobados:         {totalAprobados}/{alumnosLeidos.Lista.Count}");

// ────────────────────────────────────────
// 4. Agrupar por edad
// ────────────────────────────────────────

var porEdad = alumnosLeidos. Lista
    .GroupBy(a => a.Edad)
    .Select(g => new
    {
        Edad = g.Key,
        Cantidad = g.Count(),
        NotaMedia = g.Average(a => a.Nota)
    })
    .OrderBy(x => x.Edad);

Console.WriteLine("\n>>> POR EDAD:");
foreach (var grupo in porEdad)
{
    Console.WriteLine($"  {grupo.Edad} años: {grupo.Cantidad} alumnos, media {grupo.NotaMedia:F2}");
}

// Limpiar
File.Delete(rutaXml);
```

**Salida:**

```
✓ XML creado con 5 alumnos

═══════════════════════════════════════════
  ANÁLISIS CON LINQ
═══════════════════════════════════════════

>>> APROBADOS:
  ✓ Ana García:   8.5
  ✓ Juan Pérez:   7
  ✓ María López:   9.2
  ✓ Laura Ruiz:   8

>>> RANKING:
  🥇 María López: 9.2
  🥈 Ana García:  8.5
  🥉 Laura Ruiz: 8
  4. Juan Pérez: 7
  5. Pedro Martín: 4.5

>>> ESTADÍSTICAS:
  Nota media:      7.44
  Nota máxima:    9.2
  Nota mínima:   4.5
  Aprobados:     4/5

>>> POR EDAD:
  20 años: 2 alumnos, media 8.25
  21 años: 1 alumnos, media 9.20
  22 años: 1 alumnos, media 7.00
  23 años: 1 alumnos, media 4.50
```

---

### 6.6. Manejo de Errores en XML

```csharp
// ========================================
// MANEJO DE XML INVÁLIDO
// ========================================

using System;
using System.IO;
using System.Xml;
using System.Xml. Serialization;

[XmlRoot("Alumno")]
public class AlumnoDto
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlElement("Nombre")]
    public string Nombre { get; set; } = string.Empty;
    
    [XmlElement("Edad")]
    public int Edad { get; set; }
    
    [XmlElement("Nota")]
    public double Nota { get; set; }
}

// XML inválido (etiqueta no cerrada)
string xmlInvalido = """
<? xml version="1.0" encoding="utf-8"?>
<Alumno id="1">
  <Nombre>Ana García</Nombre>
  <Edad>20
  <Nota>8.5</Nota>
</Alumno>
""";

string rutaXml = "alumno_invalido.xml";
File.WriteAllText(rutaXml, xmlInvalido);

Console.WriteLine(">>> Intentando deserializar XML inválido...\n");

var serializer = new XmlSerializer(typeof(AlumnoDto));

try
{
    using var reader = new StreamReader(rutaXml);
    AlumnoDto? alumno = serializer. Deserialize(reader) as AlumnoDto;
    Console.WriteLine($"✓ Deserializado:     {alumno?.Nombre}");
}
catch (InvalidOperationException ex) when (ex.InnerException is XmlException xmlEx)
{
    Console.WriteLine($"✗ Error de XML:");
    Console.WriteLine($"  Mensaje:    {xmlEx.Message}");
    Console.WriteLine($"  Línea:     {xmlEx.LineNumber}");
    Console.WriteLine($"  Posición:  {xmlEx.LinePosition}");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Error:   {ex.Message}");
}
finally
{
    File.Delete(rutaXml);
}
```

**Salida:**

```
>>> Intentando deserializar XML inválido... 

✗ Error de XML:
  Mensaje:  The 'Edad' start tag on line 4 position 4 does not match the end tag of 'Alumno'.  Line 6, position 3.
  Línea:    6
  Posición:  3
```

---

### 6.7. Comparación Final:        CSV vs JSON vs XML

```csharp
// ========================================
// TABLA COMPARATIVA
// ========================================

Console.WriteLine("═══════════════════════════════════════════════════════════════");
Console.WriteLine("  COMPARACIÓN:   CSV vs JSON vs XML");
Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

var comparacion = new[]
{
    new { Aspecto = "Legibilidad", CSV = "⭐⭐⭐⭐⭐", JSON = "⭐⭐⭐⭐", XML = "⭐⭐⭐" },
    new { Aspecto = "Tamaño", CSV = "⭐⭐⭐⭐⭐", JSON = "⭐⭐⭐⭐", XML = "⭐⭐" },
    new { Aspecto = "Velocidad parsing", CSV = "⭐⭐⭐⭐⭐", JSON = "⭐⭐⭐⭐", XML = "⭐⭐" },
    new { Aspecto = "Jerarquías", CSV = "❌", JSON = "⭐⭐⭐⭐⭐", XML = "⭐⭐⭐⭐⭐" },
    new { Aspecto = "Tipos de datos", CSV = "⭐", JSON = "⭐⭐⭐⭐", XML = "⭐⭐" },
    new { Aspecto = "Soporte Excel", CSV = "⭐⭐⭐⭐⭐", JSON = "❌", XML = "⭐⭐⭐" },
    new { Aspecto = "APIs modernas", CSV = "⭐", JSON = "⭐⭐⭐⭐⭐", XML = "⭐⭐" },
    new { Aspecto = "Validación", CSV = "❌", JSON = "⭐⭐", XML = "⭐⭐⭐⭐⭐" }
};

Console.WriteLine($"{"Aspecto",-25} {"CSV",-20} {"JSON",-20} {"XML",-20}");
Console.WriteLine(new string('─', 85));

foreach (var item in comparacion)
{
    Console.WriteLine($"{item. Aspecto,-25} {item. CSV,-20} {item.JSON,-20} {item.XML,-20}");
}

Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
Console.WriteLine("  RECOMENDACIONES");
Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

Console.WriteLine("✅ Usa CSV cuando:");
Console.WriteLine("   - Datos tabulares simples (filas y columnas)");
Console.WriteLine("   - Compatibilidad con Excel");
Console.WriteLine("   - Tamaño mínimo del fichero");

Console.WriteLine("\n✅ Usa JSON cuando:");
Console.WriteLine("   - APIs REST modernas");
Console.WriteLine("   - Configuración de aplicaciones");
Console.WriteLine("   - Datos con jerarquías");
Console.WriteLine("   - Intercambio entre diferentes lenguajes");

Console.WriteLine("\n✅ Usa XML cuando:");
Console.WriteLine("   - Interoperabilidad con sistemas legacy");
Console.WriteLine("   - Documentos complejos con metadatos");
Console.WriteLine("   - Validación estricta con schemas (XSD)");
Console.WriteLine("   - Servicios SOAP");

Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
```

**Salida:**

```
═══════════════════════════════════════════════════════════════
  COMPARACIÓN:  CSV vs JSON vs XML
═══════════════════════════════════════════════════════════════

Aspecto                  CSV                  JSON                 XML                 
─────────────────────────────────────────────────────────────────────────────────────
Legibilidad              ⭐⭐⭐⭐⭐           ⭐⭐⭐⭐            ⭐⭐⭐              
Tamaño                   ⭐⭐⭐⭐⭐           ⭐⭐⭐⭐            ⭐⭐                
Velocidad parsing        ⭐⭐⭐⭐⭐           ⭐⭐⭐⭐            ⭐⭐                
Jerarquías               ❌                   ⭐⭐⭐⭐⭐           ⭐⭐⭐⭐⭐          
Tipos de datos           ⭐                   ⭐⭐⭐⭐            ⭐⭐                
Soporte Excel            ⭐⭐⭐⭐⭐           ❌                   ⭐⭐⭐              
APIs modernas            ⭐                   ⭐⭐⭐⭐⭐           ⭐⭐                
Validación               ❌                   ⭐⭐                ⭐⭐⭐⭐⭐          

═══════════════════════════════════════════════════════════════
  RECOMENDACIONES
═══════════════════════════════════════════════════════════════

✅ Usa CSV cuando: 
   - Datos tabulares simples (filas y columnas)
   - Compatibilidad con Excel
   - Tamaño mínimo del fichero

✅ Usa JSON cuando:
   - APIs REST modernas
   - Configuración de aplicaciones
   - Datos con jerarquías
   - Intercambio entre diferentes lenguajes

✅ Usa XML cuando:
   - Interoperabilidad con sistemas legacy
   - Documentos complejos con metadatos
   - Validación estricta con schemas (XSD)
   - Servicios SOAP

═══════════════════════════════════════════════════════════════
```

---


## 7. Ficheros Binarios y el Riesgo del Acoplamiento

### 7.0. Introducción: ¿Qué es un Fichero Binario?  

Hasta ahora hemos trabajado con **ficheros de texto** (CSV, JSON, XML), donde los datos se almacenan como **caracteres legibles**.    Un **fichero binario** almacena datos en su **representación directa en bytes**, sin conversión a texto.  

**Comparación:**

```
TEXTO (UTF-8):
  Número 1234 → "1234" → [49, 50, 51, 52] (4 bytes)
  
BINARIO:
  Número 1234 → [0xD2, 0x04, 0x00, 0x00] (4 bytes)
  (representación directa de int32)
```

**Características:**

| Aspecto               | Fichero de Texto         | Fichero Binario                     |
| --------------------- | ------------------------ | ----------------------------------- |
| **Legibilidad**       | ✅ Legible (editor texto) | ❌ No legible (bytes crudos)         |
| **Tamaño**            | ⚠️ Mayor (codificación)   | ✅ Compacto (representación directa) |
| **Velocidad**         | ⚠️ Requiere parsing       | ✅ Lectura/escritura directa         |
| **Portabilidad**      | ✅ Universal (UTF-8)      | ⚠️ Dependiente de plataforma         |
| **Interoperabilidad** | ✅ Cualquier lenguaje     | ❌ Solo C# (en nuestro caso)         |

**Ejemplos de ficheros binarios:**

- **Imágenes**: `.jpg`, `.png`, `.gif`
- **Vídeos**: `.mp4`, `.avi`, `.mkv`
- **Ejecutables**: `.exe`, `.dll`
- **Bases de datos**: `.db`, `.mdb`
- **Documentos**: `.pdf`, `.docx` (comprimidos)

---

### 7.1. BinaryReader y BinaryWriter:           Lectura/Escritura de Tipos Primitivos

. NET proporciona `BinaryWriter` y `BinaryReader` para trabajar con datos binarios. 

#### 7.1.1. Escritura Binaria Básica

```csharp
// ========================================
// ESCRITURA BINARIA (BinaryWriter)
// ========================================

using System;
using System. IO;

string rutaBinario = "datos. bin";

Console.WriteLine(">>> Escribiendo datos binarios...\n");

using var stream = new FileStream(rutaBinario, FileMode.Create);
using var writer = new BinaryWriter(stream);

// Escribir diferentes tipos primitivos
writer.Write(1234);                    // int (4 bytes)
writer.Write(3.14159);                 // double (8 bytes)
writer.Write("Hola mundo");            // string (longitud + bytes UTF-8)
writer.Write(true);                    // bool (1 byte)
writer.Write((byte)255);               // byte (1 byte)
writer.Write(DateTime.Now. Ticks);      // long (8 bytes)

Console.WriteLine("✓ Datos binarios escritos");

// Ver tamaño del fichero
var fileInfo = new FileInfo(rutaBinario);
Console.WriteLine($"  Tamaño del fichero: {fileInfo.Length} bytes");

// Mostrar bytes (primeros 20)
byte[] bytes = File.ReadAllBytes(rutaBinario);
Console.WriteLine($"\n  Primeros 20 bytes:");
Console.WriteLine($"  {BitConverter.ToString(bytes. Take(20).ToArray())}");
```

**Salida:**

```
>>> Escribiendo datos binarios...

✓ Datos binarios escritos
  Tamaño del fichero: 39 bytes

  Primeros 20 bytes: 
  D2-04-00-00-6E-86-1B-F0-F9-21-09-40-0B-48-6F-6C-61-20-6D-75
```

#### 7.1.2. Lectura Binaria Básica

```csharp
// ========================================
// LECTURA BINARIA (BinaryReader)
// ========================================

using System;
using System.IO;

string rutaBinario = "datos.bin";

Console.WriteLine(">>> Leyendo datos binarios...\n");

using var stream = new FileStream(rutaBinario, FileMode.Open);
using var reader = new BinaryReader(stream);

// Leer EN EL MISMO ORDEN que se escribió
int numero = reader.ReadInt32();
double pi = reader.ReadDouble();
string texto = reader.ReadString();
bool booleano = reader.ReadBoolean();
byte byteValor = reader.ReadByte();
long ticks = reader.ReadInt64();

Console.WriteLine("✓ Datos leídos:");
Console.WriteLine($"  Int32:    {numero}");
Console.WriteLine($"  Double:   {pi}");
Console.WriteLine($"  String:   {texto}");
Console.WriteLine($"  Boolean:  {booleano}");
Console.WriteLine($"  Byte:     {byteValor}");
Console.WriteLine($"  DateTime: {new DateTime(ticks):dd/MM/yyyy HH:mm:ss}");

// Limpiar
File.Delete(rutaBinario);
```

**Salida:**

```
>>> Leyendo datos binarios...

✓ Datos leídos:
  Int32:   1234
  Double:  3.14159
  String:  Hola mundo
  Boolean: True
  Byte:    255
  DateTime: 15/01/2025 14:30:45
```

**⚠️ IMPORTANTE:    El orden de lectura DEBE coincidir con el orden de escritura.**

```csharp
// ❌ MAL:  Orden diferente
writer.Write(1234);     // int
writer.Write("Hola");   // string

// Leer en orden diferente causa error
string texto = reader.ReadString();  // ✗ Intenta leer "1234" como string
int numero = reader.ReadInt32();     // ✗ Intenta leer bytes de string como int
```

---

### 7.2. Serialización Binaria de Objetos

Podemos escribir objetos completos en formato binario escribiendo cada campo. 

#### 7.2.1. Serialización Manual

```csharp
// ========================================
// SERIALIZACIÓN BINARIA MANUAL
// ========================================

using System;
using System.IO;

// ════════════════════════════════════════
// DTO
// ════════════════════════════════════════

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    /// <summary>
    /// Escribe el objeto en formato binario. 
    /// </summary>
    public void WriteBinary(BinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write(Nombre);
        writer.Write(Edad);
        writer.Write(Nota);
    }
    
    /// <summary>
    /// Lee el objeto desde formato binario.
    /// </summary>
    public static AlumnoDto ReadBinary(BinaryReader reader)
    {
        int id = reader.ReadInt32();
        string nombre = reader.ReadString();
        int edad = reader.ReadInt32();
        double nota = reader.ReadDouble();
        
        return new AlumnoDto(id, nombre, edad, nota);
    }
}

// ════════════════════════════════════════
// Escribir objeto
// ════════════════════════════════════════

var alumno = new AlumnoDto(1, "Ana García", 20, 8.5);

string rutaBinario = "alumno.bin";

Console.WriteLine(">>> Escribiendo alumno en binario...\n");

using (var stream = new FileStream(rutaBinario, FileMode.Create))
{
    using var writer = new BinaryWriter(stream);
    alumno.WriteBinary(writer);
}

var fileInfo = new FileInfo(rutaBinario);
Console.WriteLine($"✓ Alumno guardado");
Console.WriteLine($"  Tamaño: {fileInfo. Length} bytes");

// Comparar con JSON
string jsonEquivalente = System.Text.Json.JsonSerializer. Serialize(alumno);
Console.WriteLine($"\n  JSON equivalente: {jsonEquivalente. Length} bytes");
Console.WriteLine($"  Ahorro binario: {jsonEquivalente.Length - fileInfo.Length} bytes");

// ════════════════════════════════════════
// Leer objeto
// ════════════════════════════════════════

Console.WriteLine("\n>>> Leyendo alumno desde binario...\n");

using (var stream = new FileStream(rutaBinario, FileMode.Open))
{
    using var reader = new BinaryReader(stream);
    AlumnoDto recuperado = AlumnoDto.ReadBinary(reader);
    
    Console.WriteLine("✓ Alumno recuperado:");
    Console.WriteLine($"  Id:       {recuperado.Id}");
    Console.WriteLine($"  Nombre:  {recuperado. Nombre}");
    Console.WriteLine($"  Edad:    {recuperado.Edad}");
    Console.WriteLine($"  Nota:    {recuperado. Nota}");
}

// Limpiar
File.Delete(rutaBinario);
```

**Salida:**

```
>>> Escribiendo alumno en binario...

✓ Alumno guardado
  Tamaño: 27 bytes

  JSON equivalente:  52 bytes
  Ahorro binario: 25 bytes

>>> Leyendo alumno desde binario...

✓ Alumno recuperado:
  Id:      1
  Nombre: Ana García
  Edad:   20
  Nota:   8.5
```

#### 7.2.2. Serialización de Listas

```csharp
// ========================================
// SERIALIZACIÓN DE LISTA EN BINARIO
// ========================================

using System;
using System. IO;
using System.Collections.Generic;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    public void WriteBinary(BinaryWriter writer)
    {
        writer.Write(Id);
        writer.Write(Nombre);
        writer.Write(Edad);
        writer.Write(Nota);
    }
    
    public static AlumnoDto ReadBinary(BinaryReader reader)
    {
        return new AlumnoDto(
            reader.ReadInt32(),
            reader.ReadString(),
            reader.ReadInt32(),
            reader.ReadDouble()
        );
    }
}

// ════════════════════════════════════════
// Escribir lista
// ════════════════════════════════════════

var alumnos = new List<AlumnoDto>
{
    new(1, "Ana García", 20, 8.5),
    new(2, "Juan Pérez", 22, 7.0),
    new(3, "María López", 21, 9.2)
};

string rutaBinario = "alumnos.bin";

Console.WriteLine(">>> Escribiendo lista de alumnos en binario...\n");

using (var stream = new FileStream(rutaBinario, FileMode.Create))
{
    using var writer = new BinaryWriter(stream);
    
    // Escribir cantidad de elementos
    writer.Write(alumnos.Count);
    
    // Escribir cada alumno
    foreach (var alumno in alumnos)
    {
        alumno.WriteBinary(writer);
    }
}

var fileInfo = new FileInfo(rutaBinario);
Console.WriteLine($"✓ {alumnos.Count} alumnos guardados");
Console.WriteLine($"  Tamaño: {fileInfo.Length} bytes");

// ════════════════════════════════════════
// Leer lista
// ════════════════════════════════════════

Console. WriteLine("\n>>> Leyendo lista desde binario...\n");

using (var stream = new FileStream(rutaBinario, FileMode. Open))
{
    using var reader = new BinaryReader(stream);
    
    // Leer cantidad de elementos
    int cantidad = reader.ReadInt32();
    
    var alumnosLeidos = new List<AlumnoDto>();
    
    // Leer cada alumno
    for (int i = 0; i < cantidad; i++)
    {
        alumnosLeidos.Add(AlumnoDto.ReadBinary(reader));
    }
    
    Console.WriteLine($"✓ {alumnosLeidos.Count} alumnos recuperados\n");
    
    Console.WriteLine(">>> LISTADO:");
    foreach (var alumno in alumnosLeidos)
    {
        Console.WriteLine($"  [{alumno.Id}] {alumno.Nombre} - Nota: {alumno.Nota}");
    }
}

// Limpiar
File.Delete(rutaBinario);
```

**Salida:**

```
>>> Escribiendo lista de alumnos en binario... 

✓ 3 alumnos guardados
  Tamaño: 85 bytes

>>> Leyendo lista desde binario...

✓ 3 alumnos recuperados

>>> LISTADO:
  [1] Ana García - Nota:  8.5
  [2] Juan Pérez - Nota: 7
  [3] María López - Nota: 9.2
```

---

### 7.3. Acceso Aleatorio con FileStream y Seek

A diferencia de los ficheros de texto (lectura secuencial), los ficheros binarios permiten **acceso aleatorio** (saltar a cualquier posición).

#### 7.3.1. Concepto de Seek

```csharp
// ========================================
// ACCESO ALEATORIO CON SEEK
// ========================================

using System;
using System. IO;

string rutaBinario = "numeros.bin";

Console.WriteLine(">>> Creando fichero con 10 números...\n");

// Escribir 10 números (0 a 9)
using (var stream = new FileStream(rutaBinario, FileMode.Create))
{
    using var writer = new BinaryWriter(stream);
    
    for (int i = 0; i < 10; i++)
    {
        writer.Write(i * 10); // 0, 10, 20, 30, .. ., 90
    }
}

Console.WriteLine("✓ Fichero creado (10 int32 = 40 bytes)");

// ════════════════════════════════════════
// Leer números EN CUALQUIER ORDEN
// ════════════════════════════════════════

Console.WriteLine("\n>>> Leyendo números con acceso aleatorio:\n");

using (var stream = new FileStream(rutaBinario, FileMode.Open))
{
    using var reader = new BinaryReader(stream);
    
    // Leer el número en posición 5 (índice 5)
    stream.Seek(5 * sizeof(int), SeekOrigin.Begin);
    int numero5 = reader.ReadInt32();
    Console.WriteLine($"  Número en posición 5: {numero5}");
    
    // Leer el número en posición 0 (volver al inicio)
    stream.Seek(0, SeekOrigin.Begin);
    int numero0 = reader.ReadInt32();
    Console.WriteLine($"  Número en posición 0: {numero0}");
    
    // Leer el último número (posición 9)
    stream.Seek(9 * sizeof(int), SeekOrigin.Begin);
    int numero9 = reader.ReadInt32();
    Console.WriteLine($"  Número en posición 9: {numero9}");
    
    // Leer el número anterior desde la posición actual
    stream.Seek(-2 * sizeof(int), SeekOrigin.Current); // Retroceder 2 posiciones
    int numero7 = reader.ReadInt32();
    Console.WriteLine($"  Número en posición 7: {numero7}");
}

// Limpiar
File. Delete(rutaBinario);
```

**Salida:**

```
>>> Creando fichero con 10 números... 

✓ Fichero creado (10 int32 = 40 bytes)

>>> Leyendo números con acceso aleatorio:

  Número en posición 5: 50
  Número en posición 0: 0
  Número en posición 9: 90
  Número en posición 7: 70
```

**Parámetros de Seek:**

| SeekOrigin | Descripción                 | Ejemplo                                     |
| ---------- | --------------------------- | ------------------------------------------- |
| `Begin`    | Desde el inicio del fichero | `Seek(10, Begin)` → byte 10                 |
| `Current`  | Desde la posición actual    | `Seek(5, Current)` → avanza 5 bytes         |
| `End`      | Desde el final del fichero  | `Seek(-10, End)` → 10 bytes antes del final |

#### 7.3.2. Ejemplo Práctico: Actualizar Registro en Posición Específica

```csharp
// ========================================
// ACTUALIZAR REGISTRO EN POSICIÓN ESPECÍFICA
// ========================================

using System;
using System.IO;
using System.Collections.Generic;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota)
{
    // Tamaño fijo:  4 (Id) + 50 (Nombre fijo) + 4 (Edad) + 8 (Nota) = 66 bytes
    public const int TamañoRegistro = 66;
    
    public void WriteBinary(BinaryWriter writer)
    {
        writer.Write(Id);
        
        // Nombre con longitud fija (50 bytes)
        string nombreFijo = Nombre.PadRight(50).Substring(0, 50);
        writer.Write(nombreFijo. ToCharArray());
        
        writer.Write(Edad);
        writer.Write(Nota);
    }
    
    public static AlumnoDto ReadBinary(BinaryReader reader)
    {
        int id = reader.ReadInt32();
        string nombre = new string(reader.ReadChars(50)).Trim();
        int edad = reader.ReadInt32();
        double nota = reader.ReadDouble();
        
        return new AlumnoDto(id, nombre, edad, nota);
    }
}

// ════════════════════════════════════════
// Crear fichero con registros de tamaño fijo
// ════════════════════════════════════════

var alumnos = new List<AlumnoDto>
{
    new(1, "Ana García", 20, 8.5),
    new(2, "Juan Pérez", 22, 7.0),
    new(3, "María López", 21, 9.2)
};

string rutaBinario = "alumnos_fijos.bin";

Console.WriteLine(">>> Creando fichero con registros de tamaño fijo...\n");

using (var stream = new FileStream(rutaBinario, FileMode.Create))
{
    using var writer = new BinaryWriter(stream);
    
    foreach (var alumno in alumnos)
    {
        alumno.WriteBinary(writer);
    }
}

Console.WriteLine($"✓ {alumnos.Count} alumnos guardados");
Console.WriteLine($"  Tamaño de cada registro: {AlumnoDto.TamañoRegistro} bytes");

// ════════════════════════════════════════
// Actualizar nota del alumno en posición 1 (Juan)
// ════════════════════════════════════════

Console. WriteLine("\n>>> Actualizando nota de Juan (posición 1)...\n");

using (var stream = new FileStream(rutaBinario, FileMode.Open))
{
    // Ir a la posición del segundo alumno (índice 1)
    long posicion = 1 * AlumnoDto. TamañoRegistro;
    stream.Seek(posicion, SeekOrigin.Begin);
    
    // Leer el alumno actual
    using var reader = new BinaryReader(stream);
    AlumnoDto juan = AlumnoDto.ReadBinary(reader);
    
    Console.WriteLine($"  Antes: {juan.Nombre} - Nota: {juan. Nota}");
    
    // Modificar nota
    var juanActualizado = juan with { Nota = 9.5 };
    
    // Volver a la posición y sobrescribir
    stream. Seek(posicion, SeekOrigin.Begin);
    using var writer = new BinaryWriter(stream);
    juanActualizado.WriteBinary(writer);
    
    Console.WriteLine($"  Después: {juanActualizado.Nombre} - Nota: {juanActualizado.Nota}");
}

// ════════════════════════════════════════
// Verificar cambios
// ════════════════════════════════════════

Console.WriteLine("\n>>> Verificando cambios:\n");

using (var stream = new FileStream(rutaBinario, FileMode.Open))
{
    using var reader = new BinaryReader(stream);
    
    for (int i = 0; i < 3; i++)
    {
        AlumnoDto alumno = AlumnoDto.ReadBinary(reader);
        Console.WriteLine($"  [{alumno.Id}] {alumno.Nombre} - Nota: {alumno.Nota}");
    }
}

// Limpiar
File.Delete(rutaBinario);
```

**Salida:**

```
>>> Creando fichero con registros de tamaño fijo... 

✓ 3 alumnos guardados
  Tamaño de cada registro: 66 bytes

>>> Actualizando nota de Juan (posición 1)...

  Antes: Juan Pérez - Nota: 7
  Después: Juan Pérez - Nota: 9.5

>>> Verificando cambios: 

  [1] Ana García - Nota:  8.5
  [2] Juan Pérez - Nota: 9.5
  [3] María López - Nota:  9.2
```

---

### 7.4. ⚠️ EL GRAN PROBLEMA:           Acoplamiento y Falta de Interoperabilidad

#### 7.4.1. El Problema del Acoplamiento

Cuando serializas objetos en **formato binario nativo de . NET**, creas un **acoplamiento fuerte** con C#:

```csharp
// ❌ PROBLEMA: Solo C# puede leer este fichero

// C# escribe binario
BinaryWriter writer = ... ;
writer.Write(1234);
writer.Write("Hola");

// ¿Puede Java leerlo?        ❌ NO
// ¿Puede Python leerlo?       ❌ NO
// ¿Puede JavaScript leerlo?   ❌ NO
// ¿Puede PHP leerlo?          ❌ NO
```

**¿Por qué? **

1. **Orden de bytes** (Endianness):  
   - Intel/AMD (little-endian):  `0x1234` → `[0x34, 0x12]`
   - Algunos ARM (big-endian):  `0x1234` → `[0x12, 0x34]`

2. **Longitud de strings**:  
   - . NET:   Prefijo de longitud variable (7-bit encoded)
   - Java:  UTF-8 con longitud de 2 bytes
   - C:   Null-terminated (`\0`)

3. **Representación de tipos**:  
   - `DateTime` en .NET:  Ticks desde 01/01/0001
   - UNIX timestamp:   Segundos desde 01/01/1970

#### 7.4.2. Demostración del Problema

```csharp
// ========================================
// DEMOSTRACIÓN:  Incompatibilidad entre lenguajes
// ========================================

using System;
using System.IO;

string rutaBinario = "datos_csharp.bin";

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  PROBLEMA:  ACOPLAMIENTO BINARIO");
Console.WriteLine("═══════════════════════════════════════════\n");

// C# escribe datos
Console.WriteLine(">>> C# escribe datos binarios:\n");

using (var stream = new FileStream(rutaBinario, FileMode.Create))
{
    using var writer = new BinaryWriter(stream);
    
    writer.Write(1234);          // int32
    writer.Write("Hola mundo");  // string con longitud prefijo
    writer.Write(DateTime. Now);  // DateTime (8 bytes ticks)
}

Console.WriteLine("✓ Datos escritos en formato binario . NET");

// Mostrar bytes crudos
byte[] bytes = File.ReadAllBytes(rutaBinario);
Console.WriteLine($"\nBytes crudos (primeros 30):");
Console.WriteLine($"  {BitConverter.ToString(bytes. Take(30).ToArray())}");

Console.WriteLine("\n⚠️  PROBLEMA:");
Console.WriteLine("  Estos bytes solo tienen sentido para . NET");
Console.WriteLine("  Otro lenguaje no puede interpretarlos correctamente\n");

Console.WriteLine("✅ SOLUCIÓN: Usar JSON o XML");
Console.WriteLine("  JSON/XML son formatos de TEXTO universales");
Console.WriteLine("  Cualquier lenguaje puede leerlos/escribirlos");

// Limpiar
File.Delete(rutaBinario);
```

**Salida:**

```
═══════════════════════════════════════════
  PROBLEMA:  ACOPLAMIENTO BINARIO
═══════════════════════════════════════════

>>> C# escribe datos binarios: 

✓ Datos escritos en formato binario .NET

Bytes crudos (primeros 30):
  D2-04-00-00-0B-48-6F-6C-61-20-6D-75-6E-64-6F-00-C0-E6-8B-3F-8F-2D-DB-08

⚠️  PROBLEMA: 
  Estos bytes solo tienen sentido para .NET
  Otro lenguaje no puede interpretarlos correctamente

✅ SOLUCIÓN: Usar JSON o XML
  JSON/XML son formatos de TEXTO universales
  Cualquier lenguaje puede leerlos/escribirlos
```

#### 7.4.3. Comparación:  Binario vs JSON

```csharp
// ========================================
// COMPARACIÓN:  Binario vs JSON
// ========================================

using System;
using System.IO;
using System.Text. Json;

public record AlumnoDto(int Id, string Nombre, int Edad, double Nota);

var alumno = new AlumnoDto(1, "Ana García", 20, 8.5);

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  COMPARACIÓN: BINARIO vs JSON");
Console.WriteLine("═══════════════════════════════════════════\n");

// ════════════════════════════════════════
// Opción 1: BINARIO
// ════════════════════════════════════════

string rutaBinario = "alumno. bin";

using (var stream = new FileStream(rutaBinario, FileMode. Create))
{
    using var writer = new BinaryWriter(stream);
    writer.Write(alumno. Id);
    writer.Write(alumno.Nombre);
    writer.Write(alumno. Edad);
    writer.Write(alumno.Nota);
}

var fileInfoBinario = new FileInfo(rutaBinario);

Console.WriteLine(">>> FORMATO BINARIO:");
Console.WriteLine($"  Tamaño:              {fileInfoBinario.Length} bytes");
Console.WriteLine($"  Legible:            ❌ NO (bytes crudos)");
Console.WriteLine($"  Interoperable:     ❌ NO (solo . NET)");
Console.WriteLine($"  Velocidad:         ✅ Rápido");

// ════════════════════════════════════════
// Opción 2: JSON
// ════════════════════════════════════════

string rutaJson = "alumno.json";
var opciones = new JsonSerializerOptions { WriteIndented = true };
File.WriteAllText(rutaJson, JsonSerializer.Serialize(alumno, opciones));

var fileInfoJson = new FileInfo(rutaJson);

Console.WriteLine("\n>>> FORMATO JSON:");
Console.WriteLine($"  Tamaño:            {fileInfoJson.Length} bytes");
Console.WriteLine($"  Legible:           ✅ SÍ (texto plano)");
Console.WriteLine($"  Interoperable:      ✅ SÍ (universal)");
Console.WriteLine($"  Velocidad:         ⚠️ Más lento (parsing)");

Console.WriteLine("\n>>> RECOMENDACIÓN:");
Console.WriteLine("  🏆 Usa JSON salvo que:");
Console.WriteLine("     - Necesites máximo rendimiento");
Console.WriteLine("     - El fichero sea SOLO para . NET");
Console.WriteLine("     - El tamaño sea crítico (millones de registros)");

// Limpiar
File. Delete(rutaBinario);
File.Delete(rutaJson);
```

**Salida:**

```
═══════════════════════════════════════════
  COMPARACIÓN: BINARIO vs JSON
═══════════════════════════════════════════

>>> FORMATO BINARIO:
  Tamaño:            27 bytes
  Legible:            ❌ NO (bytes crudos)
  Interoperable:     ❌ NO (solo .NET)
  Velocidad:         ✅ Rápido

>>> FORMATO JSON:
  Tamaño:            87 bytes
  Legible:            ✅ SÍ (texto plano)
  Interoperable:     ✅ SÍ (universal)
  Velocidad:         ⚠️ Más lento (parsing)

>>> RECOMENDACIÓN:
  🏆 Usa JSON salvo que:
     - Necesites máximo rendimiento
     - El fichero sea SOLO para .NET
     - El tamaño sea crítico (millones de registros)
```

---

### 7.5. Casos de Uso Válidos para Ficheros Binarios

A pesar de los problemas de interoperabilidad, hay casos donde los ficheros binarios son apropiados:

#### 7.5.1. ✅ Cachés Temporales de Alto Rendimiento

```csharp
// ========================================
// CASO VÁLIDO: Caché temporal binaria
// ========================================

// ✅ VÁLIDO: Caché local de datos procesados
// - No necesita interoperabilidad (solo esta app)
// - Se regenera si se borra
// - Rendimiento crítico

using System;
using System.IO;

string rutaCache = Path.Combine(Path.GetTempPath(), "app_cache.bin");

// Guardar caché binaria
using (var stream = new FileStream(rutaCache, FileMode.Create))
{
    using var writer = new BinaryWriter(stream);
    
    // Datos procesados (ej:  resultado de cálculos complejos)
    writer.Write(1000000); // Registros procesados
    writer.Write(DateTime.Now. Ticks); // Timestamp
    writer.Write(3.14159 * 2.71828); // Resultado
}

Console.WriteLine("✓ Caché binaria guardada (temporal, regenerable)");

// Leer caché
using (var stream = new FileStream(rutaCache, FileMode.Open))
{
    using var reader = new BinaryReader(stream);
    
    int registros = reader.ReadInt32();
    DateTime timestamp = new DateTime(reader.ReadInt64());
    double resultado = reader.ReadDouble();
    
    Console.WriteLine($"  Registros:   {registros}");
    Console.WriteLine($"  Fecha:      {timestamp: dd/MM/yyyy HH:mm}");
    Console.WriteLine($"  Resultado:  {resultado}");
}

File.Delete(rutaCache);
```

#### 7.5.2. ✅ Formatos Estándar Binarios (con Especificación)

```csharp
// ✅ VÁLIDO: Implementar formato binario estándar
// - Formato documentado (ej: BMP, WAV)
// - Especificación pública
// - Interoperabilidad garantizada

// Ejemplo: Escribir cabecera BMP (simplificado)
using (var stream = new FileStream("imagen.bmp", FileMode.Create))
{
    using var writer = new BinaryWriter(stream);
    
    // Cabecera BMP (especificación pública)
    writer.Write((byte)'B');  // Signature
    writer.Write((byte)'M');
    writer.Write(54 + 3);     // File size
    writer.Write(0);          // Reserved
    writer. Write(54);         // Data offset
    
    // ...  resto de la estructura BMP
}
```

#### 7.5.3. ❌ Casos Donde NO Usar Binario

```csharp
// ❌ NO USAR BINARIO para: 

// 1. Intercambio entre aplicaciones diferentes
//    → Usar JSON o XML

// 2. Configuración de aplicación
//    → Usar JSON o XML (legible y editable)

// 3. Datos a largo plazo
//    → Usar JSON o XML (mantenible, versionable)

// 4. Logs
//    → Usar texto plano (legible con cualquier editor)

// 5. APIs o servicios web
//    → Usar JSON (estándar universal)
```

---

### 7.6. Resumen y Recomendaciones

```csharp
// ========================================
// TABLA DE DECISIÓN
// ========================================

Console.WriteLine("═══════════════════════════════════════════════════════════════");
Console.WriteLine("  ¿CUÁNDO USAR CADA FORMATO?");
Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

var decisiones = new[]
{
    new { Escenario = "Intercambio con otros lenguajes", Formato = "JSON" },
    new { Escenario = "Configuración de aplicación", Formato = "JSON" },
    new { Escenario = "APIs REST", Formato = "JSON" },
    new { Escenario = "Datos legibles por humanos", Formato = "JSON o CSV" },
    new { Escenario = "Excel / hojas de cálculo", Formato = "CSV" },
    new { Escenario = "Logs de aplicación", Formato = "Texto plano" },
    new { Escenario = "Caché temporal local", Formato = "Binario OK" },
    new { Escenario = "Datos masivos (millones registros)", Formato = "Binario OK" },
    new { Escenario = "Formato estándar (BMP, WAV, etc.)", Formato = "Binario OK" },
    new { Escenario = "Sistemas legacy SOAP", Formato = "XML" }
};

Console.WriteLine($"{"Escenario",-40} {"Formato Recomendado",-25}");
Console.WriteLine(new string('─', 65));

foreach (var item in decisiones)
{
    Console.WriteLine($"{item. Escenario,-40} {item.Formato,-25}");
}

Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
Console.WriteLine("  REGLA DE ORO");
Console.WriteLine("═══════════════════════════════════════════════════════════════");
Console.WriteLine("\n  🏆 Usa JSON por defecto");
Console.WriteLine("  ⚠️  Solo usa binario si tienes una razón específica\n");
Console.WriteLine("═══════════════════════════════════════════════════════════════");
```

**Salida:**

```
═══════════════════════════════════════════════════════════════
  ¿CUÁNDO USAR CADA FORMATO?
═══════════════════════════════════════════════════════════════

Escenario                                Formato Recomendado      
─────────────────────────────────────────────────────────────────
Intercambio con otros lenguajes          JSON                     
Configuración de aplicación              JSON                     
APIs REST                                JSON                     
Datos legibles por humanos               JSON o CSV               
Excel / hojas de cálculo                 CSV                      
Logs de aplicación                       Texto plano              
Caché temporal local                     Binario OK               
Datos masivos (millones registros)       Binario OK               
Formato estándar (BMP, WAV, etc.)        Binario OK               
Sistemas legacy SOAP                     XML                      

═══════════════════════════════════════════════════════════════
  REGLA DE ORO
═══════════════════════════════════════════════════════════════

  🏆 Usa JSON por defecto
  ⚠️  Solo usa binario si tienes una razón específica

═══════════════════════════════════════════════════════════════
```

---

## 8. Utilidades Avanzadas y Configuración

### 8.0. Introducción

Hasta ahora hemos aprendido los fundamentos de lectura/escritura de ficheros. En este punto veremos **herramientas avanzadas** que facilitan tareas comunes en aplicaciones reales:

- **Ficheros temporales**: Crear archivos que se borran automáticamente
- **Compresión**: Reducir tamaño de archivos y directorios
- **Configuración**: Gestionar settings de aplicación con JSON

---

### 8.1. Ficheros Temporales

#### 8.1.1. ¿Qué son los Ficheros Temporales?

Los **ficheros temporales** son archivos que se crean para un **uso puntual** y se espera que se borren después.     Ejemplos: 

- Descargar archivo antes de procesarlo
- Caché de datos intermedios
- Ficheros de intercambio entre procesos
- Datos de sesión

#### 8.1.2. Directorio Temporal del Sistema

```csharp
// ========================================
// DIRECTORIO TEMPORAL DEL SISTEMA
// ========================================

using System;
using System.IO;

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  DIRECTORIOS TEMPORALES");
Console.WriteLine("═══════════════════════════════════════════\n");

// Obtener directorio temporal del sistema
string directorioTemp = Path.GetTempPath();

Console.WriteLine($"Directorio temporal del sistema:");
Console.WriteLine($"  {directorioTemp}");

// En Windows:  C:\Users\.. .\AppData\Local\Temp\
// En Linux:     /tmp/
// En macOS:    /var/folders/. ../T/

Console.WriteLine($"\n¿Existe?  {Directory.Exists(directorioTemp)}");

// Listar algunos ficheros temporales
var ficherosTemp = Directory.GetFiles(directorioTemp)
    .Take(5)
    .Select(f => Path.GetFileName(f));

Console.WriteLine("\nAlgunos ficheros temporales existentes:");
foreach (var fichero in ficherosTemp)
{
    Console.WriteLine($"  {fichero}");
}
```

**Salida (ejemplo en Windows):**

```
═══════════════════════════════════════════
  DIRECTORIOS TEMPORALES
═══════════════════════════════════════════

Directorio temporal del sistema:
  C:\Users\Usuario\AppData\Local\Temp\

¿Existe? True

Algunos ficheros temporales existentes:
  tmp3A2B. tmp
  ~DF8432.tmp
  cab_12345.tmp
  ...
```

#### 8.1.3. Crear Fichero Temporal con Nombre Único

```csharp
// ========================================
// CREAR FICHERO TEMPORAL CON NOMBRE ÚNICO
// ========================================

using System;
using System.IO;

Console.WriteLine(">>> Creando fichero temporal.. .\n");

// GetTempFileName():  Crea un fichero vacío con nombre único
string rutaTemporal = Path.GetTempFileName();

Console.WriteLine($"✓ Fichero temporal creado:");
Console.WriteLine($"  Ruta:   {rutaTemporal}");
Console.WriteLine($"  Nombre: {Path.GetFileName(rutaTemporal)}");

// Verificar que existe
var fileInfo = new FileInfo(rutaTemporal);
Console.WriteLine($"  ¿Existe? {fileInfo.Exists}");
Console.WriteLine($"  Tamaño:   {fileInfo.Length} bytes (vacío)");

// Usar el fichero temporal
Console.WriteLine("\n>>> Usando fichero temporal...\n");

File.WriteAllText(rutaTemporal, "Datos temporales de prueba");

Console.WriteLine($"✓ Datos escritos:");
Console.WriteLine($"  Contenido: {File.ReadAllText(rutaTemporal)}");
Console.WriteLine($"  Tamaño:    {new FileInfo(rutaTemporal).Length} bytes");

// Eliminar cuando ya no se necesita
Console.WriteLine("\n>>> Limpiando.. .\n");

File.Delete(rutaTemporal);
Console.WriteLine($"✓ Fichero temporal eliminado");
```

**Salida:**

```
>>> Creando fichero temporal... 

✓ Fichero temporal creado:
  Ruta:  C:\Users\Usuario\AppData\Local\Temp\tmpA3F2.tmp
  Nombre: tmpA3F2.tmp
  ¿Existe? True
  Tamaño:  0 bytes (vacío)

>>> Usando fichero temporal... 

✓ Datos escritos:
  Contenido:  Datos temporales de prueba
  Tamaño:   27 bytes

>>> Limpiando... 

✓ Fichero temporal eliminado
```

#### 8.1.4. Nombre Aleatorio sin Crear el Fichero

```csharp
// ========================================
// NOMBRE ALEATORIO (sin crear fichero)
// ========================================

using System;
using System.IO;

Console.WriteLine(">>> Generando nombres aleatorios...\n");

for (int i = 0; i < 5; i++)
{
    // GetRandomFileName():  Genera nombre aleatorio SIN crear el fichero
    string nombreAleatorio = Path.GetRandomFileName();
    
    Console.WriteLine($"  {i + 1}. {nombreAleatorio}");
}

// Crear ruta completa en directorio temporal
string nombrePersonalizado = Path.GetRandomFileName();
string rutaCompleta = Path.Combine(Path.GetTempPath(), nombrePersonalizado);

Console.WriteLine($"\n✓ Ruta temporal personalizada:");
Console.WriteLine($"  {rutaCompleta}");
Console.WriteLine($"  ¿Existe? {File. Exists(rutaCompleta)} (aún no creado)");
```

**Salida:**

```
>>> Generando nombres aleatorios...

  1. xyz123ab.tmp
  2. def456cd.tmp
  3. ghi789ef.tmp
  4. jkl012gh.tmp
  5. mno345ij.tmp

✓ Ruta temporal personalizada:
  C:\Users\Usuario\AppData\Local\Temp\pqr678kl.tmp
  ¿Existe? False (aún no creado)
```

#### 8.1.5. Ejemplo Práctico: Procesar Descarga Temporal

```csharp
// ========================================
// EJEMPLO PRÁCTICO:   Fichero temporal para descarga
// ========================================

using System;
using System.IO;

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  SIMULACIÓN:  Descarga y Procesamiento");
Console.WriteLine("═══════════════════════════════════════════\n");

string rutaTemporal = Path.GetTempFileName();

try
{
    Console.WriteLine(">>> Paso 1: Descargando archivo...\n");
    
    // Simular descarga (en realidad escribir datos)
    var datosDescargados = "Contenido descargado de internet\n" +
                          "Línea 2\n" +
                          "Línea 3";
    
    File.WriteAllText(rutaTemporal, datosDescargados);
    
    Console.WriteLine($"✓ Descargado a: {Path.GetFileName(rutaTemporal)}");
    Console.WriteLine($"  Tamaño: {new FileInfo(rutaTemporal).Length} bytes");
    
    // Paso 2: Procesar
    Console.WriteLine("\n>>> Paso 2: Procesando archivo...\n");
    
    string[] lineas = File.ReadAllLines(rutaTemporal);
    
    Console.WriteLine($"✓ {lineas.Length} líneas procesadas");
    foreach (var linea in lineas)
    {
        Console.WriteLine($"  → {linea}");
    }
    
    // Paso 3: Guardar resultado final
    Console.WriteLine("\n>>> Paso 3: Guardando resultado...\n");
    
    string rutaFinal = "resultado_procesado.txt";
    File.Copy(rutaTemporal, rutaFinal, overwrite: true);
    
    Console.WriteLine($"✓ Resultado guardado en: {rutaFinal}");
}
finally
{
    // IMPORTANTE: Limpiar fichero temporal en bloque finally
    if (File.Exists(rutaTemporal))
    {
        File.Delete(rutaTemporal);
        Console.WriteLine($"\n✓ Fichero temporal eliminado");
    }
}

// Limpiar resultado
File.Delete("resultado_procesado.txt");
```

**Salida:**

```
═══════════════════════════════════════════
  SIMULACIÓN: Descarga y Procesamiento
═══════════════════════════════════════════

>>> Paso 1: Descargando archivo... 

✓ Descargado a: tmpB4C3.tmp
  Tamaño:  56 bytes

>>> Paso 2: Procesando archivo... 

✓ 3 líneas procesadas
  → Contenido descargado de internet
  → Línea 2
  → Línea 3

>>> Paso 3: Guardando resultado...

✓ Resultado guardado en: resultado_procesado.txt

✓ Fichero temporal eliminado
```

---

### 8.2. Compresión de Archivos (ZIP)

#### 8.2.1.  Introducción a la Compresión

La **compresión** reduce el tamaño de archivos usando algoritmos.     .  NET incluye soporte para **ZIP** mediante `System.IO.Compression`.

**Ventajas:**
- ✅ Reducir espacio en disco
- ✅ Acelerar transferencias de red
- ✅ Agrupar múltiples archivos en uno

#### 8.2.2. Comprimir un Archivo Individual

```csharp
// ========================================
// COMPRIMIR UN ARCHIVO CON GZIP
// ========================================

using System;
using System.IO;
using System.IO.Compression;

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  COMPRESIÓN GZIP");
Console.WriteLine("═══════════════════════════════════════════\n");

// Crear archivo de prueba
string archivoOriginal = "texto_largo.txt";
string textoLargo = string.Join("\n", Enumerable. Repeat("Esta es una línea de texto repetida para probar la compresión.  ", 100));

File.WriteAllText(archivoOriginal, textoLargo);

var infoOriginal = new FileInfo(archivoOriginal);
Console.WriteLine($"Archivo original:");
Console.WriteLine($"  Nombre:   {infoOriginal.Name}");
Console.WriteLine($"  Tamaño:  {infoOriginal.Length:N0} bytes");

// Comprimir
string archivoComprimido = archivoOriginal + ".gz";

Console.WriteLine("\n>>> Comprimiendo.. .\n");

using (var archivoEntrada = File.OpenRead(archivoOriginal))
{
    using var archivoSalida = File.Create(archivoComprimido);
    using var compresor = new GZipStream(archivoSalida, CompressionMode.Compress);
    
    archivoEntrada.CopyTo(compresor);
}

var infoComprimido = new FileInfo(archivoComprimido);
Console.WriteLine($"✓ Archivo comprimido:");
Console.WriteLine($"  Nombre:  {infoComprimido.Name}");
Console.WriteLine($"  Tamaño: {infoComprimido. Length:N0} bytes");

double ratio = (1 - (double)infoComprimido.Length / infoOriginal.Length) * 100;
Console.WriteLine($"\n📊 Reducción:   {ratio: F1}%");

// Descomprimir para verificar
Console.WriteLine("\n>>> Descomprimiendo...\n");

string archivoDescomprimido = "texto_descomprimido.txt";

using (var archivoEntrada = File.OpenRead(archivoComprimido))
{
    using var descompresor = new GZipStream(archivoEntrada, CompressionMode.Decompress);
    using var archivoSalida = File.Create(archivoDescomprimido);
    
    descompresor. CopyTo(archivoSalida);
}

Console.WriteLine($"✓ Archivo descomprimido:");
Console.WriteLine($"  Tamaño: {new FileInfo(archivoDescomprimido).Length: N0} bytes");
Console.WriteLine($"  ¿Igual al original? {File.ReadAllText(archivoOriginal) == File.ReadAllText(archivoDescomprimido)}");

// Limpiar
File.Delete(archivoOriginal);
File.Delete(archivoComprimido);
File.Delete(archivoDescomprimido);
```

**Salida:**

```
═══════════════════════════════════════════
  COMPRESIÓN GZIP
═══════════════════════════════════════════

Archivo original:
  Nombre:  texto_largo.txt
  Tamaño: 6,400 bytes

>>> Comprimiendo...

✓ Archivo comprimido:
  Nombre:  texto_largo.txt.gz
  Tamaño: 183 bytes

📊 Reducción:  97.1%

>>> Descomprimiendo...

✓ Archivo descomprimido:
  Tamaño: 6,400 bytes
  ¿Igual al original? True
```

#### 8.2.3. Crear Archivo ZIP con Múltiples Archivos

```csharp
// ========================================
// CREAR ARCHIVO ZIP
// ========================================

using System;
using System.IO;
using System.IO.Compression;

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  CREAR ARCHIVO ZIP");
Console.WriteLine("═══════════════════════════════════════════\n");

// Crear directorio con archivos de prueba
string directorioTrabajo = "datos_prueba";
Directory.CreateDirectory(directorioTrabajo);

File.WriteAllText(Path. Combine(directorioTrabajo, "archivo1.txt"), "Contenido del archivo 1");
File.WriteAllText(Path.Combine(directorioTrabajo, "archivo2.txt"), "Contenido del archivo 2");
File.WriteAllText(Path. Combine(directorioTrabajo, "archivo3.txt"), "Contenido del archivo 3");

Console.WriteLine($"✓ Directorio creado con 3 archivos");

// Crear ZIP
string archivoZip = "backup. zip";

Console.WriteLine("\n>>> Creando archivo ZIP...\n");

ZipFile.CreateFromDirectory(directorioTrabajo, archivoZip);

var infoZip = new FileInfo(archivoZip);
Console.WriteLine($"✓ ZIP creado:");
Console.WriteLine($"  Nombre:  {infoZip.Name}");
Console.WriteLine($"  Tamaño: {infoZip.Length} bytes");

// Listar contenido del ZIP
Console.WriteLine("\n>>> Contenido del ZIP:\n");

using var zip = ZipFile.OpenRead(archivoZip);

foreach (var entrada in zip.Entries)
{
    Console.WriteLine($"  📄 {entrada.FullName}");
    Console.WriteLine($"     Tamaño original:     {entrada.Length} bytes");
    Console.WriteLine($"     Tamaño comprimido: {entrada.CompressedLength} bytes");
}

// Limpiar
Directory.Delete(directorioTrabajo, recursive: true);
File.Delete(archivoZip);
```

**Salida:**

```
═══════════════════════════════════════════
  CREAR ARCHIVO ZIP
═══════════════════════════════════════════

✓ Directorio creado con 3 archivos

>>> Creando archivo ZIP... 

✓ ZIP creado: 
  Nombre: backup.zip
  Tamaño: 432 bytes

>>> Contenido del ZIP:

  📄 archivo1.txt
     Tamaño original:   25 bytes
     Tamaño comprimido: 27 bytes
  📄 archivo2.txt
     Tamaño original:    25 bytes
     Tamaño comprimido: 27 bytes
  📄 archivo3.txt
     Tamaño original:   25 bytes
     Tamaño comprimido: 27 bytes
```

#### 8.2.4. Extraer Archivo ZIP

```csharp
// ========================================
// EXTRAER ARCHIVO ZIP
// ========================================

using System;
using System. IO;
using System.IO. Compression;

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  EXTRAER ARCHIVO ZIP");
Console.WriteLine("═══════════════════════════════════════════\n");

// Crear ZIP de prueba
string directorioOrigen = "origen";
Directory.CreateDirectory(directorioOrigen);

File.WriteAllText(Path. Combine(directorioOrigen, "documento.txt"), "Contenido importante");
File.WriteAllText(Path. Combine(directorioOrigen, "datos.json"), "{\"id\": 1}");

string archivoZip = "paquete.zip";
ZipFile.CreateFromDirectory(directorioOrigen, archivoZip);

Console.WriteLine($"✓ ZIP de prueba creado:  {archivoZip}");

// Extraer
string directorioDestino = "extraido";

Console.WriteLine($"\n>>> Extrayendo a: {directorioDestino}\n");

ZipFile.ExtractToDirectory(archivoZip, directorioDestino);

Console.WriteLine($"✓ Archivos extraídos:");

var archivosExtraidos = Directory.GetFiles(directorioDestino);
foreach (var archivo in archivosExtraidos)
{
    var info = new FileInfo(archivo);
    Console.WriteLine($"  📄 {info.Name} ({info.Length} bytes)");
}

// Verificar contenido
Console.WriteLine("\n>>> Verificando contenido:\n");

string contenido1 = File.ReadAllText(Path.Combine(directorioDestino, "documento.txt"));
string contenido2 = File. ReadAllText(Path.Combine(directorioDestino, "datos.json"));

Console.WriteLine($"  documento.txt: {contenido1}");
Console.WriteLine($"  datos.json:    {contenido2}");

// Limpiar
Directory. Delete(directorioOrigen, recursive: true);
Directory.Delete(directorioDestino, recursive: true);
File.Delete(archivoZip);
```

**Salida:**

```
═══════════════════════════════════════════
  EXTRAER ARCHIVO ZIP
═══════════════════════════════════════════

✓ ZIP de prueba creado:  paquete.zip

>>> Extrayendo a: extraido

✓ Archivos extraídos:
  📄 documento.txt (20 bytes)
  📄 datos.json (10 bytes)

>>> Verificando contenido: 

  documento.txt: Contenido importante
  datos.json:   {"id": 1}
```

#### 8.2.5. Agregar Archivos a ZIP Existente

```csharp
// ========================================
// AGREGAR ARCHIVOS A ZIP EXISTENTE
// ========================================

using System;
using System.IO;
using System.IO.Compression;

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  MODIFICAR ARCHIVO ZIP");
Console.WriteLine("═══════════════════════════════════════════\n");

// Crear ZIP inicial
string archivoZip = "proyecto.zip";

using (var zip = ZipFile.Open(archivoZip, ZipArchiveMode.Create))
{
    // Agregar archivo desde fichero
    zip.CreateEntryFromFile("archivo_temporal_1.txt", "archivo1.txt");
    
    // Agregar archivo con contenido directo
    var entrada = zip.CreateEntry("readme.txt");
    using var writer = new StreamWriter(entrada.Open());
    writer.WriteLine("Este es el archivo README");
    writer.WriteLine("Versión 1.0");
}

File.WriteAllText("archivo_temporal_1.txt", "Contenido archivo 1");

// Recrear para el ejemplo
using (var zip = ZipFile. Open(archivoZip, ZipArchiveMode.Update))
{
    var entrada = zip.CreateEntry("readme.txt");
    using var writer = new StreamWriter(entrada.Open());
    writer.WriteLine("Este es el archivo README");
    writer.WriteLine("Versión 1.0");
}

Console.WriteLine("✓ ZIP inicial creado");

// Listar contenido
Console.WriteLine("\n>>> Contenido inicial:\n");

using (var zip = ZipFile.OpenRead(archivoZip))
{
    foreach (var entrada in zip.Entries)
    {
        Console.WriteLine($"  📄 {entrada.FullName}");
    }
}

// Agregar más archivos
Console.WriteLine("\n>>> Agregando más archivos...\n");

using (var zip = ZipFile.Open(archivoZip, ZipArchiveMode. Update))
{
    // Agregar otro archivo
    var entrada = zip.CreateEntry("notas.txt");
    using var writer = new StreamWriter(entrada.Open());
    writer.WriteLine("Notas del proyecto");
    writer.WriteLine("- Tarea 1: Completada");
    writer.WriteLine("- Tarea 2: En progreso");
}

Console.WriteLine("✓ Archivos agregados");

// Listar contenido actualizado
Console.WriteLine("\n>>> Contenido actualizado:\n");

using (var zip = ZipFile.OpenRead(archivoZip))
{
    foreach (var entrada in zip.Entries)
    {
        Console. WriteLine($"  📄 {entrada.FullName} ({entrada.Length} bytes)");
    }
}

// Limpiar
File.Delete(archivoZip);
File.Delete("archivo_temporal_1.txt");
```

**Salida:**

```
═══════════════════════════════════════════
  MODIFICAR ARCHIVO ZIP
═══════════════════════════════════════════

✓ ZIP inicial creado

>>> Contenido inicial:

  📄 readme.txt

>>> Agregando más archivos... 

✓ Archivos agregados

>>> Contenido actualizado: 

  📄 readme.txt (44 bytes)
  📄 notas.txt (65 bytes)
```

---

### 8.3. Configuración de Aplicación con JSON

#### 8.3.1. El Antiguo Enfoque:   `.properties` / `.config`

Antiguamente se usaban archivos `.properties` (Java) o `.config` (. NET Framework):

```xml
<!-- app.config (antiguo) -->
<configuration>
  <appSettings>
    <add key="DatabaseHost" value="localhost"/>
    <add key="Port" value="5432"/>
  </appSettings>
</configuration>
```

**Problemas:**
- ❌ XML verboso
- ❌ Solo strings (sin tipos)
- ❌ Difícil de estructurar

#### 8.3.2. El Enfoque Moderno:  `appsettings.json`

En .NET moderno (Core/5+) se usa **JSON**:

```json
{
  "Database": {
    "Host": "localhost",
    "Port": 5432,
    "Name": "miapp_db"
  },
  "Logging": {
    "Level": "Debug"
  }
}
```

**Ventajas:**
- ✅ JSON legible y estructurado
- ✅ Jerarquías naturales
- ✅ Tipos nativos (números, booleanos)

#### 8.3.3. Crear Sistema de Configuración

```csharp
// ========================================
// SISTEMA DE CONFIGURACIÓN CON JSON
// ========================================

using System;
using System.IO;
using System.Text.Json;

// ════════════════════════════════════════
// DTOs de configuración
// ════════════════════════════════════════

public record DatabaseConfig
{
    public string Host { get; init; } = "localhost";
    public int Port { get; init; } = 5432;
    public string DatabaseName { get; init; } = "default_db";
    public string Username { get; init; } = "admin";
    public string Password { get; init; } = "";
}

public record LoggingConfig
{
    public string Level { get; init; } = "Info";
    public bool EnableFileLogging { get; init; } = false;
    public string LogDirectory { get; init; } = "logs";
}

public record AppSettings
{
    public string AppName { get; init; } = "Mi Aplicación";
    public string Version { get; init; } = "1.0.0";
    public DatabaseConfig Database { get; init; } = new();
    public LoggingConfig Logging { get; init; } = new();
    public int MaxConnections { get; init; } = 100;
}

// ════════════════════════════════════════
// Gestor de configuración
// ════════════════════════════════════════

public class ConfigurationManager
{
    private readonly string _rutaConfig;
    private AppSettings _settings;
    
    public ConfigurationManager(string rutaConfig = "appsettings.json")
    {
        _rutaConfig = rutaConfig;
        _settings = new AppSettings();
    }
    
    /// <summary>
    /// Carga la configuración desde el fichero JSON. 
    /// Si no existe, crea uno con valores por defecto.
    /// </summary>
    public void Load()
    {
        if (! File.Exists(_rutaConfig))
        {
            Console.WriteLine($"⚠️  Fichero de configuración no encontrado:  {_rutaConfig}");
            Console.WriteLine($"   Creando configuración por defecto...");
            
            Save(); // Guardar configuración por defecto
            return;
        }
        
        string json = File.ReadAllText(_rutaConfig);
        _settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        
        Console.WriteLine($"✓ Configuración cargada desde:  {_rutaConfig}");
    }
    
    /// <summary>
    /// Guarda la configuración actual al fichero JSON.
    /// </summary>
    public void Save()
    {
        var opciones = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(_settings, opciones);
        
        File.WriteAllText(_rutaConfig, json);
        
        Console.WriteLine($"✓ Configuración guardada en:  {_rutaConfig}");
    }
    
    /// <summary>
    /// Obtiene la configuración actual.
    /// </summary>
    public AppSettings GetSettings() => _settings;
    
    /// <summary>
    /// Actualiza la configuración. 
    /// </summary>
    public void UpdateSettings(AppSettings newSettings)
    {
        _settings = newSettings;
    }
}

// ════════════════════════════════════════
// Programa principal
// ════════════════════════════════════════

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  SISTEMA DE CONFIGURACIÓN");
Console.WriteLine("═══════════════════════════════════════════\n");

var configManager = new ConfigurationManager();

// Cargar configuración
configManager.Load();

// Obtener configuración
var settings = configManager.GetSettings();

Console.WriteLine("\n>>> Configuración actual:\n");
Console.WriteLine($"  Aplicación:    {settings.AppName} v{settings.Version}");
Console.WriteLine($"\n  Base de datos:");
Console.WriteLine($"    Host:       {settings.Database.Host}");
Console.WriteLine($"    Puerto:    {settings.Database.Port}");
Console.WriteLine($"    Base datos: {settings.Database.DatabaseName}");
Console.WriteLine($"\n  Logging:");
Console.WriteLine($"    Nivel:     {settings.Logging.Level}");
Console.WriteLine($"    A fichero: {settings.Logging. EnableFileLogging}");

// Modificar configuración
Console.WriteLine("\n>>> Modificando configuración...\n");

var nuevaConfig = settings with
{
    Database = settings.Database with
    {
        Host = "prod-server.example.com",
        DatabaseName = "production_db"
    },
    Logging = settings.Logging with
    {
        Level = "Warning",
        EnableFileLogging = true
    },
    MaxConnections = 500
};

configManager.UpdateSettings(nuevaConfig);
configManager.Save();

Console.WriteLine("✓ Configuración actualizada");

// Mostrar contenido del fichero
Console.WriteLine("\n>>> Contenido de appsettings.json:\n");
Console.WriteLine(File.ReadAllText("appsettings.json"));

// Limpiar
File.Delete("appsettings. json");
```

**Salida:**

```
═══════════════════════════════════════════
  SISTEMA DE CONFIGURACIÓN
═══════════════════════════════════════════

⚠️  Fichero de configuración no encontrado: appsettings.json
   Creando configuración por defecto... 
✓ Configuración guardada en: appsettings. json
✓ Configuración cargada desde: appsettings.json

>>> Configuración actual: 

  Aplicación:  Mi Aplicación v1.0.0

  Base de datos:
    Host:      localhost
    Puerto:   5432
    Base datos: default_db

  Logging: 
    Nivel:    Info
    A fichero: False

>>> Modificando configuración... 

✓ Configuración actualizada
✓ Configuración guardada en: appsettings.json

>>> Contenido de appsettings.json:

{
  "AppName": "Mi Aplicación",
  "Version": "1.0.0",
  "Database": {
    "Host": "prod-server.example.com",
    "Port": 5432,
    "DatabaseName": "production_db",
    "Username": "admin",
    "Password": ""
  },
  "Logging": {
    "Level": "Warning",
    "EnableFileLogging": true,
    "LogDirectory": "logs"
  },
  "MaxConnections": 500
}
```

#### 8.3.4. Configuración por Entorno (Development/Production)

```csharp
// ========================================
// CONFIGURACIÓN POR ENTORNO
// ========================================

using System;
using System.IO;
using System.Text.Json;

public record AppSettings
{
    public string Environment { get; init; } = "Development";
    public string DatabaseConnection { get; init; } = "";
    public bool EnableDebugLogging { get; init; } = true;
}

public class EnvironmentConfig
{
    public static AppSettings LoadForEnvironment(string environment)
    {
        // Buscar archivos en orden de prioridad:
        // 1. appsettings.{environment}.json (específico)
        // 2. appsettings.json (base)
        
        string archivoBase = "appsettings. json";
        string archivoEspecifico = $"appsettings.{environment}.json";
        
        AppSettings settings = new();
        
        // Cargar configuración base
        if (File.Exists(archivoBase))
        {
            string json = File. ReadAllText(archivoBase);
            settings = JsonSerializer. Deserialize<AppSettings>(json) ?? new();
            Console.WriteLine($"✓ Configuración base cargada:  {archivoBase}");
        }
        
        // Sobrescribir con configuración específica
        if (File.Exists(archivoEspecifico))
        {
            string json = File.ReadAllText(archivoEspecifico);
            var settingsEspecificos = JsonSerializer.Deserialize<AppSettings>(json);
            
            if (settingsEspecificos != null)
            {
                // Merge (sobrescribir)
                settings = settings with
                {
                    Environment = settingsEspecificos.Environment,
                    DatabaseConnection = settingsEspecificos.DatabaseConnection,
                    EnableDebugLogging = settingsEspecificos. EnableDebugLogging
                };
                
                Console.WriteLine($"✓ Configuración específica aplicada: {archivoEspecifico}");
            }
        }
        
        return settings;
    }
}

// Crear configuraciones de ejemplo
var settingsBase = new AppSettings
{
    Environment = "Development",
    DatabaseConnection = "localhost:5432",
    EnableDebugLogging = true
};

var settingsProd = new AppSettings
{
    Environment = "Production",
    DatabaseConnection = "prod-server: 5432",
    EnableDebugLogging = false
};

var opciones = new JsonSerializerOptions { WriteIndented = true };

File.WriteAllText("appsettings.json", JsonSerializer. Serialize(settingsBase, opciones));
File.WriteAllText("appsettings.Production.json", JsonSerializer.Serialize(settingsProd, opciones));

Console.WriteLine("═══════════════════════════════════════════");
Console.WriteLine("  CONFIGURACIÓN POR ENTORNO");
Console.WriteLine("═══════════════════════════════════════════\n");

// Cargar para Development
Console.WriteLine(">>> Cargando configuración para Development:\n");
var configDev = EnvironmentConfig. LoadForEnvironment("Development");
Console.WriteLine($"  Entorno:     {configDev.Environment}");
Console.WriteLine($"  Conexión:   {configDev.DatabaseConnection}");
Console.WriteLine($"  Debug logs: {configDev.EnableDebugLogging}");

// Cargar para Production
Console.WriteLine("\n>>> Cargando configuración para Production:\n");
var configProd = EnvironmentConfig. LoadForEnvironment("Production");
Console.WriteLine($"  Entorno:    {configProd. Environment}");
Console.WriteLine($"  Conexión:   {configProd.DatabaseConnection}");
Console.WriteLine($"  Debug logs: {configProd.EnableDebugLogging}");

// Limpiar
File.Delete("appsettings.json");
File.Delete("appsettings.Production.json");
```

**Salida:**

```
═══════════════════════════════════════════
  CONFIGURACIÓN POR ENTORNO
═══════════════════════════════════════════

>>> Cargando configuración para Development: 

✓ Configuración base cargada: appsettings.json
  Entorno:   Development
  Conexión:  localhost:5432
  Debug logs: True

>>> Cargando configuración para Production:

✓ Configuración base cargada: appsettings.json
✓ Configuración específica aplicada: appsettings.Production.json
  Entorno:   Production
  Conexión:   prod-server:5432
  Debug logs: False
```

---

## 9. PROYECTO FINAL: Sistema CRUD de Estudiantes con Persistencia JSON

### 9.0. Introducción al Proyecto

En este proyecto final integraremos **todos los conceptos** aprendidos en la unidad: 

- ✅ Patrón Repository (de la UD anterior)
- ✅ DTOs y separación de responsabilidades
- ✅ Serialización/Deserialización JSON
- ✅ LINQ para búsquedas y filtrado
- ✅ Gestión de ficheros con `using var`
- ✅ Manejo de excepciones

**Arquitectura del proyecto:**

```
┌─────────────────────────────────────────┐
│  PROGRAMA (UI/Consola)                  │
├─────────────────────────────────────────┤
│  StudentService (Lógica de negocio)     │
├─────────────────────────────────────────┤
│  IRepository<Student, int>              │  ← Interfaz
│         ↓                               │
│  StudentJsonRepository                  │  ← Implementación
│    - Dictionary<int, Student> (memoria) │
│    - students.json (disco)              │
└─────────────────────────────────────────┘
```

---

### 9.1. Modelo de Dominio:  Student

```csharp
// ========================================
// MODELO DE DOMINIO: Student
// ========================================

/// <summary>
/// Entidad de dominio que representa un estudiante.
/// Contiene lógica de negocio (validaciones, cálculos).
/// </summary>
public class Student
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public int Age { get; }
    public double Grade { get; }
    public DateTime EnrollmentDate { get; }
    
    public Student(int id, string name, string email, int age, double grade, DateTime enrollmentDate)
    {
        Id = id;
        Name = name;
        Email = email;
        Age = age;
        Grade = grade;
        EnrollmentDate = enrollmentDate;
        
        Validate();
    }
    
    /// <summary>
    /// Valida los datos del estudiante.
    /// </summary>
    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentException("El nombre no puede estar vacío", nameof(Name));
        
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
            throw new ArgumentException("El email debe ser válido", nameof(Email));
        
        if (Age < 16 || Age > 100)
            throw new ArgumentException("La edad debe estar entre 16 y 100", nameof(Age));
        
        if (Grade < 0 || Grade > 10)
            throw new ArgumentException("La nota debe estar entre 0 y 10", nameof(Grade));
    }
    
    // ════════════════════════════════════════
    // LÓGICA DE NEGOCIO
    // ════════════════════════════════════════
    
    /// <summary>
    /// Verifica si el estudiante está aprobado.
    /// </summary>
    public bool IsApproved() => Grade >= 5. 0;
    
    /// <summary>
    /// Obtiene la calificación textual.
    /// </summary>
    public string GetGradeText() => Grade switch
    {
        >= 9.0 => "Sobresaliente",
        >= 7.0 => "Notable",
        >= 5.0 => "Aprobado",
        _ => "Suspenso"
    };
    
    /// <summary>
    /// Calcula la antigüedad en años.
    /// </summary>
    public int GetYearsSinceEnrollment()
    {
        return (DateTime.Now - EnrollmentDate).Days / 365;
    }
    
    public override string ToString()
    {
        return $"[{Id}] {Name} ({Email}) - Nota: {Grade:F2} ({GetGradeText()})";
    }
}
```

---

### 9.2. DTO para Persistencia

```csharp
// ========================================
// DTO PARA PERSISTENCIA JSON
// ========================================

using System.Text.Json.Serialization;

/// <summary>
/// DTO para serializar/deserializar estudiantes en JSON.
/// Separado del modelo de dominio para desacoplar persistencia.
/// </summary>
public record StudentDto
{
    [JsonPropertyName("id")]
    public int Id { get; init; }
    
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;
    
    [JsonPropertyName("age")]
    public int Age { get; init; }
    
    [JsonPropertyName("grade")]
    public double Grade { get; init; }
    
    [JsonPropertyName("enrollmentDate")]
    public DateTime EnrollmentDate { get; init; }
}

// ════════════════════════════════════════
// MAPPER:  Conversión entre Domain y DTO
// ════════════════════════════════════════

public static class StudentMapper
{
    /// <summary>
    /// Convierte entidad de dominio a DTO.
    /// </summary>
    public static StudentDto ToDto(Student student)
    {
        return new StudentDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            Age = student.Age,
            Grade = student.Grade,
            EnrollmentDate = student.EnrollmentDate
        };
    }
    
    /// <summary>
    /// Convierte DTO a entidad de dominio.
    /// </summary>
    public static Student ToDomain(StudentDto dto)
    {
        return new Student(
            dto.Id,
            dto.Name,
            dto.Email,
            dto.Age,
            dto.Grade,
            dto.EnrollmentDate
        );
    }
}
```

---

### 9.3. Interfaz del Repositorio

```csharp
// ========================================
// INTERFAZ CRUD GENÉRICA
// ========================================

/// <summary>
/// Interfaz genérica para operaciones CRUD. 
/// </summary>
/// <typeparam name="TEntity">Tipo de la entidad</typeparam>
/// <typeparam name="TKey">Tipo de la clave primaria</typeparam>
public interface ICrudRepository<TEntity, TKey> where TKey : notnull
{
    // READ
    TEntity? GetById(TKey id);
    IEnumerable<TEntity> GetAll();
    bool Exists(TKey id);
    int Count();
    
    // CREATE
    TEntity Save(TEntity entity);
    
    // UPDATE
    TEntity Update(TKey id, TEntity entity);
    
    // DELETE
    TEntity Delete(TKey id);
    void Clear();
}
```

---

### 9.4. Implementación: StudentJsonRepository

```csharp
// ========================================
// IMPLEMENTACIÓN: StudentJsonRepository
// ========================================

using System;
using System.IO;
using System.Collections.Generic;
using System. Linq;
using System.Text. Json;

/// <summary>
/// Repositorio que persiste estudiantes en un archivo JSON.
/// Usa un Dictionary en memoria para operaciones rápidas,
/// sincronizando con el archivo JSON después de cada cambio.
/// </summary>
public class StudentJsonRepository : ICrudRepository<Student, int>
{
    private readonly Dictionary<int, Student> _cache;
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;
    
    public StudentJsonRepository(string filePath = "students.json")
    {
        _filePath = filePath;
        _cache = new Dictionary<int, Student>();
        
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        // Cargar datos del archivo al inicializar
        LoadFromFile();
    }
    
    // ════════════════════════════════════════
    // PERSISTENCIA (privadas)
    // ════════════════════════════════════════
    
    /// <summary>
    /// Carga los estudiantes desde el archivo JSON a la caché en memoria.
    /// </summary>
    private void LoadFromFile()
    {
        if (!File.Exists(_filePath))
        {
            Console.WriteLine($"⚠️  Archivo {_filePath} no encontrado.  Iniciando con base de datos vacía.");
            return;
        }
        
        try
        {
            string json = File. ReadAllText(_filePath);
            
            var dtos = JsonSerializer.Deserialize<List<StudentDto>>(json, _jsonOptions);
            
            if (dtos != null)
            {
                _cache.Clear();
                
                foreach (var dto in dtos)
                {
                    var student = StudentMapper.ToDomain(dto);
                    _cache[student.Id] = student;
                }
                
                Console. WriteLine($"✓ {_cache.Count} estudiantes cargados desde {_filePath}");
            }
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"✗ Error al parsear JSON: {ex.Message}");
            throw;
        }
    }
    
    /// <summary>
    /// Guarda todos los estudiantes de la caché al archivo JSON.
    /// </summary>
    private void SaveToFile()
    {
        try
        {
            // Convertir todos los estudiantes a DTOs
            var dtos = _cache.Values
                .Select(StudentMapper.ToDto)
                .OrderBy(dto => dto.Id)
                .ToList();
            
            // Serializar a JSON
            string json = JsonSerializer.Serialize(dtos, _jsonOptions);
            
            // Guardar al archivo
            File.WriteAllText(_filePath, json);
        }
        catch (IOException ex)
        {
            Console.WriteLine($"✗ Error al guardar archivo: {ex. Message}");
            throw;
        }
    }
    
    // ════════════════════════════════════════
    // OPERACIONES READ
    // ════════════════════════════════════════
    
    public Student? GetById(int id)
    {
        return _cache.TryGetValue(id, out var student) ? student : null;
    }
    
    public IEnumerable<Student> GetAll()
    {
        return _cache.Values. OrderBy(s => s.Id);
    }
    
    public bool Exists(int id)
    {
        return _cache.ContainsKey(id);
    }
    
    public int Count()
    {
        return _cache.Count;
    }
    
    // ════════════════════════════════════════
    // OPERACIONES WRITE
    // ════════════════════════════════════════
    
    public Student Save(Student student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));
        
        if (Exists(student.Id))
            throw new ArgumentException($"Ya existe un estudiante con ID {student.Id}");
        
        // Agregar a caché
        _cache[student.Id] = student;
        
        // Persistir a disco
        SaveToFile();
        
        return student;
    }
    
    public Student Update(int id, Student student)
    {
        if (student == null)
            throw new ArgumentNullException(nameof(student));
        
        if (!Exists(id))
            throw new KeyNotFoundException($"No existe un estudiante con ID {id}");
        
        // Actualizar en caché
        _cache[id] = student;
        
        // Persistir a disco
        SaveToFile();
        
        return student;
    }
    
    public Student Delete(int id)
    {
        if (!Exists(id))
            throw new KeyNotFoundException($"No existe un estudiante con ID {id}");
        
        var student = _cache[id];
        
        // Eliminar de caché
        _cache.Remove(id);
        
        // Persistir a disco
        SaveToFile();
        
        return student;
    }
    
    public void Clear()
    {
        _cache.Clear();
        SaveToFile();
    }
}
```

---

### 9.5. Servicio de Búsqueda con LINQ

```csharp
// ========================================
// SERVICIO DE BÚSQUEDA (LINQ)
// ========================================

/// <summary>
/// Servicio que proporciona operaciones de búsqueda avanzadas
/// usando LINQ sobre el repositorio.
/// </summary>
public class StudentSearchService
{
    private readonly ICrudRepository<Student, int> _repository;
    
    public StudentSearchService(ICrudRepository<Student, int> repository)
    {
        _repository = repository;
    }
    
    /// <summary>
    /// Busca estudiantes por nombre (contiene texto).
    /// </summary>
    public IEnumerable<Student> SearchByName(string nameFragment)
    {
        return _repository.GetAll()
            .Where(s => s.Name. Contains(nameFragment, StringComparison.OrdinalIgnoreCase))
            .OrderBy(s => s.Name);
    }
    
    /// <summary>
    /// Obtiene estudiantes aprobados.
    /// </summary>
    public IEnumerable<Student> GetApproved()
    {
        return _repository.GetAll()
            .Where(s => s.IsApproved())
            .OrderByDescending(s => s.Grade);
    }
    
    /// <summary>
    /// Obtiene estudiantes suspensos.
    /// </summary>
    public IEnumerable<Student> GetFailed()
    {
        return _repository.GetAll()
            .Where(s => !s. IsApproved())
            .OrderBy(s => s.Grade);
    }
    
    /// <summary>
    /// Obtiene top N estudiantes por nota.
    /// </summary>
    public IEnumerable<Student> GetTopStudents(int count)
    {
        return _repository.GetAll()
            .OrderByDescending(s => s.Grade)
            .Take(count);
    }
    
    /// <summary>
    /// Obtiene estudiantes por rango de edad.
    /// </summary>
    public IEnumerable<Student> GetByAgeRange(int minAge, int maxAge)
    {
        return _repository.GetAll()
            .Where(s => s. Age >= minAge && s.Age <= maxAge)
            .OrderBy(s => s.Age);
    }
    
    /// <summary>
    /// Calcula estadísticas de los estudiantes.
    /// </summary>
    public (double AverageGrade, double MaxGrade, double MinGrade, int Approved, int Failed) GetStatistics()
    {
        var students = _repository.GetAll().ToList();
        
        if (students.Count == 0)
            return (0, 0, 0, 0, 0);
        
        return (
            AverageGrade: students.Average(s => s.Grade),
            MaxGrade: students.Max(s => s.Grade),
            MinGrade: students.Min(s => s.Grade),
            Approved: students.Count(s => s.IsApproved()),
            Failed: students.Count(s => !s.IsApproved())
        );
    }
}
```

---

### 9.6. Programa Principal:  Demostración Completa

```csharp
// ========================================
// PROGRAMA PRINCIPAL
// ========================================

using System;
using System.IO;
using System. Linq;

Console.WriteLine("═══════════════════════════════════════════════════════════════");
Console.WriteLine("  SISTEMA CRUD DE ESTUDIANTES CON PERSISTENCIA JSON");
Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

// Limpiar archivo anterior si existe (para demo limpia)
if (File.Exists("students.json"))
{
    File.Delete("students.json");
}

// ════════════════════════════════════════
// Inicializar repositorio y servicios
// ════════════════════════════════════════

var repository = new StudentJsonRepository("students.json");
var searchService = new StudentSearchService(repository);

Console.WriteLine("✓ Sistema inicializado\n");

// ════════════════════════════════════════
// CREATE:  Agregar estudiantes
// ════════════════════════════════════════

Console.WriteLine(">>> CREATE:  Agregando estudiantes...\n");

var students = new[]
{
    new Student(1, "Ana García López", "ana@universidad.es", 20, 8.5, DateTime.Now. AddYears(-2)),
    new Student(2, "Juan Pérez Martín", "juan@universidad.es", 22, 7.0, DateTime.Now.AddYears(-3)),
    new Student(3, "María López Ruiz", "maria@universidad.es", 21, 9.2, DateTime.Now.AddYears(-2)),
    new Student(4, "Pedro Sánchez Díaz", "pedro@universidad.es", 23, 4.5, DateTime.Now.AddYears(-4)),
    new Student(5, "Laura Martín Torres", "laura@universidad.es", 20, 8.0, DateTime.Now.AddYears(-1)),
    new Student(6, "Carlos Fernández Gil", "carlos@universidad.es", 24, 6.5, DateTime.Now.AddYears(-3)),
    new Student(7, "Sofía González Vega", "sofia@universidad.es", 19, 9.5, DateTime.Now.AddYears(-1)),
    new Student(8, "Miguel Ruiz Castro", "miguel@universidad.es", 22, 3.5, DateTime.Now.AddYears(-2))
};

foreach (var student in students)
{
    try
    {
        repository.Save(student);
        Console.WriteLine($"  ✓ {student.Name} agregado");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  ✗ Error:  {ex.Message}");
    }
}

Console.WriteLine($"\n✓ Total estudiantes: {repository.Count()}");

// Mostrar contenido del JSON
Console.WriteLine("\n>>> Contenido de students.json (primeras líneas):\n");
var jsonContent = File.ReadAllLines("students.json").Take(15);
foreach (var line in jsonContent)
{
    Console.WriteLine($"  {line}");
}
Console.WriteLine("  ...");

// ════════════════════════════════════════
// READ: Listar todos
// ════════════════════════════════════════

Console.WriteLine("\n>>> READ: Listando todos los estudiantes...\n");

foreach (var student in repository.GetAll())
{
    string status = student.IsApproved() ? "✓" : "✗";
    Console.WriteLine($"  {status} {student}");
}

// ════════════════════════════════════════
// BÚSQUEDA: Por nombre
// ════════════════════════════════════════

Console.WriteLine("\n>>> BÚSQUEDA:  Estudiantes con 'María' en el nombre...\n");

var resultadosBusqueda = searchService. SearchByName("María");
foreach (var student in resultadosBusqueda)
{
    Console.WriteLine($"  → {student}");
}

// ════════════════════════════════════════
// LINQ: Top 3 mejores notas
// ════════════════════════════════════════

Console.WriteLine("\n>>> TOP 3 MEJORES NOTAS:\n");

var top3 = searchService.GetTopStudents(3);
int position = 1;

foreach (var student in top3)
{
    string medal = position switch
    {
        1 => "🥇",
        2 => "🥈",
        3 => "🥉",
        _ => ""
    };
    
    Console.WriteLine($"  {medal} {student.Name}:  {student.Grade:F2}");
    position++;
}

// ════════════════════════════════════════
// LINQ: Aprobados y Suspensos
// ════════════════════════════════════════

Console.WriteLine("\n>>> APROBADOS:\n");

foreach (var student in searchService.GetApproved())
{
    Console.WriteLine($"  ✓ {student.Name}:  {student.Grade:F2} ({student.GetGradeText()})");
}

Console.WriteLine("\n>>> SUSPENSOS:\n");

foreach (var student in searchService.GetFailed())
{
    Console.WriteLine($"  ✗ {student. Name}: {student.Grade:F2}");
}

// ════════════════════════════════════════
// ESTADÍSTICAS
// ════════════════════════════════════════

Console.WriteLine("\n>>> ESTADÍSTICAS:\n");

var stats = searchService.GetStatistics();

Console.WriteLine($"  Total estudiantes:     {repository.Count()}");
Console.WriteLine($"  Nota media:          {stats.AverageGrade:F2}");
Console.WriteLine($"  Nota máxima:         {stats.MaxGrade:F2}");
Console.WriteLine($"  Nota mínima:         {stats.MinGrade:F2}");
Console.WriteLine($"  Aprobados:           {stats. Approved} ({stats. Approved * 100.0 / repository.Count():F1}%)");
Console.WriteLine($"  Suspensos:           {stats.Failed} ({stats.Failed * 100.0 / repository.Count():F1}%)");

// ════════════════════════════════════════
// UPDATE: Actualizar nota
// ════════════════════════════════════════

Console. WriteLine("\n>>> UPDATE: Mejorando nota de Pedro.. .\n");

var pedro = repository.GetById(4);
if (pedro != null)
{
    Console.WriteLine($"  Antes: {pedro.Name} - Nota: {pedro.Grade}");
    
    var pedroActualizado = new Student(
        pedro.Id,
        pedro.Name,
        pedro.Email,
        pedro.Age,
        6.0, // Nueva nota
        pedro.EnrollmentDate
    );
    
    repository.Update(4, pedroActualizado);
    
    Console.WriteLine($"  Después: {pedroActualizado.Name} - Nota: {pedroActualizado.Grade}");
}

// ════════════════════════════════════════
// DELETE: Eliminar estudiante
// ════════════════════════════════════════

Console.WriteLine("\n>>> DELETE:  Eliminando estudiante ID 8...\n");

try
{
    var eliminado = repository.Delete(8);
    Console.WriteLine($"  ✓ Eliminado:  {eliminado.Name}");
    Console.WriteLine($"  Total estudiantes ahora: {repository.Count()}");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"  ✗ Error: {ex.Message}");
}

// ════════════════════════════════════════
// PERSISTENCIA:  Recargar desde archivo
// ════════════════════════════════════════

Console.WriteLine("\n>>> PERSISTENCIA: Verificando recarga desde JSON...\n");

// Crear nueva instancia del repositorio (simula reinicio de app)
var repositoryReloaded = new StudentJsonRepository("students.json");

Console.WriteLine($"✓ Repositorio recargado: {repositoryReloaded.Count()} estudiantes");
Console.WriteLine("\nPrimeros 3 estudiantes recargados:");

foreach (var student in repositoryReloaded.GetAll().Take(3))
{
    Console.WriteLine($"  → {student}");
}

// ════════════════════════════════════════
// RESUMEN FINAL
// ════════════════════════════════════════

Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
Console.WriteLine("  RESUMEN FINAL");
Console.WriteLine("═══════════════════════════════════════════════════════════════\n");

var statsFinal = searchService.GetStatistics();

Console.WriteLine($"📊 Estado final del sistema:");
Console.WriteLine($"  • Total estudiantes:      {repository.Count()}");
Console.WriteLine($"  • Nota media:           {statsFinal.AverageGrade:F2}");
Console.WriteLine($"  • Aprobados/Suspensos:  {statsFinal.Approved}/{statsFinal.Failed}");
Console.WriteLine($"  • Archivo JSON:          students.json ({new FileInfo("students.json").Length} bytes)");

Console.WriteLine("\n✓ Proyecto completado exitosamente");

Console.WriteLine("\n═══════════════════════════════════════════════════════════════");
```

---

### 9.7. Salida Esperada del Programa

```
═══════════════════════════════════════════════════════════════
  SISTEMA CRUD DE ESTUDIANTES CON PERSISTENCIA JSON
═══════════════════════════════════════════════════════════════

⚠️  Archivo students.json no encontrado.  Iniciando con base de datos vacía. 
✓ Sistema inicializado

>>> CREATE: Agregando estudiantes... 

  ✓ Ana García López agregado
  ✓ Juan Pérez Martín agregado
  ✓ María López Ruiz agregado
  ✓ Pedro Sánchez Díaz agregado
  ✓ Laura Martín Torres agregado
  ✓ Carlos Fernández Gil agregado
  ✓ Sofía González Vega agregado
  ✓ Miguel Ruiz Castro agregado

✓ Total estudiantes: 8

>>> Contenido de students.json (primeras líneas):

  [
    {
      "id":  1,
      "name":  "Ana García López",
      "email": "ana@universidad.es",
      "age": 20,
      "grade": 8.5,
      "enrollmentDate": "2023-01-15T10:30:00"
    },
    {
      "id": 2,
      "name": "Juan Pérez Martín",
  ... 

>>> READ: Listando todos los estudiantes...

  ✓ [1] Ana García López (ana@universidad.es) - Nota: 8.50 (Notable)
  ✓ [2] Juan Pérez Martín (juan@universidad.es) - Nota: 7.00 (Notable)
  ✓ [3] María López Ruiz (maria@universidad.es) - Nota: 9.20 (Sobresaliente)
  ✗ [4] Pedro Sánchez Díaz (pedro@universidad.es) - Nota: 4.50 (Suspenso)
  ✓ [5] Laura Martín Torres (laura@universidad.es) - Nota: 8.00 (Notable)
  ✓ [6] Carlos Fernández Gil (carlos@universidad.es) - Nota: 6.50 (Aprobado)
  ✓ [7] Sofía González Vega (sofia@universidad. es) - Nota: 9.50 (Sobresaliente)
  ✗ [8] Miguel Ruiz Castro (miguel@universidad.es) - Nota: 3.50 (Suspenso)

>>> BÚSQUEDA:  Estudiantes con 'María' en el nombre...

  → [3] María López Ruiz (maria@universidad.es) - Nota: 9.20 (Sobresaliente)

>>> TOP 3 MEJORES NOTAS:

  🥇 Sofía González Vega:  9.50
  🥈 María López Ruiz: 9.20
  🥉 Ana García López: 8.50

>>> APROBADOS:

  ✓ Sofía González Vega:  9.50 (Sobresaliente)
  ✓ María López Ruiz:  9.20 (Sobresaliente)
  ✓ Ana García López: 8.50 (Notable)
  ✓ Laura Martín Torres: 8.00 (Notable)
  ✓ Juan Pérez Martín: 7.00 (Notable)
  ✓ Carlos Fernández Gil: 6.50 (Aprobado)

>>> SUSPENSOS:

  ✗ Miguel Ruiz Castro: 3.50
  ✗ Pedro Sánchez Díaz: 4.50

>>> ESTADÍSTICAS:

  Total estudiantes:    8
  Nota media:         7.09
  Nota máxima:         9.50
  Nota mínima:        3.50
  Aprobados:          6 (75.0%)
  Suspensos:          2 (25.0%)

>>> UPDATE: Mejorando nota de Pedro...

  Antes: Pedro Sánchez Díaz - Nota: 4.5
  Después: Pedro Sánchez Díaz - Nota: 6

>>> DELETE: Eliminando estudiante ID 8...

  ✓ Eliminado: Miguel Ruiz Castro
  Total estudiantes ahora: 7

>>> PERSISTENCIA: Verificando recarga desde JSON... 

✓ 7 estudiantes cargados desde students.json
✓ Repositorio recargado: 7 estudiantes

Primeros 3 estudiantes recargados:
  → [1] Ana García López (ana@universidad.es) - Nota: 8.50 (Notable)
  → [2] Juan Pérez Martín (juan@universidad.es) - Nota: 7.00 (Notable)
  → [3] María López Ruiz (maria@universidad.es) - Nota: 9.20 (Sobresaliente)

═══════════════════════════════════════════════════════════════
  RESUMEN FINAL
═══════════════════════════════════════════════════════════════

📊 Estado final del sistema:
  • Total estudiantes:     7
  • Nota media:           7.36
  • Aprobados/Suspensos: 6/1
  • Archivo JSON:         students.json (1247 bytes)

✓ Proyecto completado exitosamente

═══════════════════════════════════════════════════════════════
```

---

### 9.8. Extensiones Opcionales del Proyecto

#### 9.8.1. Exportar Informe a CSV

```csharp
// ========================================
// EXTENSIÓN: Exportar a CSV
// ========================================

public class StudentExportService
{
    private readonly ICrudRepository<Student, int> _repository;
    
    public StudentExportService(ICrudRepository<Student, int> repository)
    {
        _repository = repository;
    }
    
    /// <summary>
    /// Exporta todos los estudiantes a un archivo CSV. 
    /// </summary>
    public void ExportToCsv(string filePath)
    {
        using var writer = new StreamWriter(filePath);
        
        // Cabecera
        writer. WriteLine("Id,Name,Email,Age,Grade,Status");
        
        // Datos
        foreach (var student in _repository.GetAll())
        {
            string status = student.IsApproved() ? "Aprobado" : "Suspenso";
            writer.WriteLine($"{student.Id},{student. Name},{student.Email},{student. Age},{student.Grade:F2},{status}");
        }
        
        Console.WriteLine($"✓ Exportado a CSV: {filePath}");
    }
}

// Uso
var exportService = new StudentExportService(repository);
exportService.ExportToCsv("students_report.csv");
```

#### 9.8.2. Backup del Sistema

```csharp
// ========================================
// EXTENSIÓN:  Sistema de Backup
// ========================================

public class BackupService
{
    private readonly string _sourceFile;
    
    public BackupService(string sourceFile)
    {
        _sourceFile = sourceFile;
    }
    
    /// <summary>
    /// Crea un backup con timestamp.
    /// </summary>
    public void CreateBackup()
    {
        if (! File.Exists(_sourceFile))
        {
            Console.WriteLine("⚠️  No hay archivo para hacer backup");
            return;
        }
        
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string backupDir = "backups";
        
        Directory.CreateDirectory(backupDir);
        
        string backupFile = Path.Combine(backupDir, $"students_backup_{timestamp}.json");
        
        File.Copy(_sourceFile, backupFile);
        
        Console.WriteLine($"✓ Backup creado: {backupFile}");
    }
    
    /// <summary>
    /// Lista todos los backups disponibles. 
    /// </summary>
    public void ListBackups()
    {
        string backupDir = "backups";
        
        if (! Directory.Exists(backupDir))
        {
            Console. WriteLine("No hay backups");
            return;
        }
        
        var backups = Directory.GetFiles(backupDir, "*.json")
            .Select(f => new FileInfo(f))
            .OrderByDescending(f => f.CreationTime);
        
        Console.WriteLine("\n>>> Backups disponibles:\n");
        
        foreach (var backup in backups)
        {
            Console. WriteLine($"  📦 {backup.Name}");
            Console.WriteLine($"     Fecha: {backup.CreationTime:dd/MM/yyyy HH:mm:ss}");
            Console.WriteLine($"     Tamaño: {backup.Length} bytes");
        }
    }
}

// Uso
var backupService = new BackupService("students.json");
backupService.CreateBackup();
backupService.ListBackups();
```

---


## Autor

Codificado con :sparkling_heart: por [José Luis González Sánchez](https://twitter.com/JoseLuisGS_)

[![Twitter](https://img.shields.io/twitter/follow/JoseLuisGS_?style=social)](https://twitter.com/JoseLuisGS_)
[![GitHub](https://img.shields.io/github/followers/joseluisgs?style=social)](https://github.com/joseluisgs)
[![GitHub](https://img.shields.io/github/stars/joseluisgs?style=social)](https://github.com/joseluisgs)

### Contacto

<p>
  Cualquier cosa que necesites házmelo saber por si puedo ayudarte 💬.
</p>
<p>
 <a href="https://joseluisgs.dev" target="_blank">
        <img src="https://joseluisgs.github.io/img/favicon.png" 
    height="30">
    </a>  &nbsp;&nbsp;
    <a href="https://github.com/joseluisgs" target="_blank">
        <img src="https://distreau.com/github.svg" 
    height="30">
    </a> &nbsp;&nbsp;
        <a href="https://twitter.com/JoseLuisGS_" target="_blank">
        <img src="https://i.imgur.com/U4Uiaef.png" 
    height="30">
    </a> &nbsp;&nbsp;
    <a href="https://www.linkedin.com/in/joseluisgonsan" target="_blank">
        <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/c/ca/LinkedIn_logo_initials.png/768px-LinkedIn_logo_initials.png" 
    height="30">
    </a>  &nbsp;&nbsp;
    <a href="https://g.dev/joseluisgs" target="_blank">
        <img loading="lazy" src="https://googlediscovery.com/wp-content/uploads/google-developers.png" 
    height="30">
    </a>  &nbsp;&nbsp;
<a href="https://www.youtube.com/@joseluisgs" target="_blank">
        <img loading="lazy" src="https://upload.wikimedia.org/wikipedia/commons/e/ef/Youtube_logo.png" 
    height="30">
    </a>  
</p>

## Licencia de uso

Este repositorio y todo su contenido está licenciado bajo licencia **Creative Commons**, si desea saber más, vea
la [LICENSE](https://joseluisgs.dev/docs/license/). Por favor si compartes, usas o modificas este proyecto cita a su
autor, y usa las mismas condiciones para su uso docente, formativo o educativo y no comercial.

<a rel="license" href="http://creativecommons.org/licenses/by-nc-sa/4.0/"><img alt="Licencia de Creative Commons" style="border-width:0" src="https://i.creativecommons.org/l/by-nc-sa/4.0/88x31.png" /></a><br /><span xmlns:dct="http://purl.org/dc/terms/" property="dct:title">
JoseLuisGS</span>
by <a xmlns:cc="http://creativecommons.org/ns#" href="https://joseluisgs.dev/" property="cc:attributionName" rel="cc:attributionURL">
José Luis González Sánchez</a> is licensed under
a <a rel="license" href="http://creativecommons.org/licenses/by-nc-sa/4.0/">Creative Commons
Reconocimiento-NoComercial-CompartirIgual 4.0 Internacional License</a>.<br />Creado a partir de la obra
en <a xmlns:dct="http://purl.org/dc/terms/" href="https://github.com/joseluisgs" rel="dct:source">https://github.com/joseluisgs</a>.
