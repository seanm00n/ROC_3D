using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    [SerializeField] GameObject[] MonsterPref;
    [SerializeField] GameObject[] BossMonsterPref;
    [SerializeField] GameObject ItemPref;
    [SerializeField] Transform StartPos01;
    [SerializeField] Transform StartPos02;

    GameObject[] AddsMonster;
    GameObject[] BossMonster;
    Transform StartPos;
    public int CurrentMonsters = 0;//MonsterAI control this value
    float gTimer = 0f;//20m
    float aTimer = 0f;//1m
    float bTimer = 0f;//5m
    float monGenCool = 0f;
    int MaxMonsters = 60;
    int Aindex = 0;
    public int Bindex = 0;
    bool flag;

    void Start () {
        Init(); 
    }
    void Update () {
        GameTimer();
        AddsMonsterGen();
        BossMonsterGen();
    }

    void Init () {
        AddsMonster = new GameObject[21];
        for(int index01 = 0; index01 < 21; index01++) {
            AddsMonster[index01] = MonsterPref[index01];
        }
        BossMonster = new GameObject[4];
        for (int index02 = 0; index02 < 4; index02++) {
            BossMonster[index02] = BossMonsterPref[index02];
        }
        flag = false;
        CurrentMonsters = 0;
    }
    void AddsMonsterGen () {
        //잡몹 프리펩이 1분마다 변경됨, 20분 부터 함수 실행 안됨
        if (21 <= Aindex) return;
        aTimer += Time.deltaTime;
        monGenCool += Time.deltaTime;
        if (60f < aTimer) {
            aTimer = 0f;
            Aindex++;
        }
        //몬스터 수 비교해 계속 소환
        
        if (CurrentMonsters < MaxMonsters && 0.3f < monGenCool) {
            monGenCool = 0f;
            StartPos = flag ? StartPos01 : StartPos02;
            Instantiate(AddsMonster[Aindex],StartPos.position,StartPos.rotation);
            CurrentMonsters++;
            flag = flag ? false : true ;
        }
        
    }
    void BossMonsterGen () {
        //1분마다 보스몹 젠, 20분 부터 함수 실행 안됨
        if (4 <= Bindex) return;
        bTimer += Time.deltaTime;
        if (300f < bTimer) {
            StartPos = flag ? StartPos01 : StartPos02;
            Instantiate(BossMonster[Bindex], StartPos.position, StartPos.rotation);
            bTimer = 0f;
            Bindex++;
        }
    }
    void GameTimer () {
        gTimer += Time.deltaTime;
        if(1200f < gTimer) {

        }
    }
    public void GameClear () {
        //게임 클리어
        Debug.Log("GameClear");
    }
    public void ItemGen (Transform other) {
        Instantiate(ItemPref, other.position, other.rotation);
    }
}
