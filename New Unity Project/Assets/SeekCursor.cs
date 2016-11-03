using UnityEngine;
using System.Collections;

public class SeekCursor : MonoBehaviour {
    Rigidbody2D m_rigidbody;
    [SerializeField]
    private bool m_isLead;
    private float m_thisForce;

    private static Vector3 ms_cursorPosition;
    // Use this for initialization
    void Start () {
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        m_thisForce = Random.Range(10.0f, 15.0f);
        ms_cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
	
	// Update is called once per frame
	void Update () {
        if (m_isLead)
        {
            ms_cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        m_rigidbody.AddForce((ms_cursorPosition - this.transform.position) * m_thisForce);
	}
}
