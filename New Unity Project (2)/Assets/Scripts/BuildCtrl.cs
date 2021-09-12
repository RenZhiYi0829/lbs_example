using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCtrl : MonoBehaviour
{
    [SerializeField]
    GameObject Build;

    Queue<GameObject> builds;

    public static BuildCtrl Instance;

    private void Awake()
    {
        Instance = this;
        builds = new Queue<GameObject>();
    }

    public void MakeBuild()
    {
        GameObject g = GameObject.Instantiate(Build, GameObject.Find("Map").transform);
        g.transform.position = GameObject.Find("Player").transform.position;
        g.transform.position += new Vector3(0, 0, -0.5f);
        builds.Enqueue(g);
    }

    public void DestoryBuild()
    {
        if (builds.Count != 0)
            GameObject.Destroy(builds.Dequeue());
    }


}
