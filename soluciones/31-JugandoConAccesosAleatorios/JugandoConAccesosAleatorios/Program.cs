using System.Text;

Console.WriteLine("¡Bienvenido al programa de acceso aleatorio!");

// --- 1. INICIALIZACIÓN (Previene el DirectoryNotFoundException) ---
var dataPath = Path.Combine(Directory.GetCurrentDirectory(), "data");
Directory.CreateDirectory(dataPath); // Si no existe, lo crea. Si existe, no hace nada.

// (Opcional) Si quieres que enteros.dat tenga algo la primera vez que lo corres:
CrearEnterosDePruebaSiNoExisten(Path.Combine(dataPath, "enteros.dat"));
CrearTextoDePruebaSiNoExiste(Path.Combine(dataPath, "texto.txt"));

// --- 2. ORDEN DE LLAMADAS ---
//MostrarFicheroAleatorio();
//ModificarFicheroAleatorio();
//MostrarFicheroAleatorio();
LeerFicheroAleatorioTexto();
Palabras();
LeerFicheroAleatorioTexto();


// --- Funciones (Locales al Top-Level) ---

void Palabras() {
    var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "texto.txt");

    Console.WriteLine("Introduce una palabra: ");
    var palabra = Console.ReadLine() ?? throw new Exception("Palabra no válida");
    Console.WriteLine(palabra);
    
    var resultado = new StringBuilder();

    using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite)) {

        using var reader = new StreamReader(fs, Encoding.UTF8, leaveOpen: true);
       

        // Lee línea por línea, reemplaza la palabra y guarda el resultado en un StringBuilder
        while (reader.ReadLine() is { } cadena) {
            var sustituido = cadena.Replace(palabra, palabra.ToUpper());
            resultado.AppendLine(sustituido);

            Console.Error.WriteLine($"Posición actual del stream: {fs.Position}");
        }

        fs.SetLength(0); // Elimina el contenido anterior para escribir el nuevo resultado
        fs.Position = 0; // Posiciona el stream al principio para escribir
    }

    using (var sw = new StreamWriter(path, false, Encoding.UTF8)) {
        sw.Write(resultado.ToString());
        Console.WriteLine("Archivo modificado correctamente");
    }
}

void ModificarFicheroAleatorio() {
    var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "enteros.dat");
    // Cambiado a OpenOrCreate para evitar FileNotFoundException si la carpeta estaba vacía
    using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    using var reader = new BinaryReader(fs);
    using var writer = new BinaryWriter(fs);

    var longitud = fs.Length;
    var numEnteros = longitud / 4; // Cálculo de número de enteros

    if (numEnteros == 0) {
        Console.WriteLine("El fichero está vacío. No hay enteros para modificar.");
        return;
    }

    Console.WriteLine($"Numero de entradas: {numEnteros}");
    Console.WriteLine($"El entero a modificar [1-{numEnteros}]: ");
    if (!int.TryParse(Console.ReadLine(), out var entero) || entero < 1 || entero > numEnteros) {
        Console.WriteLine("El entero no es correcto");
        return;
    }

    fs.Position =
        (entero - 1) * 4L; // Posiciona el stream al inicio del entero seleccionado (cada entero ocupa 4 bytes)
    var valor = reader.ReadInt32(); // Lee el entero de la posición actual
    Console.WriteLine($"El valor del entero es: {valor}");

    Console.WriteLine("Nuevo valor: ");
    var nuevoValor = int.TryParse(Console.ReadLine(), out var nv) ? nv : 0;

    fs.Position = (entero - 1) * 4L; // Posiciona el stream de nuevo al inicio del entero para escribir el nuevo valor
    writer.Write(nuevoValor); // Escribe el nuevo valor en la posición del entero seleccionado
}

void MostrarFicheroAleatorio() {
    var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "enteros.dat");
    // Cambiado a OpenOrCreate
    using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
    using var reader = new BinaryReader(fs);

    var longitud = fs.Length; // Obtiene la longitud del fichero en bytes
    Console.WriteLine($"Longitud del fichero: {longitud}");

    var numEnteros = longitud / 4; // Cada entero ocupa 4 bytes
    for (var i = 0; i < numEnteros; i++) {
        var entero = reader.ReadInt32(); // Lee el entero de la posición actual
        Console.WriteLine($"Entero: {entero} - Posición: {fs.Position}");
    }
}

void LeerFicheroAleatorioTexto() {
    var path = Path.Combine(Directory.GetCurrentDirectory(), "data", "texto.txt");
    // Cambiado a OpenOrCreate
    using var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);

    // Diferentes formas de leer el fichero de texto, todas respetando la posición del stream y sin cerrarlo prematuramente
    using (var reader = new StreamReader(fs, Encoding.UTF8, leaveOpen: true)) {
        // Lee línea por línea hasta el final del stream
        string? linea;
        do {
            linea = reader.ReadLine();
            if (linea != null) Console.WriteLine(linea);
        } while (linea != null);
    }

    Console.WriteLine();
    fs.Position = 0; // Resetea la posición del stream al principio para la siguiente lectura

    using (var reader = new StreamReader(fs, Encoding.UTF8, leaveOpen: true)) {
        // Lee todo el contenido del stream en una cadena
        while (!reader.EndOfStream)
            Console.WriteLine(reader.ReadLine());
    }

    Console.WriteLine();
    fs.Position = 0; // Resetea la posición del stream al principio para la siguiente lectura

    var bytes = new byte[fs.Length];
    // Lee exactamente el número de bytes que tiene el stream, respetando la posición actual
    fs.ReadExactly(bytes, 0, bytes.Length);
    Console.WriteLine(Encoding.UTF8.GetString(bytes));
    Console.WriteLine();

    fs.Position = 0; // Resetea la posición del stream al principio para la siguiente lectura
    // Lee byte por byte hasta el final del stream, construyendo líneas manualmente
    while (fs.Position < fs.Length) {
        var lineBytes = new List<byte>();
        int currentByte;
        // Lee byte por byte hasta encontrar un salto de línea o el final del stream
        while (fs.Position < fs.Length) {
            currentByte = fs.ReadByte();
            if (currentByte == -1 || currentByte == '\n') break;
            if (currentByte != '\r') lineBytes.Add((byte)currentByte);
        }

        var line = Encoding.UTF8.GetString([.. lineBytes]);
        Console.WriteLine(line);
    }
}

// --- Funciones auxiliares para sembrar datos si el directorio estaba limpio ---
void CrearEnterosDePruebaSiNoExisten(string path) {
    if (!File.Exists(path)) {
        using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
        using var bw = new BinaryWriter(fs);
        bw.Write(10);
        bw.Write(20);
        bw.Write(30);
        bw.Write(40);
    }
}

void CrearTextoDePruebaSiNoExiste(string path) {
    if (!File.Exists(path))
        File.WriteAllText(path, "Hola mundo\nEsta es una prueba para el fichero\nBuscando palabras\n", Encoding.UTF8);
}