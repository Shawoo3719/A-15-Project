using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 10f; // 바운스 힘 설정

    // 트리거에 다른 Collider가 들어왔을 때 호출되는 메소드
    private void OnTriggerEnter(Collider other)
    {
        // Character Controller를 가진 오브젝트만 감지
        if (other.gameObject.CompareTag("Player"))
        {
            // 플레이어 오브젝트의 PlayerController 스크립트 가져오기
            PlayerJumpPad playerController = other.GetComponent<PlayerJumpPad>();

            // PlayerController가 존재하는지 확인
            if (playerController != null)
            {
                // 플레이어에게 바운스 힘 적용
                playerController.Bounce(bounceForce);
            }
        }
    }
}
