using Bogus;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FakeDataSearchApp
{
    internal class Order
    {
        public int Id { get; set; }
        public EFood Food { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int TotalPrice { get; set; }
        public DateTime OrderTime { get; set; }
        public User User { get; set; }



        public void Info()
        {
            Console.WriteLine("Food: {0}\nQuantity: {1}\nPrice: {2}\nOrderTime: {3}\n\tUser: ",Food,Quantity,Price,OrderTime);
            User.Info();

        }



    }
     

   
}
