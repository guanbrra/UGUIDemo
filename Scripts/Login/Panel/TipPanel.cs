using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public Button btnSure;
    public Text txtTip;
    /// <summary>
    /// 提示面板实现类
    /// 继承自自定义的 BasePanel 面板基类，用于在游戏中显示简短提示信息，并提供确认关闭功能
    /// 核心功能：显示自定义提示文本、点击确认按钮隐藏面板
    /// </summary>
    public override void Init()
    {
        btnSure.onClick.AddListener(() =>
        {
            // 给"确认"按钮添加点击事件监听
            // 点击后通过 UIManager 单例类隐藏当前 TipPanel，参数 true 通常表示是否需要显示隐藏动画（具体逻辑取决于 UIManager 实现）
            UIManager.Instance.HidePanel<TipPanel>(true);
        });

    }

    /// <summary>
    /// 更新提示面板的文本内容
    /// 用于在显示面板前，设置当前需要展示的提示信息
    /// </summary>
    /// <param name="info">需要显示的提示文本（如"登录成功"、"密码错误，请重新输入"等）</param>
    public void ChangeInfo(string info)
    {
        txtTip.text = info;
    }
}
