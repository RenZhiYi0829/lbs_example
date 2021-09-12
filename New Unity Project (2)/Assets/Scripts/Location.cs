using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer SpriteRenderer;

    public static LatLng mLatLng = null;

    string mapPath = "https://wprd02.is.autonavi.com/appmaptile?lang=zh_cn&size=1&style=7&x=54658&y=26799&z=16&scl=1&ltype=1";
    ///////http://online1.map.bdimg.com/onlinelabel/?qt=tile&x=49310&y=10242&z=18
    //private void Awake()
    //{
    //    StartCoroutine(SetMap(mapPath));
    //}

    IEnumerator SetMap(string path)
    {
        WWW www = new WWW(path);
        while(!www.isDone)
        {
            yield return null;
        }
        SpriteRenderer.sprite = Sprite.Create(www.texture,new Rect(0,0,256,256),new Vector2(0.5f,0.5f));//www.texture;SpriteRenderer.sprite.pivot
    }

    public static IEnumerator SetMap(int x,int y, SpriteRenderer spriteRenderer)
    {
        string _path = string.Format("http://webrd01.is.autonavi.com/appmaptile?x={0}&y={1}&z=16&lang=zh_cn&size=1&scale=1&style=8", x, y);
    
        //string.Format("http://online1.map.bdimg.com/onlinelabel/?qt=tile&x={0}&y={1}&z=18", x, y);
        //string.Format("https://wprd02.is.autonavi.com/appmaptile?lang=zh_cn&size=1&style=7&x={0}&y={1}&z=16&scl=1&ltype=1", x, y);

        WWW www = new WWW(_path);
        while (!www.isDone)
        {
            yield return null;
        }
        spriteRenderer.sprite = Sprite.Create(www.texture, new Rect(0, 0, 256, 256), new Vector2(0.5f, 0.5f));//www.texture;SpriteRenderer.sprite.pivot
    }

    IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location is not enabled");
            yield break;
        }
        Input.location.Start();
        int maxWait = 30;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1)
        {
            Debug.LogError("Location time out");
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }
        else
        {
            LatLng latlng = new LatLng(Input.location.lastData.longitude, Input.location.lastData.latitude);
            mLatLng = latlng;
            Debug.Log(latlng);
        }
        Input.location.Stop();
    }

    /// <summary>
    /// 将tile(瓦片)坐标系转换为LatLngt(地理)坐标系，pixelX，pixelY为图片偏移像素坐标
    /// </summary>
    /// <param name="tileX"></param>
    /// <param name="tileY"></param>
    /// <param name="zoom"></param>
    /// <param name="pixelX"></param>
    /// <param name="pixelY"></param>
    /// <returns></returns>
    public static LatLng TileXYToLatLng(int tileX, int tileY, int zoom, int pixelX = 0, int pixelY = 0)
    {
        double size = Math.Pow(2, zoom);
        double pixelXToTileAddition = pixelX / 256.0;
        double lng = (tileX + pixelXToTileAddition) / size * 360.0 - 180.0;

        double pixelYToTileAddition = pixelY / 256.0;
        double lat = Math.Atan(Math.Sinh(Math.PI * (1 - 2 * (tileY + pixelYToTileAddition) / size))) * 180.0 / Math.PI;
        return new LatLng(lng, lat);
    }
    /// <summary>
    /// 将LatLngt地理坐标系转换为tile瓦片坐标系，pixelX，pixelY为图片偏移像素坐标
    /// </summary>
    /// <param name="latlng"></param>
    /// <param name="zoom"></param>
    /// <param name="tileX"></param>
    /// <param name="tileY"></param>
    /// <param name="pixelX"></param>
    /// <param name="pixelY"></param>
    public static void LatLngToTileXY(LatLng latlng, int zoom, out int tileX, out int tileY, out int pixelX, out int pixelY)
    {
        double size = Math.Pow(2, zoom);
        double x = ((latlng.Longitude + 180) / 360) * size;
        double lat_rad = latlng.Latitude * Math.PI / 180;
        double y = (1 - Math.Log(Math.Tan(lat_rad) + 1 / Math.Cos(lat_rad)) / Math.PI) / 2;
        y = y * size;

        tileX = (int)x;
        tileY = (int)y;
        pixelX = (int)((x - tileX) * 256);
        pixelY = (int)((y - tileY) * 256);
    }

    public static void Test()
    {
        //49309  10241
        LatLng latLng = TileXYToLatLng(49309, 10241, 16, 128, 128);  // TileXYToLatLng(54658, 26799, 16, 128, 128);
        Debug.Log(latLng.Longitude + "   " + latLng.Latitude);//120.250854492188   31.128199299112

        int tileX;
        int tileY;
        int pixelX;
        int pixelY;
        LatLngToTileXY(latLng, 16, out tileX, out tileY, out pixelX, out pixelY);

        Debug.Log(tileX + "  " + tileY + "  " + pixelX + "  " + pixelY + "  ");//54659  26799  128  128  
    }
}

public class LatLng
{
    public double Longitude;
    public double Latitude;
    public LatLng(double longitude, double latitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }
}
