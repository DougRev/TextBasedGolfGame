using Repositories;
using Repositories.Entities;
using System;
using System.Collections.Generic;

namespace Golf_Game
{
    public class PlayerUI
    {
        private readonly GolferRepo _golferRepo = new GolferRepo();
        private readonly GolfCourseRepo _courseRepo = new GolfCourseRepo();
        private readonly GolfClubsRepo _clubRepo = new GolfClubsRepo();
        private readonly GolfClubsRepo _holeRepo = new GolfClubsRepo();

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
                    "4. Create Hole \n" +
                    "5. View All Courses \n" +
                    "6. Start A Round \n" +
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
                    case "4":
                        CreateHole();
                        break;
                    case "5":
                        ViewCourses();
                        break;
                    case "6":
                        StartRound();
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

        private void StartRound()
        {
            CoolLake();
        }

        private void ViewCourses()
        {
            Console.Clear();
            List<GolfCourse> courseList = _courseRepo.GetCourses();
            foreach (var course in courseList)
            {
                ViewCourseDetails(course);
            }
            Console.ReadKey();
        }

        private void CreateHole()
        {
            Console.Clear();
            Hole hole = new Hole();
            Console.WriteLine("What is the hole number?");
            hole.Number = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("What is the distance in yards?");
            hole.Distance = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("What is the par?");
            hole.Par = Convert.ToInt32(Console.ReadLine());

            bool success = _courseRepo.AddHoleToDatabase(hole);
            if (success)
            {
                Console.WriteLine($"Hole has been succesfully added to the database");

            }
            else
            {
                Console.WriteLine("Hole was not added. Try again.");
            }
            Console.WriteLine("What is the Course ID?");
            int id = Convert.ToInt32(Console.ReadLine());
            _courseRepo.GetCourseById(id);

            bool success2 = _courseRepo.AssignHole(id, hole);
            if (success2)
            {
                Console.WriteLine("Hole has been assigned!");
            }
            else
            {
                Console.WriteLine("Hole was NOT assigned.");
            }
            Console.ReadKey();
        }

        private void SeeAllGolfers()
        {
            Console.Clear();
            List<Player> playerList = _golferRepo.GetGolfers();
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
            bool success = _golferRepo.AddGolferToDatabase(player);

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
            bool success = _golferRepo.RemoveGolfer(playerName);
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
            List<GolfCourse> coursesInDb = _courseRepo.GetCourses();
            foreach (var course in coursesInDb)
            {
                ViewCourseDetails(course);
            }
            Console.WriteLine("Choose a course:");
            string choice = Console.ReadLine();
            _courseRepo.GetCourseByName(choice);
            if (choice.ToLower() == "cool lake")
            {
                CoolLake();
            }
           
        }

        private void CoolLake()
        {
            Console.Clear();
            int distHit = 0;
            GolfCourse coolLake = _courseRepo.GetCourseById(1);
            Console.WriteLine("------------Welcome to Cool Lake Golf Course here in Lebanon, Indiana!-----");
            Console.ReadKey();
            Hole hole1 = coolLake.HoleList[0];
            int distRemain = coolLake.HoleList[0].Distance - distHit;
            while (distRemain > 0)
            {
                ViewHoleDetails(hole1);
                Console.ReadKey();
                PlayHole(hole1);
            }
        }

        private void PlayHole(Hole hole)
        {
            int distHit = 0;
            int distRemain = hole.Distance - distHit;
            GolfClub golfClub = new GolfClub();
            Console.WriteLine($"You approach Hole:{hole.Number} teebox. Distance is {hole.Distance}");
            Console.ReadKey();
            while(distRemain > 0)
            {
                ViewHoleDetails(hole);
                ViewGolfClubs();
                Menu(hole);
            }
        }

        private void Menu(Hole hole)
        {
            System.Random random = new System.Random();
            int distHit = 0;
            int distRemain = hole.Distance - distHit;
            int stroke = 0;
            Console.Write("| Enter Club Choice | or | Look in your (B)ag |");
            string userInput = Console.ReadLine().ToLower();
            if (userInput == "b")
            {
                ViewGolfClubs();
                Console.ReadKey();
            }
            else if (userInput == "d")
            {
              if (stroke == 0)
                {
                    //this still needs to find a way to factor in player stats like strength
                    int power = random.Next(5, 10);
                    GolfClub club = _clubRepo.GetClub("driver");
                    int distance = club.Distance + power;

                    Console.WriteLine("You step up and aboslutely crush your drive down the middle of the fairway");
                }
            }
            else if (userInput == "5")
            {

            }
            else if (userInput == "3w")
            {

            }
            else
            {
                Console.WriteLine("You must choose a valid option. Look in your Bag for correct Keys.");
            }
        }

        private void ViewGolfClubs()
        {
            List<GolfClub> clubList = _clubRepo.GetClubs();
            foreach (var club in clubList)
            {
                ViewClubDetails(club);
            }

            Console.ReadKey();
        }
        private void ViewClubDetails(GolfClub club)
        {
            Console.WriteLine($"Type:    | {club.Type}     |");
            Console.WriteLine($"Distance:| {club.Distance} |");
        }

        private void ViewHoleDetails(Hole hole)
        {
            Console.WriteLine($"|  Hole Number: {hole.Number}   |");
            Console.WriteLine($"|     Distance: {hole.Distance} |");
            Console.WriteLine($"|          Par: {hole.Par}    |");
        }

        private void ViewCourseDetails(GolfCourse course)
        {
            Console.WriteLine($"ID: {course.Id}");
            Console.WriteLine($"Name: {course.CourseName}");
            Console.WriteLine($"Holes: {course.HoleList.Count}");
            Console.WriteLine($"Par: {course.ParTotal}");
            Console.WriteLine($"Distance: {course.TotalDistance}");
        }

        private void SeedContent()
        {
            GolfCourse coolLake = new GolfCourse();
            coolLake.Id = 1;
            coolLake.CourseName = "Cool Lake";
            coolLake.TotalDistance = 2600;
            coolLake.ParTotal = 72;
            _courseRepo.AddCourseToDatabase(coolLake);

            Hole hole = new Hole();
            hole.Number = 1;
            hole.Par = 4;
            hole.Distance = 260;
            _courseRepo.AddHoleToDatabase(hole);
            _courseRepo.AssignHole(1, hole);

            GolfClub golfClub = new GolfClub();
            golfClub.Type = "driver";
            golfClub.Distance = 225;

            GolfClub golfClub1 = new GolfClub();
            golfClub1.Type = "3 wood";
            golfClub1.Distance = 200;

            GolfClub golfClub2 = new GolfClub();
            golfClub2.Type = "5 iron";
            golfClub2.Distance = 185;

            _clubRepo.AddClubToDatabase(golfClub);
            _clubRepo.AddClubToDatabase(golfClub1);
            _clubRepo.AddClubToDatabase(golfClub2);
        }


    }
}
