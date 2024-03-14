using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using EX2.BL;
using EX2.Controllers;
using EX2.DAL;
//using RuppinProj.Models;


namespace EX2.DAL


{
    public class DBservices
    {

        public DBservices()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public SqlConnection connect(String conString)
        {

            // read the connection string from the configuration file
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json").Build();
            string cStr = configuration.GetConnectionString("myProjDB");
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }


        //--------------------------------------------------------------------------------------------------
        // This method Inserts a flat to the flat table 
        //--------------------------------------------------------------------------------------------------
        public int InsertFlat(Flat flat)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateFlatInsertCommandWithStoredProcedure("SP_InsertNewFlat", con, flat);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }


        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateFlatInsertCommandWithStoredProcedure(String spName, SqlConnection con, Flat flat)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@id", flat.Id);

            cmd.Parameters.AddWithValue("@city", flat.City);

            cmd.Parameters.AddWithValue("@address", flat.Address);

            cmd.Parameters.AddWithValue("@price", flat.Price);

            cmd.Parameters.AddWithValue("@numberOfRooms", flat.NumOfRooms);

            return cmd;
        }



        //--------------------------------------------------------------------------------------------------
        // This method Inserts a user to the user table 
        //--------------------------------------------------------------------------------------------------
        public int InsertUser(User user)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateUserInsertCommandWithStoredProcedure("SP_InsertNewUser", con, user);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }


        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateUserInsertCommandWithStoredProcedure(String spName, SqlConnection con, User user)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@firstName", user.FirstName);

            cmd.Parameters.AddWithValue("@familyName", user.FamilyName);

            cmd.Parameters.AddWithValue("@email", user.Email);

            cmd.Parameters.AddWithValue("@password", user.Password);

            //cmd.Parameters.AddWithValue("@isActive", user.IsActive);

            //cmd.Parameters.AddWithValue("@isAdmin", user.IsAdmin);

            return cmd;
        }



        //--------------------------------------------------------------------------------------------------
        // This method Updates a user at user table 
        //--------------------------------------------------------------------------------------------------

        public User UpdateUser(User user)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateUserUpdateCommandWithStoredProcedure("SP_UpdateUser", con, user);             // create the command

            try
            {
                 SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                User u = null; // Initialize the User object

                while (dataReader.Read())
                {
                    u = new User
                    {
                        Email = dataReader["email"].ToString(),
                        FamilyName = dataReader["familyName"].ToString(),
                        FirstName = dataReader["firstName"].ToString(),
                        Password = dataReader["password"].ToString(),
                        IsActive = Convert.ToBoolean(dataReader["isActive"]),
                        IsAdmin = Convert.ToBoolean(dataReader["isAdmin"])
                    };
                }

                if (u != null)
                {
                    // Login successful
                    return u;
                }
                else
                {
                    // Login failed, return null or throw an exception as needed
                    return null;
                }
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }


        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        //---------------------------------------------------------------------------------
        private SqlCommand CreateUserUpdateCommandWithStoredProcedure(String spName, SqlConnection con, User user)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);

            cmd.Parameters.AddWithValue("@FamilyName", user.FamilyName);

            cmd.Parameters.AddWithValue("@Email", user.Email);

            cmd.Parameters.AddWithValue("@Password", user.Password);

            cmd.Parameters.AddWithValue("@isActive", user.IsActive);
            
            cmd.Parameters.AddWithValue("@isAdmin", user.IsAdmin);


            return cmd;
        }

        //--------------------------------------------------------------------------------------------------
        // This method checks a user login at user table 
        //--------------------------------------------------------------------------------------------------

        public User CheckLogin(User user)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateLoginCommandWithStoredProcedure("SP_CheckLogin", con, user); // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                User u = null; // Initialize the User object

                while (dataReader.Read())
                {
                    u = new User
                    {
                        Email = dataReader["email"].ToString(),
                        FamilyName = dataReader["familyName"].ToString(),
                        FirstName = dataReader["firstName"].ToString(),
                        Password = dataReader["password"].ToString(),
                        IsActive = Convert.ToBoolean(dataReader["isActive"]),
                        IsAdmin = Convert.ToBoolean(dataReader["isAdmin"])
                    };
                }

                if (u != null)
                {
                    // Login successful
                    return u;
                }
                else
                {
                    // Login failed, return null or throw an exception as needed
                    return null;
                }
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }
        }

        private SqlCommand CreateLoginCommandWithStoredProcedure(String spName, SqlConnection con, User user)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@inputEmail", user.Email);

            cmd.Parameters.AddWithValue("@inputPassword", user.Password);
            return cmd;
        }


        //--------------------------------------------------------------------------------------------------
        // This method Inserts a vaction to the vacation table 
        //--------------------------------------------------------------------------------------------------
        public int InsertVacation(Vacation order)
        {

            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateVacationInsertCommandWithStoredProcedure("SP_InsertNewVacation", con, order);             // create the command

            try
            {
                int numEffected = cmd.ExecuteNonQuery(); // execute the command
                return numEffected;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }


        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateVacationInsertCommandWithStoredProcedure(String spName, SqlConnection con, Vacation order)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@userEmail", order.UserEmail);

            cmd.Parameters.AddWithValue("@flatId", order.FlatId);

            cmd.Parameters.AddWithValue("@startDate", order.StartDate);

            cmd.Parameters.AddWithValue("@endDate", order.EndDate);

            return cmd;
        }



        //--------------------------------------------------------------------------------------------------
        // This method reads users from the database 
        //--------------------------------------------------------------------------------------------------
        public List<User> ReadUsers()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<User> usersList = new List<User>();

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateSelectUserWithStoredProcedure("SP_ReadUsers", con);             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    User u = new User();
                    u.FirstName = dataReader["firstName"].ToString();
                    u.FamilyName = dataReader["familyName"].ToString();
                    u.Email = dataReader["email"].ToString();
                    u.Password = dataReader["password"].ToString();
                    u.IsActive = Convert.ToBoolean(dataReader["isActive"]);
                    u.IsAdmin = Convert.ToBoolean(dataReader["isAdmin"]);

                    usersList.Add(u);
                }
                return usersList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateSelectUserWithStoredProcedure(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            return cmd;
        }



        //--------------------------------------------------------------------------------------------------
        // This method reads flats from the database 
        //--------------------------------------------------------------------------------------------------
        public List<Flat> ReadFlats()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<Flat> flatsList = new List<Flat>();

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateSelectFlatWithStoredProcedure("SP_ReadFlats", con);             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Flat f = new Flat();
                    f.Id = dataReader["id"].ToString();
                    f.City = dataReader["city"].ToString();
                    f.Address = dataReader["address"].ToString();
                    f.Price = Convert.ToDouble(dataReader["price"]);
                    f.NumOfRooms = Convert.ToDouble(dataReader["numberOfRooms"]);


                    flatsList.Add(f);
                }
                return flatsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateSelectFlatWithStoredProcedure(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            return cmd;
        }

        //--------------------------------------------------------------------------------------------------
        // This method reads vacations from the database 
        //--------------------------------------------------------------------------------------------------
        public List<Vacation> ReadVacations()
        {

            SqlConnection con;
            SqlCommand cmd;
            List<Vacation> vacationsList = new List<Vacation>();

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            cmd = CreateSelectVacationWithStoredProcedure("SP_ReadVacations", con);             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    Vacation v = new Vacation();
                    v.FlatId = dataReader["flatId"].ToString();
                    v.UserEmail = dataReader["userEmail"].ToString();
                    v.StartDate = Convert.ToDateTime(dataReader["startDate"]);
                    v.EndDate = Convert.ToDateTime(dataReader["endDate"]);


                    vacationsList.Add(v);
                }
                return vacationsList;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }
        //---------------------------------------------------------------------------------
        // Create the SqlCommand using a stored procedure
        //---------------------------------------------------------------------------------
        private SqlCommand CreateSelectVacationWithStoredProcedure(String spName, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            return cmd;
        }


        public Object ReadReport(int month)
        {

            SqlConnection con;
            SqlCommand cmd;
            List<Object> reports = new List<Object>();

            try
            {
                con = connect("myProjDB"); // create the connection
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }
            cmd = CreateReportCommandWithStoredProcedure("SP_ReadReport", con, month);             // create the command

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dataReader.Read())
                {
                    reports.Add(new
                    {
                        city = dataReader["city"].ToString(),
                        averagePrice = Convert.ToDouble(dataReader["AveragePricePerNight"])
                    });
                }
                return reports;
            }
            catch (Exception ex)
            {
                // write to log
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    // close the db connection
                    con.Close();
                }
            }

        }

        private SqlCommand CreateReportCommandWithStoredProcedure(String spName, SqlConnection con, int month)
        {


            SqlCommand cmd = new SqlCommand(); // create the command object

            cmd.Connection = con;              // assign the connection to the command object

            cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

            cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

            cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

            cmd.Parameters.AddWithValue("@month", month);

            return cmd;
        }


    }
}
