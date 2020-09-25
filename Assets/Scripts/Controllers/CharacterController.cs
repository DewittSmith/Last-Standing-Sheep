using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] private float targetMinDistance;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float crashForce, crashTime;
    [SerializeField] private ParticleSystem crashParticles;

    [Range(0, 1)]
    [SerializeField] private float rotateSpeed;

    [SerializeField] private float sineSpeed;
    [SerializeField] private float sineAmplitude;    

    private Vector3 target;

    private Rigidbody rb;
    private Transform child;

    private float startY, curAng;
    private float dist;

    private Vector2 delta2D;

    public void SetTarget(Vector3 target) => this.target = target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        child = transform.GetChild(0);
        startY = child.localPosition.y;
    }

    private void Start()
    {
        target = transform.position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(target, targetMinDistance);
        Gizmos.DrawRay(transform.position, transform.forward * 10);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, delta2D * 10);
    }

    private void Update()
    {
        Vector3 dt = target - transform.position;
        delta2D = new Vector2(dt.x, dt.z).normalized;
        transform.forward = Vector3.Lerp(transform.forward, new Vector3(delta2D.x, 0, delta2D.y), rotateSpeed);

        if (curAng < 360)
            curAng += rb.velocity.magnitude * Time.deltaTime;
        else curAng = 0;

        dist = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.x, target.z));

        if (dist > targetMinDistance && crash == null)
            child.localPosition = Vector3.up * (Mathf.Abs(Mathf.Sin(curAng * sineSpeed)) * sineAmplitude + startY);
        else
            child.localPosition = Vector3.Lerp(child.localPosition, Vector3.up * startY, rotateSpeed);

    }

    Coroutine crash;
    private void FixedUpdate()
    {
        if (crash != null) return;

        if (dist <= targetMinDistance)
        {
            rb.velocity = Vector3.up * rb.velocity.y;
            return;
        }

        rb.velocity = new Vector3(delta2D.x, 0, delta2D.y) * moveSpeed * Time.fixedDeltaTime + Vector3.up * rb.velocity.y;
    }

    private IEnumerator Crash(Vector3 direction)
    {
        Vector3 startDir = direction * crashForce;
        for (float t = 0; t < crashTime; t += Time.deltaTime)
        {
            rb.velocity = Vector3.Lerp(startDir, Vector3.zero, t / crashTime);
            yield return null;
        }
        crash = null;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag != "Player") return;

        crash = StartCoroutine(Crash((transform.position - col.transform.position).normalized));
        Instantiate(crashParticles.gameObject, (transform.position + col.transform.position) * .5f, Quaternion.identity);
    }
}