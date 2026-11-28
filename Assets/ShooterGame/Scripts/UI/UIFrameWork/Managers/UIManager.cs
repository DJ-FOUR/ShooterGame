using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

/// <summary>
/// 通过字典存储所有UI信息缓存，并可以创建或者销毁UI。避免多次加载UI和关闭UI造成的性能开销
/// </summary>
public class UIManager
{
    /// <summary>
    /// 存储所有UI信息的字典，每一个UI信息都会对应一个GameObject
    /// </summary>
    private Dictionary<UIType, GameObject> dicUI;

    public UIManager()
    {
        dicUI = new Dictionary<UIType, GameObject>();
    }
    /// <summary>
    /// 获取一个UI对象
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetSingleUI(UIType type)
    {

        GameObject parent = GameObject.Find("Canvas");
        if (!parent)
        {
            Debug.Log("Canvas不存在，请仔细查找有无这个对象");
            return null;
        }
        if (dicUI.ContainsKey(type))
        {
            return dicUI[type];
        }
        GameObject ui = GameObject.Instantiate(Resources.Load<GameObject>(type.Path), parent.transform);
        ui.name = type.Name;
        
        dicUI[type] = ui;
        return ui;
    }
    /// <summary>
    /// 销毁一个UI对象
    /// </summary>
    /// <param name="type"></param>
    public void DestoryUI(UIType type)
    {
        if (dicUI.ContainsKey(type)) {
            DestoryUI(type);
            dicUI.Remove(type);
        }
    }
}
