using UnityEngine;
using UnityEngine.UI;

public class SpeedLineSpawner : MonoBehaviour
{
    [Header("Line Settings")]
    [SerializeField] private GameObject linePrefab;        // UI Image 기반 라인 프리팹
    [SerializeField] private RectTransform parentCanvas;   // Canvas RectTransform

    [Header("Spawn Control")]
    [SerializeField] private float minInterval = 0.5f;
    [SerializeField] private float maxInterval = 0.05f;
    [SerializeField] private float lineLife = 2f;

    private float _timer;

    private void Update()
    {
        if (SpeedManager.Instance == null) return;

        float speed = SpeedManager.Instance.Speed;
        float interval = Mathf.Lerp(minInterval, maxInterval, Mathf.InverseLerp(10f, 90f, speed));

        _timer += Time.deltaTime;
        if (_timer >= interval)
        {
            _timer = 0f;
            SpawnLine(speed);
        }
    }

    private void SpawnLine(float speed)
    {
        GameObject line = Instantiate(linePrefab, parentCanvas);
        RectTransform rt = line.GetComponent<RectTransform>();

        if (rt == null)
        {
            Debug.LogError("[SpeedLineSpawner] 라인 프리팹에는 RectTransform이 필요합니다.");
            Destroy(line);
            return;
        }

        // 캔버스 크기 가져오기
        float canvasWidth = parentCanvas.rect.width;
        float canvasHeight = parentCanvas.rect.height;

        // 시작 위치 (화면 우측 바깥)
        float startX = canvasWidth / 2 + 50f;
        float randomY = Random.Range(-canvasHeight / 2f, canvasHeight / 2f);

        rt.anchoredPosition = new Vector2(startX, randomY);

        // 이동 스크립트 붙이기
        var mover = line.AddComponent<SpeedLineMover>();
        mover.Init(speed, lineLife, parentCanvas);
    }
}