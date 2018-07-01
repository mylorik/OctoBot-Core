﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OctoBot.Configs.Users;
using OctoBot.Handeling;
using OctoBot.Helper;
using OctoBot.Services;

namespace OctoBot.Commands
{
    public class UserCommand : ModuleBase<SocketCommandContextCustom>
    {
        [Command("stats")]
        [Alias("статы")]
        public async Task Xp()
        {
            try
            {
            var account = UserAccounts.GetAccount(Context.User, Context.Guild.Id);

            var avatar =
                ("https://cdn.discordapp.com/avatars/" + Context.User.Id + "/" + Context.User.AvatarId + ".png");

            var usedNicks = "";
            var usedNicks2 = "";
            if (account.ExtraUserName != null)
            {

                var extra = account.ExtraUserName.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < extra.Length; i++)
                {

                    if (i == extra.Length - 1)
                    {
                        usedNicks += (extra[i]);

                    }
                    else if (usedNicks.Length <= 1000)
                    {
                        usedNicks += (extra[i] + ", ");
                    }
                    else
                    {
                        usedNicks2 += (extra[i] + ", ");
                    }
                }

            }
            else
                usedNicks = "None";

            var octopuses = "";
            if (account.Octopuses != null)
            {
                var octo = account.Octopuses.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);



                for (var i = 0; i < octo.Length; i++)
                {

                    if (i == octo.Length - 1)
                    {
                        octopuses += (octo[i]);

                    }
                    else
                    {

                        octopuses += (octo[i] + ", ");
                    }

                }

            }
            else
            {
                octopuses = "None";
            }

            string[] warns = null;
            if (account.Warnings != null)
            {
                warns = account.Warnings.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            }

            var embed = new EmbedBuilder();

            embed.WithColor(Color.Blue);
            embed.WithAuthor(Context.User);
            embed.WithFooter("lil octo notebook");
            embed.AddField("ID", "" + Context.User.Id, true);
            embed.AddField("Status", "" + Context.User.Status, true);

            embed.AddField("UserName", "" + Context.User, true);

            embed.AddField("NickName", "" + Context.User.Mention, true);
            embed.AddField("Octo Points", "" + account.Points, true);
            embed.AddField("Octo Reputation", "" + account.Rep, true);
            embed.AddField("Access LVL", "" + account.OctoPass, true);
            embed.AddField("User LVL", "" + Math.Round(account.Lvl, 2), true );
            embed.AddField("Pull Points", "" + account.DailyPullPoints, true);
                embed.AddField("Best 2048 Game Score", $"{account.Best2048Score}", true);
            if (warns != null)
                embed.AddField("Warnings", "" + warns.Length, true);
            else
                embed.AddField("Warnings", "Clear.", true);
            
