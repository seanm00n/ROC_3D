using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    [SerializeField] private int sceneNum = 0;
    private Animator animator;
    
    [Header("BlockScreen")]
    public Image blockImage;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void StartButton()
    {
        StartCoroutine(FadeOut());
    }
    public void LoadButton()
    {
        animator.SetTrigger("save");
        
    }

    public void LoadBoxExitButton()
    {
        animator.SetTrigger("exit");
    }



    public void ExitButton()
    {
        Application.Quit();
    }

    IEnumerator FadeOut() // Screen FadeOut
    {
        Time.timeScale = 1;
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
