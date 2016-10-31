using UnityEngine;
using System.Collections;

public class TownCenter : Building {
    

    public override void ActiveProduce()
    {
        TutorialManager.ms_instance.FadeOutFirstTutorial();
        base.ActiveProduce();
    }
    
}
