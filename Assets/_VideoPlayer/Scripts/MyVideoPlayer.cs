using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Experimental.Video;
using UnityEngine.Rendering.Universal;

public class MyVideoPlayer : MonoBehaviour
{
    [SerializeField] private VideoClip[] videoClips;
    public Camera camera;
    public GameObject cinemaPlane;
    public GameObject btnPlay;
    public GameObject btnPause;
    public GameObject knob;
    public GameObject progressBar;
    public GameObject progressBarBG;

    public GameObject pauseImage;
    public GameObject resumeImage;



    private float maxKnobValue;
    private float newKnobX;
    private float maxKnobX;
    private float minKnobX;
    private float knobPosY;
    private float simpleKnobValue;
    private float knobValue;
    private float progressBarWidth;
    private bool knobIsDragging;
    private bool videoIsJumping = false;
    private bool videoIsPlaying = false;
    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;
    public RawImage image;

    private void Awake()
    {
        knobPosY = knob.transform.localPosition.y;
        if (!videoPlayer) 
            videoPlayer = GetComponent<VideoPlayer>();
        image.texture = null;
        btnPause.SetActive(true);
        btnPlay.SetActive(false);
        videoPlayer.frame = (long)100;
        progressBarWidth = 8.7f;
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Start  ()
    {

        
    }

    private void OnEnable()
    {
        PlayVideoOnEnable();
    }

    private void Update()
    {
        if (!knobIsDragging && !videoIsJumping)
        {
            if (videoPlayer.frameCount > 0)
            {
                float progress = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
                
                progressBar.transform.localScale = new Vector3(progressBarWidth * progress, progressBar.transform.localScale.y, 0);
                knob.transform.localPosition = new Vector2(progressBar.transform.localPosition.x + (progressBarWidth * progress), knob.transform.localPosition.y);
            }
             
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            Collider2D hitCollider = Physics2D.OverlapPoint(camera.ScreenToWorldPoint(pos));

            if (hitCollider != null && hitCollider.CompareTag(btnPause.tag))
            {
                BtnPlayVideo();
            }
            if (hitCollider != null && hitCollider.CompareTag(btnPlay.tag))
            {
                print("playBtn");
                BtnPlayVideo();
            }
        }
    }


    private void OnVideoEnd(VideoPlayer vp)
    {

        RefrenceManager.instance.uIManager.fadeEffect.VideoFadeOut(3f);
    }

    public void KnobOnPressDown()
    {
        VideoStop();
        minKnobX = progressBar.transform.localPosition.x;
        maxKnobX = minKnobX + progressBarWidth;
    }

    public void KnobOnRelease()
    {
        knobIsDragging = false;
        CalcKnobSimpleValue();
        VideoPlay();
        VideoJump();
        StartCoroutine(DelayedSetVideoIsJumpingToFalse());
    }

    IEnumerator DelayedSetVideoIsJumpingToFalse()
    {
        yield return new WaitForSeconds(2);
        SetVideoIsJumpingToFalse();
    }

    public void KnobOnDrag()
    {
        knobIsDragging = true;
        videoIsJumping = true;
        Vector3 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        knob.transform.position = new Vector2(curPosition.x, curPosition.y);
        newKnobX = knob.transform.localPosition.x;
        if (newKnobX > maxKnobX) { newKnobX = maxKnobX; }
        if (newKnobX < minKnobX) { newKnobX = minKnobX; }
        knob.transform.localPosition = new Vector2(newKnobX, knobPosY);
        CalcKnobSimpleValue();
        progressBar.transform.localScale = new Vector3(simpleKnobValue * progressBarWidth, progressBar.transform.localScale.y, 0);
    }

    private void SetVideoIsJumpingToFalse()
    {
        videoIsJumping = false;
    }

    private void CalcKnobSimpleValue()
    {
        maxKnobValue = maxKnobX - minKnobX;
        knobValue = knob.transform.localPosition.x - minKnobX;
        simpleKnobValue = knobValue / maxKnobValue;
    }

    public void PlayVideoOnEnable()
    {
        if (Constants.currentLesson - 1 < videoClips.Length)
        {
            Debug.Log("video available: ");
            videoPlayer.clip = videoClips[Constants.currentLesson - 1];
            VideoPlay();
        }
        else
        {
            Debug.Log("video is not available: ");
            RefrenceManager.instance.uIManager.fadeEffect.VideoFadeOut(0f);
        }
        
    }
    private void VideoJump()
    {
        var frame = videoPlayer.frameCount * simpleKnobValue;
        videoPlayer.frame = (long)frame;
    }

    public void BtnPlayVideo()
    {
        if (videoIsPlaying)
        {
            VideoStop();
            StartCoroutine(VideoPauseAnimation()); 

        }
        else
        {
            VideoPlay();
            StartCoroutine(VideoPlayAnimation()); 
        }
    }

    IEnumerator VideoPauseAnimation()
    {
        pauseImage.SetActive(false);
        resumeImage.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
    }


    IEnumerator VideoPlayAnimation()
    {
        resumeImage.SetActive(false);
        pauseImage.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        pauseImage.SetActive(false);
    }


    public void VideoStop()
    {
        videoIsPlaying = false;
        videoPlayer.Pause();
    }

   
   public void VideoPlay()
    {
        Debug.Log("VideoPLaying");
        image.texture = renderTexture;
        videoIsPlaying = true;
        videoPlayer.Play();
    }

    public void SKipVideo()
    {
        RefrenceManager.instance.uIManager.fadeEffect.VideoFadeOut(0f);
    }
}
