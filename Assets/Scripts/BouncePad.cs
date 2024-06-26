using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 10f; // �ٿ �� ����

    // Ʈ���ſ� �ٸ� Collider�� ������ �� ȣ��Ǵ� �޼ҵ�
    private void OnTriggerEnter(Collider other)
    {
        // Character Controller�� ���� ������Ʈ�� ����
        if (other.gameObject.CompareTag("Player"))
        {
            // �÷��̾� ������Ʈ�� PlayerController ��ũ��Ʈ ��������
            PlayerJumpPad playerController = other.GetComponent<PlayerJumpPad>();

            // PlayerController�� �����ϴ��� Ȯ��
            if (playerController != null)
            {
                // �÷��̾�� �ٿ �� ����
                playerController.Bounce(bounceForce);
            }
        }
    }
}
