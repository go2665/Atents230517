using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    public float fullChargeTime = 3.0f;
    public Color baseColor;

    public float moveSpeed = 1.0f;
    public float rotateSpeed = 360.0f;

    public ShellType shellType = ShellType.Normal;

    protected bool isAlive = true;
    protected Vector2 inputDir = Vector2.zero;


    protected Rigidbody rigid;
    protected Slider aimSlider;

    protected PlayerInputActions inputActions;

    GameObject explosionEffect;

    bool isCharging = false;
    float chargingRate = 0;

    Transform fireTransform;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        Canvas canvas = GetComponentInChildren<Canvas>();
        Transform child = canvas.transform.GetChild(0);
        aimSlider = child .GetComponent<Slider>();

        inputActions = new PlayerInputActions();

        child = transform.GetChild(0);
        child = child.GetChild(3);
        fireTransform = child.GetChild(0);
        explosionEffect = transform.GetChild(1).gameObject;

    }
    private void Start()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.material.SetColor("_BaseColor", baseColor);
        }
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(transform.position + Time.fixedDeltaTime * moveSpeed * inputDir.y * transform.forward);
        rigid.MoveRotation(
            Quaternion.Euler(0, Time.fixedDeltaTime * rotateSpeed * inputDir.x, 0) * transform.rotation);
    }

    public void DamageTaken(float damage, Vector3 hitDir)
    {

        // 나중에 수정하기
        Die(hitDir * damage * 0.1f);
    }

    void Die(Vector3 hitDir)
    {
        if(isAlive)
        {
            //Time.timeScale = 0.01f;
            isAlive = false;
            explosionEffect.transform.SetParent(null);
            explosionEffect.SetActive(true);
            inputActions.Disable();

            //Debug.Log($"{hitDir},{hitDir.sqrMagnitude}");
            Vector3 explosionDir = hitDir + transform.up * hitDir.sqrMagnitude * 2;
            rigid.constraints = RigidbodyConstraints.None;

            Vector3 torqueAxis = Quaternion.Euler(0, Random.Range(80.0f,100.0f), 0) * explosionDir;
            //Vector3 torqueAxis = Quaternion.Euler(0, 90.0f, 0) * hitDir;

            rigid.AddForce(explosionDir, ForceMode.Impulse);
            rigid.AddTorque(torqueAxis, ForceMode.VelocityChange);            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!isAlive && !collision.gameObject.CompareTag("Shell"))
        {
            StartCoroutine(EndProcess());
        }
    }

    IEnumerator EndProcess()
    {
        rigid.angularDrag = 3.0f;
        yield return new WaitForSeconds(3);

        Collider col = GetComponent<Collider>();
        col.enabled = false;
        rigid.drag = 10.0f;

        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }    

    IEnumerator Charging()
    {
        isCharging = true;
        float timeElapsed = 0;
        while (timeElapsed < fullChargeTime)
        {
            timeElapsed += Time.deltaTime;
            chargingRate = timeElapsed / fullChargeTime;
            aimSlider.value = timeElapsed;
            yield return null;
        }

        Fire();
    }

    private void Fire()
    {
        if (isCharging)
        {
            aimSlider.value = 0;
            Factory.Inst.GetShell(shellType, fireTransform, chargingRate);
        }
        isCharging = false;
        chargingRate = 0.0f;
    }

    protected void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
    }

    protected void OnChargeStart(InputAction.CallbackContext context)
    {
        StartCoroutine(Charging());
    }

    protected void OnFire(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
        Fire();
    }
}