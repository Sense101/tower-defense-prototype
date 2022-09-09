using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretPreview : Singleton<TurretPreview>
{

    // set in inspector
    [SerializeField] GameObject previewQuad;
    [SerializeField] Transform previewCamera;

    // internal variables
    //Turret _currentTurret = null;
    Coroutine _currentCoroutine = null;

    // references
    CanvasFadeGroup fadeGroup;

    private void Start()
    {
        fadeGroup = GetComponent<CanvasFadeGroup>();
    }

    private void Update()
    {
        // turn the top to match the angle of the real turret top
        //if (_currentTurret && fadeGroup.state != CanvasFadeGroup.FadeState.hiding)
        //{
        //    topPreview.transform.localRotation = _currentTurret.top.transform.localRotation;
        //@TODO only point if mouse is not within box - aka within x+y of us

        // find the angle from the preview to the mouse
        //Vector2 mousePos = InputController.mousePos;
        //
        //bool withinX = Mathf.Abs(mousePos.x - transform.position.x) <= 1;
        //bool withinY = Mathf.Abs(mousePos.y - transform.position.y) <= 1;
        //if (withinX && withinY)
        //{
        //    return;
        //}
        //
        //Angle angleToMouse = Angle.Towards(transform.position, mousePos);
        //
        //// turn the turret preview toards the mouse
        //topPreview.transform.rotation = angleToMouse.AsQuaternion();
        //Quaternion.RotateTowards
        //(
        //    topPreview.transform.rotation, // from
        //    angleToMouse.AsQuaternion(), // to
        //    _currentTurret.info.SpinSpeedModifier / 2 * Time.deltaTime // delta speed
        //);
        //}
    }



    public void SetPreview(Turret turret)
    {
        // stop the old coroutine
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        // only rerender if it's a valid turret
        if (turret)
        {
            _currentCoroutine = StartCoroutine(RerenderPreview(turret));
        }
    }

    private IEnumerator RerenderPreview(Turret newTurret)
    {
        // first fade in the preview cover
        fadeGroup.Show();

        // wait till we are completely obscuring the camera
        yield return new WaitUntil(() => fadeGroup.Shown);

        // now move the camera ot the new position
        previewCamera.position = newTurret.transform.position;


        fadeGroup.Hide();
        yield return new WaitUntil(() => fadeGroup.Hidden);

        // coroutine is done
        _currentCoroutine = null;
    }
}