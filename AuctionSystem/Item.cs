using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionSystem
{
    /// <summary>
    ///  Item Class
    /// </summary>
    ///  <remarks>
    ///  The small class to represent an item that an auction is selling.
    /// </remarks>
    /// 
    public class Item
    {
        private string description;
        public string name = " ";

        public Item()
        {
            description = " ";
        }

        public void setDescription(string desc)
        {
            description = desc;
        }

        public string getDescription()
        {
            return description;
        }
    }
}
