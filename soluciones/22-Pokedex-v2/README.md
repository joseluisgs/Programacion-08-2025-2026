# Pokedex - Consultas LINQ con JSON

> *"Gotta catch 'em all"* — Ash Ketchum

### 1. Enunciado

Dado el ficheo `pokemons.json` del directorio `data`, debemos procesarlo y realizar consultas como si de una base de datos se tratara.

### 2. Estructura de Datos

Un **Pokemon** tiene:
- `int id` - Identificador único
- `String num` - Número del pokemon
- `String name` - Nombre del pokemon
- `String img` - URL de la imagen
- `List<String> type` - Tipos del pokemon
- `double height` - Altura en metros
- `double weight` - Peso en kg
- `String candy` - Nombre del candy
- `int candyCount` - Cantidad de candy para evolucionar
- `int egg` - Distancia del huevo
- `double spawnChance` - Probabilidad de aparición
- `double avgSpawns` - Media de apariciones
- `String spawnTime` - Hora de aparición
- `List<double> multipliers` - Multiplicadores
- `List<String> weaknesses` - Debilidades
- `List<Evolution> nextEvolution` - Próximas evoluciones
- `List<Evolution> prevEvolution` - Evolución previa

### 3. Operaciones Requeridas

Deberás realizar las siguientes consultas LINQ:

1. Todos los pokemons
2. Pokemon con id 10
3. Número de pokemons
4. 10 primeros pokemons
5. Pokemon más pesado
6. Pokemon más ligero
7. Pokemon con más evoluciones
8. Pokemon con menos evoluciones
9. Pokemon con más debilidades
10. Pokemons eléctricos
11. Pikachu
12. Número de pokemons por tipo
13. Debilidades con número de pokemons
14. Pokemons eléctricos débiles a Ground
15. Pokemons por debilidad
16. Pokemons sin evoluciones
17. Pokemons con evolución previa
18. Pokemons por tipo secundario
19. Pokemons leyenda
20. Top 5 más pesados
21. Top 5 más altos
22. Pokemons tipo dual
23. Pokemons por tipo de huevo
24. Cadenas de evolución
25. Pokemons sin debilidades

### 4. Ficheros

- **Entrada:** `pokemons.json` - Datos de pokemons (151 pokemons de la primera generación)
- **Carpeta data:** Donde se encuentra el ficheo JSON

### 5. Requisitos Técnicos

- Usar **DTOs** para representar los datos del JSON
- Usar **mappers** para convertir DTOs a modelos
- Usar **LINQ** para todas las consultas
- Usar **System.Text.Json** para parsing JSON
- Estructura limpia y separada en capas
- **Singleton** en Repository
- **Dictionary** para acceso O(1)

---

### Entrega

Sube el proyecto a tu repositorio GitHub con el nombre `Pokedex`.
