namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class DisciplinePostApiModel
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
        /// <example>162f77bf-b330-45a0-983f-6d0ef31af348</example>
        public string LectorId { get; set; }

        /// <summary>
        /// Speciality Id
        /// </summary>     
        /// <example>02d752e1-4f62-49fa-b88a-8f2d4d7cd722</example>
        public string SpecialityId { get; set; }
    }
}
