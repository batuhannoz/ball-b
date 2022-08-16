using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Turnbased {
    public class TurnManager : MonoBehaviour {

        [SerializeField] List<Player> players;
        [SerializeField] Player currentPlayer;
        [SerializeField] int currentPlayerIndex;
        [SerializeField] bool gameInProgress = true;

        public void ManagePlayers (List<Player> _players) {
            players = _players;
            StartCoroutine (TrackTurns ());
        }

        IEnumerator TrackTurns () {
            currentPlayerIndex = Random.Range (0, players.Count);
            currentPlayer = players[currentPlayerIndex];

            while (gameInProgress) {
                currentPlayer.SetTurn (true);
                for (var i = 0; i < players.Count; i++) {
                    if (players[i] != currentPlayer) players[i].SetTurn (false);
                }

                yield return new WaitForSeconds (10);
                NextPlayer ();
            }
        }

        void NextPlayer () {
            currentPlayerIndex++;
            if (currentPlayerIndex >= players.Count) {
                currentPlayerIndex = 0;
            }

            currentPlayer = players[currentPlayerIndex];
        }

    }
}