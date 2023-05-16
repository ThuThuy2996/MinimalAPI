using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendMailApi.Commons.Models;
using SendMailApi.Interface;
using System.Threading.Tasks.Dataflow;
using Microsoft.Extensions.DependencyInjection;

namespace SendMailApi.Provider
{
    public class EmailHostedService : IHostedService, IDisposable
    {
        private Task? _sendTask;
        private CancellationTokenSource? _cancellationTokenSource;
        private readonly BufferBlock<EmailModel> _mailQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMailSender _mailSender;

        public EmailHostedService(IServiceScopeFactory serviceScopeFactory)
        {

            _mailSender = new EmailService();
            _serviceScopeFactory = serviceScopeFactory;
            _cancellationTokenSource = new CancellationTokenSource();
            _mailQueue = new BufferBlock<EmailModel>();
        }
        public void Dispose()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }
            throw new NotImplementedException();
        }

         Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            _sendTask = BackgroundSendEmailAsync(_cancellationTokenSource!.Token);
            return Task.CompletedTask;
        }

        async Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource = null;
            }
            await Task.WhenAny(_sendTask!, Task.Delay(Timeout.Infinite, cancellationToken));

        }

        public async Task<bool> SendEmailAsync(EmailModel emailWithAddress)
        {
           return await _mailQueue.SendAsync(emailWithAddress);
        }


        private async Task BackgroundSendEmailAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var email = await _mailQueue.ReceiveAsync();
                    await _mailSender.SendEmail(email);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[BACKGROUND EMAIL SERVICE] {ex.Message}", "EmailHostedService");
                }
                Console.WriteLine("[BACKGROUND EMAIL SERVICE] END SEND", "EmailHostedService");
            }
        }
    }
}
