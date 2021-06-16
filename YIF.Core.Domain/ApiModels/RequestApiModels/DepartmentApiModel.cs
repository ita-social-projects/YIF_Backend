namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class DepartmentApiModel
    {
        /// <summary>
        /// Department name
        /// </summary>     
        /// <example>Охорони праці</example>
        public string Name { get; set; }

        /// <summary>
        /// Department description
        /// </summary>     
        /// <example>Згідно з Законом України "Про охорону праці" ст. 15 служба охорони праці створена в.о. ректора університету для організації виконання правових, організаційно-технічних, санітарно-гігієнічних, соціально-економічних і лікувально-профілактичних заходів, спрямованих на запобігання нещасним випадкам, професійним захворюванням і аваріям у процесі праці.</example>
        public string Description { get; set; }
    }
}
