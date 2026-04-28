// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hola Ficheros con Acceso Aleatorio!");

// Tamaño de un tipo de dato:
// int: 4 bytes (32 bits)
// long: 8 bytes (64 bits)
// float: 4 bytes (32 bits)
// double: 8 bytes (64 bits)
// char: 2 bytes (Unicode) o 1 byte (ASCII)
// string: El tamaño puede variar según el número de caracteres y el tamaño de la codificación (Unicode o ASCII).
// Boolean: 1 byte (aunque en algunos casos puede ocupar más dependiendo de la implementación).
// Para calcular la posición de un dato en el archivo,
// debes multiplicar el índice del dato por el tamaño del tipo de dato en bytes.
// Por ejemplo, para acceder al n-ésimo entero, la posición sería n * 4 bytes.

var ruta = "numeros.bin";

// Escribir 10 enteros
using var fs = new FileStream(ruta, FileMode.Create);
for (var i = 0; i < 10; i++)
    // Escribir el entero i en el archivo
    // BitConverter.GetBytes(i) convierte el entero i a un array de bytes (4 bytes para un entero)
    fs.Write(BitConverter.GetBytes(i), 0, 4);

// Leer el 5º número (posición 4 * 4 = 16)
fs.Seek(16, SeekOrigin.Begin); // Posición del 5º número (4 bytes por entero)
var buffer = new byte[4]; // Leer 4 bytes (un entero) desde la posición actual
fs.ReadExactly(buffer, 0, 4);
var valor = BitConverter.ToInt32(buffer, 0);
Console.WriteLine($"Valor en posición 5: {valor}"); // 4

ruta = "numeros2.bin";

// Escribir y leer 10 enteros
EscribirNumeros(ruta);
LeerNumero(ruta, 4);


// Debes saberte cuantos bytes ocupa cada dato para poder calcular la posición correcta en el archivo. En este caso, cada entero ocupa 4 bytes, por lo que para acceder al n-ésimo número, debes ir a la posición n * 4 en el archivo.
// En este ejemplo, escribimos los números del 0 al 9 en el archivo, y luego leemos el número en la posición 5 (que es el número 4) utilizando Seek para movernos a la posición correcta antes de leer.

// También puedes usar BinaryReader y BinaryWriter para escribir y leer datos de un archivo binario más fácilmente.
void EscribirNumeros(string ruta) {
    using var bw = new BinaryWriter(File.Open(ruta, FileMode.Create));
    for (var i = 0; i < 10; i++)
        bw.Write(i); // Escribe el entero i en el archivo
}

void LeerNumero(string ruta, int posicion) {
    using var br = new BinaryReader(File.Open(ruta, FileMode.Open));
    br.BaseStream.Seek(posicion * 4,
        SeekOrigin.Begin); // Moverse a la posición correcta (posicion * 4 bytes por entero)
    var valor = br.ReadInt32(); // Leer el entero en la posición actual
    Console.WriteLine($"Valor en posición {posicion + 1}: {valor}");
}