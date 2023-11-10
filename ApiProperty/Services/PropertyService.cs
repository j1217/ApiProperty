using System;
using ApiProperty.Models.Domain;
using ApiProperty.DataAccess;
using System.Linq;
using ApiProperty.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ApiProperty.Services
{
    public interface IPropertyService
    {
        Property CreateProperty(Property property);
        List<PropertyInfo> GetPropertiesByCriteria(PropertyFilter filter);
        Property UpdateProperty(int propertyId, Property updatedProperty);
        Property ChangePrice(int propertyId, decimal newPrice);
        PropertyImage AddPropertyImage(int propertyId, byte[] imageBytes);
    }

    public class PropertyService : IPropertyService
    {
        private readonly PropertyDbContext _context;

        public PropertyService(PropertyDbContext context)
        {
            _context = context;
        }

        public Property CreateProperty(Property property)
        {
            if (property == null)
            {
                // Manejo de error si la solicitud no contiene datos válidos
                throw new ArgumentException("Los datos de propiedad no pueden ser nulos.");
            }

            // Realizar validaciones adicionales
            if (string.IsNullOrEmpty(property.Name))
            {
                throw new ArgumentException("El nombre de la propiedad es obligatorio.");
            }

            if (property.Price <= 0)
            {
                throw new ArgumentException("El precio de la propiedad debe ser mayor que cero.");
            }

            // Validacion adicional segun lo requiera el cliente
            //if (property.Year < 1800 || property.Year > DateTime.Now.Year)
            //{
            //    throw new ArgumentException("El año de construcción debe estar dentro de un rango válido.");
            //}

            // Validación de duplicados (ejemplo: nombre de propiedad único)
            if (_context.Properties.Any(p => p.Name == property.Name))
            {
                throw new ArgumentException("Ya existe una propiedad con el mismo nombre.");
            }

            // Validacion adicional segun lo requiera el cliente
            // Validación de propietario existente (si aplica)
            //if (property.IdOwner != null)
            //{
            //    if (!_context.Owners.Any(o => o.IdOwner == property.IdOwner))
            //    {
            //        throw new ArgumentException("El propietario especificado no existe.");
            //    }
            //}

            // Guardar la propiedad en la base de datos
            _context.Properties.Add(property);
            _context.SaveChanges();

            return property;
        }

        public List<PropertyInfo> GetPropertiesByCriteria(PropertyFilter filter)
        {
            // Crear una consulta que filtra propiedades en base a los criterios del filtro
            var query = _context.Properties
                .Where(p =>
                    (filter.Year <= 0 || p.Year == filter.Year) && // Filtrar por año (si se proporciona)
                    (filter.Price <= 0 || p.Price == filter.Price) && // Filtrar por precio (si se proporciona)
                    (filter.IdOwner <= 0 || p.IdOwner == filter.IdOwner)) // Filtrar por ID del propietario (si se proporciona)
                .Include(p => p.Owner) // Incluir información del propietario
                .Include(p => p.PropertyImages) // Incluir información de las imágenes de propiedad
                .Include(p => p.PropertyTraces); // Incluir información de los rastros de propiedad

            // Proyectar los resultados a objetos PropertyInfo
            var result = query
                .Select(p => new PropertyInfo
                {
                    Name = p.Name,
                    Address = p.Address,
                    Price = p.Price,
                    CodeInternal = p.CodeInternal,
                    Year = p.Year,
                    OwnerName = p.Owner.Name, // Obtener el nombre del propietario
                    PropertyImages = p.PropertyImages.Select(pi => new PropertyImage
                    {
                        FileProperty = pi.FileProperty // Obtener las imágenes de propiedad
                    }).ToList(),
                    PropertyTraces = p.PropertyTraces.Select(pt => new PropertyTraceInfo
                    {
                        DataSale = pt.DataSale,
                        Name = pt.Name,
                        Value = pt.Value,
                        Tax = pt.Tax
                    }).ToList()
                })
                .ToList();

            return result;
        }

        public Property UpdateProperty(int propertyId, Property updatedProperty)
        {
            // Buscar la propiedad existente en la base de datos por su ID
            var existingProperty = _context.Properties
                .Include(p => p.PropertyTraces)
                .Include(p => p.PropertyImages)
                .FirstOrDefault(p => p.IdProperty == propertyId);

            if (existingProperty == null)
            {
                // Manejo de error si la propiedad no se encuentra
                throw new ArgumentException("La propiedad no existe o no se puede actualizar.");
            }

            // Realizar validaciones adicionales
            if (string.IsNullOrEmpty(updatedProperty.Name))
            {
                throw new ArgumentException("El nombre de la propiedad es obligatorio.");
            }

            if (updatedProperty.Price <= 0)
            {
                throw new ArgumentException("El precio de la propiedad debe ser mayor que cero.");
            }

            // Actualizar los campos básicos de la propiedad existente
            existingProperty.Name = updatedProperty.Name;
            existingProperty.Address = updatedProperty.Address;
            existingProperty.Price = updatedProperty.Price;
            existingProperty.CodeInternal = updatedProperty.CodeInternal;
            existingProperty.Year = updatedProperty.Year;
            existingProperty.IdOwner = updatedProperty.IdOwner;

            // Actualizar PropertyTraces (datos que se pueden modificar)
            foreach (var updatedTrace in updatedProperty.PropertyTraces)
            {
                var existingTrace = existingProperty.PropertyTraces.FirstOrDefault(t => t.IdPropertyTrace == updatedTrace.IdPropertyTrace);
                if (existingTrace != null)
                {
                    existingTrace.DataSale = updatedTrace.DataSale;
                    existingTrace.Name = updatedTrace.Name;
                    existingTrace.Value = updatedTrace.Value;
                    existingTrace.Tax = updatedTrace.Tax;
                }
            }

            // Actualizar PropertyImages (datos que se pueden modificar)
            foreach (var updatedImage in updatedProperty.PropertyImages)
            {
                var existingImage = existingProperty.PropertyImages.FirstOrDefault(i => i.IdPropertyImage == updatedImage.IdPropertyImage);
                if (existingImage != null)
                {
                    existingImage.FileProperty = updatedImage.FileProperty;
                }
            }

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return existingProperty; // Devolver la propiedad actualizada
        }

        public Property ChangePrice(int propertyId, decimal newPrice)
        {
            // Buscar la propiedad existente en la base de datos por su ID
            var existingProperty = _context.Properties.FirstOrDefault(p => p.IdProperty == propertyId);

            if (existingProperty == null)
            {
                // Manejo de error si la propiedad no se encuentra
                throw new ArgumentException("La propiedad no existe o no se puede actualizar el precio.");
            }

            if (newPrice <= 0)
            {
                // Validar que el nuevo precio sea válido
                throw new ArgumentException("El nuevo precio debe ser mayor que cero.");
            }

            // Cambiar el precio de la propiedad existente
            existingProperty.Price = newPrice;

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return existingProperty; // Devolver la propiedad actualizada
        }

        public PropertyImage AddPropertyImage(int propertyId, byte[] imageBytes)
        {
            // Buscar la propiedad existente en la base de datos por su ID
            var existingProperty = _context.Properties.FirstOrDefault(p => p.IdProperty == propertyId);

            if (existingProperty == null)
            {
                // Manejo de error si la propiedad no se encuentra
                throw new ArgumentException("La propiedad no existe.");
            }

            // Crear una nueva instancia de PropertyImage con los datos de la imagen
            var newPropertyImage = new PropertyImage
            {
                FileProperty = imageBytes,
                Enable = true // Puedes establecer el estado de habilitación según tus requisitos
            };

            // Asociar la imagen con la propiedad
            existingProperty.PropertyImages.Add(newPropertyImage);

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return newPropertyImage; // Devolver la imagen recién agregada
        }

    }
}
