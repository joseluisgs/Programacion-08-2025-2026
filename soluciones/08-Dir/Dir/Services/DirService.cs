using Dir.Configuration;

namespace Dir.Services;

public static class DirService {
    public static void Run() {
        var di = new DirectoryInfo(Config.DirectoryPath);

        Console.WriteLine($"\n    Directorio: {di.FullName}\n");

        // --- EL MOLDE ---
        // {0,-20} -> Mode (Alineado izquierda, 20 espacios)
        // {1,-17} -> LastWriteTime (Alineado izquierda, 17 espacios exactos que ocupa la fecha "30/08/2025  12:55")
        // {2,11}  -> Length (Alineado derecha, 11 posiciones. Así los números grandes crecen a la izquierda sin empujar)
        // {3}     -> Name (Separado por 1 espacio del Length, siempre empieza en la misma columna)
        var format = "{0,-20}{1,-17}{2,11} {3}";

        // 1. Pasamos los títulos por el molde
        Console.WriteLine(format, "Mode", "LastWriteTime", "Length", "Name");

        // 2. Pasamos los guiones por el molde (Esto arregla tu problema)
        Console.WriteLine(format, "----", "-------------", "------", "----");

        // 4. Ordenamos los items: primero los directorios, luego los archivos.
        // Dentro de cada grupo, ordenamos por nombre.
        var items = di.GetFileSystemInfos()
            .OrderByDescending(i => (i.Attributes & FileAttributes.Directory) != 0)
            .ThenBy(i => i.Name);

        // 5. Pasamos los items por el molde
        foreach (var item in items) {
            var isHidden = (item.Attributes & FileAttributes.Hidden) != 0;
            var isSystem = (item.Attributes & FileAttributes.System) != 0;

            // 6. Ignoramos los archivos ocultos o sistemas si el usuario ha activado la opción --force
            if (!Config.Force && (isHidden || isSystem)) continue;

            // 7. Calculamos el modo de acceso (d, -, r, h, s, l) para cada item    
            var mode = GetItemMode(item);

            // 8. Formateamos la fecha de última modificación para que ocupe exactamente 17 caracteres (dd/MM/yyyy  HH:mm)
            var lastWrite = item.LastWriteTime.ToString("dd/MM/yyyy  HH:mm");

            // 9. Calculamos el tamaño de cada archivo para que ocupe exactamente 11 caracteres (si es un archivo)
            // Si no es un archivo, el tamaño será vacío (""). 11 es el número de caracteres que se deben usar para este campo.
            var length = "";
            if (item is FileInfo fi) length = fi.Length.ToString();

            // 10. Pasamos cada item por el molde para que se alinee correctamente
            Console.WriteLine(format, mode, lastWrite, length, item.Name);
        }
    }

    private static string GetItemMode(FileSystemInfo item) {
        // Se crea un array de caracteres con 6 posiciones, cada una representando un atributo específico del item.
        char[] mode = { '-', '-', '-', '-', '-', '-' };
        var attr = item.Attributes;

        // Se verifica cada atributo del item utilizando operaciones bit a bit. Si el atributo está presente, se asigna el carácter correspondiente en la posición adecuada del array.
        if ((attr & FileAttributes.Directory) != 0) mode[0] = 'd'; // Directorio
        if ((attr & FileAttributes.Archive) != 0) mode[1] = 'a'; // Archivo
        if ((attr & FileAttributes.ReadOnly) != 0) mode[2] = 'r'; // Solo lectura
        if ((attr & FileAttributes.Hidden) != 0) mode[3] = 'h'; // Oculto
        if ((attr & FileAttributes.System) != 0) mode[4] = 's'; // Sistema
        if ((attr & FileAttributes.ReparsePoint) != 0) mode[5] = 'l'; // Enlace simbólico (Reparse Point)

        return new string(mode);
    }
}