using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private int sceneNum = 0;
    Animator anim;
    public Image blockImage;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void StartButton()
    {
        // 데이터 초기화
        StartCoroutine(FadeOut());
    }
    public void LoadButton()
    {
        anim.SetTrigger("save");
        
    }

    public void LoadBoxExitButton()
    {
        anim.SetTrigger("exit");
    }

    public void SaveLoad(int saveFileNumber = 0)
    {
        switch (saveFileNumber) 
        {
            case 1:
                // 세이브 로드
                break;

            case 2:
                // 세이브 로드
                break;

            case 3:
                // 세이브 로드
                break;
        }

        StartCoroutine(FadeOut());
    }

    public void ResetData(int saveFileNumber = 0)
    {
        switch (saveFileNumber)
        {
            case 1:
                // 세이브 삭제
                break;

            case 2:
                // 세이브 삭제
                break;

            case 3:
                // 세이브 삭제
                break;
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    IEnumerator FadeOut()
    {
        blockImage.gameObject.SetActive(true);
        Color C = new Color();
        
        for (; C.a < 1;) 
        {
            C.a += 0.2f;
            blockImage.color = C;
            yield return new WaitForSeconds(0.04f);
        }
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneNum, LoadSceneMode.Single);
    }
}
