using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class BackToMenu : MonoBehaviour
{
    [SerializeField] GameObject exitUI;

    public void OnClick() {
        exitUI.SetActive(true);
    }

    public void OnClickYes() {
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().StopClient();
    }

    public void OnClickNo() {
        exitUI.SetActive(false);
    }
}
