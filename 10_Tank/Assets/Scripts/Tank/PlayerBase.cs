using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    public Color baseColor;

    public float moveSpeed = 1.0f;
    public float rotateSpeed = 360.0f;

    protected bool isAlive = true;
    protected Vector2 inputDir = Vector2.zero;

    protected Rigidbody rigid;

    protected PlayerInputActions inputActions;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
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

    protected void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
    }

    public void DamageTaken(float damage, Vector3 hitDir)
    {

        // 나중에 수정하기
        Die(hitDir * damage * 0.1f);
    }

    public float test = 1.0f;
    public int test2 = 5;
    void Die(Vector3 hitDir)
    {
        if(isAlive)
        {
            isAlive = false;
            inputActions.Disable();

            //Time.timeScale = 0.1f;
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
}