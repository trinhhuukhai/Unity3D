using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : UICanvas
{
    public Text AliveTxt;
    
    public override void Setup()
    {
        base.Setup();
        UpdateTotalCharacter();
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.GamePlay);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
    }

    public void SettingButton()
    {
        UIManager.Ins.OpenUI<UISetting>();
    }

    public void UpdateTotalCharacter()
    {
        AliveTxt.text = LevelManager.Ins.TotalCharater.ToString();
    }
}
