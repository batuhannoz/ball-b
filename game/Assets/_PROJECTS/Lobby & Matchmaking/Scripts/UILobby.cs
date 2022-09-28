using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Networking;
using System.Text;
using TMPro;

public class UILobby : MonoBehaviour
{
    [SerializeField] GameObject errorMsg;
    [SerializeField] GameObject loadingScreen;
    public class MatchData
    {
        public string match_id;
        public int port;
    }

    public class JoinMatchRequest
    {
        public string match_id;
    }

    public class Hello
    {
        public string message;
    }

    [SerializeField] InputField joinMatchInput;

    public void CancelSearchGame() {
        loadingScreen.SetActive(false);
    }

    public void HostPublic()
    {
        Debug.Log("--- Host Public ---");
        loadingScreen.SetActive(true);
        StartCoroutine(HostPublicRequest());
    }

    private IEnumerator HostPublicRequest()
    {
        
        var postRequest = CreateRequest("http://18.185.12.220:3000/host_public", RequestType.GET, null);
        yield return postRequest.SendWebRequest();
        var res = JsonUtility.FromJson<MatchData>(postRequest.downloadHandler.text);
        if (res == null) {
            yield return new WaitForSeconds(2);
            loadingScreen.SetActive(false);    
            StartCoroutine(SendErrorMessage());
            yield return null;
        }
        Debug.Log(res.match_id);
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<TelepathyTransport>().port = (ushort)res.port;
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().StartClient();
    }

    public void HostPrivate()
    {
        Debug.Log("--- HostPrivate ---");
        loadingScreen.SetActive(true);
        StartCoroutine(HostPrivateRequest());
    }

    private IEnumerator HostPrivateRequest()
    {
        var postRequest = CreateRequest("http://18.185.12.220:3000/host_private", RequestType.GET, null);
        yield return postRequest.SendWebRequest();
        var res = JsonUtility.FromJson<MatchData>(postRequest.downloadHandler.text);
        if (res == null) {
            yield return new WaitForSeconds(2);
            loadingScreen.SetActive(false);    
            StartCoroutine(SendErrorMessage());  

            yield return null;
        }
        Debug.Log(res.match_id);
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<TelepathyTransport>().port = (ushort)res.port;
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().StartClient();
    }

    public void SearchGame()
    {
        Debug.Log("--- SearchGame ---");
        // loadingScreen.SetActive(true);

    }

    private IEnumerator SearchRequest()
    {
        var postRequest = CreateRequest("http://18.185.12.220:3000/search_match", RequestType.POST, null);
        yield return postRequest.SendWebRequest();
        var res = JsonUtility.FromJson<MatchData>(postRequest.downloadHandler.text);
        if (res == null) {
            yield return new WaitForSeconds(2);
            loadingScreen.SetActive(false);  
            StartCoroutine(SendErrorMessage());  
  
            yield return null;
        }
        Debug.Log(res.port);
        Debug.Log(res.match_id);
    }

    public void JoinGame()
    {
        Debug.Log("--- JoinGame ---");
        Debug.Log("---  \"" + joinMatchInput.text + "\" ---");
        loadingScreen.SetActive(true);
        StartCoroutine(JoinRequest(new JoinMatchRequest() { match_id = joinMatchInput.text }));
    }

    private IEnumerator JoinRequest(JoinMatchRequest dataToPost)
    {
        var postRequest = CreateRequest("http://18.185.12.220:3000/join_match", RequestType.POST, dataToPost);
        yield return postRequest.SendWebRequest();
        var res = JsonUtility.FromJson<MatchData>(postRequest.downloadHandler.text);
        if (res == null) {
            yield return new WaitForSeconds(2);
            loadingScreen.SetActive(false);  
            StartCoroutine(SendErrorMessage());  
            yield return null;
        }
        Debug.Log(res.match_id);
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<TelepathyTransport>().port = (ushort)res.port;
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().StartClient();
    }

    // ------------------------------------------------------------------------------------------------------

    IEnumerator SendErrorMessage()
    {
        errorMsg.SetActive(true);
        yield return new WaitForSeconds(3);
        errorMsg.SetActive(false);
    }
    
    private UnityWebRequest CreateRequest(string path, RequestType type = RequestType.GET, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }

    private void AttachHeader(UnityWebRequest request, string key, string value)
    {
        request.SetRequestHeader(key, value);
    }

    public enum RequestType
    {
        GET = 0,
        POST = 1,
        PUT = 2
    }
}
