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
    [SerializeField] Transform[] StartPosTrans;

    GameObject[] AddsMonster;
    GameObject[] BossMonster;
    Transform[] StartPos;
    PlayerSaveData data;//설정해주어야함
    public int CurrentMonsters = 0;//MonsterAI control this value
    int WaveNum = -1;
    float m_Time;
    float m_Time2;//addsmonstergendelay
    
    /// 코드 수정함 (변경자 : zin)
    public GameObject endPos;
    public GameObject endcamera;

    // 싱글톤
    public static MonsterController instance;

    void Start () {
        instance = this;
        Init(); 
    }
    void Update () {
        BoxMonsterGen();
        CountCurrentMonsters();
    }

    void Init () {
        AddsMonster = new GameObject[20];
        for(int index01 = 0; index01 < 20; index01++) {
            AddsMonster[index01] = MonsterPref[index01];
        }
        BossMonster = new GameObject[4];
        for (int index02 = 0; index02 < 4; index02++) {
            BossMonster[index02] = BossMonsterPref[index02];
        }
        StartPos = new Transform[6];
        for (int index03 = 0; index03 < 6; index03++)
        {
            StartPos[index03] = StartPosTrans[index03];
        }
        CurrentMonsters = 0;
    }
    void CountCurrentMonsters(){
        if(CurrentMonsters <= 0) AddsMonsterGen();
        if ((WaveNum + 1) % 5 == 0) BossMonsterGen(WaveNum);
    }
    void AddsMonsterGen () {
        //웨이브 몬스터를 전부 처치하면 CurrentMonsters변수로 확인 후 다음 웨이브 출격
        for (int i = 0; i < 6; i++)
        {
            Instantiate(AddsMonster[WaveNum], StartPos[i].position, StartPos[i].rotation);
        }
        CurrentMonsters = 6;
        WaveNum++;    
    }
    void BossMonsterGen(int WaveNum){
        //특정 웨이브를 따라 출격
        int tmp = ((WaveNum + 1) / 5) - 1;
        Instantiate(BossMonster[tmp],StartPos[tmp].position,StartPos[tmp].rotation);
    }
    void BoxMonsterGen() {
        //시간이 지남에 따라 출격
        m_Time += Time.deltaTime;
        if (300f < m_Time) {
            m_Time = 0;
            Instantiate(BoxMonsterPref, StartPos[3]);
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
        try {
            data = SaveManager.Load<PlayerSaveData>("PlayerData");
        }catch{
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
