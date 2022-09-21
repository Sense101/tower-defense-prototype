using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// this is the script that will handle all the turret interface parts
public class TurretInterface : Singleton<TurretInterface>
{
    // set in inspector - the different parts of the interface

    [SerializeField] CanvasFadeGroup fadeGroup;

    [Header("XP Bar")]
    [SerializeField] Image xpBarFillImage;
    [SerializeField] UIButton xpBarButton;
    [SerializeField] TextMeshProUGUI xpBarText;

    [Header("Turret Info")]
    [SerializeField] TextMeshProUGUI turretTitle;
    [SerializeField] TextMeshProUGUI tierDescription;
    [SerializeField] TextMeshProUGUI sellAmountText;

    // @todo
    [SerializeField] UIToggleSelector targetTypeSelector;


    // internal variables
    private Turret _selectedTurret = null;

    private UpgradeSlot[] _upgradeSlots;

    // references
    TurretPlacer _turretPlacer;
    ConfigController _configs;
    Map _map;
    GameObject _previewCamera;

    private void Start()
    {
        // set references
        _turretPlacer = TurretPlacer.Instance;
        _configs = ConfigController.Instance;
        _map = Map.Instance;
        _previewCamera = GameObject.FindWithTag("PreviewCamera");

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
        TurretController.Instance.SellTurret(_selectedTurret);

        // set the tile to not be holding a turret
        _map.SetTurretWorldSpace(_selectedTurret.transform.position, null);

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
            fadeGroup.Show();

            // add a hit event listener so we can update as the new turret changes
            turret.onBulletHitEvent.AddListener(OnTurretHit);

            UpdateInterface();

            // move things to the right position
            _previewCamera.transform.position = turret.transform.position;
            _turretPlacer.MoveToCurrentTile();
        }
        else
        {
            fadeGroup.Hide();
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

        // set turret title

        // check if we have more than two of an upgrade @todo
        // then add in that name in the middle, and add on turret at the end
        turretTitle.text = _selectedTurret.info.title;


        tierDescription.text = _selectedTurret.info.tierDescription;

        sellAmountText.text = _selectedTurret.stats.sellAmount.ToString();

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
        if (_configs.debug.instantMutations || xp >= 200)
        {
            xpBarButton.interactable = true;

            if (xpBarButton.hovering)
            {
                xpBarButton.OnHoverStart();
            }

            xpBarText.text = "Click to mutate!";
            //xpBarText.text = xp.ToString() + " / 200";
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
