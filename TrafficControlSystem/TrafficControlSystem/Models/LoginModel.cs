﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrafficControlSystem.Models
{
    public class LoginModel
    {
        [Required]
        [Display(Name="User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="RememberMe")]
        public bool RememberMe { get; set; }
    }
}