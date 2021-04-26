using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YIF.Core.Data.Entities;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class InstitutionOfEducationPostApiModel
    {
        /// <summary>
        /// Name of Institution of Education
        /// </summary>
        /// <example>Kyiv Polytechnic University</example>
        public string Name { get; set; }

        /// <summary>
        /// Abbreviation of Institution of Education
        /// </summary>
        /// <example>KPI</example>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Link to the official site of institution of education
        /// </summary>
        /// <example>kpi.ua</example>
        public string Site { get; set; }

        /// <summary>
        /// Address of the institution of education
        /// </summary>
        /// <example>37, Prosp.Peremohy, Solomyanskyi district, Kyiv, Ukraine, 03056.</example>
        public string Address { get; set; }

        /// <summary>
        /// Phone number of the institution of education
        /// </summary>
        /// <example>+380442049494</example>
        public string Phone { get; set; }

        /// <summary>
        /// Official email of institution of education
        /// </summary>
        /// <example>mail@kpi.ua</example>
        public string Email { get; set; }

        /// <summary>
        /// Description of institution of education
        /// </summary>
        /// <example>This is description of institution of education.</example>
        public string Description { get; set; }

        public float Lat { get; set; }

        public float Lon { get; set; }

        /// <summary>
        /// Type of institution of education
        /// </summary>     
        /// <example>University</example>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public InstitutionOfEducationType InstitutionOfEducationType { get; set; }

        /// <summary>
        /// Image of institution of education
        /// </summary>
        public virtual ImageApiModel ImageApiModel { get; set; }
    }
}
