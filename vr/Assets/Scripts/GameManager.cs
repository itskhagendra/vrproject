using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using System;
using UnityEngine.Video;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance; 
    
    public string DataURI;
    public APIData PlayVideo;
    [SerializeField]
    private List<APIData> _data = new List<APIData>();
    public event Action<List<APIData>> OnDataChanged;
    private string VideoScene = "VideoScene";
    
    private String Json = @"
[
    {
        ""Name"": ""Big Buck Bunny"",
        ""Image"": ""https://img.freepik.com/free-photo/majestic-mountain-peak-tranquil-winter-landscape-generated-by-ai_188544-15662.jpg"",
        ""Video"": ""http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"",
        ""is360"": false
    },
    {
        ""Name"": ""Elephant Dream"",
        ""Image"": ""https://t4.ftcdn.net/jpg/05/68/06/93/360_F_568069372_X1vDTdjtvKbVLOEegIPZZrIorrHlQc7R.jpg"",
        ""Video"": ""http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4"",
        ""is360"": false
    },
    {
        ""Name"": ""Bigger Blazes"",
        ""Image"": ""https://img.freepik.com/free-photo/cool-geometric-triangular-figure-neon-laser-light-great-backgrounds-wallpapers_181624-9331.jpg"",
        ""Video"": ""http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4"",
        ""is360"": false
    }
]
";


    public List<APIData> data { get { return _data; }
        set
        {
            _data = value;
            OnDataChanged?.Invoke(_data); // Raise event when list changes
        }}
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // If another instance exists, destroy this one
        }
        StartCoroutine("getData");
    }
    IEnumerator getData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(DataURI))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
                data = JsonConvert.DeserializeObject<List<APIData>>(Json);
            }
            else
            {
                string response = webRequest.downloadHandler.text;
                data = JsonConvert.DeserializeObject<List<APIData>>(response);
            }
        }
    }

    public void SwitchToVideo(APIData VideoData)
    {
        SceneManager.LoadScene(VideoScene);
        PlayVideo = VideoData;
    }

    // Event handler for data changes
}
