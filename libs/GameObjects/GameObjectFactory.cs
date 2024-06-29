namespace libs;

public class GameObjectFactory : IGameObjectFactory {
    private int amountOfBoxes = 0;

    public int AmountOfBoxes => amountOfBoxes;

    public GameObject CreateGameObject(dynamic obj) {
        GameObject newObj;
        int type = obj.Type;

        switch (type) {
            case (int)GameObjectType.Player:
                newObj = Player.Instance;
                newObj.PosX = obj.PosX;
                newObj.PosY = obj.PosY;
                break;
            case (int)GameObjectType.Obstacle:
                newObj = obj.ToObject<Obstacle>();
                break;
            case (int)GameObjectType.Box:
                newObj = obj.ToObject<Box>();
                IncrementAmountOfBoxes();
                break;
            case (int)GameObjectType.Target:
                newObj = obj.ToObject<Target>();
                break;
            default:
                newObj = new GameObject();
                break;
        }
        return newObj;
    }

    public void IncrementAmountOfBoxes() {
        amountOfBoxes++;
        Console.WriteLine($"Incrementing amount of boxes: {amountOfBoxes}");
    }

    public void ResetAmountOfBoxes() {
        amountOfBoxes = 0;
    }

    public void DecrementAmountOfBoxes() {
        if (amountOfBoxes > 0) {
            amountOfBoxes--;
            Console.WriteLine($"Decrementing amount of boxes: {amountOfBoxes}");
        }
    }

    public bool AreAllBoxesOnTargets(List<Box> boxes, List<Target> targets)
    {
        Console.WriteLine("Checking if all boxes are on targets.");
        foreach (var box in boxes)
        {
            bool onTarget = false;
            foreach (var target in targets)
            {
                if (box.PosX == target.PosX && box.PosY == target.PosY)
                {
                    onTarget = true;
                    break;
                }
            }
            if (!onTarget) {
                Console.WriteLine($"Box at ({box.PosX}, {box.PosY}) is not on any target.");
                return false;
            }
        }
        Console.WriteLine("All boxes are on targets.");
        return true;
    }
}
