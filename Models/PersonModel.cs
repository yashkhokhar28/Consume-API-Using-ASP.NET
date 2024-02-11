using System.ComponentModel.DataAnnotations;

namespace ConsumeAPI.Models
{
    public class PersonModel
    {
        public int PersonID { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Contact { get; set; }
        [Required]
        public string? Email { get; set; }
        public int CityID { get; set; }

        public string? CityName { get; set; }
    }

    public class CityDropDownModel
    {
        public int CityID { get; set; }

        public string? CityName { get; set; }
    }
}
