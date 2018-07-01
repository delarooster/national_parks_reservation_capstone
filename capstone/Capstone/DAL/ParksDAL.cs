using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Capstone
{
    public class ParksDAL
    {
        #region Variables

        private const string _getLastIdSQL = "SELECT CAST(SCOPE_IDENTITY() as int);";
        private string _connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security = True";

        #endregion

        #region Constructors

        public ParksDAL(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Connects to database and puts all park objects into a list
        /// </summary>
        /// <returns></returns>
        public List<Park> GetAvailableParks()
        {
            List<Park> parks = new List<Park>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string SQLGetAllParks = "SELECT * FROM park;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQLGetAllParks;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Park newPark = new Park();

                    newPark.ID = Convert.ToString(reader["park_id"]);
                    newPark.Name = Convert.ToString(reader["name"]);
                    newPark.Location = Convert.ToString(reader["location"]);
                    newPark.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                    newPark.Area = Convert.ToInt32(reader["area"]);
                    newPark.Visitors = Convert.ToInt32(reader["visitors"]);
                    newPark.Description = Convert.ToString(reader["description"]);

                    // Add the continent to the output list
                    parks.Add(newPark);
                }
            }
            return parks;
        }

        /// <summary>
        /// Makes a numbered list (dictionary with incrementing key and park object as value) of parks.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Park> GetParkDictionary()
        {
            Dictionary<string, Park> result = new Dictionary<string, Park>();
            List<Park> parks = GetAvailableParks();
            foreach (Park item in parks)
            {
                result.Add(item.ID, item);
            }
            return result;
        }

        /// <summary>
        /// Makes a list of all campgrounds in database, using the campground object.
        /// </summary>
        /// <param name="inputCommand"></param>
        /// <returns></returns>
        public List<Campground> GetAvailableCampgroundsFromParks(string inputCommand)
        {
            List<Campground> campgrounds = new List<Campground>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string SQLGetAllCampgrounds = $"SELECT * FROM campground WHERE park_id = {inputCommand};";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQLGetAllCampgrounds;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Campground newCampground = new Campground();

                    newCampground.ID = Convert.ToString(reader["campground_id"]);
                    newCampground.Name = Convert.ToString(reader["name"]);
                    newCampground.OpenFrom = Convert.ToInt32(reader["open_from_mm"]);
                    newCampground.OpenTo = Convert.ToInt32(reader["open_to_mm"]);
                    newCampground.DailyFee = Convert.ToDouble(reader["daily_fee"]);

                    // Add the continent to the output list
                    campgrounds.Add(newCampground);
                }
            }
            return campgrounds;
        }

        //should the next method be called "GetCurrentReservations" as it's returning a value of pre-made reservations
        public List<Reservation> GetAvailableReservations(string campgroundID, DateTime reqStartDate, DateTime reqEndDate)
        {
            List<Reservation> reservations = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string SQLGetAllReservations = $"SELECT * FROM reservation " +
                                               $"JOIN site " +
                                               $"ON site.site_id = reservation.site_id " +
                                               $"WHERE from_date > '{DateTime.Now}' " +
                                               $"AND campground_id = {campgroundID};";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQLGetAllReservations;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Reservation newReservation = new Reservation();

                    newReservation.ID = Convert.ToString(reader["reservation_id"]);
                    newReservation.SiteID = Convert.ToString(reader["site_id"]);
                    newReservation.Name = Convert.ToString(reader["name"]);
                    newReservation.FromDate = Convert.ToDateTime(reader["from_date"]);
                    newReservation.ToDate = Convert.ToDateTime(reader["to_date"]);
                    newReservation.CreateDate = Convert.ToDateTime(reader["create_date"]);

                    // Add the reservations to the list
                    reservations.Add(newReservation);
                }
                return reservations;
            }
        }

        //We're truly getting ALL sites from specified campground
        public List<Site> GetAvailableSites(string campgroundID)
        {
            List<Site> sites = new List<Site>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string SQLGetAllSites = $"SELECT * FROM site " +
                                        $"WHERE campground_id = {campgroundID};";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQLGetAllSites;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Site newSite = new Site();

                    newSite.ID = Convert.ToString(reader["site_id"]);
                    newSite.CampgroundID = Convert.ToString(reader["campground_id"]);
                    newSite.SiteNumber = Convert.ToInt32(reader["site_number"]);
                    newSite.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                    newSite.Accessible = Convert.ToBoolean(reader["accessible"]);
                    newSite.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                    newSite.Utilities = Convert.ToBoolean(reader["utilities"]);

                    // Add the continent to the output list
                    sites.Add(newSite);
                }
                return sites;
            }
        }

        public Dictionary<string, Campground> GetCampgroundDictionary(string inputCommand)
        {
            Dictionary<string, Campground> result = new Dictionary<string, Campground>();
            List<Campground> campgrounds = GetAvailableCampgroundsFromParks(inputCommand);
            foreach (Campground item in campgrounds)
            {
                result.Add(item.ID, item);
            }
            return result;
        }

        public List<Site> MakeReservation(ParksDAL parkObject, string campgroundNumber, DateTime reservationStartDate, DateTime reservationEndDate)
        {
            List<Site> siteList = new List<Site>();
            List<Site> openSiteList = new List<Site>();
            List<Reservation> resList = new List<Reservation>();
            //GetAvailableReservations is misnomer. Should be "GetPremadeReservations" or something along the lines of an earlier
            //reservation. THEN we compare against the current input for reservation request.
            resList = parkObject.GetAvailableReservations(campgroundNumber, reservationStartDate, reservationEndDate);
            //now we get all the sites for the available campground
            siteList = parkObject.GetAvailableSites(campgroundNumber);
            if (resList.Count == 0)
            {
                openSiteList = siteList;
            }
            else
            {
                for (int i = 0; i < siteList.Count; i++)
                {
                    foreach (Reservation item in resList.Where(r=>r.SiteID == siteList[i].ID)) //'r=>' is simply declaring an alias for resList
                    {
                        if (reservationStartDate >= item.FromDate && reservationStartDate <= item.ToDate
                            || reservationEndDate >= item.FromDate && reservationEndDate <= item.ToDate
                            || reservationEndDate >= item.ToDate && reservationStartDate <= item.FromDate)
                        //if (item.FromDate < reservationStartDate && item.ToDate > reservationStartDate && item.SiteID == siteList[i].ID
                        //   || item.FromDate < reservationEndDate && item.ToDate > reservationEndDate && item.SiteID == siteList[i].ID
                        //   || item.FromDate < reservationStartDate && item.ToDate > reservationEndDate && item.SiteID == siteList[i].ID
                        //   || item.FromDate > reservationStartDate && item.ToDate < reservationEndDate && item.SiteID == siteList[i].ID
                        {
                    siteList.RemoveAt(i);
                        }
                    }
                }
                openSiteList = siteList;
            }
            return openSiteList;
        }
        public string InsertReservationToTable(string siteInput, string nameToReserve, DateTime startDate, DateTime endDate)
        {
            string message = "";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string SQLWriteReservationToTable = $"INSERT INTO reservation (site_id, name, from_date, to_date, create_date) " +
                                                    $"VALUES (@siteInput, @name, @startDate, @endDate, @DateTimeNow);";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQLWriteReservationToTable;
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@siteInput", siteInput);
                cmd.Parameters.AddWithValue("@name", nameToReserve);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
                cmd.Parameters.AddWithValue("@DateTimeNow", DateTime.Now);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    message = "Reservation was not made successfully.";
                }
                else if (rowsAffected > 0)
                {
                    message = "Reservation was made successfully!";
                }

                return message;
            }
        }

        public string ReservationID()
        {
            string message = "";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string SQLWriteReservationToTable = $"SELECT MAX(reservation_id) AS reservationID FROM reservation;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQLWriteReservationToTable;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    message = Convert.ToString(reader["reservationID"]);
                }
            }
            return message;
        }

        public int GetDateDifference(DateTime startDate, DateTime endDate)
        {
            int dateDifference = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string SQLGetDayDifference = $"SELECT DATEDIFF(day, '{startDate}', '{endDate}') AS DateDiff;";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQLGetDayDifference;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dateDifference = Convert.ToInt32(reader["DateDiff"]);
                }
            }
            return dateDifference;
        }

        public int GetTotalFee(string campgroundNumber, int dateDiff)
        {
            int dailyFee = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string SQLGetDayDifference = $"SELECT daily_fee FROM campground WHERE campground_id = {campgroundNumber};";

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SQLGetDayDifference;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dailyFee = Convert.ToInt32(reader["daily_fee"]);
                }
            }
            return (dailyFee * dateDiff);
        }
    }

    #endregion  
}
