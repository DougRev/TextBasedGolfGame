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
                ScoreCard(course,player);
            }
        }

        private void ScoreCard(GolfCourse course, Player player)
        {
            Console.WriteLine($"Overall Score: {course.ScoreCard}");
            Console.WriteLine($"-----------------{course.CourseName}------------------------------");
            Console.WriteLine($"Hole  | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | ");
            Console.WriteLine($"Par   | {course.HoleList[0].Par} | {course.HoleList[1].Par} | {course.HoleList[2].Par} | {course.HoleList[3].Par} |");
            Console.WriteLine($"-------------------------------------------------------------------");
            Console.WriteLine($"Score | {course.HoleList[0].Strokes} | {course.HoleList[1].Strokes} | {course.HoleList[2].Strokes} | {course.HoleList[3].Strokes} | ");
            Console.ReadKey();
        }

        private void PlayHole2(Player player, Hole hole)
        {
            Random random = new Random();
            ViewHoleDetails(hole);
            Console.ReadKey();
            int distHit = 0;
            int stroke = 0;
            int distRemaining = 260;
            int distance = hole.Distance;
            distRemaining = distance - distHit;
            while (distRemaining != 0)
            {
                Console.Clear();
                Console.WriteLine($"Distance Remaining:{Math.Abs(distRemaining)}");
                Console.WriteLine($"Stroke: {hole.Strokes}");
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
                        if (distHit1 > distRemaining + 11)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            hole.Strokes++;
                            Console.ReadKey();
                            Putting(player, hole);
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            hole.Strokes++;
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
                        hole.Strokes++;
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
                    if (distHit1 > distRemaining + 11)
                    {
                        int distanceOver = Math.Abs(distRemaining1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        hole.Strokes++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            hole.Strokes++;
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            hole.Strokes++;
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
                    if (distHit1 > distRemaining + 11)
                    {
                        int distanceOver = Math.Abs(distRemaining1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        hole.Strokes++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            hole.Strokes++;
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            hole.Strokes++;
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
                    if (distHit1 > distRemaining + 11)
                    {
                        int distanceOver = Math.Abs(distRemaining1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        hole.Strokes++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            hole.Strokes++;
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            hole.Strokes++;
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
                    if (distHit1 > distRemaining + 11)
                    {
                        int distanceOver = Math.Abs(distRemaining1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        hole.Strokes++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            hole.Strokes++;
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            hole.Strokes++;
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
                    if (distHit1 > distRemaining + 11)
                    {
                        int distanceOver = Math.Abs(distRemaining1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        hole.Strokes++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            hole.Strokes++;
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            hole.Strokes++;
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
                    if (distHit1 > distRemaining + 11)
                    {
                        int distanceOver = Math.Abs(distRemaining1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        hole.Strokes++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            hole.Strokes++;
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            hole.Strokes++;
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
                    if (distHit1 > distRemaining + 11)
                    {
                        int distanceOver = Math.Abs(distRemaining1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        hole.Strokes++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            hole.Strokes++;
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            hole.Strokes++;
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
                    if (distHit1 > distRemaining + 11)
                    {
                        int distanceOver = Math.Abs(distRemaining1);
                        Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                        Console.ReadKey();
                        distHit += distHit1;
                        distRemaining = distRemaining1;
                        hole.Strokes++;
                    }
                    else
                    {
                        if (distRemaining1 <= 10)
                        {
                            hole.DistanceRemaining = distRemaining1;
                            Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                            Console.ReadKey();
                            hole.Strokes++;
                            Putting(player, hole);
                            distRemaining = 0;
                        }
                        else
                        {
                            Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            hole.Strokes++;
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
                        if (distHit1 > distRemaining + 11)
                        {
                            int distanceOver = Math.Abs(distRemaining1);
                            Console.WriteLine($"You soar your over the green {distHit1} yards. You have {distanceOver} yards left to the pin. ");
                            Console.ReadKey();
                            distHit += distHit1;
                            distRemaining = distRemaining1;
                            hole.Strokes++;
                        }
                        else
                        {
                            if (distRemaining1 <= 10)
                            {
                                hole.DistanceRemaining = distRemaining1;
                                Console.WriteLine($"What a pure shot {distHit1} yards. Hitting the green.");
                                Console.ReadKey();
                                hole.Strokes++;
                                Putting(player, hole);
                                distRemaining = 0;
                            }
                            else
                            {
                                Console.WriteLine($"Good shot {distHit1} yards down the middle of the fairway. You have {distRemaining1} left to the pin.");
                                Console.ReadKey();
                                distHit += distHit1;
                                distRemaining = distRemaining1;
                                hole.Strokes++;
                            }
                        }
                    }
                    else if(userInput1 == 2)
                    {
                        GolfClub club = _clubRepo.GetClubByName("sand wedge");
                        int chipQuality = player.Accuracy + random.Next(1, 60);
                        if (chipQuality >= 80)
                        {
                            Console.WriteLine($"What a pure shot! Hitting the green rolling up close to the pin.");
                            Console.ReadKey();
                            hole.Strokes++;
                            Putting(player, hole);
                            distRemaining = 0;

                            
                        }
                        else if (chipQuality < 80 && chipQuality >60)
                        {
                            Console.WriteLine("That was a quality chip. Not exactly what you hoped for but cant be mad.");
                            Console.ReadKey();
                            hole.Strokes++;
                            Putting(player, hole);
                            distRemaining = 0;
                            
                        }
                        else
                        {
                            int chance = random.Next(1, 10);
                            int distRemaining1 = distRemaining - chance;
                            if (distRemaining1 <= 10)
                            {
                                Console.WriteLine("Successful chip! It rolls up on to the green. Time to get the putter!");
                                hole.Strokes++;
                                Putting(player, hole);
                                distRemaining = 0;
                                
                            }
                            else
                            {
                                Console.WriteLine($"Well unfortunately that didnt go as planned. You completely duffed that. Luckily it still went {chance} yards. ");
                                Console.ReadKey();
                                distHit += chance;
                                distRemaining = distRemaining1;
                                hole.Strokes++;
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
            Console.WriteLine($"-----------Score on Hole {hole.Number}-------------------");
            Console.WriteLine($"Strokes:{hole.Strokes}");
            if(hole.Strokes == hole.Par)
            {
                Console.WriteLine("Good Par. Stay consistent.");
            }
            else if(hole.Strokes == hole.Par -1)
            {
                Console.WriteLine("Nice Birdie! Keep it up");
                player.Accuracy++;
                player.Strength++;
            }
            else if(hole.Strokes == hole.Par - 2)
            {
                Console.WriteLine("Wow what an amazing EAGLE! You are a legit beast!");
                player.Strength += 2;
                player.Accuracy += 2;
                
            }
            Console.ReadKey();
            Console.Clear();

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
            Console.WriteLine($"|          Par: {hole.Par}   |");
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
            coolLake.CourseName = "cool lake";
            coolLake.TotalDistance = 2600;
            coolLake.ParTotal = 72;
            _courseRepo.AddCourseToDatabase(coolLake);

            Hole hole = new Hole();
            hole.Number = 1;
            hole.Par = 4;
            hole.Distance = 280;
            _courseRepo.AddHoleToDatabase(hole);
            _courseRepo.AssignHole(1, hole);

            Hole hole2 = new Hole();
            hole2.Number = 2;
            hole2.Par = 4;
            hole2.Distance = 340;
            _courseRepo.AddHoleToDatabase(hole2);
            _courseRepo.AssignHole(1, hole2);

            Hole hole3 = new Hole();
            hole3.Number = 3;
            hole3.Par = 3;
            hole3.Distance = 187;
            _courseRepo.AddHoleToDatabase(hole3);
            _courseRepo.AssignHole(1, hole3);

            Hole hole4 = new Hole();
            hole4.Number = 4;
            hole4.Par = 4;
            hole4.Distance = 425;
            _courseRepo.AddHoleToDatabase(hole4);
            _courseRepo.AssignHole(1, hole4);

            /*Hole hole5 = new Hole();
            hole5.Number = 5;
            hole5.Par = 5;
            hole5.Distance = 590;
            _courseRepo.AddHoleToDatabase(hole5);
            _courseRepo.AssignHole(1, hole5);

            Hole hole6 = new Hole();
            hole6.Number = 6;
            hole6.Par = 4;
            hole6.Distance = 370;
            _courseRepo.AddHoleToDatabase(hole6);
            _courseRepo.AssignHole(1, hole6);

            Hole hole7 = new Hole();
            hole7.Number = 7;
            hole7.Par = 3;
            hole7.Distance = 160;
            _courseRepo.AddHoleToDatabase(hole7);
            _courseRepo.AssignHole(1, hole7);

            Hole hole8 = new Hole();
            hole8.Number = 8;
            hole8.Par = 5;
            hole8.Distance = 460;
            _courseRepo.AddHoleToDatabase(hole8);
            _courseRepo.AssignHole(1, hole8);

            Hole hole9 = new Hole();
            hole9.Number = 9;
            hole9.Par = 3;
            hole9.Distance = 146;
            _courseRepo.AddHoleToDatabase(hole9);
            _courseRepo.AssignHole(1, hole9);

            Hole hole10 = new Hole();
            hole10.Number = 10;
            hole10.Par = 4;
            hole10.Distance = 385;
            _courseRepo.AddHoleToDatabase(hole10);
            _courseRepo.AssignHole(1, hole10);

            Hole hole11 = new Hole();
            hole11.Number = 11;
            hole11.Par = 4;
            hole11.Distance = 410;
            _courseRepo.AddHoleToDatabase(hole11);
            _courseRepo.AssignHole(1, hole11);*/

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
