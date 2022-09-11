using Audiences;
using UnityEngine;
using Utility;

namespace Player
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private ScriptableEnum definition;

        private PreferredParty _party;

        public ScriptableEnum Definition => definition;
        public PreferredParty Party => _party;

        public void SetParty(PreferredParty party)
        {
            _party = party;
        }
    }
}