using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class GroundTrap : MonoBehaviour
{

    [SerializeField] private Color activeColor;  // ���� ���������
    [SerializeField] private Color damageColor;     // ���� �� ����� ��������� �����
    [SerializeField] private Color defaultColor;   // ���� �� ���������
    [SerializeField] private float damageAmount = 10f;          // ����, ��������� ������
    [SerializeField] private float activationDelay = 1f;        // ����� �� ��������� �����
    [SerializeField] private float rechargeTime = 5f;           // ����� �����������
    [SerializeField] private LayerMask playerLayer;             // ���� ������
    [SerializeField] private VisualEffect[] flameSpawners;
    



    private Renderer blockRenderer;
    private bool isActivated = false;       // ��������, ������������ �� �������
    private bool isRecharging = false;      // �������� �� �����������
    private GameObject playerOnBlock = null;  // ����� �� �����

    private void Start()
    {
        blockRenderer = GetComponent<Renderer>();
        blockRenderer.material.color = defaultColor;  // ������������� ��������� ����
        flameSpawners = gameObject.GetComponentsInChildren<VisualEffect>();
        TurnOnOffFire(false);

    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���������, ��� ������ �������� ������� � ��� ������� �� �� �����������
        if ((playerLayer == (playerLayer | (1 << collision.gameObject.layer))))
        {
            playerOnBlock = collision.gameObject;  // ��������� ������
            if (!isActivated && !isRecharging) // ���������� �������, ���� ��� �� ������� � �� � �����������
            {
                StartCoroutine(ActivateTrap());
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // ���� ����� ������ � �����
        if (playerOnBlock == collision.gameObject)
        {
            playerOnBlock = null;  // ������� ������ �� ������
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

        // ������ ���� �� ��������� (���������)
        blockRenderer.material.color = activeColor;

        // ���� 1 ������� ����� ���������� �����
        yield return new WaitForSeconds(activationDelay);

        // ���� ����� ��� ��� ����� �� �����, ������� ����
        if (playerOnBlock != null && playerOnBlock.TryGetComponent(out PlayerStats playerHealth))
        {
            blockRenderer.material.color = damageColor;  // ������ ���� �� ������� (��������� �����)
            playerHealth.TakeDamage(damageAmount);       // ������� ����
            TurnOnOffFire(true);  //�������� ������� ����
            // ���� 0.5 ������� ����� ��������� �����
            yield return new WaitForSeconds(0.5f);
        }

        // ���������� ���� ����� � �������� ���������
        blockRenderer.material.color = defaultColor;
        //��������� ������� ����
        TurnOnOffFire(false);
        // �������� �����������
        isRecharging = true;
        yield return new WaitForSeconds(rechargeTime);

        // ����������� ���������
        isRecharging = false;
        isActivated = false;
        

        // ���� ����� ��� ��� �� �����, ���������� ������� �����
        if (playerOnBlock != null)
        {
            StartCoroutine(ActivateTrap());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // ���� ����� ����� �����������, ����� �� ����� �� �����.
        if ((playerLayer == (playerLayer | (1 << collision.gameObject.layer))) && !isActivated && !isRecharging)
        {
            // ���� ������� �� ������������ � �� �� �����������, ���������� �
            playerOnBlock = collision.gameObject;
            StartCoroutine(ActivateTrap());
        }
    }
}