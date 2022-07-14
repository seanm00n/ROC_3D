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
    GameObject HQ;
    GameObject Player;
    [SerializeField] LayerMask Alliance;
    [SerializeField] float m_SightDistance = 0f;

    void Start(){
        Init();
    }

    void Update(){
        CheckDeath();
        SelectTarget();
        Move();
        
    }

    void Init () {

        Player = GameObject.Find("Test_Player").transform.GetChild(0).gameObject;
        HQ = GameObject.Find("HQ");
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

    void SelectTarget()
    {
        if (m_target == null)
            m_target = HQ;
        Collider[] result = new Collider[1];

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, m_SightDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                if (Vector3.Distance(transform.position, HQ.transform.position) <=
                    Vector3.Distance(transform.position, Player.transform.position))
                {
                    m_target = HQ;
                }
                else
                {
                    m_target = Player;
                }
            }
            if (hit.transform.CompareTag("Turret"))
            {
                if (Vector3.Distance(transform.position, HQ.transform.position) <=
                    Vector3.Distance(transform.position, hit.transform.position))
                {
                    m_target = HQ;
                }
                else
                {
                    m_target = result[0].transform.gameObject;
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
        //0.5초 뒤 공격 실행
        //1초마다 데미지 입힘Hit
        Player.GetComponent<PlayerAnimControl>().Hit(m_attack);
    }

    void GetHit (float damage) {
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
    private void OnTriggerStay (Collider other) {
        if(other.gameObject.tag == "Player") {
            m_isInRange = true;
            Attack();
        }
    }
    private void OnTriggerExit (Collider other) {
        if(other.gameObject.tag == "Player") {
            m_isInRange = false;
        }
    }
}

