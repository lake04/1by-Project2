using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Slider hpBar;

    // Start is called before the first frame update
    void Start()
    {
        ChageHp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChageHp()
    {
        hpBar.value = Player.Instance.curHp / Player.Instance.maxHp;
    }
}
