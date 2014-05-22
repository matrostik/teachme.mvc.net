
namespace TeachMe.Models
{
    /// <summary>
    /// Subject to teach
    /// (related to teacher)
    /// </summary>
    public class SubjectToTeach
    {

        public int Id { get; set; }
        public int SubjectId { get; set; }

        public int TeacherId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

    }
}