using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GetScript Get;
    [Space]
    [Range(0, 5)]
    [SerializeField] private float mTime;

    [Header("Camera Offset")]
    [Range(-20, 20)]
    [SerializeField] private float mOffsetX;
    [Range(-20, 20)]
    [SerializeField] private float mOffsetY;
    [Range(-20, 20)]
    [SerializeField] private float mOffsetZ;

    private Vector3 mRef;
    private Vector3 mTarget;
    private float mRefTime;

    void Start() {
        Initialize();
    }

    private void Initialize() {
        transform.position = mTarget = Get.PlayerController.transform.position + GetOffset();
    }

    void FixedUpdate() {
        Controller();
    }

    private void Controller() {
        if (!Get.PlayerController.Stop || Get.PlayerController.CheckTouch && !GetEqual() || Get.PlayerController.Stop && !GetEqual())
            CameraMove();
    }

    private void CameraMove() {
        mRefTime = Get.PlayerController.Down() ? 0 : mTime;
        transform.position = Vector3.SmoothDamp(transform.position, GetTarget(), ref mRef, mRefTime);
    }

    private Vector3 SetTarget(Vector3 extra) {
        Vector3 back = (transform.forward * -1) * (5 * (Get.PlayerController.Value / Get.PlayerController.MaxValue));
        return Get.PlayerController.transform.position + extra + back;
    }

    private Vector3 GetTarget() {
        return SetTarget(GetOffset());
    }

    private bool GetEqual() {
        return Vector3.Distance(transform.position, GetTarget()) < 0.0001f;
    }

    private Vector3 GetOffset() {
        return new Vector3(mOffsetX, mOffsetY, mOffsetZ);
    }
}
