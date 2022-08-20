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
    private enum AIState
    {
        None, Attack, MoveAndAttack
    }

    bool m_isInRange = false;
    bool m_isDeath = false;
    bool m_isAttacked = false;
    float m_time = 0f;
    float m_SightDistance = 10f;
    GameObject m_target;
    NavMeshAgent m_agent;
    MonsterController m_SController;

    [SerializeField] GameObject m_HQ;
    [SerializeField] GameObject m_Player;
    [SerializeField] GameObject m_GController;
    [SerializeField] Animator m_animator;

    [SerializeField] bool isBoss;
    [SerializeField] bool isBox;
    [SerializeField] AIState aiState;
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
        m_target = m_HQ;
        if (!m_target) 
            m_target = m_Player;
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = 6f;
        m_SController = m_GController.GetComponent<MonsterController>();
        m_animator.SetBool("Idle", true);
    }
    public void Hit (int damage) {
        m_health -= damage;
    }
    public void Attack (Collider other) {//Move to Battle.cs
        if (m_isDeath) {
            return;
        }
        if (aiState == AIState.None) return;

        m_time += Time.deltaTime;
        m_animator.SetBool("Attack", true);
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
        m_animator.SetBool("Attack", true);

        if (m_cooltime < m_time) {
            m_isAttacked = false;
            m_time = 0f;
            GameObject BossA = Instantiate(AttackPref,AttactStart.position, transform.rotation);
            BossA.GetComponent<BossAttack>().attack = m_attack;
            Destroy(BossA, 2f);
        }
        m_time += Time.deltaTime;
    }
    
    void SelectTarget () {
        if (m_isDeath) return;
        if (aiState == AIState.None) return;
        if(m_target == null) m_target = m_HQ;
        Collider[] result = Physics.OverlapSphere(transform.position, m_SightDistance, Alliance);
        for (int i = 0; i < result.Length; i++) {
            if (0 < result.Length) continue;
            if (result[i].transform.CompareTag("Player")) {
                if (Vector3.Distance(transform.position, m_HQ.transform.position) <=
                    Vector3.Distance(transform.position, m_Player.transform.position)) {
                    m_target = m_HQ;
                } else {
                    m_target = m_Player;
                }
            }
            if (result[i].transform.CompareTag("Turret")) {
                if (Vector3.Distance(transform.position, m_HQ.transform.position) <=
                    Vector3.Distance(transform.position, result[i].transform.position)) {
                    m_target = m_HQ;
                } else {
                    m_target = result[i].transform.gameObject;
                }
            }
        }

    }

    void Move () {
        if (aiState == AIState.None || aiState == AIState.Attack) return;
        m_agent.speed = 6f;
        if (m_isDeath) {
            m_agent.enabled = false;
            return;
        }  
        if (!m_isInRange) {
            m_animator.SetBool("Run", true);
            m_agent.SetDestination(m_target.transform.position);
        }
    }

    void CheckDeath () {
        if (m_health <= 0) {
            m_animator.SetBool("Death", true);
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
    }

    private void OnTriggerStay (Collider other) {
        if (aiState == AIState.None) return;
        if (IsAttackable(other)) {
            m_isInRange = true;
            if (isBoss) {
                BossAttack(other);
                return;
            }
            Attack(other);
        }
    }

    private void OnTriggerExit (Collider other) {
        if (aiState == AIState.None) return;
        if (IsAttackable(other)) {
            m_isInRange = false;
            m_animator.SetBool("Attack", false);
        }
    }
    IEnumerator DestroyMonster () {

 

        if (m_SController.endPos == gameObject)
            m_SController.GameClear();
        yield return new WaitForSeconds(3.0f);
        if (isBoss)
        {
            if (m_SController.endPos != gameObject)
                m_SController.ItemGen(transform);
        }
        m_SController.Gold(isBoss);
        m_SController.CurrentMonsters--;
        if (m_SController.endPos != gameObject)
            Destroy(gameObject);
    }
    IEnumerator DestroyBoxMonster () {
        yield return new WaitForSeconds(3.0f);
        m_SController.BoxGen(transform);
        Destroy(gameObject);
    }

    bool IsAttackable(Collider target) => target.gameObject.tag is "Player" or "HQ" or "Turret"; //공부해보기
}