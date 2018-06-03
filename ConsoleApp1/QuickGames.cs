using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System;
using ConsoleApp1;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;


public class QuickGamesCommands : ModuleBase
{

    [Command("flip"), Summary("Flip a coin!.")]
    public async Task Flip()
    {
        int amount = 2;
        Random rand = new Random();
        bool modified = false;
        bool win = false;
        var user_id = this.Context.Message.Author.AvatarId;
        long guild_id = (long)this.Context.Guild.Id;
        using (var db = new DatabaseContext())
        {
            db.Database.Initialize(true);
        }

        if(this.ValidateUser(user_id,guild_id))
        {
            using (var db = new WalletDbContext())
            {
                var row = db.Wallets.SingleOrDefault(u => u.User_id == user_id && u.Guild_id == guild_id);
                using (var wdb = new WalletInfoDbContext())
                {
                    var info = wdb.WalletInfos.SingleOrDefault(w => w.Guid == row.Guid);
                    var user = Context.User.Mention;


                    WalletInfo walletinfo = (from x in wdb.WalletInfos where x.Guid == row.Guid select x).First();
                    if (rand.Next(0, 2) == 0)
                    {
                        await this.Context.Channel.SendMessageAsync("Heads!");              
                        walletinfo.Points = walletinfo.Points + amount;
                        modified = true;
                        win = true;
                    }
                    else
                    {
                        if(walletinfo.Points - amount > 0)
                        {
                            await this.Context.Channel.SendMessageAsync("Tails!");                    
                            walletinfo.Points = walletinfo.Points - amount;
                            modified = true;
                            win = false;
                        }
                        else
                        {
                            this.Context.Channel.SendMessageAsync(user + "You may not bet more than is avaiable in your wallet. Amount to bet: " + amount + " Amount in Wallet: " + walletinfo.Points);

                        }
                    }
                    if(modified)
                    {
                        wdb.SaveChanges();
                        if(win)
                        {
                            this.Context.Channel.SendMessageAsync(user + "You Win! Your new wallet balance is : " + walletinfo.Points);
                        }
                        else
                        {
                            this.Context.Channel.SendMessageAsync(user + "You Lose. Your new wallet balance is : " + walletinfo.Points);
             
                        }
                    }                    
                }
            }
        }
        else
        {
            this.Context.Channel.SendMessageAsync("You must !register for an account before using this command. (Type !help for help)");
        }

    }

    private bool ValidateUser(string user_id, long guild_id)
    {
        using (var db = new UserDbContext())
        {
            var row = db.Users.Count(u => u.User_id == user_id && u.Guild_id == guild_id);
            if (row > 0)
            {
                return true;
            }
            else
            {
                return false;
   
            }
        }

    }
}