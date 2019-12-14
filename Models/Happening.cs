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
    public class Happening

    {
        [Key]
        public int HappeningId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(45)]
        public string Title { get; set; }

        [Required]
        [Range(1, 60)]
        public int Duration { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(7)]
        public string HoursMinutesDays { get; set; }

        public DateTime? StartTime { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string StartTimeValue
        {
            get
            {
                return StartTime.HasValue ? StartTime.Value.ToString("hh:mm tt") : string.Empty;
            }

            set
            {
                StartTime = DateTime.Parse(value);
            }
        }

        [Required]
        [NoPastDate(ErrorMessage = "Date must be a future date")]
        public DateTime Date { get; set; }


        [NotMapped]
        public int Age
        {
            get
            {
                return (int)(Date - DateTime.Now).TotalDays;

            }
        }

        [NotMapped]
        public int Sort
        {
            get
            {
                return (int)(DateTime.Now - Date).TotalDays;

            }
        }



        [Required]
        [MinLength(10)]
        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        public String Seller { get; set; }


        // public User Bidder { get; set; }

        // public RSVP rsvps { get; set; }

        public List<RSVP> RSVPs { get; set; }



        public User Creator { get; set; }



    }
    public class NoPastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if ((DateTime)value <= DateTime.Now)
                return new ValidationResult("Date must be in the future");
            return ValidationResult.Success;
        }
    }


}