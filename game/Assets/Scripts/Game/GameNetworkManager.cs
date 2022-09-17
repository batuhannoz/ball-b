using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameNetworkManager : NetworkManager
{
    GameManager gameManager;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        base.OnServerAddPlayer(conn);
        gameManager.AddPlayer(conn.identity.gameObject);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        base.OnServerDisconnect(conn);
        gameManager.RemovePlayer(conn.identity.gameObject);
    }
}
