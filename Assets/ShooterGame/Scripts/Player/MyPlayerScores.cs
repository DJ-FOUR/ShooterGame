using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerScores : MonoBehaviour
{
    public Text ScoresText;             //应用到UI文本
    public static int Scores = 0;       //
    public float PunchSize = 0.3f;      //放大增量
    public float PunchDuration = 0.2f;  //回落时长

    private int formerScore;
    private RectTransform scoresRect;
    private Coroutine punch;

    // Start is called before the first frame update
    void Start()
    {
        formerScore = Scores;
        scoresRect = ScoresText.rectTransform;
    }

    // Update is called once per frame
    void Update()
    {
        //分数变化
        if (Scores != formerScore)
        {
            if (punch != null) StopCoroutine(punch);
            punch = StartCoroutine(ScorePunch());
            formerScore = Scores;
        }
        ScoresText.text = Scores.ToString();
    }

    IEnumerator ScorePunch()
    {
        float t = 0f;
        while (t < PunchDuration)
        {
            t += Time.deltaTime;
            float s = 1f + PunchSize * (1f - Mathf.SmoothStep(0f, 1f, t / PunchDuration));
            scoresRect.localScale = new Vector3(s, s, 1f);
            yield return null;
        }
        scoresRect.localScale = Vector3.one;
        punch = null;
    }
}
