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
    int WaveNum = 0;
    
    /// 코드 수정함 (변경자 : zin)
    public GameObject endPos;
    public GameObject endcamera;

    void Start () {
        Init(); 
    }
    void Update () {
        BossMonsterGen();
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
        for (int index03 = 0; index03 < 6; index03++)
        {
            StartPos[index03] = StartPosTrans[index03];
        }
        CurrentMonsters = 0;
    }
    void CountCurrentMonsters()
    {
        if(0 < CurrentMonsters) AddsMonsterGen();
    }
    void AddsMonsterGen () {
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 6; j++) {
                Instantiate(AddsMonster[WaveNum], StartPos[j].position, StartPos[j].rotation);
            }
        }
        CurrentMonsters = 30;
        WaveNum++;
    }
    void BoxMonsterGen() {

    }
    void BossMonsterGen () {

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
