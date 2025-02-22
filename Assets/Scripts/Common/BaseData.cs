/********************************************************************
	File: 	BaseData.cs
	Author:	groundhog
	Time:	2025/2/22  22:20
	Description: 配置数据类
*********************************************************************/

using UnityEngine;

public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;

}

public class BaseData<T>
{
    public int ID;
}
