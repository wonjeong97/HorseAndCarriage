using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{   
    public static SpeedManager Instance;
    
    [Header("UI")] 
    [SerializeField] private Text speedText;
    
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    [Header("FOV Settings")]
    [SerializeField] private float fovMin = 60f;
    [SerializeField] private float fovMax = 90f;
    [SerializeField] private float fovLerpTime = 0.5f; // 부드럽게 전환 시간

    [Header("Speed Text Settings")]
    [SerializeField] private float speedTextLerpTime = 1f; // UI 속도 변화 시간
    
    private float _speed;
    private Coroutine _fovCoroutine;
    private Coroutine _speedTextCoroutine;
    
    public float Speed 
    { 
        get => _speed;
        set
        {
            if (Mathf.Approximately(_speed, value)) return;

            float oldSpeed = _speed;
            _speed = value;

            if (speedText)
            {
                UpdateSpeedText(oldSpeed, _speed);
            }

            UpdateCameraFOV();
        } 
    }

    private void Awake()
    {
        if (speedText == null || mainCamera == null)
        {
            Debug.LogWarning("[SpeedManager] SpeedText or mainCamera are not assigned");
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
        _speed = 30f;
        speedText.text = Mathf.RoundToInt(_speed).ToString();
        mainCamera.fieldOfView = fovMin;
    }

    private void UpdateSpeedText(float startValue, float endValue)
    {
        if (_speedTextCoroutine != null)
            StopCoroutine(_speedTextCoroutine);

        _speedTextCoroutine = StartCoroutine(LerpSpeedText(startValue, endValue, speedTextLerpTime));
    }

    private IEnumerator LerpSpeedText(float startValue, float endValue, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, endValue, elapsed / duration);

            // 소수점 없는 정수 표시
            speedText.text = Mathf.RoundToInt(currentValue).ToString();

            yield return null;
        }

        // 마지막 값 확정
        speedText.text = Mathf.RoundToInt(endValue).ToString();
    }
    
    private void UpdateCameraFOV()
    {
        float t = Mathf.InverseLerp(30f, 60f, _speed); 
        float targetFov = Mathf.Lerp(fovMin, fovMax, t);

        if (_fovCoroutine != null) 
            StopCoroutine(_fovCoroutine);

        _fovCoroutine = StartCoroutine(LerpFOV(targetFov, fovLerpTime));
    }

    private IEnumerator LerpFOV(float targetFov, float duration)
    {
        float startFov = mainCamera.fieldOfView;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            mainCamera.fieldOfView = Mathf.Lerp(startFov, targetFov, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.fieldOfView = targetFov;
    }
}
