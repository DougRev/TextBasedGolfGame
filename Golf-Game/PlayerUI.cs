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
        private readonly GolfCourseRepo _holeRepo = new GolfCourseRepo();

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
            Console.Clear();
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
            string userInput = Console.ReadLine().ToLower();
            GolfCourse courseChoice = _courseRepo.GetCourseByName(userInput);


            PlayCourse(player, courseChoice);
        }

        private void PlayCourse(Player player, GolfCourse course)
        {
            Console.Clear();
            Console.WriteLine($"---------------Welcome to {course.CourseName}!-----------------------");
            foreach (var hole in course.HoleList)
            {
                PlayHole2(player,hole);
            }
        }
        private void PlayHole2(Player player, Hole hole)
        {
            Random random = new Random();
            ViewHoleDetails(hole);
            int distHit = 0;
            int stroke = 0;
            int distRemaining = 260;
            int distance = hole.Distance;
            distRemaining = distance - distHit;
            while (distRemaining != 0)
            {
                Console.Clear();
                Console.WriteLine($"Distance Remaining:{distRemaining}");
                Console.WriteLine($"Stroke: {stroke}");
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
                        GolfClub club = _clubRepo.GetClubByName("driver");
                        int quality = random.Next(5, 10) + player.Strength;
                        int strength = player.Strength;
                        int clubPower = club.Distance;
                        int distHit1 = quality + clubPower + strength;
                        int distRemaining1 = distRemaining - distHit1;
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Putting(player, hole);
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                    }
                    else
                    {
                        GolfClub club = _clubRepo.GetClubByName("driver");
                        int quality = random.Next(5, 10) + player.Strength;
                        int strength = player.Strength;
                        int clubPower = club.Distance;
                        int distHit1 = quality + clubPower + strength;
                        int distRemaining1 = distRemaining;
                        Console.WriteLine($"You boldly step up to hit a driver off the deck.");
                        Console.WriteLine($"After making contact its not exactly how you intended it....but at least you tried. You have {distRemaining1} left to the pin.");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        stroke++;
                    }
                }
                else if (userInput == "3")
                {
                    GolfClub club = _clubRepo.GetClubByName("3 wood");
                    int quality = random.Next(5, 10) + player.Strength;
                    int strength = player.Strength;
                    int clubPower = club.Distance;
                    int distHit1 = quality + clubPower + strength;
                    int distRemaining1 = distRemaining - distHit1;
                    if (distHit1 > distRemaining)
                    {
                        int distanceOver = Math.Abs(distRemaining1 - distHit1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        stroke++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                    }
                }
                else if (userInput == "5")
                {
                    GolfClub club = _clubRepo.GetClubByName("5 iron");
                    int quality = random.Next(5, 10) + player.Strength;
                    int strength = player.Strength;
                    int clubPower = club.Distance;
                    int distHit1 = quality + clubPower + strength;
                    int distRemaining1 = distRemaining - distHit1;
                    if (distHit1 > distRemaining)
                    {
                        int distanceOver = Math.Abs(distRemaining1 - distHit1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        stroke++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                    }
                }
                else if (userInput == "6")
                {
                    GolfClub club = _clubRepo.GetClubByName("6 iron");
                    int quality = random.Next(5, 10) + player.Strength;
                    int strength = player.Strength;
                    int clubPower = club.Distance;
                    int distHit1 = quality + clubPower + strength;
                    int distRemaining1 = distRemaining - distHit1;
                    if (distHit1 > distRemaining)
                    {
                        int distanceOver = Math.Abs(distRemaining1 - distHit1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        stroke++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                    }
                }
                else if (userInput == "7")
                {
                    GolfClub club = _clubRepo.GetClubByName("7 iron");
                    int quality = random.Next(5, 10) + player.Strength;
                    int strength = player.Strength;
                    int clubPower = club.Distance;
                    int distHit1 = quality + clubPower + strength;
                    int distRemaining1 = distRemaining - distHit1;
                    if (distHit1 > distRemaining)
                    {
                        int distanceOver = Math.Abs(distRemaining1 - distHit1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        stroke++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                    }
                }
                else if (userInput == "8")
                {
                    GolfClub club = _clubRepo.GetClubByName("8 iron");
                    int quality = random.Next(5, 10) + player.Strength;
                    int strength = player.Strength;
                    int clubPower = club.Distance;
                    int distHit1 = quality + clubPower + strength;
                    int distRemaining1 = distRemaining - distHit1;
                    if (distHit1 > distRemaining)
                    {
                        int distanceOver = Math.Abs(distRemaining1 - distHit1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        stroke++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                    }
                }
                else if (userInput == "9")
                {
                    GolfClub club = _clubRepo.GetClubByName("9 iron");
                    int quality = random.Next(5, 10) + player.Strength;
                    int strength = player.Strength;
                    int clubPower = club.Distance;
                    int distHit1 = quality + clubPower + strength;
                    int distRemaining1 = distRemaining - distHit1;
                    if (distHit1 > distRemaining)
                    {
                        int distanceOver = Math.Abs(distRemaining1 - distHit1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        stroke++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                    }
                }
                else if (userInput == "pw")
                {
                    GolfClub club = _clubRepo.GetClubByName("pitching wedge");
                    int quality = random.Next(5, 10) + player.Strength;
                    int strength = player.Strength;
                    int clubPower = club.Distance;
                    int distHit1 = quality + clubPower + strength;
                    int distRemaining1 = distRemaining - distHit1;
                    if (distHit1 > distRemaining)
                    {
                        int distanceOver = Math.Abs(distRemaining1 - distHit1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        stroke++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                    }
                }
                else if (userInput == "aw")
                {
                    GolfClub club = _clubRepo.GetClubByName("approach wedge");
                    int quality = random.Next(5, 10) + player.Strength;
                    int strength = player.Strength;
                    int clubPower = club.Distance;
                    int distHit1 = quality + clubPower + strength;
                    int distRemaining1 = distRemaining - distHit1;
                    if (distHit1 > distRemaining)
                    {
                        int distanceOver = Math.Abs(distRemaining1 - distHit1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        stroke++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                    }
                }
                else if (userInput == "sw")
                {
                    Console.WriteLine("Are we doing a:\n" +
                        "1.Full Swing \n" +
                        "2.Chip");
                    int userInput1 = Convert.ToInt32(Console.ReadLine());
                    if (userInput1 == 1)
                    {
                        GolfClub club = _clubRepo.GetClubByName("sand wedge");
                        int quality = random.Next(5, 10) + player.Strength;
                        int strength = player.Strength;
                        int clubPower = club.Distance;
                        int distHit1 = quality + clubPower + strength;
                        int distRemaining1 = distRemaining - distHit1;
                        if (distHit1 > distRemaining)
                        {
                            int distanceOver = Math.Abs(distRemaining1 - distHit1);
                            Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                        else
                        {
                            if (distRemaining1 <= 10)
                            {
                                hole.DistanceRemaining = distRemaining1;
                                Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                                Console.ReadKey();
                                Putting(player, hole);
                                distRemaining = 0;
                            }
                            else
                            {
                                Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                                Console.ReadKey();
                                distHit += distHit1;
                                distRemaining = distRemaining1;
                                stroke++;
                            }
                        }
                    }
                    else if(userInput1 == 2)
                    {
                        GolfClub club = _clubRepo.GetClubByName("sand wedge");
                        int chipPower = club.Distance / 8;
                        int distHit1 = chipPower;
                        int distRemaining1 = distRemaining - distHit1;
                        if (distHit1 > distRemaining)
                        {
                            int distanceOver = Math.Abs(distRemaining1 - distHit1);
                            Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            stroke++;
                        }
                        else
                        {
                            if (distRemaining1 <= 10)
                            {
                                Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                                Console.ReadKey();
                                Putting(player, hole);
                                distRemaining = 0;
                            }
                            else
                            {
                                Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                                Console.ReadKey();
                                distHit += distHit1;
                                distRemaining = distRemaining1;
                                stroke++;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not a valid input");
                        Console.ReadKey();
                    }
                    
                }
                else
                {
                    Console.WriteLine("You must choose a valid option. Look in your Bag for correct Keys.");
                    Console.ReadKey();
                }
                
            }

        }

        private void Putting(Player player, Hole hole)
        {
            if (hole.DistanceRemaining <= 3)
            {
                int strokes = hole.Strokes;
                Console.WriteLine("You tap in after a great shot on the green");
                Console.ReadKey();
                hole.Strokes++;
                

            }
            else if (hole.DistanceRemaining <= 4)
            {
                int strokes = hole.Strokes;
                Console.WriteLine("You give it your best effort but you still 2 putt.");
                Console.ReadKey();
                hole.Strokes++;
               
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
            Console.WriteLine($"Type:    | {club.Name}     |");
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

        public int GoodShotMid(Player player, Hole hole,string name)
        {
            Random random = new Random();
            GolfClub club = _clubRepo.GetClubByName(name);
            int quality = random.Next(5, 10) + player.Strength;
            int strength = player.Strength;
            int clubPower = club.Distance;
            int distanceHit = quality + clubPower + strength;
            int distRemaining = hole.Distance - distanceHit;
            Console.WriteLine($"Good shot {distanceHit} yards down the middle of the fairway. You have {distRemaining} left to the pin.");
            return distRemaining;
        }

        public void GreatChipShot(Player player, Hole hole, string name)
        {
            Random random = new Random();
            GolfClub club = _clubRepo.GetClubByName(name);
            int quality = random.Next(5, 10) + player.Strength;
            int strength = player.Strength;
            int clubPower = club.Distance;
            int distanceHit = quality + clubPower + strength;
            int distance = hole.Distance;
            distance -= distanceHit;
            Console.WriteLine($"Good shot {distanceHit} yards down the middle of the fairway. You have {distance} left to the pin.");
        }

        public void TestGolfShot(Player player, Hole hole, string name)
        {
            Random random = new Random();
            GolfClub club = _clubRepo.GetClubByName("5 iron");
            int quality = random.Next(5, 10) + player.Strength;
            int strength = player.Strength;
            int clubPower = club.Distance;
            int distHit1 = quality + clubPower + strength;
            int distRemaining1 = hole.Distance - distHit1;
            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
            
        }

        private int BadDriverUsage(Player player, Hole hole, string name)
        {
            Random random = new Random();
            GolfClub club = _clubRepo.GetClubByName(name);
            int quality = random.Next(10, 40) + player.Strength;
            int strength = player.Strength;
            int clubPower = club.Distance;
            int distanceHit = clubPower - quality + strength;
            int distRemaining = hole.Distance - distanceHit;
            Console.WriteLine($"You boldly step up to hit a driver off the deck.");
            Console.WriteLine($"After making contact its not exactly how you intended it....but at least you tried. You have {distRemaining} left to the pin.");
            return distRemaining;
        }

        private void SeedContent()
        {
            GolfCourse coolLake = new GolfCourse();
            coolLake.Id = 1;
            coolLake.CourseName = "cool lake";
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
            golfClub.Name = "driver";
            golfClub.Distance = 225;

            GolfClub golfClub1 = new GolfClub();
            golfClub1.Name = "3 wood";
            golfClub1.Distance = 200;

            GolfClub golfClub2 = new GolfClub();
            golfClub2.Name = "5 iron";
            golfClub2.Distance = 185;

            GolfClub golfClub3 = new GolfClub();
            golfClub3.Name = "6 iron";
            golfClub3.Distance = 175;

            GolfClub golfClub4 = new GolfClub();
            golfClub4.Name = "7 iron";
            golfClub4.Distance = 165;

            GolfClub golfClub5 = new GolfClub();
            golfClub5.Name = "8 iron";
            golfClub5.Distance = 155;

            GolfClub golfClub6 = new GolfClub();
            golfClub6.Name = "9 iron";
            golfClub6.Distance = 145;

            GolfClub golfClub7 = new GolfClub();
            golfClub7.Name = "pitching wedge";
            golfClub7.Distance = 135;

            GolfClub golfClub8 = new GolfClub();
            golfClub8.Name = "approach wedge";
            golfClub8.Distance = 125;

            GolfClub golfClub9 = new GolfClub();
            golfClub9.Name = "sand wedge";
            golfClub9.Distance = 115;

            _clubRepo.AddClubToDatabase(golfClub);
            _clubRepo.AddClubToDatabase(golfClub1);
            _clubRepo.AddClubToDatabase(golfClub2);
            _clubRepo.AddClubToDatabase(golfClub3);
            _clubRepo.AddClubToDatabase(golfClub4);
            _clubRepo.AddClubToDatabase(golfClub5);
            _clubRepo.AddClubToDatabase(golfClub6);
            _clubRepo.AddClubToDatabase(golfClub7);
            _clubRepo.AddClubToDatabase(golfClub8);
            _clubRepo.AddClubToDatabase(golfClub9);
        }


    }
}
