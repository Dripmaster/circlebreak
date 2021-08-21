using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Text targetScoreText;
    [SerializeField] Text currentScoreText;
    [SerializeField] Slider scoreBar;

    [Header("Level Settings")]
    [SerializeField] int targetScore;
    [SerializeField] int currentScore;

    [Header("Score Effect Setings")]
    [SerializeField] float scoreEffectScale;
    [SerializeField] float currentScoreEffectDuration;
    [SerializeField] float targetScoreEffectDuration;

    IEnumerator currentScoreCoroutine;

    public void SetCurrentScoreText(int score)
    {
        if (score > targetScore)
            score = targetScore;
        if (currentScoreCoroutine != null)
            StopCoroutine(currentScoreCoroutine);
        currentScoreCoroutine = SetCurrentScore(score);
        StartCoroutine(currentScoreCoroutine);
    }
    IEnumerator SetCurrentScore(int score)
    {
        float eTime = 0f;
        int initialScore = currentScore;
        Debug.Log(initialScore + " to " + score);
        while (eTime < currentScoreEffectDuration)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
            float x = eTime / currentScoreEffectDuration;
            currentScore = (int)Mathf.Lerp(initialScore, score, x);
            scoreBar.value = currentScore;
            currentScoreText.transform.localScale
                = Mathf.Lerp(scoreEffectScale, 1, TimeCurves.ExponentialMirrored(x)) * new Vector2(1, 1);
            currentScoreText.text = string.Format("{0:#,###}", currentScore);

        }
        currentScoreText.text = string.Format("{0:#,###}", score);
        currentScoreText.transform.localScale = new Vector3(1, 1, 1);

    }
    private void Awake()
    {
        targetScoreText.text = "/0";
        currentScoreText.text = "0";

        scoreBar.maxValue = targetScore;
        scoreBar.value = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            StartCoroutine(SetTargetScore());
        if (Input.GetKeyDown(KeyCode.Y))
            SetCurrentScoreText(currentScore + 1000);

    }
    IEnumerator SetTargetScore()
    {
        float eTime = 0f;
        int currentValue = 0;
        while(eTime < targetScoreEffectDuration)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
            float x = eTime / targetScoreEffectDuration;
            currentValue = (int)Mathf.Lerp(0, targetScore, x);
            targetScoreText.transform.localScale
                = Mathf.Lerp(scoreEffectScale, 1, TimeCurves.ExponentialMirrored(x)) * new Vector2(1,1);
            targetScoreText.text = string.Format("/{0:#,###}", currentValue);
        }
        targetScoreText.text = string.Format("/{0:#,###}", targetScore);
        targetScoreText.transform.localScale = new Vector3(1, 1, 1);
    }
}
