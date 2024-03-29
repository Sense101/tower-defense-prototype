using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPoolHandler<O> : MonoBehaviour where O : PoolObject
{
    // lists of objects
    public List<O> objects = new List<O>();
    public List<O> availableObjects = new List<O>();

    // set in inspector
    public O objectPrefab = default;
    public int startingObjects = 0;

    public virtual void Awake()
    {
        CreateStartingObjects();
    }

    private void CreateStartingObjects()
    {
        for (int i = 0; i < startingObjects; i++)
        {
            // create and deactivate the starting object
            O newObject = Instantiate(objectPrefab, Vector2.zero, Quaternion.identity, transform);
            newObject.Deactivate();

            // add to lists
            objects.Add(newObject);
            availableObjects.Add(newObject);
        }
    }

    /// <summary>
    /// "creates" a new object under the specified parent
    /// </summary>
    /// <param name="location"></param>
    /// <param name="rotation"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    protected virtual O CreateObject(Vector2 location, Angle rotation, Transform parent)
    {

        O newObject;

        if (availableObjects.Count > 0)
        {
            // we have an existing object we can use
            newObject = availableObjects[0];
            availableObjects.RemoveAt(0);

            // move it to where it should be
            newObject.SetLocation(location);
            newObject.SetRotation(rotation);
            newObject.SetParent(parent);
        }
        else
        {
            // we need to create a new object
            newObject = InstantiateObject(location, rotation, parent);
        }

        return newObject;
    }

    protected virtual O InstantiateObject(Vector2 location, Angle rotation, Transform parent)
    {
        // instantiate
        O newObject = Instantiate(objectPrefab, location, rotation.AsQuaternion(), parent);

        // set references
        newObject.SetReferences();

        // add to list
        objects.Add(newObject);

        return newObject;
    }

    // disables an object and returns it to the available pool of objects
    protected virtual void DeactivateObject(O poolObject)
    {
        if (availableObjects.Contains(poolObject))
        {
            // it's already disabled
            return;
        }

        poolObject.Deactivate();
        poolObject.Reset();

        poolObject.transform.parent = transform;

        availableObjects.Add(poolObject);

    }
}
