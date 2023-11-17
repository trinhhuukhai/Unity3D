using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFail : UICanvas
{
    private int coin;
    [SerializeField] Text coinTxt;

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Finish);
    }


    public void MainMenuButton()
    {
        LevelManager.Ins.Home();
    }

    internal void SetCoin(int coin)
    {
        this.coin = coin;
        coinTxt.text = coin.ToString();
    }
}
