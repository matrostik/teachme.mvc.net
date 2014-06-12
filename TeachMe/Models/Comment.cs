using System;
using System.ComponentModel.DataAnnotations;

namespace TeachMe.Models
{
    /// <summary>
    /// Comment for teacher profile
    /// </summary>
    public class Comment
    {
        #region Fields

        public int Id { get; set; }

        [Required(ErrorMessage = "* יש להכניס את שמך")]
        [Display(Name = "שם")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "* יש להכניס תגובה")]
        [Display(Name = "תגובה")]
        public string CommentText { get; set; }

        public int TeacherId { get; set; }

        public DateTime Date { get; set; }

        #endregion
    }
}