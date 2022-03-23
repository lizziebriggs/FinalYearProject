using UnityEngine;
using UnityEngine.Assertions.Must;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace Environment
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        
        [Header("Settings")]
        [SerializeField] private int width;
        [SerializeField] private int length;
        [SerializeField] private float pathWidth;
        [SerializeField] private float pathHeight;
        [SerializeField] private GameObject floor;
        [SerializeField] private GameObject wall;

        [Header("Spawns")]
        [SerializeField] [Range(0, 1)] private float enemyChance;
        [SerializeField] [Range(0, 1)] private float pickupChance;

        private int[,] mazeData;
        private GameObject pickupsObj;
        private GameObject enemiesObj;
        private GameObject mazeObj;

        private void Start()
        {
            CreateNewMaze();
        }

        public void CreateNewMaze()
        {
            Destroy(pickupsObj);
            Destroy(enemiesObj);
            Destroy(mazeObj);
            
            pickupsObj = new GameObject();
            enemiesObj = new GameObject();
            mazeObj = new GameObject();
            
            mazeData = GenerateMazeData();
            GenerateMazeObject();
            
            SetStartPos(); SetGoalPos();
            gameManager.StartGame();
        }

        private int[,] GenerateMazeData()
        {
            int[,] newMaze = new int[width, length];

            int wMax = newMaze.GetUpperBound(0);
            int lMax = newMaze.GetUpperBound(1);

            for (int i = 0; i <= wMax; i++)
            {
                for (int j = 0; j <= lMax; j++)
                {
                    // Make all edges walls
                    if (i == 0 || j == 0 || i == wMax || j == lMax)
                        newMaze[i, j] = 1;
                    
                    // Only go through every other position
                    else if (i % 2 == 0 && j % 2 == 0)
                    {
                        newMaze[i, j] = 1;

                        // Assign a random neighbour to be a wall
                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        newMaze[i+a, j+b] = 1;
                    }
                }
            }

            return newMaze;
        }

        private void GenerateMazeObject()
        {
            int wMax = mazeData.GetUpperBound(0);
            int lMax = mazeData.GetUpperBound(1);
            float halfH = pathHeight * .5f;

            mazeObj.transform.position = Vector3.zero;
            mazeObj.name = "Maze";
            
            enemiesObj.transform.position = Vector3.zero;
            enemiesObj.name = "Enemies";
            
            pickupsObj.transform.position = Vector3.zero;
            pickupsObj.name = "Pickups";
            
            for (int i = 0; i <= wMax; i++)
            {
                for (int j = 0; j <= lMax; j++)
                {
                    if (mazeData[i, j] == 1) continue;

                    Vector3 floorPos = new Vector3(j * pathWidth, 0, i * pathWidth);

                    Instantiate(floor,
                        floorPos, Quaternion.Euler(90, 0, 0))
                        .transform.parent = mazeObj.transform;
                    
                    SpawnObject(new Vector3(floorPos.x, floorPos.y + 1f, floorPos.z),
                        enemiesObj, pickupsObj);
                    
                    // Face forward
                    if (i - 1 < 0 || mazeData[i-1, j] == 1)
                        Instantiate(wall,
                            new Vector3(j * pathWidth, halfH, (i-.5f) * pathWidth),
                            Quaternion.Euler(0, 180, 0))
                            .transform.parent = mazeObj.transform;

                    // Face left
                    if (j + 1 > lMax || mazeData[i, j+1] == 1)
                        Instantiate(wall,
                            new Vector3((j+.5f) * pathWidth, halfH, i * pathWidth),
                            Quaternion.Euler(0, 90, 0))
                            .transform.parent = mazeObj.transform;

                    // Face right
                    if (j - 1 < 0 || mazeData[i, j-1] == 1)
                        Instantiate(wall,
                            new Vector3((j-.5f) * pathWidth, halfH, i * pathWidth),
                            Quaternion.Euler(0, 270, 0))
                            .transform.parent = mazeObj.transform;

                    // Face back
                    if (i + 1 > wMax || mazeData[i+1, j] == 1)
                        Instantiate(wall,
                            new Vector3(j * pathWidth, halfH, (i+.5f) * pathWidth),
                            Quaternion.Euler(0, 0, 0))
                            .transform.parent = mazeObj.transform;
                }
            }
        }

        private void SpawnObject(Vector3 pos, GameObject enemyParent, GameObject pickupParent)
        {
            float chance = Random.Range(0f, 1f);
            
            // Check to spawn enemy
            if (chance <= enemyChance)
                Instantiate(gameManager.EnemyPrefab, pos, Quaternion.identity)
                    .transform.parent = enemyParent.transform;
            
            // Check to spawn pickup
            if (chance <= pickupChance)
            {
                int pickup = Random.Range(0, gameManager.PickupPrefabs.Length);
                Instantiate(gameManager.PickupPrefabs[pickup], pos, Quaternion.identity)
                    .transform.parent = pickupParent.transform;;
            }
        }

        private void SetStartPos()
        {
            int[,] maze = mazeData;

            int wMax = maze.GetUpperBound(0);
            int lMax = maze.GetUpperBound(1);

            for (int i = 0; i <= wMax; i++)
            {
                for (int j = 0; j <= lMax; j++)
                {
                    // Use first empty space as start pos
                    if (maze[i, j] != 0) continue;

                    gameManager.StartX = i * pathWidth;
                    gameManager.StartZ = j * pathWidth;
                    return;
                }
            }
        }

        private void SetGoalPos()
        {
            int[,] maze = mazeData;

            int wMax = maze.GetUpperBound(0);
            int lMax = maze.GetUpperBound(1);

            for (int i = wMax; i >= 0; i--)
            {
                for (int j = lMax; j >= 0; j--)
                {
                    // Use last empty space as end pos
                    if (maze[i, j] != 0) continue;
                    
                    gameManager.GoalX = i * pathWidth;
                    gameManager.GoalZ = j * pathWidth;
                    return;
                }
            }
        }
    }
}
