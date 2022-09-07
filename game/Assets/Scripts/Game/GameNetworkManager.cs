using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameNetworkManager : NetworkManager
{
    GameManager gameManager;
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
}
