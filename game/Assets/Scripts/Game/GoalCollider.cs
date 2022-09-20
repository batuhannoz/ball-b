using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GoalCollider : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            switch (gameObject.tag)
            {
                case "LeftGoal":
                    gameManager.IncreaseRight();
                    break;
                case "RightGoal":
                    gameManager.IncreaseLeft();
                    break;
            }
        }
    }
}

