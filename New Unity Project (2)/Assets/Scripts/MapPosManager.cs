using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPosManager : MonoBehaviour
{
    [SerializeField]
    GameObject Map;

    [SerializeField]
    GameObject Player;

    Vector2 DragPos;

    int level;

    [SerializeField]
    float lx;
    [SerializeField]
    float rx;
    [SerializeField]
    float ty;
    [SerializeField]
    float by;

    float DragFlag = 15f;

    Map map;

    private void Start()
    {
        map = Map.GetComponent<Map>();
        Player = GameObject.Find("Player");
        level = map.l;

        int f = level - 2;
        float f1 = f * 2.56f;
        float f2 = f * 2.56f * Mathf.Cos(45);

        lx = f1;
        rx = -f1;
        ty = -f2;
        by = f2;

    }

    void Update()
    {
        Gesture currentGesture = EasyTouch.current;//获取当前手势
        //注意 当手机屏幕上没有任何操作的时候currentGesture是null的,要添加一个判断,防止空指针异常
        if (currentGesture == null) return;
        //以下是事件监听
        if (EasyTouch.EvtType.On_TouchStart == currentGesture.type)//按下
        {
            OnTouchStart(currentGesture);

            //可以把方法写在里面
            print("OnTouchStart");
            print("StartPosition" + currentGesture.startPosition);//触摸位置
        }

        if (EasyTouch.EvtType.On_TouchUp == currentGesture.type)//抬起
        {
            OnTouchEnd(currentGesture);
        }

        if (EasyTouch.EvtType.On_Swipe == currentGesture.type)//滑动
        {
            OnSwipe(currentGesture);
        }

        if (Map.transform.position.x > lx)
        {
            map.MapUpdate(Direction.left);
            lx += 2.56f * 3;
            rx += 2.56f * 3;
        }
        if (Map.transform.position.x < rx)
        {
            map.MapUpdate(Direction.right);
            rx -= 2.56f * 3;
            lx -= 2.56f * 3;
        }
        if (Map.transform.position.y < ty)
        {
            map.MapUpdate(Direction.up);
            ty -= (2.56f * Mathf.Cos(45) * 4 );
            by -= (2.56f * Mathf.Cos(45) * 4 );

        }
        if (Map.transform.position.y > by)
        {
            map.MapUpdate(Direction.down);
            by += (2.56f * Mathf.Cos(45) * 4 );
            ty += (2.56f * Mathf.Cos(45) * 4 );
        }
    }

    void OnTouchStart(Gesture gesture)
    {
        print("OnTouchStart");
        print("StartPosition" + gesture.startPosition);//触摸位置
        Player.SetActive(false);
        DragPos = gesture.position;
    }
    void OnTouchEnd(Gesture gesture)
    {
        print("OnTouchEnd");
        print("OnTouchEnd" + gesture.actionTime);//触摸时间
        Player.SetActive(true);
    }

    void OnSwipe(Gesture gesture)
    {
        print("OnSwipe");
        print("Type" + gesture.type);//触摸类型
        Vector2 offset = (gesture.position - DragPos).normalized;
        Map.transform.position += new Vector3(offset.x, offset.y * Mathf.Cos(45), offset.y * Mathf.Cos(45)) * Time.deltaTime * DragFlag;
        DragPos = gesture.position;
    }

    public void MoveMap(Vector3 vector3)
    {
        Map.transform.position -= new Vector3(vector3.x, vector3.z * Mathf.Cos(45), vector3.z * Mathf.Cos(45));
    }
}
