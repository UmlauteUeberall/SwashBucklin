using UnityEngine;
using System.Collections;

public class WaterController : MonoBehaviour
{
    private Renderer m_Renderer;
//    public Vector2 m_WaterTextureSpeed = new Vector2(0.01f, 0f);
    public Vector2 m_WaterHM1Speed = new Vector2(0.05f, 0.02f);
    public Vector2 m_WaterHM2Speed = new Vector2(0.02f, 0.03f);

//    private Vector2 mi_waterTextureOffset;
    private Vector2 mi_water1Offset;
    private Vector2 mi_water2Offset;


	// Use this for initialization
	void Awake ()
    {
        m_Renderer = GetComponent<Renderer>();
//        mi_waterTextureOffset = m_Renderer.material.GetTextureOffset("_MainTex");
        mi_water1Offset = m_Renderer.material.GetTextureOffset("_WaterHM1");
        mi_water2Offset = m_Renderer.material.GetTextureOffset("_WaterHM2");

    }
	
	// Update is called once per frame
	void Update ()
    {
//        mi_waterTextureOffset += Time.deltaTime * m_WaterTextureSpeed;
        mi_water1Offset += Time.deltaTime * m_WaterHM1Speed;
        mi_water2Offset += Time.deltaTime * m_WaterHM2Speed;

//        m_Renderer.material.SetTextureOffset("_MainTex", mi_waterTextureOffset);
        m_Renderer.material.SetTextureOffset("_WaterHM1", mi_water1Offset);
        m_Renderer.material.SetTextureOffset("_WaterHM2", mi_water2Offset);
	}
}
