﻿using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using OctoBot.Configs;

namespace OctoBot.Handeling
{
    internal class ConsoleHandeling
    {

        internal static void ConsoleInput(DiscordSocketClient client)
        {
            var input = string.Empty;
        

            while (input != null && input.Trim().ToLower() != "block")
            {
                input = Console.ReadLine();

                if (input != null && input.Trim().ToLower() == "mess")
                    ConsoleSendMessage(client);

            }

        }

        private static async void ConsoleSendMessage(DiscordSocketClient client)
        {
            Console.WriteLine("Guilds: ");
            var guild = GetSelectedGuild(client.Guilds);
            var textChannel = GetSelectedTextChannel(guild.TextChannels);
            var msg = string.Empty;



            while (msg != null && msg.Trim() == string.Empty)
            {
                Console.WriteLine("Сообщение: ");
                msg = Console.ReadLine();
            }


            if (msg == null) return;
            var prefixCheck = msg.ToCharArray();
            var prefix = Config.Bot.Prefix.ToCharArray();
            if (prefixCheck[0] == prefix[0])
            {
                await textChannel.SendMessageAsync(msg);
                var items = await textChannel.GetMessagesAsync(1).Flatten();
                await textChannel.DeleteMessagesAsync(items);

                Console.WriteLine("Команда выполненая каппатан бу!");
            }
            else
            {
                await textChannel.SendMessageAsync(msg);
                Console.WriteLine("Отправлено!");
            }
        }

        private static SocketTextChannel GetSelectedTextChannel(IEnumerable<SocketTextChannel> channels)
        {
            var textChannels = channels.ToList();
            var maxIntex = textChannels.Count - 1;
            for (var i = 0; i <= maxIntex; i++)
            {
                Console.WriteLine($"{i} - {textChannels[i].Name}");
            }
            var selectedIndex = -1;
            while (selectedIndex < 0 || selectedIndex > maxIntex)
            {
                var success = int.TryParse(Console.ReadLine()?.Trim(), out selectedIndex);
                if (!success || selectedIndex < 0 || selectedIndex > maxIntex)
                {
                    Console.WriteLine("Incorrect index.");
                    selectedIndex = -1;
                }
            }
            return textChannels[selectedIndex];
        }


        private static SocketGuild GetSelectedGuild(IEnumerable<SocketGuild> guilds)
        {
            var socketGuilds = guilds.ToList();
            var maxIntex = socketGuilds.Count - 1;
            for (var i = 0; i <= maxIntex; i++)
            {
                Console.WriteLine($"{i} - {socketGuilds[i].Name}");
            }
            var selectedIndex = -1;
            while (selectedIndex < 0 || selectedIndex > maxIntex)
            {
                var success = int.TryParse(Console.ReadLine()?.Trim(), out selectedIndex);
                if (!success || selectedIndex < 0 || selectedIndex > maxIntex)
                {
                    Console.WriteLine("Incorrect index.");
                    selectedIndex = -1;
                }
            }
            return socketGuilds[selectedIndex];
        }








    }
}