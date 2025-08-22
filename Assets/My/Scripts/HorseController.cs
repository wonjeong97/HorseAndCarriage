using System;
using UnityEngine;

public class HorseController : MonoBehaviour
{
    public static HorseController Instance;

    [Header("Horse Sprite")]
    [SerializeField] private GameObject horse1;
    [SerializeField] private GameObject horse2;
    [SerializeField] private GameObject horse3;
    [SerializeField] private GameObject horse4;
    [SerializeField] private GameObject horse5;

    private int _upgradeCount;

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

        horse2.SetActive(false);
        horse3.SetActive(false);
        horse4.SetActive(false);
        horse5.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpgradeHorses();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DowngradeHorses();
        }
    }

    private void UpgradeHorses()
    {
        if (_upgradeCount >= 5) return;
        _upgradeCount++;
        SpeedManager.Instance.Speed += 5f;
        
        switch (_upgradeCount)
        {
            case 2:
                horse2.SetActive(true);
                break;
            case 3:
                horse3.SetActive(true);
                break;
            case 4:
                horse4.SetActive(true);
                break;
            case 5:
                horse5.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void DowngradeHorses()
    {
        if (_upgradeCount <= 1) return;
        _upgradeCount--;
        SpeedManager.Instance.Speed -= 5f;
        
        switch (_upgradeCount)
        {
            case 4:
                horse5.SetActive(false);
                break;
            case 3:
                horse4.SetActive(false);
                break;
            case 2:
                horse3.SetActive(false);
                break;
            case 1:
                horse2.SetActive(false);
                break;
            default:
                break;
        }
    }
}