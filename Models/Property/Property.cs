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
    }
}