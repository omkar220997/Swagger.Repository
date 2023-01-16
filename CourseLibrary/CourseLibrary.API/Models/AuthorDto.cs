using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Models
{
    /// <summary>
    /// An Author with Id, Name, Age and Category fields
    /// 
    /// </summary>
    public class AuthorDto
    {
        /// <summary>
        /// The Id of the Author
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name of the author
        /// </summary>
        public string Name { get; set; } 
        /// <summary>
        /// Age of Author
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Category of AAuthor
        /// </summary>
        public string MainCategory { get; set; }
    }
}
