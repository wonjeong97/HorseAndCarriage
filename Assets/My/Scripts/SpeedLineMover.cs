using UnityEngine;

public class SpeedLineMover : MonoBehaviour
{
    private float _life;
    private float _moveSpeed;
    private RectTransform _rt;
    private RectTransform _canvas;

    public void Init(float speed, float life, RectTransform canvas)
    {
        _life = life;
        _moveSpeed = speed * 20f; // 속도 기반 이동
        _rt = GetComponent<RectTransform>();
        _canvas = canvas;

        if (_rt == null)
        {
            Debug.LogError("[SpeedLineMover] RectTransform이 필요합니다.");
            Destroy(gameObject);
            return;
        }

        Destroy(gameObject, _life);
    }

    private void Update()
    {
        if (_rt == null) return;

        // 좌측 이동
        _rt.anchoredPosition += Vector2.left * (_moveSpeed * Time.deltaTime);

        // 화면 왼쪽 끝을 벗어나면 제거
        if (_rt.anchoredPosition.x < -(_canvas.rect.width / 2f) - 50f)
        {
            Destroy(gameObject);
        }
    }
}