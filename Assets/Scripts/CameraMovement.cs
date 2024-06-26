using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Setting")]
    // 카메라가 따라갈 오브젝트
    public Transform objectToFollow;
    // 카메라가 따라가는 속도
    public float followSpeed = 10f;
    // 마우스 감도
    public float sensitivity = 100f;
    // 카메라 각도 제한 (카메라가 한 바퀴 도는 것을 방지)
    public float clampAngle = 70f;

    // 마우스 입력을 저장할 변수들
    private float rotX;
    private float rotY;

    // 실제 카메라의 Transform
    public Transform realCamera;
    // 카메라의 정규화된 방향 벡터
    private Vector3 dirNormalized;
    // 카메라의 최종 방향 벡터
    private Vector3 finalDir;
    // 카메라의 최소 거리
    public float minDistance = 1f;
    // 카메라의 최대 거리
    public float maxDistance = 5f;
    // 카메라의 최종 거리
    public float finalDistance;
    // 카메라의 움직임 부드러움 정도
    public float smoothness = 10f;

    void Start()
    {
        // Start() 메서드:
        // 초기 회전값을 설정하고 카메라의 방향 벡터와 거리를 초기화합니다.

        // 현재 로컬 회전각을 이용하여 초기 회전값 설정
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x; // x축 회전각 초기화
        rotY = rot.y; // y축 회전각 초기화

        // 카메라의 방향 벡터와 거리 초기화
        dirNormalized = realCamera.localPosition.normalized; // 카메라 위치를 정규화하여 방향 벡터 계산
        finalDistance = realCamera.localPosition.magnitude;  // 카메라와 타겟 사이의 거리 초기화

        // 마우스 커서를 없앰
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Update() 메서드:
        // 마우스 입력을 받아 카메라를 회전시키고, 상하 각도를 제한합니다.

        // 마우스 입력을 받아 카메라 회전 처리
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime; // 마우스 Y 입력으로 x축 회전값 증가, 마우스 방향을 바르게 수정
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // 마우스 X 입력으로 y축 회전값 증가, 마우스 방향을 바르게 수정

        // 상하 각도를 제한하여 카메라가 한 바퀴 도는 것을 방지
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        // 회전값을 Quaternion으로 변환하여 적용
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot; // 계산된 회전값을 카메라에 적용
    }

    private void LateUpdate()
    {
        // LateUpdate() 메서드:
        // 카메라를 타겟 오브젝트로 부드럽게 이동시키고, 레이캐스트를 통해 장애물을 감지하여 최종 거리를 조정합니다.
        // Vector3.Lerp를 사용하여 카메라를 부드럽게 이동시킵니다.

        if (objectToFollow == null)
        {
            return; // objectToFollow가 null인 경우 처리 중단
        }

        // 타겟 오브젝트를 부드럽게 따라감
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, followSpeed * Time.deltaTime);

        // 카메라의 최종 방향을 설정
        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        // 레이캐스트 충돌 정보를 저장할 변수
        RaycastHit hit;

        // 카메라와 최종 방향 사이에 장애물이 있는지 검사
        if (Physics.Linecast(transform.position, finalDir, out hit))
        {
            // 장애물이 있으면 장애물까지의 거리를 최종 거리로 설정 (최소, 최대 거리 제한)
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            // 장애물이 없으면 최대 거리를 최종 거리로 설정
            finalDistance = maxDistance;
        }

        // 카메라를 부드럽게 이동시켜 최종 위치로 설정
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }
}
