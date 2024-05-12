using DG.Tweening;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUI.Tabs
{
    [DisallowMultipleComponent]
    [AddComponentMenu("EasyUI/Tabs/Tab Button")]
    public class TabButtonUI : MonoBehaviour
    {
        public Button button;
        public Image image { get; private set; }

        #region rippleAnimation

        public GameObject ripplePrefab;
        public float rippleScale = 1f;
        public float rippleDuration = 0.5f;
        public float offsetX = 10f; //
        #endregion


        public Text lessonName;
        public Text lessonNo;
        public Image fillBar;
        public GameObject greenCheck, orangeCheck;
        public Image inactiveCircle;
        public Sprite activeCircle, inactiveCircleSprite, fullCircleSprite;
        public Sprite activeButtonOutline, inactiveButton, orangeButton, activeGreenButton;
        public Color activeTextColor, inactiveTextColor, activeButtonTextColor;

        public GameObject contentDuplicate, oldContent;

        Image greenCheckColor, orangeCheckColor, circleColor, fillbarColor;


        // public LayoutElement layoutElement { get; private set; }

        private void OnValidate()
        {
            button = GetComponent<Button>();
            image = GetComponent<Image>();


        }

        private void Awake()
        {
            greenCheckColor = greenCheck.GetComponent<Image>();
            orangeCheckColor = orangeCheck.GetComponent<Image>();
            circleColor = inactiveCircle.GetComponent<Image>();
            fillbarColor = fillBar.GetComponent<Image>();
        }

        public void ResetButtonContents()
        {
            lessonNo.color = inactiveTextColor;
            lessonName.color = inactiveTextColor;
            button.GetComponent<Image>().sprite = inactiveButton;
            inactiveCircle.sprite = inactiveCircleSprite;
        }

        public void Complete_UnSelectedLesson()
        {
            button.GetComponent<Image>().sprite = activeButtonOutline;
            inactiveCircle.sprite = activeCircle;
            lessonName.color = activeTextColor;
            lessonNo.color = activeTextColor;
        }

        public void Locked_UnSelectedLesson()
        {
            button.GetComponent<Image>().sprite = inactiveButton;
            inactiveCircle.sprite = inactiveCircleSprite;
            lessonName.color = inactiveTextColor;
            lessonNo.color = inactiveTextColor;
            greenCheck.SetActive(false);
            orangeCheck.SetActive(false);
        }

        public void Unlocked_UnselectedLesson()
        {

            button.GetComponent<Image>().sprite = activeButtonOutline;
            inactiveCircle.sprite = activeCircle;
            lessonName.color = activeTextColor;
            lessonNo.color = activeTextColor;
        }

        public void ActiveButtonTheme()
        {
            button.GetComponent<Image>().sprite = orangeButton;
            inactiveCircle.sprite = fullCircleSprite;
            lessonName.color = activeTextColor;
            lessonNo.color = activeButtonTextColor;
        }

        public void CheckMarkStatus(int correct, int total)
        {
            if (correct.ToString() == total.ToString())
            {
                greenCheck.SetActive(true);
                orangeCheck.SetActive(false);
            }
            else
            {
                greenCheck.SetActive(false);
                orangeCheck.SetActive(true);
            }
        }

        public void DisableAllContents()
        {
            SetAlpha(0, circleColor);
            SetAlpha(0, greenCheckColor);
            SetAlpha(0, orangeCheckColor);
            SetAlpha(0, fillbarColor);

            lessonName.gameObject.SetActive(false);
            lessonNo.gameObject.SetActive(false);
            this.GetComponent<Image>().enabled = false;
        }

        public void EnableAllContents()
        {
            contentDuplicate.SetActive(false);
            //oldContent.SetActive(true);

            SetAlpha(1, circleColor);
            SetAlpha(1, greenCheckColor);
            SetAlpha(1, orangeCheckColor);
            SetAlpha(1, fillbarColor);

            lessonName.gameObject.SetActive(true);
            lessonNo.gameObject.SetActive(true);

            this.GetComponent<Image>().enabled = true;
        }

        void SetAlpha(float alpha, Image _image)
        {
            Color color = _image.color; 
            color.a = alpha;
            _image.color = color;
        }

        public void Animation()
        {
            RectTransform rt = Instantiate(ripplePrefab, transform).GetComponent<RectTransform>();
            rt.localScale = Vector3.zero;

            // Set the initial position to the center of the button
            rt.anchoredPosition = Vector2.zero;

            // Use DOTween to animate the ripple effect
            rt.DOScale(Vector3.one * 2.5f, 0.5f).SetEase(Ease.Linear).OnComplete(() => Destroy(rt.gameObject));


        }

    }
}

