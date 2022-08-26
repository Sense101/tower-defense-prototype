using System;
using System.Collections;
using System.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// this is the script that will handle all the turret interface parts
public class TurretInterface : Singleton<TurretInterface>
{
    // set in inspector - the different parts of the interface

    [Header("XP Bar")]
    [SerializeField] Image xpBarFillImage;
    [SerializeField] UIButton xpBarButton;
    [SerializeField] TextMeshProUGUI xpBarText;

    [Header("@TODO")]
    [SerializeField] TextMeshProUGUI turretName;
    [SerializeField] UIToggleSelector targetTypeSelector;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI rangeText;


    // internal variables
    private Turret _selectedTurret = null;
    private CanvasFadeGroup _fadeGroup;

    // references
    TurretPlacer _turretPlacer;
    TurretController _turretController;
    UpgradeController _upgradeController;
    AugmentInterface _augmentInterface;
    Map _map;

    Camera _previewCamera;

    private void Start()
    {
        // set references
        _turretPlacer = TurretPlacer.Instance;
        _turretController = TurretController.Instance;
        _upgradeController = UpgradeController.Instance;
        _augmentInterface = AugmentInterface.Instance;
        _map = Map.Instance;
        _previewCamera = GameObject.FindWithTag("PreviewCamera").GetComponent<Camera>();

        _fadeGroup = GetComponent<CanvasFadeGroup>();
    }

    // bunch of scripts called by the buttons within the interface

    public void SellTurret()
    {
        if (!_selectedTurret)
        {
            // we don't have a turret selected
            return;
        }

        Debug.Log("selling turret");
        // sell the turret
        _turretController.SellTurret(_selectedTurret);

        // set the tile to not be holding a turret
        _map.SetTurretWorldSpace(Vector2Int.RoundToInt(_selectedTurret.transform.position), null);

        // hide interface
        SetTurret(null);
    }

    public void SetTurretTargetType(Turret.TargetType targetType)
    {
        if (!_selectedTurret)
        {
            // we don't have a turret selected
            return;
        }

        _selectedTurret.targetType = targetType;
    }

    // called by buttons
    public void ApplyUpgrade(int index)
    {
        if (!_selectedTurret)
        {
            // no turret selected, do nothing
            return;
        }

        UpgradeInfo upgrade;
        int tier;
        if (index == 0)
        {
            upgrade = _upgradeController.damageUpgrade;
            _selectedTurret.damageAugmentLevel++;
            tier = _selectedTurret.damageAugmentLevel;
            damageText.text = $"damage upgrade ({tier}/5)";
        }
        else
        {
            upgrade = _upgradeController.rangeUpgrade;
            tier = _selectedTurret.rangeAugmentLevel++;
            rangeText.text = $"range upgrade ({tier}/5)";
        }

        _upgradeController.ApplyUpgrade(_selectedTurret.stats, upgrade.id, tier);
        //@TODO
        _turretPlacer.UpdateRangeScale(_selectedTurret);
    }
    public void ChooseAugment()
    {
        _augmentInterface.Show();
    }

    // IMPORTANT: this cannot be called till after start
    // selects the turret
    public void SetTurret(Turret turret)
    {
        if (_selectedTurret == turret)
        {
            // it's the same
            return;
        }

        bool wasSelecting = false;
        if (_selectedTurret)
        {
            // we are changing, remove the event link from the old turret
            _selectedTurret.onBulletHitEvent.RemoveListener(OnTurretHit);
            wasSelecting = true;
        }

        // actually update the reference
        _selectedTurret = turret;

        // set the range preview visibility
        GameObject rangePreviewObject = _turretPlacer.rangePreview.gameObject;
        rangePreviewObject.SetActive(turret);

        if (turret)
        {
            // we've selected a turret, show the main interface
            _fadeGroup.Show();

            // add a hit event listener so we can update as the new turret changes
            turret.onBulletHitEvent.AddListener(OnTurretHit);

            // update stuff within the interface - @TODO
            _previewCamera.transform.position = turret.transform.position;

            turretName.text = turret.gameObject.name;
            targetTypeSelector.SelectByIndex((int)turret.targetType);
            UpdateXpBar(turret.stats.xp, !wasSelecting);

            // update the range preview
            _turretPlacer.UpdateRangeScale(turret);
            _turretPlacer.MoveToCurrentTile();
        }
        else
        {
            _fadeGroup.Hide();
        }
    }

    public Turret GetTurret()
    {
        return _selectedTurret;
    }

    // called when the current turret hits an enemy
    private void OnTurretHit()
    {
        UpdateXpBar(_selectedTurret.stats.xp);
    }

    private void UpdateXpBar(int xp, bool instant = false)
    {
        if (xp >= 200)
        {
            xpBarButton.interactable = true;

            if (xpBarButton.hovering)
            {
                xpBarButton.OnHoverStart();
            }

            xpBarText.text = "Mutation Ready!";
        }
        else
        {
            xpBarButton.interactable = false;
            xpBarText.text = xp.ToString() + " / 200";
        }

        if (instant)
        {
            xpBarFillImage.fillAmount = (float)xp / 200f;
        }
        else
        {
            DOTween.To(() => xpBarFillImage.fillAmount, x => xpBarFillImage.fillAmount = x, (float)xp / 200f, 0.3f);
        }
    }
}
