using System;
using Application.Interface;
using Application.Request.ChatMessage;
using Application.Response;
using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.SignalR;

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
                Console.WriteLine("🔥🔥🔥 SignalR Connected! 🔥🔥🔥");
                Console.WriteLine($"User Connected: {Context.ConnectionId}");

                await Clients.Caller.SendAsync("TestConnection", "Connected to SignalR!");
                await base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in OnConnectedAsync: {ex.Message}");
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"❌ User Disconnected: {Context.ConnectionId}");
            await Clients.All.SendAsync("DebugMessage", $"User {Context.ConnectionId} disconnected.");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            Console.WriteLine($"✅ User {Context.ConnectionId} joined group {groupId}");
        }

        public async Task ReceiveTextMessage(ChatMessageRequest chatMessageRequest)
        {
            Console.WriteLine("📥 Message received!");
            Console.WriteLine($"🔹 Message content: {chatMessageRequest.Content}");

            var response = await chatMessageService.AddChatMessageByAccountId(chatMessageRequest);

            // Get the mutual group ID
            var currentGroupChat = await usersInGroupService.GetGroupChatByUsersInGroup(chatMessageRequest.UsersInGroupId);
            if (currentGroupChat == null)
            {
                Console.WriteLine($"❌ No group found for UsersInGroupId: {chatMessageRequest.UsersInGroupId}");
                return;
            }

            if (response.IsSuccess)
            {
                Console.WriteLine($"📤 Sending message to group {currentGroupChat}");
                await Clients.Group(currentGroupChat).SendAsync("ReceiveTextMessage", response.Result);
            }
            else
            {
                Console.WriteLine($"❌ Error sending message, notifying sender {chatMessageRequest.AccountId}");
                await Clients.User(chatMessageRequest.AccountId).SendAsync("ErrorMessage", response);
            }
        }
    }
}
