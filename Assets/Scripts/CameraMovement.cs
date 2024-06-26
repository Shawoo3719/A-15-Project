using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Setting")]
    // ī�޶� ���� ������Ʈ
    public Transform objectToFollow;
    // ī�޶� ���󰡴� �ӵ�
    public float followSpeed = 10f;
    // ���콺 ����
    public float sensitivity = 100f;
    // ī�޶� ���� ���� (ī�޶� �� ���� ���� ���� ����)
    public float clampAngle = 70f;

    // ���콺 �Է��� ������ ������
    private float rotX;
    private float rotY;

    // ���� ī�޶��� Transform
    public Transform realCamera;
    // ī�޶��� ����ȭ�� ���� ����
    private Vector3 dirNormalized;
    // ī�޶��� ���� ���� ����
    private Vector3 finalDir;
    // ī�޶��� �ּ� �Ÿ�
    public float minDistance = 1f;
    // ī�޶��� �ִ� �Ÿ�
    public float maxDistance = 5f;
    // ī�޶��� ���� �Ÿ�
    public float finalDistance;
    // ī�޶��� ������ �ε巯�� ����
    public float smoothness = 10f;

    void Start()
    {
        // Start() �޼���:
        // �ʱ� ȸ������ �����ϰ� ī�޶��� ���� ���Ϳ� �Ÿ��� �ʱ�ȭ�մϴ�.

        // ���� ���� ȸ������ �̿��Ͽ� �ʱ� ȸ���� ����
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x; // x�� ȸ���� �ʱ�ȭ
        rotY = rot.y; // y�� ȸ���� �ʱ�ȭ

        // ī�޶��� ���� ���Ϳ� �Ÿ� �ʱ�ȭ
        dirNormalized = realCamera.localPosition.normalized; // ī�޶� ��ġ�� ����ȭ�Ͽ� ���� ���� ���
        finalDistance = realCamera.localPosition.magnitude;  // ī�޶�� Ÿ�� ������ �Ÿ� �ʱ�ȭ

        // ���콺 Ŀ���� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Update() �޼���:
        // ���콺 �Է��� �޾� ī�޶� ȸ����Ű��, ���� ������ �����մϴ�.

        // ���콺 �Է��� �޾� ī�޶� ȸ�� ó��
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime; // ���콺 Y �Է����� x�� ȸ���� ����, ���콺 ������ �ٸ��� ����
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; // ���콺 X �Է����� y�� ȸ���� ����, ���콺 ������ �ٸ��� ����

        // ���� ������ �����Ͽ� ī�޶� �� ���� ���� ���� ����
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        // ȸ������ Quaternion���� ��ȯ�Ͽ� ����
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot; // ���� ȸ������ ī�޶� ����
    }

    private void LateUpdate()
    {
        // LateUpdate() �޼���:
        // ī�޶� Ÿ�� ������Ʈ�� �ε巴�� �̵���Ű��, ����ĳ��Ʈ�� ���� ��ֹ��� �����Ͽ� ���� �Ÿ��� �����մϴ�.
        // Vector3.Lerp�� ����Ͽ� ī�޶� �ε巴�� �̵���ŵ�ϴ�.

        if (objectToFollow == null)
        {
            return; // objectToFollow�� null�� ��� ó�� �ߴ�
        }

        // Ÿ�� ������Ʈ�� �ε巴�� ����
        transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, followSpeed * Time.deltaTime);

        // ī�޶��� ���� ������ ����
        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        // ����ĳ��Ʈ �浹 ������ ������ ����
        RaycastHit hit;

        // ī�޶�� ���� ���� ���̿� ��ֹ��� �ִ��� �˻�
        if (Physics.Linecast(transform.position, finalDir, out hit))
        {
            // ��ֹ��� ������ ��ֹ������� �Ÿ��� ���� �Ÿ��� ���� (�ּ�, �ִ� �Ÿ� ����)
            finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
        }
        else
        {
            // ��ֹ��� ������ �ִ� �Ÿ��� ���� �Ÿ��� ����
            finalDistance = maxDistance;
        }

        // ī�޶� �ε巴�� �̵����� ���� ��ġ�� ����
        realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
    }
}
