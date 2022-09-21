using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] public float ShootSpeed = 5f; 
    private Collider2D BallCollider;
    private Rigidbody2D BallRb2D;
    private PlayerShoot ShootButton;
    Vector2 ForcePosition;
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Ball") {
            BallCollider = other;
            BallRb2D = other.GetComponent<Rigidbody2D>();  
        }
    }
    [ServerCallback]
    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Ball") {
            BallCollider = null;
            BallRb2D = null;
        }
    }
    [ServerCallback]
    public void Shoot() {
        if (BallCollider != null && BallRb2D != null) {
            ForcePosition = new Vector2(BallRb2D.position.x - transform.position.x, BallRb2D.position.y - transform.position.y); 
            BallRb2D.AddForce(ForcePosition * ShootSpeed, ForceMode2D.Impulse);
            player.RpcPlayAudio();
        }
    }
}
