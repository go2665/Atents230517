using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : PooledObject
{
    public float explosionRadius = 2.0f;
    public float explosionForce = 10.0f;

    public float firePower = 10.0f;
    public GameObject explosionPrefab;

    public AnimationCurve explotionCurve;

    protected Rigidbody rigid;
    Collider col;

    protected bool isExplosion = false;

    private void Awake()
    {        
        rigid = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    protected virtual void OnEnable()
    {
        rigid.velocity = transform.forward * firePower;
        rigid.angularVelocity = Vector3.zero;
        isExplosion = false;

        StartCoroutine(LifeOver(120.0f));
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explosion(collision.contacts[0].point, collision.contacts[0].normal);
    }

    protected void Explosion(Vector3 pos, Vector3 normal)
    {
        if (!isExplosion)
        {
            //Time.timeScale = 0.00f;
            StopAllCoroutines();

            isExplosion = true;
            Factory.Inst.GetExplosion(pos, normal);

            Collider[] colliders = Physics.OverlapSphere(pos, explosionRadius, LayerMask.GetMask("ExplosionTarget", "Players"));
            if (colliders.Length > 0)
            {
                foreach (Collider collider in colliders)
                {
                    Rigidbody targetRigid = collider.GetComponent<Rigidbody>();
                    if (targetRigid != null)
                    {
                        targetRigid.AddExplosionForce(explosionForce, pos, explosionRadius);
                    }
                    PlayerBase player = collider.GetComponent<PlayerBase>();
                    if (player != null)
                    {
                        Vector3 dir = player.transform.position - pos;
                        float ratio = dir.magnitude / explosionRadius;
                        float damage = explotionCurve.Evaluate(ratio) * explosionForce;

                        dir = dir.normalized * (1 - ratio);
                        player.DamageTaken(damage, dir);
                    }
                }
            }

            OnExplosion();
            StartCoroutine(EndProcess());
        }
    }

    protected virtual void OnExplosion()
    {
    }

    IEnumerator EndProcess()
    {
        yield return new WaitForSeconds(3);

        col.enabled = false;
        rigid.drag = 20.0f;
        rigid.angularDrag = 20.0f;

        yield return new WaitForSeconds(5);

        gameObject.SetActive(false);

        col.enabled = true;
        rigid.drag = 0.0f;
        rigid.angularDrag = 0.05f;
    }
}