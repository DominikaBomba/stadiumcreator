using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Threading; 

namespace stadium_creator
{
    public interface IDisplay
    {
        void DisplayInformation();
    }
    public enum StandType
    {
         LowerStand = 1, UpperStand = 2
    }
    public class Person : IDisplay
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BirthDate { get; set; }
        public Person()
        {

        }
        public Person(string name, string surname, int birthDate)
        {
            Name = name;
            Surname = surname;
            BirthDate = birthDate;

        }
        public void DisplayInformation()
        {
            Console.WriteLine($"Name: {Name}, Surname: {Surname}, Birth date: {BirthDate}");
        }

    }
    public class Participant : Person, IDisplay
    {
        public string Id = Guid.NewGuid().ToString("N").Substring(16);

        public Participant(string name, string surname, int birthDate) : base(name, surname, birthDate)
        {
           
        }
        public void DisplayInformation()
        {
            Console.WriteLine($"ID: {Id}, Name: {Name}, Surname: {Surname}, Birth date: {BirthDate}");
        }

    }
    public class Stadium
    {
        public string Name { get; set; }

        public List<Event> EventList = new List<Event>();


       public  int NumberOfUpperStands { get; set; }
        public int NumberOfLowerStands { get; set; }
    
        public Stadium()
        {

        }
        public Stadium(string name, int numberOfUpperStands, int numberOfLowerStands)
        {
            Name = name;
            NumberOfUpperStands = numberOfUpperStands;
            NumberOfLowerStands = numberOfLowerStands;
        }
        
    }
    public class Stand
    {
        public int NumberOfColumns { get; set; }
        public int NumberOfRows { get; set; }

        
        public bool[,] Seats { get; set; }
        public Stand(int numberofcolumns, int numberofrows)
        {
            NumberOfColumns = numberofcolumns;
            NumberOfRows = numberofrows;
            Seats = new bool[numberofcolumns, numberofrows];
            for (int i = 0; i < Seats.GetLength(0); i++)
            {
                
                for (int j = 0; j < Seats.GetLength(1); j++)
                {

                    Seats[i, j] = false;

                }
                
            }

        }
        public void DisplaySeats()
        {

            {

                for (int i = 0; i < Seats.GetLength(0); i++)
                {
                    Console.Write($"{i + 1}".PadRight(3, ' '));
                }
                Console.ResetColor();
                Console.WriteLine(" ");

                for (int i = 0; i < Seats.GetLength(0); i++)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"{i + 1}".PadRight(4, ' '));
                    Console.ResetColor();
                    for (int j = 0; j < Seats.GetLength(1); j++)
                    {

                        if (Seats[i, j] == true)
                        {
                            Console.Write(" \u25a1 ");
                        }
                        else
                        {
                            Console.Write(" \u25a0 ");
                        }

                    }
                    Console.WriteLine("");
                }

            }

        }
    }

    public class Event
    {
       public  string Title { get; set; }
        public int CostOfUpperStandSeat { get; set; }
        public int CostOfLowerStandSeat { get; set; }
        public Performer Performer { get; set; }

        public List<Stand> UpperStandList = new List<Stand>();

        public List<Stand> LowerStandList = new List<Stand>();

        public List<Ticket> ListTicket = new List<Ticket>();
        public Stadium Stadium { get; set; }
        public DateTime Date { get; set; }
        public Event(string title, int costOfUpperStandSeat, int costOfLowerStandSeat, Performer performer, DateTime date, Stadium stadium)
        {
            Title = title;
            CostOfUpperStandSeat = costOfUpperStandSeat;
            CostOfLowerStandSeat = costOfLowerStandSeat;
            
           
            Performer = performer;
            ListTicket = new List<Ticket>();
            Date = date;
            Stadium = stadium;
            
            for(int i = 0; i < stadium.NumberOfUpperStands; i++)
            {
                UpperStandList.Add(new Stand(10, 10));
            }

            for (int i = 0; i < stadium.NumberOfLowerStands; i++)
            {
               LowerStandList.Add(new Stand(10, 10));
            }
        }
        public Ticket AddTicket()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"Adding ticket to event {Title}");
            Console.WriteLine("------------------------------------------");
            Console.ResetColor();
            /*adding participants name*/
            Console.WriteLine("\nEnter participants name: ");
            
            Console.Write("\t");
            string name = Console.ReadLine();

            while (name.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter a correct name (min. 2 signs)");
                Console.ResetColor();
                Console.Write("\t");
                name = Console.ReadLine();
            }

            /*adding participants surname*/
            Console.WriteLine("\nEnter participants surname: ");
            Console.Write("\t");
            string surname = Console.ReadLine();

            while (surname.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter a correct name (min. 2 signs)");
                Console.ResetColor();
                Console.Write("\t");
                surname = Console.ReadLine();
            }

            /*adding participants year of birth*/
            Console.WriteLine("\nEnter participants year of Birth: ");
            uint year = 0;
            while ((!uint.TryParse(Console.ReadLine(), out year)) || year > DateTime.Now.Year)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\tEnter correct values");
                Console.ResetColor();
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"Adding ticket to event {Title}");
            Console.WriteLine("------------------------------------------");
            Console.ResetColor();
            Console.WriteLine("Which stand would you like to choose: ENTER (1/2)");
            string choice;
            do
            {
                Console.WriteLine($"\t1. Lower stands \n\t\tin a price {CostOfLowerStandSeat}$ per seat\n");
                Console.WriteLine($"\t2. Upper stands \n\t\tin a price {CostOfUpperStandSeat}$ per seat");
                Console.Write("\t");
                choice = Console.ReadLine();
            } while (!(choice == "1" || choice == "2"));
           
            Stand chosenStand = ChooseStand(byte.Parse(choice));
            List<Stand> StandList;
            if (choice == "1")
            {
                StandList = LowerStandList;
            }
            else
            {
                StandList = UpperStandList;
            }
            bool SeatChosen = false;
            int row=0;
            int column=0;
            do
            {
                Console.Clear();
                Console.ForegroundColor= ConsoleColor.DarkCyan;
                Console.WriteLine($"Choosing seat on a stand {StandList.IndexOf(chosenStand) + 1}");
                Console.WriteLine("------------------------------------------");
                chosenStand.DisplaySeats();
                Console.ResetColor();
                
                SeatChosen = false;
                do
                {
                    Console.WriteLine("Choose a row: ");
                } while (!(int.TryParse(Console.ReadLine(), out row)) || row <= 0 || row > chosenStand.NumberOfRows);

                do
                {
                    Console.WriteLine("Choose a column: ");
                } while (!(int.TryParse(Console.ReadLine(), out column)) || column <= 0 || column > chosenStand.NumberOfColumns);
                if (chosenStand.Seats[ column-1, row -1] != false)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("\t!!! Seat occupied !!!");
                    Console.WriteLine("\tChoose a different seat");
                    SeatChosen = false;
                }
                else
                {
                    SeatChosen = true;
                }
            } while (!SeatChosen);
            chosenStand.Seats[column-1,row-1] = true;
            Ticket ticket = new Ticket((StandType)(int.Parse(choice)), StandList.IndexOf(chosenStand), column, row, new Participant(name, surname,(int)year));
            ListTicket.Add(ticket);
            return ticket ;
        }
        public Stand ChooseStand(byte choosenType)
        {
            bool StandChosen = false;
            List<Stand> StandList;
            if (choosenType == 1)
            {
                StandList = LowerStandList;
            }
            else
            {
                StandList = UpperStandList;
            }
            byte standNumber = 1;
            do
            {
                Console.Clear();
                Console.ForegroundColor= ConsoleColor.DarkCyan;
                Console.WriteLine($"Chose a stand for ticket to event {Title}");
                Console.WriteLine("------------------------------------------");
                Console.ResetColor();
                Console.WriteLine($"Stand {standNumber}:");
                StandList[standNumber - 1].DisplaySeats();
                Console.WriteLine("Press: \n\t1.to choose this stand \n\t2. To see the next one");
                if (Console.ReadLine() == "1")
                {
                    StandChosen = true;
                }
                else
                {
                    StandChosen = false;
                    if (standNumber < StandList.Count)
                    {
                        standNumber++;
                    }
                    else
                    {
                        standNumber = 1;
                    }
                }
            } while (!StandChosen);
            return StandList[standNumber - 1];
        }
    }
    public class Ticket
    {
        public StandType StandType { get; set; }

        public int NumberOfaStand { get; set; }
        public int SeatColumn { get; set; }
        public int SeatRow { get; set; }
        public Participant Participant { get; set; }
        
        public Ticket(StandType standType, int numberOfaStand, int seatColumn, int seatRow, Participant participant)
        {
            StandType = standType;
            NumberOfaStand = numberOfaStand;
            SeatColumn = seatColumn;
            SeatRow = seatRow;
            Participant = participant;
        }
       
    
    }
    public class Performer : Person
    {
        List<Song> SongList = new List<Song>();
        public Performer(string name, string surname, int birthDate) : base(name, surname, birthDate)
        {
            Console.WriteLine($"Performer {name} {surname} ({birthDate}) added successfully");
        }
        public void AddSong()
        {

        }
    }
    class Song
    {
        string Title { get; set; }
        Performer Performer { get; set; }

    }
    public class StatiumCreator
    {
       public  List<Performer> PerformerList = new List<Performer>();
       public List<Stadium> StadiumList = new List<Stadium>();
       public List<Event> EventList = new List<Event>();
       
        public void DisplayAllTickets()
        {
            string horizontalSpace = new string(' ', 50);
            string horizontalLine = new string(' ', 50);
            foreach (Event e in EventList)
            {
                foreach (Ticket t in e.ListTicket)
                {
                    /*Ticket information - event*/

                    string eventName = "Event:   " + e.Title;
                    string performer = "Performs:    " + e.Performer.Name + " " + e.Performer.Surname;
                    Console.WriteLine(horizontalSpace);

                    Console.WriteLine($"| {eventName.PadRight(50)} |");
                    Console.WriteLine($"| {performer.PadRight(50)} |");
                    Console.WriteLine(horizontalSpace);
                    Console.WriteLine("|-" + horizontalLine + "-|");

                    /*Ticket information - participant*/
                    Console.WriteLine(horizontalSpace);
                    string name = "Participant: " + t.Participant.Name + " " + t.Participant.Surname;
                    Console.WriteLine($"| {name.ToString().PadRight(50)} |");

                    /*Ticket information - Stand and Seat*/
                    string stand = "Stand type: " + t.StandType.ToString() + " " + t.NumberOfaStand.ToString() + ", Seat: " + t.SeatColumn.ToString() + ", " + t.SeatColumn.ToString();

                    Console.WriteLine($"| {stand.PadRight(50)} |");
                    Console.WriteLine(horizontalSpace);
                    /*Ticket information - Price*/
                    string price;
                    if (t.StandType == StandType.LowerStand)
                    {
                        price = e.CostOfLowerStandSeat.ToString() + "$";
                        
                    }
                    else
                    {
                        price = e.CostOfUpperStandSeat.ToString() + "$";
                       
                    }
                    Console.WriteLine("|-" + horizontalLine + "-|");

                    Console.Write($"|");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(price.PadLeft(52));
                    Console.ResetColor();
                    Console.Write("|\n");


                    Console.WriteLine("+" + horizontalLine + "+");
                }
                Console.WriteLine("\n");
            }
        }
        public void DisplayStadium_Without_Checking_Avaliablity()
        {
            string horizontalLine = new string('-', 50);
           
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("All stadiums: ");
            Console.WriteLine(horizontalLine+"\n" );
            Console.ResetColor();

            for (int i = 0; i < StadiumList.Count; i++)
            {
               Console.WriteLine($"\t{i + 1}. {StadiumList[i].Name}");

                if (StadiumList[i].EventList.Count != 0)
                {
                    foreach (Event e1 in StadiumList[i].EventList)
                    {
                        Console.WriteLine($"\t\t {e1.Date.ToShortDateString()} - {e1.Title}, performs: {e1.Performer.Name} {e1.Performer.Surname}");
                    }
                }
            }
            Console.ReadKey();
        }
        public void DisplayStadiums(DateTime date)
        {
          
            Console.WriteLine($"Stadium avaliable on {date.ToShortDateString()}: ");
         
            for (int i = 0; i < StadiumList.Count; i++)
            {
                /*checking if there is no event on the stadium on the entered date*/
                bool isEvent = false;
                if (StadiumList[i].EventList.Count != 0)
                {
                    foreach (Event e1 in StadiumList[i].EventList)
                    {
                        if (e1.Date.ToShortDateString() == date.ToShortDateString())
                        {
                            isEvent = true;
                        }
                    }
                }
                if (isEvent == false)
                {
                    Console.Write($"\t{i + 1}. {StadiumList[i].Name} - stadium");
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(" avaliable\n");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write($"\t{i + 1}. {StadiumList[i].Name} - stadium");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("not avaliable\n");
                    Console.ResetColor();
                }
                    
                    if (StadiumList[i].EventList.Count != 0)
                    {
                        foreach (Event e1 in StadiumList[i].EventList)
                        {
                            Console.WriteLine($"\t\t {e1.Date.ToShortDateString()} - {e1.Title}, performs: {e1.Performer.Name} {e1.Performer.Surname}");
                        }
                    }
                }
            
                
            }

        public void AddStadium()
        {
            
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Adding stadium");
            Console.ResetColor();
            /*adding stadium name*/

            Console.WriteLine("\nEnter Stadium name: ");
            Console.Write("\t");
            string name = Console.ReadLine();

            while (name.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter a correct name (min. 2 signs)");
                Console.ResetColor();
                Console.Write("\t");
                name = Console.ReadLine();
            }


            /*Adding number of lower stands*/
            Console.WriteLine("Enter Number Of Lower Stands (1-100):");
            Console.Write("\t");
            uint numberOfLowerStands;
            while ((!uint.TryParse(Console.ReadLine(), out numberOfLowerStands)) || numberOfLowerStands<=0 || numberOfLowerStands > 100)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter correct values");
                Console.ResetColor ();
                Console.Write("\t");
            }

            /*Adding number of lower stands*/
            Console.WriteLine("Enter Number Of Upper Stands(1-100): ");
            Console.Write("\t");
            uint numberOfUpperStands = 0;
            while ((!uint.TryParse(Console.ReadLine(), out numberOfUpperStands)) || numberOfUpperStands <= 0 || numberOfUpperStands > 100)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter correct values");
                Console.ResetColor();
                Console.Write("\t");
            }
            StadiumList.Add(new Stadium(name, (int)numberOfLowerStands, (int)numberOfUpperStands));
            
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Stadium added successfully");
            Console.WriteLine("PRESS ENTER TO CONTINUE");
            Console.ResetColor();
            Console.ReadKey();
        }
        public void RemoveStadium()
        {

            string numberofs;
            int numberOfaStadium= 0;
            bool isCorrect=false;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Removing Stadium");
                Console.ResetColor();
                Console.WriteLine("\nSelect an Stadium:");

                for (int i = 0; i < StadiumList.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {StadiumList[i].Name}");
                }

                    try
                    {
                        numberofs = Console.ReadLine();

                    numberOfaStadium = int.Parse(numberofs);
                    if (numberOfaStadium >= 1 && numberOfaStadium <= (StadiumList.Count))
                    {
                        isCorrect = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"Enter a value from {1} to {(StadiumList.Count - 1)}");
                        Console.ResetColor();
                       
                       
                    }

                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (OverflowException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


            } while (!isCorrect);
            foreach(Event e3 in StadiumList[numberOfaStadium - 1].EventList)
            {
                EventList.Remove(e3);
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Removing stadium {StadiumList[numberOfaStadium - 1].Name}");
            StadiumList.Remove(StadiumList[numberOfaStadium - 1]);
            DisplayStadiums(DateTime.Parse("11-11-1120"));
            Console.ResetColor();

        }
        /*performers functions*/
        public void DisplayPerformers()
        {
            Console.WriteLine("Performers List: ");
            for (int i = 0; i < PerformerList.Count; i++)
            {
                Console.WriteLine($"\t{i + 1}. {PerformerList[i].Name} {PerformerList[i].Surname} ({DateTime.Now.Year - PerformerList[i].BirthDate} years old)");
            }

        }
        public void AddPerformer()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Adding Performer");
            Console.ResetColor();

            /*adding performers name*/
            Console.WriteLine("\nEnter performers name: ");
            Console.Write("\t");
            string name = Console.ReadLine();

            while (name.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter a correct name (min. 2 signs)");
                Console.ResetColor();
                Console.Write("\t");
                name = Console.ReadLine();
            }

            /*adding performers surname*/
            Console.WriteLine("\nEnter performers surname: ");
            Console.Write("\t");
            string surname = Console.ReadLine();

            while (surname.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter a correct name (min. 2 signs)");
                Console.ResetColor();
                Console.Write("\t");
                surname = Console.ReadLine();
            }

            /*adding performers year of birth*/
            Console.WriteLine("\nEnter performers Year of Birth: ");
            Console.Write("\t");
            uint year = 0;
            while ((!uint.TryParse(Console.ReadLine(), out year)) || year > DateTime.Now.Year)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\tEnter correct values");
                Console.Write("\t");
                Console.ResetColor();
            }
            Performer p1 = new Performer(name, surname, (int)year);
            PerformerList.Add(p1);

        }
        /*event funkcje*/
        public void AddEvent()
        {
            string horizontalLine = new string('-', 50);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            /*getting information from user about event*/
            Console.WriteLine("Adding an event");
            Console.WriteLine(horizontalLine);
            Console.ResetColor();

            /*name of the event*/
            Console.WriteLine("\nEnter a name of the event: ");
            Console.Write("\t");
            string title = Console.ReadLine();
            while (title.Length <2)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter a correct name (min. 2 signs)");
                Console.ResetColor();
                Console.Write("\t");
                title = Console.ReadLine();
            }

            /*Cost of the seat on an Upper Stand*/
            Console.WriteLine("\nEnter Cost of the seat on an Upper Stand: ");
            uint costUpperSeats = 0;
            Console.Write("\t");
            while (!uint.TryParse(Console.ReadLine(), out costUpperSeats) || costUpperSeats <= 0 || costUpperSeats >= 1000)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter a correct value (greater than 0 and less than 1000)");
                Console.ResetColor();
                Console.Write("\t");
            }

            /*Cost of the seat on an Lower Stand*/
            Console.WriteLine("\nEnter Cost of the seat on an Lower Stand: ");
            Console.Write("\t");
            uint costLowerSeats = 0;
            while (!uint.TryParse(Console.ReadLine(), out costLowerSeats) || costLowerSeats <= 0 || costLowerSeats >= 1000)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter a correct value (0 - 1000)");
                Console.ResetColor();
                Console.Write("\t");
            }

            /*Cost of the seat on an Lower Stand*/
            Console.WriteLine($"\nEnter the date of the {title} event: (DD-MM-YYYY)");
            DateTime date;
            Console.Write("\t");
            while ((!DateTime.TryParse(Console.ReadLine(), out date)) || date > DateTime.Now)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Enter correct date");
                Console.ResetColor();
                Console.Write("\t");
            }

            /*Choosing or adding a performer*/
            bool isCorrect = false;
            int numberOfaPerformer = 0;
            string numberofap;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Choosing a performer");
                Console.WriteLine(horizontalLine );
                Console.ResetColor();

                Console.WriteLine("\nSelect a number performer: \n");
                DisplayPerformers();
                if(PerformerList.Count == 0)
                {
                    Console.ForegroundColor= ConsoleColor.DarkRed;
                    Console.WriteLine("It looks like there aren't any performers avaliable");
                    Console.ResetColor();
                    Console.WriteLine("\nDo you want to add one? (yes/no)");
                    string answer = Console.ReadLine();
                    Console.Clear();
                    if(answer.ToLower() == "yes")
                    {
                        AddPerformer();
                        Console.Clear();

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Choosing a performer");
                        Console.WriteLine(horizontalLine);
                        Console.ResetColor();
                        Console.WriteLine("\nSelect a number performer: \n");
                        DisplayPerformers();
                    }
                    else
                    {
                        Console.WriteLine("Add a performer, in order to create an event");
                        Thread.Sleep(2000);
                        return;
                    }
                }
                try
                {
                    numberofap = Console.ReadLine();

                    numberOfaPerformer = int.Parse(numberofap);
                    if (numberOfaPerformer >=1 && numberOfaPerformer <= (PerformerList.Count))
                    {
                        isCorrect = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor= ConsoleColor.DarkRed;
                        Console.WriteLine($"Enter a value from {1} to {(PerformerList.Count - 1)}");
                        Console.ResetColor();
                    }   
                      
                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (OverflowException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                

            } while (!isCorrect);

      
            isCorrect = false;
            int numberOfaStadium = 0;
            string numberofs;
            /*checking avaliablity of a stadium */
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Select stadium");
                Console.WriteLine(horizontalLine);
                Console.ResetColor();
              
                if (StadiumList.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("\nIt looks like there aren't any stadiums added\n");
                    Console.ResetColor();
                    Console.WriteLine("Do you want to add one? (yes/no)");
                    string answer = Console.ReadLine();
                    if (answer.ToLower() == "yes")
                    {
                        Console.Clear();
                        AddStadium();
                    }
                    else
                    {
                        Console.WriteLine("\nAdd a stadium, in order to create an event");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("PRESS ENTER to continue");
                        Console.ResetColor();
                        Console.ReadKey();
                        return;
                    }
                }

                
                
                try
                {
                    DisplayStadiums(date);
                    
                    numberofs = Console.ReadLine();

                    numberOfaStadium= int.Parse(numberofs);

                    if (numberOfaStadium >= 1 && numberOfaStadium <= (StadiumList.Count))
                    {
                        isCorrect= true;
                        foreach(Event e2 in StadiumList[numberOfaStadium - 1].EventList)
                        {
                            if(e2.Date == date)
                            {
                                isCorrect = false;
                                Console.ForegroundColor= ConsoleColor.DarkRed;
                                Console.WriteLine($"Stadium {StadiumList[numberOfaStadium - 1].Name}not avaliable - on {date.ToShortDateString()} ");
                                Console.WriteLine("Choose different Stadium");
                                Console.ResetColor();
                                Console.ReadKey ();
                            }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"Enter a value from {1} to {(StadiumList.Count - 1)}");

                    }

                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (OverflowException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


            } while (!isCorrect);
            


            Event e1 = new Event(title, (int)costUpperSeats, (int)costUpperSeats, PerformerList[numberOfaPerformer - 1], date, StadiumList[numberOfaStadium-1] );
            EventList.Add(e1);
            StadiumList[numberOfaStadium - 1].EventList.Add(e1);

         
            Console.Clear();
            Console.WriteLine($"Event {e1.Title} ({StadiumList[numberOfaStadium - 1].Name}, {date.ToShortDateString()}) added successfully");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("PRESS ENTER TO CONTINUE");
            Console.ReadKey();
            
        }

        public void DisplayEvents()
        {
            

            for (int i = 0; i < EventList.Count; i++)
            {
                Console.WriteLine($"\t{i + 1}. {EventList[i].Title} ({EventList[i].Date.ToShortDateString()}) - performs {EventList[i].Performer.Name} {EventList[i].Performer.Surname}");
            }
            

        }

        public void RemoveEvent()
        {
            
            bool isCorrect = false;
            int numberOfanEvent = 0;
            string numberofe;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Removing Event");
                Console.ResetColor();
                Console.WriteLine("\nSelect an Event:");
                DisplayEvents();


                try
                {
                    numberofe = Console.ReadLine();

                    numberOfanEvent = int.Parse(numberofe);
                    if (numberOfanEvent >= 1 && numberOfanEvent <= (EventList.Count))
                    {
                        isCorrect = true;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Enter a value from {1} to {(StadiumList.Count - 1)}");
                    }

                }
                catch (FormatException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (OverflowException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                

            } while (!isCorrect);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Removing event {EventList[numberOfanEvent-1].Title}");
            EventList[numberOfanEvent - 1].Stadium.EventList.Remove(EventList[numberOfanEvent-1]);
            EventList.RemoveAt(numberOfanEvent-1);
            Console.ResetColor();
            

        }
        public void AddingTicket_ChoosingEvent()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Adding ticket");
            Console.WriteLine("------------------------------------------");
            Console.ResetColor();
            Console.WriteLine("Choose an Event: \n");

            if (EventList.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("It looks like there aren't any events avaliable");
                Console.ResetColor();
                Console.WriteLine("\nDo you want to add one? (yes/no)");
                string answer = Console.ReadLine();
                Console.Clear();
                if (answer.ToLower() == "yes")
                {
                    AddEvent();
                    Console.Clear();

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Choosing an Event\n");
                    Console.ResetColor();
                    Console.WriteLine("Select an Event:");
                   
                }
                else
                {
                    Console.WriteLine("Add an event, in order to create an event");
                    /*dodanie timera*/
                    return;
                }
            }

            DisplayEvents();
                string numberofe;
                int numberOfanEvent = 1;
                bool isCorrect = false;
                do
                {
                    try
                    {
                        numberofe = Console.ReadLine();

                        numberOfanEvent = int.Parse(numberofe);
                        if (numberOfanEvent >= 1 && numberOfanEvent <= (EventList.Count))
                        {
                            isCorrect = true;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"Enter a value from {1} to {(EventList.Count - 1)}");
                            Console.WriteLine("------------------------------------------\n");
                            DisplayEvents();

                        }

                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (OverflowException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }


                } while (!isCorrect);
            
            List<Ticket> tickets = new List<Ticket>();

            
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Adding ticket");
                Console.WriteLine("------------------------------------------");
                Console.ResetColor();



                Console.WriteLine("Do you want to add another ticket? ENTER (yes/no)");
                if (Console.ReadLine() == "yes")
                {
                    Ticket ticket = EventList[numberOfanEvent - 1].AddTicket();
                    tickets.Add(ticket);

                  
                }
                else
                {
                    string horizontalLine = new string('-', 50);
                    string horizontalSpace = "| " + new string(' ', 50) + " |";
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Summary of purchused tickets: ");
                    Console.WriteLine(horizontalLine + horizontalLine);
                    Console.ResetColor();
                    int totalCost = 0;
                    foreach (Ticket t in tickets)
                    {
                        Console.WriteLine($"\n\nTicket {tickets.IndexOf(t)+1}\n");
                        Console.WriteLine("+-" + horizontalLine + "-+");

                        /*Ticket information - event*/

                        string eventName = "Event:   " + EventList[numberOfanEvent - 1].Title;
                        string performer = "Performs:    " + EventList[numberOfanEvent - 1].Performer.Name + " " + EventList[numberOfanEvent - 1].Performer.Surname;
                        Console.WriteLine(horizontalSpace);

                        Console.WriteLine($"| {eventName.PadRight(50)} |");
                        Console.WriteLine($"| {performer.PadRight(50)} |");
                        Console.WriteLine(horizontalSpace);
                        Console.WriteLine("|-" + horizontalLine + "-|");

                        /*Ticket information - participant*/
                        Console.WriteLine(horizontalSpace);
                        string name = "Participant: " + t.Participant.Name + " " + t.Participant.Surname;
                        Console.WriteLine($"| {name.ToString().PadRight(50)} |");

                        /*Ticket information - Stand and Seat*/
                        string stand = "Stand type: " + t.StandType.ToString() + " " + t.NumberOfaStand.ToString() + ", Seat: " + t.SeatColumn.ToString() + ", " + t.SeatColumn.ToString();

                        Console.WriteLine($"| {stand.PadRight(50)} |");
                        Console.WriteLine(horizontalSpace);
                        /*Ticket information - Price*/
                        string price;
                        if (t.StandType == StandType.LowerStand)
                        {
                            price = EventList[numberOfanEvent - 1].CostOfLowerStandSeat.ToString() + "$";
                            totalCost += EventList[numberOfanEvent - 1].CostOfLowerStandSeat;
                        }
                        else
                        {
                            price = EventList[numberOfanEvent - 1].CostOfUpperStandSeat.ToString() + "$";
                            totalCost += EventList[numberOfanEvent - 1].CostOfUpperStandSeat;
                        }
                        Console.WriteLine("|-" + horizontalLine + "-|");

                        Console.Write($"|");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(price.PadLeft(52));
                        Console.ResetColor();
                        Console.Write("|\n");


                        Console.WriteLine("+" + horizontalLine + "+");
                    }
                    Console.ForegroundColor= ConsoleColor.DarkRed;
                    Console.WriteLine($"\n\nTotal cost of all tickets is:" + totalCost + "$");
                    
                    Console.ResetColor();

                    Console.ReadKey();
                    break;
                }
               
            }while (true);
        }
        

    }
    internal class Program
    {
        static void PressKeyToContinue()
        {
            Console.ForegroundColor= ConsoleColor.DarkGreen;
            Console.WriteLine("PRESS ENTER to continue");
            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
        }
        
        static string DisplayMenu()
        {

            Console.Clear();
            string horizontalSpace = new string('-', 50);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Stadium Meneger!");
            Console.ResetColor();
            Console.WriteLine(horizontalSpace);
            

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\tManaging: ");
            Console.ResetColor();
            Console.WriteLine("\t\t1. Adding a stadium");
            Console.WriteLine("\t\t2. Adding an event");
            Console.WriteLine("\t\t3. Adding an performer");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n\tDisplaying Informations: ");
            Console.ResetColor();
            Console.WriteLine("\t\t4. Display all stadiums");
            Console.WriteLine("\t\t5. Display all events");
            Console.WriteLine("\t\t6. Display all performers");
            Console.WriteLine("\t\t7. Display all tickets");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n\tTicket Management: ");
            Console.ResetColor();
            Console.WriteLine("\t\t8. Adding ticket");

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\nLeaving program - enter q");
            Console.ResetColor();

            Console.WriteLine("\nSelect option: ");
            
            

            return Console.ReadLine();

            
        }
        static void Main(string[] args)
        {
        
            string horizontalSpace = new string(' ', 50);
           
            
           
            string asciiArt = @"
                 .d8888b.  888                  888 d8b                                       
                d88P  Y88b 888                  888 Y8P                                       
                Y88b.      888                  888                                           
                 ""Y888b.   888888  8888b.   .d88888 888 888  888 88888b.d88b.                 
                    ""Y88b. 888        ""88b d88"" 888 888 888  888 888 ""888 ""88b               
                      ""888 888    .d888888 888  888 888 888  888 888  888  888                
                Y88b  d88P Y88b.  888  888 Y88b 888 888 Y88b 888 888  888  888                
                 ""Y8888P""   ""Y888 ""Y888888  ""Y88888 888  ""Y88888 888  888  888                
                                                                              
                                                                              
                                                                              
                 .d8888b.                                              888                    
                d88P  Y88b                                             888                    
                888    888                                             888                    
                888         .d88b.  88888b.   .d88b.  888d888  8888b.  888888  .d88b.  888d888
                888  88888 d8P  Y8b 888 ""88b d8P  Y8b 888P""       ""88b 888    d88""88b 888P""  
                888    888 88888888 888  888 88888888 888     .d888888 888    888  888 888    
                Y88b  d88P Y8b.     888  888 Y8b.     888     888  888 Y88b.  Y88..88P 888    
                 ""Y8888P88  ""Y8888  888  888  ""Y8888  888     ""Y888888  ""Y888  ""Y88P""  888    
                ";
            Console.WriteLine(asciiArt);
            Thread.Sleep(2000);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Welcome to the Stadium Event Manager!".PadLeft(70, ' '));
            Console.ResetColor();
            Thread.Sleep(2000);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("PRESS ENTER TO CONTINUE");

            Console.ResetColor();
            Console.ReadKey();
            Console.Clear();
            StatiumCreator s1 = new StatiumCreator();
            
            Console.WriteLine(" It allows you to:\n\tcreate stadiums, \n\tadd events, with different performers, \n\tsell tickets to particular events, choosing seats");
            PressKeyToContinue();
            string answer = "";
            do
            {
                answer = DisplayMenu();
                switch(answer)
                {
                    case "1"://Adding a stadium
                        Console.Clear();
                        s1.AddStadium();
                        break;

                    case "2":// Adding an event
                        Console.Clear();
                        s1.AddEvent();
                        break;


                    case "3":// Adding an event
                        Console.Clear();
                        s1.AddPerformer();
                        break;
                        
                    case "4"://Display all Stadiums
                        Console.Clear();
                        s1.DisplayStadium_Without_Checking_Avaliablity();
                        break;

                    case "5"://Display all events
                        Console.Clear();
                        string horizontalLine = new string('-', 50);

                        Console.ForegroundColor = ConsoleColor.DarkCyan;

                        Console.WriteLine("All events: ");
                        Console.WriteLine(horizontalLine);
                        Console.ResetColor();
                        
                        s1.DisplayEvents();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("PRESS ENTER TO CONTINUE");
                        Console.ReadKey();
                        break;

                    case "6"://Display all Performers
                        Console.Clear();
                        s1.DisplayPerformers();
                        Console.ReadKey();
                        break;

                    case "7"://Display all tickets
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        s1.DisplayAllTickets();
                        Console.ReadKey();
                        break;
                    case "8"://Adding tickets
                        Console.Clear();
                        s1.AddingTicket_ChoosingEvent();
                        Console.ReadKey();
                        break;
                    case "q":
                        return;

                    default:
                        Console.ForegroundColor= ConsoleColor.DarkRed;
                        Console.WriteLine("Enter value 1 - 8");
                        Console.ResetColor();
                        break;
                   
        
                   


                }


            } while (true);

            Console.ReadKey();
            
        }
       
    }
}
