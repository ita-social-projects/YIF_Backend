using YIF.Core.Data.Entities;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF_XUnitTests.Unit.TestData
{
    public static class InstitutionOfEducationTestData
    {
        public static InstitutionOfEducationDTO GetInstitutionOfEducationDTO()
        {
            return new InstitutionOfEducationDTO()
            {
                Id = "Id",
                Name = "Name",
                Abbreviation = "Abbreviation",
                Description = "Description",
                Email = "Email@gmail.com",
                Phone = "3801234567",
                Site = "https://www.localHost.com/"
            };
        }

        public static InstitutionOfEducation GetInstitutionOfEducation()
        {
            return new InstitutionOfEducation()
            {
                Id = "Id",
                Name = "Name",
                Abbreviation = "Abbreviation",
                Description = "Description",
                Email = "Email@gmail.com",
                Phone = "3801234567",
                Site = "https://www.localHost.com/"
            };
        }
    }
}
