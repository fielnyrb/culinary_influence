using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IntroductionUi : MonoBehaviour
    {
        private static readonly int Introduce = Animator.StringToHash("introduce");
        [SerializeField] private Image render;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource source;

        public void Play(Sprite avatar, Vector2 avatarOffset, string description, AudioClip catchphrase)
        {
            render.sprite = avatar;
            render.transform.localPosition = avatarOffset;

            text.text = description;

            source.clip = catchphrase;
            source.Play();

            animator.SetTrigger(Introduce);
        }

        public void ContinueGame()
        {
            Time.timeScale = 1.0f;
        }
    }
}