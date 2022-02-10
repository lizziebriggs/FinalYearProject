using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private float startX;
    public float StartX { set => startX = value; }

    private float startZ;
    public float StartZ { set => startZ = value; }

    private float goalX;
    public float GoalX { set => goalX = value; }

    private float goalZ;
    public float GoalZ { set => goalZ = value; }

    
    public void StartGame()
    {
        player.transform.position = new Vector3(startX, 1f, startZ);
    }
}
