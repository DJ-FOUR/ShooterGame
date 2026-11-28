using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// 面板管理器，用来存储UI
/// </summary>
public class PanelManager
{
    /// <summary>
    /// 存储UI面板的栈
    /// </summary>
    private Stack<BasePanel> stackPanel;
    /// <summary>
    /// UI管理器
    /// </summary>
    private UIManager uIManager;
    private BasePanel panel;

    public PanelManager()
    {
        stackPanel = new Stack<BasePanel>();
        uIManager = new UIManager();
    }
    /// <summary>
    /// UI的入栈操作，此操作会显示一个面板
    /// </summary>
    /// <param name="nextPanel">要显示的面板</param>
    public void Push(BasePanel nextPanel)
    {
        if (stackPanel.Count > 0)
        {
            panel = stackPanel.Peek();
            panel.Onpause();
        }
        stackPanel.Push(nextPanel);     //入栈
        GameObject panelGo = uIManager.GetSingleUI(nextPanel.MYUI);
    }
    /// <summary>
    /// 执行面板的出栈操作，此操作会执行面板的
    /// </summary>
    public void Pop()
    {
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().OnExit();
            stackPanel.Pop();
        }
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().OnResume();
        }
    }

}
