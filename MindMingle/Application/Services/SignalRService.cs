using System;
using Application.Interface;
using Application.Request.ChatMessage;
using Application.Response;
using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.SignalR;
using Twilio.TwiML.Messaging;

namespace Application.Services
{
    public class SignalRService : Hub, ISignalRService
    {
        private readonly IChatMessageService chatMessageService;

        public SignalRService(IChatMessageService chatMessageService)
        {
            this.chatMessageService = chatMessageService;
        }
        public override async Task OnConnectedAsync()
        {
            try
            {
                Console.WriteLine("🔥🔥🔥 SignalR Connected! 🔥🔥🔥");  // This should appear
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
            Console.WriteLine($"User {Context.ConnectionId} joined group {groupId}");
        }
        // DONT TOUCH, NOT WORKING :<
        public async Task JoinCallRoom(string accountId)
        {
            await Clients.User(accountId).SendAsync("JoinCallRoom", "Some Message...");
        }
        // Parameter sent from the FrontEnd (My AccountId + UsersInGroup Id + my message)
        public async Task ReceiveTextMessage(ChatMessageRequest chatMessageRequest)
        {
            Console.WriteLine("Message received!");
            Console.WriteLine($"Hope this work: {chatMessageRequest.Content}");
            var response = await chatMessageService.AddChatMessageByAccountId(chatMessageRequest);
            if (response.IsSuccess)
            {
                // Group Chat SignalR? wow, this is cool
               await Clients.Group(chatMessageRequest.UsersInGroupId).SendAsync("ReceiveTextMessage", response);
            }
            else
            {
                // Group Chat SignalR? wow, this is cool
                await Clients.User(chatMessageRequest.AccountId).SendAsync("ErrorMessage", response);
            }
        }
    }
}

