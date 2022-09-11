using System;
using Audiences;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace Player
{
    [Serializable]
    public class PlayerSpawnDetails
    {
        public Transform spawnPoint;
        public HealthUI healthUIElement;
        public IntroductionUi introduction;
    }

    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerSpawnDetails[] spawning;
        [SerializeField] private AudienceManager audience;

        private AudienceMember _currentMember;

        private PlayerInputManager _manager;

        private void Awake()
        {
            EndOfMapController.OnPlayerLost += OnPlayerDied;
        }

        private void Start()
        {
            _manager = GetComponent<PlayerInputManager>();
            TryUpdateNextPlayer();
        }

        private void OnDestroy()
        {
            EndOfMapController.OnPlayerLost -= OnPlayerDied;
        }

        private void OnPlayerDied(ScriptableEnum player, PreferredParty party)
        {
            if (audience.GetNonDecidedMember() == null)
            {
                _manager.DisableJoining();
                return;
            }

            _manager.EnableJoining();
        }

        private void OnPlayerJoined(PlayerInput input)
        {
            _currentMember.Use();
            TryUpdateNextPlayer();

            if (_currentMember == null)
            {
                _manager.DisableJoining();
            }


            PlayerSpawnDetails spawnDetails = spawning[input.playerIndex];
            input.transform.position = spawnDetails.spawnPoint.position;

            var b = input.GetComponent<PlayerBehaviour>();
            b.SetParty(input.playerIndex % 2 == 0 ? PreferredParty.Blue : PreferredParty.Red);

            var c = input.GetComponent<CharacterController>();

            spawnDetails.healthUIElement.Init(c.healthSystem);

            if (_manager.maxPlayerCount == input.playerIndex + 1)
            {
                _manager.DisableJoining();
            }

            spawnDetails.introduction.Play(b.Avatar, b.Offset, b.Description);
        }

        private void TryUpdateNextPlayer()
        {
            _currentMember = audience.GetNonDecidedMember();
            if (_currentMember == null)
            {
                return;
            }

            _manager.playerPrefab = _currentMember.PeekPlayerPrefab().gameObject;
        }
    }
}