namespace FicherosCsv.Models;

public class Estudiante {
    public int Id { get; set; } = 0;
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; } = 0;
    public double Nota { get; set; } = 0.0;
    public bool Aprobado  { get; set; } = false;
}