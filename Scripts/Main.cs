using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TipPanel t = UIManager.Instance.ShowPanel<TipPanel>();
        //t.ChangeInfo("1234567");
        UIManager.Instance.ShowPanel<LoginBKPanel>();
        UIManager.Instance.ShowPanel<LoginPanel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
