using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    //Do not change values with inspector
    int healhPoint = 1;
    int attackPoint = 1;
    float speedPoint = 100;
    int abilityConst = 2; //Ability constant per stage
    Vector3 target;
    Vector3 force;
    public int stage = 1;
    public bool isBoss = false;


    void Start(){
        InitMonster();
    }

    void Update(){
        //CheckDeath();
        MonsterBehavior();
        //rotate to target
        //avoid obstacles
    }

    void InitMonster () {
        healhPoint = stage * abilityConst;
        attackPoint = stage * abilityConst;
        speedPoint = stage * abilityConst;
    }

    void MonsterBehavior () {
        //Move to HQ
        target = GameObject.Find("HQ").transform.position; Debug.Log("target" + target);  
        force = target - gameObject.transform.position; Debug.Log("thisPos" + gameObject.transform.position); Debug.Log("force"+force); 
        force.Normalize();
        Debug.Log("normalize"+force);
        force = new Vector3(force.x * speedPoint, force.y * speedPoint, force.z * speedPoint);
        Debug.Log("multSpeed"+force);
        GetComponent<Rigidbody>().velocity = force;
        //GetComponent<Animator>().SetBool("Run", true);
    }

    void MonsterAttack () {
        //attack target
        //GetComponent<Animator>().SetBool ("Attack", true);//"Attack"수정 필요
    }

    private void OnCollisionEnter (Collision collision) {
        if(collision.gameObject.tag == "") {

        }
    }
    void CheckDeath () {
        if (healhPoint <= 0) {
            //GetComponent<Animator>().SetBool("Death", true);//"Death" 수정 필요
            StartCoroutine("DestroyDelay");
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyDelay () {
        yield return new WaitForSeconds(3f);
    }
}
