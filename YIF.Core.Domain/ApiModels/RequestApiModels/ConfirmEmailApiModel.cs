using System;
using System.Collections.Generic;
using System.Text;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class ConfirmEmailApiModel
    {
        /// <summary>
        /// User id
        /// </summary>
        /// <example>f1db5e6e-3570-4bd7-a0a6-7c7fa94e7f10</example>
        public string Id { get; set; }
        /// <summary>
        /// Token for email confirm
        /// </summary>
        /// <example>CfDJ8IBKFkxEPOdInaRMJmhKUK+Lmr7g+QCcIVRDS/suDF0eEnOFTynP/CYfw999+SrQN43OQEirQZttrcrC+xYYs/h51TRFQ67QNthG9V6Qj/izHXYm0mDK6zA4wg89p41PoEADHFFFvOhD8X8g8KC+EFNBsn5kuVuXHXrg3F0rTMnhaQD0TjRT6HWwaaKrE2bzAiuvYMUu4+wb+FfFZbqctwlqgHi3XAcwXjNC9Qc5zI69A0Pbj6llDNHnUUr+xSZQPA==</example>
        public string Token { get; set; }
    }
}
