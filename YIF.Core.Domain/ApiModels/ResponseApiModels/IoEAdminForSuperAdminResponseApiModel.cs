namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class IoEAdminForSuperAdminResponseApiModel
    {
        /// <summary>
        /// Primary key for user.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Primary key for IoEAdmin.
        /// </summary>
        public string IoEAdminId { get; set; }

        /// <summary>
        /// IoEAdmin email
        /// </summary>
        /// <example>mail@nuwm.edu.ua</example>
        public string Email { get; set; }

        /// <summary>
        /// Status of banning
        /// </summary>
        public bool IsBanned { get; set; }
    }
}
