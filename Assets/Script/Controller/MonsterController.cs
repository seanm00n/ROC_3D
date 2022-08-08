using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    [SerializeField] GameObject[] MonsterPref;
    [SerializeField] GameObject[] MonsterBossPref;
    [SerializeField] GameObject ItemPref;
    public GameObject[] Monster;
    GameObject[] BMonster;
    public int MIndex = 0;
    int MaxMonsters = 20;

    void Start () {
        Init(); 
    }
    void Update () {
        MonsterGen();
    }

    void Init () {
        Monster = new GameObject[MaxMonsters];
    }
    void MonsterGen () {
        if (Input.GetKeyDown(KeyCode.J)) {//임시 소환
            if (MIndex < MaxMonsters) {
                Monster[MIndex] = Instantiate(MonsterPref[0], transform.position, transform.rotation);
                Monster[MIndex].GetComponent<MonsterAI>().myIndex = MIndex;
                MIndex++;
            }
        }
    }
    public void ItemGen (int index) {
        Instantiate(ItemPref, Monster[index].transform.position, Monster[index].transform.rotation);
    }
}
