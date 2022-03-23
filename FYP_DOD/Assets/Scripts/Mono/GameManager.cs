using UnityEngine;

namespace Mono
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        private int health;

        private void Awake()
        {
            instance = this;
        }

        public void UpdateHealth(int value)
        {
            health = value;
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(10, 10, 100, 25), "Health: " + health);
        }
    }
}
