using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachMe.Models
{
    /// <summary>
    /// Comment for teacher profile
    /// </summary>
    public class Comment
    {
        #region Fields

        public int Id { get; set; }

        public string AuthorName { get; set; }

        public string CommentText { get; set; }

        public int TeacherId { get; set; }

        public DateTime Date { get; set; }

        #endregion
    }
}