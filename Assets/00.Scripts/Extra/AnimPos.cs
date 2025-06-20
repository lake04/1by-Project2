using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPos : MonoBehaviour
{
    private Vector3 originalScale;

    private Camera mainCamera;

    void Start()
    {
        originalScale = transform.localScale;

        mainCamera = Camera.main;
    }

    void Update()
    {
   
        if (mainCamera == null) return;

        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

 
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null)
        {
       
            if (hit.collider.CompareTag("Enemy"))
            {
                transform.localScale = new Vector3(0.35f, 0.35f, 1f);
            }
            else
            {
                transform.localScale = originalScale;
            }
        }
        else
        {
            transform.localScale = originalScale;
        }
    }
}