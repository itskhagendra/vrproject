using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSONNeil;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public class DataRec
    {
        public string imageURL = "";
    }
    [SerializeField] private Image image;
    string url1;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartCoroutine(GetRequest("http://localhost:3000/image"));
        }
    }

    // Update is called once per frame
    IEnumerator GetRequest(string url)
    {
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
                StartCoroutine(LoadImage(dat1[0]));
            }
        }
    }

    IEnumerator LoadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        } else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
            image.sprite = newSprite;
        }
    }
}
