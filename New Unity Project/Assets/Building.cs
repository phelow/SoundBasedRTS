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

    private float m_yield = 1.0f;

    // Use this for initialization
    protected void Start() {
        m_originalColor = m_image.color;
    }

    // Update is called once per frame
    void Update() {

    }

    public void ResetYield()
    {
        SetYield(Random.Range(.1f, 3.0f));
    }

    public void SetYield(float yield)
    {
        m_yield = yield;
    }

    public float GetYield()
    {
        return m_yield;
    }

    protected bool Produce()
    {
        if(m_quadTriggered)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject.Instantiate(m_spawnObject, transform.position, transform.rotation, null);
                
            }
        }

        if (m_triggered)
        {
            for (int i = 0; i < (int)Mathf.Lerp(10,100, m_yield/10.0f); i++)
            {
                (GameObject.Instantiate(m_spawnObject, transform.position, transform.rotation, null) as GameObject).GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)).normalized * m_yield);
                
            }
            return true;
        }

        if (Random.Range(0, 10) < 4)
        {
            GameObject.Instantiate(m_spawnObject, transform.position, transform.rotation, null);
        }
        return false;
    }

    public void SetPassiveImage()
    {
        m_image.color = Color.Lerp(m_image.color, m_originalColor, .5f);
    }

    public void SetActiveImage()
    {
        m_image.color = Color.Lerp(m_image.color, m_selectedColor, .9f);
    }


    public virtual void ActiveProduce()
    {
        m_image.color = Color.Lerp(m_image.color, m_selectedColor, .8f);
        m_triggered = true;
    }

    public virtual void QuadrupleProduce()
    {
        if (m_triggered)
        {
            return;
        }

        m_image.color = Color.Lerp(m_image.color, m_selectedColor, .6f);
        
        m_quadTriggered = true;
    }

    public virtual bool PassiveProduce()
    {
        SetPassiveImage();
        return Produce();
    }
}
