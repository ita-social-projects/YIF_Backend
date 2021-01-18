using System;
using System.Collections.Generic;
using System.Text;
using YIF.Core.Domain.DtoModels.EntityDTO;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class UniversityPostApiModel
    {

        public string Name { get; set; }
        /// <summary>
        /// Short name of university
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Description of university
        /// </summary> 
        public string Description { get; set; }
    }
}
