namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class IoEAdminAddFromModeratorsApiModel
    {
        /// <summary>
        /// Id of moderator chosen to be an admin
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string UserId { get; set; }

        /// <summary>
        /// Id of Institution of Education
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string IoEId { get; set; }
    }
}
