using UnityEngine;

public class MouseFollowBackground : MonoBehaviour
{
    public float moveAmount = 10f;     // �󸶳� �������� (�ȼ� ����)
    public float smoothSpeed = 2f;     // �󸶳� �ε巴�� ��������

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // ���콺 ��ġ�� ȭ�� ���� �������� (-1, 1) ������ ����ȭ
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;

        // Ÿ�� ��ġ ���
        Vector3 targetPos = new Vector3(mouseX * moveAmount, 0 * moveAmount, 0f) + startPos;

        // �̵� ���� (�ɼ�): �ִ� �̵� ���� ����
        float maxX = moveAmount;
        targetPos.x = Mathf.Clamp(targetPos.x, startPos.x - maxX, startPos.x + maxX);

        // �ε巴�� �̵�
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * smoothSpeed);
    }
}
