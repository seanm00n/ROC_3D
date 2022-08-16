using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ROC;

public class MonsterController : MonoBehaviour {
    [SerializeField] GameObject[] MonsterPref;
    [SerializeField] GameObject[] BossMonsterPref;
    [SerializeField] GameObject BoxMonsterPref;
    [SerializeField] GameObject BoxPref;
    [SerializeField] GameObject ItemPref;
    [SerializeField] Transform StartPos01;
    [SerializeField] Transform StartPos02;

    GameObject[] AddsMonster;
    GameObject[] BossMonster;
    Transform StartPos;
    public int CurrentMonsters = 0;//MonsterAI control this value
    PlayerSaveData data;//설정해주어야함
    float aTimer = 0f;//1m
    float bTimer = 0f;//5m
    float xTimer = 0f;//50s
    float monGenCool = 0f;
    int MaxMonsters = 60;
    int Aindex = 0;
    public int Bindex = 0;
    bool flag;
    
    /// 코드 수정함 (변경자 : zin)
    public GameObject endPos;
    public GameObject endcamera;

    void Start () {
        Init(); 
    }
    void Update () {
        AddsMonsterGen();
        BossMonsterGen();
        BoxMonsterGen();
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
        xTimer = 50;
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
    void BoxMonsterGen() {
        xTimer += Time.deltaTime;
        if (50f < xTimer) {
            xTimer = 0f;
            StartPos = flag ? StartPos01 : StartPos02;
            Instantiate(BoxMonsterPref, StartPos.position, StartPos.rotation);
            flag = flag ? false : true;
        }
    }
    void BossMonsterGen () {
        //1분마다 보스몹 젠, 20분 부터 함수 실행 안됨
        if (4 <= Bindex) return;
        bTimer += Time.deltaTime;
        if (300f < bTimer) { 
            StartPos = flag ? StartPos01 : StartPos02;

            /// 코드 수정함 (변경자 : zin) 드래곤 위치 받기
            if (Bindex == 3)
            {
                endPos = Instantiate(BossMonster[Bindex], StartPos.position, StartPos.rotation);
            }
            else
            Instantiate(BossMonster[Bindex], StartPos.position, StartPos.rotation);
            bTimer = 0f;
            Bindex++;
        }
    }
    public void GameClear() 
    {
        //게임 클리어

        /// 코드 수정함 (변경자 : zin)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if(Camera.main)
        Camera.main.gameObject.SetActive(false);
        Player.instance.ui.gameObject.SetActive(false);
        Player.instance.gameObject.SetActive(false);

        endcamera.SetActive(true);
        endcamera.transform.position = endPos.transform.position + new Vector3(0,20,0) - endPos.transform.forward;
        endcamera.transform.LookAt(endPos.transform.position);

    }
    public void Gold (bool isBoss)
    {
        /// 코드 수정함 (변경자 : zin) 골드 수급 관련 능력 보유시 더 많이 획득
        try
        {
            data = SaveManager.Load<PlayerSaveData>("PlayerData");
        }
        catch
        {
            data = new PlayerSaveData();
        }
        if (isBoss) {
            PlayerSaveData.gold += 1000;
       
            float value = 1000 * (data.getMoreGold / 100f);
            PlayerSaveData.gold += (int)value;
        }
        else {
            PlayerSaveData.gold += 5;
            
            float value = 5 * (data.getMoreGold / 100f);
            PlayerSaveData.gold += (int)value;
        }
        
    }
    public void ItemGen (Transform other) {
        Instantiate(ItemPref, other.position, other.rotation);
        try {
            data = SaveManager.Load<PlayerSaveData>("PlayerData");
        }
        catch {
            data = new PlayerSaveData();
        }
        data.bone += 50;
        /// 코드 수정함 (변경자 : zin) 뼈 수급 관련 능력 보유시 더 많이 획득
        data.bone += data.getMoreBone;
        SaveManager.Save("PlayerData", data);
    }
    public void BoxGen (Transform other) {
        Instantiate(BoxPref, other.position, other.rotation);
    }
}
