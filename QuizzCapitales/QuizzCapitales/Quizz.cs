using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics.Metrics;


namespace QuizzCapitales
{
    internal class Quizz
    {
        public static string[] Countries = GetCountries();
        public static string[] Capitals = GetCapitals();
        public static int GameMode = 0;
        public static GameModeInfo? GameModeObject;
        public static int GoodAnswers = 0;
        public static int BadAnswers = 0;
        public static int OldGoodAnswers = 0;
        public static int OldBadAnswers = 0;
        public static int OldGameMode = 0;

        public static int EasyQuestionNumber = 10;
        public static int MediumQuestionNumber = 25;
        public static int HardQuestionNumber = 50;

        private static int? TotalAnsweredQuestion;
        public static void Jouer()
        {
            bool GameAgain = true;
            Random Random = new Random();
            List<int> UsedIndices = new List<int>();

            while (GameAgain)
            {
                DisplayGameMenu(UsedIndices);
                int TotalQuestions = TotalQuestionGameModeType(GameMode);

                if (OldGameMode != 0)
                {
                    Console.WriteLine($"Résultats du jeu précédent : \n{OldGoodAnswers} bonne(s) réponse(s) sur {TotalQuestionGameModeType(OldGameMode)} questions au total.");
                }
                for (int i = 0; i < TotalQuestions; i++)
                {
                    int RandomIndex;
                    do
                    {
                        RandomIndex = Random.Next(Countries.Length);
                    } while (UsedIndices.Contains(RandomIndex));
                    UsedIndices.Add(RandomIndex);
                    DispatchGameModeAnswers(RandomIndex);

                }
                TotalAnsweredQuestion = GoodAnswers + BadAnswers;
                OldBadAnswers = BadAnswers;
                OldGoodAnswers = GoodAnswers;
                OldGameMode = GameMode;
                Console.WriteLine($"\n{GoodAnswers} bonne(s) réponse(s) sur {TotalAnsweredQuestion} question(s) au total !");
                GameAgain = AskGameAgain();
            }
        }

        public static string[] GetCountries()
        {
            return new string[] {
                "Albanie", "Allemagne", "Andorre", "Autriche", "Belgique", "Biélorussie", "Bosnie-Herzégovine", "Bulgarie",
                "Chypre", "Croatie", "Danemark", "Espagne", "Estonie", "Finlande", "France", "Grèce", "Hongrie", "Irlande",
                "Italie", "Lettonie", "Lituanie", "Luxembourg", "Malte", "Moldavie", "Monaco", "Monténégro", "Norvège", "Pays-Bas",
                "Pologne", "Portugal", "République tchèque", "Roumanie", "Royaume-Uni", "Russie", "Saint-Marin", "Serbie",
                "Slovaquie", "Slovénie", "Suède", "Suisse", "Ukraine", "Afghanistan", "Arabie saoudite", "Arménie", "Azerbaïdjan",
                "Bahreïn", "Bangladesh", "Bhoutan", "Brunei", "Cambodge", "Chine", "Corée du Nord", "Corée du Sud", "Émirats arabes unis",
                "Géorgie", "Inde", "Indonésie", "Irak", "Iran", "Israël", "Japon", "Jordanie", "Kazakhstan", "Kirghizistan", "Koweït",
                "Laos", "Liban", "Malaisie", "Maldives", "Mongolie", "Myanmar", "Népal", "Oman", "Ouzbékistan", "Pakistan", "Palestine",
                "Philippines", "Qatar", "Singapour", "Sri Lanka", "Syrie", "Tadjikistan", "Taïwan", "Thaïlande", "Timor oriental", "Turkménistan",
                "Turquie", "Viêt Nam", "Yémen", "Zambie", "Zimbabwe", "Canada", "Mexique", "Australie", "Nouvelle-Zélande", "Brésil", "Argentine", "Chili", "Colombie", "Pérou", "Équateur", "Afrique du Sud", "Égypte", "Kenya", "Nigeria", "Sénégal", "Maroc", 
            };
        }

