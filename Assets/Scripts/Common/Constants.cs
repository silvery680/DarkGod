/****************************************************
    File：Constants.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:56:38
	Description ：常量配置
*****************************************************/

public enum TxtColor
{
    Red,
    Green,
    Blue,
    Yellow,
}

public class Constants 
{
    private const string ColorRed = "<color=#FF0000FF>";
    private const string ColorGreen = "<color=#00FF00FF>";
    private const string ColorBlue = "<color=#00B4FFFF>";
    private const string ColorYellow = "<color=#FFFF00FF>";
    private const string ColorEnd = "</color>";

    public static string Color(string str, TxtColor c)
    {
        string result = "";
        switch(c)
        {
            case TxtColor.Red:
                result = ColorRed + str + ColorEnd;
                break;
            case TxtColor.Green:
                result = ColorGreen + str + ColorEnd;
                break;
            case TxtColor.Blue:
                result = ColorBlue + str + ColorEnd;
                break;
            case TxtColor.Yellow:
                result = ColorYellow + str + ColorEnd;
                break;
        }
        return result;
    }

    // AutoGuideNPC
    public const int NPCWiseManID = 0;
    public const int NPCGeneralID = 1;
    public const int NPCArtisanID = 2;
    public const int NPCTraderID = 3;

    public const string NPCWiseManName = "智者";
    public const string NPCGeneralName = "将军";
    public const string NPCArtisanName = "工匠";
    public const string NPCTraderName = "商人";
    public const string NPCGuideName = "小芸";

    // 场景名称/ID
    public const string SceneLogin = "SceneLogin";
    public const int MainCityMapID = 10000;
    // public const string SceneMainCity = "SceneMainCity";

    #region BGM
    // 登录场景音效
    public const string BGLogin = "bgLogin";
    // 主城场景音效
    public const string BGMainCity = "bgMainCity";
    #endregion

    // 登录按钮音效
    public const string UILoginBtn = "uiLoginBtn";
    // 主城菜单展开音效
    public const string UIExtenBtn = "uiExtenBtn";
    // 常规UI点击音效
    public const string UIClickBtn = "uiClickBtn";
    // 窗口打开音效
    public const string UIOpenPage = "uiOpenPage";

    // 屏幕标准宽高比
    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;

    // 摇杆点标准距离
    public const int ScreenOPDis = 90;

    // 混合参数
    public const int BlendIdle = 0;
    public const int BlendWalk = 1;

    // 角色移动速度
    public const int PlayerMoveSpeed = 8;
    public const int MonsterMoveSpeed = 4;

    // 运动平滑加速度
    public const float AccelerSpeed = 5;
}