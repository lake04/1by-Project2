using UnityEngine;

public class MouseFollowBackground : MonoBehaviour
{
    public float moveAmount = 10f;     // 얼마나 움직일지 (픽셀 단위)
    public float smoothSpeed = 2f;     // 얼마나 부드럽게 움직일지

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // 마우스 위치를 화면 비율 기준으로 (-1, 1) 범위로 정규화
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;

        // 타겟 위치 계산
        Vector3 targetPos = new Vector3(mouseX * moveAmount, 0 * moveAmount, 0f) + startPos;

        // 이동 제한 (옵션): 최대 이동 범위 설정
        float maxX = moveAmount;
        targetPos.x = Mathf.Clamp(targetPos.x, startPos.x - maxX, startPos.x + maxX);

        // 부드럽게 이동
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smoothSpeed);
    }
}
