using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class RestoreSuperAdminApiModel
    {
        /// <summary>
        /// UserID by which user will be restored
        /// </summary>
        public string UserId { get; set; }
    }
}
