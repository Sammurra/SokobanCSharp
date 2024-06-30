using libs;  // Make sure this is included
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        ShowMenu();
    }

    static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("Sokoban Game");
        Console.WriteLine("============");
        Console.WriteLine("1. Start New Game");
        Console.WriteLine("2. Exit");
        Console.WriteLine("Select an option:");

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        HandleMenuOption(keyInfo.Key);
    }

    static void HandleMenuOption(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                StartNewGame();
                break;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                ExitGame();
                break;
            default:
                ShowMenu();
                break;
        }
    }

    static void StartNewGame()
    {
        Console.Clear();
        Console.WriteLine("Starting new game...");

        var dialogHandler = new DialogHandler();  // Ensure DialogHandler is in the libs namespace
        dialogHandler.LoadDialog("../dialog.json");
        dialogHandler.DisplayDialog();  // Display the introduction dialog

        var engine = GameEngine.Instance;
        var inputHandler = InputHandler.Instance;

        engine.Setup();
        RunGameLoop(engine, inputHandler);
    }

    static void ExitGame()
    {
        Console.WriteLine("Exiting game...");
        Environment.Exit(0);
    }

    static void RunGameLoop(GameEngine engine, InputHandler inputHandler)
    {
        while (true)
        {
            engine.Render();

            // Handle keyboard input
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            inputHandler.Handle(keyInfo);
            engine.CheckWinCondition();

            // Check if the player has won and handle level progression
            if (engine.CurrentGameState == GameState.Won)
            {
                Console.WriteLine("Press any key to continue to the next level...");
                Console.ReadKey(true);
                engine.SetGameState(GameState.Running);
                engine.IncrementLevel();
                engine.Setup();
            }
        }
    }
}
