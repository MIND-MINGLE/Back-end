using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Application.Interface;
using Application.Request.ChatMessage;
using Application.Response;

namespace Application.Services
{
    public class SignalRService : Hub, ISignalRService
    {
        private readonly IChatMessageService chatMessageService;
        private readonly IUsersInGroupService usersInGroupService;

        public SignalRService(IChatMessageService chatMessageService, IUsersInGroupService usersInGroupService)
        {
            this.chatMessageService = chatMessageService;
            this.usersInGroupService = usersInGroupService;
        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                Console.WriteLine("SignalR Connected!");
                Console.WriteLine($"User Connected: {Context.ConnectionId}");

                await Clients.Caller.SendAsync("TestConnection", "Connected to SignalR!");
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnConnectedAsync: {ex.Message}");
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"User Disconnected: {Context.ConnectionId}");
            await Clients.All.SendAsync("DebugMessage", $"User {Context.ConnectionId} disconnected.");
            await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId); // Notify for call cleanup
            await base.OnDisconnectedAsync(exception);
        }

        // Chat: Join a group (already used for chat, now also for calls)
        public async Task JoinGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            Console.WriteLine($"User {Context.ConnectionId} joined group {groupId}");

            // Notify group of new user (for call initialization)
            await Clients.Group(groupId).SendAsync("UserConnected", Context.ConnectionId);
        }

        // Chat: Receive and broadcast text messages
        public async Task ReceiveTextMessage(ChatMessageRequest chatMessageRequest)
        {
            Console.WriteLine("Message received!");
            Console.WriteLine($"Message content: {chatMessageRequest.Content}");

            var response = await chatMessageService.AddChatMessageByAccountId(chatMessageRequest);

            var currentGroupChat = await usersInGroupService.GetGroupChatByUsersInGroup(chatMessageRequest.UsersInGroupId);
            if (currentGroupChat == null)
            {
                Console.WriteLine($"No group found for UsersInGroupId: {chatMessageRequest.UsersInGroupId}");
                return;
            }

            if (response.IsSuccess)
            {
                Console.WriteLine($"Sending message to group {currentGroupChat}");
                await Clients.Group(currentGroupChat).SendAsync("ReceiveTextMessage", response.Result);
            }
            else
            {
                Console.WriteLine($"Error sending message, notifying sender {chatMessageRequest.AccountId}");
                await Clients.User(chatMessageRequest.AccountId).SendAsync("ErrorMessage", response);
            }
        }

        // Call: Send WebRTC offer to the other user in the group
        public async Task SendOffer(string groupId, string offer)
        {
            Console.WriteLine($"Sending offer from {Context.ConnectionId} to group {groupId}");
            await Clients.OthersInGroup(groupId).SendAsync("ReceiveOffer", offer, Context.ConnectionId);
        }

        // Call: Send WebRTC answer to the caller in the group
        public async Task SendAnswer(string groupId, string answer, string callerId)
        {
            Console.WriteLine($"Sending answer from {Context.ConnectionId} to caller {callerId} in group {groupId}");
            await Clients.Client(callerId).SendAsync("ReceiveAnswer", answer);
        }

        // Call: Send ICE candidate to the other user in the group
        public async Task SendCandidate(string groupId, string candidate)
        {
            Console.WriteLine($"Sending ICE candidate from {Context.ConnectionId} to group {groupId}");
            await Clients.OthersInGroup(groupId).SendAsync("ReceiveCandidate", candidate, Context.ConnectionId);
        }
    }
}