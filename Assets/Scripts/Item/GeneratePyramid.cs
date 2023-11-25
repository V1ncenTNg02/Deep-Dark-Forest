using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CreatePyramid : MonoBehaviour
{
    public Material material;
    void Start()
    {
        Mesh mesh = new Mesh();

        // Define the 4 vertices of the tetrahedron
        Vector3[] vertices = {
            new Vector3(0, (2.0f / 3.0f) * Mathf.Sqrt(6.0f), 0),
            new Vector3(0, 0, (2.0f / 3.0f) * Mathf.Sqrt(3.0f)),
            new Vector3(1f, 0, -Mathf.Sqrt(3.0f) / 3.0f),
            new Vector3(-1f, 0,-Mathf.Sqrt(3.0f) / 3.0f)
        };

        // Connect vertices to form the faces of the tetrahedron
        int[] triangles = {
            0, 1, 2,
            0, 2, 3, 
            0, 3, 1,
            1, 3, 2
        };

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0.5f, 1f),
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0.5f)
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals();  // Update normals to ensure proper lighting
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = material;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshCollider>().convex = true;
        
    }
}
