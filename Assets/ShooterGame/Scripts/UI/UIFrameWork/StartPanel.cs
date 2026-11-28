using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 开始主面板
/// </summary>
public class StartPanel : BasePanel
{
    static readonly string path = "test";

    public static UIType startPanel = new UIType(path);
    public StartPanel() : base(startPanel) { }

}
