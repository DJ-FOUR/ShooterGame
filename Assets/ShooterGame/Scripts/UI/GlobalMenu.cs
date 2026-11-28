using UnityEngine;
using UnityEngine.SceneManagement; // 场景管理 API 所在的命名空间

public class GlobalMenu : MonoBehaviour
{
    public GameObject panel;      // 全局面板
    public GameObject backButton; // 返回按钮，只在游戏场景显示
    public GameObject blocker;    // 全屏遮罩，挡住面板外所有 UI 点击

    // 面板是否打开（状态）默认false
    public static bool IsOpen { get; private set; }

    // Play 开始时只生成一次（场景文件里不再存它）， Awake 里 DDOL 常驻
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Spawn()
    {
        Instantiate(Resources.Load<GameObject>("Panel/GlobalUI"));
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 切场景不销毁
    }
    //只要本物体存在，场景加载时就会调用OnSceneLoaded
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyWorldState();
    }

    void Update()
    {
        // Update 不受 timeScale 影响，暂停时依然能检测 ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePanel();
        }
    }

    // 切换面板状态
    public void TogglePanel()
    {
        SetOpen(!IsOpen);
    }
    // 调整面板开/关
    void SetOpen(bool show)
    {
        panel.SetActive(show);
        blocker.SetActive(show);
        IsOpen = show;

        ApplyWorldState();
    }

    // 判断游戏是否需要暂停，面板内是否需要增加按钮
    void ApplyWorldState()
    {
        //判断的当前场景是不是MainScene
        bool inGame = SceneManager.GetActiveScene().name == "MainScene";
        //(面板是否激活 && 当前场景是不是MainScene)同时满足才会出现back按钮，游戏暂停
        backButton.SetActive(panel.activeSelf && inGame);       
        Time.timeScale = (panel.activeSelf && inGame) ? 0f : 1f;
    }

    // 返回开始界面
    public void BackToStart()
    {
        SetOpen(false);     //关掉面板
        Time.timeScale = 1f; // 切场景前必须先恢复时间，否则开始场景也是"暂停"的
        SceneManager.LoadScene("StartScene");
    }
}