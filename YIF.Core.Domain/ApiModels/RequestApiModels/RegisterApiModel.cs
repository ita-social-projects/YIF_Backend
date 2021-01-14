using System.ComponentModel.DataAnnotations;

namespace YIF.Core.Domain.ApiModels.RequestApiModels
{
    public class RegisterApiModel
    {
        /// <summary>
        /// User email address. Used for login.
        /// </summary>     
        /// <example>test333@gmail.com</example>
        [Required]
        public string Email { get; set; }

        /// <summary>
        /// Unique username.
        /// </summary>     
        /// <example>TestName</example>
        [Required]
        public string Username { get; set; }

        /// <summary>
        /// User password. Must have at least one non alphanumeric character.
        /// 
        /// </summary>     
        /// <example>QWerty-1</example>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Repeat user password. Must match password.
        /// </summary>     
        /// <example>QWerty-1</example>
        [Required]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Google Recaptcha Token
        /// </summary>
        /// <example>03AGdBq25YLH-yC_93jfCWQBUm3bGFwnZBh1vyA4KmSeqtYlfDD7sgCHy9LxnYwqGpQPOTRIwkCbCoG2ZGQlPyHuwKZaEXZU3L9R_Oel8J_mJsVHJReRn9tDXinrw6uXG16Abgc-UoTW_DoBNFA8ScJ0W97TR2ThYB0Mh1dO-wv0JLUknKA5Dubvb5jLvsgx4QKtiNUNexXQxHP-LBUaJFIGwg1QD_5DVJ4HzXlGRrDBCQhBkvuew9znk-EnLvyP1bpUXfix2T1lVTxwFNNw-yiLWZFXZIzCt2JrreEOSmImE-7eQKguD27-xu4qkmGDZSMyyB8w8WrvkLYnglNxWbWSscZg0jbEF-NQMB3NW-Z2KytnOg7TocV-fxf11OjEu2H0rcmMLNk7s9yLOOPnJlO-C8t2SeaLu99XFkFWN5AVTV-ikReaX0wWTS8edKD5rAdIbMNeZugFLs</example>
        public string RecaptchaToken { get; set; }
    }
}