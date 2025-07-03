using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletUi : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;


    void Start()
    {
        
    }

    void Update()
    {
        currnetBulletUi();
    }

    public void currnetBulletUi()
    {
        ammoText.text = Player.Instance.gun.curAmmo.ToString() +"/"+ Player.Instance.gun.maxAmmo.ToString();
    }
}
