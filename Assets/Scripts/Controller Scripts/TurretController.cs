using System.Collections.Generic;

public class TurretController : Singleton<TurretController>
{
    public readonly List<Turret> _turrets = new List<Turret>();

    // Update is called once per frame
    private void Update()
    {
        for (int i = 0; i < _turrets.Count; i++)
        {
            var turret = _turrets[i];
            if (!turret)
            {
                _turrets.RemoveAt(i);
                continue;
            }

            if (turret._firingState == TurretInfo.FiringState.none)
            {
                turret.ChooseNewTarget();
            }

            if (turret._firingState != TurretInfo.FiringState.none)
            {
                turret.Turn();
            }
        }
    }

    private void TurnTurret(Turret turret)
    {

    }
}
