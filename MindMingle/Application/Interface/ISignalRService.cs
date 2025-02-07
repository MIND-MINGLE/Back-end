using System;
using Application.Response;

namespace Application.Interface
{
	public interface ISignalRService
	{
		public Task SendTextMessage();
		public Task ReceiveTextMessage();
		public Task JoinCallRoom();
	}
}

