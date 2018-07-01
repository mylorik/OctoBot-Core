﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using OctoBot.Configs;
using OctoBot.Configs.Users;
using OctoBot.Handeling;
using OctoBot.Services;

namespace OctoBot.Commands
{
   public class Top : ModuleBase<SocketCommandContextCustom>
    {
        [Command("topo")]
        [Alias("topp")]
        public async Task TopByOctoPoints(int page = 1)
        {
            try {
            if (page < 1)
            {
                
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, null,  "Are you fucking sure about that?");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, "edit",  "Are you fucking sure about that?");
                }
                return;
            }

            var currentGuildUsersId = Context.Guild.Users.Select(user => user.Id);
            // Get only accounts of this server
            var accounts = UserAccounts.GetFilteredAccounts(acc => currentGuildUsersId.Contains(acc.Id), Context.Guild.Id);

            const int usersPerPage = 9;

            var lastPage = 1 + (accounts.Count / (usersPerPage+1));
            if (page > lastPage)
            {
                
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, null, $"Boole. Last Page is {lastPage}");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, "edit", $"Boole. Last Page is {lastPage}");
                }
                return;
            }
       
            var ordered = accounts.OrderByDescending(acc => acc.Points).ToList();

            var embB = new EmbedBuilder()
                .WithTitle("Top By Octo Points:")
                .WithFooter($"Page {page}/{lastPage}");
           


            page--;
                for (var j = 0; j < ordered.Count; j++)
                {
                    if(ordered[j].Id == Context.User.Id)
                        embB.WithDescription($"**#{j + usersPerPage * page + 1} {Context.User.Username} {ordered[j].Points} OctoPoints**\n**______**");

                }
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
              
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);
                   embB.AddField($"#{i + usersPerPage * page} {user.Username}", $"{account.Points} OctoPoints", true);
            }

           
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, embB);
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, embB, "edit");
                }
            }
            catch
            {
                await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **topp [page_number]**(Top By Activity)\nAlias: topo");
            }
        }

        [Command("tops")]
        public async Task TopBySubc(int page = 1)
        {
            try {
            if (page < 1)
            {
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, null,  "Are you fucking sure about that?");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, "edit",  "Are you fucking sure about that?");
                }
                return;
            }

            // Get only accounts of this server
            var accounts = UserAccounts.GetFilteredAccounts(acc => Context.Guild.Users.Select(user => user.Id).Contains(acc.Id), Context.Guild.Id);

            const int usersPerPage = 9;

            var lastPage = 1 + (accounts.Count / (usersPerPage+1));
            if (page > lastPage)
            {
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, null, $"Boole. Last Page is {lastPage}");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, "edit", $"Boole. Last Page is {lastPage}");
                }
                return;
            }

            foreach (var t in accounts)
            {
                if (t.SubedToYou == null)
                    t.SubedToYou = "0";
            }
            var ordered = accounts.OrderByDescending(acc => acc.SubedToYou.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries).Length).ToList();

            var embB = new EmbedBuilder()
                .WithTitle("Top By Subs Qty:")
                .WithFooter($"Page {page}/{lastPage}");
   
            page--;
                for (var j = 0; j < ordered.Count; j++)
                {
                    if (ordered[j].Id == Context.User.Id)
                    {
                        var size = ordered[j].SubedToYou.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries).Length;
                        if ( size == 1)
                            size = 0;
                        embB.WithDescription(
                            $"**#{j + usersPerPage * page + 1} {Context.User.Username} {size} Subscribers**\n**______**");
                    }
                }
        
            
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
               
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);

                var size = account.SubedToYou.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries).Length;
                if ( size == 1)
                    size = 0;
             embB.AddField($"#{i + usersPerPage * page} {user.Username}", $"{size} Subscribers", true);
            }
            
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, embB);
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, embB, "edit");
                }
            }
            catch
            {
                await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **tops [page_number]**(Top By Subs Qty)");
            }
        }

        [Command("top")]
        [Alias("topl")]
        public async Task TopByLvL(int page = 1)
        {
            try {
            if (page < 1)
            {
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, null,  "Are you fucking sure about that?");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, "edit",  "Are you fucking sure about that?");
                }
                return;
            }


            var currentGuildUsersId = Context.Guild.Users.Select(user => user.Id);
            // Get only accounts of this server
            var accounts = UserAccounts.GetFilteredAccounts(acc => currentGuildUsersId.Contains(acc.Id), Context.Guild.Id);


            foreach (var t in accounts)
            {
                t.Lvl = Math.Sqrt((double)t.LvlPoinnts / 150);
                UserAccounts.SaveAccounts(Context.Guild.Id);
            }

            const int usersPerPage = 9;

            var lastPage = 1 + (accounts.Count / (usersPerPage+1));
            if (page > lastPage)
            {
               
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, null, $"Boole. Last Page is {lastPage}");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, "edit", $"Boole. Last Page is {lastPage}");
                }
                return;
            }
       
            var ordered = accounts.OrderByDescending(acc => acc.Lvl).ToList();

            var embB = new EmbedBuilder()
                .WithTitle("Top By Activity:")
                .WithFooter($"Page {page}/{lastPage}");


            page--;
                for (var j = 0; j < ordered.Count; j++)
                {
                    if(ordered[j].Id == Context.User.Id)
                    embB.WithDescription($"**#{j + usersPerPage * page + 1} {Context.User.Username} {Math.Round(ordered[j].Lvl, 2)} LVL**\n**______**");
                }
               
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
              
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);  
                embB.AddField($"#{i + usersPerPage * page} {user.Username}", $"{Math.Round(account.Lvl, 2)} LVL", true);
            }

                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, embB);
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, embB, "edit");
                }
            }
            catch
            {
                await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **top [page_number]**(Top By Activity)\nAlias: topl");
            }
        }

        [Command("topr")]
        [Alias("topb")]
        public async Task TopByRating(int page = 1)
        {
            try {
            if (page < 1)
            {
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, null,  "Are you fucking sure about that?");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, "edit",  "Are you fucking sure about that?");
                }
                return;
            }


            var currentGuildUsersId = Context.Guild.Users.Select(user => user.Id);
            // Get only accounts of this server
            var accounts = UserAccounts.GetFilteredAccounts(acc => currentGuildUsersId.Contains(acc.Id), Context.Guild.Id);


            foreach (var t in accounts)
            {
                if (t.BlogVotesQty <= 0)
                {
                    t.BlogAvarageScoreVotes = (float)0.0;
                }
                else
                {
                    t.BlogAvarageScoreVotes = (float)t.BlogVotesSum / t.BlogVotesQty;
                }

                UserAccounts.SaveAccounts(Context.Guild.Id);
            }

            const int usersPerPage = 9;

            var lastPage = 1 + (accounts.Count / (usersPerPage+1));
            if (page > lastPage)
            {
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, null, $"Boole. Last Page is {lastPage}");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, "edit", $"Boole. Last Page is {lastPage}");
                }
                return;
            }
       
            var ordered = accounts.OrderByDescending(acc => acc.BlogAvarageScoreVotes).ToList();

            var embB = new EmbedBuilder()
                .WithTitle("Top By Avarage Rating in Blog:")
                .WithFooter($"Page {page}/{lastPage}");


            page--;
                for (var j = 0; j < ordered.Count; j++)
                {
                    if(ordered[j].Id == Context.User.Id)
                        embB.WithDescription($"**#{(j + usersPerPage * page) +1} {Context.User.Username} {Math.Round(ordered[j].BlogAvarageScoreVotes, 2)}" +
                                             $" out of 5 ({ordered[j].BlogVotesQty} votes)**\n**______**");

                }
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
              
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);
              embB.AddField($"#{i + usersPerPage * page} {user.Username}", $"**{Math.Round(account.BlogAvarageScoreVotes, 2)}** out of 5 ({account.BlogVotesQty} votes)", true);
            }

                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, embB);
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, embB, "edit");
                }
            }
            catch
            {
                await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **topr [page_number]**(Top by blog rating)\nAlias: topb");
            }
        }

        [Command("topa")]
        public async Task TopByRatingArt(int page = 1)
        {
            try {
            if (page < 1)
            {
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, null,  "Are you fucking sure about that?");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, "edit",  "Are you fucking sure about that?");
                }
                return;
            }


            var currentGuildUsersId = Context.Guild.Users.Select(user => user.Id);
            // Get only accounts of this server
            var accounts = UserAccounts.GetFilteredAccounts(acc => currentGuildUsersId.Contains(acc.Id), Context.Guild.Id);


            foreach (var t in accounts)
            {
                if (t.ArtVotesQty <= 0)
                {
                    t.ArtAvarageScoreVotes = (float)0.0;
                }
                else
                {
                    t.ArtAvarageScoreVotes = (float)t.ArtVotesSum / t.ArtVotesQty;
                }

                UserAccounts.SaveAccounts(Context.Guild.Id);
            }

            const int usersPerPage = 9;

            var lastPage = 1 + (accounts.Count / (usersPerPage+1));
            if (page > lastPage)
            {
                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, null, $"Boole. Last Page is {lastPage}");
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, null, "edit", $"Boole. Last Page is {lastPage}");
                }
                return;
            }
       
            var ordered = accounts.OrderByDescending(acc => acc.ArtAvarageScoreVotes).ToList();
            
            var embB = new EmbedBuilder()
                .WithTitle("Top By :art: Rating Messages:")
                .WithFooter($"Page {page}/{lastPage}");


            page--;
                for (var j = 0; j < ordered.Count; j++)
                {
                    if(ordered[j].Id == Context.User.Id)
                        embB.WithDescription($"**#{(j + usersPerPage * page) + 1} {Context.User.Username} {Math.Round(ordered[j].ArtAvarageScoreVotes, 2)} out of 5 ({ordered[j].BlogVotesQty} votes)**\n**______**");

                }
            for (var i = 1; i <= usersPerPage && i + usersPerPage * page <= ordered.Count; i++)
            {
              
                var account = ordered[i - 1 + usersPerPage * page];
                var user = Global.Client.GetUser(account.Id);
                 embB.AddField($"#{i + usersPerPage * page} {user.Username}", $"**{Math.Round(account.ArtAvarageScoreVotes, 2)}** out of 5 ({account.ArtVotesQty} votes)", true);
            }

                if (Context.MessageContentForEdit != "edit")
                {
                    await CommandHandeling.SendingMess(Context, embB);
  
                }
                else if(Context.MessageContentForEdit == "edit")
                {
                    await CommandHandeling.SendingMess(Context, embB, "edit");
                }
            }
            catch
            {
                await ReplyAsync("boo... An error just appear >_< \nTry to use this command properly: **topa [page_number]**(Top by :art: rating messages)");
            }
        }
    }
}
