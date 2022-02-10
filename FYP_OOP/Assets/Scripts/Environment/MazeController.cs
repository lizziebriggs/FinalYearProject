using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(MazeGenerator))]
    public class MazeController : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private MazeGenerator mazeGenerator;
        
        [Header("Style")]
        [SerializeField] private Material floorMat;
        [SerializeField] private Material wallMat;

        private void Start()
        {
            if (!mazeGenerator) mazeGenerator = GetComponent<MazeGenerator>();

            mazeGenerator.MazeData = mazeGenerator.GenerateMazeData();
            CreateMazeObject();
            
            SetStartPos(); SetGoalPos();
            gameManager.StartGame();
        }


        private void CreateMazeObject()
        {
            GameObject mazeObj = new GameObject();
            mazeObj.transform.position = Vector3.zero;
            mazeObj.name = "Procedural Maze";

            MeshFilter mf = mazeObj.AddComponent<MeshFilter>();
            mf.mesh = mazeGenerator.GenerateMazeMesh();

            MeshCollider mc = mazeObj.AddComponent<MeshCollider>();
            mc.sharedMesh = mf.mesh;

            MeshRenderer mr = mazeObj.AddComponent<MeshRenderer>();
            mr.materials = new Material[2] {floorMat, wallMat};
        }

        private void SetStartPos()
        {
            int[,] maze = mazeGenerator.MazeData;

            int wMax = maze.GetUpperBound(0);
            int lMax = maze.GetUpperBound(1);

            for (int i = 0; i <= wMax; i++)
            {
                for (int j = 0; j <= lMax; j++)
                {
                    // Use first empty space as start pos
                    if (maze[i, j] != 0) continue;

                    gameManager.StartX = i * mazeGenerator.PathWidth;
                    gameManager.StartZ = j * mazeGenerator.PathWidth;
                    return;
                }
            }
        }

        private void SetGoalPos()
        {
            int[,] maze = mazeGenerator.MazeData;

            int wMax = maze.GetUpperBound(0);
            int lMax = maze.GetUpperBound(1);

            for (int i = wMax; i >= 0; i--)
            {
                for (int j = lMax; j >= 0; j--)
                {
                    // Use last empty space as end pos
                    if (maze[i, j] != 0) continue;
                    
                    gameManager.GoalX = i * mazeGenerator.PathWidth;
                    gameManager.GoalZ = j * mazeGenerator.PathWidth;
                    return;
                }
            }
        }
    }
}
