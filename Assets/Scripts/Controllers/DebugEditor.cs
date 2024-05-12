using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEditor : MonoBehaviour
{
    #region variables
    public bool unlockAllLevels;
    public bool isTesting = false;
    public bool LessonOne;
    public bool LessonTwo;
    public bool LessonThree;
    public bool LessonFour;
    public bool LessonFive;
    public bool LessonSix;
    public NeoData neoData;

    #endregion

    private void Start()
    {
        StartCoroutine(RemoveListElements());
    }

    /// <summary>
    /// Remove questions from list to test some questions not all
    /// </summary>
    /// <returns></returns>
     IEnumerator RemoveListElements()
    {
        yield return new WaitForSecondsRealtime(3);

        int listCountLesson1 = neoData.lesson.lessons[0].questions.Count;
/*        int listCountLesson2 = neoData.lesson.lessons[1].questions.Count;
        int listCountLesson3 = neoData.lesson.lessons[2].questions.Count;
        int listCountLesson4 = neoData.lesson.lessons[3].questions.Count;
        int listCountLesson5 = neoData.lesson.lessons[4].questions.Count;
        int listCountLesson6 = neoData.lesson.lessons[5].questions.Count;
        */
        
   
        if (isTesting)
        {

            if (LessonOne)
            {
                neoData.lesson.lessons[0].questions.RemoveRange(0, listCountLesson1 - 1);
            }
            /*            if (LessonTwo)
                        {
                            neoData.lesson.lessons[1].questions.RemoveRange(1, listCountLesson2-1);
                        }
                        if (LessonThree)
                        {
                            neoData.lesson.lessons[2].questions.RemoveRange(1, listCountLesson3-1);
                        }
                        if (LessonFour)
                        {
                            neoData.lesson.lessons[3].questions.RemoveRange(1, listCountLesson4-1);
                        }
                        if (LessonFive)
                        {
                            neoData.lesson.lessons[4].questions.RemoveRange(1, listCountLesson5-1);
                        }
                        if (LessonSix)
                        {
                            neoData.lesson.lessons[5].questions.RemoveRange(1, listCountLesson6-1);
                        }*/

        }
    }
}
