using Audiences;
using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private ScriptableEnum definition;

        [SerializeField] private string description;
        [SerializeField] private Vector2 offset;
        [SerializeField] private Sprite avatar;
        [SerializeField] private AudioClip catchphrase;

        public ScriptableEnum Definition => definition;
        public PreferredParty Party { get; private set; }

        public string Description => description;
        public Vector2 Offset => offset;
        public Sprite Avatar => avatar;

        public AudioClip CatchPhrase => catchphrase;

        public void SetParty(PreferredParty party)
        {
            Party = party;
        }
    }
}