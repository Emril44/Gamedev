using System.Collections;
using UnityEngine;

public class LiquidLever : Lever
{
    [SerializeField] private GameObject liquid;
    private Vector3 liquidScale1, liquidScale2;
    private Vector3 liquidPosition1, liquidPosition2;
    [SerializeField] private bool defaultFilled; // if true, default state of target liquid will be filled (its on-awake position and scale). otherwise, it will be empty (disabled) by default
    [SerializeField] private float fillDuration;

    private void Awake()
    {
        if (defaultFilled)
        {
            liquidScale1 = liquid.transform.localScale;
            liquidPosition1 = liquid.transform.position;
            liquidScale2 = liquid.transform.localScale - new Vector3(0, liquid.transform.localScale.y, 0); // empty liquid scale
            liquidPosition2 = liquid.transform.position - new Vector3(0, liquid.transform.localScale.y / 2, 0); // empty liquid location
        }
        else
        {
            liquidScale2 = liquid.transform.localScale;
            liquidPosition2 = liquid.transform.position;
            liquidScale1 = liquid.transform.localScale - new Vector3(0, liquid.transform.localScale.y, 0); // empty liquid scale
            liquidPosition1 = liquid.transform.position - new Vector3(0, liquid.transform.localScale.y / 2, 0); // empty liquid location
            liquid.transform.localScale = liquidScale1;
            liquid.transform.position = liquidPosition1;
        }
    }

    public override Vector3 LookPosition()
    {
        return liquidPosition1;
    }

    protected override IEnumerator DoAction()
    {
        float time = 0;
        isLocked = true;
        while (time < fillDuration)
        {
            time += Time.fixedDeltaTime;
            if (isOn)
            {
                liquid.transform.position = Vector3.Lerp(liquidPosition1, liquidPosition2, time/fillDuration);
                liquid.transform.localScale = Vector3.Lerp(liquidScale1, liquidScale2, time / fillDuration);
            }
            else
            {
                liquid.transform.position = Vector3.Lerp(liquidPosition2, liquidPosition1, time / fillDuration);
                liquid.transform.localScale = Vector3.Lerp(liquidScale2, liquidScale1, time / fillDuration);
            }
            yield return new WaitForFixedUpdate();
        }
        if (isOn)
        {
            liquid.transform.position = liquidPosition2;
        }
        else
        {
            liquid.transform.position = liquidPosition1;
        }
        isLocked = false;
    }
}
