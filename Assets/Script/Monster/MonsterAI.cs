using System.Collections;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR

using UnityEditor;

#endif
public class MonsterAI : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmosSelected () {
        Handles.color = new Color(0, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, transform.up, m_SightDistance);
    }
#endif
    //Do not change values with inspector
    int m_stage = 3; //Get value on Instantiate
    float m_health;
    float m_attack;
    bool m_isBoss = true;
    public bool m_isInRange = false;
    GameObject m_target;
    NavMeshAgent m_agent;
    GameObject HQ;
    GameObject Player;
    [SerializeField] GameObject DropItem;
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

    void SelectTarget () {
        if(m_target == null)
            m_target = HQ;
        Collider[] result = new Collider[1];
        Physics.OverlapSphereNonAlloc(transform.position, m_SightDistance, result, Alliance);
        if (result[0] && result[0].transform.CompareTag("Player")) 
        {
            if (Vector3.Distance(transform.position, HQ.transform.position) <=
                Vector3.Distance(transform.position, Player.transform.position)) {
                m_target = HQ;
            } 
            else 
            {
                m_target = Player;
            }
        }
        if (result[0] && result[0].transform.CompareTag("Turret")) {
            if (Vector3.Distance(transform.position, HQ.transform.position) <=
                Vector3.Distance(transform.position, result[0].transform.position)) {
                m_target = HQ;
            } else {
                m_target = result[0].transform.gameObject;
            }
        }
    }

    void Move () {
        if (!m_isInRange) {
            //GetComponent<Animator>().SetBool("Run", true);
            m_agent.SetDestination(m_target.transform.position);
        }
    }

    public void Attack () {
        //GetComponent<Animator>().SetBool("Attack", true);
        //0.5초 뒤 공격 실행
        //1초마다 데미지 입힘Hit
        Player.GetComponent<PlayerAnimControl>().Hit(m_attack);
        Debug.Log("Attack Player");
    }

    void GetHit (float damage) {
        //GetComponent<Animator>().SetBool("Hit", true);
        //enemy.GetComponent<Player>().m_attackPoint - m_health;
    }
    void CheckDeath () {
        if (m_health <= 0) {
            //GetComponent<Animator>().SetBool("Death", true);
            if (m_isBoss) {
                GameObject dropItem = Instantiate(DropItem,transform.position, transform.rotation);
            }
            StartCoroutine("DestroyDelay");
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyDelay () {
        yield return new WaitForSeconds(3f);
    }
}

