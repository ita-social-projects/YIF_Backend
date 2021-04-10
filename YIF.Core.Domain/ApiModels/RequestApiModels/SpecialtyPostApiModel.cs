namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class SpecialtyPostApiModel
    {
        /// <summary>
        /// Name
        /// </summary>     
        /// <example>Комп'ютерні науки</example>
        public string Name { get; set; }
        /// <summary>
        /// Direction Id
        /// </summary>     
        /// <example>914b5a3e-5c11-4e9a-91da-ccc7caf50111</example>
        public string DirectionId { get; set; }
        /// <summary>
        /// Description
        /// </summary>     
        /// <example>Це базовий опис спеціальності. Ця спеціальність підійде для тих хто хоче реалізувати себе у майбутньому у даній галузі. Для здобувачів вищої освіти вона буде цікавою тому що вони зможуть розкрити себе у даному напрямку за рахунок актуальної інформації, яку будуть доносити ним професіонали своєї справи, які є майстрами у своїй галузі.</example>
        public string Description { get; set; }
        /// <summary>
        /// Code
        /// </summary>     
        /// <example>053</example>
        public string Code { get; set; }
    }
}
