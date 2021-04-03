namespace Reddit2Kindle.Contracts
{
    public class SubredditRequest : Request
    {
        public string Subreddit { get; set; }

        public TimePeriod TimePeriod { get; set; } = TimePeriod.Week;
    }
}
