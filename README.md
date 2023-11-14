# Sistema de Gestión de Propiedades

## Introducción

El proyecto de "Sistema de Gestión de Propiedades" fue desarrollado en Visual Studio 2022 utilizando el framework .NET 6. Se centra en la administración eficiente de propiedades, propietarios y sus respectivas transacciones. La base de datos subyacente es SQL Server, y se implementa la metodología de código en el enfoque de Entity Framework y Scaffolding. Las consultas a la base de datos se realizaron mediante LINQ y se utilizó First Database para mejorar la eficiencia de las operaciones. A continuación, se proporciona una visión general del proyecto, su estructura y las tecnologías utilizadas.

## Objetivos del Proyecto

El objetivo principal del proyecto es ofrecer una plataforma robusta para la gestión integral de propiedades, permitiendo a los usuarios realizar operaciones como crear propiedades, actualizar información, agregar imágenes y realizar consultas basadas en diversos criterios.

## Tecnologías Utilizadas

- **Visual Studio 2022:** El entorno de desarrollo integrado (IDE) utilizado para la creación del proyecto.
- **.NET 6:** Framework utilizado para el desarrollo de la aplicación.
- **SQL Server:** Motor de base de datos relacional utilizado para almacenar y gestionar los datos.
- **Entity Framework:** ORM (Object-Relational Mapping) utilizado para la interacción con la base de datos.
- **XUnit:** Framework de pruebas unitarias utilizado para asegurar la calidad del código.
- **LINQ:** Lenguaje Integrado de Consulta utilizado para realizar consultas a la base de datos.
- **First Database:** Estrategia utilizada para mejorar la eficiencia de las operaciones de base de datos.
- **Inyección de Dependencias:** Se emplea para mejorar la modularidad y flexibilidad del código, respetando los principios SOLID.
- **Clean Code:** Se aplican prácticas de programación limpias y legibles para mejorar la comprensión y mantenimiento del código.
- **Documentación de .NET:** Se utiliza la herramienta de documentación de .NET para generar comentarios y documentación en el código fuente.

## Principios SOLID y Clean Code

El código sigue los principios SOLID, que son un conjunto de buenas prácticas de diseño de software, y se adhiere a los principios de Clean Code, fomentando la legibilidad y mantenimiento del código.

## Estructura del Proyecto

El proyecto está estructurado en capas, siguiendo una arquitectura limpia y modular:

- **ApiProperty:** Capa de presentación que expone endpoints API para interactuar con el sistema.
- **ApiProperty.DataAccess:** Capa de acceso a datos que contiene los modelos y el contexto de Entity Framework.
- **ApiProperty.Models:** Contiene las clases que definen los modelos de dominio y DTOs.
- **ApiProperty.Services:** Capa de servicios que encapsula la lógica de negocio.
- **ApiProperty.Test:** Proyecto de pruebas unitarias utilizando XUnit para garantizar la calidad del código.

## Implementación de Base de Datos en Código

La metodología de implementación de base de datos en código se realizó mediante el uso de Entity Framework y Scaffolding. Esto proporciona una manera eficiente de generar modelos y contexto a partir de una base de datos existente.

## Pruebas Unitarias

Se realizaron pruebas unitarias exhaustivas utilizando XUnit para verificar la funcionalidad del sistema. Estas pruebas abarcaron escenarios exitosos y no exitosos para métodos críticos, incluyendo `CreateProperty`, `GetPropertiesByCriteria`, `UpdateProperty`, `ChangePrice`, y `AddPropertyImage`. Cada prueba está diseñada para garantizar la integridad y corrección de la lógica de negocio implementada.

## Comentarios y Documentación

El código está documentado utilizando la herramienta de documentación de .NET para facilitar la comprensión y el mantenimiento. Se han incluido comentarios en español para explicar las secciones clave del código, los métodos y las pruebas unitarias.

## Conclusiones

El proyecto proporciona una solución escalable y mantenible para la gestión de propiedades. La implementación en .NET 6 y el uso de tecnologías como Entity Framework, SQL Server, LINQ y First Database garantizan un rendimiento eficiente y una escalabilidad adecuada.
