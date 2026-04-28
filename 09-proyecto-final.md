- [9. PROYECTO FINAL: Sistema CRUD de Estudiantes con Persistencia JSON](#9-proyecto-final-sistema-crud-de-estudiantes-con-persistencia-json)
  - [9.1. Introducción al Proyecto](#91-introducción-al-proyecto)
  - [9.2. Modelo de Dominio: Student](#92-modelo-de-dominio-student)
  - [9.3. DTO para Persistencia](#93-dto-para-persistencia)
  - [9.4. Interfaz del Repositorio](#94-interfaz-del-repositorio)
  - [9.5. Implementación: StudentJsonRepository](#95-implementación-studentjsonrepository)
  - [9.6. Servicio de Búsqueda con LINQ](#96-servicio-de-búsqueda-con-linq)
  - [9.7. Programa Principal](#97-programa-principal)

# 9. PROYECTO FINAL: Sistema CRUD de Estudiantes con Persistencia JSON

## 9.1. Introducción al Proyecto

Vamos a construir un sistema completo de gestión de estudiantes (CRUD) que persista los datos en ficheros JSON.

**Funcionalidades:**
- ✅ Crear estudiante
- ✅ Leer todos los estudiantes
- ✅ Actualizar estudiante
- ✅ Eliminar estudiante
- ✅ Buscar por nombre
- ✅ Filtrar por nota
- ✅ Exportar a CSV

## 9.2. Modelo de Dominio: Student

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public double Grade { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    public bool IsApproved() => Grade >= 5.0;
}
```

## 9.3. DTO para Persistencia

```csharp
using System.Text.Json.Serialization;

public record StudentDto(
    [property: JsonPropertyName("id")] 
    int Id,
    
    [property: JsonPropertyName("name")] 
    string Name,
    
    [property: JsonPropertyName("age")] 
    int Age,
    
    [property: JsonPropertyName("grade")] 
    double Grade,
    
    [property: JsonPropertyName("createdAt")] 
    DateTime CreatedAt
);

// Mapper
public static class StudentMapper
{
    public static StudentDto ToDto(Student s) => new(s.Id, s.Name, s.Age, s.Grade, s.CreatedAt);
    public static Student ToDomain(StudentDto dto) => new() 
    { 
        Id = dto.Id, 
        Name = dto.Name, 
        Age = dto.Age, 
        Grade = dto.Grade, 
        CreatedAt = dto.CreatedAt 
    };
}
```

## 9.4. Interfaz del Repositorio

```csharp
public interface IStudentRepository
{
    List<Student> GetAll();
    Student? GetById(int id);
    void Add(Student student);
    void Update(Student student);
    void Delete(int id);
    List<Student> Search(string name);
    List<Student> FilterByGrade(double minGrade);
}
```

## 9.5. Implementación: StudentJsonRepository

```csharp
using System.Text.Json;

public class StudentJsonRepository : IStudentRepository
{
    private readonly string _filePath;
    private List<Student> _students = new();
    
    public StudentJsonRepository(string filePath = "students.json")
    {
        _filePath = filePath;
        Load();
    }
    
    private void Load()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            var dtos = JsonSerializer.Deserialize<List<StudentDto>>(json) ?? new();
            _students = dtos.Select(StudentMapper.ToDomain).ToList();
        }
    }
    
    private void Save()
    {
        var opciones = new JsonSerializerOptions { WriteIndented = true };
        var dtos = _students.Select(StudentMapper.ToDto).ToList();
        var json = JsonSerializer.Serialize(dtos, opciones);
        File.WriteAllText(_filePath, json);
    }
    
    public List<Student> GetAll() => _students.ToList();
    
    public Student? GetById(int id) => _students.FirstOrDefault(s => s.Id == id);
    
    public void Add(Student student)
    {
        student.Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
        student.CreatedAt = DateTime.Now;
        _students.Add(student);
        Save();
    }
    
    public void Update(Student student)
    {
        var index = _students.FindIndex(s => s.Id == student.Id);
        if (index >= 0)
        {
            _students[index] = student;
            Save();
        }
    }
    
    public void Delete(int id)
    {
        var student = _students.FirstOrDefault(s => s.Id == id);
        if (student != null)
        {
            _students.Remove(student);
            Save();
        }
    }
    
    public List<Student> Search(string name) =>
        _students.Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    
    public List<Student> FilterByGrade(double minGrade) =>
        _students.Where(s => s.Grade >= minGrade).ToList();
}
```

## 9.6. Servicio de Búsqueda con LINQ

```csharp
public class StudentService
{
    private readonly IStudentRepository _repository;
    
    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }
    
    // Mejores estudiantes
    public List<Student> GetTopStudents(int count = 5) =>
        _repository.GetAll()
            .OrderByDescending(s => s.Grade)
            .Take(count)
            .ToList();
    
    // Estadísticas
    public double GetAverageGrade() =>
        _repository.GetAll().Average(s => s.Grade);
    
    // Porcentaje de aprobados
    public double GetApprovalRate()
    {
        var all = _repository.GetAll();
        if (!all.Any()) return 0;
        return (double)all.Count(s => s.IsApproved()) / all.Count * 100;
    }
    
    // Agrupar por edad
    public Dictionary<int, List<Student>> GetGroupedByAge() =>
        _repository.GetAll()
            .GroupBy(s => s.Age)
            .ToDictionary(g => g.Key, g => g.ToList());
}
```

## 9.7. Programa Principal

```csharp
class Program
{
    static void Main()
    {
        var repo = new StudentJsonRepository("students.json");
        var service = new StudentService(repo);
        
        // Añadir estudiantes de prueba
        repo.Add(new Student { Name = "Ana García", Age = 20, Grade = 8.5 });
        repo.Add(new Student { Name = "Juan Pérez", Age = 22, Grade = 7.0 });
        repo.Add(new Student { Name = "María López", Age = 21, Grade = 9.2 });
        
        Console.WriteLine("=== CRUD DE ESTUDIANTES ===\n");
        
        // Mostrar todos
        Console.WriteLine(">>> Todos los estudiantes:");
        foreach (var s in repo.GetAll())
        {
            Console.WriteLine($"  {s.Id}: {s.Name} - Nota: {s.Grade}");
        }
        
        // Buscar
        Console.WriteLine("\n>>> Buscar 'Ana':");
        foreach (var s in repo.Search("Ana"))
        {
            Console.WriteLine($"  {s.Name}: {s.Grade}");
        }
        
        // Estadísticas
        Console.WriteLine($"\n>>> Nota media: {service.GetAverageGrade():F2}");
        Console.WriteLine($">>> Tasa aprobación: {service.GetApprovalRate():F1}%");
    }
}
```

> 📝 **Nota del Profesor**: Este proyecto integra todo lo aprendido: DTOs, JSON, LINQ, interfaces, y operaciones CRUD. Es un ejemplo completo de arquitectura limpia.

> 💡 **Tip del Examinador**: Este tipo de proyecto es ideal para el examen. Practica creando variaciones como exportar a CSV, añadir más filtros, o usar XML.
