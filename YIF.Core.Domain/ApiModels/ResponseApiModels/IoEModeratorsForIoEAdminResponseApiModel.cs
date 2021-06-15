namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class IoEModeratorsForIoEAdminResponseApiModel
    {
        /// <summary>
        /// Moderator Id
        /// </summary>
        public string ModeratorId { get; set; } 
        /// <summary>
        /// User Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Is banned
        /// </summary>
        public string IsBanned { get; set; }
    }
}
