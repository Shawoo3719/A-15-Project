using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerJumpPad : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 moveDirection;
    public float moveSpeed = 5f;
    public float gravity = 9.81f;
    public float x;
    public float z;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
      

        // 지면에 있는지 확인
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(x, 0, z);
        }
        else
        {
            moveDirection.x = x;
            moveDirection.z = z;
        }

        // 중력 적용
        moveDirection.y -= gravity * Time.deltaTime;

        // 이동 적용
        characterController.Move(moveDirection * Time.deltaTime);
    }

    // 바운스 메소드
    public void Bounce(float bounceForce)
    {
        moveDirection.y = bounceForce;
    }
}
