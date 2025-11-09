using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI管理器：采用单例模式设计，全局唯一实例
/// 核心职责：统一管理所有UI面板的加载、显示、隐藏、销毁及生命周期
/// 通过字典缓存已创建面板，避免重复实例化，提升性能
/// </summary>
public class UIManager 
{
    // 单例实例（私有构造函数确保全局唯一）
    private static UIManager instance = new UIManager();
    /// <summary>
    /// 公开的单例访问接口，提供全局唯一的UIManager访问点
    /// </summary>
    public static UIManager Instance => instance;

    /// <summary>
    /// Canvas的Transform引用（所有UI面板的父容器）
    /// 确保所有UI面板都挂载在Canvas下，保证正确的渲染层级和事件响应
    /// </summary>
    private Transform canvasTrans;

    // 私有构造函数：初始化时绑定Canvas并设置不销毁
    private UIManager()
    {
        // 查找场景中的Canvas对象
        canvasTrans = GameObject.Find("Canvas").transform;
        // 确保Canvas在场景切换时不会被销毁
        GameObject.DontDestroyOnLoad(canvasTrans.gameObject);
    }
    /// <summary>
    /// 面板缓存字典：存储已创建的UI面板实例
    /// Key：面板类名（字符串，如"MainPanel"）
    /// Value：面板实例（BasePanel子类对象）
    /// 作用：避免重复加载和实例化相同面板，提升性能并便于状态管理
    /// </summary>
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    /// <summary>
    /// 显示指定类型的UI面板（泛型方法）
    /// </summary>
    /// <typeparam name="T">面板类型（必须继承自BasePanel）</typeparam>
    /// <returns>显示的面板实例（T类型）</returns>
    public T ShowPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        // 若面板已存在，直接返回
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        // 从Resources加载面板预制体（路径: Resources/UI/面板类名）
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        // 将面板挂载到Canvas下，第二个参数设为false：保持面板自身的局部坐标（避免缩放/位置异常）
        panelObj.transform.SetParent(canvasTrans, false);

        // 获取面板组件并存入字典
        T panel = panelObj.GetComponent<T>();
        panelDic.Add(panelName, panel);
        // 调用面板的显示逻辑（需在子类实现）
        panel.ShowMe();
        return panel;

    }
    /// <summary>
    /// 隐藏指定类型的UI面板
    /// </summary>
    /// <typeparam name="T">面板类型（必须继承自BasePanel）</typeparam>
    /// <param name="isFade">是否使用淡出动画（true：播放动画后销毁；false：直接销毁）</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        // 检查面板是否存在
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                panelDic[panelName].HideMe(() =>
                {
                    // 淡出完成后销毁面板对象
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    // 从字典移除
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                // 不使用动画：直接销毁面板并移除缓存
                GameObject.Destroy(panelDic[panelName].gameObject);
                panelDic.Remove(panelName);
            }

        }
    }

    /// <summary>
    /// 获取已创建的指定类型面板实例
    /// </summary>
    /// <typeparam name="T">面板类型（必须继承自BasePanel）</typeparam>
    /// <returns>面板实例（T类型）；若未创建则返回null</returns>
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        // 检查缓存中是否存在该面板
        if (panelDic.ContainsKey(panelName))
        {
            return panelDic[panelName] as T;
        }
        // 面板未创建时返回null（外部需判断是否为null再使用）
        return null;
    }


}
