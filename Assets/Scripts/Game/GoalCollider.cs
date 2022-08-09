using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GoalCollider : MonoBehaviour
{
    [Server] 
    private void OnTriggerEnter2D(Collider2D other) {
        switch(gameObject.tag) {
            case "LeftGoal":
                // +1 point right
                // restart game logic 
                break;
            case "RightGoal":
                // +1 point left
                // restart game logic
                break;    
        }
    }
}
