using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{
    PanelManager panelManager;
    private void Awake()
    {
        panelManager = new PanelManager();
    }
    private void Start()
    {
        panelManager.Push(new StartPanel());
    }

    public void OnButtonClick()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    // setting 按钮的中转：GlobalUI 运行时才生成，编辑期拖不到，由场景脚本运行时找它
    public void OpenSettings()
    {
        FindObjectOfType<GlobalMenu>().TogglePanel();
    }


}
