/****************************************************
    File：Constants.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:56:38
	Description ：常量配置
*****************************************************/

public class Constants 
{
    // 场景名称
    public const string SceneLogin = "SceneLogin";
    public const string SceneMainCity = "SceneMainCity";

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