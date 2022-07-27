using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private int sceneNum = 0;
    Animator anim;
    
    [Header("BlockScreen")]
    public Image blockImage;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void StartButton()
    {
        // Data Reset
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
                // Load save
                break;

            case 2:
                // Load save
                break;

            case 3:
                // Load save
                break;
        }

        StartCoroutine(FadeOut());
    }

    public void ResetData(int saveFileNumber = 0)
    {
        switch (saveFileNumber)
        {
            case 1:
                // Delete save
                break;

            case 2:
                // Delete save
                break;

            case 3:
                // Delete save
                break;
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    IEnumerator FadeOut() // Screen FadeOut
    {
        blockImage.gameObject.SetActive(true);
        Color c = new Color();
        
        for (; c.a < 1;) 
        {
            c.a += 0.2f;
            blockImage.color = c;
            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneNum, LoadSceneMode.Single);
    }
}
