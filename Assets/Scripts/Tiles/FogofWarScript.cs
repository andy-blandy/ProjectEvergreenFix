using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogofWarScript : MonoBehaviour
{
    public GameObject m_fogOfWarPlane;
//	public Transform m_player;
	public LayerMask m_fogLayer;
	public float m_radius = 5f;
	private float m_radiusSqr { get { return m_radius*m_radius; }}
	
	private Mesh m_mesh;
	private Vector3[] m_vertices;
	private Color[] m_colors;
	private Camera gameCamera;
	
	// Use this for initialization
	void Start ()
	{
		gameCamera = GameManager.instance.gameCamera;
		Initialize();
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit rayHit;
		if (Input.GetMouseButtonDown(0))
		{
			if (Physics.Raycast(ray, out rayHit, 1000, m_fogLayer, QueryTriggerInteraction.Collide)) {
			for (int i=0; i< m_vertices.Length; i++) {
				Vector3 v = m_fogOfWarPlane.transform.TransformPoint(m_vertices[i]);
				float dist = Vector3.SqrMagnitude(v - rayHit.point);
				if (dist < m_radiusSqr) {
					float alpha = Mathf.Min(m_colors[i].a, dist/m_radiusSqr);
					m_colors[i].a = alpha;
				}
			}
			UpdateColor();
			}
		}
	}

	public Vector3 GetMouseWorldPosition()
    {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
//		RaycastHit rayHit;
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        } else
        {
            return Vector3.zero;
        }
    }
	
	void Initialize() {
		m_mesh = m_fogOfWarPlane.GetComponent<MeshFilter>().mesh;
		m_vertices = m_mesh.vertices;
		m_colors = new Color[m_vertices.Length];
		for (int i=0; i < m_colors.Length; i++) {
			m_colors[i] = Color.black;
		}
		UpdateColor();
	}
	
	void UpdateColor() {
		m_mesh.colors = m_colors;
	}
}
