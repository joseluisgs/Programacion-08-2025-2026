# 🚗 Práctica 05: Análisis de Accidentes de Madrid

> *"Los datos son como las huellas dactilares: únicos, diversos y revelan mucho de quien los deja"* — Anónimo

### 📝 1. Enunciado

Dado el ficheo de accidentes de Madrid en formato CSV, debes procesarlo, exportarlo a JSON y XML, y realizar diversas consultas analíticas.

### 🏗️ 2. Estructura de Datos

El dataset contiene información sobre accidentes con campos como:
- Fecha y hora
- Distrito
- Tipo de vehículo involucrado
- Sexo del conductor
- Estado meteorológico
- Alcohol/drogas
- Víctimas mortales
- Tipo de accidente (atropello, colisión, etc.)

### 🔄 3. Operaciones Requeridas

Implementa las siguientes consultas:

1. **Accidentes con alcohol o drogas** - Filtrar accidentes donde estén implicados
2. **Número de positivos en alcohol y drogas**
3. **Accidentes agrupados por sexo**
4. **Accidentes agrupados por meses**
5. **Accidentes agrupados por tipos de vehículos**
6. **Accidentes en la calle de Leganés**
7. **Número de accidentes por distrito**
8. **Estadísticas por distrito**
9. **Accidentes por distrito (orden descendente)**
10. **Accidentes de fin de semana y noche** (desde las 20:00 hasta las 06:00)
11. **Accidentes de fin de semana, noche y positivos en alcohol**
12. **Accidentes con más de un fallecido**
13. **Distrito con más accidentes** vs **distrito con más accidentes en fin de semana**
14. **Accidentes con alcohol/drogas y víctimas mortales**
15. **Número de atropellos a personas**
16. **Accidentes agrupados por estado meteorológico**
17. **Lista de atropellos a animales**

### 📁 4. Ficheros

- **Entrada:** `2023_Accidentalidad.csv` - Datos de accidentes
- **Salida:** Ficheros JSON y XML exportados
- **Documentación:** `Estructura_ConjuntoDatos_Accidentesv2.pdf`

### ⚙️ 5. Requisitos Técnicos

- Usar **DTOs** para mapear los datos del CSV
- Exportar a **JSON** usando `System.Text.Json`
- Exportar a **XML** usando `XmlSerializer`
- Usar **LINQ** para todas las consultas
- Usar sintaxis moderna `using var`

### 📊 6. Consultas Avanzadas

- Generar estadísticas comparativas entre distritos
- Analizar patrones de accidentes (hora, condiciones meteorológicas)
- Comparar accidentes entre semana vs fin de semana

---

### 📤 Entrega

Sube el proyecto a tu repositorio GitHub con el nombre `05-Accidentes`.
