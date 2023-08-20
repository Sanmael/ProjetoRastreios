using Entities;
using Entities.Interfaces;

namespace MeuContexto.Repositorys
{
    public class MailQueueRepository : IMailQueueRepository
    {
        private readonly IRepository _repository;

        public MailQueueRepository(IRepository repository)
        {
            _repository = repository;
        }

        public async void AddNewMailQueue(MailQueue mailQueue)
        {
            await _repository.SaveEntityAsync<MailQueue>(mailQueue);
        }

        public async Task AddNewMailQueueAsync(string toEmail, string message, string subject, string body)
        {
            await _repository.SaveEntityAsync(new MailQueue(toEmail,message,subject,body));
        }
    }
}
