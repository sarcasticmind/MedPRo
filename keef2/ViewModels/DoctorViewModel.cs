using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace keef2.ViewModels
{
    public class DoctorViewModel
    {
        public int id { get; set; }
        [Required]
        [MinLength(3)]
        public string fName { get; set; }
        [Required]
        [MinLength(3)]
        public string lName { get; set; }

        [RegularExpression(@"^01[0|1|2|5]{1}[0-9]{8}$")]
        public string? mobilePhone { get; set; }
        [RegularExpression(@"^0[0-9]{8,9}$")]
        public string? clinicPhone { get; set; }

        public IFormFile img { get; set; }
        [Required]
        public bool available { get; set; }
        public string? time { get; set; }
        public string? description { get; set; }
        [MinLength(3)]
        public string? government { get; set; }
        [MinLength(3)]
        public string? city { get; set; }
        [Range(0, 100)]
        public int? offer { get; set; }
        public System.DateTime? offerDate { get; set; }
        public int dept_id { get; set; }

    }
}
