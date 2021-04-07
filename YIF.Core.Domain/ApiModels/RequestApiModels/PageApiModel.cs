namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class PageApiModel
    {
        /// <summary>
        /// Number of page
        /// </summary>
        /// <example>10</example>
        public int Page { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        /// <example>api/Users/GetAll</example>
        public string Url { get; set; }
    }
}
