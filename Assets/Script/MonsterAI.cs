using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    //Do not change values with inspector
    int m_stage = 3; //Get value on Instantiate
    float m_health;
    float m_attack;
    bool m_isBoss = false;
    bool m_isInRange = false;
    GameObject m_target;
    NavMeshAgent m_agent;
    [SerializeField] GameObject HQ;
    [SerializeField] GameObject Player;
    [SerializeField] LayerMask Alliance;
    [SerializeField] float m_SightDistance = 0f;
    void Start(){
        Init();
    }

    void Update(){
        CheckDeath();
        SelectTarget();
        Move();
        Debug.Log("Target : " + m_target);
    }

    void Init () {
        m_target = HQ;
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = m_stage * 1.6f;
        m_health = m_stage * 1.4f;
        m_attack = m_stage * 1.5f;
        if (m_isBoss) {
            m_agent.avoidancePriority = 0;
        } else {
            m_agent.avoidancePriority = 1;
        }
    }

    void SelectTarget () {
        m_target = HQ;
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, m_SightDistance)) {
            if (hit.transform.name == "Player") {
                if (Vector3.Distance(transform.position, HQ.transform.position) <=
                    Vector3.Distance(transform.position, Player.transform.position)) {
                    m_target = HQ;
                } else {
                    m_target = Player;
                }
            }
            if (hit.transform.CompareTag("Turret")) {
                if (Vector3.Distance(transform.position, HQ.transform.position) <=
                    Vector3.Distance(transform.position, hit.transform.position)) {
                    m_target = HQ;
                } else {
                    m_target = hit.transform.gameObject;
                }
            }
        }
    }

    void Move () {
        if (!m_isInRange) {
            //GetComponent<Animator>().SetBool("Run", true);
            m_agent.SetDestination(m_target.transform.position);
        }
    }

    void Attack () {
        //GetComponent<Animator>().SetBool("Attack", true);
    }

    void GetHit (GameObject enemy) {
        //GetComponent<Animator>().SetBool("Hit", true);
        //enemy.GetComponent<Player>().m_attackPoint - m_health;
    }
    void CheckDeath () {
        if (m_health <= 0) {
            //GetComponent<Animator>().SetBool("Death", true);
            StartCoroutine("DestroyDelay");
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyDelay () {
        yield return new WaitForSeconds(3f);
    }

    private void OnCollisionEnter (Collision collision) {//change to other method
        if (collision.gameObject == m_target) {
            m_isInRange = true;
            Attack();
            Debug.Log("enter");
        }
        
    }

    private void OnCollisionExit (Collision collision) {
        if (collision.gameObject == m_target) {
            m_isInRange = false;
            Debug.Log("exit");
        }
    }
}

