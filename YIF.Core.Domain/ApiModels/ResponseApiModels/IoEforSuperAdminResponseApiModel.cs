using System.Text.Json.Serialization;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class IoEforSuperAdminResponseApiModel
    {
        /// <summary>
        /// Unique id
        /// </summary>
        /// <example>e2bd4ad9-060b-4d53-8222-9f3e5efbcfc7</example>
        public string IoEId { get; set; }

        /// <summary>
        /// Name of institutionOfEducation
        /// </summary>
        /// <example>Національний університет водного господарства та природокористування</example>
        public string Name { get; set; }

        /// <summary>
        /// Short name of institutionOfEducation
        /// </summary>
        /// <example>НУВГП</example>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Site of institutionOfEducation
        /// </summary>
        /// <example>https://nuwm.edu.ua/</example>
        public string Site { get; set; }

        /// <summary>
        /// Address of institutionOfEducation
        /// </summary>
        /// <example>вулиця Соборна, 11, Рівне, Рівненська область, 33000</example>
        public string Address { get; set; }

        /// <summary>
        /// Phone of institutionOfEducation
        /// </summary>
        /// <example>380362633209</example>
        public string Phone { get; set; }

        /// <summary>
        /// AdminEmail of institutionOfEducation
        /// </summary>
        /// <example>mail@nuwm.edu.ua</example>
        public string Email { get; set; }

        /// <summary>
        /// Description of institutionOfEducation
        /// </summary> 
        /// <example>Єдиний в Україні вищий навчальний заклад водогосподарського профілю. Заклад є навчально-науковим комплексом, що здійснює підготовку висококваліфікованих фахівців, науково-педагогічних кадрів, забезпечує підвищення кваліфікації фахівців та проводить науково-дослідну роботу.</example>
        public string Description { get; set; }

        /// <summary>
        /// Latitude of institutionOfEducation
        /// </summary>
        /// <example>50.61798</example>
        public float Lat { get; set; }

        /// <summary>
        /// Longitude of institutionOfEducation
        /// </summary>
        /// <example>26.258654</example>
        public float Lon { get; set; }

        /// <summary>
        /// Path of the institutionOfEducation image in project directory
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Email of the admin of IoE
        /// </summary>
        public string AdminEmail { get; set; }

        /// <summary>
        /// IoEAdmin's Id
        /// </summary>
        public string AdminId { get; set; }

        /// <summary>
        /// Type of institution Of Education
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public InstitutionOfEducationType InstitutionOfEducationType { get; set; }
    }
}
