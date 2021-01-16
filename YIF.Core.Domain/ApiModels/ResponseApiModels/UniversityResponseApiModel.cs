using System;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class UniversityResponseApiModel
    {
        public string Id { get; set; }
        /// <summary>
        /// Name of university
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Short name of university
        /// </summary>
        public string Abbreviation { get; set; }
        /// <summary>
        /// Site of university
        /// </summary>
        public string Site { get; set; }
        /// <summary>
        /// Address of university
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Phone of university
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Email of university
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Description of university
        /// </summary> 
        public string Description { get; set; }
        /// <summary>
        /// Path of the university image in project directory
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Latitude of university
        /// </summary>
        public float Lat { get; set; }
        /// <summary>
        /// Longitude of university
        /// </summary>
        public float Lon { get; set; }
        /// <summary>
        /// Start date of the entrance campaign
        /// </summary>
        public DateTime StartOfCampaign { get; set; }
        /// <summary>
        /// End date of the entrance campaign
        /// </summary>
        public DateTime EndOfCampaign { get; set; }
    }
}
