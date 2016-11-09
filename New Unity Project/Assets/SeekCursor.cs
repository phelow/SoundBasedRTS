using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SeekCursor : MonoBehaviour {
    Rigidbody2D m_rigidbody;
    [SerializeField]
    private bool m_isLead;

    [SerializeField]
    private Image m_image;

    private float m_thisForce;
    [SerializeField]
    private bool fixToCursor;

    private static Vector3 ms_cursorPosition;
    // Use this for initialization
    void Start () {
        Cursor.visible = false;

        m_rigidbody = this.GetComponent<Rigidbody2D>();
        m_thisForce = Random.Range(10.0f, 15.0f);
        ms_cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (fixToCursor)
        {
            StartCoroutine(InterpolateCursor());
        }
    }
	
    private IEnumerator InterpolateCursor()
    {
        while (true)
        {
            float t;
            float totalTime = .2f;
            t = totalTime;
            while(t > 0.0f)
            {
                t -= Time.deltaTime;
                m_image.color = Color.Lerp(Color.green, Color.clear, t / totalTime);
                yield return new WaitForEndOfFrame();
            }
            t = totalTime;
            while (t > 0.0f)
            {
                t -= Time.deltaTime;
                m_image.color = Color.Lerp(Color.clear, Color.green, t / totalTime);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (m_isLead)
        {
            ms_cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (fixToCursor)
        {
            this.transform.position = new Vector2( ms_cursorPosition.x,ms_cursorPosition.y);
        }

        m_rigidbody.AddForce((ms_cursorPosition - this.transform.position) * m_thisForce);
	}
}
