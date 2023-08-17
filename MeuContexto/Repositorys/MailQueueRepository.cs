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

        public void AddNewMailQueue(MailQueue mailQueue)
        {
            _repository.SaveEntity<MailQueue>(mailQueue);
        }

        public void AddNewMailQueue(string toEmail, string message, string subject, string body)
        {            
            _repository.SaveEntity(new MailQueue(toEmail,message,subject,body));
        }
    }
}
