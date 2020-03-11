namespace Blog.Core.Emails
{
    using System.Collections.Generic;
    public class Email
    {
        public Email()
        {
            Attachments = new List<byte[]>();
        }

        public string Body { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public List<byte[]> Attachments { get; set; }
    }
}
