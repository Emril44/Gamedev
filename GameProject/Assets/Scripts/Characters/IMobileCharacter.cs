using System.Collections;
using UnityEngine;

// Character that can move to locations and jump, controlled by a script (for cutscenes)
// Implementations should also manage contacts with objects tagged as Moving
public interface IMobileCharacter
{
    public IEnumerator MoveTo(Vector3 position, float velocity);
    public void Jump(float jumpVelocity);
}
