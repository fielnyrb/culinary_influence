using System;
using Audiences;
using Player;
using UnityEngine;
using Utility;

public class EndOfMapController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerBehaviour = collision.transform.parent.GetComponent<PlayerBehaviour>();
        if (!playerBehaviour)
        {
            return;
        }

        OnPlayerLost?.Invoke(playerBehaviour.Definition, playerBehaviour.Party);
        Destroy(playerBehaviour.gameObject);
    }

    public static event Action<ScriptableEnum, PreferredParty> OnPlayerLost;
}