using System.Collections.Generic;
using UnityEngine;

public class DestroyInside : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private List<string> destroyableTags;

    private void OnTriggerEnter(Collider col)
    {
        if (destroyableTags.Contains(col.tag))
        {
            if (col.GetComponent<CharacterController>())
                gm.RemoveSheep(col.gameObject);
            else Destroy(col.gameObject);
        }
    }
}