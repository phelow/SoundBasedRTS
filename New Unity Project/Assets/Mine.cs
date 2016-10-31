using UnityEngine;
using System.Collections;

public class Mine : Building {

    void Start()
    {
        TutorialManager.ms_instance.UnlockSecondTutorial(transform);
        base.Start();
    }
    
    public override void ActiveProduce()
    {
        TutorialManager.ms_instance.FadeOutSecondTutorial();
        base.ActiveProduce();
    }
}
