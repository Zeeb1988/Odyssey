using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    [Header("Skill")]
    public float kickForce = 25;
    public GameObject rockPrefab;
    public Transform handPos;

    //Animation Event
    void KickOff()
    {
        

        if (attackTarget != null && transform.IsFaceTarger(attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            Vector3 direction = (attackTarget.transform.position - transform.position).normalized;
            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
            attackTarget.GetComponent<NavMeshAgent>().ResetPath();

            targetStats.TakeDamage(characterStats, targetStats);

        }
    }

    //Animation Event
    public void ThrowRock() 
    {

        if (attackTarget != null) 
        {
           var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
           rock.GetComponent<Rock>().target = attackTarget; 
        }
    }
}
