namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class IoEAdminForIoEModeratorResponseApiModel
    {
        /// <summary>
        /// AdminEmail of institutionOfEducation
        /// </summary>
        /// <example>mail@nuwm.edu.ua</example>
        public string Email { get; set; }
        /// <summary>
        /// AdminName of institutionOfEducation
        /// </summary>
        /// <example>Petro</example>
        public string Name { get; set; }
    }
}
