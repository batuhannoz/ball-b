using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class ShootButton : MonoBehaviour
{
    [SerializeField] Button button;
    private PlayerController player;
    private void Update() {
        if (NetworkClient.ready && player == null )
        {
            NetworkClient.localPlayer?.TryGetComponent<PlayerController>(out player);
            button.onClick.AddListener(Shoot);
        }
    }

    private void Shoot() {
        if (NetworkClient.active) {
            player.Shoot();
        }
    }
}
