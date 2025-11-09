using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SVRightBtn : MonoBehaviour
{
    public Button btnSelf;
    public Image isNew;
    public Text txtName;
    public Image state;
    //当前按钮 代表哪个服务器 之后会使用其中数据
    public ServerInfo nowServerInfo;

    void Start()
    {
        btnSelf.onClick.AddListener(() => 
        { 
            //记录当前选择的服务器
            LoginMgr.Instance.LoginData.frontServerID = nowServerInfo.id;

            //隐藏 选服面板
            UIManager.Instance.HidePanel<ServerSelectPanel>();

            //显示 服务器面板
            UIManager.Instance.ShowPanel<ServerPanel>();

        });

    }
    /// <summary>
    /// 初始化按钮
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(ServerInfo info)
    {
        //记录当前按钮代表的服务器
        nowServerInfo = info;
        //更新按钮上的信息
        txtName.text = $"{info.id}区  {info.name}";
        //是否是新服
        isNew.gameObject.SetActive(info.isNew);

        //加载图集
        SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas/Login");
        state.gameObject.SetActive(true);
        //更新按钮上的状态
        switch (info.state)
        {
            case 0:
                state.gameObject.SetActive(false);
                break;
            case 1://流畅
                state.sprite = atlas.GetSprite("ui_DL_liuchang_01");
                break;
            case 2://繁忙
                state.sprite = atlas.GetSprite("ui_DL_fanhua_01");
                break;
            case 3://火爆
                state.sprite = atlas.GetSprite("ui_DL_huobao_01");
                break;
            case 4://维护
                state.sprite = atlas.GetSprite("ui_DL_weihu_01");
                break;
        }


    }
}