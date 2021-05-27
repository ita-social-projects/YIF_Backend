namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class ChangePasswordApiModel
    {
        /// <summary>
        /// User id
        /// </summary>
        /// <example>a47e79d8-7e68-43c5-a5c9-1c6dff1e1693</example>
        public string UserId { get; set; }
        /// <summary>
        /// User old password
        /// </summary>
        /// <example>QWerty-1</example>
        public string OldPassword { get; set; }
        /// <summary>
        /// User new password
        /// </summary>
        /// <example>QWerty-12</example>
        public string NewPassword { get; set; }
        /// <summary>
        /// Confirm new password
        /// </summary>
        /// <example>QWerty-12</example>
        public string ConfirmNewPassword { get; set; }
        /// <summary>
        /// Google Recaptcha Token
        /// </summary>
        /// <example>03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs</example>
        public string RecaptchaToken { get; set; }
    }
}
