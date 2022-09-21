using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SyncVar (hook = nameof(OnColorChange))] public Color32 PlayerColor;
    [SerializeField] AudioSource ShootAudioSource;
    [SerializeField] AudioClip ShootAudioClip;
    [SerializeField] GameObject PlayerShootCircle;
    [SerializeField] float MovementSpeed = 16f;
    [SerializeField] float Friction = 300f;
    [SerializeField] float MaxSpeed = 2.7f;
    private Vector2 MoveDirection;
    private FixedJoystick joystick;
    private PlayerShoot PlayerShoot;
    private Rigidbody2D rb;

    #region Server
    [ServerCallback]
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerShoot = PlayerShootCircle.GetComponent<PlayerShoot>();
    }

    [Command]
    private void CmdShoot()
    {
        PlayerShoot.Shoot();
    }
    
    [ClientRpc]
    public void RpcPlayAudio() {
        ShootAudioSource.PlayOneShot(ShootAudioClip);
    }

    [Command]
    private void CmdMove(Vector2 NewMoveDirection)
    {
        MoveDirection = NewMoveDirection;
    }

    [ServerCallback]
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
    [ClientCallback]
    public void Move(Vector2 NewMoveDirection)
    {
        CmdMove(NewMoveDirection);
    }

    [ClientCallback]
    public void Shoot()
    {
        CmdShoot();
    }
    #endregion

    public void OnColorChange(Color32 oldColor, Color32 newColor) {
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }
}