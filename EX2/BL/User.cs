using System;
using System.Collections.Generic;
using EX2.DAL;
using Microsoft.AspNetCore.Mvc;

namespace EX2.BL
{
    public class User
    {
        private string firstName;
        private string familyName;
        private string email;
        private string password;
        private bool isActive;
        private bool isAdmin;
        private static List<User> usersList = new List<User>();

        public User()
        {
        }

        public User(string firstName, string familyName, string email, string password, bool isActive = true, bool isAdmin = false)
        {
            FirstName = firstName;
            FamilyName = familyName;
            Email = email;
            Password = password;
            IsActive = isActive;
            IsAdmin = isAdmin;
        }

        public string FirstName { get => firstName; set => firstName = value; }
        public string FamilyName { get => familyName; set => familyName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public bool IsAdmin { get => isAdmin; set => isAdmin = value; }

        public int InsertUser()
        {
            try
            {
                DBservices dbs = new DBservices();
                usersList.Add(this);
                return dbs.InsertUser(this);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new Exception("Error inserting user", ex);
            }
        }

        public List<User> ReadUsers()
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.ReadUsers();
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new Exception("Error reading users", ex);
            }
        }

        public User CheckLogin()
        {
            try
            {
                DBservices dbs = new DBservices();
                return dbs.CheckLogin(this);
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new Exception("Error checking login", ex);
            }
        }

        public User UpdateUser(User newUser)
        {
            try
            {
                // Find the user in the UsersList by email
                User userToUpdate = usersList.Find(u => string.Equals(u.Email.Trim(), newUser.Email.Trim(), StringComparison.OrdinalIgnoreCase));

                if (userToUpdate != null)
                {
                    // Update user information
                    userToUpdate.FirstName = newUser.FirstName;
                    userToUpdate.FamilyName = newUser.FamilyName;
                    userToUpdate.Password = newUser.Password;
                    userToUpdate.IsActive = newUser.IsActive;

                    // Update in the database (assuming DBservices has an UpdateUser method)
                    DBservices dbs = new DBservices();
                    return dbs.UpdateUser(userToUpdate);
                }
                else
                {
                    // User not found, handle the case appropriately (return null, throw an exception, etc.)
                    return null; // Or throw new Exception("User not found");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw new Exception("Error updating user", ex);
            }
        }
    }
}
