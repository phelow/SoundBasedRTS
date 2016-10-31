using UnityEngine;
using System.Collections;

public class Armory : Building {

    void Start()
    {
        TutorialManager.ms_instance.UnlockThirdTutorial(transform);
        base.Start();
    }

    public override void ActiveProduce()
    {
        TutorialManager.ms_instance.FadeOutThirdTutorial();
        base.ActiveProduce();
    }
}
