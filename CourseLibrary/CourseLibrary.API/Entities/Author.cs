using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Entities
{
    /// <summary>
    /// Author with Id, FirstName, LastName,DateOfBirth and MainCategory fields
    /// </summary>
    public class Author
    {
        /// <summary>
        /// The Id of the Author
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// First Name of the author
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the author
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Date of birth of author
        /// </summary>
        [Required]
        public DateTimeOffset DateOfBirth { get; set; }          

        /// <summary>
        /// Category if the author
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string MainCategory { get; set; }

        public ICollection<Course> Courses { get; set; }
            = new List<Course>();
    }
}
