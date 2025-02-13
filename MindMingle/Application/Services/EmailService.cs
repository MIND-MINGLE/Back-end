using Application.Interface;
using Application.Response;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class EmailService : IEmailService
	{
		public const string EmailUserSystem = "mindmingleskill@gmail.com";
		public const string EmailPasswordSystem = "uksq gcmk oahz wddx";
		public async Task<ApiResponse> SendValidationEmail(string receivedUser, string emailContent)
		{
			try
			{
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress("Mindmingle", "mindmingleskill.com"));
				message.To.Add(new MailboxAddress("", receivedUser));
				message.Subject = $"Verification Email";

				var bodyBuilder = new BodyBuilder();
				bodyBuilder.HtmlBody = emailContent;
				message.Body = bodyBuilder.ToMessageBody();

				using (var client = new SmtpClient())
				{
					await client.ConnectAsync("smtp.gmail.com", 465, true);
					await client.AuthenticateAsync(EmailUserSystem, EmailPasswordSystem);
					await client.SendAsync(message);
					await client.DisconnectAsync(true);
				}
				return new ApiResponse().SetOk("Mail Sent!");
			}
			catch(Exception ex)
			{
				return new ApiResponse().SetBadRequest($"Something went wrong: {ex.Message}");
			}
		}
	}
}
