using System;

namespace Reddit2Kindle.Contracts
{
    public class PostRequest : Request
    {
        public Uri Post { get; set; }
    }
}
