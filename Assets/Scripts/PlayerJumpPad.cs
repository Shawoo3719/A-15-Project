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
      

        // ���鿡 �ִ��� Ȯ��
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(x, 0, z);
        }
        else
        {
            moveDirection.x = x;
            moveDirection.z = z;
        }

        // �߷� ����
        moveDirection.y -= gravity * Time.deltaTime;

        // �̵� ����
        characterController.Move(moveDirection * Time.deltaTime);
    }

    // �ٿ �޼ҵ�
    public void Bounce(float bounceForce)
    {
        moveDirection.y = bounceForce;
    }
}
