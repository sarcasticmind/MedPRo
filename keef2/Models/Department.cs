using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Keefa1.Models
{
    public class Department
    {
        public int id { get; set; }
        [Required]
        [MinLength(3)]
        public string name { get; set; }

        public string? img { get; set; }

        [JsonIgnore]
        public virtual List<Doctor> Doctors { get; set; }
    }
}
