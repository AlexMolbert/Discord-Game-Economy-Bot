namespace ConsoleApp1
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations;
    //using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    /// <summary>
    /// The person model.
    /// </summary>
    public class User
    {

        public int Id { get; set; }

        

        [MaxLength(35)]
        public string User_id { get; set; }
        
        //[Range(0, 35)]
        public long Guild_id { get; set; }

        //[Range(0, 3)]
        public int Status { get; set; }

        //public ICollection<Wallet> Wallets { get; set; }

        public DateTime Created_on { get; set; }
    }


    public class Wallet
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        //[Range(0, 35)]
        public string User_id { get; set; }

        //[Range(0, 35)]
        public long Guild_id { get; set; }
    }

    public class WalletInfo
    {
        public WalletInfo()
        {
            Points = 20;
            Won = 0;
            Lost = 0;
            TopupAmount = 0;
        }

        public int Id { get; set; }

        public Guid Guid { get; set; }

        public int Points { get; set; }

        public int Won { get; set; }

        public int Lost { get; set; }

        public int TopupAmount { get; set; }

        public int TopupCount { get; set; }

        public int TopupFail { get; set; }

        public DateTime Created_on { get; set; }

        public DateTime Modified_on { get; set; }
    }
}