using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class MatchIDText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI MatchID_UI;

    public string MatchID
    {
        get { return _MatchID; }
        set
        {
            _MatchID = value;
            MatchID_UI.SetText("Match ID: "+value);

        }
    }
    private string _MatchID;
}