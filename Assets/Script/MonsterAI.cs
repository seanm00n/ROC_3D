using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    //Do not change values with inspector
    int healhPoint = 1;
    int attackPoint = 1;
    int speedPoint = 1;
    int abilityConst = 2; //Ability constant per stage
    public int stage = 1;
    public bool isBoss = false;

    void Start(){
        InitMonster();
    }

    void Update(){
        CheckDeath();
        MonsterBehavior();
    }

    void InitMonster () {
        healhPoint = stage * abilityConst;
        attackPoint = stage * abilityConst;
        speedPoint = stage * abilityConst;
    }

    void MonsterBehavior () {
        //get target

        //rotate to target

        //move to target
        GetComponent<Animator>().SetBool("Run", true);//"Run" ���� �ʿ�

        //avoid obstacles

        //attack target
        GetComponent<Animator>().SetBool("Attack", true);//"Attack"���� �ʿ�

    }

    void CheckDeath () {
        if (healhPoint <= 0) {
            GetComponent<Animator>().SetBool("Death", true);//"Death" ���� �ʿ�
            StartCoroutine("DestroyDelay");
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyDelay () {
        yield return new WaitForSeconds(3f);
    }
}
