using System;
using Audiences;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        private static readonly int GameOver = Animator.StringToHash("GameOver");

        [SerializeField] private Animator animator;
        [SerializeField] private Image badge;

        [SerializeField] private Sprite blueBadge;
        [SerializeField] private Sprite redBadge;


        public void ShowGameOverScreen(PreferredParty party)
        {
            badge.sprite = party switch
            {
                PreferredParty.Blue => blueBadge,
                PreferredParty.Red => redBadge,
                _ => throw new ArgumentOutOfRangeException(nameof(party), party, null)
            };

            animator.SetTrigger(GameOver);
        }

        public void Restart()
        {
            SceneManager.LoadScene("Main");
        }

        public void GoToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}