using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BallNetworkManager : NetworkBehaviour
{
    [SerializeField] public float Friction = 22f;
    [SerializeField] float MaxSpeed = 4.5f;
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] GameObject Ball;
    
    #region Server
    [Server] 
    public override void OnStartServer()
    {
        base.OnStartServer();
        rb2D.simulated = true;
        Ball.transform.position = new Vector3(0,0,-1); 
    }

    [Server]
    private void Start() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    [Server]
    private void Update() {
        if (rb2D.velocity.magnitude > 0) {
            rb2D.AddForce(-rb2D.velocity * Friction * Time.deltaTime);
        }
        if (rb2D.velocity.magnitude > MaxSpeed) {
            rb2D.velocity = Vector2.ClampMagnitude(rb2D.velocity, MaxSpeed);
        }
    }
    #endregion
}
