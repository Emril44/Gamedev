using UnityEngine;

// Objective whose completion is triggered by going to a specific area in the world. Entry tracking is delegated to ObjectiveZone
[CreateAssetMenu(fileName = "New Location Objective", menuName = "Location Objective")]
public class LocationObjective : Objective
{
    private bool active = false;

    override public void SetActive(bool active)
    {
        this.active = active;
    }

    public void ReactToEntry()
    {
        if (active) Complete();
    }
}