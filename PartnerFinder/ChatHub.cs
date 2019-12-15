using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Service;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerFinder
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IUnitOfWork _unitOfWork;
        public ChatHub(IMessageService messageService, IUnitOfWork unitOfWork)
        {
            _messageService = messageService;
            _unitOfWork = unitOfWork;
        }
        public async Task SendChatMessage(string to, string message)
        {
            var userId = Context.User.Claims.First(c => c.Type == "userId").Value;
            await _messageService.AddOne(userId, to, message);
            await _unitOfWork.Commit();
            await Clients.Groups(userId).SendAsync("sendChatMessage", message, userId);
            await Clients.Groups(to).SendAsync("sendChatMessage", message, userId);

        }
        public override Task OnConnectedAsync()
        {
            var userId = Context.User.Claims.First(c => c.Type == "userId").Value;
            Groups.AddToGroupAsync(Context.ConnectionId, userId);
            return base.OnConnectedAsync();
        }
    }
}
