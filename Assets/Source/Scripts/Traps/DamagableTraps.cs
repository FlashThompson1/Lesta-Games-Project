using UnityEngine;

public class DamagableTraps : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerStats player))
            player.TakeDamage(_damage);
    }

}
