using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class NationalParkCLI
    {
        #region Variables
        const string Command_GetAvailableParks = "";
        const string Command_GetAvailableCampgrounds = "";
        const string Command_GetCampgroundsByPark = "3";
        const string Command_GetSelectedParkInformation = "1";
        const string Command_Quit = "q";
        const string DatabaseConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Campground;Integrated Security = True";
        private string _campgroundNumber;
        private string _command;
        private DateTime _reservationStartDate;
        private DateTime _reservationEndDate;

        ParksDAL ParkObject { get; set; } = new ParksDAL(DatabaseConnectionString);
        CLIHelper helpMe { get; set; } = new CLIHelper();
        Dictionary<string, Park> Parks { get; set; } = new Dictionary<string, Park>();
        Dictionary<string, Campground> CampgroundDictionary { get; set; } = new Dictionary<string, Campground>();
        List<Campground> CampgroundList { get; set; } = new List<Campground>();
        #endregion

        public void RunCLI()
        {
            PrintMenu();

            while (true)
            {
                string command = Console.ReadKey().KeyChar.ToString();

                Console.Clear();

                switch (command.ToLower())
                {
                    //case Command_GetAvailableParks:
                    //    GetAllParks();
                    //    break;

                    //case Command_GetAvailableCampgrounds:
                    //    GetAllCampgrounds();
                    //    Console.WriteLine("Press any key to return to main menu");
                    //    Console.ReadKey();
                    //    break;

                    //case Command_GetCampgroundsByPark:
                    //    ParkReservation();
                    //    break;

                    case Command_GetSelectedParkInformation:
                        GetSelectedParkInformation();
                        break;

                    case Command_Quit:
                        Console.WriteLine("Thank you for using the National Park Registration system.");
                        return;

                    default:
                        Console.WriteLine("The command provided was not a valid command, press any key to try again.");
                        Console.ReadKey();
                        break;
                }

                PrintMenu();
            }
        }

        private void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine(@" ____    ____  ______  ____  ___   ____    ____  _          ____    ____  ____   __  _ ");
            Console.WriteLine(@"|    \  /    T|      Tl    j/   \ |    \  /    T| T        |    \  /    T|    \ |  l/ ]");
            Console.WriteLine(@"|  _  YY  o  ||      | |  TY     Y|  _  YY  o  || |        |  o  )Y  o  ||  D  )|  ' / ");
            Console.WriteLine(@"|  |  ||     |l_j  l_j |  ||  O  ||  |  ||     || l___     |   _/ |     ||    / |    \ ");
            Console.WriteLine(@"|  |  ||  _  |  |  |   |  ||     ||  |  ||  _  ||     T    |  |   |  _  ||    \ |     Y");
            Console.WriteLine(@"|  |  ||  |  |  |  |   j  ll     !|  |  ||  |  ||     |    |  |   |  |  ||  .  Y|  .  |");
            Console.WriteLine(@"l__j__jl__j__j  l__j  |____j\___/ l__j__jl__j__jl_____j    l__j   l__j__jl__j\_jl__j\_j");
            Console.WriteLine(@"  |    \   /  _] /    Tl    j/ ___/|      T|    \  /    T|      Tl    j/   \ |    \ ");
            Console.WriteLine(@"  |  D  ) /  [_ Y   __j |  T(   \_ |      ||  D  )Y  o  ||      | |  TY     Y|  _  Y");
            Console.WriteLine(@"  |    / Y    _]|  T  | |  | \__  Tl_j  l_j|    / |     |l_j  l_j |  ||  O  ||  |  |");
            Console.WriteLine(@"  |    \ |   [_ |  l_ | |  | /  \ |  |  |  |    \ |  _  |  |  |   |  ||     ||  |  |");
            Console.WriteLine(@"  |  .  Y|     T|     | j  l \    |  |  |  |  .  Y|  |  |  |  |   j  ll     !|  |  |");
            Console.WriteLine(@"  l__j\_jl_____jl___,_j|____j \___j  l__j  l__j\_jl__j__j  l__j  |____j\___/ l__j__j");
            Console.WriteLine(@"                                                                                    ");
            Console.WriteLine();
            Console.WriteLine("Main Menu \nType in a command\n");
            Console.WriteLine(" 1 - Select Park to View Campgrounds ");
            Console.WriteLine(" Q - Quit");
        }
        /// <summary>
        /// Prints out all available parks for user
        /// </summary>
        private void GetAllParks()
        {
            List<Park> result = ParkObject.GetAvailableParks();
            Console.WriteLine("{0, 10}{1, 18}{2, 28}{3, 25}{4, 25}",
                             "Park Name", "Location", "Date Established", "Total Area", "Annual Visitors");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------");
            foreach (var item in result)
            {
                string area = Convert.ToDecimal(item.Area).ToString("#,##0 sq km");
                string visitors = Convert.ToDecimal(item.Visitors).ToString("#,##0");
                string date = item.EstablishDate.ToString("MM/dd/yyyy");;
                Console.Write("{0, 0}{1, 10}{2, 15}{3, 28}{4, 25}{5, 25}",
                                 item.ID + ")", item.Name, item.Location, date, area, visitors);
                Console.WriteLine("\n" + item.Description + "\n");
            }
            Console.Write("Press any key to return to Main Menu");
            Console.ReadKey();
            Console.Clear();
        }

        //private void GetAllParksRedone()
        //{
        //    List<Park> result = ParkObject.GetAvailableParks();
        //    Console.WriteLine("{0, 11}{1, 25}",
        //                     "Park Name", "Location");
        //    Console.WriteLine("------------------------------------------------");
        //    foreach (var item in result)
        //    {
        //        Console.Write("{0, 0}{1, -5}{2, 25}\n",
        //                         item.ID + ")", "   " + item.Name, item.Location);
        //    }
        //    Console.WriteLine("Please choose a campground to view:");
        //    GetSelectedParkCampgrounds();
        //}
        /// <summary>
        /// Prints out all available park campgrounds for user
        /// </summary>
        private void GetAllCampgrounds()
        {

            Dictionary<string, Park> result = ParkObject.GetParkDictionary();
            Console.WriteLine("{0, 13}{1, 20}",
                             "Park Name", "Location");
            Console.WriteLine("---------------------------------------------------");
            foreach (var item in result)
            {
                Console.WriteLine("{0, -5}{1, -22}{2, -20}",
                              item.Value.ID + ")  ", item.Value.Name, item.Value.Location);
            }
            Console.WriteLine("");
            Console.WriteLine("Please choose a campground to view: ");
            GetSelectedParkCampgrounds();
        }
        /// <summary>
        /// Gets available parks, displays info, and allows selection of campgrounds and sites
        /// </summary>
        private void GetSelectedParkInformation() //newest menu
        {
            GetAllCampgrounds();
            List<Park> result = ParkObject.GetAvailableParks();
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("{0, 10}{1, 18}{2, 28}{3, 25}{4, 25}",
                             "Park Name", "Location", "Date Established", "Total Area", "Annual Visitors");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------");
            bool exit = false;
            while (!exit)
            { 
                if (Parks.ContainsKey(_command))
                {
                    string area = Convert.ToDecimal(Parks[_command].Area).ToString("#,##0 sq km");
                    string visitors = Convert.ToDecimal(Parks[_command].Visitors).ToString("#,##0");
                    string date = Parks[_command].EstablishDate.ToString("MM/dd/yyyy"); 
                    Console.Write("{0, 10}{1, 15}{2, 28}{3, 25}{4, 25}",
                                     Parks[_command].Name, Parks[_command].Location, date, area, visitors);
                    Console.WriteLine("\n" + Parks[_command].Description + "\n");
                    Console.WriteLine("");
                    exit = true;
                }
                else
                {
                    Console.WriteLine("Not a valid entry. Please try again.");
                }
                ParkReservation();
            }
        }

        public void GetSelectedParkCampgrounds()
        {
            _command = Console.ReadKey().KeyChar.ToString();
            Parks = ParkObject.GetParkDictionary();
            bool exit = false;
            while (!exit)
            {
                if (Parks.ContainsKey(_command))
                {
                    CampgroundList = ParkObject.GetAvailableCampgroundsFromParks(_command);

                    exit = true;
                }
                else
                {
                    Console.WriteLine("The option provided was not a valid selection, please try again.");
                    _command = Console.ReadKey().KeyChar.ToString();
                }
            }
            Console.Clear();
            Console.WriteLine("{0, 25}{1, 32}{2, 25}{3, 25}",
                             "Campground Name", "Month Open", "Month Closed", "Daily Fee");
            Console.WriteLine("----------------------------------------------------------------------------------------------------------");
            foreach (var item in CampgroundList)
            {
                Console.WriteLine("{0, 2}{1, -25}{2, 25}{3, 25}{4, 25}",
                                  item.ID + ") ",  item.Name, item.OpenFrom, item.OpenTo, "$" + item.DailyFee);
            }
        }


        public void ParkReservation()
        {
            bool exit = false;
            //GetAllCampgrounds();
            while (!exit)
            {
                Console.WriteLine("Search for a Campground Reservation: ");
                //Method for printing Campground and Availability
                Console.Write("Which campground would you like to make a reservation at? (enter 0 to cancel) ");
                _campgroundNumber = Console.ReadKey().KeyChar.ToString();
                CampgroundDictionary = ParkObject.GetCampgroundDictionary(_command);
                if (_campgroundNumber == "0")
                {
                    exit = true;
                }
                else if (CampgroundDictionary.ContainsKey(_campgroundNumber))
                {
                    bool done = false;
                    while (!done)
                    {
                        List<DateTime> dateTimes = new List<DateTime>();
                        Console.Write("\nWhat is the arrival date? (month/day/year)? ");
                        var arrivalDate = Console.ReadLine();
                        Console.Write("\nWhat is the departure date? (month/day/year)? ");
                        var departureDate = Console.ReadLine();
                        //check to see if date is available
                        Console.WriteLine("");
                        if (DateTime.TryParse(arrivalDate, out DateTime startDate)
                            && DateTime.TryParse(departureDate, out DateTime endDate))
                        {
                            if (startDate.Month >= CampgroundDictionary[_campgroundNumber].OpenFrom
                                && endDate.Month <= CampgroundDictionary[_campgroundNumber].OpenTo)
                            {
                                if (endDate > startDate)
                                {
                                    if (startDate > DateTime.Now)
                                    {
                                        Console.WriteLine("Date validation passed.");
                                        _reservationStartDate = startDate;
                                        _reservationEndDate = endDate;
                                        List<Site> availableSites = ParkObject.MakeReservation(ParkObject, _campgroundNumber, _reservationStartDate, _reservationEndDate);
                                        List<Site> topFiveSites = this.WriteTopFiveSites(availableSites, _campgroundNumber);
                                        if (topFiveSites.Count > 0)
                                        {
                                            SelectCampsite(topFiveSites);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Press any key to return to main menu");
                                            Console.ReadKey();
                                        }
                                        done = true;
                                        exit = done;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Did you try to make a reservation in the past?  Check your dates and try again!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Your departure is earlier than your arrival. Try again, Bozo.");
                                }

                            }
                            else
                            {
                                Console.WriteLine("The campground is not currently open during selection. Please try again.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid date entry. Remember: month/day/year (e.g. 07/24/1986).");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("That is not a valid entry. Please try again.");
                }
            }
        }

        public void SelectCampsite(List<Site> topFiveSites)
        {
            Console.WriteLine("Select an available camp site: ");
            //Method for printing Campground and Availability
            Console.Write("Which camp site would you prefer? (enter 0 to cancel) ");
            string campsiteSelection = Console.ReadLine();
            bool print = false;
            bool exit = false;
            while (!exit)
            {
                if (campsiteSelection == "0")
                {
                    exit = true;
                }
                else //needed an else so the entry (to exit or for a successful choice) didn't get run through the try/catch
                {
                    try
                    {
                        //bool print = false;
                        foreach (var item in topFiveSites)
                        {
                            if (item.ID == campsiteSelection) //causing issues with looping and printing 'Invalid' after valid campsite selection
                            {
                                Console.WriteLine("What name would you like to reserve this under?");
                                string nameForReservation = Console.ReadLine();
                                string message = ParkObject.InsertReservationToTable(campsiteSelection, nameForReservation, _reservationStartDate, _reservationEndDate);
                                int dateDiff = ParkObject.GetDateDifference(_reservationStartDate, _reservationEndDate); //calculates how many days in reservation
                                int totalFee = ParkObject.GetTotalFee(_campgroundNumber, dateDiff);
                                Console.WriteLine($"{message}  Your reservation number is {ParkObject.ReservationID()} and your total cost is ${totalFee}." +
                                                  $"\nPress any key to return to main menu."); //need to move total cost to display of camp sites for comparison
                                exit = true;
                                Console.ReadKey();
                            }
                            else if (item == topFiveSites[topFiveSites.Count - 1])
                            {
                                Console.WriteLine("Invalid entry. Please try again.");
                                campsiteSelection = Console.ReadLine();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            if (print)
            {
                Console.WriteLine("Invalid entry.");
            }
        }

        public List<Site> WriteTopFiveSites(List<Site> listOpenSites, string campsiteSelection)
        {
            List<Site> topFiveSites = new List<Site>();
            int dateDiff = ParkObject.GetDateDifference(_reservationStartDate, _reservationEndDate); //calculates how many days in reservation
            int totalFee = ParkObject.GetTotalFee(_campgroundNumber, dateDiff);

            if (listOpenSites.Count >= 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine($"Campsite {listOpenSites.ElementAt(i).ID} is available and your total cost will be ${totalFee}.");
                    topFiveSites.Add(listOpenSites[i]);
                }
            }
            else if (listOpenSites.Count > 0)
            {
                foreach(Site item in listOpenSites)
                {
                    Console.WriteLine($"Campsite {item.ID} is available and your total cost will be ${totalFee}.");
                    topFiveSites.Add(item);
                }
            }
            else
            {
                Console.WriteLine("No campsites are available at requested time.");
            }
            return topFiveSites;
        }
    }
}
