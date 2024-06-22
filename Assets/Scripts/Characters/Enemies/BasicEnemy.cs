
public class BasicEnemy : Enemy
{

    override
    public void StartTurn()
    {
        StartCoroutine(TakeBasicTurn());
    }
}
