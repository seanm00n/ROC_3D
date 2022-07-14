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
        // ������ �ʱ�ȭ
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
                // ���̺� �ε�
                break;

            case 2:
                // ���̺� �ε�
                break;

            case 3:
                // ���̺� �ε�
                break;
        }

        StartCoroutine(FadeOut());
    }

    public void ResetData(int saveFileNumber = 0)
    {
        switch (saveFileNumber)
        {
            case 1:
                // ���̺� ����
                break;

            case 2:
                // ���̺� ����
                break;

            case 3:
                // ���̺� ����
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
