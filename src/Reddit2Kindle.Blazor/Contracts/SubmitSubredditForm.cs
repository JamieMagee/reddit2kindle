using System.ComponentModel.DataAnnotations;
using Reddit2Kindle.Contracts;

namespace Reddit2Kindle.Blazor.Contracts
{
    public class SubmitSubredditForm

    {
        [Required]
        public string Subreddit { get; set; }

        public TimePeriod TimePeriod { get; set; } = TimePeriod.Week;

        [Required]
        public string Email { get; set; }

        [Required]
        public string Domain { get; set; } = "@kindle.com";
    }
}
