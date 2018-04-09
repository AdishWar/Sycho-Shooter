using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable {

    public event System.Action OnDeath;

    public float startingHealth;

    protected float health;
    protected bool dead;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public void TakeHit(float damage, RaycastHit hit)
    {
        // do some stuff here with HIT argument
        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0 && !dead)
        {
            die();
        }
    }

    void die()
    {
        dead = true;
        if(OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }

}
