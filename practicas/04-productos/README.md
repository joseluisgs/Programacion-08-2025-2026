# 🛒 Práctica 04: Procesador de Productos con CSV y LINQ

> *"Los datos son el nuevo petróleo"* — Clive Humby

### 📝 1. Enunciado

Dado el ficheo `products.csv`del directorio `data`, debemos procesarlo y realizar consultas como si de una base de datos se tratara.

### 🏗️ 2. Estructura de Datos

Un **Producto** tiene:
- `int id` - Identificador único
- `String name` - Nombre del producto
- `int supplier` - Identificador del proveedor
- `int category` - Identificador de categoría
- `double unitPrice` - Precio unitario
- `int unitsInStock` - Unidades en stock

### 🔄 3. Operaciones Requeridas

Deberás realizar las siguientes consultas LINQ equivalente a SQL:

1. **Todos los productos** - Equivalente a `SELECT * FROM products`
2. **Nombre de los productos** - Equivalente a `SELECT name FROM products`
3. **Productos con stock menor que 10** - Equivalente a `SELECT name FROM products WHERE units_in_stock < 10`
4. **Productos con stock menor a 5 ordenados por stock** - Equivalente a `SELECT name FROM products WHERE units_in_stock < 5 ORDER BY units_in_stock ASC`
5. **Número de proveedores existentes** - Equivalente a `SELECT COUNT(1), supplierID FROM products GROUP BY supplierID`
6. **Número de existencias por producto**
7. **Número de productos por proveedor**
8. **Media de precio por proveedor**
9. **Producto más caro**
10. **Proveedores con más de 5 productos**
11. **Proveedores cuya suma de precios supere 100**
12. **Categorías y número de productos por categoría**
13. **Categoría más cara**
14. **Precio máximo, mínimo, medio y cantidad por categoría**

### 📁 4. Ficheros

- **Entrada:** `products.csv` - Datos de productos
- **Carpeta data:** Donde se encuentra el ficheo CSV

### ⚙️ 5. Requisitos Técnicos

- Usar **DTOs** para representar los datos
- Usar **LINQ** para todas las consultas
- Usar sintaxis moderna `using var`
- Estructura limpia y separada

---

### 📤 Entrega

Sube el proyecto a tu repositorio GitHub con el nombre `04-Productos`.
