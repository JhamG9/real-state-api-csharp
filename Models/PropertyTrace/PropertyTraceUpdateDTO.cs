using System.ComponentModel.DataAnnotations;

namespace RealEstate.API.Models
{
    public class PropertyTraceUpdateDTO
    {
        public DateTime? DateSale { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Value must be a positive number")]
        public decimal? Value { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Tax must be a positive number")]
        public decimal? Tax { get; set; }

        public string? IdProperty { get; set; }
    }
}