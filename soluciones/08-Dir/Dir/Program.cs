// See https://aka.ms/new-console-template for more information

using Dir.Configuration;
using Dir.Services;

AnalizarArgumentos(args);
DirService.Run();
return;

void AnalizarArgumentos(string[] args) {
    foreach (var arg in args)
        if (arg.Equals("-Force", StringComparison.OrdinalIgnoreCase))
            Config.Force = true;
        else if (Directory.Exists(arg)) Config.DirectoryPath = arg;
}