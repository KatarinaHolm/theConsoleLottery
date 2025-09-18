using System;

namespace theConsoleLottery_gruppövning
{
    internal class Program
    {
        //METOD för att kolla av användares inmatning är korrekt.
        //Tar in parametrar för lägsta och högsta tillåtna värde på inmatning. 
        static int UserInput(int minValue, int maxValue)
        {
            while (true)
            {
                // Omvandling från string till int, felhantering om användaren skrivit annat än siffror
                bool isNUmber = int.TryParse(Console.ReadLine(), out int userInput);

                //Kollar om input är siffra som överensstämmer med inskickat min- och maxvärde
                if (isNUmber && userInput >= minValue && userInput <= maxValue) 
                {
                    return userInput;
                }

                // Om användare skrivit annat än siffror eller siffra utanför valt spann ges felmeddelande.
                else
                {
                    Console.Write($"\nDu måste mata in en giltig siffra mellan {minValue} och {maxValue}: ");
                }
            }
        }

        //METOD för användaren ska välja lottnummer och kolla att användare inte har angivit samma siffra (lottnummer) flera gånger. 
        //Vid dublett ges felmeddelande och användare får chans att skriva ny siffra.
        static int[] CollectUniqueTickets(int[] userNumbers)
        {
            //While-loop för att kunna repetera så många gånger som behövs ifall användare råkar skriva samma tal flera gånger.
            //CountForIndexhåller reda på hur många gånger som körts och används för att spara värdena på rätt indexnummer
            //Whileloopen körs tills att countForIndex är lika mycket som userNumbers längd.
            int countForIndex = 0; 
            while (countForIndex<userNumbers.Length) 
            {
                bool exist = false;

                Console.Write("\nSkriv in ett nummer mellan 1-50: ");
                int userInput = UserInput(1, 50); //Kallar på metod UserInput för se att inmatning innehåller tillåtna siffror.

                //Loopar så många gånger som countForIndexs värde
                for (int i = 0; i < countForIndex; i++)
                {
                    //Jämför varje indexplats som har lagts in hittills med användarens senaster inmatning.
                    // Om siffran redan finns i userNumbers ändras värde på "exist" och felmeddelande skrivs ut längre ner. 
                    if (userNumbers[i]== userInput)
                    {
                        exist = true;
                        break; //Går ur for-loop
                    }
                }

                //Skriver ut felmeddelande om siffran redan finns. Går tillbaka från början av while-loopen sedan.
                if (exist)
                {
                    Console.WriteLine("Fel, du har redan skrivit siffran.");
                }

                //Om tillåten siffra skrivits (dvs. har inte skrivits innan) sparas siffran i array.
                //Counter ökar för att hålla reda var i arrayen nästa siffra ska sparas. 
                else
                {
                    userNumbers[countForIndex] = userInput;
                    countForIndex++;
                }
            }
            //När array är full skickas värdena tillbaka.
            return userNumbers;            
        }

        //METOD för att ta fram slumpnummer som är "vinstlotter", kollar att det inte blir några dubletter.
        static int[] GenerateWinningNumbers(int[] winningNumbers)
        {
            var random = new Random();
            bool isDouble = false;

            while (true)
            {
                // Tar fram slumpade nummer och sparar i array för "vinstnummer"
                for (int i = 0; i < winningNumbers.Length; i++)
                {
                    winningNumbers[i] = random.Next(1, 51);
                }

                // Jämför en siffra i taget i winningNumbers med resterande siffror i arrayn för att se om det finns dubletter. 
                for (int i = 0; i < winningNumbers.Length; i++)
                {
                    for (int j = i + 1; j < winningNumbers.Length; j++)
                    {
                        //Om dublett hittas ändras boolen och whileloopen startas igen.
                        if (winningNumbers[i] == winningNumbers[j])
                        {
                            isDouble = true;
                        }
                    }
                }

                //Om ingen dublett har hittas är boolen fortfarande falsk, arrayn skickas tillbaka.
                if (!isDouble)
                {
                    return winningNumbers;
                }
            }
        }

        //METOD för jämföra användares lottnummer med vinstnummer
        static void CompareTickets(int[] winningNumbers, int[] userNumbers)
        {
            int totalWinners = 0; // Count för hålla reda på hur många vinstlotter

            //Loopar igenom användarens nummer och vinstnumren och kollar om någon överensstämmer = vinst!
            for (int i = 0; i < userNumbers.Length; i++)
            {
                bool isWinner = false; //Initierar bool och nollställer när loopen kör nästa varv.

                for (int j = 0; j < winningNumbers.Length; j++)
                {
                    if (userNumbers[i] == winningNumbers[j])
                    {
                        isWinner = true;
                    }
                }

                if (isWinner) //Om vinst har hittats läggs det på countern totalWinners
                {
                    totalWinners++;
                }
            }

            //Meddelande om inget av användarens nummer gav vinst
            if (totalWinners == 0)
            {
                Console.WriteLine("\nDu är en förlorare ;)");
            }

            //Meddelande om hur många vinstlotter användaren hade. 
            else
            {
                Console.WriteLine($"\nAntal vinnande lotter: {totalWinners}");
            }
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Välkommen till Konsollotteriet!");
            bool isPlaying = true;

            while (isPlaying)
            {

                Console.Write("Hur många lotter vill du köpa? (max 10) ");
                int amountOfTickets = UserInput(1, 10); // Metoden kollar att användaren skriver en siffra mellan 1 och 10.

                int[] userNumbers = new int[amountOfTickets]; //Array för användarens valda lottnummer
                int[] winningNumbers = new int[3];        //Array för vinstnummer framtagna med slumpen      
                               
                userNumbers = CollectUniqueTickets(userNumbers);   //Kallar på metod för att användaren ska välja lottnummer              

                GenerateWinningNumbers(winningNumbers); //Skapar vinstnummer med hjälp av slumpen

                CompareTickets(winningNumbers, userNumbers); //Jämför vinstnummer med användares lottnummer: vinst?

                Console.Write("\nVill du spela igen? Välj 1:(ja) eller 2:(nej): ");
                int userChoice = UserInput(1, 2);

                //Bryter loopen om användare inte vill spela mer
                if (userChoice == 2)
                {
                    isPlaying = false;
                }

                //Rensar konsol om använare vill fortsätta spela, program börjar från början igen. 
                else
                {
                    Console.Clear();
                }
            }
        }
    }
}
