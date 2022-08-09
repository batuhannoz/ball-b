using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class ShootButton : MonoBehaviour
{
    [SerializeField] Button button;
    private PlayerController player;
    private void Start() {
        button.onClick.AddListener(Shoot);
    }
    private void Update() {
        if (NetworkClient.active && player == null)
        {
            NetworkClient.localPlayer?.TryGetComponent<PlayerController>(out player);
        }
    }
    public void Shoot() {
        if (NetworkClient.ready) {
            player.Shoot();
        }
    }
}
