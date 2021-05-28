namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class DirectionPostApiModel
    {
        /// <summary>
        /// Name
        /// </summary>     
        /// <example>Інформаційні технології</example>
        public string Name { get; set; }
        /// <summary>
        /// Description
        /// </summary>     
        /// <example>Базовий опис напряму</example>
        public string Description { get; set; }
        /// <summary>
        /// Code
        /// </summary>     
        /// <example>12</example>
        public string Code { get; set; }
    }
}
