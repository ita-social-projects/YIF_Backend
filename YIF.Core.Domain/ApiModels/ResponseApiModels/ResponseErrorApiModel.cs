using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class ResponseErrorApiModel
    {
        /// <summary>
        /// Gets or sets error for response result.
        /// </summary>
        /// <example>Поле є обов'язковим.</example>
        [Required]
        public string Message { get; set; }
    }
}
