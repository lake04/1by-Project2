using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private float mouseInfluence = 0.5f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();

    }

    private void Start()
    {
        if (player == null)
        {
            Player playerComponent = FindObjectOfType<Player>();
            if (playerComponent != null)
            {
                player = playerComponent.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 desiredPosition = Vector3.Lerp(player.position, mouseWorldPos, mouseInfluence);


        desiredPosition.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}