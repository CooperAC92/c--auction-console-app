using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem
{
    public class Auction
    {
        //-------CONSTRUCTORS--------------------------
        public Auction(string aucName)
        {
            name = aucName;
            startPrice = 0;
            reservePrice = 0;
            closeDate = DateTime.Now.AddMinutes(1);
            status = 'n';
            myItem = new Item();
            isRunning = false;
            ID = 0;
        }

        public Auction(string aucName, double s_price, double reserve)
        {
            name = aucName;
            startPrice = s_price;
            reservePrice = reserve;
            closeDate = DateTime.Now.AddMinutes(1);
            status = 'n';
            myItem = new Item();
            isRunning = false;
            ID = 0;
            block = false;
        }

        public Auction(string aucName, double s_price, double reserve, DateTime close, bool trunning)
        {
            name = aucName;
            startPrice = s_price;
            reservePrice = reserve;
            closeDate = close;
            status = 'n';
            myItem = new Item();
            isRunning = trunning;
            ID = 0;
        }

        //--------CLASS ATTRIBUTES----------------------
        private double startPrice;
        private double reservePrice;
        private double currentPrice;
        private int ID; 
        private DateTime closeDate;
        private DateTime finishTime;
        private char status;
        private string name = " ";
        string currentBidder = " ";
        bool isRunning;
        bool block;
        bool sold = false;

        List<Bid> bids = new List<Bid>();
        Item myItem;
        Seller auctionSeller;
        Buyer winner;

        //----------GETTERS / SETTERS------------------
        public void setStartPrice(double price)
        {
            startPrice = price;
        }

        public void setSold(bool outcome)
        {
            sold = outcome;
        }

        public void setBlocked(bool tblock)
        {
            block = tblock;
        }

        public bool getBlocked()
        {
            return block;
        }

        public bool isSold()
        {
            return sold;
        }

        public void setID(int tID)
        {
            ID = tID;
        }

        public int getID()
        {
            return ID;
        }

        public void setReservePrice(double reserve)
        {
            reservePrice = reserve;
        }

        public void setBuyer(Buyer tbuyer)
        {
            winner = tbuyer;
        }

        public void setFinishTime(DateTime now)
        {
            finishTime = now;
        }

        public DateTime getFinishTime()
        {
            return finishTime;
        }

        public Buyer getBuyer()
        {
            return winner;
        }

        public string getBuyerName()
        {
            if (winner == null || getCurrentPrice() < getReservePrice()) // was throwing null exception event
            {
                Console.ForegroundColor = ConsoleColor.Red;
                string str = "No winner.";
                Console.ForegroundColor = ConsoleColor.White;
                return str;               
            }
            else
                return winner.getName();
        }

        public void setCloseDate(DateTime close)
        {
            closeDate = close;
        }

        public void setStatus(char t_status)
        {
            status = t_status;
        }

        public void setIsRunning(bool running)
        {
            isRunning = running;
        }

        public bool getIsRunning()
        {
            return isRunning;
        }

        public void setCurrentPrice(double amt)
        {
            currentPrice = amt;
        }

        public double getCurrentPrice()
        {
            return currentPrice;
        }

        public string getName()
        {
            return name;
        }

        public double getStartPrice()
        {
            return startPrice;
        }

        public double getReservePrice()
        {
            return reservePrice;
        }

        public DateTime getCloseDate()
        {
            return closeDate;
        }

        public char getStatus()
        {
            return status;
        }

        //----------PUBLIC METHODS--------------------
        public void placeBid(Bid tbid, double amt, Buyer buyer, DateTime date)
        {
            tbid = new Bid(amt, buyer, date);
            Bid _bid = tbid;
            if (_bid.getAmount() > getCurrentPrice())
            {
                bids.Add(_bid);
                //testing
                Console.WriteLine("Bid added! View bids below:");
                currentBidder = _bid.getBuyer().getName();
                browseBids();
            }
        }

        public string getCurrentBidder()
        {
            return currentBidder;
        }
        public void browseBids()
        {
            foreach (Bid tbid in bids)
            {
                Console.WriteLine("Bid : {0}, Bidder : {1}", tbid.getAmount(), tbid.getBuyer().getName());
            }
        }

        //function checking to see if an auction reached its reserve price
        public void verify() 
        {    
            if (getCurrentPrice() >= getReservePrice())
            {
                setSold(true);
                setStatus('C');
                setIsRunning(false);
                winner.victory(); 
            }
            else
            {
                setSold(false);
                setStatus('C');
                setIsRunning(false);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Auction : {0} Reserve not met. Auction cancelled.", getName());
            }
        }

        //used in the thread
        public void checkToClose()
        {
            if (DateTime.Now >= getCloseDate() && getBuyerName() != null) 
            {
                setIsRunning(false);
                setFinishTime(DateTime.Now);
                verify();          
            }
        }
    }
}
