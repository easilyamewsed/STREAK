using System.Collections;
using UnityEngine;

public class TransitionAnimation : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayTransitionAnimation();
        }
    }

    public void PlayTransitionAnimation()
    {
        StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
    }
}

