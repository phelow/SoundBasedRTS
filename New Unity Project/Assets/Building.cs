using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Building : MonoBehaviour {
    [SerializeField]
    private Image m_image;
    [SerializeField]
    private Color m_originalColor;
    [SerializeField]
    private Color m_selectedColor;
    [SerializeField]
    private float m_spawnTime;
    [SerializeField]
    private GameObject m_spawnObject;

    private bool m_triggered = false;
    private bool m_quadTriggered = false;

    private float m_timeToSpawn = 1.0f;

    // Use this for initialization
    protected void Start () {
        m_originalColor = m_image.color;
	}
	
	// Update is called once per frame
	void Update () {

    }

    protected bool Produce()
    {
        if(m_quadTriggered)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject.Instantiate(m_spawnObject, transform.position, transform.rotation, null);
            }
        }

        if (m_triggered)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject.Instantiate(m_spawnObject, transform.position, transform.rotation, null);

            }
            return true;
        }
        GameObject.Instantiate(m_spawnObject, transform.position, transform.rotation, null);
        return false;
    }

    protected void SetPassiveImage()
    {
        m_image.color = Color.Lerp(m_image.color, m_originalColor, .5f);
    }

    protected void SetActiveImage()
    {
        m_image.color = Color.Lerp(m_image.color, m_selectedColor, .5f);
    }


    public virtual void ActiveProduce()
    {
        m_image.color = Color.Lerp(m_image.color, m_selectedColor, .8f);
        m_triggered = true;
    }

    public virtual void QuadrupleProduce()
    {
        m_quadTriggered = true;
    }

    public virtual bool PassiveProduce()
    {
        SetPassiveImage();
        return Produce();
    }
}
