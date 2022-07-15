using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_Cloud : MonoBehaviour
{
    int speed;
    RectTransform myRect;
    // Start is called before the first frame update
    void Start()
    {
        myRect = GetComponent<RectTransform>();
        speed = Random.Range(1,6);
    }

    // Update is called once per frame
    void Update()
    {
        if (myRect.anchoredPosition.x > 2800)
        {
            speed = Random.Range(1, 6);
            myRect.anchoredPosition = new Vector2(-4444, Random.Range(-770, 2300));
        }
        myRect.anchoredPosition = new Vector2(myRect.anchoredPosition.x + speed * 20 * Time.deltaTime, myRect.anchoredPosition.y);
        
    }
}
