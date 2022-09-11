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

        public void Play(Sprite avatar, Vector2 avatarOffset, string description)
        {
            render.sprite = avatar;
            render.transform.localPosition = avatarOffset;

            text.text = description;

            animator.SetTrigger(Introduce);
        }
    }
}