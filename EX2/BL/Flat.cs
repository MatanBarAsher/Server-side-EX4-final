using System;
using System.Collections.Generic;
using EX2.DAL;

namespace EX2.BL
{
    public class Flat
    {
        private string id;
        private string city;
        private string address;
        private double price; // in USD
        private double numOfRooms;
        private static List<Flat> flatsList = new List<Flat>();

        // Standard constructor
        public Flat(string id, string city, string address, double numOfRooms, double price)
        {
            Id = id;
            City = city;
            Address = address;
            NumOfRooms = numOfRooms;
            Price = Discount(price, numOfRooms);
        }

        // Empty constructor
        public Flat()
        {
        }

        public string Id { get => id; set => id = value; }
        public string City { get => city; set => city = value; }
        public string Address { get => address; set => address = value; }
        public double NumOfRooms { get => numOfRooms; set => numOfRooms = value; }
        public double Price { get => price; set => price = Discount(value, NumOfRooms); }

        public int InsertFlat()
        {
            DBservices dbs = new DBservices();
            try
            {
                if (CheckFlatId(this))
                {
                    flatsList.Add(this);
                    return dbs.InsertFlat(this);
                }
                else
                {
                    throw new Exception("Flat ID already exists.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting flat: " + ex.Message);
            }
        }

        // Check if flat ID already exists
        public bool CheckFlatId(Flat flat)
        {
            foreach (Flat exFlat in flatsList)
            {
                if (exFlat.Id == flat.Id)
                {
                    return false;
                }
            }
            return true;
        }

        // Read all flats from the database
        public List<Flat> ReadFlats()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadFlats();
        }

        // Apply discount based on the number of rooms and original price
        public double Discount(double price, double rooms)
        {
            if (rooms > 1 && price > 100)
            {
                return price * 0.9;
            }
            else
            {
                return price;
            }
        }

        // Get flats with a price equal to or lower than the given price in a specific city
        public static IEnumerable<Flat> GetFlatMaxPriceByCity(double price, string city)
        {
            List<Flat> tempList = new List<Flat>();
            foreach (Flat flat in flatsList)
            {
                if (flat.Price <= price && flat.City == city)
                {
                    tempList.Add(flat);
                }
            }
            return tempList;
        }
    }
}
