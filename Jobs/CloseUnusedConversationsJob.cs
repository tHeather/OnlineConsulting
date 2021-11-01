using OnlineConsulting.Services.Repositories.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineConsulting.Jobs
{
    public class CloseUnusedConversationsJob : IJob
    {
        private readonly IConversationRepository _conversationRepository;

        public CloseUnusedConversationsJob(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository ?? throw new ArgumentNullException(nameof(conversationRepository));
        }

        public async Task Run(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await _conversationRepository.CloseUnusedConversationsAsync();
        }

    }
}
