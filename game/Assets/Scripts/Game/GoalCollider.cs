using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GoalCollider : MonoBehaviour
{
    GameManager gameManager;
    [ServerCallback]
    private void start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (gameObject.tag)
        {
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
