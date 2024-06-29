namespace libs;

public sealed class InputHandler {
    private static InputHandler? _instance;
    private GameEngine engine;

    public static InputHandler Instance {
        get {
            if (_instance == null) {
                _instance = new InputHandler();
            }
            return _instance;
        }
    }

    private InputHandler() {
        engine = GameEngine.Instance;
    }

    public void Handle(ConsoleKeyInfo keyInfo) {
       
        if (GameEngine.Instance.CurrentGameState == GameState.Won) {
            Console.WriteLine("U won dont move.");
            return;  
        }

       
        if (keyInfo.Key == ConsoleKey.Escape) {
            Console.WriteLine(" YO YO Saving and exiting game...");
            GameEngine.Instance.SaveGameState(); 
            Environment.Exit(0);
        }

     
        GameObject focusedObject = engine.GetFocusedObject();

        if (focusedObject != null) {
            switch (keyInfo.Key) {
                case ConsoleKey.UpArrow:
                    focusedObject.Move(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    focusedObject.Move(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    focusedObject.Move(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    focusedObject.Move(1, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
