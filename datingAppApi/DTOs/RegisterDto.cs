using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace datingAppApi.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string password { get; set; }
    }
}
