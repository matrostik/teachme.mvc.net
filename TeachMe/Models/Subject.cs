
namespace TeachMe.Models
{
    /// <summary>
    /// Subject  of study
    /// (general info)
    /// </summary>
    public class Subject
    {

        #region Fields

        public int Id { get; set; }

        public string Name { get; set; }

        // maybe list of subtypes separated by coma for example
        public string Description { get; set; }
        
        #endregion

    }
}