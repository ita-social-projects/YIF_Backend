using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.DtoModels.SchoolModerator
{
    public class SchoolModeratorDTO
    {
        public string Id { get; set; }
        public string SchoolId { get; set; }
        public string AdminId { get; set; }
        public string UserId { get; set; }
    }
}
