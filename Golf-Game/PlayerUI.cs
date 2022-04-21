using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Golf_Game
{
    public class PlayerUI
    {
        private readonly GolferRepo _gRepo = new GolferRepo();
        private readonly GolfCourseRepo _cRepo = new GolfCourseRepo();
        private readonly GolfClubsRepo _clubRepo = new GolfClubsRepo();

        public void Run()
        {
            SeedContent();
            RunApplication();
        }

       

        private void RunApplication()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("Welcome to Golf \n" +
                    "1. New Character \n" +
                    "2. See All Characters \n" +
                    "3. Delete Character \n" +
                    "99. Close Application");
                string charChoice = Console.ReadLine();
                switch (charChoice)
                {
                    case "1":
                        NewCharacter();
                        break;
                    case "2":
                        SeeAllGolfers();
                        break;
                    case "3":
                        DeleteCharacter();
                        break;
                    case "99":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Choose a number between 1-4");
                        break;

                }
            }
        }

        private void SeeAllGolfers()
        {
            Console.Clear();
            List<Player> playerList = _gRepo.GetGolfers();
            foreach (var player in playerList)
            {
                ViewPlayerDetails(player);
            }
            Console.ReadKey();
        }

        private void ViewPlayerDetails(Player player)
        {
            Console.WriteLine($"Name:{player.Name}");
            Console.WriteLine($"Strength:{player.Strength}");
            Console.WriteLine($"Accuracy:{player.Accuracy}");
            Console.WriteLine($"Stamina:{player.Stamina}");
            Console.WriteLine($"Handicap:{player.Handicap}");
        }
     
        private void NewCharacter()
        {
            Console.Clear();
            Player player = new Player();
            Console.WriteLine("*NEW GAME*\n" +
                "What is your characters name?");
            player.Name = Console.ReadLine();
            bool success = _gRepo.AddGolferToDatabase(player);

            if (success)
            {
                Console.WriteLine($"{player.Name} has been added to the database.");
                ChooseCourse();
            }
            else
            {
                Console.WriteLine("FAILED");
            }
            Console.ReadKey();
        }
        private void DeleteCharacter()
        {
            Console.Clear();
            Console.WriteLine("Please enter the Golfer's Name");
            string playerName = Console.ReadLine();
            bool success = _gRepo.RemoveGolfer(playerName);
            if (success)
            {
                Console.WriteLine("SUCCESS");
            }
            else
            {
                Console.WriteLine($"Golfer with name: {playerName} does not exist.");
            }
            Console.ReadKey();
        }

        private void ChooseCourse()
        {
            List<GolfCourse> coursesInDb = _cRepo.GetCourses();
            foreach (var course in coursesInDb)
            {
                ViewCourseDetails(course);
            }
            Console.WriteLine("Choose a course:");
            string userInput = Console.ReadLine();
            if (userInput.ToLower() == "cool lake")
            {
                StartRound();
            }
           
        }

        private void ViewGolfClubs(GolfClub club)
        {
            Console.Clear();
            List<GolfClub> clubList = _clubRepo.GetClubs();
            foreach (var player in clubList)
            {
                ViewPlayerDetails(club);
            }
            Console.ReadKey();
        }

        private void ViewCourseDetails(GolfCourse course)
        {
            Console.WriteLine($"Name: {course.CourseName}");
            Console.WriteLine($"Holes: {course.Holes}");
            Console.WriteLine($"Par: {course.ParTotal}");
            Console.WriteLine($"Distance: {course.TotalDistance}");
        }

        private void StartRound()
        {
            Console.WriteLine("Fuck Yes");
            
        }

        private void SeedContent()
        {
            GolfCourse coolLake = new GolfCourse();
            coolLake.CourseName = "Cool Lake";
            coolLake.Holes = 18;
            coolLake.TotalDistance = 2600;
            coolLake.ParTotal = 72;

            _cRepo.AddCourseToDatabase(coolLake);
        }
    }
}
