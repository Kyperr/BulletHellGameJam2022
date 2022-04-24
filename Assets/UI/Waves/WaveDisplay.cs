using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class WaveDisplay : MonoBehaviour
{

    [SerializeField]
    private Text text;

    [SerializeField]
    private List<WaveFrameStep> waveFrameSets;

    [SerializeField]
    private GameObject twinkle;

    [SerializeField]
    private float twinkleScale = 1f;

    [SerializeField]
    private float twinkleTime = 5f;

    [SerializeField]
    private float twinkleScaleSpeed = 1f;

    private WaveFrameStep currentWaveStep;

    void Start()
    {
        EnemySpawner.Instance.OnWaveChange += (wave) =>
        {
            UpdateWave(wave);
        };
        currentWaveStep = waveFrameSets[0];
        waveFrameSets[0].display.SetActive(true);
    }

    void UpdateWave(int wave)
    {
        text.text = "Wave: " + wave;
        WaveFrameStep newFrameStep = waveFrameSets[0];
        for (int i = 1; i < waveFrameSets.Count; i++)
        {
            if (waveFrameSets[i].wave <= wave)
            {
                newFrameStep = waveFrameSets[i];
                waveFrameSets[i - 1].display.SetActive(false);
            }
            else
            {
                break;
            }
        }

        if (newFrameStep != currentWaveStep)
        {
            currentWaveStep = newFrameStep;
            StartCoroutine(TwinkleAnimation(newFrameStep));
            newFrameStep.display.SetActive(true);
        }

    }

    private IEnumerator TwinkleAnimation(WaveFrameStep step)
    {
        Image image = twinkle.GetComponent<Image>();
        float alpha = 150;

        twinkle.SetActive(true);
        image.color = new Color(step.color.r, step.color.g, step.color.b, alpha);

        float timeScaling = 0;
        Vector3 endScale = new Vector3(twinkleScale, twinkleScale, twinkleScale);
        twinkle.transform.localScale = new Vector3();

        Debug.Log("Scale and rotate. End scale: " + endScale);
        // Scale and rotate
        while (timeScaling < twinkleTime)
        {
            twinkle.transform.localScale = Vector3.Lerp(twinkle.transform.localScale, endScale, twinkleScaleSpeed * timeScaling);
            twinkle.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 10);
            timeScaling += Time.deltaTime;
            yield return 0;
        }

        timeScaling = 0;
        // Reverse it
        while (timeScaling < twinkleTime)
        {
            twinkle.transform.localScale = Vector3.Lerp(twinkle.transform.localScale, new Vector3(0, 0, 0), twinkleScaleSpeed * timeScaling);
            twinkle.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 10);
            timeScaling += Time.deltaTime;
            yield return 0;
        }

        Debug.Log("Now delete.");
        twinkle.SetActive(false);

        yield break;
    }

    [Serializable]
    public class WaveFrameStep
    {
        public int wave;

        public GameObject display;
        public Color color;
    }
}
