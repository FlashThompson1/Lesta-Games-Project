using System.Collections;
using UnityEngine;

public class WindTrap : MonoBehaviour
{
    [SerializeField] private float windForce = 10f;          // ���� �����
    [SerializeField] private float directionChangeInterval = 2f;  // �������� ����� ����������� �����
    [SerializeField] private LayerMask playerLayer;          // ���� ������

    private Vector3 windDirection;  // ������� ����������� �����
    private CharacterController playerController;  // CharacterController ������

    private void Start()
    {
        // �������� ���� ����� ����������� �����
        StartCoroutine(ChangeWindDirection());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���������, ��� ������ �������� �������
        if ((playerLayer & (1 << collision.gameObject.layer)) != 0)
        {
            playerController = collision.gameObject.GetComponent<CharacterController>();
            if (playerController != null)
            {
                Debug.Log("Player entered wind zone");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // ���� ����� ������� �� ���� �����
        if (playerController != null && collision.gameObject.GetComponent<CharacterController>() == playerController)
        {
            playerController = null;  // ������� ������ �� ������
            Debug.Log("Player exited wind zone");
        }
    }

    private void Update()
    {
        // ���� ����� ��������� � ���� �����
        if (playerController != null)
        {
            // ��������� ���� ����� � ������
            Vector3 windImpact = windDirection * windForce * Time.deltaTime;
            playerController.Move(windImpact);
        }
    }

    private IEnumerator ChangeWindDirection()
    {
        while (true)
        {
            // �������� ��������� ����������� ����� (���� �����, ���� ������)
            int randomDirection = Random.Range(0, 2);  // 0 - �����, 1 - ������
            windDirection = randomDirection == 0 ? Vector3.left : Vector3.right;
            Debug.Log("Wind direction changed to: " + windDirection);

            // ���� ��������� ����� ����� ������ �����������
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void OnDrawGizmos()
    {
        // ������������ ����������� �����
        Gizmos.color = Color.blue;
        Vector3 direction = windDirection == Vector3.left ? Vector3.left : Vector3.right;
        Gizmos.DrawLine(transform.position, transform.position + direction * 2f);
    }
}