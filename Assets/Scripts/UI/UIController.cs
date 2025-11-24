using TMPro;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {

        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text movesText;
        [SerializeField] private GameObject GameOverPanel;

        void Start()
        {
            GameOverPanel.SetActive(false);
            UpdateScore(0);
            UpdateMoves(5);
        }

        public void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void UpdateMoves(int moves)
        {
            movesText.text = moves.ToString();
        }

        public void ShowGameOver()
        {
            GameOverPanel.SetActive(true);
        }

        public void HideGameOver()
        {
            GameOverPanel.SetActive(false);
        }

        
    }
}