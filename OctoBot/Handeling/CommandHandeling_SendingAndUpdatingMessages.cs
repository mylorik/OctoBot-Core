﻿using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using OctoBot.Configs;
using OctoBot.Configs.LvLingSystem;
using OctoBot.Services;
using Discord;
using OctoBot.Configs.Server;


namespace OctoBot.Handeling
{


   public class CommandHandelingSendingAndUpdatingMessages
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.

        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private const string LogFile = @"OctoDataBase/Log.json";

        public CommandHandelingSendingAndUpdatingMessages(IServiceProvider services, CommandService commands, DiscordSocketClient client)
        {
            _commands = commands;
            _services = services;
            _client = client;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(
                Assembly.GetEntryAssembly(), 
                _services);
        }


        public async Task _client_MessageUpdated(Cacheable<IMessage, ulong> messageBefore,
            SocketMessage messageAfter, ISocketMessageChannel arg3)
        {
            if (messageAfter.Author.IsBot)
                return;
            var after = messageAfter as IUserMessage;
            if (messageAfter.Content == null)
            {
                return;
            }

            var before = (messageBefore.HasValue ? messageBefore.Value : null) as IUserMessage;
            if (before == null)
                return;
            if (arg3 == null)
                return;
            if (before.Content == after?.Content)
                return;



            var list = Global.CommandList;
            foreach (var t in list)
            {
                if (t.UserSocketMsg.Id != messageAfter.Id) continue;

                if (!(messageAfter is SocketUserMessage message)) continue;

                if (t.BotSocketMsg == null)
                    return;

                var context = new SocketCommandContextCustom(_client, message, "edit");
                var argPos = 0;

                if (message.Channel is SocketDMChannel)
                {
                    await _commands.ExecuteAsync(
                        context,
                        argPos,
                        _services);
                    return;
                }


                var guild = ServerAccounts.GetServerAccount(context.Guild);

                if (!message.HasStringPrefix(guild.Prefix, ref argPos) &&
                    !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) continue;
                await _commands.ExecuteAsync(
                    context,
                    argPos,
                    _services);
                return;
            }
            await Task.CompletedTask;
        }

        public static async Task SendingMess(SocketCommandContextCustom context, EmbedBuilder embed, string edit = null, [Remainder]string regularMess = null)
        {
            if (edit == null && regularMess == null) 
            {
                var message = await context.Channel.SendMessageAsync("", false, embed.Build());
                var kek = new Global.CommandRam(context.User, context.Message, message);
                Global.CommandList.Add(kek);
            }
            else if (edit == "edit" && regularMess == null)
            {
                foreach (var t in Global.CommandList)
                {
                    if (t.UserSocketMsg.Id == context.Message.Id)
                    {
                        await t.BotSocketMsg.ModifyAsync(message =>
                        {
                            message.Content = "";
                            message.Embed = embed.Build();
                        });
                    }
                }
            }
            else if (regularMess != null)
            {

                if (edit == null)
                {
                    var message = await context.Channel.SendMessageAsync($"{regularMess}");
                    var kek = new Global.CommandRam(context.User, context.Message, message);
                    Global.CommandList.Add(kek);
                }
                else if (edit == "edit")
                {
                    foreach (var t in Global.CommandList)
                    {
                        if (t.UserSocketMsg.Id == context.Message.Id)
                        {
                       
                            await t.BotSocketMsg.ModifyAsync(m =>
                            {
                                m.Embed = null;
                                m.Content = regularMess.ToString();
                            });
                        }
                    }
                }    
            }
        }



        public async Task HandleCommandAsync(SocketMessage msg)
        
        {
            var message = msg as SocketUserMessage;

            if (message == null) return;
            var context = new SocketCommandContextCustom(_client, message);
            var argPos = 0;


            switch (message.Channel)
            {
                case SocketDMChannel _ when context.User.IsBot:
                    return;
                case SocketDMChannel _:
                    var resultTask = _commands.ExecuteAsync(
                        context: context, 
                        argPos: argPos, 
                        services: _services);
                    resultTask.ContinueWith(task =>
                    {
                        if (!task.Result.IsSuccess)
                        {
                            Console.ForegroundColor = LogColor("red");
                            Console.WriteLine(
                                $"{DateTime.Now.ToLongTimeString()} - DM: ERROR '{context.Channel}' {context.User}: {message} || {task.Result.ErrorReason}");
                            Console.ResetColor();

                            File.AppendAllText(LogFile,
                                $"{DateTime.Now.ToLongTimeString()} - DM: ERROR '{context.Channel}' {context.User}: {message} || {task.Result.ErrorReason} \n");
                        }
                        else
                        {
                            Console.ForegroundColor = LogColor("white");
                            Console.WriteLine(
                                $"{DateTime.Now.ToLongTimeString()} - DM: '{context.Channel}' {context.User}: {message}");
                            Console.ResetColor();

                            File.AppendAllText(LogFile,
                                $"{DateTime.Now.ToLongTimeString()} - DM: '{context.Channel}' {context.User}: {message} \n");
                        }
                    });
                    await Task.CompletedTask;
                    return;
            }


            // Leveling up
                LvLing.UserSentMess((SocketGuildUser)context.User, (SocketTextChannel)context.Channel, message);


            var guild = ServerAccounts.GetServerAccount(context.Guild);
            if (message.HasStringPrefix(guild.Prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {

                var resultTask = _commands.ExecuteAsync(
                    context: context, 
                    argPos: argPos, 
                    services: _services);
                 resultTask.ContinueWith(task =>
                {
                    if (!task.Result.IsSuccess)
                    {
                        Console.ForegroundColor = LogColor("red");
                        Console.WriteLine(
                            $"{DateTime.Now.ToLongTimeString()} - ERROR '{context.Channel}' {context.User}: {message} || {task.Result.ErrorReason}");
                        Console.ResetColor();

                        File.AppendAllText(LogFile,
                            $"{DateTime.Now.ToLongTimeString()} - ERROR '{context.Channel}' {context.User}: {message} || {task.Result.ErrorReason} \n");
                    }
                    else
                    {
                        Console.ForegroundColor = LogColor("white");
                        Console.WriteLine(
                            $"{DateTime.Now.ToLongTimeString()} - '{context.Channel}' {context.User}: {message}");
                        Console.ResetColor();

                        File.AppendAllText(LogFile,
                            $"{DateTime.Now.ToLongTimeString()} - '{context.Channel}' {context.User}: {message} \n");
                    }
                });


            }
        }


        private static ConsoleColor LogColor(string color)
        {
            switch (color)
            {
                case "red":  //Critical or Error
                    return ConsoleColor.Red;
                case "green":    //Debug
                    return ConsoleColor.Green;
                case "cyan":     //Info
                    return ConsoleColor.Cyan;
                case "white":   //Regular
                    return ConsoleColor.White;
                case "yellow":  // Warning
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.White;
            }
        }


    }
}