using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController cc;
    private FixedJoystick joystick;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        joystick = FindObjectOfType<FixedJoystick>();
    }

    private void Update()
    {
        Vector3 pos = new Vector3(joystick.Direction.x, 0, joystick.Direction.y).normalized * 16 + cc.transform.position;
        cc.SetTarget(pos);
    }
}