using Bogus;
using Bogus.DataSets;
using Elastic.Clients.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeDataSearchApp
{
    internal class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime BirthDay { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }

        

       

        public void Info()
        {
            Console.WriteLine("\tFirstName: {0}\n\tLastName: {1}\n\tEmailAddress: {2}\n\tBirthDay: {3}\n\tPhoneNumber: {4}\n\t\tAdress:", FirstName,LastName,EmailAddress,BirthDay,PhoneNumber);
            Address.Info();

        }
    }
}
