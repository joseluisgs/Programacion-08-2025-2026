# Repositorio Binario - Apuntes de Serialización Binaria

## 📚 Introducción

En este proyecto exploramos tres técnicas diferentes de persistencia binaria en C# para gestionar una colección de personas. Cada técnica tiene sus ventajas e inconvenientes.

---

## 🗂️ Las Tres Técnicas

### 1️⃣ Repositorio Secuencial (`PersonasSecuencialRepository`)

**Concepto:** Escribe y lee todos los datos secuencialmente en un único archivo, campo por campo.

**Archivos generados:** `personas_secuencial.dat`

```
[cantidad][nextId][persona1][persona2]...[personaN]
```

**Cómo funciona:**
- Al guardar: escribir todo el diccionario de personas de una sola vez
- Al cargar: leer todo el archivo y reconstruir el diccionario en memoria

```csharp
// Save() - Escribe todo el diccionario
using var stream = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
using var writer = new BinaryWriter(stream);
writer.Write(_personas.Count);
foreach (var persona in _personas.Values) {
    writer.Write(persona.Id);
    writer.Write(persona.Nombre);
    writer.Write(persona.Edad);
    writer.Write(persona.Email);
}
```

---

### 2️⃣ Repositorio Serial (`PersonasSerialRepository`) ⚠️ OBSOLETO

**Concepto:** Usa `BinaryFormatter` de .NET para serializar objetos automáticamente.

**Archivos generados:** `personas_serial.dat`

**PROS:**
- Código muy simple (pocas líneas)
- No requiere escribir código de serialización/deserialización manual
- Funciona automáticamente si cambias el modelo

**CONTRAS:**
- ❌ **OBSOLETO desde .NET 5** por problemas de seguridad
- Solo funciona en .NET (no portable a Kotlin, Java, Python, etc.)
- Formato propietario de Microsoft
- Vulnerable a ataques de deserialización

> ⚠️ **Este repositorio está deshabilitado en el proyecto actual.**

---

### 3️⃣ Repositorio de Acceso Aleatorio (`PersonasRandomAccessRepository`) ⭐ RECOMENDADO

**Concepto:** Utiliza tres archivos separados para lograr acceso directo O(1) y gestión eficiente de espacio.

**Archivos generados:**
- `personas.dat` - Datos (heap con registros de tamaño variable)
- `personas.idx` - Índice (ID → posición, longitud)
- `personas.frx` - Huecos libres (free list)

### 🎯 ¿Por qué guardamos la posición (offset)?

Imagina el archivo de datos como una regla numerada:

```
Bytes:   0   5   15  30  45  60  75  90  105...
         ┌───┬───┬───┬───┬───┬───┬───┬───┐
Datos:   │ P1│ P2│ P3│ P4│ P5│ P6│ P7│ P8│
         └───┴───┴───┴───┴───┴───┴───┴───┘
```

Cada persona ocupa un espacio (bytes). El **offset** es la posición exacta donde empieza cada registro.

**¿Qué es el offset?**
- Es un número que indica la posición en bytes desde el inicio del archivo
- Ejemplo: Si Persona 3 empieza en el byte 30, su offset = 30
- Es lo que se conoce como "puntero" o "dirección" en memoria

**¿Para qué sirve?**
- Permite acceder directamente a cualquier registro sin leer los anteriores
- Es como tener un índice de un libro: vas directamente a la página que buscas
- Sin offset, tendrías que leer desde el principio hasta encontrarlo (lectura secuencial O(n))

**Estructura del índice:**
```csharp
Dictionary<int, (long offset, int length)> _indice;
// {1: (0, 150), 2: (150, 145), 3: (295, 160)...}
```

**Algoritmo de búsqueda por ID:**
1. Consultar índice → obtener offset y longitud
2. `Seek(offset)` → ir directamente a esa posición en el archivo
3. Leer `longitud` bytes → deserialize

---

### 🔄 Lista de Huecos Libres (Free List)

**Concepto:** Cuando borramos un registro, no eliminamos los bytes del archivo. Marcamos ese espacio como "libre" para reutilizarlo.

