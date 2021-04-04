using Discord;
using Discord.WebSocket;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Ralph
{
    class Program
    {
        DiscordSocketClient client;

        static void Main(string[] args) =>
            new Program().RalphBotAsync().GetAwaiter().GetResult();

        async Task RalphBotAsync()
        {
            client = new DiscordSocketClient();
            client.Log += Log;
            client.MessageReceived += MessageReceived;

            string[] lines = File.ReadAllLines("Secret.txt");
            string tok = lines[0];

            await client.LoginAsync(TokenType.Bot, tok);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        Task MessageReceived(SocketMessage msg)
        {
            Console.WriteLine($"Recieved Message from '{msg.Author}': '{msg.Content}'");
            return Task.CompletedTask;
        }

        Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}
