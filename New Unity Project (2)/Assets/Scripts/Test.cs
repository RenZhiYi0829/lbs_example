using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    [SerializeField]
    Button destoryBtn;
    [SerializeField]
    Button createBtn;
    string txt = "CA：";

    [SerializeField]
    Test2 test2;

    private void Awake()
    {
        test2 = GetComponent<Test2>();
    }
    // Start is called before the first frame update
    void Start()
    {
        createBtn.onClick.AddListener(test2.OnBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        //if(GUI.Button(new Rect(0,520,300,100), txt))
        //{
        //    CallAndroid();
        //}
    }

    public void PrintText(string str)
    {
        txt += str;
    }
    //unity访问非静态
    public void CallAndroid()
    {
        AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        int result = activity.Call<int>("addNumber", 10, 30);
        txt += result.ToString();
        //activity.Call("Init");
    }
    ////静态com.Company.MyGame
    //public void CallAndroidStatic()
    //{
    //    AndroidJavaClass unityClass = new AndroidJavaClass("com.sdk.OliverInterface");
    //    unityClass.CallStatic("Login");
    //}
    //回调unity
    public void ReceiveNotifyPrSDK(string s)
    {
        Debug.LogError("Unity :" + s);
    }
}
