using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RealEstate.API.Models
{
    public class PropertyTrace
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPropertyTrace { get; set; } = string.Empty;

        [Required]
        public DateTime DateSale { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Value must be a positive number")]
        public decimal Value { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Tax must be a positive number")]
        public decimal Tax { get; set; }

        [Required]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdProperty { get; set; } = string.Empty;
    }
}