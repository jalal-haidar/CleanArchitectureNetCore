using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.RequestModels.Auth
{
    public class ResetPasswordRequestModel
    {
        public string Password { get; set; }
        /// <summary>
        /// E stands for email. E is encoded email and will be sent as a query string parameter in the reset password link
        /// </summary>
        [Required]
        public string E { get; set; }
        /// <summary>
        /// ET stands for expire time. ET is encoded expire time and will be sent as a query string parameter in the reset password link
        /// </summary>
        [Required]
        public string ET { get; set; }
        /// <summary>
        /// T stands for token. T is encoded token and will be sent as a query string parameter in the reset password link
        /// </summary>
        [Required]
        public string T { get; set; }

        /// <summary>
        /// UT stands for UserType which differentiate between different staff and patient
        /// </summary>
        public string UT { get; set; }
    }
}
