namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class DirectionToInstitutionOfEducationDTO
    {
        /// <summary>
        /// Id of direction
        /// </summary>
        public string DirectionId { get; set; }
        /// <summary>
        /// Id of institutionOfEducation
        /// </summary>
        public string InstitutionOfEducationId { get; set; }

        public virtual DirectionDTO Direction { get; set; }
        public virtual InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
    }
}
