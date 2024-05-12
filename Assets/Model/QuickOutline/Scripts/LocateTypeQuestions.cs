using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LocateTypeQuestions : MonoBehaviour
{
    public List<Outline> outlineObjects;

    int i = 0;
    
    public async Task HighlitObjects(int number)
    {

        outlineObjects[number].GetComponent<GetElements>().SelectableID = i;
        i++;
        /*outlineObjects[i].GetComponent<Outline>().enabled = true;*/

    }



    public void HighlitObjectsSetToOff()
    {
        i = 0;
        foreach (var obj in outlineObjects)
        {
            obj.GetComponent<Outline>().enabled = false;
            GetElements element = obj.GetComponent<GetElements>();

            //reset materials
            for (int i = 0; i < element.hoverObjects.Length; i++)
            {
                element.hoverObjects[i].GetComponent<MeshRenderer>().material = element.defaultMaterial[i];
            }
            //obj.GetComponent<MeshRenderer>().material = obj.GetComponent<GetElements>().defaultMaterial[0];
        }
    }

    public void ResetMaterial()
    {

    }


    public void AssignDiffrentTagsObjects(int index)
    {

        outlineObjects[index].gameObject.tag = Constants.tagforSelectableOption;
        
        if (outlineObjects[index].GetComponent<MeshCollider>())
        {
            outlineObjects[index].GetComponent<MeshCollider>().enabled = true;
            return;
        }
        outlineObjects[index].GetComponent<BoxCollider>().enabled = true;
    }

    public void ReasignDefaultTagsObjects()
    {
        foreach (var outlineObj in outlineObjects)
        {
            outlineObj.gameObject.tag = "Untagged";
            outlineObj.gameObject.transform.localScale = Constants.originalScale;
            if (outlineObj.GetComponent<MeshCollider>())
            {
                outlineObj.GetComponent<MeshCollider>().enabled = false;

            }
            else
            {

                outlineObj.GetComponent<BoxCollider>().enabled = false;
            }
        }

    }
}
