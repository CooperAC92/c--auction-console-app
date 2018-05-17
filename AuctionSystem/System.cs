using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

/// <summary>
///  The System Class
/// </summary>
///  <remarks>
///  This is the main class in the application. 
/// </remarks>
/// 
namespace AuctionSystem
{
    public class System
    {
        //--------CLASS ATTRIBUTES-------------------
        //two lists holding our system user and auctions
        List<User> SystemUsers = new List<User>();
        List<Auction> Auctions = new List<Auction>();
        User tcurrent = null; // this will hold the current user logged in, abstract class initialised to null
        char type = ' ';

        //--------SAMPLE HARDCODE USER DATA--------------
        User adam = new Seller("adam", "abc123");
        User glo = new Buyer("glo", "112233");
        User mike = new Seller("mike", "12345");
        User John = new Buyer("john", "abcd");

        public void createSampleUser()
        {
            SystemUsers.Add(adam);
            SystemUsers.Add(glo);
            SystemUsers.Add(mike);
            SystemUsers.Add(John);
        }

        //-------PUBLIC METHODS---------------------

        /// <summary>
        ///  This function is used to run our auction.
        /// </summary>
        ///  <remarks>
        ///  Run the application loop
        /// </remarks>
        public void run()
        {
            //variables to hold our user menu choice
            string userInput;
            int choice;
            int exit = 5;
            // --------------- Sample Auctions ------------------------------------------
            Auction test;
            test = new Auction("Test");
            placeAuction(test);
            test.setCurrentPrice(99.99);
            test.setReservePrice(250);
            test.setCloseDate(DateTime.Now.AddSeconds(30));
            test.setIsRunning(true);

            Auction car;
            car = new Auction("car");
            car.setCurrentPrice(2000.99);
            car.setCloseDate(DateTime.Now.AddMinutes(7));
            car.setIsRunning(true);
            car.setReservePrice(3000);
            placeAuction(car);

            Auction laptop;
            laptop = new Auction("Laptop", 100, 300);
            laptop.setCloseDate(DateTime.Now.AddMinutes(2));
            laptop.setIsRunning(true);
            laptop.setCurrentPrice(laptop.getStartPrice());
            placeAuction(laptop);

            Auction ipad;
            ipad = new Auction("ipad", 150, 300, DateTime.Now.AddMinutes(3), true);
            ipad.setCurrentPrice(ipad.getStartPrice());
            placeAuction(ipad);

            //create a thread to automatically close auctions
            Thread checkAuctions = new Thread(threadedCheckAucClose);
            checkAuctions.Start();

            //------------------------------------------------------
            do
            {
                //load the menu and sample data and wait for user response
                Console.ResetColor();
                //Console.Clear();

                currentUser();
                createSampleUser();
                displayMenu();
                userInput = Console.ReadLine();

                //Handles format exception if user enters invalid characters
                if (!Int32.TryParse(userInput, out choice) || choice > 6)
                {
                    Console.WriteLine("Please enter valid input.");
                }
                else
                {
                    choice = Int32.Parse(userInput);
                } 

                switch(choice)
                {
                    case 1:
                        logIn();
                        break;
                    case 2:
                        createUser();
                        break;
                    case 3:
                        browseAuction();
                        break;
                    case 4:
                        if(tcurrent == null)
                        {
                            break;
                        }
                        if (tcurrent.isLoggedIn() == true) // only show this menu if user is logged in
                        {
                            createAuction();
                        }
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                    case 6:
                        if (tcurrent.isLoggedIn() == true) // log off user
                        {
                            tcurrent.setLoggedIn(false);
                        }
                        break;
                }                              
            } while (choice != exit);
        }
        
        public void logIn()
        {
            Console.WriteLine("Please enter username: ");
            string user = Console.ReadLine();
            Console.WriteLine("Please enter Password: ");
            string pass = Console.ReadLine();
            checkLogIn(user, pass); // take the username and password entered check if the login is successful 
        }

        public void createUser()
        {
            Console.WriteLine("Please enter username: ");
            string user = Console.ReadLine();
            Console.WriteLine("Please enter Password: ");
            string pass = Console.ReadLine();
            Console.WriteLine("Setup account as buyer or seller? (enter b or s):");
            type = Console.ReadKey().KeyChar;
            setupAccount(user, pass, type);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Congratulations you created an account! Login to use the Auction System.");
        }

