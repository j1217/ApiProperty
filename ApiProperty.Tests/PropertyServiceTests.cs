using System;
using ApiProperty.DataAccess;
using ApiProperty.Models.Domain;
using ApiProperty.Models.DTO;
using ApiProperty.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ApiProperty.Test
{
    public class PropertyServiceTests
    {
        [Fact]
        public void CreateProperty_Successful()
        {
            // Arrange
            // Configuración: Crear un contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_Successful")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Actuar
                // Crear una instancia del servicio de propiedades
                var propertyService = new PropertyService(context);

                // Crear una propiedad válida
                var propertyToCreate = new Property
                {
                    Name = "Test Property",
                    Price = 100000,
                    // ... otros campos requeridos
                };

                // Actuar y Verificar
                // Crear la propiedad y asegurarse de que se haya creado correctamente
                var createdProperty = propertyService.CreateProperty(propertyToCreate);

                // Verificar que la propiedad creada tenga los valores esperados
                Assert.NotNull(createdProperty);
                Assert.Equal(propertyToCreate.Name, createdProperty.Name);
                // Agregar más aserciones según tus requisitos específicos
            }
        }

        [Fact]
        public void CreateProperty_NullInput()
        {
            // Arrange
            // Configuración: Crear un contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_NullInput")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Actuar
                // Crear una instancia del servicio de propiedades
                var propertyService = new PropertyService(context);

                // Actuar y Verificar
                // Intentar crear una propiedad con entrada nula y asegurarse de que se lance una excepción
                Assert.Throws<ArgumentException>(() => propertyService.CreateProperty(null));
            }
        }

        [Fact]
        public void CreateProperty_DuplicateName()
        {
            // Arrange
            // Configuración: Crear un contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_DuplicateName")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Configuración: Agregar una propiedad existente a la base de datos
                context.Properties.Add(new Property { Name = "Existing Property", Price = 50000 });
                context.SaveChanges();

                // Actuar
                // Crear una instancia del servicio de propiedades
                var propertyService = new PropertyService(context);

                // Actuar y Verificar
                // Intentar crear una nueva propiedad con el mismo nombre y asegurarse de que se lance la excepción adecuada
                var propertyToCreate = new Property { Name = "Existing Property", Price = 75000 };
                Assert.Throws<ArgumentException>(() => propertyService.CreateProperty(propertyToCreate));
            }
        }

        /// <summary>
        /// Prueba el escenario en el que existen propiedades que cumplen con el filtro.
        /// </summary>
        [Fact]
        public void GetPropertiesByCriteria_Successful()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_GetPropertiesByCriteria_Successful")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Agregar datos de prueba al contexto
                var owner = new Owner { Name = "John Doe" };
                var property1 = new Property { Name = "Property 1", Price = 100000, IdOwner = owner.IdOwner };
                var property2 = new Property { Name = "Property 2", Price = 150000, IdOwner = owner.IdOwner };
                context.Owners.Add(owner);
                context.Properties.AddRange(property1, property2);
                context.SaveChanges();

                var propertyService = new PropertyService(context);

                // Actuar
                // Filtrar propiedades con un precio específico
                var filter = new PropertyFilter { Price = 100000 };
                var properties = propertyService.GetPropertiesByCriteria(filter);

                // Verificar
                // Asegurarse de que se devuelva la cantidad correcta de propiedades
                Assert.Equal(1, properties.Count);
                // Asegurarse de que las propiedades devueltas cumplan con el filtro
                Assert.Equal(filter.Price, properties.First().Price);
            }
        }

        /// <summary>
        /// Prueba el escenario en el que no hay propiedades que cumplan con el filtro.
        /// </summary>
        [Fact]
        public void GetPropertiesByCriteria_NoResults()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_GetPropertiesByCriteria_NoResults")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                var propertyService = new PropertyService(context);

                // Actuar
                // Filtrar propiedades con un precio específico en un contexto vacío
                var filter = new PropertyFilter { Price = 100000 };
                var properties = propertyService.GetPropertiesByCriteria(filter);

                // Verificar
                // Asegurarse de que no se hayan devuelto propiedades
                Assert.Empty(properties);
            }
        }

        /// <summary>
        /// Prueba el escenario en el que el filtro no coincide con ninguna propiedad existente.
        /// </summary>
        [Fact]
        public void GetPropertiesByCriteria_InvalidFilter()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_GetPropertiesByCriteria_InvalidFilter")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Agregar datos de prueba al contexto
                var owner = new Owner { Name = "John Doe" };
                var property1 = new Property { Name = "Property 1", Price = 100000, IdOwner = owner.IdOwner };
                context.Owners.Add(owner);
                context.Properties.Add(property1);
                context.SaveChanges();

                var propertyService = new PropertyService(context);

                // Actuar
                // Filtrar propiedades con un precio que no existe
                var filter = new PropertyFilter { Price = 200000 };
                var properties = propertyService.GetPropertiesByCriteria(filter);

                // Verificar
                // Asegurarse de que no se hayan devuelto propiedades
                Assert.Empty(properties);
            }
        }

        /// <summary>
        /// Prueba el escenario en el que se actualiza una propiedad con datos válidos.
        /// </summary>
        [Fact]
        public void UpdateProperty_Successful()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_UpdateProperty_Successful")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Agregar datos de prueba al contexto
                var owner = new Owner { Name = "John Doe" };
                var property = new Property { Name = "Property 1", Price = 100000, IdOwner = owner.IdOwner };
                context.Owners.Add(owner);
                context.Properties.Add(property);
                context.SaveChanges();

                var propertyService = new PropertyService(context);

                // Actuar
                // Actualizar la propiedad con datos válidos
                var updatedProperty = new Property
                {
                    Name = "Updated Property",
                    Price = 150000,
                    IdOwner = owner.IdOwner
                    // Agregar otros campos según sea necesario
                };

                var result = propertyService.UpdateProperty(property.IdProperty, updatedProperty);

                // Verificar
                // Asegurarse de que la propiedad se haya actualizado correctamente
                Assert.NotNull(result);
                Assert.Equal(updatedProperty.Name, result.Name);
                Assert.Equal(updatedProperty.Price, result.Price);
                // Agregar más aserciones según tus necesidades específicas
            }
        }

        /// <summary>
        /// Prueba el escenario en el que se intenta actualizar una propiedad inexistente.
        /// </summary>
        [Fact]
        public void UpdateProperty_NonExistentProperty()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_UpdateProperty_NonExistentProperty")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                var propertyService = new PropertyService(context);

                // Actuar y Verificar
                // Intentar actualizar una propiedad inexistente
                var updatedProperty = new Property
                {
                    Name = "Updated Property",
                    Price = 150000,
                    // Otros campos
                };

                // Asegurarse de que se lanza una excepción ArgumentException
                Assert.Throws<ArgumentException>(() => propertyService.UpdateProperty(1, updatedProperty));
            }
        }

        /// <summary>
        /// Prueba el escenario en el que se intenta actualizar una propiedad con datos inválidos.
        /// </summary>
        [Fact]
        public void UpdateProperty_InvalidData()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_UpdateProperty_InvalidData")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Agregar datos de prueba al contexto
                var owner = new Owner { Name = "John Doe" };
                var property = new Property { Name = "Property 1", Price = 100000, IdOwner = owner.IdOwner };
                context.Owners.Add(owner);
                context.Properties.Add(property);
                context.SaveChanges();

                var propertyService = new PropertyService(context);

                // Actuar y Verificar
                // Intentar actualizar la propiedad con datos inválidos (por ejemplo, nombre nulo)
                var updatedProperty = new Property
                {
                    Name = null, // Nombre nulo, lo cual es inválido
                    Price = 150000,
                    IdOwner = owner.IdOwner
                    // Otros campos
                };

                // Asegurarse de que se lanza una excepción ArgumentException
                Assert.Throws<ArgumentException>(() => propertyService.UpdateProperty(property.IdProperty, updatedProperty));
            }
        }

        /// <summary>
        /// Prueba el escenario en el que se cambia exitosamente el precio de una propiedad.
        /// </summary>
        [Fact]
        public void ChangePrice_Successful()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_ChangePrice_Successful")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Agregar datos de prueba al contexto
                var property = new Property { Name = "Property 1", Price = 100000 };
                context.Properties.Add(property);
                context.SaveChanges();

                var propertyService = new PropertyService(context);

                // Actuar
                // Cambiar el precio de la propiedad
                var newPrice = 150000;
                var result = propertyService.ChangePrice(property.IdProperty, newPrice);

                // Verificar
                // Asegurarse de que la propiedad se haya actualizado correctamente
                Assert.NotNull(result);
                Assert.Equal(newPrice, result.Price);
            }
        }

        /// <summary>
        /// Prueba el escenario en el que se intenta cambiar el precio de una propiedad inexistente.
        /// </summary>
        [Fact]
        public void ChangePrice_NonExistentProperty()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_ChangePrice_NonExistentProperty")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                var propertyService = new PropertyService(context);

                // Actuar y Verificar
                // Intentar cambiar el precio de una propiedad inexistente
                var newPrice = 150000;

                // Asegurarse de que se lanza una excepción ArgumentException
                Assert.Throws<ArgumentException>(() => propertyService.ChangePrice(1, newPrice));
            }
        }

        /// <summary>
        /// Prueba el escenario en el que se intenta cambiar el precio de una propiedad a un valor no válido.
        /// </summary>
        [Fact]
        public void ChangePrice_InvalidPrice()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_ChangePrice_InvalidPrice")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Agregar datos de prueba al contexto
                var property = new Property { Name = "Property 1", Price = 100000 };
                context.Properties.Add(property);
                context.SaveChanges();

                var propertyService = new PropertyService(context);

                // Actuar y Verificar
                // Intentar cambiar el precio a un valor no válido (por ejemplo, precio negativo)
                var newPrice = -50000;

                // Asegurarse de que se lanza una excepción ArgumentException
                Assert.Throws<ArgumentException>(() => propertyService.ChangePrice(property.IdProperty, newPrice));
            }
        }

        /// <summary>
        /// Prueba el escenario en el que se agrega exitosamente una imagen a una propiedad.
        /// </summary>
        [Fact]
        public void AddPropertyImage_Successful()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_AddPropertyImage_Successful")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Agregar datos de prueba al contexto
                var property = new Property { Name = "Property 1" };
                context.Properties.Add(property);
                context.SaveChanges();

                var propertyService = new PropertyService(context);

                // Actuar
                // Agregar una imagen a la propiedad
                var imageBytes = new byte[] { 0x1, 0x2, 0x3 }; // Datos de imagen simulados
                var result = propertyService.AddPropertyImage(property.IdProperty, imageBytes);

                // Verificar
                // Asegurarse de que la imagen se haya agregado correctamente
                Assert.NotNull(result);
                Assert.True(result.Enable.Value);
                // Agregar más aserciones según tus necesidades específicas
            }
        }

        /// <summary>
        /// Prueba el escenario en el que se intenta agregar una imagen a una propiedad inexistente.
        /// </summary>
        [Fact]
        public void AddPropertyImage_NonExistentProperty()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_AddPropertyImage_NonExistentProperty")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                var propertyService = new PropertyService(context);

                // Actuar y Verificar
                // Intentar agregar una imagen a una propiedad inexistente
                var imageBytes = new byte[] { 0x1, 0x2, 0x3 };

                // Asegurarse de que se lanza una excepción ArgumentException
                Assert.Throws<ArgumentException>(() => propertyService.AddPropertyImage(1, imageBytes));
            }
        }

        /// <summary>
        /// Prueba el escenario en el que se intenta agregar una imagen con datos no válidos.
        /// </summary>
        [Fact]
        public void AddPropertyImage_InvalidImageData()
        {
            // Arrange
            // Configuración: Crear opciones para el contexto de base de datos en memoria
            var options = new DbContextOptionsBuilder<DbpropertyJfazContext>()
                .UseInMemoryDatabase(databaseName: "InMemory_PropertyService_AddPropertyImage_InvalidImageData")
                .Options;

            using (var context = new DbpropertyJfazContext(options))
            {
                // Agregar datos de prueba al contexto
                var property = new Property { Name = "Property 1" };
                context.Properties.Add(property);
                context.SaveChanges();

                var propertyService = new PropertyService(context);

                // Actuar y Verificar
                // Intentar agregar una imagen con datos no válidos (por ejemplo, datos de imagen nulos)
                byte[] invalidImageData = null;

                // Asegurarse de que se lanza una excepción ArgumentException
                Assert.Throws<ArgumentException>(() => propertyService.AddPropertyImage(property.IdProperty, invalidImageData));
            }
        }
    }
}
