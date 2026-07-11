using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Application.DTO.Auth
{
    public class responseforgetpass
    {
        public string token {  get; set; }=string.Empty;

        [EmailAddress]
        public string email { get; set; } = string.Empty;

        public string errorMessage { get; set; } = string.Empty;
    }
}
