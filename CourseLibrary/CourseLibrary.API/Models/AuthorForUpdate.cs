using System;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    /// <summary>
    /// Author for update with firstname and lastname fields
    /// </summary>
    public class AuthorForUpdate
    {
        /// <summary>
        /// The Id  of Author
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// The First Name of Author
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// the Last Name of Author
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
    }
}
