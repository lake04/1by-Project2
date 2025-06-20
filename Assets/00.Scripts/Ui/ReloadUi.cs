using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadUi : MonoBehaviour
{
    [SerializeField] Vector3 distance = Vector3.up * 20.0f;
    private Transform playerTransform;
    private RectTransform rectTransform;
    private Slider slider;

    private float curAp;
    private float maxAp;


   public void SetUp(Transform taget)
    {
        playerTransform = taget;

        rectTransform = GetComponent<RectTransform>();

        slider = GetComponent<Slider>();    
    }
    private void Update()
    {
        slider.value = curAp / maxAp;
    }

    private void LateUpdate()
    {
        if(playerTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        rectTransform.position = screenPosition + distance;
    }

}