**Problema sin huecos:**
- Tienes un archivo de 1GB
- Borras 500MB de registros (la mitad)
- El archivo sigue ocupando 1GB (espacio desperdiciado)

**Solución con huecos:**
- Al borrar, añadimos la posición y tamaño a una lista
- Al crear un nuevo registro, primero miramos si hay un hueco del tamaño adecuado
- Si hay un hueco, escribimos ahí en lugar de al final

**Estructura de huecos:**
```csharp
List<(long posicion, int longitud)> _huecos;
// [(500, 150), (2000, 100), (3500, 200)...]
```

**Ejemplo visual:**

```
Estado inicial:     [P1][P2][P3][P4][P5]
Borramos P2 y P4:  [P1][  ][P3][  ][P5]
                     ↑──┬──↑     ↑──┬──↑
                    huecos de P2 y P4

Al crear P6 (tamaño 30): busca hueco → encuentra el de P2 (40) → reutiliza
```

**Ventajas:**
- No se desperdicia espacio
- Las escrituras son más rápidas (a veces)
- El archivo no crece descontroladamente

---

### 🧹 Defragmentación (Compactación)

**Problema:** Con el tiempo, pueden acumularse muchos huecos pequeños que no se pueden reutilizar.

```
Archivo con muchos huecos:
[P1][---hueco 10---][P2][----hueco 50----][P3][hueco 5][P4]
                           ↑ Los nuevos registros no caben aquí
```

Si un nuevo registro necesita 30 bytes:
- El hueco de 10 ❌ (muy pequeño)
- El hueco de 50 ✅ (perfecto)
- El hueco de 5 ❌ (muy pequeño)

**Solución: Compactación**
Cuando los huecos superan el 30% del tamaño total del archivo, se ejecuta:

1. **Leer todos los registros** que aún existen
2. **Crear un archivo nuevo** vacío
3. **Escribir todos los registros secuencialmente** (sin huecos)
4. **Reconstruir el índice** con las nuevas posiciones
5. **Vaciar la lista de huecos**

**Resultado:**
```
Antes: [P1][hueco][P2][hueco][P3][hueco][hueco][P4]  (500KB, 200KB en huecos = 40%)
Después: [P1][P2][P3][P4]                                 (300KB, 0 huecos)
```

---

## ⚠️ Errores Comunes a Evitar

### 1. No verificar si el archivo existe
```csharp
// ❌ Mal: asume que el archivo siempre existe
var datos = File.ReadAllBytes("archivo.dat");

// ✅ Bien: verificar primero
if (File.Exists("archivo.dat")) {
    // leer archivo
}
```

### 2. Olvidar cerrar los archivos
```csharp
// ❌ Mal: no se cierra el archivo
var stream = new FileStream("archivo.dat", FileMode.Open);
// al salir del método, el archivo queda abierto

// ✅ Bien: usar 'using' (se cierra automáticamente)
using var stream = new FileStream("archivo.dat", FileMode.Open);
// al salir del bloque, se cierra solo
```

### 3. No manejar excepciones en operaciones de archivo
```csharp
// ❌ Mal: si falla, el programa crashea
var datos = File.ReadAllBytes("archivo.dat");

// ✅ Bien: capturar la excepción
try {
    var datos = File.ReadAllBytes("archivo.dat");
}
catch (Exception ex) {
    Console.WriteLine($"Error al leer: {ex.Message}");
}
```

### 4. Confundir lectura con escritura
```csharp
// ❌ Mal: FileMode.Open con FileAccess.Write da error
using var stream = new FileStream("archivo.dat", FileMode.Open, FileAccess.Write);

// ✅ Bien: usar el modo correcto
using var streamLectura = new FileStream("archivo.dat", FileMode.Open, FileAccess.Read);
using var streamEscritura = new FileStream("archivo.dat", FileMode.Create, FileAccess.Write);
```

### 5. No sincronizar el índice con los datos
En Random Access, si guardas los datos pero no el índice, pierdes el acceso a los registros.

### 6. Escribir en una posición incorrecta
Olvidar usar `Seek()` antes de escribir causa que los datos se escriban en el lugar wrong.

---

## 💾 ¿Qué pasa si el programa crashea? ¿Se pierden datos?

