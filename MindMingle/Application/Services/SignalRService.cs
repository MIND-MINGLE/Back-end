using System;
using Application.Interface;
using Application.Response;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services
{
	public class SignalRService : Hub, ISignalRService
	{

        public async Task JoinCallRoom()
        {
            await Clients.User("1").SendAsync("JoinCallRoom","Some Message...");
        }

        public async Task ReceiveTextMessage()
        {
            await Clients.User("1").SendAsync("ReceiveTextMessage", "Some Message...");
            
        }

        public async Task SendTextMessage()
        {
            await Clients.User("1").SendAsync("SendTextMessage", "Some Message...");
           
        }
    }
}

