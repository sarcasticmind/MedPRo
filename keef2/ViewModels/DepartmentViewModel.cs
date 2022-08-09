using Keefa1.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace keef2.ViewModels
{
    public class DepartmentViewModel
    {
        public int id { get; set; }
        [Required]
        [MinLength(3)]
        public string name { get; set; }

        public IFormFile img { get; set; }

    }
}
