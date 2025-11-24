using UI;
using UnityEngine;

namespace Core
{
    public class GameController : MonoBehaviour
    {

        GameController instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        [SerializeField] private int moves;
        [SerializeField] private int currentMoves;
        [SerializeField] private int score;
        [SerializeField] private int currentScore;

        [SerializeField] private UIController uiController; 
        void Start()
        {
            StartGame();
        }



        public void AddMove()
        {
            uiController.UpdateMoves(--currentMoves);
            if(currentMoves <= 0)
            {
                uiController.ShowGameOver();
            }
        }

        public void AddScore(int points)
        {
            currentScore += points;
            uiController.UpdateScore(currentScore);
        }

        public void StartGame()
        {
            currentMoves = moves;
            currentScore = score;
            uiController.UpdateMoves(currentMoves);
            uiController.UpdateScore(currentScore);
            uiController.HideGameOver();
        }

        public void RestartGame()
        {
            StartGame();
        }

        public void TestButton()
        {
            AddMove();            
            AddScore(10);
        }
    }
}
