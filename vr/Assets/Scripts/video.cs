using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class video : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    void Start()
    {
        VideoPlayer.source = VideoSource.Url;
        VideoPlayer.url = GameManager.Instance?.PlayVideo.Video;
    }

    public void BackToMainScreen()
    {
        SceneManager.LoadScene("Menu");
    }
}
