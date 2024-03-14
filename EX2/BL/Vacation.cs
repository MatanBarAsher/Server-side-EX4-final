using System;
using System.Collections.Generic;
using EX2.DAL;

namespace EX2.BL
{
    public class Vacation
    {
        private string userEmail;
        private string flatId;
        private DateTime startDate; // yy-mm-dd
        private DateTime endDate; // yy-mm-dd
        private static List<Vacation> ordersList = new List<Vacation>();

        public Vacation(string userEmail, string flatId, DateTime startDate, DateTime endDate)
        {
            UserEmail = userEmail;
            FlatId = flatId;
            StartDate = startDate;
            EndDate = endDate;
        }

        public Vacation()
        {
        }

        public string UserEmail { get => userEmail; set => userEmail = value; }
        public string FlatId { get => flatId; set => flatId = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }

        public int InsertVacation()
        {
            DBservices dbs = new DBservices();
            try
            {
                if (IsAvailable(this))
                {
                    ordersList.Add(this);
                    return dbs.InsertVacation(this);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                throw new Exception("Dates are taken or ID already exists.");
            }
        }

        public List<Vacation> ReadVacations()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadVacations();
        }

        public bool IsAvailable(Vacation order)
        {
            foreach (Vacation existingOrder in ordersList)
            {
                // Check if the flatId matches
                if (order.FlatId == existingOrder.FlatId)
                {
                    // Check for date overlap
                    if (IsDateRangeOverlap(order.StartDate, order.EndDate, existingOrder.StartDate, existingOrder.EndDate))
                    {
                        // There is an overlap, so the flat is not available
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool IsDateRangeOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return start1 <= end2 && end1 >= start2;
        }

        public static IEnumerable<Vacation> GetOrdersByDates(DateTime startDate, DateTime endDate)
        {
            List<Vacation> tempList = new List<Vacation>();
            foreach (Vacation existingOrder in ordersList)
            {
                if (existingOrder.StartDate >= startDate && existingOrder.EndDate <= endDate)
                {
                    tempList.Add(existingOrder);
                }
            }
            return tempList;
        }

        public static object Report(int month)
        {
            DBservices dbs = new DBservices();
            return dbs.ReadReport(month);
        }
    }
}
