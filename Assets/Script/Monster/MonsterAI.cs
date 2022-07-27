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
    public int m_stage = 3; //set value
    public bool m_isInRange = false;
    public int myIndex;
    float m_health;
    float m_attack;
    bool m_isDeath = false;
    bool m_isAttacked = false;
    float m_cooltime = 0f;
    GameObject m_target;
    NavMeshAgent m_agent;
    GameObject HQ;
    GameObject Player;
    GameObject MController;
    
    [SerializeField] bool m_isBoss = false;
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
    public void Attack () {
        m_cooltime += Time.deltaTime;
        GetComponent<Animator>().SetBool("Attack", true);
        if(1f < m_cooltime) {
            m_isAttacked = false;
            m_cooltime = 0f;
        }
        if (!m_isAttacked) {
            if (0.5f < m_cooltime) {
                PlayerAnimControl.instance.Hit(m_attack);
                m_isAttacked = true;//�ִϸ��̼� 1ȸ �� 1�� ����
                Debug.Log("Attack Player");
            }
        }
    }
    void Init () {
        Player = GameObject.Find("Wizard_Player").transform.GetChild(0).gameObject;
        MController = GameObject.Find("MonsterController");
        HQ = GameObject.Find("HQ");//���� ����
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
        if (m_isDeath)
            return;
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
            m_health -= other.GetComponent<Skill_Attack>().skill_Damage_Value;
        }
    }
    private void OnTriggerStay (Collider other) {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "HQ" ||
            other.gameObject.tag == "Turret") {
            m_isInRange = true;
            Attack();
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
        yield return new WaitForSeconds(5f);
        MController.GetComponent<MonsterController>().ItemGen(myIndex);
        Destroy(gameObject);
    }
}