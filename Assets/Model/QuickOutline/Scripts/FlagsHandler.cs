using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FlagsHandler : MonoBehaviour
{
    [SerializeField] List<FlagsId> flags;
    [SerializeField] Transform camerTransform;
    List<GameObject> flagsWithIndex = new List<GameObject>();
    int lastFlagNo = -1 ;

    

    public void RaiseFlag(int flagNo, string optionText)
    {
    
        flags[flagNo-1].gameObject.SetActive(true);
        flags[flagNo-1].GetComponentInChildren<Text>().text = optionText;
        lastFlagNo = flagNo-1;

    }


    public List<string> flagsId = new List<string>();

    /// <summary>
    /// perform actions for flag raise
    /// </summary>
    /// <param name="highlightedPoints"></param>
    public void SetFlagRaise(List<string> highlightedPoints)
    {
        
        foreach (var flag in flags)
        {
            flagsId.Add(flag.Id);
        }

        // unraise all raised flags
        UnraiseFlags();

        // clear list of first
        flagsWithIndex.Clear();


        for (int i = 0; i < highlightedPoints.Count; i++)
        {
            ProcessHighlightedPoint(highlightedPoints[i]);
        }



    }

    /// <summary>
    /// raise flag for specific question
    /// </summary>
    /// <param name="index"></param>
    void RaiseFlag(int index)
    {
        
        flags[index].gameObject.SetActive(true);
    }


    /// <summary>
    /// unraise flags when we dont use them
    /// </summary>
    public void UnraiseFlags()
    {
        if (flagsId.Count > 0)
        {
            foreach (var flag in flagsWithIndex)
            {
                flag.SetActive(false);
            }
        }
        
    }

    /// <summary>
    /// check and match highlighted points
    /// </summary>
    /// <param name="highlightedPoint"></param>
    void ProcessHighlightedPoint(string highlightedPoint)
    {
        for (int j = 0; j < flagsId.Count; j++)
        {
            if (highlightedPoint == flagsId[j])
            {
               
                flags[j].GetComponentInChildren<Text>().text = flags[j].option;
                flagsWithIndex.Add(flags[j].gameObject);
                RaiseFlag(j);
                break; // Break out of the inner loop when a match is found
            }
        }
    }



}
