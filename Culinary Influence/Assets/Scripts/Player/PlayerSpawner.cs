using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [Serializable]
    public class PlayerSpawnDetails
    {
        public Transform spawnPoint;
        public GameObject[] players;
    }

    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerSpawnDetails[] spawning;

        private PlayerInputManager _manager;

        private void Awake()
        {
            _manager = GetComponent<PlayerInputManager>();
        }


        private void OnPlayerJoined(PlayerInput input)
        {
            PlayerSpawnDetails spawnDetails = spawning[input.playerIndex];
            input.transform.position = spawnDetails.spawnPoint.position;

            if (_manager.maxPlayerCount == input.playerIndex + 1)
            {
                _manager.DisableJoining();
            }
        }
    }
}