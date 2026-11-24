using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUIManager : MonoBehaviour
{

    public void OnButtonClick()
    {
        SceneManager.LoadScene("MainScene");
    }

}
