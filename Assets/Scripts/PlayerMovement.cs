using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// 플레이어의 움직임을 제어하는 스크립트
public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Animator animator;
    public AudioSource mySfx;
    public AudioClip jumpSfx;

    Animator _animator; // 애니메이터 컴포넌트를 참조하는 변수입니다.
    Camera _camera; // 메인 카메라를 참조하는 변수입니다.
    CharacterController _controller; // 캐릭터 컨트롤러를 참조하는 변수입니다.
    Rigidbody _rb; // 리지드바디 컴포넌트를 참조하는 변수입니다.

    // 컴포넌트에 표시 할 함수는 public으로, 표시하지 않을 것이라면 private로 선언합니다.
  
    [Header("Behavior Check")]
    public bool cameraRotation; // 카메라 회전 토글 변수입니다.
    public bool run; // 달리기 변수입니다.
    bool isGrounded;
    
  
    [Header("Movement")]
    public float speed = 3f; // 걷는 속도를 나타내는 변수입니다.
    public float runSpeed = 6f; // 뛰는 속도를 나타내는 변수입니다.
    public float finalSpeed; // 최종 이동 속도를 나타내는 변수입니다.
    public float cameraSmoothness = 10f; // 회전의 부드러움 정도를 나타내는 변수입니다.
    public float gravity = -9.1f;
    public float jumpHeight = 1f;
    

    [Header("Ground Check")]
    public Transform ground_check;
    public float ground_distance = 0.4f;
    public LayerMask ground_mask;


    private Vector3 velocity; // 플레이어의 속도를 나타내는 변수입니다.



    // 게임 오브젝트가 활성화될 때 호출되는 함수입니다.
    void Start()
    {
        _animator = GetComponent<Animator>(); // 애니메이터 컴포넌트를 가져옵니다.
        _camera = Camera.main; // 메인 카메라를 가져옵니다.
        _controller = GetComponent<CharacterController>(); // 캐릭터 컨트롤러 컴포넌트를 가져옵니다.
    }

    // 매 프레임마다 호출되는 함수입니다.
    void Update()
    {
        GetInput(); // 키 입력값을 불러옵니다.
        Move(); // 입력한 값에 따라 플레이어를 이동시킵니다.
        Jump(); // 입력한 값에 따라 플레이어를 점프시킵니다.

    }


    // 플레이어의 입력 값을 받아옵니다.
    void GetInput()
    {
        cameraRotation = Input.GetKey(KeyCode.LeftAlt); // Alt 키가 눌렸는지 확인하여 카메라 회전 토글 상태를 업데이트합니다.
        run = Input.GetKey(KeyCode.LeftShift); // Shift 키가 눌렸는지 확인하여 달리기 상태를 업데이트합니다.    
    }

    // 모든 프레임이 업데이트된 후 호출되는 함수입니다.
    private void LateUpdate()
    {
        if (!cameraRotation)
        {
            // 카메라 회전 토글이 비활성화된 경우 플레이어 방향을 부드럽게 회전합니다.
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * cameraSmoothness);
        }
    }

    // 플레이어의 입력에 따라 움직임을 처리하는 함수입니다.
    void Move()
    {
        finalSpeed = run ? runSpeed : speed; // 달리기 상태에 따라 최종 이동 속도를 결정합니다.

        // 플레이어의 전방 및 우측 벡터를 계산합니다.
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // 입력에 따라 이동 방향을 계산합니다.
        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        // 캐릭터 컨트롤러를 통해 플레이어를 이동시킵니다.
        _controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);

        // 이동 애니메이션의 blend 값을 설정합니다.
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
