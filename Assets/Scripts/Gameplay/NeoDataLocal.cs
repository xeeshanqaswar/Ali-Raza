using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NeoData;

[CreateAssetMenu(fileName = "NeoDataLocal", menuName = "Custom/Neo Data Local", order = 2)]
public class NeoDataLocal : ScriptableObject
{



    [System.Serializable]
    public struct Led
    {
        public bool changeEmission;
        public int index;
        public Color color;
        public bool isFlashing;
        public bool isChangingColor;
        public Color changingColor;
        public bool flashingBackForth;
    }

    [System.Serializable]

    public class Options
    {

        public string optionText;
        public bool correct;
        public int highlitedPoints;
    }

    [System.Serializable]
    public class Question
    {
        public QuestionType type;
        public string questionname;
        public bool isConstraint;
        public bool someConstraint;
        public bool enableZoom;
        public string questionText;
        public int cameraAngle; // For Locate type questions
        //public string[] optionTexts;
        public Options[] options;   
        public int[] highlitedPoints;
        public int correctAnswerIndex;
        public Led[] led;
        public int questionNumber;
    }
    [System.Serializable]
    public class Lesson
    {
        public string name;
     
        public Question[] questions;
    }


    public Lesson[] lessons;

   
}