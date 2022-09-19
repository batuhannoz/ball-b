using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameNetworkManager : NetworkManager
{
    GameManager gameManager;
    [ServerCallback]
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.AddPlayer(conn.identity.gameObject);
    }
    [ServerCallback]
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.RemovePlayer(conn.identity.gameObject);
        base.OnServerDisconnect(conn);
    }
}
