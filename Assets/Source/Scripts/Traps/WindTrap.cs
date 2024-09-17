using System.Collections;
using UnityEngine;

public class WindTrap : MonoBehaviour
{
    [SerializeField] private float windForce = 10f;          // Сила ветра
    [SerializeField] private float directionChangeInterval = 2f;  // Интервал смены направления ветра
    [SerializeField] private LayerMask playerLayer;          // Слой игрока

    private Vector3 windDirection;  // Текущее направление ветра
    private CharacterController playerController;  // CharacterController игрока

    private void Start()
    {
        // Начинаем цикл смены направления ветра
        StartCoroutine(ChangeWindDirection());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, что объект является игроком
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
        // Если игрок выходит из зоны ветра
        if (playerController != null && collision.gameObject.GetComponent<CharacterController>() == playerController)
        {
            playerController = null;  // Убираем ссылку на игрока
            Debug.Log("Player exited wind zone");
        }
    }

    private void Update()
    {
        // Если игрок находится в зоне ветра
        if (playerController != null)
        {
            // Применяем силу ветра к игроку
            Vector3 windImpact = windDirection * windForce * Time.deltaTime;
            playerController.Move(windImpact);
        }
    }

    private IEnumerator ChangeWindDirection()
    {
        while (true)
        {
            // Выбираем случайное направление ветра (либо влево, либо вправо)
            int randomDirection = Random.Range(0, 2);  // 0 - влево, 1 - вправо
            windDirection = randomDirection == 0 ? Vector3.left : Vector3.right;
            Debug.Log("Wind direction changed to: " + windDirection);

            // Ждем указанное время перед сменой направления
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    private void OnDrawGizmos()
    {
        // Визуализация направления ветра
        Gizmos.color = Color.blue;
        Vector3 direction = windDirection == Vector3.left ? Vector3.left : Vector3.right;
        Gizmos.DrawLine(transform.position, transform.position + direction * 2f);
    }
}