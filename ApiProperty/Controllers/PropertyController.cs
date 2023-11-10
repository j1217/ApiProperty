using System;
using ApiProperty.Models.Domain;
using ApiProperty.Models.DTO;
using ApiProperty.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiProperty.Controllers
{
    [Route("api/properties")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        // Endpoint para crear una propiedad
        [HttpPost]
        public IActionResult CreateProperty([FromBody] Property property)
        {
            try
            {
                if (property == null)
                {
                    return BadRequest("La solicitud no contiene datos de propiedad válidos.");
                }

                var createdProperty = _propertyService.CreateProperty(property);

                return Created($"/api/properties/{createdProperty.IdProperty}", createdProperty);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Manejo de errores de validación
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor"); // Manejo de otros errores
            }
        }

        // Endpoint para obtener propiedades según criterios
        [HttpGet]
        public IActionResult GetPropertiesByCriteria([FromQuery] PropertyFilter filter)
        {
            try
            {
                var properties = _propertyService.GetPropertiesByCriteria(filter);
                return Ok(properties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor"); // Manejo de errores
            }
        }

        // Endpoint para actualizar una propiedad
        [HttpPut("{propertyId}")]
        public IActionResult UpdateProperty(int propertyId, [FromBody] Property updatedProperty)
        {
            try
            {
                var property = _propertyService.UpdateProperty(propertyId, updatedProperty);
                return Ok(property);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Manejo de errores de validación
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor"); // Manejo de otros errores
            }
        }

        // Endpoint para cambiar el precio de una propiedad
        [HttpPut("{propertyId}/changePrice")]
        public IActionResult ChangePrice(int propertyId, [FromBody] decimal newPrice)
        {
            try
            {
                var property = _propertyService.ChangePrice(propertyId, newPrice);
                return Ok(property);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Manejo de errores de validación
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor"); // Manejo de otros errores
            }
        }

        // Endpoint para agregar una imagen a la propiedad
        [HttpPost("{propertyId}/images")]
        public IActionResult AddPropertyImage(int propertyId, [FromBody] byte[] imageBytes)
        {
            try
            {
                var image = _propertyService.AddPropertyImage(propertyId, imageBytes);
                return Created($"/api/properties/{propertyId}/images/{image.IdPropertyImage}", image);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Manejo de errores de validación
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor"); // Manejo de otros errores
            }
        }
    }
}
