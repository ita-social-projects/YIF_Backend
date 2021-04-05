namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class SchoolAdminApiModel
    {
        /// <summary>
        /// School name
        /// </summary>
        /// <example>School 12</example>
        public string SchoolName { get; set; }

        /// <summary>
        /// School email
        /// </summary>
        /// <example>rivne.nvk12@gmail.com</example>
        public string Email { get; set; }

        /// <summary>
        /// School admin password
        /// </summary>
        /// <example>QWerty-12</example>
        public string Password { get; set; }
    }
}
