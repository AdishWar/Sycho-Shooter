using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

    public enum State { Idle, Chasing, Attacking };
    State currentState;

    Material skinMaterial;
    Color originalColor;

	NavMeshAgent pathfinder;
	Transform target;

    float nextAttackTime;
    float timeBetweenAttacks = 1;

    float attackDistanceThreshold = 1.5f;

    float myCollisionRadius;
    float targetCollisionRadius;

    protected override void Start ()
    {
        base.Start();
        currentState = State.Chasing;
		pathfinder = GetComponent<NavMeshAgent> ();
		target = GameObject.FindGameObjectWithTag("Player").transform;

        myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        StartCoroutine( UpdatePath() );
	}
	
	void Update ()
    {
		if(Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + timeBetweenAttacks;
            float sqDistToTarget = (transform.position - target.position).sqrMagnitude;
            if(sqDistToTarget <= Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2) )
            {
                StartCoroutine( Attack() );
            }
        }
	}
    
    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 currentPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 targetPosition = target.position - dirToTarget * targetCollisionRadius;

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;
        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float interpolation = 4 * ( -Mathf.Pow(percent, 2) + percent );
            transform.position = Vector3.Lerp(currentPosition, targetPosition, interpolation);

            yield return null;
        }
        skinMaterial.color = originalColor;

        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

	IEnumerator UpdatePath()
    {
		float refreshRate = .25f;

		while(target != null)
        {
            if(currentState == State.Chasing)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (targetCollisionRadius + myCollisionRadius + attackDistanceThreshold/2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }

            yield return new WaitForSeconds(refreshRate);

        }
	}
}
