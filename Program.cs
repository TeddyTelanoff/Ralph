using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Ralph
{
    public class Program
    {
        private static readonly string[] sadReplies = {
            // Unlucky replies
            "Be a man, and stop that yipper-yapping!",
            "Suck it up.",

            // Positive replies
            "Don't worry, everything is gonna be allright!",
            "Here, have a lollypop :lollipop:",
            "Don't be so sad, help is on the horizon",
            "Studies show that taking a break can help clear your mind...",
            "Studies show that taking a break can help clear your mind...",

            // Acceptance
            "Me too, kid. :cry:",
        };
        private static readonly string[] poopReplies = {
            "Remember to pull down your pants! Don't want your mom complaining about {0}, pooping his pants again.",
            "{0} thinks GCC/G++ is :poop:",
            "Hey {0}! Go in the bathroom!",
            "{0} went :poop:",
        };

        private DiscordSocketClient client;
        private CommandService commands;
        private bool alive;

        private static void Main(string[] args)
        {
            var prog = new Program();
            var task = prog.RalphBotAsync();
            while (prog.alive)
                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    prog.alive = false;
            task.Wait();
        }

        private async Task RalphBotAsync()
        {
            alive = true;
            client = new DiscordSocketClient();
            client.Log += Log;
            client.MessageReceived += HandleCommandAsync;

            commands = new CommandService();
            commands.CommandExecuted += ExecuteCommandAsync;

            string[] lines = File.ReadAllLines(@"..\..\..\Secret.txt");
            string tok = lines[0];

            await client.LoginAsync(TokenType.Bot, tok);
            await client.StartAsync();

            // Wait
            while (alive);
        }

        private async Task ExecuteCommandAsync(Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            var user = context.User;
            var channel = context.Channel;

            // Get Command
            int argPos = 0;
            context.Message.HasCharPrefix('!', ref argPos);
            string cmd = context.Message.Content.Substring(argPos);

            switch (cmd.ToLower())
            {
            case "sad":
            {
                string reply = sadReplies[new Random().Next(sadReplies.Length)];
                await channel.SendMessageAsync(reply);
                break;
            }
            case "poop":
            {
                string reply = poopReplies[new Random().Next(poopReplies.Length)];
                if (user.Username == "Loltz")
                    reply = poopReplies[1];

                await channel.SendMessageAsync(string.Format(reply, user.Mention));
                break;
            }
            }
        }

        private async Task HandleCommandAsync(SocketMessage socketMsg)
        {
            var msg = socketMsg as SocketUserMessage;
            if (msg is null)
                return;

            int argPos = 0;
            if (!msg.HasCharPrefix('!', ref argPos))
                return;

            if (msg.Author.IsBot)
                return;

            var context = new SocketCommandContext(client, msg);
            await commands.ExecuteAsync(context, argPos, null);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}