### Escenario: Crashe durante una operación de escritura

**Si usas `using` (recomendado):**
- El archivo se cierra correctamente al salir del bloque
- Si crashea dentro del bloque, los datos pueden estar incompletos
- Pero el archivo no queda corrupto

**Si NO usas `using` (peligroso):**
- El archivo queda abierto
- Puedes perder datos o dejar el archivo corrupto

### Conclusión según el tipo de repositorio:

| Repositorio | Riesgo de pérdida | Qué se pierde si crashea |
|-------------|-------------------|---------------------------|
| **Secuencial** | Bajo | Todo el archivo si estaba escribiendo |
| **Serial** | Medio | Todo el diccionario |
| **Random Access** | Bajo | Solo el registro que se estaba escribiendo |

### Cómo minimizar el riesgo:

1. **Usar siempre `using`** - cierra archivos correctamente
2. **Escritura atómica** - escribir en archivo temporal, luego renombrar
3. **Hacer backup** - copiar el archivo antes de modificarlo
4. **Logs** - guardar qué operación estaba haciendo

### En Random Access específico:
- El índice y los datos están separados
- Si crashea al escribir datos, el índice puede quedar inconexo
- Al reiniciar, el repositorio carga lo que pueda (tolerante a errores)

---

## 🏢 Ejemplo Real: ¿Cuándo usar uno u otro en un proyecto?

### Ejemplo 1: Agenda de contactos personal (100-500 contactos)
```
Necesidades: Guardar contactos, buscar por nombre, editar, borrar
Usuario: Una persona
Riesgo de pérdida: Bajo

→ REPOSITORIO SECUENCIAL ✅
- Código simple, fácil de mantener
- Con 500 registros, es rápido enough
- No necesitas rendimiento extremo
```

### Ejemplo 2: Sistema de facturación (10.000-100.000 facturas)
```
Necesidades: Buscar facturas por número, fecha, cliente
Accesos: Muchos por día, principalmente lecturas
Riesgo de pérdida: Medio

→ REPOSITORIO RANDOM ACCESS ⭐
- Acceso directo por número de factura (O(1))
- Muchas actualizaciones de facturas
- Necesitas eficiencia
```

### Ejemplo 3: Base de datos de empleados (1.000.000+ registros)
```
Necesidades: Búsquedas complejas, filtros, estadísticas
Usuario: Empresa grande
Riesgo de pérdida: Alto

→ Base de datos real (SQL Server, PostgreSQL) o Random Access muy tuneado
- Los repositorios aquí se quedan cortos
- Necesitas índices complejos, transacciones, etc.
```

### Ejemplo 4: Caché de sesión de usuario
```
Necesidades: Guardar datos temporales de sesión
Tiempo de vida: Horas o días
Riesgo de pérdida: Bajo (se puede regenerar)

→ REPOSITORIO SECUENCIAL o en memoria
- Los datos no son críticos
- Simplicidad > eficiencia
```

### Resumen Visual:

