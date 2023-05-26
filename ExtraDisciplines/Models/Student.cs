using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExtraDisciplines.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public double AverageScore { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
