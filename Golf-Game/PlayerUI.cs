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
            MainMenu();
        }

       

        private void MainMenu()
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
                    "6. New Game \n" +
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
                        NewGame();
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

        private void NewGame()
        {
            Console.WriteLine("1. Choose a character \n" +
                "99. Go Back");
            string userInput = Console.ReadLine();
            switch (userInput)
            {
                case "1":
                    ChooseCharacter();
                    break;
                case "99":
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("Please choose a number between 1 or 99.");
                    break;

            }
        }

        private void ChooseCharacter()
        {
            Console.Clear();
            SeeAllGolfers();
            Console.WriteLine("Choose your Golfer and enter their ID");
            int userInput = Convert.ToInt32(Console.ReadLine());
            Player playerChoice = _golferRepo.GetGolferById(userInput);
            StartRound(playerChoice);
            
        }

        private void StartRound(Player player)
        {
            ChooseCourse(player);
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
            Console.WriteLine($"ID: {player.Id}");
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
                ChooseCourse(player);
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

        private void ChooseCourse(Player player)
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
                CoolLake(player);
            }
           
        }

        private void CoolLake(Player player)
        {
            Console.Clear();
            Console.WriteLine("------------Welcome to Cool Lake Golf Course here in Lebanon, Indiana!-----");
            GolfCourse coolLake = _courseRepo.GetCourseById(1);
            Console.ReadKey();
            Hole hole1 = coolLake.HoleList[0];
            int distHit = 0;
            int distRemain = coolLake.HoleList[0].Distance - distHit;
            ViewHoleDetails(hole1);
            Console.WriteLine($"Distance Remaining: {distRemain}");
            Console.ReadKey();
            while (distRemain > 0)
            {
                PlayHole(player, hole1);
            }
        }

        private void PlayHole(Player player, Hole hole)
        {
            Random random = new Random();
            int distance = hole.Distance;
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
                    int quality = random.Next(5, 10) + player.Strength;
                    int strength = player.Strength;
                    GolfClub club = _clubRepo.GetClubByType("driver");
                    int power = club.Distance;
                    int distanceHit = quality + power + strength;
                    int distRemain = hole.Distance - distanceHit;
                    Console.WriteLine($"You step up and aboslutely crush your drive {distanceHit} yards down the middle of the fairway. You have {distRemain} left to the pin.");
                }
            }
            else if (userInput == "5")
            {
                GoodShotMid(player, hole, "5wood");
            }
            else if (userInput == "3")
            {
                GoodShotMid(player, hole, "3wood");
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

        public void GoodShotMid(Player player, Hole hole,string type)
        {
            Random random = new Random();
            GolfClub club = _clubRepo.GetClubByType(type);
            int quality = random.Next(5, 10) + player.Strength;
            int strength = player.Strength;
            //throws error below
            int power = club.Distance;
            int distanceHit = quality + power + strength;
            int distRemain = hole.Distance - distanceHit;
            Console.WriteLine($"You step up and aboslutely crush your drive {distanceHit} yards down the middle of the fairway. You have {distRemain} left to the pin.");
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

            Player player = new Player();
            player.Id = 1;
            player.Name = "DJ";
            player.Strength = 0;
            player.Accuracy = 0;
            player.Stamina = 0;
            player.Handicap = 0;
            player.HolesPlayed = 0;
            _golferRepo.AddGolferToDatabase(player);

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
