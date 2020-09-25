using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DestroyParticlesOnEnd : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake() => ps = GetComponent<ParticleSystem>();

    private void Update()
    {
        if (!ps.IsAlive())
            Destroy(gameObject);
    }
}
