using System;
using System.Drawing;
using System.IO;
using NAudio.Wave; 
using System.Threading;

class Program
{
    static void Main()
    {
        // Play the audio greeting
        PlayVoiceGreeting();

        // Show welcome message and logo
        DisplayWelcomeMessage();
        DisplayAsciiArt();

        // Start chatbot interaction
        StartChatbot();
        ChatbotResponses();
    }

    // Method to play voice greeting from a .wav file
    static void PlayVoiceGreeting()
    {
        try
        {
            // Dynamically finds the folder where the app is running
            string audioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "voicenote.wav");

            if (!File.Exists(audioPath))
            {
                Console.WriteLine($"[System] Audio file missing at: {audioPath}");
                return;
            }

            using (var audioFile = new WaveFileReader(audioPath))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();

                // Wait until playback finishes
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error playing audio: " + ex.Message);
            Console.ResetColor();
        }
    }

    // Method to display the ASCII art logo from image
    static void DisplayAsciiArt()
    {
        // Dynamically finds the folder where the app is running
        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.jpg");

        try
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine($"[System] Image file missing at: {imagePath}");
                return;
            }

            using (Bitmap logo = new Bitmap(imagePath))
            {
                int newWidth = 100;
                int newHeight = (int)(logo.Height * newWidth / logo.Width * 0.5);

                using (Bitmap resizedLogo = new Bitmap(logo, new Size(newWidth, newHeight)))
                {
                    string asciiChars = " .:-=+*%@#";

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    for (int y = 0; y < resizedLogo.Height; y++)
                    {
                        for (int x = 0; x < resizedLogo.Width; x++)
                        {
                            Color pixelColor = resizedLogo.GetPixel(x, y);
                            int gray = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;
                            char asciiChar = asciiChars[gray * (asciiChars.Length - 1) / 255];
                            Console.Write(asciiChar);
                        }
                        Console.WriteLine();
                    }
                    Console.ResetColor();
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error loading image: {ex.Message}");
            Console.ResetColor();
        }
    }

    static void DisplayWelcomeMessage()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n          WELCOME TO THE CYBERSECURITY AWARENESS BOT ");
        Console.ResetColor();
    }

    static void StartChatbot()
    {
        Console.Write("\nEnter your name: ");
        string name = Console.ReadLine();

        while (string.IsNullOrWhiteSpace(name))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Name cannot be empty. Please enter your name: ");
            Console.ResetColor();
            name = Console.ReadLine();
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nHello, {name}! Welcome to your Cybersecurity Awareness Bot.");
        Console.WriteLine("You can ask me about cybersecurity topics like passwords, phishing, and safe browsing.");
        Console.ResetColor();
    }

    static void ChatbotResponses()
    {
        string[] questions = {
            "how are you?", "what's your purpose?", "what can i ask you about?",
            "password safety", "phishing", "safe browsing", "exit"
        };

        string[] answers = {
            "I'm a bot, so I don't have feelings, but I'm here to help!",
            "I provide cybersecurity tips to keep you safe online.",
            "You can ask me about password safety, phishing, and safe browsing.",
            "Use strong passwords with a mix of uppercase, lowercase, numbers, and symbols. Avoid using personal details.",
            "Be cautious of emails asking for personal information. Verify links before clicking.",
            "Keep your software updated, avoid suspicious websites, and use antivirus protection.",
            "Goodbye! Stay safe online."
        };

        while (true)
        {
            Console.Write("\nAsk me a question: ");
            string question = Console.ReadLine()?.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(question)) continue;

            bool found = false;
            for (int i = 0; i < questions.Length; i++)
            {
                if (question == questions[i])
                {
                    Console.ForegroundColor = (questions[i] == "exit") ? ConsoleColor.Green : ConsoleColor.White;
                    TypingEffect(answers[i]);
                    Console.ResetColor();
                    if (questions[i] == "exit") return;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                TypingEffect("I didn't quite understand that. Try asking about 'phishing' or 'passwords'.");
                Console.ResetColor();
            }
        }
    }

    static void TypingEffect(string message)
    {
        foreach (char c in message)
        {
            Console.Write(c);
            Thread.Sleep(30);
        }
        Console.WriteLine();
    }
}