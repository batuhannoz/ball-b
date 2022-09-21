using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MatchIDText : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnMatchIDChange))]
    public string MatchID;

    [SerializeField] TextMeshProUGUI MatchID_UI;
    
    [ClientCallback]
    void OnMatchIDChange(string oldID, string newID) {
        MatchID_UI.SetText(newID);
    } 
}
