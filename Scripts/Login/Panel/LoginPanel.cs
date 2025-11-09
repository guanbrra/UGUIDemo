using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using ExcelToJsonConverter;

public class LoginPanel : BasePanel
{
    public Button btnRegister;
    public Button btnSure;

    public InputField inputUserName;
    public InputField inputPassword;

    public Toggle togRememberPW;
    public Toggle togAutoLogin;

    //private bool isOn;
    public override void Init()
    {
        btnRegister.onClick.AddListener(() =>
        {
            //显示注册面板
            UIManager.Instance.ShowPanel<RegisterPanel>();
            print("注册");
            UIManager.Instance.HidePanel<LoginPanel>();
        });

        btnSure.onClick.AddListener(() =>
        {
            //登录
            print("登录");
            //判断是否合法
            if (inputUserName.text.Length < 6 || inputPassword.text.Length < 6)
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名或密码长度不能小于6位");
                return;
            }
            // 验证是否合法
            if (LoginMgr.Instance.CheckPassword(inputUserName.text, inputPassword.text))
            {
                // 登录成功
                print("登录成功");
                //记录数据
                LoginMgr.Instance.LoginData.userName = inputUserName.text;
                LoginMgr.Instance.LoginData.passWord = inputPassword.text;
                LoginMgr.Instance.LoginData.isRememberPw = togRememberPW.isOn;
                LoginMgr.Instance.LoginData.isAutoLogin = togAutoLogin.isOn;
                LoginMgr.Instance.SaveLoginData();
                //切换页面 服务器页面
                if (LoginMgr.Instance.LoginData.frontServerID <= 0)
                {
                    //直接打开选服面板
                    UIManager.Instance.ShowPanel<ServerSelectPanel>();
                }
                else
                {
                    //打开服务器面板
                    UIManager.Instance.ShowPanel<ServerPanel>();
                }


                //隐藏自己
                UIManager.Instance.HidePanel<LoginPanel>();
            }
            else
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名或密码错误");
            }
            //UIManager.Instance.HidePanel<LoginPanel>();
        });

        togRememberPW.onValueChanged.AddListener((isOn) =>
        {
            if (!isOn)
            {
                togAutoLogin.isOn = false;
            }

        });

        togAutoLogin.onValueChanged.AddListener((isOn) =>
        {
            if (isOn)
            {
                togRememberPW.isOn = true;
            }

        });

    }

    public override void ShowMe()
    {
        base.ShowMe();
        LoginData loginData = LoginMgr.Instance.LoginData;
        togRememberPW.isOn = loginData.isRememberPw;
        togAutoLogin.isOn = loginData.isAutoLogin;

        inputUserName.text = loginData.userName;

        if (togRememberPW.isOn)
        {
            inputPassword.text = loginData.passWord;
        }
        //自动登录做什么?
        if (togAutoLogin.isOn)
        {
            print("自动登录");
            //验证用户名和密码
            if (LoginMgr.Instance.CheckPassword(inputUserName.text, inputPassword.text))
            {
                // 
                //切换页面 服务器页面
                if (LoginMgr.Instance.LoginData.frontServerID <= 0)
                {
                    //直接打开选服面板
                    UIManager.Instance.ShowPanel<ServerSelectPanel>();
                }
                else
                {
                    //打开服务器面板
                    UIManager.Instance.ShowPanel<ServerPanel>();
                }

                //隐藏自己
                UIManager.Instance.HidePanel<LoginPanel>(false);
            }
            else
            {
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("用户名或密码错误");
            }
        }
    }
    public void ChangeInputUserName(string info)
    {
        inputUserName.text = info;
    }

    //private void ChangeExcelToJson()
    //{
    //    try
    //    {
    //        // 定义文件夹路径
    //        string inputFolder = @"E:\Unity\UGUI_Demo\UGUI_Demo\Assets\ArtRes\ServerInfo.csv";
    //        string outputFolder = @"C:\JSONOutput";

    //        // 调用转换接口
    //        int processedCount = ExcelToJsonConverter.ConvertFolder(inputFolder, outputFolder);

    //        Console.WriteLine($"成功转换 {processedCount} 个CSV文件");
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"转换失败: {ex.Message}");
    //        // 实际应用中应添加更详细的错误处理
    //    }
    //}
    
}
