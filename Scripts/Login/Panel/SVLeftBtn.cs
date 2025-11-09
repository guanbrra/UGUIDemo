using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SVLeftBtn : MonoBehaviour
{
    public Button btnSelf;

    //显示区间内容
    public Text txtInfo;

    private int beginIndex = 0;
    private int endIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        btnSelf.onClick.AddListener(() => 
        {
            //通知选服面板 改变右侧区间内容
            //获得ServerSelectPanel
            ServerSelectPanel serverSelectPanel = UIManager.Instance.GetPanel<ServerSelectPanel>();
            //调用UpdatePanel方法
            serverSelectPanel.UpdatePanel(beginIndex, endIndex);

        });

        
    }

    public void InitInfo(int beginIndex, int endIndex)
    {
        //记录设置显示区间内容
        this.beginIndex = beginIndex;
        this.endIndex = endIndex;

        txtInfo.text = $"{beginIndex}—{endIndex}区";
    }

}
