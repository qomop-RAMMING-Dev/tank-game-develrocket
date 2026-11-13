using UnityEngine;

public class TankFollowCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 2.3f, -5);
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float lookAtHeight = 1f;

    private Vector3 velocity;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("<color=red>Target이 지정되지 않았습니다.</color>");
            enabled = false;
            return;
        }

        Debug.Log($"<color=yellow><b>{target.name}이 연결되었습니다.</b></color>");

        transform.position = target.TransformPoint(offset);
        UpdateCameraRotation();
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 dest = target.TransformPoint(offset);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            dest,
            ref velocity,
            smoothTime
            );

        UpdateCameraRotation();
    }

    private void UpdateCameraRotation()
    {
        Vector3 lookAtPos = target.position;
        lookAtPos.y += lookAtHeight;

        transform.LookAt(lookAtPos);
    }
}
