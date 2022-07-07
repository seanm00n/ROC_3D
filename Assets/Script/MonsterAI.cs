using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    //Do not change values with inspector
    int stage = 3; //Get value on Instantiate
    float health;
    float attack;
    public bool isBoss = false;
    GameObject target;
    NavMeshAgent agent;
    [SerializeField] //?
    public LayerMask neutral;

    void Start(){
        Init();
    }

    void Update(){
        CheckDeath();
        SelectTarget();
        Move();
    }

    void Init () {
        target = GameObject.Find("HQ");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = stage * 1.6f;
        health = stage * 1.4f;
        attack = stage * 1.5f;
    }

    void Move () {
        agent.SetDestination(target.transform.position);
    }

    void CheckDeath () {
        if (health <= 0) {
            //GetComponent<Animator>().SetBool("Death", true);
            StartCoroutine("DestroyDelay");
            Destroy(gameObject);
        }
    }
    void SelectTarget () {
        if (true) {
            target = GameObject.Find("HQ");
        }
        IEnumerator DestroyDelay () {
            yield return new WaitForSeconds(3f);
        }
    }
}
