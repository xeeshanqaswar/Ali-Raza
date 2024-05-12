using System;
using System.Collections.Generic;
using UnityEngine;

namespace Drag_Drop_Question
{
    [CreateAssetMenu(menuName = "ScriptableObject/Question/Drag Drop")]
    public class DragDropQuestionSO : ScriptableObject
    {
        public List<DragDropQuestionData> dragDropQuestions = new List<DragDropQuestionData>();
    }

    [Serializable]
    public class DragDropQuestionData
    {
        public int questionNumber;
        public string lesson;
        public string question;
    }
}
