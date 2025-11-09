using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginData 
{
    // 记录玩家的用户名
    public string userName;
    // 记录玩家的密码
    public string passWord;

    // 标记是否记住密码的状态
    public bool isRememberPw;
    // 标记是否自动登录的状态
    public bool isAutoLogin;


    //服务器相关
    public int frontServerID = -1; //-1表示没有选择服务器
    //public int serverID;
    public string serverName;
    
}
