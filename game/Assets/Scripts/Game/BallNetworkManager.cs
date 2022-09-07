using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BallNetworkManager : NetworkBehaviour
{
    [SerializeField] public float Friction = 20f;
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] GameObject Ball;
    
    #region Server
    [ServerCallback]
    private void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        Ball.transform.position = new Vector3(0,0,-1); 
    }

    [ServerCallback]
    private void Update() {
        if (rb2D.velocity.magnitude > 0) {
            rb2D.AddForce(-rb2D.velocity * Friction * Time.deltaTime);
        }
    }
    #endregion
}
