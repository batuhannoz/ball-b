using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CornerCollision : MonoBehaviour
{
    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(other.collider, other.otherCollider);
        }
    }
}
