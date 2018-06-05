using Discord;
using Discord.Commands;
using DiscordBot;
using Discord.WebSocket;
using System.Threading.Tasks;
using System;
using ConsoleApp1;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.Extensions;


public class HelpModule : ModuleBase<SocketCommandContext>
{
    [Command("help"), Summary("Flip a coin!.")]
    public async Task Help()
    {
        this.Context.Channel.SendMessageAsync("!register!wallet!topup!flip");
    }
}