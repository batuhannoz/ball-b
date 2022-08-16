using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics {

    public class UIPlayer : MonoBehaviour {

        [SerializeField] Text text;
        Player player;

        public void SetPlayer (Player player) {
            this.player = player;
            text.text = "Player " + player.playerIndex.ToString ();
        }

    }
}