using Entities;
using MeuContexto.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SendTrackingMail.Service
{
    internal class SendTrackingCode
    {
        private readonly IUnitOfWork _unitOfWork;

        public SendTrackingCode(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task GetMailQueuesAsync()
        {
            List<TrackingCode> codes = await _unitOfWork.TrackingService.GetTrackingCodeActiveAsync();

            //refatorar depois
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 10;

            Parallel.ForEach(codes, parallelOptions, code =>
            {
                //ProcessTrackingCodeAsync(code);
            });
        }

        private async Task SendMailQueueAsync(MailQueue emailQueue)
        {
            try
            {
                string fromEmail = "seuemail@gmail.com";
                
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential(fromEmail, "suaSenha");
                    smtpClient.EnableSsl = true;

                    using (var message = new MailMessage(fromEmail, emailQueue.ToEmail, emailQueue.Subject, emailQueue.Body))
                    {
                        await smtpClient.SendMailAsync(message);
                        emailQueue.Status = MailQueueStatus.Send;
                    }
                }
            }
            catch (Exception ex)
            {
                emailQueue.Status = MailQueueStatus.Error;
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
        }
    }
}
