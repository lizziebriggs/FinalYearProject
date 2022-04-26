using System.IO;
using UnityEngine;

namespace Mono
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private int health;
        private int width, length;
        private float enemyChance, pickupChance;

        public int Width { set => width = value; }
        public int Length { set => length = value; }
        public float EnemyChance { set => enemyChance = value; }
        public float PickupChance { set => pickupChance = value; }

        private float frameDelta = 0f;
        private float displayVal;
        private const string fileDest = "D:/Year 3/Final Year Project/Benchmark Test/DOD_PlayResults.txt";

        private void Awake()
        {
            instance = this;
        }

        public void UpdateHealth(int value)
        {
            health = value;
        }

        private void Update()
        {
            // Calculate average cumulative framerate
            frameDelta += (Time.deltaTime - frameDelta) * 0.1f;
            displayVal = 1f / frameDelta;
            
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.Escape))
                Application.Quit();
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(10, 10, 100, 25), "Health: " + health);
            GUI.Box(new Rect(10, 50, 150, 25), "FPS: " + displayVal);
        }

        private void OnApplicationQuit()
        {
            StreamWriter writer = new StreamWriter(fileDest, true);
            writer.WriteLine("Maze Size: " + width + " x " + length);
            writer.WriteLine("Enemy Chance: " + enemyChance);
            writer.WriteLine("Pickup Chance: " + pickupChance);
            writer.WriteLine("FPS: " + displayVal + "\n\n");
            writer.Close();
        }
    }
}
