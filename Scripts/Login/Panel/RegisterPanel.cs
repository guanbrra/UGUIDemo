using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BasePanel
{
    public Button btnCancel;
    public Button btnSure;

    public InputField txtUserName;
    public InputField txtPassword;
    /// <summary>
    /// 注册面板类，继承自基础面板类，负责处理用户注册的UI交互逻辑
    /// </summary>
    public override void Init()
    {
        btnCancel.onClick.AddListener(() => 
        {
            //隐藏自己
            UIManager.Instance.HidePanel<RegisterPanel>();
            //显示登录面板
            UIManager.Instance.ShowPanel<LoginPanel>();
        });
        btnSure.onClick.AddListener(() =>
        {
            if (txtPassword.text.Length < 6 || txtUserName.text.Length < 6)
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名或密码长度不能小于6位");
                return;
            }
            //注册账号密码
            if (LoginMgr.Instance.Register(txtUserName.text, txtPassword.text))
            {
                //清理登录数据
                LoginMgr.Instance.ClearLoginData();

                //注册成功，显示登录面板，更新用户名
                //显示登录面板，更新用户名
                UIManager.Instance.ShowPanel<LoginPanel>().ChangeInputUserName(txtUserName.text);
                //隐藏自己
                UIManager.Instance.HidePanel<RegisterPanel>();

                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("注册成功");
            }
            else
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名已存在");
                txtUserName.text = "";// 清空用户名输入框
                txtPassword.text = "";// 清空密码输入框
            }
                
            //UIManager.Instance.HidePanel<RegisterPanel>();
            
        });

    }
}
