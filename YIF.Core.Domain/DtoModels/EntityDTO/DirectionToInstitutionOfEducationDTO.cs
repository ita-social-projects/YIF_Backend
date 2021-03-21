namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class DirectionToInstitutionOfEducationDTO
    {
        public string DirectionId { get; set; }
        public string InstitutionOfEducationId { get; set; }

        public virtual DirectionDTO Direction { get; set; }
        public virtual InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
    }
}
