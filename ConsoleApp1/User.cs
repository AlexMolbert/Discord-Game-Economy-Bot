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


public class UserCommands : ModuleBase
{

    [Command("register"), Summary("Creates user.")]
    public async Task Register()
    {
        var user_id = this.Context.Message.Author.AvatarId;
        long guild_id = (long)this.Context.Guild.Id;
        using (var db = new DatabaseContext())
        {
            db.Database.Initialize(true);
        }
        using (var db = new UserDbContext())
        {
            var user = new User
            {
                User_id = user_id,
                Guild_id = guild_id,
                Status = 1,
                Created_on = DateTime.Now
            };
            db.Database.CreateIfNotExists();

            var row = db.Users.Count(u => u.User_id == user_id && u.Guild_id == guild_id);
            if (row > 0)
            {
                this.Context.Channel.SendMessageAsync("User Already Exists On This Server");

            }
            else
            {


                db.Users.Add(user);
                db.SaveChanges();
                this.Context.Channel.SendMessageAsync("User successfully saved");

            }
        }
        using (var db = new WalletDbContext())
        {
            var wallet = new Wallet
            {
                Guid = Guid.NewGuid(),
                User_id = user_id,
                Guild_id = guild_id
            };

            var row = db.Wallets.Count(w => w.User_id == user_id && w.Guild_id == guild_id);
            if (row > 0)
            {
                this.Context.Channel.SendMessageAsync("Wallet Already Exists For This User On This Server");

            }
            else
            {
                db.Wallets.Add(wallet);
                db.SaveChanges();
                this.Context.Channel.SendMessageAsync("User Wallet successfully saved");
                using (var wdb = new WalletInfoDbContext())
                {
                    var walletInfo = new WalletInfo()
                    {
                        Guid = wallet.Guid,
                        Created_on = DateTime.Now,
                        Modified_on = DateTime.Now
                    };
                    wdb.Database.CreateIfNotExists();
                    wdb.WalletInfos.Add(walletInfo);
                    wdb.SaveChanges();
                    this.Context.Channel.SendMessageAsync("User Wallet Info successfully saved");
                }
            }
        }

    }

    [Command("wallet"), Summary("Echos a message.")]
    public async Task Wallet()
    {
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
                    var info = wdb.WalletInfos.SingleOrDefaultAsync(w => w.Guid == row.Guid);
                    var user = Context.User.Mention;
                    this.Context.Channel.SendMessageAsync(user + "Your wallet balance is : " + info.Result.Points);
                }

            }
        }
        else
        {
            this.Context.Channel.SendMessageAsync("You must !register first to obtain a wallet.");

        }

    }

    [Command("topup"), Summary("Echos a message.")]
    public async Task Topup()
    {
        var amount = 20;
        var time = 1;
        var user_id = this.Context.Message.Author.AvatarId;
        long guild_id = (long)this.Context.Guild.Id;
        DateTime current = DateTime.Now;

        using (var db = new DatabaseContext())
        {
            db.Database.Initialize(true);
        }
        using (var db = new WalletDbContext())
        {
            var row = db.Wallets.SingleOrDefault(u => u.User_id == user_id && u.Guild_id == guild_id);
            using (var wdb = new WalletInfoDbContext())
            {
                var info = wdb.WalletInfos.SingleOrDefaultAsync(w => w.Guid == row.Guid);
                var user = Context.User.Mention;
                if (info.Result.Modified_on <= current.AddHours(-time))
                {
                    WalletInfo walletinfo = (from x in wdb.WalletInfos where x.Guid == row.Guid select x).First();
                    walletinfo.Points = walletinfo.Points + amount;
                    walletinfo.Modified_on = current;
                    wdb.SaveChanges();
                    this.Context.Channel.SendMessageAsync(user + "Your new wallet balance is : " + walletinfo.Points);

                }
                else
                {
                    TimeSpan span = current.Subtract(info.Result.Modified_on);
                    this.Context.Channel.SendMessageAsync(user + "You may only top up once per hour. It has been " + (int)span.TotalMinutes + " minutes since your last topup.");         
                }
            }

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