using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 登录管理器类，采用单例模式，负责登录数据（LoginData）的加载与保存
/// </summary>
public class LoginMgr
{
    /// <summary>
    /// 登录管理器的单例实例
    /// </summary>
    private static LoginMgr instance = new LoginMgr();
    public static LoginMgr Instance => instance;

    /// <summary>
    /// 存储登录相关数据的对象
    /// </summary>
    private LoginData loginData;
    //公共属性 方便外面获取
    public LoginData LoginData => loginData;

    //存储注册相关数据的对象
    private RegisterData registerData;
    public RegisterData RegisterData => registerData;

    //所有服务器数据
    private List<ServerInfo> serverData;
    public List<ServerInfo> ServerData => serverData;

    
    /// <summary>
    /// 私有构造函数，初始化时从JSON加载登录数据
    /// </summary>
    private LoginMgr()
    {
        //从JSON文件中加载登录数据
        loginData = JsonMgr.Instance.LoadData<LoginData>(JsonType.LitJson, "LoginData");

        //从JSON文件中加载注册数据
        registerData = JsonMgr.Instance.LoadData<RegisterData>(JsonType.LitJson, "RegisterData");

        //从JSON文件中加载服务器数据
        serverData = JsonMgr.Instance.LoadData<List<ServerInfo>>(JsonType.LitJson, "ServerInfo");

    }

    #region 登录
    /// <summary>
    /// 保存登录数据到JSON文件
    /// </summary>
    public void SaveLoginData()
    {
        JsonMgr.Instance.SaveData(loginData, JsonType.LitJson, "LoginData");
    }

    //注册成功后清理 登录数据
    public void ClearLoginData()
    {
        loginData.frontServerID = 0;
        loginData.isAutoLogin = false;
        loginData.isRememberPw = false;
    }
    #endregion

    #region 注册
    //存储注册数据
    public void SaveRegisterData()
    {
        JsonMgr.Instance.SaveData(registerData, JsonType.LitJson, "RegisterData");
    }

    //注册方法
    public bool Register(string username, string password)
    {
        //判断用户名是否已存在
        if (registerData.rigisterInfo.ContainsKey(username))
        {
            UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名已存在");
            return false;           
        }
        //存储新用户名和密码
        registerData.rigisterInfo.Add(username, password);
        //保存注册数据
        SaveRegisterData();
        return true;
    }

    //验证用户验证密码
    public bool CheckPassword(string username, string password)
    {
        //判断用户是否存在
        if (registerData.rigisterInfo.ContainsKey(username))
        {   //判断密码是否正确
            if (registerData.rigisterInfo[username] == password)
            {
                return true;
            }
        }
        //UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名不存在或密码错误");
        return false;
    }


    #endregion


}
