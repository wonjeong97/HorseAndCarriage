using System.Collections;
using UnityEngine;

public class CarriageController : MonoBehaviour
{
    public static CarriageController Instance;

    [Header("Carriage Sprite")]
    [SerializeField] private GameObject carriage;
    
    [Header("Upgrade Settings")]
    [SerializeField] private float scaleTime = 0.5f; 
    [SerializeField] private float scaleStep = 0.2f; 

    private int _upgradeCount;
    private Vector3 _currentScale;
    private bool _isScaling;
    
    private void Awake()
    {
        if (carriage == null)
        {
            Debug.LogWarning("[CarriageController] _carriage is not assigned]");
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
        _currentScale = gameObject.transform.localScale;
        _upgradeCount = 1;
    }

    private void Update()
    {   
        if (_isScaling) return; // 스케일 중이면 입력 무시
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpgradeCarriage();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DowngradeCarriage();
        }
    }

    private void UpgradeCarriage()
    {
        if (_upgradeCount >= 5) return;

        _upgradeCount++;
        SpeedManager.Instance.Speed -= 5f;
        
        Vector3 targetScale = new Vector3(_currentScale.x + scaleStep, _currentScale.y + scaleStep, _currentScale.z + scaleStep);
        StopAllCoroutines(); // 중복 방지
        StartCoroutine(ScaleOverTime(carriage, targetScale, scaleTime));
        _currentScale = targetScale;
    }

    private void DowngradeCarriage()
    {
        if (_upgradeCount <= 1) return;

        _upgradeCount--;
        SpeedManager.Instance.Speed += 5f;
        
        Vector3 targetScale = new Vector3(_currentScale.x - scaleStep, _currentScale.y - scaleStep, _currentScale.z - scaleStep);
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(carriage, targetScale, scaleTime));
        _currentScale = targetScale;
    }

    private IEnumerator ScaleOverTime(GameObject target, Vector3 targetScale, float duration)
    {   
        _isScaling = true;
        Vector3 startScale = target.transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.transform.localScale = targetScale;
        _isScaling = false;
    }
}