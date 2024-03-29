using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;


namespace Domain.Entities
{
    public class Villa
    {
        public int Id{ get; set; }
        public required string Name { get; set; }
        public string? Description{ get; set; }
        [Display(Name = "Square feet per meter")]
        public int Sqft{ get; set; }
        [Display(Name = "Price per night")]
        public decimal Price { get; set; }
        public int Occupancy{ get; set; }
        [NotMapped]
        public IFormFile? Image{ get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate{ get; set; }
        public DateTime? UpdatedDate{ get; set; }

        [ValidateNever]
        public IEnumerable<Amenity> VillaAmenity { get; set; }

    }
}
