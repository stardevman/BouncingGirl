using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GetScript Get;

    [Header("Cylinder About")]
    [SerializeField] private Transform mCylinder;
    [Range(0, 100)]
    [SerializeField] private float mCylinderRotateSpeed;

    [Header("About Throwing")]
    public float Value;
    [Range(0, 100)]
    [SerializeField] private float mValueSpeed;
    [Range(0, 100)]
    [SerializeField] private float mPower;
    [Range(0, 5)]
    [SerializeField] private float mTimerLimit;

    [Header("About Finish")]
    public Transform finishPoint;
    public float moveSpeed;
    public bool finish;
    public ParticleSystem confeti1;
    public ParticleSystem confeti2;
    public GameObject sunlight;
    [Space]

    [Header("About Progress")]
    public Transform progress;
    private float maxDistance;
    [Space]

    public bool Stop;
    public bool CheckTouch;

    private Animator PlayerAnimator;
    [HideInInspector] public Rigidbody mPlayerRigidbody;

    [HideInInspector] public float MaxValue;
    private bool mAddForceOnce;

    private bool mTimerStart;
    private bool mRotateCheck;
    private float mTimer;

    //private float mFirstSaveY;
    private string StrechingAnimName;
    private string JumpAnimName;

    void Start() {
        Initialize();
    }

    private void Initialize() {
        StrechingAnimName = "Streching";
        JumpAnimName = "Jump";
        Stop = true;
        MaxValue = 30;
        mTimer = 0;
        maxDistance = finishPoint.position.y - transform.position.y;
        //  mFirstSaveY = transform.position.y;
        mPlayerRigidbody = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
    }

    void Update() {
        if (finish)
            return;

        Controller();

        progress.localScale = new Vector3(Mathf.Clamp(transform.position.y / maxDistance, 0.08f, 1), 1, 1);
    }

    private void Controller() {
        TouchController();

        if (transform.position.y < 0.1f && !Stop && !mAddForceOnce) FallOnTheGround();
        // if ((transform.position.y - mFirstSaveY) < float.Epsilon && !Stop && !mAddForceOnce) FallOnTheGround();
    }

    private void TouchController() {
        if (mTimerStart)
        {
            if (mTimer < mTimerLimit) mTimer += Time.deltaTime;
            else { mRotateCheck = true; mTimer = 0; mTimerStart = false; }
        }

        if (Input.touchCount > 0)
        {
            Touch mTouch = Input.GetTouch(0);

            if (Stop)
            {
                if (mTouch.phase == TouchPhase.Began)
                {
                    CheckTouch = true;
                    PlayerAnimator.SetBool("idle", false);
                    PlayerAnimator.SetBool("stretching", false);
                }
                else if (mTouch.phase == TouchPhase.Ended)
                {
                    CheckTouch = false;

                    if (Value > 10) PlayerAnimator.SetBool("jump", true);
                    else if (Value <= 10)
                    {
                        PlayerAnimator.SetBool("idle", true);
                        Value = 0;
                    }
                }
                if (mTouch.deltaPosition.y != 0) Value = Mathf.Clamp(Value += Time.deltaTime * mValueSpeed * (mTouch.deltaPosition.y * -1), 0, MaxValue);

                float time = Value / MaxValue;
                PlayerAnimator.Play(StrechingAnimName, 0, time);
            }
            if (mRotateCheck)
            {
                if (mTouch.deltaPosition.normalized.x != 0) mCylinder.Rotate(new Vector3(0, mTouch.deltaPosition.normalized.x, 0) * 10 * -mCylinderRotateSpeed * Time.deltaTime);
            }
        }

        if (PlayerAnimator.GetCurrentAnimatorStateInfo(0).IsName(JumpAnimName) && PlayerAnimator.GetBool("jump"))
        {
            PlayerAnimator.SetBool("jump", false);
            PlayerAnimator.SetBool("fly", true);

            mPlayerRigidbody.isKinematic = false;
            mAddForceOnce = true;
            Stop = false;
            mTimerStart = true;
        }
    }

    /*
    private void Ray() {
        if (Down())
        {
            PlayerAnimator.SetBool("fly", false);
            PlayerAnimator.SetBool("falling", true);

            bool result = Physics.Raycast(mRayOrigin1.position, mRayOrigin1.forward, out hit, 1) ? true : Physics.Raycast(mRayOrigin2.position, mRayOrigin2.forward, out hit, 1) ? true : Physics.Raycast(mRayOrigin3.position, mRayOrigin3.forward, out hit, 1);

            if (result)
            {
                if (hit.collider.gameObject.tag == "hold")
                {
                    PlayerStopped();
                    transform.position = new Vector3(hit.point.x, hit.point.y, transform.position.z);

                    PlayerAnimator.SetBool("idle", true);
                    PlayerAnimator.SetBool("fly", false);
                    PlayerAnimator.SetBool("jump", false);
                    PlayerAnimator.SetBool("stretching", false);
                    PlayerAnimator.SetBool("falling", false);
                }

                //Debug.DrawLine(mRayOrigin.position, hit.point, Color.red);
            }
           
        } 
    }*/

    private void PlayerStopped() {
        mPlayerRigidbody.isKinematic = true;
        Stop = true;
        mTimerStart = false;
        mRotateCheck = false;
        Value = 0;
    }

    private void MoveFinishArea() {
        Vector3 newPos = Vector3.MoveTowards(transform.position, finishPoint.position, moveSpeed);
        mPlayerRigidbody.MovePosition(newPos);
    }

    private void StartConfeti() {
        confeti1.Play();
        confeti2.Play();

        sunlight.SetActive(true);
    }


    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag != "hold")
            return;

        if (Down())
        {
            PlayerStopped();

            BoxCollider collider = col.gameObject.GetComponent<BoxCollider>();

            Vector3 target = new Vector3(collider.bounds.center.x, col.transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, 5);

            PlayerAnimator.SetBool("idle", true);
            PlayerAnimator.SetBool("fly", false);
            PlayerAnimator.SetBool("jump", false);
            PlayerAnimator.SetBool("stretching", false);
            PlayerAnimator.SetBool("falling", false);
        }
    }

    public void FallOnTheGround() {
        mPlayerRigidbody.isKinematic = true;

        PlayerAnimator.SetBool("falling", false);
        PlayerAnimator.SetBool("fall", true);

        // transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        Get.GameManager.FailedLevel();

        enabled = false;
    }

    public void FallBack() {
        mPlayerRigidbody.velocity = Vector3.down;
    }

    public bool Down() {
        return mPlayerRigidbody.velocity.normalized.y < 0 ? true : false;
    }

    public bool Move() {
        return mPlayerRigidbody.velocity.normalized.y != 0 ? true : false;
    }

    public void Finish() {
        Get.GameManager.CompleteLevel();

        finish = true;

        mPlayerRigidbody.constraints = RigidbodyConstraints.None;
        mPlayerRigidbody.isKinematic = true;
        PlayerAnimator.SetBool("stay", true);
        PlayerAnimator.SetInteger("dance", Random.Range(0, 2));

        Invoke("StartConfeti", 0.5f);
    }

    void FixedUpdate() {
        if (finish)
            MoveFinishArea();

        //    Ray();

        if (Down())
        {
            PlayerAnimator.SetBool("fly", false);
            PlayerAnimator.SetBool("falling", true);
        }

        if (mAddForceOnce)
        {
            mPlayerRigidbody.AddForce(Vector3.up * mPower * Value);
            Value = 0;
            mAddForceOnce = false;
        }
    }
}
