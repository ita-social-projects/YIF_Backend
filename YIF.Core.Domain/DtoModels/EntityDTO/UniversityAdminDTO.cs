namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class UniversityAdminDTO
    {
        public string Id { get; set; }
        public string UniversityId { get; set; }
        public string UniversityName { get; set; }
        public UniversityDTO University { get; set; }
        /// <summary>
        /// Link to university moderator
        /// </summary>
        public UniversityModeratorDTO Moderator { get; set; }
    }
}
