using System;
using Application.Response;

namespace Application.Interface
{
	public interface ISignalRService
	{
		public Task<ApiResponse> SendTextMessage();
		public Task<ApiResponse> ReceiveTextMessage();
		public Task<ApiResponse> JoinCallRoom();
	}
}

