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
        

 
    }
 public void ResetAmountOfBoxes() {
        amountOfBoxes = 0;
    }
    public void DecrementAmountOfBoxes() {
        if (amountOfBoxes > 0) {
            amountOfBoxes--;
        }
    }
}
