using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class WolfAIController : MonoBehaviour
{
    [HideInInspector] public GameManager GameManager;
    [HideInInspector] public float Lifetime;
    [SerializeField] private float elevationCutoff = 5f;
    private List<GameObject> targetSheeps;
    private CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        Messenger.AddListener("Eat", Sort);
    }

    private IEnumerator Start()
    {
        Sort();
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
    }

    private void Sort()
    {
        List<GameObject> sheeps = GameManager.GetSheeps();
        sheeps.Sort((i, j) => Vector3.Distance(transform.position, i.transform.position).CompareTo(Vector3.Distance(transform.position, j.transform.position)));
        targetSheeps = sheeps.Where(i => i.transform.position.y < elevationCutoff).ToList();
    }

    private void Update()
    {
        if (targetSheeps.Count > 0)
        {
            if (targetSheeps[0])
                cc.SetTarget(targetSheeps[0].transform.position);
            else targetSheeps.RemoveAt(0);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (targetSheeps.Contains(col.gameObject))
        {
            GameManager.RemoveSheep(col.gameObject);
            Messenger.Broadcast("Eat");
        }
    }
}
