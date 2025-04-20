using System.ComponentModel.DataAnnotations;

namespace EnhancementAPI.Models
{
    public class TaskDetails
    {
        public Guid ID { get; set; }

        public int SrNo { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }


        public string Status { get; set; }

        public string AssignedTo { get; set; }

        public DateTime DueDate { get; set; }
    }
}
