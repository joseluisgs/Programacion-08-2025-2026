// See https://aka.ms/new-console-template for more information

using System.Text;

Console.WriteLine("Hola Ficheros Texto con File");

var path = "texto.txt";
EscribirArchivo(path);
LeerArchivo(path);
AñadirArchivo(path);
LeerArchivo(path);
ExperimentoBorrarLinea(path);
LeerArchivo(path);


void EscribirArchivo(string fichero) {
    // Si no existe el archivo, lo crea. Si existe, lo sobrescribe
    // Si hay salto de linea usar Environment.NewLine
    // para que sea compatible con cualquier sistema operativo
    
    // Escribe todo el texto de una sola vez
    // Cuidado si el archivo es muy grande, puede consumir mucha memoria
    File.WriteAllText(fichero, "Hola Mundo" + Environment.NewLine + "1DAW aprende C#");
    
    string [] lineas = [
        "Hola Mundo",
        "1DAW aprende C#",
        "Linea 1",
        "Linea 2",
        "Linea 3"
    ];
    
    // Escribe cada línea individualmente
    // Este método consume más memoria que WriteAllText
    // pero es útil para escribir archivos con muchas líneas
    File.WriteAllLines(fichero, lineas);
}

void LeerArchivo(string fichero) {
    
    // Lee todo el texto de una sola vez
    // Cuidado si el archivo es muy grande, puede consumir mucha memoria
    string contenido = File.ReadAllText(fichero);
    Console.WriteLine($"Contenido del archivo:");
    Console.WriteLine(contenido);
    
    // Lee cada línea individualmente
    // Obtenemos un array de strings con cada línea del archivo
    // Este método consume más memoria que ReadAllText
    // pero es útil para procesar archivos con muchas líneas
    string[] lineas = File.ReadAllLines(fichero);
    Console.WriteLine("Líneas del archivo:");
    foreach (var linea in lineas) {
        Console.WriteLine(linea);
    }
    Console.WriteLine();
    
    // Lee el contenido del archivo y lo almacena en un IEnumerable<string>
    // para procesarlo línea por línea
    // Este método es más eficiente que ReadAllLines
    // pero consume más memoria que ReadAllText
    Console.WriteLine("Líneas del archivo (con IEnumerable):");
    File.ReadLines(fichero)
        .ToList()
        .ForEach(Console.WriteLine);
}

void AñadirArchivo(string fichero) {
    // Añade texto al final del archivo
    // Si el archivo no existe, lo crea
    File.AppendAllText(fichero, "Texto adicional" + Environment.NewLine);
    
    // Añade una línea al final del archivo
    File.AppendAllLines(fichero, 
        [
            "Línea adicional", 
            "Otra línea adicional"
        ]);
}

void ExperimentoBorrarLinea(string fichero) {
    // Para borrar una línea específica, debemos leer el archivo, eliminar la línea deseada y escribir el archivo de nuevo
    var lineas = File.ReadAllLines(fichero).ToList();
    
    // Supongamos que queremos borrar la línea "Linea 2"
    var res = lineas.Where(linea => !linea.Contains("DAW", StringComparison.CurrentCultureIgnoreCase));
    
    // Escribimos el archivo de nuevo sin la línea eliminada
    File.WriteAllLines(fichero, res);
}
