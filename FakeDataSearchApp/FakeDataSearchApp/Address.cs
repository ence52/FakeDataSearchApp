using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FakeDataSearchApp
{
    internal class Address
    {
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public static Address getRandomAdress()
        {
            Bogus.Faker faker = new Bogus.Faker();
            return new Address
            {
                Street = faker.Address.StreetName(),
                BuildingNumber = faker.Address.BuildingNumber(),
                City = faker.Address.City(),
                State = faker.Address.State(),
                Zip = faker.Address.ZipCode()
            };
        }
        public void Info()
        {
            Console.WriteLine("\t\tStreet: {0}\n\t\tBuildingNumber: {1}\n\t\tCity: {2}\n\t\tState: {3}\n\t\tZip: {4}\n ", Street, BuildingNumber, City, State,Zip );
        }

    }
}
