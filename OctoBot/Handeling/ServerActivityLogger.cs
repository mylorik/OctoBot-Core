﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using OctoBot.Commands;
using OctoBot.Commands.PersonalCommands;
using OctoBot.Configs;
using OctoBot.Configs.Users;
using static OctoBot.Configs.Global;

namespace OctoBot.Handeling
{
    public class EveryLogHandeling 
    {
        private static readonly DiscordSocketClient Client = Global.Client;

        static readonly SocketTextChannel LogTextChannel =
            Global.Client.GetGuild(375104801018609665).GetTextChannel(454435962089373696);
        


        public static Task _client_Ready()
        {
            Client.JoinedGuild += Client_JoinedGuild; // Please Check more options!
            Client.ReactionAdded += Client_ReactionAddedAsyncForBlog;
            Client.ReactionRemoved += Client_ReactionRemovedForBlog;
            Client.ReactionAdded += Client_ReactionAddedForArtVotes;
            Client.ReactionRemoved += Client_ReactionRemovedForArtVotes;
            Client.Disconnected += Client_Disconnected;
            Client.Connected += Client_Connected;
            Client.MessageUpdated += Client_MessageUpdated;
            Client.MessageDeleted += Client_MessageDeleted;
            Client.ChannelCreated += Client_ChannelCreated;
            Client.ChannelDestroyed += Client_ChannelDestroyed;  
            Client.RoleDeleted += Client_RoleDeleted;
            Client.RoleUpdated += Client_RoleUpdated;

            /////////////////////////////////////////////
            Client.ChannelUpdated += Client_ChannelUpdated;
    
            Client.GuildMemberUpdated += Client_GuildMemberUpdated;
            return Task.CompletedTask;

        }

        private static async Task Client_RoleUpdated(SocketRole arg1, SocketRole arg2)
        {

            var before = arg1 as IRole;
            var after = arg2 as IRole;
            if(after == null)
                return;
            if(before == after)
                return;
            var log = await before.Guild.GetAuditLogAsync(1);
            var audit = log.ToList();

            var extra = "";
           var afterList = after.Permissions.ToList();
            for (var i = 0; i < afterList.Count; i++)
            {
                extra += $"{afterList[i].ToString()}\n";

            }

            var embed = new EmbedBuilder();
            embed.WithColor(Color.DarkTeal);
            embed.AddField("Role Updatet", $"WHO: {audit[0].User.Mention}\n" +
                                           $"ID: {before.Id}\n" +
                                           $"Guild: {before.Guild.Name}\n" +
                                           $"__**Before:**__\n" +
                                           $"Name: **{before}**\n" +
                                           $"Color: {before.Color}\n" +

                                           $"__**After:**__\n" +
                                           $"Name: **{after}**\n" +
                                           $"Color: {after.Color}\n" +
                                           $"{after.Mention}\n" +
                                           $"Permissions:\n{extra}");
            await LogTextChannel.SendMessageAsync("", false, embed.Build());

        }

        private static async Task Client_ChannelUpdated(SocketChannel arg1, SocketChannel arg2)
        {


            if (arg1 is ITextChannel arg)
            {
                var before = arg1 as ITextChannel;
                var after = arg2 as ITextChannel;
                if (before == after)
                    return;
                if(after == null)
                    return;
                var log = await arg.Guild.GetAuditLogAsync(1);
                
                var audit = log.ToList();
              
                var permAf =  after.PermissionOverwrites.ToList();
                var permBef =  before.PermissionOverwrites.ToList();
                var extra = "";

                for (var i = 0; i < permAf.Count; i++)
                {
                    extra += $"<@&{permAf[i].TargetId}>\n";
                    var kek1 = permAf[i].Permissions.ToAllowList();
                    var kek2 = permBef[i].Permissions.ToAllowList();

                    for (var k = 0; k < kek1.Count; k++)
                    {
                        if(!kek1[k].Equals(kek2[k]))       
                        extra += $"{kek1[k]}\n";
                    }
                    
                }
                var embed = new EmbedBuilder();
                embed.WithColor(Color.DarkBlue);
                embed.AddField("Channel Updated", $"WHO: {audit[0].User.Mention}\n" +
                                                  $"Action: {audit[0].Action.ToString()}\n" +
                                                  $"Guild: {before.Guild}\n" +
                                                  $"ID: {before.Id}\n" +
                                                  $"__**Before:**__\n" +
                                                  $"Name: {before.Name}\n" +
                                                  $"__**After:**__\n" +
                                                  $"Name: {after.Mention}\n");
                embed.AddField("Extra:", $"{extra}");
   

                await LogTextChannel.SendMessageAsync("", false, embed.Build());
            }
        }

