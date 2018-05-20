﻿using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using OctoBot.Configs.Users;

namespace OctoBot.Commands
{
    public class OctoGift : ModuleBase<SocketCommandContext>
    {
        [Command("GiftCooki")]
        [Alias("Gift Cooki","подаритьКуки", "Подарить Куки")]
        public async Task GiftCooki(IGuildUser user)
        {

            var contextUser = UserAccounts.GetAccount(Context.User);

            var account = UserAccounts.GetAccount((SocketUser)user);
            if (account.Cooki >= 1)
            {
                await Context.Channel.SendMessageAsync($"{user.Mention} Already have Cooki, you should choose another Octopus or Turtle!");
                return;
            }

            if (contextUser.Points >= 1488)
            {

                contextUser.Points -= 1488;
                UserAccounts.SaveAccounts();

                account.Cooki += 1;
                account.Octopuses += ("Cooki|");
                UserAccounts.SaveAccounts();

                var embed = new EmbedBuilder();
                embed.WithColor(244, 66, 107);
                embed.WithTitle($"You gave {user} Cooki!!");
                embed.WithFooter("lil octo notebook");
                embed.AddField("Fees was applied (1488 OctoPoints)", $"{contextUser.Points} Octo Points left");
                embed.WithImageUrl("https://i.imgur.com/dCJwloV.jpg");

                await Context.Channel.SendMessageAsync("", embed: embed);

            }
            else
            {
                await Context.Channel.SendMessageAsync($"You do not have enough OktoPoints to give **Cooki**!");
            }

        }

        [Command("GiftPinki")]
        [Alias("Gift Pinki", "Подарить Пинки", "ПодаритьПинки")]
        public async Task GiftPinki(IGuildUser user)
        {

            var contextUser = UserAccounts.GetAccount(Context.User);

            var account = UserAccounts.GetAccount((SocketUser)user);
            if (account.Pinki >= 1)
            {
                await Context.Channel.SendMessageAsync($"{user.Mention} Already have Pinki, you should choose another Octopus or Turtle!");
                return;
            }

            if (contextUser.Points >= 1488)
            {

                contextUser.Points -= 1488;
                UserAccounts.SaveAccounts();

                account.Pinki += 1;
                account.Octopuses += ("Pinki|");
                UserAccounts.SaveAccounts();

                var embed = new EmbedBuilder();
                embed.WithColor(244, 66, 107);
                embed.WithTitle($"You gave {user} Pinki!!");
                embed.WithFooter("lil octo notebook");
                embed.AddField("Fees was applied (1488 OctoPoints)", $"{contextUser.Points} Octo Points left");
                embed.WithImageUrl("https://i.imgur.com/xxE7EeX.jpg");

                await Context.Channel.SendMessageAsync("", embed: embed);

            }
            else
            {
                await Context.Channel.SendMessageAsync($"You do not have enough OktoPoints to give **Pinki**!");
            }

        }




        [Command("GiftRainbow")]
        [Alias("Gift Rainbow", "Подарить рудужного", "Подарить радужный", "ПодаритьРадужный", "ПодаритьРадужного")]
        public async Task GiftRainbow(IGuildUser user)
        {

            var contextUser = UserAccounts.GetAccount(Context.User);

            var account = UserAccounts.GetAccount((SocketUser)user);
            if (account.Raqinbow >= 1)
            {
                await Context.Channel.SendMessageAsync($"{user.Mention} Already have Rainbow, you should choose another Octopus or Turtle!");
                return;
            }

            if (contextUser.Points >= 1488)
            {

                contextUser.Points -= 1488;
                UserAccounts.SaveAccounts();

                account.Raqinbow += 1;
                account.Octopuses += ("Rainbow|");
                UserAccounts.SaveAccounts();

                var embed = new EmbedBuilder();
                embed.WithColor(244, 66, 107);
                embed.WithFooter("lil octo notebook");
                embed.WithTitle($"You gave {user} Rainbow!!");
                embed.AddField("Fees was applied (1488 OctoPoints)", $"{contextUser.Points} Octo Points left");
                embed.WithImageUrl("https://i.imgur.com/Ufky6UB.jpg");

                await Context.Channel.SendMessageAsync("", embed: embed);

            }
            else
            {
                await Context.Channel.SendMessageAsync($"You do not have enough OktoPoints to give **Rainbow**!");
            }

        }

        [Command("AllOcto")]
        [Alias("All Octo", "ВсеОкто", "Все Окто")]
        public async Task AllOcto()
        {
            var embed = new EmbedBuilder();
            embed.WithColor(Color.Blue);
            embed.WithFooter("lil octo notebook");
            embed.WithTitle("Available Octopus:");
            embed.AddField("**Cooki!**", "`GiftCooki [user]` cost: **1488** ОктоПоинтов");
            embed.AddField("**Pinki~**", "`GiftPinki [user]` cost: **1488** ОктоПоинтов");
            embed.AddField("**Rainbow** :gay_pride_flag:", "`GiftRainbow [user]` cost: **1488** Octo Points");
            await Context.Channel.SendMessageAsync("", embed: embed);


        }

    }
}
