using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem
{
    /// <summary>
    ///  Buyer Class
    /// </summary>
    ///  <remarks>
    ///  The Buyer class represents our buyers in the system and inherits from the user class.
    /// </remarks>
    /// 
    public class Buyer : User
    {
        //-----------------------CLASS ATTRIBUTES---------------------------
        List<Auction> myWins = new List<Auction>(); // list to hold auctions the buyer has won
  
        //-----------------------CONSTRUCTOR---------------------------------
        public Buyer(string name, string pass) : base(name, pass) 
        {

        }

        //---------------------PUBLIC METHODS-------------------------------
        // not sure what this method is to do
        public void victory()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("     Congratulations {0} you have won the auction!", getName());
            //myWins.Add(win);
        }

        public override string getType()
        {
            string temp = "Buyer";
            return temp;
        }

        
        public override void showAuctions()
        {
            //Console.WriteLine("my auctions");
        }

        public override void addAuction(Auction newAuc)
        {
           // Console.WriteLine("Buyer cannot create auction");
        }
    }
}
