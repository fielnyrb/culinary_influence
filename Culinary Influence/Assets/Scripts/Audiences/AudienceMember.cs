using System;
using Player;
using UnityEngine;
using Utility;

namespace Audiences
{
    public enum PreferredParty
    {
        None,
        Red,
        Blue
    }

    public class AudienceMember : MonoBehaviour
    {
        [SerializeField] private ScriptableEnum memberIdentifier;
        [SerializeField] private PlayerBehaviour playerPrefab;
        [SerializeField] private Sprite blueParty;
        [SerializeField] private Sprite redParty;

        [SerializeField] private SpriteRenderer render;

        private PreferredParty _party = PreferredParty.None;

        public bool HasNoParty => _party == PreferredParty.None;

        public PreferredParty Party => _party;
        
        public ScriptableEnum MemberIdentifier => memberIdentifier;

        public PlayerBehaviour PeekPlayerPrefab()
        {
            return playerPrefab;
        }

        public void Use()
        {
            gameObject.SetActive(false);
        }
        
        private void Awake()
        {
            SetParty(PreferredParty.None);
        }

        public void SetParty(PreferredParty preferredParty)
        {
            _party = preferredParty;
            render.sprite = _party switch
            {
                PreferredParty.Red => redParty,
                PreferredParty.Blue => blueParty,
                PreferredParty.None => null,
                _ => throw new ArgumentOutOfRangeException(nameof(_party), _party, null)
            };
        }
    }
}