using System;

namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class UniversityResponseApiModel
    {
        /// <summary>
        /// Unique id
        /// </summary>
        /// <example>e2bd4ad9-060b-4d53-8222-9f3e5efbcfc7</example>
        public string Id { get; set; }

        /// <summary>
        /// Name of university
        /// </summary>
        /// <example>Національний університет водного господарства та природокористування</example>
        public string Name { get; set; }

        /// <summary>
        /// Short name of university
        /// </summary>
        /// <example>НУВГП</example>
        public string Abbreviation { get; set; }

        /// <summary>
        /// Site of university
        /// </summary>
        /// <example>https://nuwm.edu.ua/</example>
        public string Site { get; set; }

        /// <summary>
        /// Address of university
        /// </summary>
        /// <example>вулиця Соборна, 11, Рівне, Рівненська область, 33000</example>
        public string Address { get; set; }

        /// <summary>
        /// Phone of university
        /// </summary>
        /// <example>380362633209</example>
        public string Phone { get; set; }

        /// <summary>
        /// Email of university
        /// </summary>
        /// <example>mail@nuwm.edu.ua</example>
        public string Email { get; set; }

        /// <summary>
        /// Description of university
        /// </summary> 
        /// <example>Єдиний в Україні вищий навчальний заклад водогосподарського профілю. Заклад є навчально-науковим комплексом, що здійснює підготовку висококваліфікованих фахівців, науково-педагогічних кадрів, забезпечує підвищення кваліфікації фахівців та проводить науково-дослідну роботу.</example>
        public string Description { get; set; }

        /// <summary>
        /// Path of the university image in project directory
        /// </summary>
        /// <example></example>
        public string ImagePath { get; set; }

        /// <summary>
        /// Latitude of university
        /// </summary>
        /// <example>50.61798</example>
        public float Lat { get; set; }

        /// <summary>
        /// Longitude of university
        /// </summary>
        /// <example>26.258654</example>
        public float Lon { get; set; }

        /// <summary>
        /// Start date of the entrance campaign
        /// </summary>
        /// <example>2021-08-13T00:00:00</example>
        public DateTime StartOfCampaign { get; set; }

        /// <summary>
        /// End date of the entrance campaign
        /// </summary>
        /// <example>2021-08-13T00:00:00</example>
        public DateTime EndOfCampaign { get; set; }

        /// <summary>
        /// Is the university favorite
        /// </summary>
        /// <example>true</example>
        public bool IsFavorite { get; set; }
    }
}
