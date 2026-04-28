// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hola Ficheros en C#!");

const string Ruta = "datos.bin";

EscribirFicheroBinario(Ruta);
LeerFicheroBinario(Ruta);
BorrarFichero(Ruta);

void EscribirFicheroBinario(string ruta) {
// Escribir tipos primitivos
    using var writer = new BinaryWriter(File.Create(ruta));

    writer.Write(42); // int
    Console.WriteLine("✓ Entero escrito: 42");
    writer.Write(3.14); // double
    Console.WriteLine("✓ Double escrito: 3.14");
    writer.Write("Hola mundo"); // string
    Console.WriteLine("✓ String escrito: 'Hola mundo'");
    writer.Write(true); // bool
    Console.WriteLine("✓ Booleano escrito: true");

    Console.WriteLine("✓ Binario escrito");

// Ver contenido (será ilegible)
    Console.WriteLine($"Tamaño: {new FileInfo(ruta).Length} bytes");
}

void LeerFicheroBinario(string ruta) {
    using var reader = new BinaryReader(File.OpenRead(ruta));

// Lo importante es leer en el mismo orden que se escribió
// Ademas decir el tipo de dato que se espera leer,
// sino se obtendrán resultados erróneos o excepciones
    var numero = reader.ReadInt32();
    Console.WriteLine($"int: {numero}");
    var decimals = reader.ReadDouble();
    Console.WriteLine($"double: {decimals}");
    var texto = reader.ReadString();
    Console.WriteLine($"string: {texto}");
    var booleano = reader.ReadBoolean();
    Console.WriteLine($"bool: {booleano}");
  
}

void BorrarFichero(string ruta) {
    if (File.Exists(ruta)) {
        File.Delete(ruta);
        Console.WriteLine("✓ Fichero borrado");
    }
    else {
        Console.WriteLine("El fichero no existe");
    }
}