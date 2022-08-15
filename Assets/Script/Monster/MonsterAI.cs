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
    bool m_isInRange = false;
    bool m_isDeath = false;
    bool m_isAttacked = false;
    float m_time = 0f;
    float m_SightDistance = 10f;
    GameObject m_target;
    NavMeshAgent m_agent;
    GameObject HQ;
    GameObject m_Player;
    GameObject Controller;
    
    [SerializeField] bool isBoss;
    [SerializeField] bool isBox;
    [SerializeField] int m_health;
    [SerializeField] int m_attack;
    [SerializeField] float m_cooltime;
    [SerializeField] LayerMask Alliance;
    [SerializeField] Transform AttactStart;
    [SerializeField] GameObject AttackPref;


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
        m_time += Time.deltaTime;
        GetComponent<Animator>().SetBool("Attack", true);
        if (1f < m_time) {
            m_isAttacked = false;
            m_time = 0f;
        }
        if (!m_isAttacked) {
            if (0.5f < m_time) {
                other.GetComponent<IBattle>().Hit(m_attack);
                m_isAttacked = true;
            }
        }
    }
    public void BossAttack (Collider other) {
        if (m_isDeath) {
            return;
        }
        GetComponent<Animator>().SetBool("Attack", true);

        if (m_cooltime < m_time) {
            m_isAttacked = false;
            m_time = 0f;
            GameObject BossA = Instantiate(AttackPref,AttactStart.position, transform.rotation);
            BossA.GetComponent<BossAttack>().attack = m_attack;
            Destroy(BossA, 2f);
        }
        m_time += Time.deltaTime;
    }
    
    void SelectTarget () {//Edit after adding turret
        if (m_isDeath) return;
        if(m_target == null) m_target = HQ;
        Collider[] result = new Collider[100];
        Physics.OverlapSphereNonAlloc(transform.position, m_SightDistance, result, Alliance);
        for (int i = 0; i < 100; i++) {
            if (result[i] && result[i].transform.CompareTag("Player")) {
                if (Vector3.Distance(transform.position, HQ.transform.position) <=
                    Vector3.Distance(transform.position, m_Player.transform.position)) {
                    m_target = HQ;
                } else {
                    m_target = m_Player;
                }
            }
            if (result[i] && result[i].transform.CompareTag("Turret")) {
                if (Vector3.Distance(transform.position, HQ.transform.position) <=
                    Vector3.Distance(transform.position, result[i].transform.position)) {
                    m_target = HQ;
                } else {
                    m_target = result[i].transform.gameObject;
                }
            }
        }

    }

    void Move () {
        m_agent.speed = 6f;
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
            if (isBox) {
                GetComponent<Rigidbody>().AddForce(new Vector3(0,0,1),ForceMode.Force);
                StartCoroutine(DestroyBoxMonster());
                m_isDeath = true;
                return;
            }
            StartCoroutine(DestroyMonster());
            m_isDeath = true;

            /// 코드 수정함 (변경자 : zin) 엔딩 호출 제거
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "PlayerAttack") {
            Hit(other.gameObject.GetComponent<SkillAttack>().skillDamageValue);
        }
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "HQ" ||
            other.gameObject.tag == "Turret") {
        }
        
    }

    private void OnTriggerStay (Collider other) {
        if (other.gameObject.tag == "Player" ||
            other.gameObject.tag == "HQ" ||
            other.gameObject.tag == "Turret") {
            m_isInRange = true;
            if (isBoss) {
                BossAttack(other);
                return;
            }
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

        /// 코드 수정함 (변경자 : zin) 마지막 보스 아이템 생성X , 사라짐 X, 엔딩 호출

        if (Controller.GetComponent<MonsterController>().endPos == gameObject)
            Controller.GetComponent<MonsterController>().GameClear();
        yield return new WaitForSeconds(3.0f);
        if (isBoss)
        {
            if (Controller.GetComponent<MonsterController>().endPos != gameObject)
                Controller.GetComponent<MonsterController>().ItemGen(transform);
        }
        Controller.GetComponent<MonsterController>().Gold(isBoss);
        Controller.GetComponent<MonsterController>().CurrentMonsters--;
        if (Controller.GetComponent<MonsterController>().endPos != gameObject)
            Destroy(gameObject);
    }
    IEnumerator DestroyBoxMonster () {
        yield return new WaitForSeconds(3.0f);
        Controller.GetComponent<MonsterController>().BoxGen(transform);
        Destroy(gameObject);
    }
}