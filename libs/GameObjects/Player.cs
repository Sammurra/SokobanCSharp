namespace libs;

public class Player : GameObject
{
    private static Player instance = null;

    public Map map = GameEngine.Instance.GetMap();

    private Player() : base()
    {
        Type = GameObjectType.Player;
        CharRepresentation = '☻';
        Color = ConsoleColor.DarkYellow;

    }

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Player();
            }
            return instance;
        }

        set { }
    }

    public override void Move(int dx, int dy)
    {
        int goToX = PosX + dx;
        int goToY = PosY + dy;


        GameObject? PotentialBox = map.Get(goToY, goToX);


        if (PotentialBox.Type == GameObjectType.Obstacle) return;

      



        if (PotentialBox.Type == GameObjectType.Box)
        {
            GameObject? NextObject = map.Get(goToY + dy, goToX + dx);


            if (NextObject.Type == GameObjectType.Obstacle
            || NextObject.Type == GameObjectType.Box) return;

            PotentialBox.Move(dx, dy);
            PotentialBox.Color = ConsoleColor.Red;
        }

        this.SetPrevPosY(this.PosY);
        this.SetPrevPosX(this.PosX);
        this.PosX += dx;
        this.PosY += dy;
    }
}