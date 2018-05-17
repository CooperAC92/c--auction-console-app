using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
///  Entry Point of our application
/// </summary>

namespace AuctionSystem
{
    class entry
    {
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            System auction;
            auction = new System();
            auction.run();
        }
    }
}
