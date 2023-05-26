using System.ComponentModel.DataAnnotations;

namespace ExtraDisciplines.Models
{
    public class Enrollment
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string StudentId { get; set; }

        [Range(0, 100)]
        public int Score { get; set; }

        public bool IsPassed { get; set; }

        //public virtual Course Course { get; set; }

        public virtual Student Student { get; set; }
    }
}
