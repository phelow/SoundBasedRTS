using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialManager : MonoBehaviour {
    public static TutorialManager ms_instance;
    public bool m_firstTutorialFadedOut = false;
    public bool m_secondTutorialUnlocked = false;
    public bool m_secondTutorialFadedOut = false;
    public bool m_thirdTutorialUnlocked = false;
    public bool m_thirdTutorialFadedOut = false;
    [SerializeField]
    public Text m_firstTutorialText;
    [SerializeField]
    public Text m_secondTutorialText;
    [SerializeField]
    public Text m_thirdTutorialText;
    // Use this for initialization
    void Awake () {
        ms_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator FadeOutTutorialCoroutine(Text fadeOutText)
    {
        float t = 0;
        while(t < 1.0f)
        {
            t += Time.deltaTime;
            fadeOutText.color = Color.Lerp(Color.white, Color.clear, t);
            yield return new WaitForEndOfFrame();
        }
    }

    public void UnlockSecondTutorial(Transform tutorialLocation)
    {
        if(m_secondTutorialUnlocked == true)
        {
            return;
        }
        m_secondTutorialUnlocked = true;

        m_secondTutorialText.transform.position = new Vector3(tutorialLocation.position.x, tutorialLocation.position.y,0);
    }

    public void UnlockThirdTutorial(Transform tutorialLocation)
    {
        if (m_thirdTutorialUnlocked == true)
        {
            return;
        }
        m_thirdTutorialUnlocked = true;

        m_thirdTutorialText.transform.position = new Vector3(tutorialLocation.position.x, tutorialLocation.position.y, 0);
    }

    public void FadeOutFirstTutorial()
    {
        if(m_firstTutorialFadedOut == true)
        {
            return;
        }
        m_firstTutorialFadedOut = true;

        StartCoroutine(FadeOutTutorialCoroutine(m_firstTutorialText));
    }

    public void FadeOutSecondTutorial()
    {
        if (m_secondTutorialFadedOut == true)
        {
            return;
        }
        m_secondTutorialFadedOut = true;

        StartCoroutine(FadeOutTutorialCoroutine(m_secondTutorialText));
    }

    public void FadeOutThirdTutorial()
    {
        if (m_thirdTutorialFadedOut == true)
        {
            return;
        }
        m_thirdTutorialFadedOut = true;

        StartCoroutine(FadeOutTutorialCoroutine(m_thirdTutorialText));
    }

}
