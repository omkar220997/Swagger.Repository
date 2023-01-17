using System;

namespace CourseLibrary.API.Models
{
    public class AuthorWithConcatenatedName
    {
        public Guid Id { get; set; }

        public string AuthorName { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

        public string MainCategory { get; set; }

    }
}
