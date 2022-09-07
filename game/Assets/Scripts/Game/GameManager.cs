using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Networking;
using System.Text;
using TMPro;

public class GameManager : NetworkBehaviour
{

    public class MatchData
    {
        public string match_id;
    }
    [SerializeField] Transform LeftSpawnPoint;
    [SerializeField] Transform RightSpawnPoint;
    [SerializeField] TextMeshProUGUI LeftScore;
    [SerializeField] TextMeshProUGUI RightScore;
    [SerializeField] GameObject Ball;
    List<GameObject> players = new List<GameObject>();
    MatchIDText MatchIDText;
    private string MatchID;

    public void AddPlayer(GameObject playerObject)
    {
        players.Add(playerObject);
    }

    public void IncreaseRight()
    {
        int NewPoint = int.Parse(RightScore.text);
        NewPoint++;
        RightScore.text = NewPoint.ToString();
        restartGame();
    }

    public void IncreaseLeft()
    {
        int NewPoint = int.Parse(LeftScore.text);
        NewPoint++;
        LeftScore.text = NewPoint.ToString();
        restartGame();
    }

    private void restartGame()
    {
        for (int i = 0; i < players.Count; i++) {
            if (i % 2 == 0) {
                
            } 
        }
        Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Ball.transform.position = new Vector3(0,0,-1);
    }

    [ServerCallback]
    void Start()
    {
        StartCoroutine(ContainerReady());
    }
    [ServerCallback]
    private IEnumerator ContainerReady()
    {
        var postRequest = CreateRequest("http://52.29.249.251:3000/ready", RequestType.GET, null);
        yield return postRequest.SendWebRequest();
        var res = JsonUtility.FromJson<MatchData>(postRequest.downloadHandler.text);
        MatchID = res.match_id;
        MatchIDText.MatchID = res.match_id;
        GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>().StartClient();
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
