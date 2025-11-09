using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //整体控制淡入淡出的画布组件
    private CanvasGroup canvasGroup;
    private float alphaSpeed = 10f;
    private bool isShow = false;

    private UnityAction hideCallback;
    protected virtual void Awake()
    {
        //一开始获取面板上挂载的组件
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null )
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    protected virtual void Start()
    {
        Init();
        
    }
    public abstract void Init();

    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }
    public virtual void HideMe(UnityAction callback)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        hideCallback = callback;
    }

    // Update is called once per frame
    void Update()
    {
        // 修正逻辑：优先完成当前动画，再响应新状态
        if (isShow)
        {
            // 仅当未完全显示时进行显示动画
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha + Time.deltaTime * alphaSpeed, 0, 1);
                //print("alpha:" + canvasGroup.alpha);
            }
        }
        else
        { // isShow = false
          // 仅当未完全隐藏时进行隐藏动画
            if (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha - Time.deltaTime * alphaSpeed, 0, 1);
                if (canvasGroup.alpha <= 0)
                {
                    hideCallback?.Invoke();
                }
            }
        }
    }
}
//if (isShow && canvasGroup.alpha != 1)
//{
//    canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha + Time.deltaTime * alphaSpeed, 0, 1);//Mathf.Clamp防止超出范围
//    print("alpha:" + canvasGroup.alpha);
//}
//else if (!isShow)
//{
//    canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha - Time.deltaTime * alphaSpeed, 0, 1);
//    print("alpha:" + canvasGroup.alpha);
//    if (canvasGroup.alpha <= 0)
//    {
//        hideCallback?.Invoke();
//    }
//}
