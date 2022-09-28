using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerJoystick : Joystick
{      
    [SerializeField] Image JoystickBack;
    [SerializeField] Image JoystickHandle;
    private PlayerController player;

    [ClientCallback]
    void Update()
    {   
        if (NetworkClient.ready && player == null )
        {
            NetworkClient.localPlayer?.TryGetComponent<PlayerController>(out player);
        }
        if (player != null)
        {
            Vector2 moveDirection = new Vector2(Horizontal, Vertical);
            if (NetworkClient.active) {
                player.Move(moveDirection);    
            }
        }
    }
    
    [ClientCallback]
    public override void OnPointerDown(PointerEventData eventData)
    {JoystickBack.color = new Color32(255,255,255,50);
        
        base.OnPointerDown(eventData);
        JoystickBack.color = new Color32(255,255,255,35);
        JoystickHandle.color = new Color32(255,255,255,35);
    }

    [ClientCallback]
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        JoystickBack.color = new Color32(255,255,255,130);
        JoystickHandle.color = new Color32(255,255,255,170);
    }
}