        private static async Task Client_RoleDeleted(SocketRole arg)
        {
            try
            {
                
                var log = await arg.Guild.GetAuditLogsAsync(1).FlattenAsync();
                var audit = log.ToList();
                var name = audit[0].Action == ActionType.RoleDeleted ? audit[0].User.Mention : "error";

                var embed = new EmbedBuilder();
                embed.WithColor(Color.DarkOrange);
                embed.AddField("Role Deleted", $"Name: {name}\n" +
                                               $"Time: {DateTime.Now.ToLongTimeString()}\n" +
                                               $"Name: {arg.Name} ({arg.Guild})\n" +
                                               $"Color: {arg.Color}\n" +
                                               $"ID: {arg.Id}\n");

                await LogTextChannel.SendMessageAsync("", false, embed.Build());
            }
            catch
            {
                //
            }
        }

        public async Task NonStaticMethod(Cacheable<IUserMessage, ulong> arg1, SocketReaction arg3)
        {
            try
            {
               var artVoteMess = new ArtVotes(arg1.Value.Author, arg1.Value, arg1.Value.Author, arg3.Emote.Name);
                ArtVotesList.Add(artVoteMess);
            }
            catch (Exception e)
            {
                Console.WriteLine("NonStaticMethod");
                Console.WriteLine(e.Message);
            }

            await Task.CompletedTask;
        }

