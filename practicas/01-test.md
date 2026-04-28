# Test: Ficheros, Streams y Formatos de Intercambio

- [Test: Ficheros, Streams y Formatos de Intercambio](#test-ficheros-streams-y-formatos-de-intercambio)
  - [Bloque 1: Fundamentos de I/O y Streams](#bloque-1-fundamentos-de-io-y-streams)
  - [Bloque 2: Sistema de Ficheros](#bloque-2-sistema-de-ficheros)
  - [Bloque 3: Ficheros de Texto](#bloque-3-ficheros-de-texto)
  - [Bloque 4: Formatos CSV y JSON](#bloque-4-formatos-csv-y-json)
  - [Bloque 5: Serialización y XML](#bloque-5-serialización-y-xml)
  - [Bloque 6: Recursos y using](#bloque-6-recursos-y-using)

#### Bloque 1: Fundamentos de I/O y Streams

1. **¿Qué es un Stream en C#?**
   a) Una clase para crear gráficos 3D.
   b) Una abstracción que representa una secuencia de datos que se procesa de forma continua.
   c) Un tipo de variable para almacenar texto.
   d) Un método para ordenar arrays.

2. **¿Por qué es importante usar Streams para ficheos grandes?**
   a) Porque son más fáciles de escribir que otros métodos.
   b) Porque permiten procesar los datos por partes sin cargar todo en memoria.
   c) Porque automáticamente encriptan los datos.
   d) Porque funcionan sin necesidad de permisos.

3. **¿Cuál es la principal ventaja de MemoryStream frente a FileStream?**
   a) Guarda los datos permanentemente.
   b) Es más rápido porque opera en RAM.
   c) No necesita dispose.
   d) Solo funciona con texto.

4. **¿Qué clases heredan de Stream en .NET?**
   a) Solo FileStream.
   b) FileStream, MemoryStream, NetworkStream, entre otras.
   c) StringStream y NumberStream.
   d) Ninguna, Stream es una interfaz.

#### Bloque 2: Sistema de Ficheros

5. **¿Cuál es la diferencia entre File y FileInfo?**
   a) File es para texto y FileInfo para binarios.
   b) File tiene métodos estáticos, FileInfo requiere una instancia.
   c) FileInfo es más rápido siempre.
   d) No hay diferencia, son iguales.

6. **¿Para qué sirve Path.Combine()?**
   a) Para comprimir ficheos.
   b) Para combinar rutas de forma segura independiente del SO.
   c) Para fusionar dos ficheos en uno.
   d) Para convertir texto a binario.

7. **¿Qué método usamos para verificar si un ficheo existe?**
   a) File.Check()
   b) File.Exists()
   c) File.Find()
   d) File.Open()

8. **¿Cómo podemos buscar ficheos con un patrón específico como "*.txt"?**
   a) File.GetFiles("*.txt")
   b) Directory.GetFiles("*.txt")
   c) File.Find("*.txt")
   d) Path.Search("*.txt")

#### Bloque 3: Ficheros de Texto

9. **¿Cuál es la diferencia entre StreamReader y StreamWriter?**
   a) StreamReader es para escribir, StreamWriter para leer.
   b) StreamReader lee texto, StreamWriter escribe texto.
   c) No hay diferencia, son iguales.
   d) StreamReader usa menos memoria.

10. **¿Qué método de StreamWriter fuerza la escritura inmediata al disco?**
    a) Save()
    b) Write()
    c) Flush()
    d) Commit()

11. **¿Cuál es la diferencia entre Write() y WriteLine()?**
    a) Write() es más rápido.
    b) WriteLine() añade un salto de línea al final.
    c) WriteLine() no funciona con StreamWriter.
    d) No hay diferencia.

12. **¿Qué codificación我们应该 usar para texto en español?**
    a) ASCII
    b) UTF-8
    c) Unicode-16
    d) Latin1

#### Bloque 4: Formatos CSV y JSON

13. **¿Qué significa CSV?**
    a) Computer System Virtual
    b) Comma Separated Values
    c) Central System Version
    d) Code Standard Variable

14. **¿Cuál es una ventaja de JSON sobre CSV?**
    a) JSON es más antiguo.
    b) JSON soporta estructuras jerárquicas y objetos anidados.
    c) JSON solo funciona con números.
    d) JSON no puede almacenar texto.

15. **¿Qué método de System.Text.Json usamos para serializar un objeto?**
    a) JsonConverter.Serialize()
    b) JsonSerializer.Serialize()
    c) JSON.Stringify()
    d) JsonConvert.ToJson()

16. **¿Qué significa serializar un objeto?**
    a) Eliminar un objeto de la memoria.
    b) Convertir un objeto a un formato que pueda almacenarse o transmitirse.
    c) Copiar un objeto.
    d) Crear un nuevo objeto.

#### Bloque 5: Serialización y XML

17. **¿Qué clase se usa en C# para serializar a XML?**
    a) XmlWriter
    b) XmlSerializer
    c) DataContractSerializer
    d) XmlFormatter

18. **¿Por qué XML ha perdido popularidad frente a JSON?**
    a) XML es más rápido.
    b) JSON es más ligero y legible.
    c) XML no soporta texto.
    d) XML solo funciona en Windows.

19. **¿Qué formato es mejor para APIs REST modernas?**
    a) XML
    b) JSON
    c) CSV
    d) Binario

20. **¿Qué formato es mejor para intercambio con Excel?**
    a) JSON
    b) XML
    c) CSV
    d) Binario

#### Bloque 6: Recursos y using

21. **¿Por qué es importante cerrar los recursos como ficheos?**
    a) Para liberar memoria del programa.
    b) Para liberar el candado (lock) del sistema operativo.
    c) Para hacer el código más largo.
    d) No es importante, el sistema lo hace solo.

22. **¿Qué interfaz implementan las clases que necesitan liberarse manualmente?**
    a) IClosable
    b) IDisposable
    c) IResource
    d) IReleaseable

23. **¿Cuál es la forma moderna recomendada (C# 8+) de usar IDisposable?**
    a) try-finally manual.
    b) using (var x = new...) { }
    c) using var x = new...;
    d) new using var x = ...

24. **¿Qué hace automáticamente la palabra clave "using"?**
    a) Abre el recurso.
    b) Libera el recurso automáticamente al final del scope.
    c) Comprime el ficheo.
    d) Convierte el texto a binario.

25. **¿Qué problema evita el uso de "using"?**
    a) Los bucles infinitos.
    b) Las fugas de recursos (resource leaks).
    c) Los errores de sintaxis.
    d) Los tipos nulos.
