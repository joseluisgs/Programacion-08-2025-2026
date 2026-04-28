# 🌤️ Práctica 06: Servicio Meteorológico AEMET

> *"El clima es lo que esperas; el tiempo es lo que obtienes"* — Robert Heinlein

### 📝 1. Enunciado

Dado los ficheos CSV del servicio meteorológico (AEMET), debes combinarlos, procesarlos y generar informes.

### 🏗️ 2. Estructura de Datos

Los ficheos contienen datos meteorológicos con campos como:
- Provincia
- Localidad/Lugar
- Fecha
- Temperatura máxima y mínima
- Hora de temperatura máxima y mínima
- Precipitación
- other relevant meteorological data

### 🔄 3. Operaciones Requeridas

#### Parte 1: Combinación de Ficheros
1. Leer los tres ficheos CSV (`Aemet20171029.csv`, `Aemet20171030.csv`, `Aemet20171031.csv`)
2. Combinarlos en un único dataset
3. Completar los datos incluyendo el campo fecha

#### Parte 2: Exportación
4. Exportar el resultado combinado a **JSON**
5. Exportar el resultado combinado a **XML**

#### Parte 3: Consultas LINQ
6. **Temperatura máxima por día y lugar**
7. **Temperatura mínima por día y lugar**
8. **Temperatura máxima por provincia** (día, lugar, valor y momento)
9. **Temperatura mínima por provincia** (día, lugar, valor y momento)
10. **Temperatura media por provincia** (día, lugar y valor)
11. **Precipitación media por día y provincia**
12. **Número de lugares donde llovió por día y provincia**
13. **Temperatura media de la provincia de Madrid**
14. **Media de temperatura máxima total**
15. **Media de temperatura mínima total**
16. **Lugares donde la máxima ha sido antes de las 15:00 por día**
17. **Lugares donde la mínima ha sido después de las 17:30 por día**

#### Parte 4: Informe Especial (Madrid)
18. Para la provincia de Madrid, exportar en **JSON** y **XML** un informe con:
    - Por cada día:
      - Temperatura media
      - Temperatura máxima (lugar y momento)
      - Temperatura mínima (lugar y momento)
      - Si hubo precipitación (sí/no) y valor de la misma

### 📁 4. Ficheros

- **Entrada:** 
  - `Aemet20171029.csv`
  - `Aemet20171030.csv`
  - `Aemet20171031.csv`
- **Salida:** Ficheros JSON y XML combinados

### ⚙️ 5. Requisitos Técnicos

- Usar **DTOs** para representar los datos meteorológicos
- Usar **LINQ** para procesamiento y consultas
- Usar **System.Text.Json** para JSON
- Usar **XmlSerializer** para XML
- Usar sintaxis moderna `using var`

---

### 📤 Entrega

Sube el proyecto a tu repositorio GitHub con el nombre `06-Aemet`.
