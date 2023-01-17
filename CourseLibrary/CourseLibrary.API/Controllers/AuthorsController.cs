using AutoMapper;
using CourseLibrary.API.Attributes;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Helpers;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Controllers
{
   
    [ApiController]
    [Route("api/authors")]
    //[ApiExplorerSettings(GroupName = "CourseLibraryOpenApiSpecificationAuthors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository,
            IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors(
            [FromQuery] AuthorsResourceParameters authorsResourceParameters)
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorsResourceParameters);
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));
        }

        //[HttpGet("{authorId}")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[Produces("application/vnd.marvin.bookwithconcatenatedname+json")]
        //public ActionResult<AuthorWithConcatenatedName>GetAuthorWithConcatenstedAuthorName(Guid authorId)
        //{
        //    if (!_courseLibraryRepository.AuthorExists(authorId))
        //    {
        //        return NotFound();
        //    }
        //    var authorFromRepo=_courseLibraryRepository.GetAuthor(authorId);
        //    if(authorFromRepo == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(_mapper.Map<AuthorWithConcatenatedName>(authorFromRepo));
        //}




        /// <summary>
        /// Get an Author by his/her id
        /// </summary>
        /// <param name="authorId">The id of the author you want to get</param> 
        /// <returns>An Author with id, firstname and lastname fields</returns>
        [HttpGet("{authorId}", Name ="GetAuthor")]

        public IActionResult GetAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }
             
            return Ok(_mapper.Map<AuthorForUpdate>(authorFromRepo));
        }
        /// <summary>
        /// Update an Author by his/her Id
        /// </summary>
        /// <param name="authorId">The Id of an Author you want to get.</param>
        /// <param name="authorForUpdate">The fields of an author you want to update</param>
        /// <returns>An Author with updated id, firstname and lastname</returns>
        [HttpPut("{authorId}",Name ="PutAuthor")]
        public ActionResult<AuthorForUpdate> UpdateAuthor(Guid authorId,AuthorForUpdate authorForUpdate)
        {
            var authorFromRepo= _courseLibraryRepository.GetAuthor(authorId);
            if(authorFromRepo == null)
            {
                return NotFound();
            }
            var authorEntity = _mapper.Map<Entities.Author>(authorForUpdate);
            if (authorEntity == null)
            {
                return NotFound();
            }
            var authorToReturn=_mapper.Map<Models.AuthorForUpdate>(authorEntity);
            _courseLibraryRepository.UpdateAuthor(authorEntity);
            _courseLibraryRepository.Save();
            return CreatedAtRoute("GetAuthor", new { authorId = authorToReturn.Id }, authorToReturn);
        }
        /// <summary>
        /// Partially update the author
        /// </summary>
        /// <param name="authorId">the author id to get</param>
        /// <param name="patchAuthor">The fields of author you want to update</param>
        /// <returns>An ActionResult of type Author </returns>
        /// <remarks>
        /// Sample request (this request updates for author's **FirstName** )   
        ///     PATCH /authors/id   
        ///     [   
        ///         {   
        ///             "op": "replace",   
        ///              "path": "/firstname",   
        ///              "value": "new first name"   
        ///          }   
        ///     ] 
        ///</remarks>
        [HttpPatch("{authorId}")]
        public ActionResult PartiallyUpdateAuthor(Guid authorId, JsonPatchDocument<AuthorForPartiallyUpdateDto> patchAuthor)
        {
            var authorEntity=_mapper.Map<Entities.Author>(patchAuthor);
            if(authorEntity == null)
            {
                return NotFound();
            }
            var authorToReturn= _mapper.Map<Models.AuthorForPartiallyUpdateDto>(authorEntity);
            _courseLibraryRepository.UpdateAuthor(authorEntity);
            _courseLibraryRepository.Save();
            return CreatedAtRoute("GetAuthor", new { authorId }, authorToReturn);

        }

        [HttpPost]
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author)
        {
            var authorEntity = _mapper.Map<Entities.Author>(author);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtRoute("GetAuthor",
                new { authorId = authorToReturn.Id },
                authorToReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            _courseLibraryRepository.DeleteAuthor(authorFromRepo);

            _courseLibraryRepository.Save();

            return NoContent();
        }
    }
}
