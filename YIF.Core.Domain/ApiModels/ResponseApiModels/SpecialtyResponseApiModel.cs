﻿namespace YIF.Core.Domain.ApiModels.ResponseApiModels
{
    public class SpecialtyResponseApiModel
    {
        /// <summary>
        /// Gets or sets the primary key for this specialty.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the specialty name for this specialty.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the description for this specialty.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the direction id to which this specialty belongs.
        /// </summary>
        public string DirectionId { get; set; }
        /// <summary>
        /// Gets or sets the direction name to which this specialty belongs.
        /// </summary>
        public string DirectionName { get; set; }
    }
}
