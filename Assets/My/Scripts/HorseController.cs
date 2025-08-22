using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseController : MonoBehaviour
{
    public static HorseController Instance;

    [Header("Fade Time")]
    public float horseFadeTime = 0.5f;

    [Header("Horse Sprite")] 
    public GameObject horse1;
    public GameObject horse2;
    public GameObject horse3;
    public GameObject horse4;
    public GameObject horse5;

    private int _upgradeCount;
    private readonly List<GameObject> _horseList = new List<GameObject>();
    private List<SpriteRenderer> _horseRenderers = new List<SpriteRenderer>();
    private bool _isFading = false;

    private void Awake()
    {
        if (!horse1 || !horse2 || !horse3 || !horse4 || !horse5)
        {
            Debug.LogWarning("[HorseController] Please assign the horses");
            return;
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _upgradeCount = 1;
        InitializeHorses();
    }

    private void Update()
    {
        if (_isFading) return;
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpgradeHorses();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DowngradeHorses();
        }
    }

    private void InitializeHorses()
    {
        _horseList.Add(horse1);
        _horseList.Add(horse2);
        _horseList.Add(horse3);
        _horseList.Add(horse4);
        _horseList.Add(horse5);

        foreach (var horse in _horseList)
        {
            var sr = horse.GetComponentInChildren<SpriteRenderer>();
            _horseRenderers.Add(sr);

            // 첫 번째 말만 보이고 나머지는 투명
            if (sr != null && horse != horse1)
            {
                sr.color = new Color(1, 1, 1, 0);
            }
        }
    }

    private IEnumerator FadeHorse(int index, float startAlpha, float targetAlpha, float duration)
    {
        var horseSprite = _horseRenderers[index];
        if (horseSprite)
        {
            _isFading = true;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                horseSprite.color = new Color(1, 1, 1,
                    Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration));
                elapsed += Time.deltaTime;
                yield return null;
            }

            horseSprite.color = new Color(1, 1, 1, targetAlpha);
            _isFading = false;
        }
    }

    private void UpgradeHorses()
    {
        if (_upgradeCount >= _horseList.Count) return;
        _upgradeCount++;
        SpeedManager.Instance.Speed += 5f;

        StopAllCoroutines();
        StartCoroutine(FadeHorse(_upgradeCount - 1, 0, 1, horseFadeTime));
    }

    private void DowngradeHorses()
    {
        if (_upgradeCount <= 1) return;
        StopAllCoroutines();

        StartCoroutine(FadeHorse(_upgradeCount - 1, 1, 0, horseFadeTime));
        _upgradeCount--;
        SpeedManager.Instance.Speed -= 5f;
    }
}