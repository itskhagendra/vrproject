using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using System;
using UnityEngine.Video;


public class NetworkManager : MonoBehaviour
{
    public VisualTreeAsset CardAsset;
    public UIDocument document;
    private VisualElement root;
    private VisualElement cardContainer;
    public StyleSheet VideoCardStyle;
    public string DataURI;
    private List<APIData> localData = new List<APIData>();
    //public VideoPlayer VideoPlayer;
    //public GameObject CanvasPlayer;
    //private RenderTexture videotexture;
    //public List<APIData> data = new List<APIData>();

    void OnEnable()
    {
        var uiDocument = document.GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;
        cardContainer = root.Q<VisualElement>("cardContainer");
        cardContainer.styleSheets.Add(VideoCardStyle);
        //cardContainer.style.display = DisplayStyle.None;
        document.panelSettings.targetTexture = null;
        //CanvasPlayer.SetActive(false);
        GameManager.Instance.OnDataChanged += getAPIData;
        if(localData.Count==0)
        {
            localData = GameManager.Instance.data;
            getAPIData(localData);
        }
        //StartCoroutine(getData());
    }
    
    void getAPIData(List<APIData> datas)
    {
        Debug.Log("received Datas");
        localData = datas;
        foreach (var x in datas)
        {
            StartCoroutine(getImage(x));
        }
    }
    void createCard(string Label, Texture2D image, string VideoURL, APIData data)
    {
        var CardInstance = CardAsset.CloneTree();
        var Titletext = CardInstance.Q<Label>("title");
        var imageElement = CardInstance.Q<Image>("image");
        var cardButton = CardInstance.Q<Button>("cardButton");

        Titletext.text = Label;
        imageElement.style.backgroundImage = new StyleBackground(image);

        cardButton.clicked += () => SetupPlayer(data);
        cardContainer.Add(CardInstance);
    }

    public IEnumerator getImage(APIData data)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(data.Image))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Texture2D texture2D = new Texture2D(2, 2);
                texture2D.LoadImage(webRequest.downloadHandler.data);
                createCard(data.Name, texture2D, data.Video, data);
            }
        }
    }

    void SetupPlayer(APIData VideoURL)
    {
        GameManager.Instance.SwitchToVideo(VideoURL);
    }   
   
}

[Serializable]
public class APIData
{
    public string Name;
    public string Image;
    public string Video;
    public bool is360;
}
