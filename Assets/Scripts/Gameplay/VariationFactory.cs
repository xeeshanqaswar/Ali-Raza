using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariationFactory : MonoBehaviour
{
    [SerializeField] private LessonData lessonData;

    public Variation CreateRandomVariation()
    {
        // Choose a random lesson
        int lessonIndex = Random.Range(0, lessonData.lessons.Length);

        // Choose a random question from the selected lesson
        int questionIndex = Random.Range(0, lessonData.lessons[lessonIndex].questions.Length);

        // Choose a random variation from the selected question
        int variationIndex = Random.Range(0, lessonData.lessons[lessonIndex].questions[questionIndex].variations.Length);

        return lessonData.lessons[lessonIndex].questions[questionIndex].variations[variationIndex];
    }


    /*public class ExampleUsage : MonoBehaviour
    {
        [SerializeField] private VariationFactory variationFactory;

        private void Start()
        {
            // Create a random variation using the VariationFactory
            Variation randomVariation = variationFactory.CreateRandomVariation();

            // Access the properties of the random variation
            Debug.Log($"Question: {randomVariation.question}");
            Debug.Log($"Side: {randomVariation.side}");
            Debug.Log($"Answer: {randomVariation.answer}");
            Debug.Log($"Highlighted Point: {randomVariation.highlightedPoint}");
        }
    }*/


    [System.Serializable]
    public class Variation
    {
        public string question;
        public string side;
        public int answer;
        public int highlightedPoint;
    }

    [System.Serializable]
    public class Question
    {
        public int questionNumber;
        public Variation[] variations;
    }

    [System.Serializable]
    public class Lesson
    {
        public int lessonNumber;
        public Question[] questions;
    }

    [System.Serializable]
    public class LessonData
    {
        public Lesson[] lessons;
    }

}