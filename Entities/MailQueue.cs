using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MailQueue
    {
        public long EmailQueueId { get; set; }
        public string ToEmail { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public MailQueueStatus Status { get; set; }
        public DateTime NextSend { get; set; }
        public int NumberOfTries { get; set; }

        public MailQueue(string toEmail, string message, string subject, string body)
        {
            ToEmail = toEmail;
            Message = message;
            Subject = subject;
            Body = body;
            Status = MailQueueStatus.Pending;
            NextSend = DateTime.Now;
            NumberOfTries = 0;
        }
    }
}
