using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSONNeil;
using UnityEngine.UI;

public class video : MonoBehaviour
{
    UnityEngine.Video.VideoPlayer videoPlayer;
    bool paused;
    public class DataRec
    {
        public string imageURL = "";
    }
    [SerializeField] private Image image;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("http://localhost:8000/video"));
        Button btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(onPlayButtonClick);

    }

    public void onPlayButtonClick()
    {
        if (paused)
        {
            videoPlayer.Play();
            paused = false;
        }
        else
        {
            Debug.Log("hello");
            videoPlayer.Pause();
            paused = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    if (paused)
        //    {
        //        videoPlayer.Play();
        //        paused = false;
        //    }
        //    else
        //    {
        //        videoPlayer.Pause();
        //        paused = true;
        //    }
        //}
        
    }

    IEnumerator GetRequest(string url)
    {
        Debug.Log("hello");
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                SimpleJSONNeil.JSONNode dat1 = SimpleJSONNeil.JSON.Parse(json);
                LoadVideo(dat1[0]);
            }
        }
    }

    void LoadVideo(string url)
    {
        //UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        //yield return request.SendWebRequest();
        //if (request.result == UnityWebRequest.Result.ConnectionError)
        //{
        //    Debug.Log(request.error);
        //}
        //else
        //{
        //    Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        //    Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
        //    image.sprite = newSprite;
        //}
        Debug.Log(url);
        GameObject camera = GameObject.Find("Main Camera");

        videoPlayer = camera.AddComponent<UnityEngine.Video.VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

        videoPlayer.url = url;

        videoPlayer.isLooping = true;

        videoPlayer.Play();
    }
}
