using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryCWeb : MonoBehaviour
{
#if UNITY_ANDROID
    private static AndroidJavaObject currentActivity;
    private static AndroidJavaObject unityContentView;

    private static AndroidJavaObject webview;
#endif

    private static bool webShowing = false;
    private static bool inited = false;

    private static void initWebView()
    {
#if UNITY_ANDROID
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        unityContentView = getContentView();

        inited = true;
#endif
    }

#if UNITY_ANDROID
    private static AndroidJavaObject getContentView()
    {
        return currentActivity.Call<AndroidJavaObject>("getUnityPlayer");
    }
#endif

    /// <summary>
    /// 定位
    /// </summary>
    /// <param name="text"></param>
    public static void GetLocation(Text text)
    {
#if UNITY_ANDROID
        if (!inited)
        {
            initWebView();
        }
        text.text += currentActivity.Call<string>("GetInfo");
#endif
    }

    /// <summary>
    /// 加载网页
    /// </summary>
    /// <param name="webPath"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void showWebView(string webPath, float width, float height)
    {
#if UNITY_ANDROID
        if (!inited)
        {
            initWebView();
        }

        if (currentActivity == null)
        {
            Debug.LogError("Please init WebView first");
            return;
        }

        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
            webview = new AndroidJavaObject("android.webkit.WebView", currentActivity);

            //AndroidJavaObject layerOutParams = new AndroidJavaObject("android.widget.FrameLayout.LayoutParams", (int)(Screen.width * width), (int)(Screen.height * height));

            webview.Call<AndroidJavaObject>("getSettings").Call("setJavaScriptEnabled", true);
            webview.Call<AndroidJavaObject>("getSettings").Call("setGeolocationEnabled", false);
            webview.Call("setWebViewClient", new AndroidJavaObject("android.webkit.WebViewClient"));
            webview.Call("setWebChromeClient", new AndroidJavaObject("android.webkit.WebChromeClient"));
            if (!webShowing)
            {
                unityContentView.Call("addView", webview, (int)(Screen.width * width), (int)(Screen.height * height));
                webShowing = true;
            }
            webview.Call("loadUrl", webPath.toJavaString());
        }
        ));
#endif
    }

    public static void closeWebView()
    {

        if (webShowing)
        {
#if UNITY_ANDROID
            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                unityContentView.Call("removeView", webview);
                webShowing = false;
            }
            ));
#endif
        }
    }
}
