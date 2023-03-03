using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    #region definitions
    private Slider hSlider;
    [SerializeField] private SanityController sController;
    [SerializeField] private float healthValue;
    private bool PHR_running = false, CR_running = false;
    [SerializeField] private bool passiveHealthRegen = true;
    private float destination, originalVal, duration;
    #endregion

    void Start()
    {
        hSlider = GetComponent<Slider>();
        healthValue = hSlider.value;
    }

    void Update()
    {
        healthValue = healthValue > 1 ? 1 : healthValue <= 0 ? 0.001f : healthValue;
        hSlider.value = healthValue;

        if (!PHR_running && passiveHealthRegen)
        {
            StartCoroutine(PassiveHealthRegen(1));
        }
    }

    /* PUBLIC METHODS */

    public void instantlyChangeHealth(float towards)
    {
        if (towards < healthValue)
            StartCoroutine(startPassiveHealthRegenDelay(4));

        healthValue = towards;
    }

    public void linearlyChangeHealth(float amount, float durationInSeconds)
    {
        if (amount < 0 && amount > healthValue) amount = healthValue;
        else if (amount > 0 && amount + healthValue > 1) amount = 1.001f - healthValue;

        if (amount < 0) passiveHealthRegen = false;

        if (!CR_running) StartCoroutine(changeHealth(amount, durationInSeconds));
        else { destination += amount; duration = (durationInSeconds + duration) / 2; }
    }

    /* PASSIVE HEALTH REGEN */

    private IEnumerator PassiveHealthRegen(float afterDelay)
    {
        PHR_running = true;

        yield return StartCoroutine(addDelay(afterDelay));
        instantlyChangeHealth(healthValue + 0.01f);

        PHR_running = false;

    }

    private IEnumerator startPassiveHealthRegenDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        passiveHealthRegen = true;
    }

    /* HEALTH CHANGING COROUTINE */
    private IEnumerator changeHealth(float amount, float durationInSeconds)
    {
        CR_running = true;
        destination = healthValue + amount;
        originalVal = healthValue;
        duration = durationInSeconds;

        for (float elapsedTime = 0f; elapsedTime < durationInSeconds; elapsedTime += Time.deltaTime)
        {
            healthValue = Mathf.Lerp(originalVal, destination, elapsedTime / durationInSeconds);
            yield return null;
        }
        healthValue = destination;

        if (amount < 0)
            StartCoroutine(startPassiveHealthRegenDelay(4));

        CR_running = false;
    }

    /* DELAY ADDER */

    private IEnumerator addDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
   
}
