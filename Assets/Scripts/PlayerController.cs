using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] GameObject PlayerShootCircle;
    [SerializeField] float MovementSpeed = 16f;
    [SerializeField] float Friction = 300f;
    [SerializeField] float MaxSpeed = 2.7f;
    private Vector2 MoveDirection;
    private FixedJoystick joystick;
    private PlayerShoot PlayerShoot;
    private Rigidbody2D rb;

    #region Server
    [Server]
    private void Awake()
    {  
        rb = GetComponent<Rigidbody2D>();
        PlayerShoot = PlayerShootCircle.GetComponent<PlayerShoot>();
    }

    [Command]
    public void CmdShoot()
    {
        PlayerShoot.Shoot();
    }

    [Command]
    private void CmdMove(Vector2 NewMoveDirection)
    {
        MoveDirection = NewMoveDirection;
    }

    [Server]
    private void Update()
    {
        rb.AddRelativeForce(MoveDirection * Time.deltaTime * MovementSpeed, ForceMode2D.Impulse);
        if (rb.velocity.magnitude > 0)
        {
            rb.AddForce(-rb.velocity * Friction * Time.deltaTime);
        }
        if (rb.velocity.magnitude > MaxSpeed)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);
        }
    }
    #endregion

    #region Client
    [Client]
    public void Move(Vector2 NewMoveDirection) {
        CmdMove(NewMoveDirection);
    }

    [Client]
    public void Shoot() {
        CmdShoot();
    }
    #endregion
}     