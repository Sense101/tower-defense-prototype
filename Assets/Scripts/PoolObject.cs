using UnityEngine;

// IMPORTANT: starts visible and deactivated
public abstract class PoolObject : MonoBehaviour
{
    // sets the references, only needs to be done once
    // each individual object type can also have a method to set references if needed
    public virtual void SetReferences() { }

    // activates the object, rendering it visible
    public abstract void Activate();

    // deactivates the object, stopping everything it does and rendering it invisible
    public abstract void Deactivate();

    // completely resets the object to its base state
    public abstract void Reset();

    // ----------------
    // Methods that are called upon activation
    // ----------------

    public virtual void SetLocation(Vector2 location)
    {
        transform.position = location;
    }
    public virtual void SetRotation(Angle rotation)
    {
        transform.rotation = rotation.AsQuaternion();
    }
    public virtual void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
}