        public static string[] GetCapitals()
        {
            return new string[] {
                "Tirana", "Berlin", "Andorre-la-Vieille", "Vienne", "Bruxelles", "Minsk", "Sarajevo", "Sofia",
                "Nicosie", "Zagreb", "Copenhague", "Madrid", "Tallinn", "Helsinki", "Paris", "Athènes", "Budapest", "Dublin",
                "Rome", "Riga", "Vilnius", "Luxembourg", "La Valette", "Chisinau", "Monaco", "Podgorica", "Oslo", "Amsterdam",
                "Varsovie", "Lisbonne", "Prague", "Bucarest", "Londres", "Moscou", "Saint-Marin", "Belgrade", "Bratislava", "Ljubljana",
                "Stockholm", "Berne", "Kiev", "Kaboul", "Riyad", "Erevan", "Bakou", "Manama", "Dacca", "Thimphou", "Bandar Seri Begawan",
                "Phnom Penh", "Pékin", "Pyongyang", "Séoul", "Abou Dabi", "Tbilissi", "New Delhi", "Jakarta", "Bagdad", "Téhéran",
                "Jérusalem", "Tokyo", "Amman", "Astana", "Bichkek", "Koweït", "Vientiane", "Beyrouth", "Kuala Lumpur", "Malé", "Oulan-Bator",
                "Naypyidaw", "Katmandou", "Mascate", "Tachkent", "Islamabad", "Ramallah / Jérusalem", "Manille", "Doha", "Singapour", "Colombo",
                "Damas", "Douchanbé", "Taipei", "Bangkok", "Dili", "Achgabat", "Ankara", "Hanoï", "Sanaa", "Lusaka", "Harare", "Ottawa", "Mexico", 
                "Canberra", "Wellington", "Brasília", "Buenos Aires", "Santiago", "Bogota", "Lima", "Quito",  "Pretoria", "Le Caire", "Nairobi", "Abuja", "Dakar", "Rabat"
            };
        }

        public static int TotalQuestionGameModeType(int GameMode)
        {
            switch (GameMode)
            {
                case 1:
                    return EasyQuestionNumber;
                case 2:
                    return MediumQuestionNumber;
                case 3:
                    return HardQuestionNumber;
                default:
                    return EasyQuestionNumber;
            }
        }

        static void DisplayGameMenu(List<int> UsedIndices)
        {
            UsedIndices.Clear();
            Console.Title = "Bienvenue sur Quizz Capitales !";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("________        .__                 _________               .__  __         .__                 \r\n\\_____  \\  __ __|__|_______________ \\_   ___ \\_____  ______ |__|/  |______  |  |   ____   ______\r\n /  / \\  \\|  |  \\  \\___   /\\___   / /    \\  \\/\\__  \\ \\____ \\|  \\   __\\__  \\ |  | _/ __ \\ /  ___/\r\n/   \\_/.  \\  |  /  |/    /  /    /  \\     \\____/ __ \\|  |_> >  ||  |  / __ \\|  |_\\  ___/ \\___ \\ \r\n\\_____\\ \\_/____/|__/_____ \\/_____ \\  \\______  (____  /   __/|__||__| (____  /____/\\___  >____  >\r\n       \\__>              \\/      \\/         \\/     \\/|__|                 \\/          \\/     \\/ \n\n");
            Console.ResetColor();

            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Veuillez préciser le mode de jeu que vous souhaitez via le numéro :");
            Console.Write("\t1. ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Facile ({TotalQuestionGameModeType(1)} questions & 3 propositions)");
            Console.ResetColor();

            Console.Write("\n\t2. ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Moyen ({TotalQuestionGameModeType(2)} questions & 5 propositions)");
            Console.ResetColor();

            Console.Write("\n\t3. ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Difficile ({TotalQuestionGameModeType(3)} questions & résultat à écrire)");
            Console.ResetColor();

            Console.WriteLine("\n------------------------------------------------------------------");

            Console.Write("→ ");
            GameMode = int.Parse(Console.ReadLine());
            GameModeInfo? GameModeObject = GetGameModeInfo(GameMode);
            Console.Clear();
        }

        static void DispatchGameModeAnswers(int RandomIndex)
        {
            switch (GameMode)
            {
                case 1:
                    if (CountryQuestionEasy(RandomIndex)) GoodAnswers++;
                    else BadAnswers++;
                    break;
                case 2:
                    if (CountryQuestionMedium(RandomIndex)) GoodAnswers++;
                    else BadAnswers++;
                    break;
                case 3:
                    if (CountryQuestionHard(RandomIndex)) GoodAnswers++;
                    else BadAnswers++;
                    break;
                default:
                    break;
            }

            Console.Title = $"Quizz Capitales - {GoodAnswers} ✓ / {GoodAnswers + BadAnswers} ✘";
        }


        static bool CountryQuestionHard(int CountryIndex)
        {
            AskWichCapital(Countries[CountryIndex]);
            Console.Write("\n→ ");
            string? PlayerQuestionAnswer = Console.ReadLine();
            if (PlayerQuestionAnswer?.ToLower() == Capitals[CountryIndex].ToLower())
            {
                GoodAnswer();
                return true;
            }
            else
            {
                BadAnswer(Capitals[CountryIndex]);
                return false;
            }
        }

