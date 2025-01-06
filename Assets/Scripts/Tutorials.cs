using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorials : MonoBehaviour
{
    public Image displayImage;
    public Sprite[] imageList;
    private int currentIndex = 0;
    public Button nextButton;
    public Button prevButton;
    public Button hideButton;
    public Button showButton;

    public Image backgroundImageBlack;
    public Image backgroundImageSand;

    public GameObject gameObjectMenu1;
    public GameObject gameObjectMenu2;

    public Animator transition;
    public float transitionTime = 1f;
    public string enterLeftTrigger = "EnterLeft";
    public string exitRightTrigger = "ExitRight";
    public string enterRightTrigger = "EnterRight";
    public string exitLeftTrigger = "ExitLeft";
    public string IdleTrigger = "Idle";

    void Start()
    {
        displayImage.gameObject.SetActive(false);
        nextButton.onClick.AddListener(NextTutorial);
        prevButton.onClick.AddListener(PrevTutorial);
        hideButton.onClick.AddListener(() => StartCoroutine(HideTransitionSequence()));
        showButton.onClick.AddListener(() => StartCoroutine(ShowTransitionSequence()));
        backgroundImageBlack.gameObject.SetActive(true);
        backgroundImageSand.gameObject.SetActive(false);
        gameObjectMenu1.SetActive(true);
        gameObjectMenu2.SetActive(false);
    }

    void NextTutorial()
    {
        currentIndex = (currentIndex + 1) % imageList.Length;
        displayImage.sprite = imageList[currentIndex];
    }

    void PrevTutorial()
    {
        currentIndex = (currentIndex - 1 + imageList.Length) % imageList.Length;
        displayImage.sprite = imageList[currentIndex];
    }

    void HideTutorial()
    {
        displayImage.gameObject.SetActive(false);
        backgroundImageBlack.gameObject.SetActive(true);
        backgroundImageSand.gameObject.SetActive(false);
        gameObjectMenu1.SetActive(true);
        gameObjectMenu2.SetActive(false);
    }

    void ShowTutorial()
    {
        displayImage.gameObject.SetActive(true);
        displayImage.sprite = imageList[0];
        displayImage.rectTransform.anchoredPosition = Vector2.zero;
        backgroundImageBlack.gameObject.SetActive(false);
        backgroundImageSand.gameObject.SetActive(true);
        gameObjectMenu1.SetActive(false);
        gameObjectMenu2.SetActive(true);
    }

    IEnumerator ShowTransitionSequence()
    {
        transition.SetTrigger(enterLeftTrigger);
        yield return new WaitForSeconds(transitionTime);
        ShowTutorial();
        transition.SetTrigger(exitRightTrigger);
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger(IdleTrigger);
    }

    IEnumerator HideTransitionSequence()
    {
        transition.SetTrigger(enterRightTrigger);
        yield return new WaitForSeconds(transitionTime);
        HideTutorial();
        transition.SetTrigger(exitLeftTrigger);
        yield return new WaitForSeconds(transitionTime);
        transition.SetTrigger(IdleTrigger);
    }

}
