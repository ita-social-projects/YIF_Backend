namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class IoEInfromationResponseApiModel
    {
        /// <summary>
        /// Unique id
        /// </summary>
        /// <example>e2bd4ad9-060b-4d53-8222-9f3e5efbcfc7</example>
        public string Id { get; set; }

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
    }
}
