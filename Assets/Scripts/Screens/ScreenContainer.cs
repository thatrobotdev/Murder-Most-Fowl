using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ScreenContainer : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera m_VirtualCamera;
    [SerializeField]
    private PolygonCollider2D m_CameraBounds;

    // Start is called before the first frame update
    void Start()
    {
        FixColliderCorner();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixColliderCorner()
    {
        Vector2 maxCorner = Vector2.negativeInfinity;
        Vector2 minCorner = Vector2.positiveInfinity;
        foreach (Vector2 corner in m_CameraBounds.points)
        {
            maxCorner = Vector2.Max(corner, maxCorner);
            minCorner = Vector2.Min(corner, minCorner);
        }
        m_CameraBounds.points = new Vector2[] {maxCorner, new Vector2(maxCorner.x, minCorner.y), minCorner, new Vector2(minCorner.x, maxCorner.y)};
    }
}
