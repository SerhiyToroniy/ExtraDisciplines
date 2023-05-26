using System.ComponentModel.DataAnnotations;

namespace ExtraDisciplines.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Maximum Capacity")]
        public int MaxCapacity { get; set; }

        public int AlreadyEnrolledCount { get; set; }

        public virtual ICollection<Enrollment>? Enrollments { get; set; }
    }
}