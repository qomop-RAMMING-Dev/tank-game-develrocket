using UnityEditor.TerrainTools;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class EnemyVisionCone : MonoBehaviour
{
    [Header("Vision Settings")]
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float detectionAngle = 45f;
    [SerializeField] private int segments = 30;

    [Header("Vision Colors")]
    [SerializeField] private Color normalColor =
        new Color(1f, 0.8f, 0f, 0.25f);

    [SerializeField] private Color detectedColor =
        new Color(1f, 0f, 0f, 0.35f);

    private Mesh mesh;
    private MeshRenderer meshRenderer;
    private Material visionMaterial;

    private void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Enemy Vision Cone";

        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();

        visionMaterial = new Material(
            Shader.Find("Universal Render Pipline/Unlit")
            );

        visionMaterial.color = normalColor;
        meshRenderer.material = visionMaterial;

        GenerateMesh();
    }

    private void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        // 부채꼴 중심점
        vertices[0] = Vector3.zero;

        float halfAngle = detectionAngle;

        // 부채꼴 끝점 생성
        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfAngle +
                (2f * halfAngle * i / segments);

            float radian = angle * Mathf.Deg2Rad;

            vertices[i + 1] = new Vector3(
                Mathf.Sin(radian) * detectionRange,
                0f,
                Mathf.Cos(radian) * detectionRange
                );
        }

        // 삼각형 연결
        for (int i = 0; i < segments; i++)
        {
            int index = i * 3;

            triangles[index] = 0;
            triangles[index + 1] = i + 1;
            triangles[index + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public void SetDetected(bool detected)
    {
        visionMaterial.color =
            detected ? detectedColor : normalColor;
    }

    public void SetVisionSettings(float range, float angle)
    {
        detectionRange = range;
        detectionAngle = angle;

        GenerateMesh();
    }
}