            embed.AddField("OctoCollection ", "" + octopuses);
            embed.AddField("Used Nicknames", "" + usedNicks);
            if (usedNicks2.Length >= 5)
                embed.AddField("Extra Nicknames", "" + usedNicks2);
            embed.WithThumbnailUrl($"{avatar}");
            //embed.AddField("Роли", ""+avatar);

                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, embed);
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, embed, "edit");
                }
        }
        catch
        {
              //  await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **Stats**");
            }
        }

        [Command("OctoRep")]
        [Alias("Octo Rep", "Rep", "октоРепа", "Окто Репа", "Репа")]
        //[RequireUserPermission(GuildPermission.Administrator)]
        public async Task AddPoints(IGuildUser user, long rep)
        {
            try{
            var comander = UserAccounts.GetAccount(Context.User, Context.Guild.Id);
            if (comander.OctoPass >= 100)
            {
                var account = UserAccounts.GetAccount((SocketUser) user, Context.Guild.Id);
                account.Rep += rep;
                UserAccounts.SaveAccounts(Context.Guild.Id);

                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,  $"{rep} Octo Reputation were credited, altogether {user.Mention} have {account.Rep} Octo Reputation!");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",  $"{rep} Octo Reputation were credited, altogether {user.Mention} have {account.Rep} Octo Reputation!");
                }

            }
            else
              
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,  "Boole! You do not have a tolerance of this level!");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",  "Boole! You do not have a tolerance of this level!");
                }
            }
            catch
            {
             //   await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **OctoRep [ping_user(or user ID)] [number_of_points]**\n" +
             //                    "Alias: Rep, октоРепа, Репа, Окто Репа");
            }
        }

        [Command("OctoPoint")]
        [Alias("Octo Point", "OctoPoints", "Octo Points", "ОктоПоинты", "Окто Поинты", "Поинты", "points", "point")]
       // [RequireUserPermission(GuildPermission.Administrator)]
        public async Task GivePoints(IGuildUser user, long points)
        {
            try {
            var comander = UserAccounts.GetAccount(Context.User, Context.Guild.Id);
            if (comander.OctoPass >= 100)
            {


                var account = UserAccounts.GetAccount((SocketUser) user, Context.Guild.Id);
                account.Points += points;
                UserAccounts.SaveAccounts(Context.Guild.Id);
             

                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,   $"{points} Octo Points were credited, altogether {user.Mention} have {account.Points} Octo Points!");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",   $"{points} Octo Points were credited, altogether {user.Mention} have {account.Points} Octo Points!");
                }
            }
            else
            if (Context.MessageContentForEdit != "edit")
            {
                await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,  "Boole! You do not have a tolerance of this level!");
  
            }
            else if(Context.MessageContentForEdit == "edit")
            {
                await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",  "Boole! You do not have a tolerance of this level!");
            }
            }
            catch
            {
              //  await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **OctoPoint [ping_user(or user ID)] [number_of_points]**\n" +
               //                  "Alias: OctoPoints, ОктоПоинты, Поинты, points, point");
            }
        }


        [Command("stats")]
        [Alias("Статы")]
        public async Task CheckUser(IGuildUser user)
        {
            try {
            var comander = UserAccounts.GetAccount(Context.User, Context.Guild.Id);
            if (comander.OctoPass >= 4)
            {


                var account = UserAccounts.GetAccount((SocketUser) user, Context.Guild.Id);

                var avatar = ("https://cdn.discordapp.com/avatars/" + user.Id + "/" + user.AvatarId + ".png");

                var usedNicks = "";
                var usedNicks2 = "";
                if (account.ExtraUserName != null)
                {

                    var extra = account.ExtraUserName.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
         
                    for (var i = 0; i < extra.Length; i++)
                    {
                    
                        if (i == extra.Length - 1)
                        {
                            usedNicks += (extra[i]);

                        }
                        else if(usedNicks.Length <= 1000)
                        {
                            usedNicks += (extra[i] + ", ");
                        }
                        else
                        {
                            usedNicks2 += (extra[i] + ", ");
                        }
                    }

                }
                else
                    usedNicks = "None";



                var octopuses = "";
                if (account.Octopuses != null)
                {
                    var octo = account.Octopuses.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);



                    for (var i = 0; i < octo.Length; i++)
                    {

                        if (i == octo.Length - 1)
                        {
                            octopuses += (octo[i]);

                        }
                        else
                        {

                            octopuses += (octo[i] + ", ");
                        }

                    }

                }
                else
                {
                    octopuses = "None";
                }

                var warnings = "None";
                if (account.Warnings != null)
                {
                    var warns = account.Warnings.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    warnings = "";
                    for (var index = 0; index < warns.Length; index++)
                    {
                        var t = warns[index];
                        warnings += t + "\n";
                    }
                }
                var embed = new EmbedBuilder();
               
                embed.WithColor(Color.Purple);
                embed.WithAuthor(user);
                embed.WithFooter("lil octo notebook");
                embed.AddField("ID", "" + user.Id, true);
                embed.AddField("Status", "" + user.Status, true);
                embed.AddField("Registered", "" + user.CreatedAt, true);
                embed.AddField("UserName", "" + user, true);
                embed.AddField("Joined", "" +  user.JoinedAt, true);
                embed.AddField("NickName", "" + user.Mention, true);
                embed.AddField("Octo Points", "" + account.Points, true);
                embed.AddField("Octo Reputation", "" + account.Rep, true);
                embed.AddField("Access LVL", "" + account.OctoPass, true);
                embed.AddField("User LVL", "" + Math.Round(account.Lvl, 2), true);
                embed.AddField("Pull Points", "" + account.DailyPullPoints, true);
                embed.AddField("Best 2048 Game Score", $"{account.Best2048Score}", true);
                embed.AddField("Warnings", "" + warnings);
                
                embed.AddField("OctoCollection ", "" + octopuses);
                embed.AddField("Used Nicknames", "" + usedNicks);
                if(usedNicks2.Length >=5)
                    embed.AddField("Extra Nicknames", "" + usedNicks2);
                embed.WithThumbnailUrl($"{avatar}");

                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, embed);
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, embed, "edit");
                }
            }else
            if (Context.MessageContentForEdit != "edit")
            {
                await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,  "Boole! You do not have a tolerance of this level!");
  
            }
            else if(Context.MessageContentForEdit == "edit")
            {
                await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",  "Boole! You do not have a tolerance of this level!");
            }
            }
            catch
            {
            //    await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **Stats [user_ping(or user ID)]**");
            }
        }

        [Command("pass", RunMode = RunMode.Async)]
        [Alias("Пасс", "Купить Пропуск", "Пропуск", "КупитьПропуск", "Доступ")]
        public async Task BuyPass()
        {
            try {
                var account = UserAccounts.GetAccount(Context.User, Context.Guild.Id);
                var cost = 4000 * (account.OctoPass + 1);

            if (account.Points >= cost)
            {
                await Context.Channel.SendMessageAsync(
                    $"Are you sure about buying pass #{account.OctoPass + 1} for {cost} Octo Points? Than write **yes**!");
                var response = await AwaitForUserMessage.AwaitMessage(Context.User.Id, Context.Channel.Id, 6000);
               
                if (response.Content == "yes" || response.Content == "Yes")
                {
                    account.OctoPass += 1;
                    account.Points -= cost;
                    UserAccounts.SaveAccounts(Context.Guild.Id);
                   
                    if (Context.MessageContentForEdit != "edit")
                    {
                        await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,   $"Booole! You've Got Access **#{account.OctoPass}**");
  
                    }
                    else if(Context.MessageContentForEdit == "edit")
                    {
                        await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",   $"Booole! You've Got Access **#{account.OctoPass}**");
                    }
                }
                else
                {
                  
                    if (Context.MessageContentForEdit != "edit")
                    {
                        await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,   "You should say `yes` или `Yes` in 6s to get the pass.");
  
                    }
                    else if(Context.MessageContentForEdit == "edit")
                    {
                        await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",  "You should say `yes` или `Yes` in 6s to get the pass.");
                    }
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync(
                    $"You did not earn enough Octo Points, current amount: **{account.Points}**\nFor pass #{account.OctoPass + 1} you will need **{cost}** Octo Points!");
            }
            }
            catch
            {
               // await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **pass**\n Alias: Пасс, Пропуск, Доступ, КупитьПропуск, Купить Пропуск");
            }
        }

        [Command("CheckLvlLOL")]
        public async Task Check(uint xp)
        {
            try {
            var level = (uint)Math.Sqrt((double)xp / 100);
            await Context.Channel.SendMessageAsync("Это " + level + "сможешь ли ты достичь высот?");
            }
            catch
            {
                await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **Stats**");
            }
        }

        [Command("GiftPoints")]
        [Alias("Gift Points", "GiftPoint", "Gift Point")]
        public async Task GidftPoints(IGuildUser user, long points)
        {
            try {
            var passCheck = UserAccounts.GetAccount(Context.User, Context.Guild.Id);

            if (passCheck.OctoPass >= 1)
            {

                if (points <= 0)
                {
                    
                    if (Context.MessageContentForEdit != "edit")
                    {
                        await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,   "You cannot send 0 or -number, boo!");
  
                    }
                    else if(Context.MessageContentForEdit == "edit")
                    {
                        await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",  "You cannot send 0 or -number, boo!");
                    }
                    return;
                }


                if (passCheck.Points >= points)
                {
                    var account = UserAccounts.GetAccount((SocketUser) user, Context.Guild.Id);

                    var taxes = points * 0.9;
                    var bot = UserAccounts.GetAccount(Context.Client.CurrentUser, Context.Guild.Id);


                    account.Points += (int) taxes;
                    passCheck.Points -= points;


                    var toBank = (points * 1.1) - points;
                    bot.Points += (int) toBank;
                    UserAccounts.SaveAccounts(Context.Guild.Id);

                    if (Context.MessageContentForEdit != "edit")
                    {
                        await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,   $"Was transferred{points}\n {user.Mention} now have {account.Points} Octo Points!\nyou have left {passCheck.Points}\ntaxes: {taxes}");
  
                    }
                    else if(Context.MessageContentForEdit == "edit")
                    {
                        await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",  $"Was transferred{points}\n {user.Mention} now have {account.Points} Octo Points!\nyou have left {passCheck.Points}\ntaxes: {taxes}");
                    }

                }
                else
                    
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,   $"You do not have enough Octo Points to pass them.");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",  $"You do not have enough Octo Points to pass them.");
                }

            }
            else
            {
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, null,  "Boole! You do not have a tolerance of this level!");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandelingSendingAndUpdatingMessages.SendingMess(Context, null, "edit",  "Boole! You do not have a tolerance of this level!");
                }
            }
            }
            catch
            {
           //     await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **GiftPoints [ping_user(or user ID)] [number_of_points]**\nAlias: GiftPoint ");
            }
        }

    }
}
