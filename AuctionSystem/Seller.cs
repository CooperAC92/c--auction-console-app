using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem
{
    /// <summary>
    ///  Seller Class
    /// </summary>
    ///  <remarks>
    ///  The seller class represents our sellers in the system and inherits from the user class.
    /// </remarks>
    /// 
    public class Seller : User
    {
        private bool blocked;
        List<Auction> myAuctions = new List<Auction>();

        public Seller(string name, string pass) : base(name, pass) 
        {

        }

        public override void addAuction(Auction newAuc)
        {
            newAuc = new Auction(" ");
            myAuctions.Add(newAuc);
        }

        public override void showAuctions()
        {
            foreach(Auction auc in myAuctions)
            {
                int i =1;
                Console.WriteLine("Current Auction number: {0}", i);
                Console.WriteLine("Current Auction name: {0}", auc.getName());
            }
        }

        public bool isBlocked()
        {
            return blocked;
        }

        public void setBlocked()
        {
            blocked = true;
        }

        public override string getType()
        {
            string temp = "Seller";
            return temp;
        }
    }
}