        /// <summary>
        ///  A function for creating an auction.
        /// </summary>
        ///  <remarks>
        ///  This function takes the user through the process of creating an auction.
        /// </remarks>
        /// 
        public void createAuction()
        {
            // only display if user is a seller
            if (tcurrent.GetType() == typeof(Seller))
            {
                Console.WriteLine("Welcome to Auction screen!");
                Console.WriteLine("What are you selling?");
                string name = Console.ReadLine();

                Auction tauc;
                tauc = new Auction(name);

                Console.WriteLine("What would you like the starting price to be?");
                double start_price = double.Parse(Console.ReadLine());
                tauc.setStartPrice(start_price);
                tauc.setCurrentPrice(start_price);

                Console.WriteLine("Set a reserve price:");
                double reserve = double.Parse(Console.ReadLine());
                tauc.setReservePrice(reserve);

                Console.WriteLine("How long in minutes would you like the auction to last: ");
                int minutes = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Press Y or N to confirm or cancel auction:");
                string startAuction = Console.ReadLine();

                tauc.setStatus('p'); // set status to p for pending

                if (startAuction == "y" || startAuction == "Y")
                {
                    tauc.setIsRunning(true);
                    Console.WriteLine("Success Auction Created! It will end at: ");
                    DateTime current = DateTime.Now;
                    tauc.setCloseDate(current.AddMinutes(minutes));
                    Console.WriteLine(tauc.getCloseDate().ToString("h:mm:ss tt"));
                    getCurrentUser().addAuction(tauc);
                    placeAuction(tauc);
                }
                else
                {
                    Console.WriteLine("Auction cancelled :(");
                }
            }
            else
                Console.WriteLine("Account not a Seller.");
        }

