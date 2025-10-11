using UnityEngine;

[ExecuteInEditMode]
public class GridDrawer : MonoBehaviour
{
    public int size = 10;     // 그리드 총 크기
    public float spacing = 1; // 간격
    public Color color = Color.gray;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        for ( float x = -size; x <= size; x += spacing )
        {
            Gizmos.DrawLine(new Vector3(x, 0, -size), new Vector3(x, 0, size));
        }
        for ( float z = -size; z <= size; z += spacing )
        {
            Gizmos.DrawLine(new Vector3(-size, 0, z), new Vector3(size, 0, z));
        }
    }
}
