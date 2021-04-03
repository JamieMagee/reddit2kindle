using System.ComponentModel.DataAnnotations;

namespace Reddit2Kindle.Blazor.Contracts
{
    public class SubmitPostForm
    {
        [Required]
        public string Post { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Domain { get; set; } = "@kindle.com";
    }
}
