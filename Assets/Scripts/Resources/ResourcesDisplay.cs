using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText = null;

    private RTSPlayer player;
    private void GetPlayer()
    {
        player = NetworkClient.connection.identity.GetComponent<RTSPlayer>();
    }
    private void Update()
    {
        if (player == null) { GetPlayer(); }
        if(player != null)
        {
            ClientHandleGoldUpdated(player.GetGold());
            player.ClientOnGoldUpdated += ClientHandleGoldUpdated;
        }

    }

    private void ClientHandleGoldUpdated(int gold)
    {
        goldText.text = $"Gold: {gold}";
    }

    private void OnDestroy()
    {
        player.ClientOnGoldUpdated -= ClientHandleGoldUpdated;
    }
}

