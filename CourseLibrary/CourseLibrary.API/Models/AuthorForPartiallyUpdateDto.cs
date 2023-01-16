using System;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    /// <summary>
    /// Partially update an author of firstname 
    /// </summary>
    public class AuthorForPartiallyUpdateDto
    {
       
        /// <summary>
        /// First Name of author
        /// </summary>
        [Required(ErrorMessage ="You should have to fill this")]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
      
    }
}
