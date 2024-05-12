using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExitPanelHandling : MonoBehaviour
{
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public async void DisbaleSelf() {

        await Task.Delay(400);
        panel.gameObject.SetActive(false);
    }
    public async void EnableLesson()
    {
       await Task.Delay(400);
        RefrenceManager.instance.lessonScreen.ExitLesson();
    }
}
