using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject[] pickupPrefabs;

    public GameObject EnemyPrefab => enemyPrefab;
    public GameObject[] PickupPrefabs => pickupPrefabs;

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
