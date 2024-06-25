using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using System;

public class NetworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    public VisualTreeAsset CardAsset;
    public UIDocument document;
    private VisualElement root;
    public StyleSheet VideoCardStyle;
    public string DataURI;
    public List<APIData> data = new List<APIData>();

    void OnEnable()
    {
        var uiDocument = document.GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        root.styleSheets.Add(VideoCardStyle);
        StartCoroutine("getData");
    }


    void createCard(string Label, Texture2D image, string VideoURL,string imageURL)
    {
        var CardInstance = CardAsset.CloneTree();
        var Titletext = CardInstance.Q<Label>("title");
        var imageElement = CardInstance.Q<Image>("image");
        var cardVisualElement = CardInstance.Q<VisualElement>("card");
        var cardButton = CardInstance.Q<Button>("cardButton");
        Titletext.text = Label;
        imageElement.style.backgroundImage = new StyleBackground(image);
        //imageElement.style.backgroundImage = url(imageURL);
        root.Add(CardInstance);

         cardButton.clicked += () => Debug.Log("Card clicked: " + VideoURL);

    }

    IEnumerator getData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(DataURI))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(webRequest.result.ToString());
            }
            else
            {
                string resposne = webRequest.downloadHandler.text;
                data = JsonConvert.DeserializeObject<List<APIData>>(resposne);
                //StartCoroutine(data.getImage());
                foreach(var x in data)
                {
                    StartCoroutine(getImage(x));
                }
            }
        }
    }

    public IEnumerator getImage(APIData data)
    {
        using(UnityWebRequest webRequest = UnityWebRequest.Get(data.Image))
        {
            yield return webRequest.SendWebRequest();
            if(webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(webRequest.result.ToString());
            }
            else{
                Debug.Log(webRequest.downloadHandler.data);
                Texture2D texture2D = new Texture2D(2,2);
                //Texture Texture = DownloadHandlerTexture.GetContent(webRequest);
                texture2D.LoadImage(webRequest.downloadHandler.data);
                createCard(data.Name,texture2D,data.Video,data.Image);
            }
        }
    }
}


[Serializable]
public class APIData
{
    public string Name;
    public string Image;
    public string Video;

}
