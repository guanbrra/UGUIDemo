using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerPanel : BasePanel
{
    public Button btnChange;
    public Button btnBack;
    public Button btnSure;
    public Text txtServer;
    public override void Init()
    {
        btnBack.onClick.AddListener(() => 
        {
            //隐藏服务器面板
            UIManager.Instance.HidePanel<ServerPanel>();
            //避免自动登录 返回时 出现问题
            if (LoginMgr.Instance.LoginData.isAutoLogin)
                LoginMgr.Instance.LoginData.isAutoLogin = false;

            UIManager.Instance.ShowPanel<LoginPanel>();
        });
        btnSure.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ServerPanel>();

            //隐藏登录背景图
            UIManager.Instance.HidePanel<LoginBKPanel>();

            //存储当前显示的服务器信息
            LoginMgr.Instance.SaveLoginData();

            //切场景
            SceneManager.LoadScene("GameScene");
        });
        btnChange.onClick.AddListener(() =>
        {
            //显示服务器列表面板
            UIManager.Instance.ShowPanel<ServerSelectPanel>();
            
            //隐藏服务器面板
            UIManager.Instance.HidePanel<ServerPanel>();
        });
        
    }
    public override void ShowMe()
    {
        base.ShowMe();
        //更新获取服务器信息
        //获取上一次服务器ID
        int id = LoginMgr.Instance.LoginData.frontServerID;
        if (id <= 0)
        {
            txtServer.text = "请选择服务器";
        }
        else
        {
            //根据服务器ID获取服务器信息
            ServerInfo info = LoginMgr.Instance.ServerData[id - 1];
            txtServer.text = info.id +"区 "+  info.name;
        }
        
    }

}
