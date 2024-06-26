using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// �÷��̾��� �������� �����ϴ� ��ũ��Ʈ
public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public AudioSource mySfx;
    public AudioClip jumpSfx;

    Animator _animator; // �ִϸ����� ������Ʈ�� �����ϴ� �����Դϴ�.
    Camera _camera; // ���� ī�޶� �����ϴ� �����Դϴ�.
    CharacterController _controller; // ĳ���� ��Ʈ�ѷ��� �����ϴ� �����Դϴ�.
    Rigidbody _rb; // ������ٵ� ������Ʈ�� �����ϴ� �����Դϴ�.

    // ������Ʈ�� ǥ�� �� �Լ��� public����, ǥ������ ���� ���̶�� private�� �����մϴ�.
  
    [Header("Behavior Check")]
    public bool cameraRotation; // ī�޶� ȸ�� ��� �����Դϴ�.
    public bool run; // �޸��� �����Դϴ�.
    bool isGrounded;
    
  
    [Header("Movement")]
    public float speed = 3f; // �ȴ� �ӵ��� ��Ÿ���� �����Դϴ�.
    public float runSpeed = 6f; // �ٴ� �ӵ��� ��Ÿ���� �����Դϴ�.
    public float finalSpeed; // ���� �̵� �ӵ��� ��Ÿ���� �����Դϴ�.
    public float cameraSmoothness = 10f; // ȸ���� �ε巯�� ������ ��Ÿ���� �����Դϴ�.
    public float gravity = -9.1f;
    public float jumpHeight = 1f;
    

    [Header("Ground Check")]
    public Transform ground_check;
    public float ground_distance = 0.4f;
    public LayerMask ground_mask;


    private Vector3 velocity; // �÷��̾��� �ӵ��� ��Ÿ���� �����Դϴ�.



    // ���� ������Ʈ�� Ȱ��ȭ�� �� ȣ��Ǵ� �Լ��Դϴ�.
    void Start()
    {
        _animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ�� �����ɴϴ�.
        _camera = Camera.main; // ���� ī�޶� �����ɴϴ�.
        _controller = GetComponent<CharacterController>(); // ĳ���� ��Ʈ�ѷ� ������Ʈ�� �����ɴϴ�.
    }

    // �� �����Ӹ��� ȣ��Ǵ� �Լ��Դϴ�.
    void Update()
    {
        GetInput(); // Ű �Է°��� �ҷ��ɴϴ�.
        Move(); // �Է��� ���� ���� �÷��̾ �̵���ŵ�ϴ�.
        Jump(); // �Է��� ���� ���� �÷��̾ ������ŵ�ϴ�.

    }


    // �÷��̾��� �Է� ���� �޾ƿɴϴ�.
    void GetInput()
    {
        cameraRotation = Input.GetKey(KeyCode.LeftAlt); // Alt Ű�� ���ȴ��� Ȯ���Ͽ� ī�޶� ȸ�� ��� ���¸� ������Ʈ�մϴ�.
        run = Input.GetKey(KeyCode.LeftShift); // Shift Ű�� ���ȴ��� Ȯ���Ͽ� �޸��� ���¸� ������Ʈ�մϴ�.    
    }

    // ��� �������� ������Ʈ�� �� ȣ��Ǵ� �Լ��Դϴ�.
    private void LateUpdate()
    {
        if (!cameraRotation)
        {
            // ī�޶� ȸ�� ����� ��Ȱ��ȭ�� ��� �÷��̾� ������ �ε巴�� ȸ���մϴ�.
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * cameraSmoothness);
        }
    }

    // �÷��̾��� �Է¿� ���� �������� ó���ϴ� �Լ��Դϴ�.
    void Move()
    {
        finalSpeed = run ? runSpeed : speed; // �޸��� ���¿� ���� ���� �̵� �ӵ��� �����մϴ�.

        // �÷��̾��� ���� �� ���� ���͸� ����մϴ�.
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // �Է¿� ���� �̵� ������ ����մϴ�.
        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        // ĳ���� ��Ʈ�ѷ��� ���� �÷��̾ �̵���ŵ�ϴ�.
        _controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);

        // �̵� �ִϸ��̼��� blend ���� �����մϴ�.
        float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
        animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }

    void Jump()
    {
        isGrounded = Physics.CheckSphere(ground_check.position, ground_distance, ground_mask);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            JumpSound();
        }
       
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void JumpSound()
    {
        mySfx.PlayOneShot(jumpSfx);
    }

    

}
