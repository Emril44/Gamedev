using UnityEngine;

// Objective whose completion is triggered when a certain threshold of sparks is reached
// Objective's message is modified to include " (m/n)" after the defined message, where m is current spark count and n is target
[CreateAssetMenu(fileName = "New Sparks Objective", menuName = "Sparks Objective")]
public class SparksObjective : Objective
{
    [field: SerializeField] public int targetCount { get; private set; }

    public override void SetActive(bool active)
    {
        if (active)
        {
            if (DataManager.Instance.sparksAmount >= targetCount)
            {
                Complete();
                return;
            }
            DataManager.Instance.onSparkCollect += CheckForCompletion;
        }
        else
        {
            DataManager.Instance.onSparkCollect -= CheckForCompletion;
        }
    }

    private void CheckForCompletion()
    {
        if (DataManager.Instance.sparksAmount >= targetCount)
        {
            Complete();
        }
    }

    public override string GetMessage()
    {
        return message + " (" + DataManager.Instance.sparksAmount + "/" + targetCount + ")";
    }

}

