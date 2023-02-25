using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SanityController : MonoBehaviour
{
    private Slider sSlider;
    [SerializeField] private Fade fade;
    public float sanityValue;
    private bool CR_running = false;
    [SerializeField] private float blinkMinTime;
    [SerializeField] private float blinkMaxTime;

    void Start()
    {
        sSlider = GetComponent<Slider>();
        sanityValue = sSlider.value;
    }

    
    void Update()
    {
        sanityValue = sanityValue > 1 ? 1 : sanityValue <= 0 ? 0.001f : sanityValue;
        sSlider.value = sanityValue;

        if (!CR_running)
        {
            StartCoroutine(blink(Random.Range(blinkMinTime, blinkMaxTime)));
        }

        sanityBehaviour();
    }

    public void instantlyChangeSanity(float destination)
    {
        sanityValue = destination;
    }

    public void linearlyChangeSanity(float amount, float durationInSeconds)
    {
        if (amount < 0 && amount > sanityValue) amount = sanityValue;
        else if (amount > 0 && amount + sanityValue > 1) amount = 1.001f - sanityValue;
        StartCoroutine(ChangeSanity(amount, durationInSeconds));
    }

    private void sanityBehaviour()
    {
        // just math; put it in desmos if you want to see the graphs
        blinkMinTime = sanityValue + Mathf.Abs(Mathf.Sqrt(sanityValue) * sanityValue / (Mathf.Log(sanityValue) - 1)) + 0.25f;
        blinkMaxTime = sanityValue + Mathf.Abs(1 + sanityValue * Mathf.Log(sanityValue)) + Mathf.Sqrt(sanityValue);

        // Below 75%
        if (sanityValue < 0.75f)
        {
        }

        // Below 50%
        if (sanityValue < 0.5f)
        {
        }

        // Below 25%
        if (sanityValue < 0.25f)
        {
        }

        // Below 10%
        if (sanityValue < 0.1f)
        {
        }

        // 0
        if (sanityValue <= 0.0001f)
        {
        }
    }

    private IEnumerator blink(float afterDelay)
    {
        CR_running = true;

        yield return StartCoroutine(addDelay(afterDelay));
        fade.QuickFadeIn();
        fade.QuickFadeOut();

        CR_running = false;

    }

        
    private IEnumerator addDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    private IEnumerator ChangeSanity(float amount, float durationInSeconds)
    {
        float destination = sanityValue + amount;
        float originalVal = sanityValue;
        
        for (float elapsedTime = 0f; elapsedTime < durationInSeconds; elapsedTime += Time.deltaTime) 
        {
            sanityValue = Mathf.Lerp(originalVal, destination, elapsedTime / durationInSeconds);
            yield return null;
        }
        sanityValue = destination;
    }

}
