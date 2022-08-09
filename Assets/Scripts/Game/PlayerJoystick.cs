using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerJoystick : MonoBehaviour
{
    [SerializeField] FixedJoystick joystick;
    private PlayerController player;
    void Update()
    {  
        if (NetworkClient.ready && player == null )
        {
            NetworkClient.localPlayer?.TryGetComponent<PlayerController>(out player);
        }
        if (player != null)
        {
            Vector2 moveDirection = new Vector2(joystick.Horizontal, joystick.Vertical);
            if (NetworkClient.active) {
                player.Move(moveDirection);    
            }
        }
    }
}
