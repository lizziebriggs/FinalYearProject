using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Environment
{
    public class MazeGenerator : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int width;
        [SerializeField] private int length;
        [SerializeField] private float pathWidth;
        [SerializeField] private float pathHeight;

        public int[,] MazeData { get; set; }

        public float PathWidth => pathWidth;
        public float PathHeight => pathHeight;


        public int[,] GenerateMazeData()
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


        public Mesh GenerateMazeMesh()
        {
            Mesh newMaze = new Mesh();
            
            List<Vector3> newVerts = new List<Vector3>();
            List<Vector2> newUVs = new List<Vector2>();

            newMaze.subMeshCount = 2;
            List<int> floorTriangles = new List<int>();
            List<int> wallTriangles = new List<int>();

            int wMax = MazeData.GetUpperBound(0);
            int lMax = MazeData.GetUpperBound(1);
            float halfH = pathHeight * .5f;

            for (int i = 0; i <= wMax; i++)
            {
                for (int j = 0; j <= lMax; j++)
                {
                    if (MazeData[i, j] == 1) continue;
                    
                    // Create floors
                    CreateQuad(Matrix4x4.TRS(
                        new Vector3(j * pathWidth, 0, i *pathWidth),
                        Quaternion.LookRotation(Vector3.up),
                        new Vector3(pathWidth, pathWidth, 1)
                    ), ref newVerts, ref newUVs, ref floorTriangles );
                        
                    // Create ceiling
                    // CreateQuad(Matrix4x4.TRS(
                    //     new Vector3(j * pathWidth, pathHeight, i * pathWidth),
                    //     Quaternion.LookRotation(Vector3.down),
                    //     new Vector3(pathWidth, pathWidth, 1)
                    // ), ref newVerts, ref newUVs, ref floorTriangles);

                    if (i - 1 < 0 || MazeData[i-1, j] == 1)
                    {
                        CreateQuad(Matrix4x4.TRS(
                            new Vector3(j * pathWidth, halfH, (i-.5f) * pathWidth),
                            Quaternion.LookRotation(Vector3.forward),
                            new Vector3(pathWidth, pathHeight, 1)
                        ), ref newVerts, ref newUVs, ref wallTriangles);
                    }

                    if (j + 1 > lMax || MazeData[i, j+1] == 1)
                    {
                        CreateQuad(Matrix4x4.TRS(
                            new Vector3((j+.5f) * pathWidth, halfH, i * pathWidth),
                            Quaternion.LookRotation(Vector3.left),
                            new Vector3(pathWidth, pathHeight, 1)
                        ), ref newVerts, ref newUVs, ref wallTriangles);
                    }

                    if (j - 1 < 0 || MazeData[i, j-1] == 1)
                    {
                        CreateQuad(Matrix4x4.TRS(
                            new Vector3((j-.5f) * pathWidth, halfH, i * pathWidth),
                            Quaternion.LookRotation(Vector3.right),
                            new Vector3(pathWidth, pathHeight, 1)
                        ), ref newVerts, ref newUVs, ref wallTriangles);
                    }

                    if (i + 1 > wMax || MazeData[i+1, j] == 1)
                    {
                        CreateQuad(Matrix4x4.TRS(
                            new Vector3(j * pathWidth, halfH, (i+.5f) * pathWidth),
                            Quaternion.LookRotation(Vector3.back),
                            new Vector3(pathWidth, pathHeight, 1)
                        ), ref newVerts, ref newUVs, ref wallTriangles);
                    }
                }
            }

            newMaze.vertices = newVerts.ToArray();
            newMaze.uv = newUVs.ToArray();
    
            newMaze.SetTriangles(floorTriangles.ToArray(), 0);
            newMaze.SetTriangles(wallTriangles.ToArray(), 1);

            newMaze.RecalculateNormals();

            return newMaze;
        }


        private static void CreateQuad(Matrix4x4 matrix, ref List<Vector3> newVerts, ref List<Vector2> newUVs,
            ref List<int> newTriangles)
        {
            int index = newVerts.Count;
            
            Vector3 vert1 = new Vector3(-.5f, -.5f, 0);
            Vector3 vert2 = new Vector3(-.5f, .5f, 0);
            Vector3 vert3 = new Vector3(.5f, .5f, 0);
            Vector3 vert4 = new Vector3(.5f, -.5f, 0);
            
            newVerts.Add(matrix.MultiplyPoint3x4(vert1));
            newVerts.Add(matrix.MultiplyPoint3x4(vert2));
            newVerts.Add(matrix.MultiplyPoint3x4(vert3));
            newVerts.Add(matrix.MultiplyPoint3x4(vert4));
            
            newUVs.Add(new Vector2(1, 0));
            newUVs.Add(new Vector2(1, 1));
            newUVs.Add(new Vector2(0, 1));
            newUVs.Add(new Vector2(0, 0));
            
            newTriangles.Add(index+2);
            newTriangles.Add(index+1);
            newTriangles.Add(index);
            
            newTriangles.Add(index+3);
            newTriangles.Add(index+2);
            newTriangles.Add(index);
        }
        
        
        // private void OnGUI()
        // {
        //     int[,] mazeToDraw = this.mazeData;
        //     int rMax = mazeToDraw.GetUpperBound(0);
        //     int cMax = mazeToDraw.GetUpperBound(1);
        //
        //     string msg = "";
        //
        //     for (int i = rMax; i >= 0; i--)
        //     {
        //         for (int j = 0; j <= cMax; j++)
        //         {
        //             if (mazeToDraw[i, j] == 0) msg += "....";
        //             else msg += "==";
        //         }
        //         msg += "\n";
        //     }
        //
        //     GUI.Label(new Rect(20, 20, 500, 500), msg);
        // }
    }
}
