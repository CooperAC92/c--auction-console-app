using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem
{
    /// <summary>
    ///  Abstract User Class
    /// </summary>
    ///  <remarks>
    ///  This is an abstract class that both Seller class and Buyer class will inherit from.
    /// </remarks>
    /// 
    public abstract class User 
    {
        //-----------------------CLASS ATTRIBUTES---------------------------
        protected string USERNAME;
        protected string PASSWORD;
        bool passSuccess;
        bool loggedIn = false;
        List<Bid> tbids = new List<Bid>(); //generic declaration ?

        //---------------------CONSTRUCTOR---------------------------------
        public User(string name, string pass)
        {
            USERNAME = name;
            PASSWORD = pass;
        }

        //--------------------PUBLIC METHODS------------------------------

        public void addBid(Bid nbid)
        {
            tbids.Add(nbid);
        }

        public void createUser(string user, string pass)
        {
            USERNAME = user;
            PASSWORD = pass;
        }

        public void setName(string tname)
        {
            USERNAME = tname;
        }

        public string getName()
        {
            return USERNAME;
        }

        public void setLoggedIn(bool success)
        {
            loggedIn = success;
        }

        public bool isLoggedIn()
        {
            return loggedIn;
        }

        /// <summary>
        ///  Check Password function
        /// </summary>
        ///  <remarks>
        ///  This function checks whether the password entered matches 
        ///  the correct password for the user and returns true if succesful.
        /// </remarks>
        /// 
        public bool checkPassword(string attempt)
        {

            if (PASSWORD == attempt)
            {
                passSuccess = true;
                return passSuccess;
            }
            else
            {
                passSuccess = false;
                return passSuccess;
            }
        }

        //---------ABSTRACT CLASSES THAT WILL BE OVERLOADED-----------------//
        public abstract string getType();

        public abstract void showAuctions();

        public abstract void addAuction(Auction newAuc);   

    }
}
