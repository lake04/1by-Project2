using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class AnimPos : MonoBehaviour
{
    private Vector3 originalScale;

    private Camera mainCamera;

    private float targetSize = 10f;
    private float smoothTime = 0.2f;
    private float velocity = 0f;


    void Start()
    {
        originalScale = transform.localScale;

        mainCamera = Camera.main;
    }

    void Update()
    {
   
        if (mainCamera == null) return;

        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);


        mainCamera.orthographicSize = Mathf.SmoothDamp(mainCamera.orthographicSize, targetSize, ref velocity, smoothTime);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            targetSize = 9.8f;
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
        else
        {
            transform.localScale = originalScale;
            targetSize = 10f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            transform.localScale = originalScale;
            targetSize = 10f;
        }
        
    }
}