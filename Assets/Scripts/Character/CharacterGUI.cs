using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGUI : MonoBehaviour
{
    
    [SerializeField]
    public CharStats charStats;
    private Slider _healthSlider;
    private CharacterController _controller;
    void Start()
    {
        _controller = GetComponentInParent<CharacterController>();
        _healthSlider = GetComponentInChildren<Slider>();
        
        
        _healthSlider.maxValue = charStats.GetStat(Stat.Health);
        _healthSlider.value = charStats.GetStat(Stat.Health);
    }

    public void TakeDamageAnimate(float diff, int animationLength)
    {
        float startValue = _healthSlider.value;
        float targetValue = startValue + diff;
        
        
        StartCoroutine(InterpValue(startValue, targetValue, animationLength));
    }
    
    private IEnumerator InterpValue(float startValue, float targetValue, int animationLength)
    {
        float elapsedTime = 0f;
        while (elapsedTime < animationLength)
        {
            // Calculate the interpolated value between startValue and targetValue based on the elapsed time
            float t = elapsedTime / animationLength;
            float interpolatedValue = Mathf.Lerp(startValue, targetValue, t);
            
            // Update the slider value
            _healthSlider.value = interpolatedValue;
            // Increment the elapsed time
            
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the slider value is set to the target value at the end
        SetHP(targetValue);
        TurnManager.GetActivePlayer().EndOtherActionPhase();
    }

    private void SetHP(float value)
    {
        _healthSlider.value = value;
    }

    public void ResetHP()
    {
        SetHP(_healthSlider.maxValue);
    }
}
