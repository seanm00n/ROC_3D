using System.Collections;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR

using UnityEditor;

#endif
public class MonsterAI : MonoBehaviour, IBattle {
    // Boss avoidancePriority = 0 else avoidancePriority = 1
#if UNITY_EDITOR
private void OnDrawGizmosSelected () {
        Handles.color = new Color(0, 0, 0, 0.1f);
        Handles.DrawSolidDisc(transform.position, transform.up, m_SightDistance);
    }
#endif
    public int myIndex;
    
    bool m_isInRange = false;
    bool m_isDeath = false;
    bool m_isAttacked = false;
    float m_cooltime = 0f;
    float m_deadTime = 0f;
    float m_SightDistance = 10f;
    GameObject m_target;
    NavMeshAgent m_agent;
    GameObject HQ;
    GameObject m_Player;
    GameObject Controller;

    [SerializeField] bool isBoss;
    [SerializeField] int m_health;
    [SerializeField] int m_attack;
    [SerializeField] LayerMask Alliance;
    

    void Start(){
        Init();
    }

    void Update(){
        CheckDeath();
        SelectTarget();
        Move();

    }

    void Init () {
        Controller = GameObject.Find("MonsterController");
        m_Player = GameObject.Find("Player");
        HQ = GameObject.Find("HQ");//change in case : 'name value changes'
        m_target = HQ;
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = 6f;
    }
    public void Hit (int damage) {
        m_health -= damage;
    }
    public void Attack (Collider other) {//Move to Battle.cs
        if (m_isDeath) {
            return;
        }
        m_cooltime += Time.deltaTime;
        GetComponent<Animator>().SetBool("Attack", true);
        if (1f < m_cooltime) {
            m_isAttacked = false;
            m_cooltime = 0f;
        }
        if (!m_isAttacked) {
            if (0.5f < m_cooltime) {
                other.GetComponent<IBattle>().Hit(m_attack);
                m_isAttacked = true;
            }
        }
    }
    void SelectTarget () {//Edit after adding turret
        if (m_isDeath) return;
        if(m_target == null) m_target = HQ;
        Collider[] result = new Collider[1];
        Physics.OverlapSphereNonAlloc(transform.position, m_SightDistance, result, Alliance);
        if (result[0] && result[0].transform.CompareTag("Player")) {
            if (Vector3.Distance(transform.position, HQ.transform.position) <=
                Vector3.Distance(transform.position, m_Player.transform.position)) {
                m_target = HQ;
            } 
            else 
            {
                m_target = m_Player;
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
        if (m_isDeath) {
            m_agent.enabled = false;
            return;
        }  
        if (!m_isInRange) {
            GetComponent<Animator>().SetBool("Run", true);
            m_agent.SetDestination(m_target.transform.position);
        }
    }

    void CheckDeath () {
        if (m_health <= 0) {
            GetComponent<Animator>().SetBool("Death", true);
            StartCoroutine(DestroyMonster());
            m_isDeath = true;
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "PlayerAttack") {
            Hit(other.gameObject.GetComponent<SkillAttack>().skillDamageValue);
        }
    }

    private void OnTriggerStay (Collider other) {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "HQ" ||
            other.gameObject.tag == "Turret") {
            m_isInRange = true;
            Attack(other);
        }
    }

    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "HQ" ||
            other.gameObject.tag == "Turret") {
            m_isInRange = false;
            GetComponent<Animator>().SetBool("Attack", false);
        }
    }
    IEnumerator DestroyMonster () {
        yield return new WaitForSeconds(3.0f);
        if (isBoss) {
            Controller.GetComponent<MonsterController>().ItemGen(myIndex);
        }
        Destroy(gameObject);
    }
}