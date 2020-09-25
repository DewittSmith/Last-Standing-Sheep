using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AIController : MonoBehaviour
{
    [SerializeField] private float randomizeRadius;
    [SerializeField] private Vector2 waitRange;
    private CharacterController cc;

    private void Awake() => cc = GetComponent<CharacterController>();

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            Vector3 point = Random.insideUnitSphere * randomizeRadius;
            cc.SetTarget(point);

            yield return new WaitForSeconds(Random.Range(waitRange.x, waitRange.y));
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Vector3.zero, randomizeRadius);
    }
}