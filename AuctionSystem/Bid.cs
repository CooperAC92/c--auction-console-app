using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem
{
    /// <summary>
    ///  Bid Class
    /// </summary>
    ///  <remarks>
    ///  A class to enable our buyers to make bids on auctions.
    /// </remarks>
    /// 
    public class Bid
    {
        //-------CLASS ATTRIBUTES--------------
        private double amount;
        private Buyer bidder;
        private DateTime timeOfDate;

        //------CONSTRUCTORS------------------
        public Bid(double amt, Buyer buyer, DateTime when)
        {
            this.amount = amt;
            this.bidder = buyer;
            this.timeOfDate = when;
        }

        //-------GETTERS / SETTERS-------------
        public void setAmount(double bid)
        {
            amount = bid;
        }

        public void getBuyer(Buyer bidder)
        {
            this.bidder = bidder;
        }

        public void setDate(DateTime bidDate)
        {
            timeOfDate = bidDate;
        }

        public double getAmount()
        {
            return amount;
        }

        public Buyer getBuyer()
        {
            return this.bidder;
        }

        public DateTime getDate()
        {
            return timeOfDate;
        }
    }
}
