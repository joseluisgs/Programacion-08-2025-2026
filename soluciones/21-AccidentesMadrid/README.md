# Accidentes Madrid - Consultas LINQ con CSV

> *"Los datos son el nuevo petróleo"* — Clive Humby

### 1. Enunciado

Dado el ficheo `2025_Accidentalidad.csv` del directorio `data`, debemos procesarlo y realizar consultas como si de una base de datos se tratara.

### 2. Estructura de Datos

Un **Accidente** tiene:
- `string NumExpediente` - Identificador único
- `DateTime Fecha` - Fecha del accidente
- `TimeSpan Hora` - Hora del accidente
- `string Localizacion` - Calle o cruce
- `int Numero` - Número de calle
- `int CodDistrito` - Código del distrito
- `string Distrito` - Nombre del distrito
- `string TipoAccidente` - Tipo de accidente
- `string EstadoMeteorologico` - Condición climática
- `string TipoVehiculo` - Tipo de vehículo implicado
- `TipoPersona TipoPersona` - Conductor, Pasajero, Peatón
- `string RangoEdad` - Tramo de edad
- `Sexo Sexo` - Hombre, Mujer, NoAsignado
- `string CodLesividad` - Código de lesividad
- `string Lesividad` - Descripción de lesividad
- `bool PositivoAlcohol` - Si ha dado positivo en alcohol
- `bool PositivoDroga` - Si ha dado positivo en drogas

### 3. Operaciones Requeridas

Deberás realizar las siguientes consultas LINQ:

1. 5 primeros accidentes
2. Accidentes con alcohol o drogas
3. Positivos alcohol Y drogas
4. Por sexo
5. Por meses
6. Mes con más accidentes
7. Por tipo de vehículo
8. Accidentes en calle Leganés
9. Por distrito (ASC)
10. Accidentes en USERA
11. Stats por distrito (Max/Min/Avg)
12. Por distrito (DESC)
13. Fin de semana + noche + alcohol
14. Por lesividad
15. Fallecidos
16. Fallecidos + alcohol/drogas
17. Por meteorología
18. Granizo por distrito
19. Alcohol/Drogas/Nada
20. Distrito más alcohol
21. Distrito más drogas
22. Distrito más alcohol+drogas

### 4. Ficheros

- **Entrada:** `2025_Accidentalidad.csv` - Datos de accidentes de Madrid (2025)
- **Carpeta data:** Donde se encuentra el fichero CSV

### 5. Requisitos Técnicos

- Usar **DTOs** para representar los datos del CSV
- Usar **mappers** para convertir DTOs a modelos
- Usar **enums** para Sexo y TipoPersona
- Usar **LINQ** para todas las consultas
- **SIN logger en inserts** del repository (son ~20k registros)
- Estructura limpia y separada en capas
- **Singleton** en Repository
- **Dictionary** para acceso O(1)

---

### Entrega

Sube el proyecto a tu repositorio GitHub con el nombre `AccidentesMadrid`.
