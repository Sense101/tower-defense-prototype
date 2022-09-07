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

    private UpgradeSlot[] _upgradeSlots;

    // references
    TurretPlacer _turretPlacer;
    TurretController _turretController;
    UpgradeController _upgradeController;
    Map _map;

    GameObject _previewCamera;

    private void Start()
    {
        // set references
        _turretPlacer = TurretPlacer.Instance;
        _turretController = TurretController.Instance;
        _upgradeController = UpgradeController.Instance;
        _map = Map.Instance;
        _previewCamera = GameObject.FindWithTag("PreviewCamera");

        _fadeGroup = GetComponent<CanvasFadeGroup>();

        ConnectUpgradeSlots();
    }

    private void ConnectUpgradeSlots()
    {
        _upgradeSlots = GetComponentsInChildren<UpgradeSlot>();
        for (int i = 0; i < _upgradeSlots.Length; i++)
        {
            _upgradeSlots[i].SetSlotIndex(i);
        }
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
        // @todo this all needs remaking
        //if (!_selectedTurret)
        //{
        //    // no turret selected, do nothing
        //    return;
        //}
        //
        //UpgradeInfo upgrade;
        //int tier;
        //if (index == 0)
        //{
        //    upgrade = _upgradeController.damageUpgrade;
        //    _selectedTurret.damageAugmentLevel++;
        //    tier = _selectedTurret.damageAugmentLevel;
        //    damageText.text = $"damage upgrade ({tier}/5)";
        //}
        //else
        //{
        //    upgrade = _upgradeController.rangeUpgrade;
        //    //tier = _selectedTurret.rangeAugmentLevel++;
        //    //rangeText.text = $"range upgrade ({tier}/5)";
        //}
        //
        //_upgradeController.ApplyUpgrade(_selectedTurret.stats, upgrade.id, tier);
        ////@TODO
        //_turretPlacer.UpdateRangeScale(_selectedTurret);
    }//
    public void MutateTurret()
    {
        MutationInterface.Instance.Open();
    }

    public void ChooseUpgrade(int index)
    {
        UpgradeInterface.Instance.currentSlotIndex = index;
        UpgradeInterface.Instance.Open();
    }

    public void SetTurret(Turret turret)
    {
        if (_selectedTurret == turret)
        {
            // it's the same
            return;
        }

        if (!_turretPlacer)
        {
            Debug.LogError("SetTurret cannot be called till after start!");
            return;
        }

        bool newTurret = _selectedTurret;
        if (newTurret)
        {
            // we are changing, remove the event link from the old turret
            _selectedTurret.onBulletHitEvent.RemoveListener(OnTurretHit);
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

            UpdateInterface();

            // move things to the right position
            _previewCamera.transform.position = turret.transform.position;
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

    public void UpdateInterface()
    {
        if (!_selectedTurret)
        {
            Debug.LogError("Can't update the turret interface without a turret selected!");
            return;
        }

        UpdateXpBar(_selectedTurret.stats.xp, true);
        turretName.text = _selectedTurret.info.title;
        targetTypeSelector.SelectByIndex((int)_selectedTurret.targetType);

        // update the upgrade slots
        for (int i = 0; i < _upgradeSlots.Length; i++)
        {
            UpgradeSlot slot = _upgradeSlots[i];
            UpgradeInfo upgrade = _selectedTurret.upgrades[i];

            slot.button.interactable = !upgrade;
            if (upgrade)
            {
                slot.backgroundImage.color = upgrade.color;
                //slot.icon = @todo
                slot.titleText.text = upgrade.title;
            }
            else
            {
                // reset to default
                slot.backgroundImage.color = Color.white; // temp
                slot.titleText.text = "Empty Upgrade Slot";
            }
        }

        // update the range preview
        _turretPlacer.UpdateRangeScale(_selectedTurret);
    }

    // called when the current turret hits an enemy
    private void OnTurretHit()
    {
        UpdateXpBar(_selectedTurret.stats.xp);
    }

    private void UpdateXpBar(int xp, bool instant = false)
    {
        if (true || xp >= 200)
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
