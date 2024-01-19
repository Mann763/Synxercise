using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObjectsManager : MonoBehaviour
{
    public FloatingObject[] floatingObjects; // Assign your floating objects in the Inspector

    void Start()
    {
        floatingObjects = GetComponentsInChildren<FloatingObject>();

        foreach (var floatingObject in floatingObjects)
        {
            floatingObject.Initialize();
        }
    }

    void Update()
    {
        foreach (var floatingObject in floatingObjects)
        {
            floatingObject.UpdateFloating();
        }
    }
}
