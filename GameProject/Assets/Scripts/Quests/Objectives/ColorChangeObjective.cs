using UnityEngine;

// Objective whose completion is triggered when the world is changed to a specific color using a prism shard
[CreateAssetMenu(fileName = "New Color Change Objective", menuName = "Color Change Objective")]
public class ColorChangeObjective : Objective
{
    [field: SerializeField] public PrismColor color { get; private set; }

    public override void SetActive(bool active)
    {
        if (active)
        {
            if (EnvironmentManager.Instance.CurrentColor == color)
            {
                Complete();
                return;
            }
            EnvironmentManager.Instance.onColorChange += CheckForCompletion;
        }
        else
        {
            EnvironmentManager.Instance.onColorChange -= CheckForCompletion;
        }
    }

    private void CheckForCompletion()
    {
        if (EnvironmentManager.Instance.CurrentColor == color)
        {
            Complete();
        }
    }

}