        public void displayMenu()
        {
            Console.WriteLine("Welcome to the online Auction System! \n");

            if (getCurrentUser() == null || tcurrent.isLoggedIn() == false) // boolean to control what user can see based on if they are logged in or not
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press 1 to Log In: ");
                Console.WriteLine("Press 2 to Create an Account: ");
                Console.WriteLine("Press 3 to Browse Auctions: ");
                Console.WriteLine("Enter your choice : ");
            }
            else if (getCurrentUser().isLoggedIn() == true) // if user is logged in display additional options
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press 3 to Browse Auctions: ");
                Console.WriteLine("Press 4 to Create Auctions: \n");
                Console.WriteLine("Press 5 to Exit Program: ");
                Console.WriteLine("Press 6 to log out: ");
                Console.WriteLine("Enter your choice : ");
            }
        }

        public void placeAuction(Auction newAuc)
        {
            Auctions.Add(newAuc);
        }

        //thread to check and close auctions when their close date is met
        public void threadedCheckAucClose()
        {
            Thread.Sleep(1000);
            foreach (Auction Auc in Auctions)
            {
                if(Auc.getIsRunning() == true)
                Auc.checkToClose(); // check to close the auction
            }
            threadedCheckAucClose();
        }

        public void browseAuction()
        {
#if DEBUG
            Console.WriteLine("Entered Browse Auction method");
#endif
            int i = 1;
            Console.WriteLine("Current Auctions: \n");
            //loop through and show all auctions
            foreach (Auction Auc in Auctions)
            {
                if (Auc.getIsRunning() == true)
                {
                    Auc.setStatus('R'); 
                    Console.WriteLine("     Auction {0} : {1} - Current Price £{2}", i, Auc.getName(), Auc.getCurrentPrice());
                    Console.WriteLine("     Auction closes: {0}", Auc.getCloseDate().ToString("h:mm:ss tt"));
                    Console.WriteLine("     Reserve Price: £{0}", Auc.getReservePrice());
                    Console.WriteLine("     Status: {0}", Auc.getStatus());
                    Console.WriteLine("     Current Highest Bidder: {0}", Auc.getCurrentBidder());
                    Auc.checkToClose(); // check to close the auction
                    Console.WriteLine("---------------------------------------------------------------------------");
                }
                i++;
                Auc.setID(i - 1); // needed to deduct 1 from i to get correct id
            }

            Console.WriteLine("Finished Auctions: \n");
            foreach (Auction Auc in Auctions)
            {
                if (Auc.getIsRunning() == false)
                {
                        //string finished = DateTime.Now.ToString("h:mm:ss tt");
                        Console.WriteLine("     Auction : {0} - Final Price £{1}", Auc.getName(), Auc.getCurrentPrice());                       
                        Console.WriteLine("     Auction closed at: {0}", Auc.getFinishTime().ToString("h:mm:ss tt"));                             
                        Console.WriteLine("     Bought by: {0}", Auc.getBuyerName());
                        Console.WriteLine("---------------------------------------------------------------------------");
                }
            }
            makeBid();
        }
        
        /// <summary>
        ///  Function for handling bids made
        /// </summary>
        ///  <remarks>
        ///  This function allows Buyers to make bids on auctions
        /// </remarks>
        /// 
        public void makeBid()
        {
            //firstly check if a user is logged in
            if (tcurrent == null)
            {
                Console.WriteLine("Please Log In to make a bid.");
            }
            //display bidding options if user is logged in and is of type buyer

            else if (tcurrent.isLoggedIn() == true && tcurrent.GetType() == typeof(Buyer))
            {
                Console.WriteLine("Would you like to make a bid?(enter y or n)");
                string answer = Console.ReadLine();
                if(answer != "y")
                {
                    return;
                }

                Console.WriteLine("Which auction would you like to bid on(enter auction number):");
                int choice = int.Parse(Console.ReadLine());

                // loop through the auctions to find the auction we want to bid on
                foreach (Auction auc in Auctions)
                {                  
                    if (choice == auc.getID())
                    {
                        double bid;
                        Console.WriteLine("How much would you like to bid: ");
                        string s_bid = Console.ReadLine();

                        if(!double.TryParse(s_bid, out bid))
                        {
                            Console.WriteLine("Please enter correct data type.");
                        }

                        //double bid = double.Parse(Console.ReadLine());
                        DateTime ctime = DateTime.Now;

                        //create a new bid object with its new constructor details
                        Bid tbid = new Bid(bid, (Buyer)tcurrent, ctime);
                                            
                        if(DateTime.Now > auc.getCloseDate())
                        {
                            auc.setIsRunning(false);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Auction closed");
                        }
                        // checking bid is with 10% and 20% limit constraints
                        if (tbid.getAmount() > auc.getCurrentPrice() && tbid.getAmount() < (auc.getCurrentPrice() + auc.getCurrentPrice() / 100 * 20)
                            && auc.getIsRunning() == true)
                        {
                            tcurrent.addBid(tbid); // add bid to buyer
                            auc.placeBid(tbid, bid, (Buyer)tcurrent, ctime); // add bid to auctions bids list

                            Console.WriteLine("Bid : {0}", tbid.getAmount());
                            auc.setCurrentPrice(bid);
                            auc.setBuyer((Buyer)tcurrent);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Bid too small or big!");
                        }
                    }
                }
            }
        }

        //display to show details of the current user logged in
        public void currentUser()
        {
            if (tcurrent == null)
            {
                Console.WriteLine("No user logged in.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Welcome : {0}", tcurrent.getName());
                Console.WriteLine("Your info : {0}", tcurrent.getType());
                Console.WriteLine("-------------------------------------");
            }
        }

        //important added function as need to find out who the current user logged in is
        public User getCurrentUser()
        {
            return tcurrent;
        }

        //testing purposes - displays all the users in the system
        public void displayUsers()
        {
            foreach (User User in SystemUsers)
            {
                Console.WriteLine("User : {0}", User.getName());
            }
        }

        public void showUserAuctions()
        {
            tcurrent.showAuctions();
            //tcurrent.getName();
        }

        public void setupAccount(string user, string pass, char type)
        {
            if (type == 's')
            {
                User newUser = new Seller(" ", " ");
                newUser.createUser(user, pass);
                //tcurrent = newUser;
                SystemUsers.Add(newUser);
            }
            else if (type == 'b')
            {
                User newUser = new Buyer(" ", " ");
                newUser.createUser(user, pass);
                SystemUsers.Add(newUser);
            }
        }

        public void informBuyers()
        {
            //inform users if no sale
        }

        //trying to validate a user log in
        /// <summary>
        ///  Validate Login Function
        /// </summary>
        ///  <remarks>
        ///  This function checks the username and password entered and checks if such a user exists.
        /// </remarks>
        /// 
        public void checkLogIn(string user, string pass)
        {
            User result = null;
            result = SystemUsers.Find(
                delegate(User find)
                {
                    return find.getName() == user;
                });

            if (result != null)
            {
                if (result.checkPassword(pass) == true)
                {
                    result.setLoggedIn(true);
                    Console.WriteLine("Congratulations you logged in!");
                    tcurrent = result;
                }

                else if (result.checkPassword(pass) == false)
                {
                    result.setLoggedIn(false);
                    Console.WriteLine("Login failed!!");
                }
            }
        }
    }
}
