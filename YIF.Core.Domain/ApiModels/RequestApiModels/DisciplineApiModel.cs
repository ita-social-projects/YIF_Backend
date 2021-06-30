namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class DisciplineApiModel
    {
        /// <summary>
        /// Discipline name
        /// </summary>     
        /// <example>Дискретна математика</example>
        public string Name { get; set; }

        /// <summary>
        /// Discipline description
        /// </summary>     
        /// <example>Дискре́тна матема́тика — галузь математики, що вивчає властивості будь-яких дискретних структур.</example>
        public string Description { get; set; }

        /// <summary>
        /// Lector id
        /// </summary>     
        /// <example>914b5a3e-5c11-4e9a-91da-ccc7caf50111</example>
        public string LectorId { get; set; }

        /// <summary>
        /// Speciality Id
        /// </summary>     
        /// <example>914b5a3e-5c11-4e9a-91da-ccc7caf50111</example>
        public string SpecialityId { get; set; }
    }
}
