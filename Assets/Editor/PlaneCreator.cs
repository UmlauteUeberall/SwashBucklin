using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PlaneCreator : EditorWindow
{
    private int m_xSegments;
    private int m_zSegments;

    public Material m_PlaneMat;
    public Vector2 m_Size;

    [MenuItem("Window/PlaneCreator")]
    public static void ShowWindow()
    {
        GetWindow<PlaneCreator>();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("X Segments");
        m_xSegments = EditorGUILayout.IntSlider(m_xSegments, 2, 255);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Y Segments");
        m_zSegments = EditorGUILayout.IntSlider(m_zSegments, 2, 255);
        GUILayout.EndHorizontal();

        //GUILayout.BeginHorizontal();
        m_Size = EditorGUILayout.Vector2Field("Size", m_Size);
        //GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Material");
        m_PlaneMat = (Material) EditorGUILayout.ObjectField(m_PlaneMat, typeof(Material), false);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Generate"))
        {
            GeneratePlane();
        }
    }

    private void GeneratePlane()
    {
        GameObject go = new GameObject("Plane");
        MeshFilter filter = go.AddComponent<MeshFilter>();
        MeshRenderer renderer = go.AddComponent<MeshRenderer>();
        MeshCollider col = go.AddComponent<MeshCollider>();

        Mesh mesh = new Mesh();
        mesh.name = "Plane";

        List<Vector3> vertices = new List<Vector3>(m_xSegments * m_zSegments);
        List<Vector2> uvs = new List<Vector2>(vertices.Capacity);

        float xPos, zPos;
        // Vertexbuffer
        for (int x = 0; x < m_xSegments; x++)
        {
            for (int z = 0; z < m_zSegments; z++)
            {
                xPos = x / (m_xSegments - 1f);
                zPos = z / (m_zSegments - 1f);
                vertices.Add(new Vector3(xPos * m_Size.x, 0, zPos * m_Size.y) 
                            - new Vector3(m_Size.x, 0, m_Size.y) * 0.5f);
                uvs.Add(new Vector2(xPos , zPos));
            }
        }

        // Indexbuffer
        List<int> indices = new List<int>((m_xSegments - 1) * (m_zSegments - 1 ) * 6);
        int counter = 0;
        for (int x = 0; x < m_xSegments - 1; x++)
        {
            for (int z = 0; z < m_zSegments - 1; z++)
            {
                indices.Add(counter);
                indices.Add(counter + 1);
                indices.Add(counter + m_zSegments);

                indices.Add(counter + 1);
                indices.Add(counter + m_zSegments + 1);
                indices.Add(counter + m_zSegments);
                counter++;
            }
            counter++;
        }

        // FinalStuff
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = indices.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        filter.mesh = mesh;
        renderer.material = m_PlaneMat;
        col.sharedMesh = mesh;


        go.AddComponent<WaterController>();
    }

}
