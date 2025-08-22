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
    
    private float _currentSpeed;
    private Coroutine _fovCoroutine;

    public float Speed { get; set; }

    private void Awake()
    {
        if (speedText == null || mainCamera == null)
        {
            Debug.LogWarning("[SpeedManager] SpeedText or mainCamara are not assigned");
            return;
        }
        
        if (Instance == null)
        {
            Instance = this;
        }

        if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Speed = 30f;
        _currentSpeed = Speed;
        UpdateSpeedText();
        mainCamera.fieldOfView = fovMin;
    }

    private void Update()
    {
        if (!Mathf.Approximately(_currentSpeed, Speed))
        {   
            _currentSpeed = Speed;
            UpdateSpeedText();
            UpdateCameraFOV();
        }
    }

    private void UpdateSpeedText()
    {
        speedText.text = $"{Speed}";
    }
    
    private void UpdateCameraFOV()
    {
        float t = Mathf.InverseLerp(30f, 60f, Speed); 
        float targetFov = Mathf.Lerp(fovMin, fovMax, t);

        if (_fovCoroutine != null) StopCoroutine(_fovCoroutine);
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
