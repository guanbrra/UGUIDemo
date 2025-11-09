using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ServerSelectPanel : BasePanel
{
    //范围
    public Text texRange;
    //服务器列表
    public ScrollRect svLeft;
    public ScrollRect svRight;
    //上一次登录信息
    public Text texFrontServer;
    public Image imgFrontState;

    private List<GameObject> itemList = new List<GameObject>();
    public override void Init()
    {
        //动态创建左侧服务器列表

        //获取服务器列表数据
        List<ServerInfo> svList = LoginMgr.Instance.ServerData;

        //得到一共要循环创建多少个区间按钮
        //由于向上取整所以 要+1 就代表平均分成了num个按钮
        int num = svList.Count / 5 + 1;
        //print("svList.Count:" + svList.Count);
        //int num = Mathf.CeilToInt(svList.Count / 5f);

        for (int i = 0; i < num; i++)
        {
            GameObject item = Instantiate(Resources.Load<GameObject>("UI/SVLeftBtn"));
            item.transform.SetParent(svLeft.content, false);
            //初始化按钮
            SVLeftBtn leftBtn = item.GetComponent<SVLeftBtn>();
            int beginIndex = i * 5 + 1;
            //int endIndex = beginIndex + 4;
            int endIndex = 5 * (i + 1);
            //如果最后一个区间按钮的结束索引大于服务器列表长度
            if (endIndex > svList.Count)
            {
                endIndex = svList.Count;
            }
            //初始化区间按钮
            leftBtn.InitInfo(beginIndex, endIndex);
            //print("beginIndex:" + beginIndex + " endIndex:" + endIndex);
        }

    }

    override public void ShowMe()
    {
        base.ShowMe();
        int id = LoginMgr.Instance.LoginData.frontServerID;
        //初始化上一次选择的服务器
        if (id <= 0)
        {
            texFrontServer.text = "无";
            imgFrontState.gameObject.SetActive(false);
        }
        else
        {
            ServerInfo info = LoginMgr.Instance.ServerData[id - 1];
            //拼接服务器信息
            texFrontServer.text = $"{id}区  {info.name}";

            //加载图集
            SpriteAtlas atlas = Resources.Load<SpriteAtlas>("Atlas/Login");
            imgFrontState.gameObject.SetActive(true);
            //更新按钮上的状态
            switch (info.state)
            {
                case 0:
                    imgFrontState.gameObject.SetActive(false);
                    break;
                case 1://流畅
                    imgFrontState.sprite = atlas.GetSprite("ui_DL_liuchang_01");
                    break;
                case 2://繁忙
                    imgFrontState.sprite = atlas.GetSprite("ui_DL_fanhua_01");
                    break;
                case 3://火爆
                    imgFrontState.sprite = atlas.GetSprite("ui_DL_huobao_01");
                    break;
                case 4://维护
                    imgFrontState.sprite = atlas.GetSprite("ui_DL_weihu_01");
                    break;
            }

        }

        //更新当前选择
        UpdatePanel(1, 5 > LoginMgr.Instance.ServerData.Count ? LoginMgr.Instance.ServerData.Count : 5);

    }

    public void UpdatePanel(int beginIndex, int endIndex)
    {
        //更新服务器区间显示
        texRange.text = $"服务器 {beginIndex}-{endIndex}";

        //删除之前按钮
        for (int i = 0; i < itemList.Count; i++)
        {
            Destroy(itemList[i]);
        }
        itemList.Clear();//清空列表

        //创建新的按钮
        for (int i = beginIndex; i <= endIndex; i++)
        {
            //获取服务器信息
            ServerInfo nowInfo = LoginMgr.Instance.ServerData[i - 1];
            //创建预设体
            GameObject item = Instantiate(Resources.Load<GameObject>("UI/SVRightBtn"));
            item.transform.SetParent(svRight.content, false);
            //更新按钮数据
            SVRightBtn rightBtn = item.GetComponent<SVRightBtn>();
            rightBtn.InitInfo(nowInfo);
            //创建成功后把按钮添加到列表中
            itemList.Add(item);


        }
        
    }

}
