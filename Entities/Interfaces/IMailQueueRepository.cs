﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMailQueueRepository
    {
        public Task AddNewMailQueueAsync(string toEmail, string message, string subject, string body);
    }
}
