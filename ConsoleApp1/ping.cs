using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System;

public class Ping : ModuleBase
{

    [Command("ping"), Summary("Echos a message.")]
    public async Task Pong()
    {
        this.Context.Channel.SendMessageAsync("pong");
    }

}

