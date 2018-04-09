using UnityEngine;

public class Projectile : MonoBehaviour {
	
	public LayerMask collisionMask;
	float speed = 10;
    float damage = 1;

    float lifetime = 1;
    float skinWidth = 0.1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject( initialCollisions[0] );
        }
    }

    public void setSpeed(float newSpeed) {
		speed = newSpeed;
	}

	void Update () {
		float moveDistance = speed * Time.deltaTime;
		checkCollisions(moveDistance);
		transform.Translate(Vector3.forward * moveDistance);
	}

	void checkCollisions(float moveDistance) {
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {
			OnHitObject(hit);
		}
	}

	void OnHitObject(RaycastHit hit) {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hit);
        }

		GameObject.Destroy(gameObject);
	}

    void OnHitObject(Collider c)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }

        GameObject.Destroy(gameObject);
    }
}
