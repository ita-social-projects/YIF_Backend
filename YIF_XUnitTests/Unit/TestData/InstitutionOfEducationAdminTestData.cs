using System.Collections.Generic;
using YIF.Core.Domain.ApiModels.IdentityApiModels;
using YIF.Core.Domain.ApiModels.RequestApiModels;
using YIF.Core.Domain.ApiModels.ResponseApiModels;
using YIF.Core.Domain.DtoModels.EntityDTO;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.EntityForResponse;

namespace YIF_XUnitTests.Unit.TestData
{
    public static class InstitutionOfEducationAdminTestData
    {
        public static InstitutionOfEducationAdminSortingModel GetEmptyInstitutionOfEducationAdminSortingModel()
        {
            return new InstitutionOfEducationAdminSortingModel
            {
                UserName = null,
                Email = null,
                InstitutionOfEducationName = null,
                IsBanned = null
            };
        }

        public static IEnumerable<InstitutionOfEducationAdminDTO> GetIEnumerableInstitutionOfEducationAdminDTO()
        {
            return new List<InstitutionOfEducationAdminDTO>
            {
                new InstitutionOfEducationAdminDTO
                {
                    Id = "1",
                    IsBanned = false,
                    UserId = "1",
                    User = new UserDTO()
                    {
                        Id = "1",
                        UserName = "UserName1",
                        Email = "Email1"
                    },
                    InstitutionOfEducation = new InstitutionOfEducationDTO()
                    {
                        Name = "InstitutionOfEducation1",
                    }
                },
                new InstitutionOfEducationAdminDTO
                {
                    Id = "2",
                    IsBanned = false,
                    UserId = "2",
                    User = new UserDTO()
                    {
                        Id = "2",
                        UserName = "UserName2",
                        Email = "Email2"
                    },
                    InstitutionOfEducation = new InstitutionOfEducationDTO()
                    {
                        Name = "InstitutionOfEducation2",
                    }
                },
                new InstitutionOfEducationAdminDTO
                {
                    Id = "3",
                    IsBanned = false,
                    UserId = "3",
                    User = new UserDTO()
                    {
                        Id = "3",
                        UserName = "UserName3",
                        Email = "Email3"
                    },
                    InstitutionOfEducation = new InstitutionOfEducationDTO()
                    {
                        Name = "InstitutionOfEducation3",
                    }
                }
            };
        }

        public static IEnumerable<InstitutionOfEducationAdminResponseApiModel> GetInstitutionOfEducationAdminResponseApiModels()
        {
            return new List<InstitutionOfEducationAdminResponseApiModel>
            {
                new InstitutionOfEducationAdminResponseApiModel
                {
                    Id = "1",
                    IsBanned = false,
                    User = new UserForInstitutionOfEducationAdminResponseApiModel()
                    {
                        UserName = "UserName1",
                        Email = "Email1"
                    },
                    InstitutionOfEducation = new InstitutionOfEducationForInstitutionOfEducationAdminResponseApiModel()
                    {
                        Name = "InstitutionOfEducation1",
                    }
                },
                new InstitutionOfEducationAdminResponseApiModel
                {
                    Id = "2",
                    IsBanned = false,
                    User = new UserForInstitutionOfEducationAdminResponseApiModel()
                    {
                        UserName = "UserName2",
                        Email = "Email2"
                    },
                    InstitutionOfEducation = new InstitutionOfEducationForInstitutionOfEducationAdminResponseApiModel()
                    {
                        Name = "InstitutionOfEducation2",
                    }
                },
                new InstitutionOfEducationAdminResponseApiModel
                {
                    Id = "3",
                    IsBanned = false,
                    User = new UserForInstitutionOfEducationAdminResponseApiModel()
                    {
                        UserName = "UserName3",
                        Email = "Email3"
                    },
                    InstitutionOfEducation = new InstitutionOfEducationForInstitutionOfEducationAdminResponseApiModel()
                    {
                        Name = "InstitutionOfEducation3",
                    }
                }
            };
        }
    }
}
