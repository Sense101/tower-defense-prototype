using UnityEngine;
using UnityEngine.InputSystem;

public class TurretPlacer : Singleton<TurretPlacer>
{
    [SerializeField] GameObject BasicTurretPrefab = default;

    Map _map;
    TurretController _turretController;
    EnemyController _enemyController;
    private void Start()
    {
        _map = Map.Instance;
        _turretController = TurretController.Instance;
        _enemyController = EnemyController.Instance;

        InputManager.leftButtonEvent += OnLeftButton;
        InputManager.rightButtonEvent += OnRightButton;
    }

    private void OnLeftButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var mousePos = Vector2Int.RoundToInt(InputManager.mousePos);
            var tileInfo = _map.TryGetTileWorldSpace(mousePos);

            // check to see if tile exists
            if (tileInfo == null)
            {
                return;
            }

            if (tileInfo.Placeable && !tileInfo.Turret)
            {
                Debug.Log("can place");

                // creat the turret
                var newTurret = Instantiate(
                        BasicTurretPrefab,
                        (Vector3Int)mousePos,
                        Quaternion.identity
                    ).GetComponent<Turret>();

                // initialize
                newTurret.Initialize(_enemyController);
                // add to the controller
                _turretController._turrets.Add(newTurret);
                tileInfo.Turret = newTurret;
            }
            else
            {
                Debug.Log("cannot place");
            }
        }
    }
    private void OnRightButton(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // @TODO simplify to one function
            var mousePos = Vector2Int.RoundToInt(InputManager.mousePos);
            var tileInfo = _map.TryGetTileWorldSpace(mousePos);

            // check to see if tile exists
            if (tileInfo == null) return;

            if (tileInfo.Turret)
            {
                Debug.Log("can destroy");
                Destroy(tileInfo.Turret.gameObject);
                tileInfo.Turret = null;
            }
            else
            {
                Debug.Log("cannot destroy");
            }
        }
    }
}
