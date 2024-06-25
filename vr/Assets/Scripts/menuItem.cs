using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSONNeil;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class menuItem : MonoBehaviour, IPointerClickHandler 
{
    // Start is called before the first frame update
    public class DataRec
    {
        public string imageURL = "";
    }
    [SerializeField] private Image image;
    string url1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetRequest("http://localhost:8000/image"));
    }

    private void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked");
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
        }
        else
        {
            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite newSprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
            Debug.Log(image);
            image.sprite = newSprite;
        }
    }
}
