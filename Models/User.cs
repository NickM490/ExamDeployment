using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
namespace Exam.Models

{

    public class User
    {

        [Key]
        public int UserId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(45)]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [Required]
        [RegularExpression(@"^((?=.*\d)(?=.*[A-Z])(?=.*\W).{8,8})$", ErrorMessage = "Password must contain at least 1 number, 1 letter, and a special character.")]


        [MinLength(8, ErrorMessage = "Better make it 8. Eight is great, less is a mess!!")]
        public string Password { get; set; }


        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // List of products that are purchased by the user
        // public List<Participant> Participants { get; set; }

        // This is a list of products that will be sold by the user
        public List<Happening> Happenings { get; set; }

    }

    public class LoginUser
    {
        [Required]
        // [EmailAddress]
        public string LoginUsername { get; set; }



        [Required]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}



