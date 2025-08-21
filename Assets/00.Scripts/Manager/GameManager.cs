using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Slider hpBar;
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;
    public int kill;

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
