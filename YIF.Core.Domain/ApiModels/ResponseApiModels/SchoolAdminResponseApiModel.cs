namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SchoolAdminResponseApiModel
    {
        /// <summary>
        /// School admin Id
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string Id { get; set; }
        /// <summary>
        /// School Id
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string SchoolId { get; set; }
        /// <summary>
        /// School name
        /// </summary>
        /// <example>School 12</example>
        public string SchoolName { get; set; }
    }
}
