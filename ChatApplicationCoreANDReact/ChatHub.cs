using Application.Services;
using ChatApplicationCoreANDReact.Models;
using Domain.Entities;
using Domain.UnitOfWork;
using Microsoft.AspNetCore.SignalR;

namespace ChatApplicationCoreANDReact
{
    public class ChatHub : Hub
    {
        private readonly IUserService _UserService;
        private readonly IUserMessagesService _UserMessagesService;
        private readonly IUnitOfWorkAsync _unitOfWorkAsync;

        public ChatHub(IUserService UserService, IUnitOfWorkAsync unitOfWorkAsync, IUserMessagesService UserMessagesService)
        {
            _UserService = UserService;
            _UserMessagesService = UserMessagesService;
            _unitOfWorkAsync = unitOfWorkAsync;
        }

        public override Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;

            // Optional: Track user connection
            return base.OnConnectedAsync();
        }

    
        public async Task RegisterConnectionId(long userId)
        {
            var user =  await _UserService.FindAsync(userId);
            if (user != null)
            {
                user.SignalID = Context.ConnectionId;
                _UserService.Update(user);
                 await _unitOfWorkAsync.SaveChangesAsync();
            }
        }

        public async Task Send(long senderId, long receiverId, string message)
        {
            var sender = await _UserService.FindAsync(senderId);
            var recipient = await _UserService.FindAsync(receiverId);

          //  if (recipient?.SignalID == null) return;

            var chatMessage = new UserMessages
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = message,
                IsSender = true,
                Timestamp = DateTime.UtcNow
            };

            _UserMessagesService.Insert(chatMessage);
            await _unitOfWorkAsync.SaveChangesAsync();

            MessageViewDTO messageReceive = new MessageViewDTO
            (
                Id: chatMessage.Id,
                SenderId : senderId,
                SenderName : sender.UserName,
                ReceiverName : recipient.UserName,
                Message : chatMessage.Content,
                Timestamp : chatMessage.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                IsSender: false
            );

            await Clients.Client(recipient.SignalID).SendAsync("ReceiveMessage", messageReceive);
        }
    }
}
