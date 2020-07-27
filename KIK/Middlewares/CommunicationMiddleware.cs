using Microsoft.AspNetCore.Http;
using org.omg.IOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Encodings;
using System.Linq.Expressions;
using System.IO;
using java.io;
using KIK.Model;
using java.sql;
using Newtonsoft.Json;

namespace KIK.Middlewares
{
    public class CommunicationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserService _userService;

        public CommunicationMiddleware(RequestDelegate next, IUserService user)
        {
            _next = next;
            _userService = user;
        }

        /*
        public async Task Invoke(HttpContext context)
        {
            if(context.Request.Path.Equals("/CheckEmailConfirmationStatus"))
            {
                await ProcessEmailConfirmation(context);
            }
            else
            {
                await _next?.Invoke(context);
            }
        }
        */

        public async Task Invoke(HttpContext context)
        {
            if(context.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                var token = context.RequestAborted;
                var json = await ReceiveStringAsync(webSocket, token);
                var command = JsonConvert.DeserializeObject<dynamic>(json);

                switch (command.Operation.ToString())
                {
                    case "CheckEmailConfirmationStatus":
                        {
                            await ProcessEmailConfirmation(context, webSocket, token, command.Parameters.ToString());
                            break;
                        }
                }
            }
            else if (context.Request.Path.Equals("/CheckEmailConfirmationStatus"))
            {
                await ProcessEmailConfirmation(context);
            }
            else
            {
                await _next?.Invoke(context);
            }
        }

        private async Task ProcessEmailConfirmation(HttpContext context)
        {
            var email = context.Request.Query["email"];
            var user = await _userService.GetUserByEmail(email);

            if(string.IsNullOrEmpty(email))
            {
                await context.Response.WriteAsync("Nieprawidlowo wykonano procedure. Wymagany jest email.");
            }
            else if( (await _userService.GetUserByEmail(email)).IsEmailConfirmed)
            {
                await context.Response.WriteAsync("OK");
            }
            else
            {
                await context.Response.WriteAsync("Oczekiwanie na potwierdzenie adresu e-mail");
                user.IsEmailConfirmed = true;
                user.EmailConfirmationDate = DateTime.Now;
                _userService.UpdateUser(user).Wait();
            }


        }

        private static async Task<Task> SendStringAsync(WebSocket webSocket, 
           string data, CancellationToken token = default(CancellationToken))
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);

            return webSocket.SendAsync(segment, WebSocketMessageType.Text, true, token);
        }

        private static async Task<string> ReceiveStringAsync(WebSocket webSocket,
        CancellationToken token = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);

            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    token.ThrowIfCancellationRequested();

                    result = await webSocket.ReceiveAsync(buffer, token);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if(result.MessageType!=WebSocketMessageType.Text)
                {
                    throw new Exception("Blad"); 
                }

                using (var reader = new StreamReader(ms, System.Text.Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }

            }

        }



        public async Task ProcessEmailConfirmation(HttpContext context, WebSocket currSocket, CancellationToken token, string email)
        {
            UserModel user = await _userService.GetUserByEmail(email);
            while(!token.IsCancellationRequested && !currSocket.CloseStatus.HasValue && user?.IsEmailConfirmed == false)
            {
                if(user.IsEmailConfirmed)
                {
                    await SendStringAsync(currSocket, "OK", token);
                }
                else
                {
                    user.IsEmailConfirmed = true;
                    user.EmailConfirmationDate = DateTime.Now;
                    await _userService.UpdateUser(user);
                    await SendStringAsync(currSocket, "OK", token);
                }
                Task.Delay(500).Wait();
                user = await _userService.GetUserByEmail(email);
            }
        }
    }
}
