using UnityEngine;
using UnityEngine.SceneManagement; // 场景管理 API 所在的命名空间

public class GlobalMenu : MonoBehaviour
{
    public GameObject panel;      // 全局面板
    public GameObject backButton; // 返回按钮，只在游戏场景显示
    public GameObject blocker;    // 全屏遮罩，挡住面板外所有 UI 点击

    // 面板是否打开，全局只读，只有 GlobalMenu 自己能写
    public static bool IsOpen { get; private set; }

    // Play 开始时只生成一次（场景文件里不再存它）， Awake 里 DDOL 常驻
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Spawn()
    {
        Instantiate(Resources.Load<GameObject>("GlobalUI"));
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 切场景不销毁
    }

    void OnEnable()
    {
        // 订阅"场景加载完成"事件，切场景后重应用暂停状态
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 注销订阅，防止重复调用
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyWorldState(); // 切场景后重应用暂停/返回按钮，防止带面板进游戏时世界不暂停
    }

    void Update()
    {
        // Update 不受 timeScale 影响，暂停时依然能检测 ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePanel();
        }
    }

    public void TogglePanel()
    {
        bool show = !panel.activeSelf;
        panel.SetActive(show);
        IsOpen = show;
        blocker.SetActive(show);

        ApplyWorldState();
    }

    // 暂停与返回按钮只在游戏场景生效
    void ApplyWorldState()
    {
        bool inGame = SceneManager.GetActiveScene().name == "MainScene";
        backButton.SetActive(panel.activeSelf && inGame);
        Time.timeScale = (panel.activeSelf && inGame) ? 0f : 1f;
    }

    // 返回按钮的 OnClick 绑这个方法
    public void BackToStart()
    {
        Time.timeScale = 1f; // 切场景前必须先恢复时间，否则开始场景也是"暂停"的
        SceneManager.LoadScene("StartScene");
    }
}