        private static async Task Client_ReactionAddedForArtVotes(Cacheable<IUserMessage, ulong> arg1,
            ISocketMessageChannel arg2, SocketReaction arg3)
        {
           

            if (arg3.User.Value.IsBot)
                return;


            var artMessagesList = ArtVotesList;

            if (arg3.Emote.Name == "📊" || arg3.Emote.Name == "🎨" || arg3.Emote.Name == "🏆")
            {


                foreach (ArtVotes v in artMessagesList)
                {
                    if (arg1.Value.Id == v.SocketMsg.Id)
                        return;
                }

                try
                {
                    EveryLogHandeling everyLogHandeling = new EveryLogHandeling();
                    await everyLogHandeling.NonStaticMethod(arg1, arg3);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                await arg1.DownloadAsync().Result
                    .RemoveReactionAsync(arg3.Emote, arg3.User.Value, RequestOptions.Default);            
                await arg1.Value.AddReactionAsync(new Emoji("1⃣"));
                await arg1.Value.AddReactionAsync(new Emoji("2⃣"));
                await arg1.Value.AddReactionAsync(new Emoji("3⃣"));
                await arg1.Value.AddReactionAsync(new Emoji("4⃣"));
                await arg1.Value.AddReactionAsync(new Emoji("5⃣"));
            }


            if(arg3.User.Value.Id == arg1.Value.Author.Id)
                return;
            foreach (ArtVotes v1 in artMessagesList)
            {
                if (!v1.UserVoted.Contains(arg3.User.Value) && v1.SocketMsg == arg1.Value) 
                {


                 // Console.WriteLine($"working2");
                    var account = UserAccounts.GetAccount(v1.BlogAuthor);
                    switch (arg3.Emote.Name)
                    {
                        case "1⃣":
                            account.ArtVotesQty += 1;
                            account.ArtVotesSum += 1;
                            UserAccounts.SaveAccounts();
                            break;
                        case "2⃣":
                            account.ArtVotesQty += 1;
                            account.ArtVotesSum += 2;
                            UserAccounts.SaveAccounts();
                            break;
                        case "3⃣":
                            account.ArtVotesQty += 1;
                            account.ArtVotesSum += 3;
                            UserAccounts.SaveAccounts();
                            break;
                        case "4⃣":
                            account.ArtVotesQty += 1;
                            account.ArtVotesSum += 4;
                            UserAccounts.SaveAccounts();
                            break;
                        case "5⃣":
                            account.ArtVotesQty += 1;
                            account.ArtVotesSum += 5;
                            UserAccounts.SaveAccounts();
                            break;
                    }

                    v1.UserVoted.Add(arg3.User.Value);
                    v1.Emotename.Add(arg3.Emote.Name);
                }

                //return;
            }

            await Task.CompletedTask;
        }

        private static async Task Client_ReactionRemovedForArtVotes(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (arg3.User.Value.IsBot)
                return;
            if(arg3.User.Value.Id == arg1.Value.Author.Id)
                return;

            var artMessagesList = ArtVotesList;
            foreach (ArtVotes v in artMessagesList)
            {
                if (v.UserVoted.Contains(arg3.User.Value) && v.SocketMsg == arg1.Value)
                {
                    for (var j = 0; j < v.UserVoted.Count; j++)
                    {
                      //  Console.WriteLine($"working remove voted123 emote: {artMessagesList[i].Emotename[j]}  entered: {arg3.Emote.Name}");
                        if (arg3.Emote.Name == v.Emotename[j] && arg3.User.Value.Id == v.UserVoted[j].Id)
                        {
                           // Console.WriteLine($"working remove voted = {artMessagesList[i].UserVoted.Count}");
                            var account = UserAccounts.GetAccount(v.BlogAuthor);
                            switch (arg3.Emote.Name)
                            {
                                case "1⃣":
                                    account.ArtVotesQty -= 1;
                                    account.ArtVotesSum -= 1;
                                    UserAccounts.SaveAccounts();
                                    break;
                                case "2⃣":
                                    account.ArtVotesQty -= 1;
                                    account.ArtVotesSum -= 2;
                                    UserAccounts.SaveAccounts();
                                    break;
                                case "3⃣":
                                    account.ArtVotesQty -= 1;
                                    account.ArtVotesSum -= 3;
                                    UserAccounts.SaveAccounts();
                                    break;
                                case "4⃣":
                                    account.ArtVotesQty -= 1;
                                    account.ArtVotesSum -= 4;
                                    UserAccounts.SaveAccounts();
                                    break;
                                case "5⃣":
                                    account.ArtVotesQty -= 1;
                                    account.ArtVotesSum -= 5;
                                    UserAccounts.SaveAccounts();
                                    break;
                            }

                            v.UserVoted.Remove(arg3.User.Value);
                            v.Emotename.Remove(arg3.Emote.Name);
                          //  Console.WriteLine($"removed from voted = {artMessagesList[i].UserVoted.Count}");
                        }
                    }
                }
            }

            await Task.CompletedTask;
        }

        private static async Task Client_ChannelDestroyed(IChannel  arg)
        {
            try
            {

                var embed = new EmbedBuilder();
                embed.WithColor(Color.DarkGreen);

                if (arg is ITextChannel channel)
                {
                    var log = await channel.Guild.GetAuditLogAsync(1);
                    var audit = log.ToList();
                    var name = audit[0].Action == ActionType.ChannelDeleted ? audit[0].User.Mention : "error";
                    embed.AddField("Channel Destroyed", $"Name: {name}\n" +
                                                        $"Time: {DateTime.Now.ToLongTimeString()}\n" +
                                                        $"Name: {arg.Name}\n" +
                                                        $"NSFW: {channel.IsNsfw}\n" +
                                                        $"Category: {channel.GetCategoryAsync().Result.Name}\n" +
                                                        $"ID: {arg.Id}\n");
                }

                await LogTextChannel.SendMessageAsync("", false, embed.Build()); 

            }
            catch
            {
//
            }
        }

        private static async Task Client_ChannelCreated(IChannel arg)
        {
            
            try
            {
                if (!(arg is ITextChannel channel))
                    return;
                
                    var log = await channel.Guild.GetAuditLogAsync(1);
                    var audit = log.ToList();
                var name = audit[0].Action == ActionType.ChannelCreated ? audit[0].User.Mention : "error";
                    var embed = new EmbedBuilder();
                    embed.WithColor(Color.DarkBlue);
                    embed.AddField("Channel Created", $"Name: {name}\n" +
                                                      $"Time: {DateTime.Now.ToLongTimeString()}\n" +
                                                      $"Name: {arg.Name}\n" +
                                                      $"NSFWL {channel.IsNsfw}\n" +
                                                      $"Category: {channel.GetCategoryAsync().Result.Name}\n" +
                                                      $"ID: {arg.Id}\n");
               
                await LogTextChannel.SendMessageAsync("", false, embed.Build());
            }
            catch
            {
//
            }
        }

        private static async Task Client_GuildMemberUpdated(SocketGuildUser before, SocketGuildUser after)
        {
              try
              {
                  if (after == null || before == after || before.IsBot)
                      return;

                 
                 

                  var embed = new EmbedBuilder();
                  if (before.Nickname != after.Nickname)
                  {
                      var log = await before.Guild.GetAuditLogsAsync(1).FlattenAsync();
                      var audit = log.ToList();
                      var beforeName = before.Nickname ?? before.Username;

                      var afterName = after.Nickname ?? after.Username;

                      embed.WithColor(Color.DarkGreen);
                      embed.AddField("Nickname Changed:", $"Time: **{DateTime.Now.ToLongTimeString()}**\n" +
                                                          $"User: **{before.Username} {before.Id}**\n" +
                                                          $"Server: **{before.Guild.Name}**\n" +
                                                          $"Before:\n" +  
                                                          $"**{beforeName}**\n" +
                                                          $"After:\n" +
                                                          $"**{afterName}**");
                      if (audit[0].Action == ActionType.MemberUpdated)
                          embed.AddField("WHO:", $"{audit[0].User.Mention}\n");

                      await LogTextChannel.SendMessageAsync("", false, embed.Build());

                      var userAccount = UserAccounts.GetAccount(after);
                      var user = after;
                      if (userAccount.ExtraUserName != null)
                      {

                          var dublicate = 0;
                          var extra = userAccount.ExtraUserName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                          for (var i = 0; i < extra.Length; i++)
                          {
                              if (extra[i] == user.Nickname && extra[i] != null)
                                  dublicate = 1;
                          }

                          if (dublicate != 1 && user.Nickname != null)
                              userAccount.ExtraUserName += (user.Nickname + "|");

                      }
                      else if (user.Nickname != null)
                          userAccount.ExtraUserName = (user.Nickname + "|");
                      UserAccounts.SaveAccounts();
                  }
                  if (before.GetAvatarUrl() != after.GetAvatarUrl())
                  {
                      embed.WithColor(Color.DarkGreen);
                      embed.AddField("Avatar Changed:", $"Time: **{DateTime.Now.ToLongTimeString()}**\n" +
                                                          $"User: **{before.Username} {before.Id}**\n" +
                                                          $"Server: **{before.Guild.Name}**\n" +
                                                          $"Before:\n" +
                                                          $"**{before.GetAvatarUrl()}**\n" +
                                                          $"After:\n" +
                                                          $"**{after.GetAvatarUrl()}**");

                      await LogTextChannel.SendMessageAsync("", false, embed.Build());
                  }
                  if (before.Username != after.Username || before.Id != after.Id)
                  {
                      embed.WithColor(Color.DarkRed);
                      embed.AddField("USERNAME Changed:", $"Time: **{DateTime.Now.ToLongTimeString()}**\n" +                                               
                                                        $"Server: **{before.Guild.Name}**\n" +
                                                        $"Before:\n" +
                                                        $"**{before.Username} {before.Id}**\n" +
                                                        $"After:\n" +
                                                        $"**{after.Username} {after.Id}**\n");
                      await LogTextChannel.SendMessageAsync($"<@181514288278536193> here is a gay:");
                      await LogTextChannel.SendMessageAsync("", false, embed.Build());
                  }
                  if (before.Roles.Count != after.Roles.Count)
                  {
                      
                      string roleString;                 
                      var list1 = before.Roles.ToList();
                      var list2 = after.Roles.ToList();
                      var role = "";
                      if (before.Roles.Count > after.Roles.Count)
                      {
                          roleString = "Removed";
                          IEnumerable<SocketRole> differenceQuery = list1.Except(list2);
                          var socketRoles = differenceQuery as SocketRole[] ?? differenceQuery.ToArray();
                          for (var i = 0; i < socketRoles.Count(); i++)
                              role += socketRoles[i];
                      }
                      else
                      {
                          roleString = "Added";
                          IEnumerable<SocketRole> differenceQuery = list2.Except(list1);
                          var socketRoles = differenceQuery as SocketRole[] ?? differenceQuery.ToArray();
                          for (var i = 0; i < socketRoles.Count(); i++)
                              role += socketRoles[i];
                          if (role == "LoL")
                          {
                              await Global.Client.GetGuild(338355570669256705).GetTextChannel(429345059486564352)
                                  .SendMessageAsync($"Буль тебе, {after.Mention}! Если ты новенький в этом мире, то ты можешь попросить у нас реферальную ссылку, чтобы получить **сразу 50 персов на аккаунт**\n" +
                                                    $"А если ты профи, то можешь попробовать спросить mylorik аккаунт с персонажами, на время, разумеется.");

                          }
                      }

                      embed.WithColor(Color.DarkGreen);
                      embed.AddField($"Role Update (Role {roleString}):", $"Time: **{DateTime.Now.ToLongTimeString()}**\n" +
                                                        $"User: **{before.Username} {before.Id}**\n" +
                                                        $"Server: **{before.Guild.Name}**\n" +
                                                                     $"Role ({roleString}): **{role}**");

                      await LogTextChannel.SendMessageAsync("", false, embed.Build());
                  }
                  
              }
                catch
                {
                    // ignored
                }
        }

        public static async Task Client_JoinedGuild(SocketGuild arg)
        {
            await LogTextChannel.SendMessageAsync($"<@181514288278536193> OctoBot have been connected to {arg.Name}");
        }

        public static async Task Client_Connected()
        {
            await LogTextChannel.SendMessageAsync($"OctoBot on Duty!");
        }

        public static async Task Client_Disconnected(Exception arg)
        {
            Global.Client.Ready -= GreenBuuTimerClass.StartTimer;             ////////////// Timer1 Green Boo starts
            Global.Client.Ready -= DailyPull.CheckTimerForPull;                 ////////////// Timer3 For Pulls   
            Global.Client.Ready -= Reminder.CheckTimer;                       ////////////// Timer4 For For Reminders
            Global.Client.Ready -= ForBot.TimerForBotAvatar;   
            Global.Client.Ready -= _client_Ready;
            await LogTextChannel.SendMessageAsync($"OctoBot Disconnect: {arg.Message}");
            await LogTextChannel.SendMessageAsync($"<@181514288278536193> Disconnect!");
        }

        public static async Task Client_MessageUpdated(Cacheable<IMessage, ulong> messageBefore,
            SocketMessage messageAfter, ISocketMessageChannel arg3)
        {
            try
            {
                if(messageAfter.Author.IsBot)
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


                var embed = new EmbedBuilder();
                embed.WithColor(Color.DarkPurple);
                embed.WithTitle($"📝 Updated Message: {after?.Channel.Name}");
                embed.WithDescription($"Mess Author: **{after?.Author}**\n" +
                                      $"Time: **{DateTime.Now.ToLongTimeString()}**\n");
                embed.AddField("Before:", $"{messageBefore.Value.Content}\n**______**");
                if (messageBefore.Value.Attachments.Any())
                    embed.AddField("attachments", $"{messageBefore.Value.Attachments.FirstOrDefault()?.Url}");
                embed.AddField("After:", $"{after}");
                if (messageAfter.Attachments.Any())
                    embed.AddField("attachments", $"{messageAfter.Attachments.FirstOrDefault()?.Url}");

                await LogTextChannel.SendMessageAsync("", false, embed.Build());
            }
            catch
            {
                Console.WriteLine("Cath messupd");
            }
        }
      
        private static async Task Client_MessageDeleted(Cacheable<IMessage, ulong> messageBefore,
            ISocketMessageChannel arg3)
        {
            try
            {
                if (messageBefore.Value.Channel is ITextChannel kek)
                {
                    var log = await kek.Guild.GetAuditLogAsync(1);
                    var audit = log.ToList();
                    var name = audit[0].Action == ActionType.MessageDeleted ? audit[0].User.Mention : messageBefore.Value.Author.Mention;

                    var embedDel = new EmbedBuilder();
                    embedDel.WithColor(Color.DarkPurple);
                    embedDel.WithTitle($"🗑 Deleted Message in {messageBefore.Value.Channel.Name}");
                    embedDel.WithDescription($"WHO: **{name}**\n" +
                                             $"Mess Author: **{messageBefore.Value.Author}**\n" +
                                             $"Time: **{DateTime.Now.ToLongTimeString()}**\n");
                    embedDel.AddField("Content", $"{messageBefore.Value.Content}");
                    embedDel.AddField("Mess ID", $"{messageBefore.Id}");
                    if (messageBefore.Value.Attachments.Any())
                        embedDel.AddField("attachments", $"URL: {messageBefore.Value.Attachments.FirstOrDefault()?.Url}\n" +
                                                         $"Proxy URL: {messageBefore.Value.Attachments.FirstOrDefault()?.ProxyUrl}"); 
   

                    await LogTextChannel.SendMessageAsync("", false, embedDel.Build());
                }
            }
            catch
            {

                //
            }
        }

        public static async Task Client_ReactionAddedAsyncForBlog(Cacheable<IUserMessage, ulong> arg1,
            ISocketMessageChannel arg2, SocketReaction arg3)
        {
            if (arg3.User.Value.IsBot)
                return;

            var blogList = BlogVotesMessIdList;
            foreach (BlogVotes v in blogList)
            {


                if (v.SocketMsg.Id == arg1.Id && v.ReactionUser.Id == arg3.User.Value.Id)
                {

                    if (arg3.User.Value.Id == v.BlogAuthor.Id)
                    {
                        
                        await arg3.Channel.SendMessageAsync("Ты не можешь ставить оценку самому себе!");
                        return;
                    }
                    /*
                    if (blogList[i].Available == 0)
                    {
                       await arg3.Channel.SendMessageAsync($"Ты уже голосовал! Сними прошлую оценку, чтобы поставить новую.");
                        continue;
                    }*/
                    var account = UserAccounts.GetAccount(v.BlogAuthor);
                    
                    switch (arg3.Emote.Name)
                    {
                        case "1⃣":
                            account.BlogVotesQty += 1;
                            account.BlogVotesSum += 1;
                            UserAccounts.SaveAccounts();
                            break;
                        case "2⃣":
                            account.BlogVotesQty += 1;
                            account.BlogVotesSum += 2;
                            UserAccounts.SaveAccounts();
                            break;
                        case "3⃣":
                            account.BlogVotesQty += 1;
                            account.BlogVotesSum += 3;
                            UserAccounts.SaveAccounts();
                            break;
                        case "4⃣":
                            account.BlogVotesQty += 1;
                            account.BlogVotesSum += 4;
                            UserAccounts.SaveAccounts();
                            break;
                        case "zazz":
                            account.BlogVotesQty += 1;
                            account.BlogVotesSum += 5;
                            UserAccounts.SaveAccounts();
                            break;
                    }   
                }
            }

            await Task.CompletedTask;
        }

        public static async Task Client_ReactionRemovedForBlog(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            var blogList = BlogVotesMessIdList;
            foreach (BlogVotes v in blogList)
            {


                if (v.SocketMsg.Id == arg1.Id && v.ReactionUser.Id == arg3.User.Value.Id)
                {
                    if (arg3.User.Value.Id == v.BlogAuthor.Id)
                    {
                        return;
                    }

                    var account = UserAccounts.GetAccount(v.BlogAuthor);
                    switch (arg3.Emote.Name)
                    {

                        case "1⃣":
                            account.BlogVotesQty--;
                            account.BlogVotesSum -= 1;
                            UserAccounts.SaveAccounts();
                            break;
                        case "2⃣":
                            account.BlogVotesQty--;
                            account.BlogVotesSum -= 2;
                            UserAccounts.SaveAccounts();
                            break;
                        case "3⃣":
                            account.BlogVotesQty--;
                            account.BlogVotesSum -= 3;
                            UserAccounts.SaveAccounts();
                            break;
                        case "4⃣":
                            account.BlogVotesQty--;
                            account.BlogVotesSum -= 4;
                            UserAccounts.SaveAccounts();
                            break;
                        case "zazz":
                            account.BlogVotesQty--;
                            account.BlogVotesSum -= 5;
                            UserAccounts.SaveAccounts();
                            break;
                    }
                    v.Available = 1;
                }
            }

            await Task.CompletedTask;
        }
    }
}
