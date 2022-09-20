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
    [SerializeField] TextMeshProUGUI LeftTMPro;
    [SerializeField] TextMeshProUGUI RightTMPro;
    [SerializeField] MatchIDText matchIDChange;
    [SerializeField] GameObject Ball;
    [SyncVar(hook = nameof(OnLeftScoreChange))] int LeftScore = 0;
    [SyncVar(hook = nameof(OnRightScoreChange))] int RightScore = 0;
    List<GameObject> players = new List<GameObject>();

    private string MatchID;

    public void OnLeftScoreChange(int oldScore, int newScore)
    {
        LeftTMPro.text = newScore.ToString();
    }

    public void OnRightScoreChange(int oldScore, int newScore)
    {
        RightTMPro.text = newScore.ToString();
    }

    public void RemovePlayer(GameObject playerObject)
    {
        if (players.Count == 1)
        {
            StartCoroutine(ContainerClose(new MatchData { match_id = MatchID }));
        }
        players.Remove(players.Find(x => x == playerObject));
    }

    private IEnumerator ContainerClose(MatchData matchData)
    {
        var postRequest = CreateRequest("http://18.185.12.220:3000/match_over", RequestType.POST, matchData);
        yield return postRequest.SendWebRequest();
    }


    public void AddPlayer(GameObject playerObject)
    {
        players.Add(playerObject);
        if (players.Count == 2) {
            Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            Ball.transform.position = new Vector3(0, 0, -1);
            RightScore = 0;
            LeftScore = 0;
            for (int i = 0; i < players.Count; i++){
                if (i % 2 == 0){
                    players[i].GetComponent<PlayerController>().PlayerColor = new Color32(72,92,192,255);
                    players[i].transform.position = LeftSpawnPoint.position;
                } else {
                    players[i].GetComponent<PlayerController>().PlayerColor = new Color32(192,64,74,255);
                    players[i].transform.position = RightSpawnPoint.position;
                }
            }
        } else {
            if (players.Count % 2 == 1) {
                playerObject.GetComponent<PlayerController>().PlayerColor = new Color32(72,92,192,255);
                playerObject.transform.position = LeftSpawnPoint.position;
            } else {
                playerObject.GetComponent<PlayerController>().PlayerColor = new Color32(192,64,74,255);
                playerObject.transform.position = RightSpawnPoint.position;
            }
        }
    }

    public void IncreaseRight()
    {
        RightScore++;
        restartGame();
    }

    public void IncreaseLeft()
    {
        LeftScore++;
        restartGame();
    }

    private void restartGame()
    {

        for (int i = 0; i < players.Count; i++)
        {
            if (i % 2 == 0)
            {
                players[i].transform.position = LeftSpawnPoint.position;
            }
            else
            {
                players[i].transform.position = RightSpawnPoint.position;
            }
        }

        Ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        Ball.transform.position = new Vector3(0, 0, -1);
    }

    [ServerCallback]
    void Start()
    {
        StartCoroutine(ContainerReady());
    }
    [ServerCallback]
    private IEnumerator ContainerReady()
    {
        var postRequest = CreateRequest("http://18.185.12.220:3000/ready", RequestType.GET, null);
        yield return postRequest.SendWebRequest();
        var res = JsonUtility.FromJson<MatchData>(postRequest.downloadHandler.text);
        MatchID = res.match_id;
        matchIDChange.MatchID = res.match_id;
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
