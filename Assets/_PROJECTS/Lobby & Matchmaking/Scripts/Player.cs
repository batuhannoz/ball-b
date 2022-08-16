using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MirrorBasics
{

    [RequireComponent(typeof(NetworkMatch))]
    public class Player : NetworkBehaviour
    {

        public static Player localPlayer;
        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;

        NetworkMatch networkMatch;

        [SyncVar] public Match currentMatch;

        [SerializeField] GameObject playerLobbyUI;

        Guid netIDGuid;

        void Awake()
        {
            networkMatch = GetComponent<NetworkMatch>();
        }

        public override void OnStartServer()
        {
            Debug.Log("OnStartServer");
            netIDGuid = netId.ToString().ToGuid();
            networkMatch.matchId = netIDGuid;
        }

        public override void OnStartClient()
        {
            Debug.Log("OnStartClient");
            if (isLocalPlayer)
            {
                localPlayer = this;
            }
            else
            {
                Debug.Log($"Spawning other player UI Prefab");
                playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab(this);
            }
        }

        public override void OnStopClient()
        {
            Debug.Log($"Client Stopped");
            ClientDisconnect();
        }

        public override void OnStopServer()
        {
            Debug.Log($"Client Stopped on Server");
            ServerDisconnect();
        }

        /* 
            HOST MATCH
        */

        public void HostGame(bool publicMatch)
        {
            string matchID = MatchMaker.GetRandomMatchID();
            CmdHostGame(matchID, publicMatch);
        }

        [Command]
        void CmdHostGame(string _matchID, bool publicMatch)
        {
            matchID = _matchID;
            if (MatchMaker.instance.HostGame(_matchID, this, publicMatch, out playerIndex))
            {
                Debug.Log($"<color=green>Game hosted successfully</color>");
                networkMatch.matchId = _matchID.ToGuid();
                TargetHostGame(true, _matchID, playerIndex);
            }
            else
            {
                Debug.Log($"<color=red>Game hosted failed</color>");
                TargetHostGame(false, _matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetHostGame(bool success, string _matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID}");
            UILobby.instance.HostSuccess(success, _matchID);
        }

        /* 
            JOIN MATCH
        */

        public void JoinGame(string _inputID)
        {
            CmdJoinGame(_inputID);
        }

        [Command]
        void CmdJoinGame(string _matchID)
        {
            matchID = _matchID;
            if (MatchMaker.instance.JoinGame(_matchID, this, out playerIndex))
            {
                Debug.Log($"<color=green>Game Joined successfully</color>");
                networkMatch.matchId = _matchID.ToGuid();
                TargetJoinGame(true, _matchID, playerIndex);

                //Host
                if (isServer && playerLobbyUI != null)
                {
                    playerLobbyUI.SetActive(true);
                }
            }
            else
            {
                Debug.Log($"<color=red>Game Joined failed</color>");
                TargetJoinGame(false, _matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string _matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID}");
            UILobby.instance.JoinSuccess(success, _matchID);
        }

        /* 
            DISCONNECT
        */

        public void DisconnectGame()
        {
            CmdDisconnectGame();
        }

        [Command]
        void CmdDisconnectGame()
        {
            ServerDisconnect();
        }

        void ServerDisconnect()
        {
            MatchMaker.instance.PlayerDisconnected(this, matchID);
            RpcDisconnectGame();
            networkMatch.matchId = netIDGuid;
        }

        [ClientRpc]
        void RpcDisconnectGame()
        {
            ClientDisconnect();
        }

        void ClientDisconnect()
        {
            if (playerLobbyUI != null)
            {
                if (!isServer)
                {
                    Destroy(playerLobbyUI);
                }
                else
                {
                    playerLobbyUI.SetActive(false);
                }
            }
        }

        /* 
            SEARCH MATCH
        */

        public void SearchGame()
        {
            CmdSearchGame();
        }

        [Command]
        void CmdSearchGame()
        {
            if (MatchMaker.instance.SearchGame(this, out playerIndex, out matchID))
            {
                Debug.Log($"<color=green>Game Found Successfully</color>");
                networkMatch.matchId = matchID.ToGuid();
                TargetSearchGame(true, matchID, playerIndex);

                //Host
                if (isServer && playerLobbyUI != null)
                {
                    playerLobbyUI.SetActive(true);
                }
            }
            else
            {
                Debug.Log($"<color=red>Game Search Failed</color>");
                TargetSearchGame(false, matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetSearchGame(bool success, string _matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchID: {matchID} == {_matchID} | {success}");
            UILobby.instance.SearchGameSuccess(success, _matchID);
        }

        /* 
            MATCH PLAYERS
        */

        [Server]
        public void PlayerCountUpdated(int playerCount)
        {
            TargetPlayerCountUpdated(playerCount);
        }

        [TargetRpc]
        void TargetPlayerCountUpdated(int playerCount)
        {
            if (playerCount > 1)
            {
                UILobby.instance.SetStartButtonActive(true);
            }
            else
            {
                UILobby.instance.SetStartButtonActive(false);
            }
        }

        /* 
            BEGIN MATCH
        */

        public void BeginGame()
        {
            CmdBeginGame();
        }

        [Command]
        void CmdBeginGame()
        {
            MatchMaker.instance.BeginGame(matchID);
            Debug.Log($"<color=red>Game Beginning</color>");
        }

        public void StartGame()
        { 
            //Server
            TargetBeginGame();
        }

        [TargetRpc]
        void TargetBeginGame()
        {
            Debug.Log($"MatchID: {matchID} | Beginning");
            //Additively load game scene
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
            GameObject.Find("LobbyUI").SetActive(false);
        }
    }
}