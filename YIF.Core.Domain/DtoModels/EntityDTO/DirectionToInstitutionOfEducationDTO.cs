namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class DirectionToInstitutionOfEducationDTO
    {
        public string DirectionId { get; set; }
        public string InstitutionOfEducationId { get; set; }

        public DirectionDTO Direction { get; set; }
        public InstitutionOfEducationDTO InstitutionOfEducation { get; set; }
    }
}
