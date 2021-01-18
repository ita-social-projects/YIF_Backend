using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    /// <summary>
    /// Class for paginated response
    /// </summary>
    /// <typeparam name="T">The class that is the element of the response collection.</typeparam>
    public class PageResponseApiModel<T>
    {
        /// <summary>
        /// Current page number(from 0 to total pages)
        /// </summary>
        /// <example>2</example>
        [Required]
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total number of pages
        /// </summary>
        /// <example>3</example>
        [Required]
        public int TotalPages { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        /// <example>10</example>
        [Required]
        public int PageSize { get; set; }

        /// <summary>
        /// Link to next page
        /// </summary>
        /// <example>https://yifbackend.tk/api/University?page=3</example>
        public string NextPage { get; set; }

        /// <summary>
        /// Link to previous page
        /// </summary>
        /// <example>https://yifbackend.tk/api/University?page=1</example>
        public string PrevPage { get; set; }

        /// <summary>
        /// Element collection
        /// </summary>
        [Required]
        public IEnumerable<T> ResponseList { get; set; }
    }
}
