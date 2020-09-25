using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEvents : MonoBehaviour
{
    public void Destroy() => Destroy(gameObject);
}
