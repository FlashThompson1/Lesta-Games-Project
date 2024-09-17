using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class GroundTrap : MonoBehaviour
{

    [SerializeField] private Color activeColor;  // Цвет активации
    [SerializeField] private Color damageColor;     // Цвет во время нанесения урона
    [SerializeField] private Color defaultColor;   // Цвет по умолчанию
    [SerializeField] private float damageAmount = 10f;          // Урон, наносимый игроку
    [SerializeField] private float activationDelay = 1f;        // Время до нанесения урона
    [SerializeField] private float rechargeTime = 5f;           // Время перезарядки
    [SerializeField] private LayerMask playerLayer;             // Слой игрока
    [SerializeField] private VisualEffect[] flameSpawners;
    



    private Renderer blockRenderer;
    private bool isActivated = false;       // Проверка, активирована ли ловушка
    private bool isRecharging = false;      // Проверка на перезарядку
    private GameObject playerOnBlock = null;  // Игрок на блоке

    private void Start()
    {
        blockRenderer = GetComponent<Renderer>();
        blockRenderer.material.color = defaultColor;  // Устанавливаем начальный цвет
        flameSpawners = gameObject.GetComponentsInChildren<VisualEffect>();
        TurnOnOffFire(false);

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, что объект является игроком и что ловушка не на перезарядке
        if ((playerLayer == (playerLayer | (1 << collision.gameObject.layer))))
        {
            playerOnBlock = collision.gameObject;  // Сохраняем игрока
            if (!isActivated && !isRecharging) // Активируем ловушку, если она не активна и не в перезарядке
            {
                StartCoroutine(ActivateTrap());
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Если игрок уходит с блока
        if (playerOnBlock == collision.gameObject)
        {
            playerOnBlock = null;  // Убираем ссылку на игрока
        }
    }

    private void TurnOnOffFire(bool activate)
    {
        

        foreach (VisualEffect effect in flameSpawners)
        {
            if (activate == true)
            {
                effect.Play();
            }
            else if (activate == false)
            {
                effect.Stop();
            }
        }
    }



    private IEnumerator ActivateTrap()
    {
        isActivated = true;

        // Меняем цвет на оранжевый (активация)
        blockRenderer.material.color = activeColor;

        // Ждем 1 секунду перед нанесением урона
        yield return new WaitForSeconds(activationDelay);

        // Если игрок все еще стоит на блоке, наносим урон
        if (playerOnBlock != null && playerOnBlock.TryGetComponent(out PlayerStats playerHealth))
        {
            blockRenderer.material.color = damageColor;  // Меняем цвет на красный (нанесение урона)
            playerHealth.TakeDamage(damageAmount);       // Наносим урон
            TurnOnOffFire(true);  //Включаем эффекты огня
            // Ждем 0.5 секунды перед возвратом цвета
            yield return new WaitForSeconds(0.5f);
        }

        // Возвращаем цвет блока в исходное состояние
        blockRenderer.material.color = defaultColor;
        //Отключаем эффекты огня
        TurnOnOffFire(false);
        // Начинаем перезарядку
        isRecharging = true;
        yield return new WaitForSeconds(rechargeTime);

        // Перезарядка закончена
        isRecharging = false;
        isActivated = false;
        

        // Если игрок все еще на блоке, активируем ловушку снова
        if (playerOnBlock != null)
        {
            StartCoroutine(ActivateTrap());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Этот метод будет отслеживать, стоит ли игрок на блоке.
        if ((playerLayer == (playerLayer | (1 << collision.gameObject.layer))) && !isActivated && !isRecharging)
        {
            // Если ловушка не активирована и не на перезарядке, активируем её
            playerOnBlock = collision.gameObject;
            StartCoroutine(ActivateTrap());
        }
    }
}