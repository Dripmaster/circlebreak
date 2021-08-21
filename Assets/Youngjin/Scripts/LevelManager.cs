using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int Score { get { return currentScore; } }

    [Header("References")]
    [SerializeField] Text targetScoreText;
    [SerializeField] Text currentScoreText;
    [SerializeField] Slider scoreBar;
    [SerializeField] MapEffects mapEffector;

    [Header("Level Settings")]
    [SerializeField] int targetScore;
    public int NormalWallScore;

    [Header("Score Effect Setings")]
    [SerializeField] float scoreEffectScale;
    [SerializeField] float currentScoreEffectDuration;
    [SerializeField] float targetScoreWaitDuration;
    [SerializeField] float targetScoreEffectDuration;

    IEnumerator currentScoreCoroutine;
    int currentShownScore;
    int currentScore = 0;


    public void SetScore(int score)
    {
        if (score > targetScore)
            score = targetScore;
        currentScore = score;
        if (currentScoreCoroutine != null)
            StopCoroutine(currentScoreCoroutine);
        currentScoreCoroutine = SetCurrentScore(score);
        StartCoroutine(currentScoreCoroutine);

        if(score == targetScore)
        {
            mapEffector.OnGameClear();
        }
    }
    IEnumerator SetCurrentScore(int score)
    {
        float eTime = 0f;
        int initialScore = currentShownScore;
        while (eTime < currentScoreEffectDuration)
        {
            yield return null;
            eTime += Time.unscaledDeltaTime;
            float x = eTime / currentScoreEffectDuration;
            currentShownScore = (int)Mathf.Lerp(initialScore, score, x);
            scoreBar.value = currentShownScore;
            currentScoreText.transform.localScale
                = Mathf.Lerp(scoreEffectScale, 1, TimeCurves.ExponentialMirrored(x)) * new Vector2(1, 1);
            currentScoreText.text = string.Format("{0:#,###}", currentShownScore);

        }
        currentScoreText.text = string.Format("{0:#,###}", currentScore);
        currentScoreText.transform.localScale = new Vector3(1, 1, 1);

    }
    private void Awake()
    {
        targetScoreText.text = "/0";
        currentScoreText.text = "0";

        scoreBar.maxValue = targetScore;
        scoreBar.value = 0;
        StartCoroutine(SetTargetScore());
    }
    IEnumerator SetTargetScore()
    {
        float eTime = 0f;
        int currentValue = 0;
        yield return new WaitForSeconds(targetScoreWaitDuration);
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
    public void ChangeScene()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
