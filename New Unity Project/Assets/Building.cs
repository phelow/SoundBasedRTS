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

    private float m_timeToSpawn = 1.0f;

    // Use this for initialization
    protected void Start () {
        m_originalColor = m_image.color;
	}
	
	// Update is called once per frame
	void Update () {

    }

    protected void Produce()
    {
        if(m_timeToSpawn > 0.0f)
        {
            m_timeToSpawn -= Time.deltaTime;
            return;
        }

        m_timeToSpawn = m_spawnTime;
        GameObject.Instantiate(m_spawnObject, transform.position, transform.rotation, null);
    }

    protected void SetPassiveImage()
    {
        m_image.color = Color.Lerp(m_image.color, m_originalColor, Time.deltaTime);
    }

    protected void SetActiveImage()
    {
        m_image.color = Color.Lerp(m_image.color, m_selectedColor, Time.deltaTime);
    }

    public virtual void ActiveProduce()
    {
        SetActiveImage();
        Produce();
        Produce();
        Produce();
        Produce();
        Produce();
    }

    public virtual void PassiveProduce()
    {
        SetPassiveImage();
        Produce();
    }
}
