using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebViewTest : MonoBehaviour
{
    public Button showWebButton;
    public Button hideWebButton;
    public Button showMapButton;
    public Button BuildButton;

    [SerializeField]
    GameObject ScroView;

    [SerializeField]
    Text text;

    // Use this for initialization
    void Start()
    {
        if (showWebButton != null)
        {
            showWebButton.onClick.AddListener(delegate
            {
                //Visit local web file located on /Assets/StreamingAssets/
                MemoryCWeb.showWebView("file:///android_asset/temp.html",1f,0.9f);

                //Visit web by url
                //MemoryCWeb.showWebView("http://m.baidu.com", 1f, 0.9f);
            });
        }

        if (hideWebButton != null)
        {
            hideWebButton.onClick.AddListener(delegate
            {
                BuildCtrl.Instance.DestoryBuild();
                MemoryCWeb.closeWebView();
                ScroView.SetActive(false);
                text.text = string.Empty;
            });
        }

        if (showMapButton != null)
        {
            showMapButton.onClick.AddListener(delegate
            {
                if(text!=null)
                {
                    MemoryCWeb.GetLocation(text);
                    ScroView.SetActive(true);
                }
                    
            });
        }

        if (BuildButton != null)
        {
            BuildButton.onClick.AddListener(delegate
            {
                BuildCtrl.Instance.MakeBuild();
            });
        }
    }
}
