using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [Header("Scroll Settings")] 
    [SerializeField] private float resetX = 70f; // 되돌릴 X 위치

    [SerializeField] private float thresholdX = -70f; // 임계값 (이 좌표보다 작아지면 리셋)

    private Transform _tf;

    private void Awake()
    {
        _tf = transform;
    }

    private void Update()
    {
        if (!SpeedManager.Instance) return;

        // SpeedManager의 속도값 가져오기
        float speed = SpeedManager.Instance.Speed;

        // 왼쪽으로 이동 (deltaTime 적용)
        _tf.Translate(Vector3.left * ((speed - 7) * Time.deltaTime));

        // x 좌표가 임계값보다 작아졌으면 resetX로 순간이동
        if (_tf.position.x <= thresholdX)
        {
            Vector3 pos = _tf.position;
            pos.x = resetX;
            _tf.position = pos;
        }
    }
}