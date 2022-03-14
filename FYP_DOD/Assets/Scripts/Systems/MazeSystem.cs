using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace Systems
{
    public class MazeSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!TryGetSingleton(out Maze maze)) return;
            
            int width = maze.width;
            int length = maze.length;
            float pathWidth = maze.pathWidth;
            float pathHeight = maze.pathHeight;
                
            int[,] mazeData = GenerateMazeData(width, length);
            
            int wMax = mazeData.GetUpperBound(0);
            int lMax = mazeData.GetUpperBound(1);
            float halfH = pathHeight * .5f;
            
            for (int i = 0; i <= wMax; i++)
            {
                for (int j = 0; j <= lMax; j++)
                {
                    if (mazeData[i, j] == 1) continue;

                    float3 floorPos = new float3(j * pathWidth, 0, i * pathWidth);

                    Entity floor = EntityManager.Instantiate(maze.floorQuad);
                    EntityManager.SetComponentData(floor, new Translation() {Value = floorPos});

                    float chance = Random.Range(0f, 1f);

                    if (chance <= maze.enemyChance)
                    {
                        Entity enemy = EntityManager.Instantiate(maze.enemyPrefab);
                        EntityManager.SetComponentData(enemy, 
                            new Translation() {Value = new float3(floorPos.x, floorPos.y + 1f, floorPos.z)});
                    }

                    if (chance <= maze.pickupChance)
                    {
                        int type = Random.Range(0, 1);
                        Entity pickup = EntityManager.Instantiate(
                            (type == 0 ? maze.speedPickupPrefab : maze.immunityPickupPrefab));
                        
                        EntityManager.SetComponentData(pickup, 
                            new Translation() {Value = new float3(floorPos.x, floorPos.y + 1f, floorPos.z)});
                    }
                    
                    // Face forward
                    if (i - 1 < 0 || mazeData[i-1, j] == 1)
                    {
                        Entity wall = EntityManager.Instantiate(maze.wallQuad);
                        EntityManager.SetComponentData(wall, new Translation() {Value = new float3(j * pathWidth, halfH, (i-.5f) * pathWidth)});
                        EntityManager.SetComponentData(wall, new Rotation(){Value = quaternion.EulerZXY(0, math.PI / 180 * 180, 0)});
                    }

                    // Face left
                    if (j + 1 > lMax || mazeData[i, j+1] == 1)
                    {
                        Entity wall = EntityManager.Instantiate(maze.wallQuad);
                        EntityManager.SetComponentData(wall, new Translation() {Value = new float3((j+.5f) * pathWidth, halfH, i * pathWidth)});
                        EntityManager.SetComponentData(wall, new Rotation(){Value = quaternion.EulerZXY(0, math.PI / 180 * 90, 0)});
                    }

                    // Face right
                    if (j - 1 < 0 || mazeData[i, j-1] == 1)
                    {
                        Entity wall = EntityManager.Instantiate(maze.wallQuad);
                        EntityManager.SetComponentData(wall, new Translation() {Value = new float3((j-.5f) * pathWidth, halfH, i * pathWidth)});
                        EntityManager.SetComponentData(wall, new Rotation(){Value = quaternion.EulerZXY(0, math.PI / 180 * 270, 0)});
                    }

                    // Face back
                    if (i + 1 > wMax || mazeData[i+1, j] == 1)
                    {
                        Entity wall = EntityManager.Instantiate(maze.wallQuad);
                        EntityManager.SetComponentData(wall, new Translation() {Value = new float3(j * pathWidth, halfH, (i+.5f) * pathWidth)});
                        EntityManager.SetComponentData(wall, new Rotation(){Value = quaternion.EulerZXY(0, math.PI / 180 * 0, 0)});
                    }
                }
            }

            Entities.ForEach((ref Maze mazeComp) =>
            {
                // Set start position
                for (int i = 0; i <= wMax; i++)
                {
                    for (int j = 0; j <= lMax; j++)
                    {
                        // Use first empty space as start pos
                        if (mazeData[i, j] != 0) continue;
            
                        mazeComp.startX = i * pathWidth;
                        mazeComp.startY = j * pathWidth;
                        return;
                    }
                }
            
                // Set end position
                for (int i = wMax; i >= 0; i--)
                {
                    for (int j = lMax; j >= 0; j--)
                    {
                        // Use first empty space as start pos
                        if (mazeData[i, j] != 0) continue;
            
                        mazeComp.endX = i * pathWidth;
                        mazeComp.endY = j * pathWidth;
                        return;
                    }
                }
            }).WithoutBurst().Run();
            
            Enabled = false;
        }

        private static int[,] GenerateMazeData(int width, int length)
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
                    
                    else if (i % 2 == 0 && j % 2 == 0)
                    {
                        newMaze[i, j] = 1;

                        // Choose random neighbour to be a wall
                        int a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                        int b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                        newMaze[i+a, j+b] = 1;
                    }
                }
            }

            return newMaze;
        }
    }
}