```
┌─────────────────────────────────────────────────────────────┐
│                    ¿QUIÉN SOY?                              │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│   ¿Tengo menos de 1.000 registros?                          │
│   └─ SÍ → Secuencial                                        │
│   └─ NO → ¿Tengo muchas lecturas y pocas escrituras?        │
│            └─ SÍ → ¿Necesito buscar por ID frecuentemente?  │
│                  └─ SÍ → Random Access ⭐                   │
│                  └─ NO → Secuencial                         │
│            └─ NO → ¿Tengo muchas actualizaciones?           │
│                  └─ SÍ → Random Access ⭐                   │
│                  └─ NO → Secuencial                         │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## 📊 Comparación Técnica

| Aspecto | Secuencial | Serial | Random Access |
|---------|-------------|--------|---------------|
| **Complejidad código** | Media | Baja | Alta |
| **Portabilidad** | ✅ Alta | ❌ No | ✅ Alta |
| **Acceso por ID** | O(n) | O(n) | **O(1)** ⭐ |
| **Actualizaciones** | Escribe todo | Escribe todo | Solo lo necesario |
| **Espacio** | Fijo | Fijo | **Variable** ⭐ |
| **Gestión huecos** | ❌ No | ❌ No | **✅ Sí** ⭐ |
| **Memoria necesaria** | Alta | Alta | **Baja** ⭐ |
| **Seguridad** | ✅ Segura | ❌ Peligrosa | ✅ Segura |
| **Mantenimiento** | Fácil | Muy fácil | Medio |

---

## 🎯 ¿Cuándo usar cada una?

### ✅ Usa el Repositorio Secuencial cuando:
- Tienes pocos registros (< 1.000)
- Las operaciones son mayormente lectura
- No necesitas acceso aleatorio frecuente
- La simplicidad es prioritaria
- Ejemplos: configuraciones, cachés pequeños

### ❌ No uses el Repositorio Serial:
-Está obsoleto desde .NET 5
- Tiene vulnerabilidades de seguridad
- Solo funciona en .NET

### ✅ Usa el Repositorio Random Access cuando:
- Tienes muchos registros (> 10.000)
- Necesitas acceso rápido por ID
- Tienes muchas actualizaciones
- Necesitas optimizar escritura (no reescribir todo)
- Necesitas gestionar espacio eficientemente
- Es un sistema de producción

---

## 🏆 Conclusión: ¿Cuál es la mejor?

### Para **aprendizaje y ejercicios académicos** → Secuencial
- Más fácil de entender
- Código claro y legible
- Enseña los fundamentos

### Para **producción y aplicaciones reales** → Random Access ⭐
- Escalabilidad (funciona con millones de registros)
- Eficiencia (solo lee/escribe lo necesario)
- Acceso directo O(1)
- Gestión inteligente de espacio
- Portabilidad (legible desde cualquier lenguaje)

---

## 💡 Ventajas del Random Access para tu futuro profesional

1. **Escalabilidad infinita**: Con 1 millón de registros, las otras técnicas fallan
2. **Eficiencia**: Solo escribe los bytes necesarios, no todo el diccionario
3. **Acceso directo**: Buscar por ID es instantáneo, no遍历 todo el archivo
4. **Gestión de espacio**: Reutiliza huecos, compactación automática
5. **Patrón real**: Es como funcionan las bases de datos internamente

---

## 🔧 Detalles Técnicos del Random Access

### Formato del archivo de datos (`personas.dat`)
```
[registro1][registro2]...[registroN]
```
Cada registro: `nombre(string) + edad(int) + email(string)` (tamaño variable)

### Formato del índice (`personas.idx`)
```
[nextId][cantidad][(id, offset, longitud)...]
```

### Formato de huecos (`personas.frx`)
```
[cantidad][(posicion, longitud)...]
```

### Compactación
- Se activa cuando los huecos superan el 30% del archivo
- Reescribe todos los registros secuencialmente
- Reconstruye el índice

---

## 🌐 Lectura desde otros lenguajes

Los archivos del Random Access son **legibles** desde:

| Lenguaje | Cómo leer |
|----------|-----------|
| **Kotlin** | `DataInputStream.readUTF()`, `readInt()` |
| **Java** | `DataInputStream` igual |
| **Python** | `struct.unpack()` o lectura directa |
| **C/C++** | Lectura directa de tipos primitivos |

---

## 📁 Estructura del Proyecto

```
RepositorioBinario/
├── RepositorioBinario.slnx
├── RepositorioBinario/
│   ├── Program.cs                    # Pruebas CRUD
│   ├── Models/
│   │   └── Persona.cs                # Modelo dato
│   └── Repositories/
│       ├── IPersonasRepository.cs     # Interfaz
│       ├── Common/
│       │   └── ICrudRepository.cs    # Contrato genérico
│       ├── PersonasSecuencialRepository.cs
│       ├── PersonasSerialRepository.cs    # Deshabilitado
│       └── PersonasRandomAccessRepository.cs
```

---

## 🚀 Ejecutar las pruebas

```bash
cd RepositorioBinario
dotnet run
```

Verás pruebas completas de:
- Create (10 personas)
- Read (todas + por ID)
- Update (modificar registros)
- Delete (eliminar registros)

---

*Apuntes creados para el módulo de Programación de DAW*