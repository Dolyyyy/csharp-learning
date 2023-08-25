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

        public static int EasyQuestionNumber = 10;
        public static int MediumQuestionNumber = 25;
        public static int HardQuestionNumber = 50;

        private static string? PlayerAnswerGameAgain;
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
                Console.WriteLine($"\n{GoodAnswers} bonne(s) réponse(s) sur {TotalAnsweredQuestion} questions au total !");
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
                "Turquie", "Viêt Nam", "Yémen"
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
                "Naypyidaw", "Katmandou", "Mascate", "Tachkent", "Islamabad", "Jérusalem", "Manille", "Doha", "Singapour", "Colombo",
                "Damas", "Douchanbé", "Taipei", "Bangkok", "Dili", "Achgabat", "Ankara", "Hanoï", "Sanaa"
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

            Console.WriteLine($"Veuillez préciser le mode de jeu que vous souhaitez via le numéro :\n1. Facile ({TotalQuestionGameModeType(1)} questions)\n2. Moyen ({TotalQuestionGameModeType(2)} questions)\n3. Difficile ({TotalQuestionGameModeType(3)} questions)");
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
        }


        static bool CountryQuestionHard(int CountryIndex)
        {
            Console.WriteLine($"\nQuelle est la capitale du pays suivant : {Countries[CountryIndex]} ({CountryIndex}) ?");
            string? PlayerQuestionAnswer = Console.ReadLine();
            if (PlayerQuestionAnswer == Capitals[CountryIndex])
            {
                Console.WriteLine("Félicitations !");
                return true;
            }
            else
            {
                Console.WriteLine($"Mauvaise réponse, la capitale était {Capitals[CountryIndex]} !");
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

            Console.WriteLine($"\nQuelle est la capitale du pays suivant : {Countries[CountryIndex]} ?");

            for (int i = 0; i < randomIndices.Count; i++)
            {
                int optionIndex = randomIndices[i];
                Console.WriteLine($"{i + 1}. {Capitals[optionIndex]}");
            }
            int correctOption = randomIndices.IndexOf(CountryIndex) + 1;
            string? playerAnswer = Console.ReadLine();

            if (playerAnswer == correctOption.ToString())
            {
                Console.WriteLine("Félicitations !");
                return true;
            }
            else
            {
                Console.WriteLine($"Mauvaise réponse, la capitale était {Capitals[CountryIndex]} !");
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

            Console.WriteLine($"\nQuelle est la capitale du pays suivant : {Countries[CountryIndex]} ?");

            for (int i = 0; i < randomIndices.Count; i++)
            {
                int optionIndex = randomIndices[i];
                Console.WriteLine($"{i + 1}. {Capitals[optionIndex]}");
            }
            int correctOption = randomIndices.IndexOf(CountryIndex) + 1;
            string? playerAnswer = Console.ReadLine();

            if (playerAnswer == correctOption.ToString())
            {
                Console.WriteLine("Félicitations !");
                return true;
            }
            else
            {
                Console.WriteLine($"Mauvaise réponse, la capitale était {Capitals[CountryIndex]} !");
                return false;
            }
        }




        static bool AskGameAgain()
        {
            Console.WriteLine("Voulez-vous rejouer ? (O/N)");
            PlayerAnswerGameAgain = Console.ReadLine();
            return GetGameAgainVerification();
        }

        static bool GetGameAgainVerification()
        {
            if (PlayerAnswerGameAgain == "O" || PlayerAnswerGameAgain == "o")
            {
                Console.Clear();
                return true;
            }
            else
            {
                Console.WriteLine("Merci d'avoir joué, à la prochaine !");
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
                Console.WriteLine($"\n{GoodAnswers} bonne(s) réponse(s) !");

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
                Console.WriteLine($"\n{GoodAnswers} bonne(s) réponse(s) !");

                GameAgain = AskGameAgain();
            }
        }
    }
}