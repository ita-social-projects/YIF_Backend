using System;
using System.Collections.Generic;
using System.Text;
using YIF.Core.Domain.DtoModels.IdentityDTO;
using YIF.Core.Domain.Models.IdentityDTO;

namespace YIF.Core.Domain.DtoModels.EntityDTO
{
    public class LectureDTO
    {
        public string Id { get; set; }
        public string UniversityId { get; set; }

        //.....

        public UniversityDTO University { get; set; }
        /// <summary>
        /// Link to Identity user
        /// </summary>
        public UserDTO User { get; set; }
    }
}
