namespace libs;

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
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        dynamic gameData = FileHandler.ReadJson();
        map.MapWidth = gameData.map.width;
        map.MapHeight = gameData.map.height;
        gameObjects.Clear();


        dynamic objectsToLoad = gameData.First.gameObjects;
        if (currentLevel == 2)
        {
            objectsToLoad = gameData.Second.gameObjects;
            for (int i = 0; i < 300; i++)
            {
                Console.WriteLine("Hi from 2");
            }

        }

        else if (currentLevel == 3)
        {
            objectsToLoad = gameData.Third.gameObjects;

            for (int i = 0; i < 300; i++)
            {
                Console.WriteLine("Hi from 3");
            }

        }


        foreach (var gameObject in objectsToLoad)
        {
            AddGameObject(CreateGameObject(gameObject));
        }

        _focusedObject = gameObjects.OfType<Player>().First();
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
            if (obj is Box && map.Get(obj.PosY, obj.PosX).Type == GameObjectType.Target)
            {
                gameObjectFactory.DecrementAmountOfBoxes();
                for (int i = 0; i < 3000; i++)
                {
                    Console.WriteLine("Hi from place");
                }
            }
        }
    }

    public void CheckWinCondition()
    {
        if (gameObjectFactory.AmountOfBoxes == 5 && currentLevel == 1)
        {

            currentLevel++;

            Setup();
    
        }
        else if (gameObjectFactory.AmountOfBoxes == 10 && currentLevel == 2)
        {

              for (int i = 0; i < 300; i++)
            {
                Console.WriteLine(gameObjectFactory.AmountOfBoxes);
            }
            currentLevel++;
            Setup();
            
        }
       
        else
        {
            CurrentGameState = GameState.Won;
            for (int i = 0; i < 300; i++)
            {
                Console.WriteLine("you won!!! :))) ");
            }
        }

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
