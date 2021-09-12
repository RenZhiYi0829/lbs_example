using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    int _level;
    public int level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
            SetMap();
        }
    }
    [SerializeField]
    GameObject TileMap;

    [SerializeField]
    List<GameObject> TileMaps;

    LatLng latLng;
    int tileX;
    int tileY;
    int pixelX;
    int pixelY;


    public void AddLevel()
    {
        level++;
    }

    void SetMap()
    {
    }

    private void Start()
    {
        //latLng = new LatLng(112.843434, 34.170877);

        //Location.LatLngToTileXY(latLng, 16, out tileX, out tileY, out pixelX, out pixelY);
        //Debug.Log(tileX + "  " + tileY + "  " + pixelX + "  " + pixelY + "  ");//54659  26799  128  128  

        ////x , y 是中心点
        //Init();


        //StartCoroutine(move());
        //MapUpdate(Direction.right);
    }

    void  MapDraw()
    {

    }

    IEnumerator move()
    {
        while(true)
        {
            yield return new WaitForSeconds(3f);
            MapUpdate(Direction.left);
        }
    }

    [SerializeField]
    public readonly int l = 5;

    private void Init()
    {
        //
        int x = tileX - (l - 1) / 2;
        int y = tileY - (l - 1) / 2;
        //
        Vector3 sour = new Vector3(0 - 1.28f * (l - 1), 0 + 1.28f * (l - 1), 0);
        TileMaps = new List<GameObject>(l * l);
        GameObject[] gameObjects = new GameObject[l * l];
        for (int i=0;i<l;i++)
        {
            for (int j=0;j<l;j++)
            {
                GameObject g = GameObject.Instantiate(TileMap, transform);
                g.transform.localPosition = sour + new Vector3(j * 2.56f, 0, 0);
                //
                int _x = x + j;
                StartCoroutine(Location.SetMap(_x, y, g.GetComponent<SpriteRenderer>()));
                //
                gameObjects[j + i * l] = g;
                g.SetActive(true);
            }
            //
            y += 1;
            //
            sour -= new Vector3(0, 2.56f, 0);
        }
        TileMaps.AddRange(gameObjects);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (transform.localScale.x + 0.1f <= 3.1f)
            {
                transform.localScale += new Vector3(0.1f, 0.1f, 0);
                GameObject.Find("Player").transform.localScale += new Vector3(0.1f, 0.1f, 0.1f)*0.5f;
            }
                
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (transform.localScale.x - 0.1 >= 1.5f)
            {
                transform.localScale -= new Vector3(0.1f, 0.1f, 0);
                GameObject.Find("Player").transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f) * 0.5f;
            }
                
        }

        if (Location.mLatLng!=null)
        {
            latLng = Location.mLatLng;//new LatLng(112.843434, 34.170877);
            Location.LatLngToTileXY(latLng, 16, out tileX, out tileY, out pixelX, out pixelY);
            Debug.Log(tileX + "  " + tileY + "  " + pixelX + "  " + pixelY + "  ");//54659  26799  128  128  
            //x , y 是中心点
            Init();
            Location.mLatLng = null;
        }
    }


    public void MapUpdate(Direction direction = Direction.up)
    {
        int x;
        int y;

        switch (direction)
        {
            case Direction.up:
                //下向上更新
                x = tileX - (l - 1) / 2;
                y = tileY - (l - 1) / 2 - 1;
                for (int i = 0; i < l; i++)
                {
                    //把最后一排搬到第一排
                    TileMaps[i + l * (l - 1)].transform.localPosition += new Vector3(0, 2.56f * l, 0);

                    StartCoroutine(Location.SetMap(x, y, TileMaps[i + l * (l - 1)].GetComponent<SpriteRenderer>()));
                    x++;
                    //冒泡
                    for (int j = 1; j <= l - 1; j++)
                    {
                        GameObject temp = TileMaps[i + l * (l - j)];
                        TileMaps[i + l * (l - j)] = TileMaps[i + l * (l - j - 1)];
                        TileMaps[i + l * (l - j - 1)] = temp;
                    }
                }
                tileY--;
                break;
            case Direction.down:
                x = tileX - (l - 1) / 2;
                y = tileY + (l - 1) / 2 + 1;
                for (int i = 0; i < l; i++)
                {
                    //把第一排搬到最后一排
                    TileMaps[i].transform.localPosition -= new Vector3(0, 2.56f * l, 0);
                    StartCoroutine(Location.SetMap(x, y, TileMaps[i].GetComponent<SpriteRenderer>()));
                    x++;
                    //冒泡
                    for (int j = 0; j < l - 1; j++)
                    {
                        GameObject temp = TileMaps[i + l * j];
                        TileMaps[i + l * j] = TileMaps[i + l * (j + 1)];
                        TileMaps[i + l * (j + 1)] = temp;
                    }
                }
                tileY++;
                break;
            case Direction.left:
                x = tileX - (l - 1) / 2 - 1;
                y = tileY - (l - 1) / 2;
                for (int i = 0; i < l; i++)
                {
                    //把最右列移到最左列
                    TileMaps[i * l + (l-1)].transform.localPosition -= new Vector3(2.56f * l, 0, 0);
                    StartCoroutine(Location.SetMap(x, y, TileMaps[i * l + (l-1)].GetComponent<SpriteRenderer>()));
                    y++;
                    //冒泡
                    for (int j = 1; j <= l - 1; j++)
                    {
                        GameObject temp = TileMaps[i * l + (l - j)];
                        TileMaps[i * l + (l - j)] = TileMaps[i * l + (l - j - 1)];
                        TileMaps[i * l + (l - j - 1)] = temp;
                    }
                }
                tileX--;
                break;
            case Direction.right:
                x = tileX + (l - 1) / 2 + 1;
                y = tileY - (l - 1) / 2;
                for (int i = 0; i < l; i++)
                {
                    //把最左列移到最右列
                    TileMaps[i * l].transform.localPosition += new Vector3(2.56f * l, 0, 0);
                    StartCoroutine(Location.SetMap(x, y, TileMaps[i * l].GetComponent<SpriteRenderer>()));
                    y++;
                    //冒泡
                    for (int j = 0; j < l - 1; j++)
                    {
                        GameObject temp = TileMaps[i * l + j];
                        TileMaps[i * l + j] = TileMaps[i * l + (j + 1)];
                        TileMaps[i * l + (j + 1)] = temp;
                    }                    
                }
                tileX++;
                break;
        }
    }

    //1.5--3
}


public enum Direction
{
    up,
    down,
    left,
    right
}
