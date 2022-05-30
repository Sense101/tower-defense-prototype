using UnityEngine;

public class Bullet : MonoBehaviour
{
    // internal variables

    // should be called by the gun on creation
    private void Initialize()
    {

    }

    private void Update()
    {

    }

    public void AnimatorDestroy()
    {
        // called by the animator to destroy this bullet
        // @TODO should not really destroy, but reuse
        Destroy(gameObject);
    }
}
