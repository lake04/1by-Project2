using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : Singleton<GunManager>
{
    public List<Sprite> sprites = new List<Sprite>();
    public List<GunData> gunDatas = new List<GunData>();

    public int maxammo = 100;
    public int curAmmo;


    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    private void Init()
    {
        curAmmo = maxammo;
    }
}
