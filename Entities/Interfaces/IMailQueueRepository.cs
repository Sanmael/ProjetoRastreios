using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Interfaces
{
    public interface IMailQueueRepository
    {
        public void AddNewMailQueue(string toEmail, string message, string subject, string body);
    }
}
