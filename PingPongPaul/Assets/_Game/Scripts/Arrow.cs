using UnityEditor;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    void OnDrawGizmos()
    {

        Handles.color = Color.cyan;

        Handles.ArrowHandleCap(0,
                                transform.position,
                                Quaternion.LookRotation(transform.right, transform.up),
                                1f,
                                EventType.Repaint);
        Handles.color = Color.white;

    }
}
