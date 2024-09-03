using System.ComponentModel.DataAnnotations;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Models
{
    public class VillaIndexViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public double Price { get; set; }
        
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
    }
}