        static bool CountryQuestionMedium(int CountryIndex)
        {
            Random random = new Random();
            List<int> randomIndices = new List<int>();

            while (randomIndices.Count < 4)
            {
                int randomIndex = random.Next(Countries.Length);
                if (randomIndex != CountryIndex && !randomIndices.Contains(randomIndex))
                {
                    randomIndices.Add(randomIndex);
                }
            }

            randomIndices.Add(CountryIndex);

            randomIndices = randomIndices.OrderBy(i => random.Next()).ToList();

            AskWichCapital(Countries[CountryIndex]);

            for (int i = 0; i < randomIndices.Count; i++)
            {
                int optionIndex = randomIndices[i];
                Console.WriteLine($"{i + 1}. {Capitals[optionIndex]}");
            }
            int correctOption = randomIndices.IndexOf(CountryIndex) + 1;
            Console.Write("\n→ ");
            string? playerAnswer = Console.ReadLine();

            if (playerAnswer?.ToLower() == correctOption.ToString().ToLower())
            {
                GoodAnswer();
                return true;
            }
            else
            {
                BadAnswer(Capitals[CountryIndex]);
                return false;
            }
        }
        static bool CountryQuestionEasy(int CountryIndex)
        {
            Random random = new Random();
            List<int> randomIndices = new List<int>();

            while (randomIndices.Count < 2)
            {
                int randomIndex = random.Next(Countries.Length);
                if (randomIndex != CountryIndex && !randomIndices.Contains(randomIndex))
                {
                    randomIndices.Add(randomIndex);
                }
            }

            randomIndices.Add(CountryIndex);

            randomIndices = randomIndices.OrderBy(i => random.Next()).ToList();

            AskWichCapital(Countries[CountryIndex]);

            for (int i = 0; i < randomIndices.Count; i++)
            {
                int optionIndex = randomIndices[i];
                Console.WriteLine($"{i + 1}. {Capitals[optionIndex]}");
            }
            int correctOption = randomIndices.IndexOf(CountryIndex) + 1;
            Console.Write("\n→ ");
            string? playerAnswer = Console.ReadLine();

            if (playerAnswer?.ToLower() == correctOption.ToString().ToLower())
            {
                GoodAnswer();
                return true;
            }
            else
            {
                BadAnswer(Capitals[CountryIndex]);
                return false;
            }
        }
        static void AskWichCapital(string country)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\nQuelle est la capitale du pays suivant : {country} ?");
            Console.ResetColor();
        }
        static void BadAnswer(string correctAnswer)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Mauvaise réponse, la capitale était {correctAnswer} !");
            Console.ResetColor();
        }
        static void GoodAnswersCounter(string counter)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n{counter} bonne(s) réponse(s) !");
            Console.ResetColor();
        }

        static void GoodAnswer()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bonne réponse !");
            Console.ResetColor();
        }

        static bool AskGameAgain()
        {
            string playerAnswerGameAgain;

            do
            {
                Console.WriteLine("Voulez-vous rejouer ? (O/N)");
                playerAnswerGameAgain = Console.ReadLine();
                playerAnswerGameAgain = playerAnswerGameAgain.ToLower();
            } while (playerAnswerGameAgain != "o" && playerAnswerGameAgain != "n");

            return GetGameAgainVerification(playerAnswerGameAgain);
        }

        static bool GetGameAgainVerification(string playerAnswer)
        {
            if (playerAnswer == "o")
            {
                Console.Clear();
                GoodAnswers = 0;
                BadAnswers = 0;

                return true;
            }
            else
            {
                return false;
            }
        }

        public class GameModeInfo
        {
            public string? Label { get; set; }
            public string? Value { get; set; }
        }
        static GameModeInfo? GetGameModeInfo(int gameModeIndex)
        {
            if (gameModeIndex == 1)
            {
                return new GameModeInfo { Label = "Facile", Value = "easy" };
            }
            else if (gameModeIndex == 2)
            {
                return new GameModeInfo { Label = "Moyen", Value = "medium" };
            }
            else if (gameModeIndex == 3)
            {
                return new GameModeInfo { Label = "Difficile", Value = "hard" };
            }
            else
            {
                return null;
            }
        }

        public static void JouerOld()
        {
            bool GameAgain = true;
            while (GameAgain)
            {
                int GoodAnswers = 0;

                for (int i = 0; i < Countries.Length; i++)
                {
                    if (CountryQuestionEasy(i)) GoodAnswers++;
                }
                GoodAnswersCounter(GoodAnswers.ToString());

                GameAgain = AskGameAgain();
            }
        }

        public static void JouerOld(params int[] CountrieList)
        {
            bool GameAgain = true;
            while (GameAgain)
            {
                int GoodAnswers = 0;

                foreach (int Country in CountrieList)
                {
                    if (Country > 0 && Country <= Countries.Length && CountryQuestionEasy(Country)) GoodAnswers++;
                }
                GoodAnswersCounter(GoodAnswers.ToString());

                GameAgain = AskGameAgain();
            }
        }
    }
}