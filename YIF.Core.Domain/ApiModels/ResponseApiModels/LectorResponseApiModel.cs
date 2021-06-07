using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class LectorResponseApiModel
    {
        /// <summary>
        /// Id of lector
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string UserId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        /// <example>example@gmail.com</example>
        public string Email { get; set; }
        /// <summary>
        /// Id of Institution of Education
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string IoEId { get; set; }
    }
}
