using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;

public class PingDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI PingTMPro;
    private double ping;
    [ClientCallback]
    private void Update() {
        if (ping == Math.Round(NetworkTime.rtt * 1000)) return;
        ping = Math.Round(NetworkTime.rtt * 1000);
        
        PingTMPro.SetText($"{ping} ms");
        if (ping <= 65) {
            PingTMPro.color = Color.green;
        } else if (ping <= 100) {
            PingTMPro.color = Color.yellow;
        } else {
            PingTMPro.color = Color.red;
        }
    }
}
