using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.API.Models
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdProperty { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string CodeInternal { get; set; } = null!; // código único interno de la propiedad

        [Required]
        public int Year { get; set; }  // año de construcción

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOwner { get; set; } = null!; // relación con el Owner

        // Lista de imágenes de la propiedad (no se almacena en MongoDB, se llena desde PropertyImageService)
        [BsonIgnore]
        public List<string> Images { get; set; } = new List<string>();

        // Información completa del propietario (no se almacena en MongoDB, se llena desde OwnerService)
        [BsonIgnore]
        public Owner? Owner { get; set; }

        // Lista de transacciones/traces de la propiedad (no se almacena en MongoDB, se llena desde PropertyTraceService)
        [BsonIgnore]
        public List<PropertyTrace> PropertyTraces { get; set; } = new List<PropertyTrace>();
    }
}