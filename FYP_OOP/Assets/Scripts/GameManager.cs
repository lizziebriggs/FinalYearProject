using System.IO;
using Environment;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private MazeGenerator mazeGen;
    
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

    private float frameDelta = 0f;
    private float displayVal;
    private const string fileDest = "D:/Year 3/Final Year Project/Benchmark Test/OOP_PlayResults.txt";


    public void StartGame()
    {
        player.transform.position = new Vector3(startX, 1f, startZ);
    }

    private void Update()
    {
        frameDelta += (Time.deltaTime - frameDelta) * 0.1f;
        displayVal = 1f / frameDelta;
        
        if (player.Health <= 0)
        {
            mazeGen.CreateNewMaze();
            player.Reset();
        }
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(10, 50, 150, 25), "FPS: " + displayVal);
    }

    private void OnApplicationQuit()
    {
        StreamWriter writer = new StreamWriter(fileDest, true);
        writer.WriteLine("Maze Size: " + mazeGen.Width + " x " + mazeGen.Length);
        writer.WriteLine("Enemy Chance: " + mazeGen.EnemyChance);
        writer.WriteLine("Pickup Chance: " + mazeGen.PickupChance);
        writer.WriteLine("FPS: " + displayVal + "\n\n");
        writer.Close();
    }
}
