using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest
{
    public bool isActive { get; private set; }

    public abstract bool CheckCompletion();
}
