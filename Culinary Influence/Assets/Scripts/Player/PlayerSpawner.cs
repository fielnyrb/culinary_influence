using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [Serializable]
    public class PlayerSpawnDetails
    {
        public Transform spawnPoint;
        public GameObject[] players;
        public HealthUI healthUIElement;
    }

    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerSpawnDetails[] spawning;

        [SerializeField] private HealthSystem healthSystem;

        private PlayerInputManager _manager;

        private void Awake()
        {
            _manager = GetComponent<PlayerInputManager>();
        }


        private void OnPlayerJoined(PlayerInput input)
        {
            PlayerSpawnDetails spawnDetails = spawning[input.playerIndex];
            input.transform.position = spawnDetails.spawnPoint.position;
            CharacterController c = input.GetComponent<CharacterController>();

            //input.GetComponent<CharacterController>().healthSystem.IncreaseHealth(spawnDetails.healthUIElement, 1);
            spawnDetails.healthUIElement.Init(c.healthSystem);

            if (_manager.maxPlayerCount == input.playerIndex + 1)
            {
                _manager.DisableJoining();
            }

        }

    }
}