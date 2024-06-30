namespace libs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class GameEngine
    {
        private static GameEngine? _instance;
        public GameObjectFactory gameObjectFactory;

        public GameState CurrentGameState { get; private set; } = GameState.Running;

        public static GameEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameEngine();
                }
                return _instance;
            }
        }

        private GameEngine()
        {
            gameObjectFactory = new GameObjectFactory();
        }

        private GameObject? _focusedObject;
        private List<GameObject> gameObjects = new List<GameObject>();
        private Map map = new Map();

        private int currentLevel = 1;
        public Map GetMap()
        {
            return map;
        }

        public GameObject GetFocusedObject()
        {
            return _focusedObject;
        }

    public void Setup()
{
    Console.WriteLine("Setting up level: " + currentLevel);
    Console.OutputEncoding = System.Text.Encoding.UTF8;
    dynamic gameData = FileHandler.ReadJson();
    map.MapWidth = gameData.map.width;
    map.MapHeight = gameData.map.height;
    gameObjects.Clear();
    gameObjectFactory.ResetAmountOfBoxes();
    SetGameState(GameState.Running);

    var levelData = new Dictionary<int, dynamic>
    {
        { 1, gameData.First.gameObjects },
        { 2, gameData.Second.gameObjects },
        { 3, gameData.Third.gameObjects }
    };

    if (!levelData.TryGetValue(currentLevel, out var objectsToLoad))
    {
        Console.WriteLine("Congratulations! You've completed all levels!");
        Environment.Exit(0);  // Gracefully exit the game
        return;
    }

    foreach (var gameObject in objectsToLoad)
    {
        AddGameObject(gameObjectFactory.CreateGameObject(gameObject));
        Console.WriteLine($"Added game object: {gameObject.Type} at ({gameObject.PosX}, {gameObject.PosY})");
    }

    _focusedObject = gameObjects.OfType<Player>().FirstOrDefault();
    if (_focusedObject != null)
    {
        Console.WriteLine("Focused object set to player at: (" + _focusedObject.PosX + ", " + _focusedObject.PosY + ")");
    }
    Render();
}


        public void Render()
        {
            Console.Clear();
            map.Initialize();
            PlaceGameObjects();
            for (int i = 0; i < map.MapHeight; i++)
            {
                for (int j = 0; j < map.MapWidth; j++)
                {
                    DrawObject(map.Get(i, j));
                }
                Console.WriteLine();
            }
        }

        public GameObject CreateGameObject(dynamic obj)
        {
            return gameObjectFactory.CreateGameObject(obj);
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            if (gameObject.Type == GameObjectType.Box)
            {
                gameObjectFactory.IncrementAmountOfBoxes();
            }
        }

        private void PlaceGameObjects()
        {
            foreach (var obj in gameObjects)
            {
                map.Set(obj);
            }
        }

        public void CheckWinCondition()
{
    var boxes = gameObjects.OfType<Box>().ToList();
    var targets = gameObjects.OfType<Target>().ToList();

   

    bool allBoxesOnTargets = boxes.All(box => targets.Any(target => target.PosX == box.PosX && target.PosY == box.PosY));

    if (allBoxesOnTargets)
    {
        Console.WriteLine("You won!!!");
        SetGameState(GameState.Won);
    }
  
}

        public void SetGameState(GameState state)
        {
            CurrentGameState = state;
        }

        public void IncrementLevel()
        {
            currentLevel++;
        }

        public void SaveGameState()
        {
            var gameState = new
            {
                CurrentLevel = currentLevel,
                PlayerPosition = new { _focusedObject.PosX, _focusedObject.PosY },
                Boxes = gameObjects.Where(go => go.Type == GameObjectType.Box).Select(box => new { box.PosX, box.PosY }),
                GameState = CurrentGameState.ToString()
            };

            FileHandler.SaveJson(gameState);
            Console.WriteLine("Game state saved!");
        }

        private void DrawObject(GameObject gameObject)
        {
            Console.ResetColor();
            if (gameObject != null)
            {
                Console.ForegroundColor = gameObject.Color;
                Console.Write(gameObject.CharRepresentation);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(' ');
            }
        }
    }
}