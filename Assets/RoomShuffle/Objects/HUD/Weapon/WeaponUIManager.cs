using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Debug = UnityEngine.Debug;

public class WeaponUIManager : MonoBehaviour

{
    public GameObject[] WeaponImage = new GameObject[Inventory.MAX_WEAPON_SLOTS];
    public GameObject[] WeaponImageCopy = new GameObject[Inventory.MAX_WEAPON_SLOTS];
    public GameObject[] WeaponImageGroup = new GameObject[Inventory.MAX_WEAPON_SLOTS];
    public GameObject[] DurabilityText = new GameObject[Inventory.MAX_WEAPON_SLOTS];
    public Sprite blankSprite;

    private WeaponInstance[] _weapon = new WeaponInstance[Inventory.MAX_WEAPON_SLOTS];
    private Image[] _image = new Image[Inventory.MAX_WEAPON_SLOTS];
    private Image[] _imageCopy = new Image[Inventory.MAX_WEAPON_SLOTS];
    private int[] _lastDurability = new int[Inventory.MAX_WEAPON_SLOTS];
    private TextMeshProUGUI[] DurabilityTextMeshPro = new TextMeshProUGUI[Inventory.MAX_WEAPON_SLOTS];

    void Start()
    {
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            _image[i] = WeaponImage[i].GetComponent<Image>();
            _imageCopy[i] = WeaponImageCopy[i].GetComponent<Image>();
            DurabilityTextMeshPro[i] = DurabilityText[i].GetComponent<TextMeshProUGUI>();
        }
        
    }
    
    void Update()
    {
        Inventory inventory = Commons.Inventory;
        
        UpdateWeaponImage(inventory);
        UpdateCoolDown(inventory);
        UpsizeSelectedWeapon(inventory);
        AutoEquipWeapon(inventory);
        AutoSwitchWeapon(inventory);
        UpdateDurabilityCounter(inventory);
        
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            _lastDurability[i] = inventory.WeaponSlots[i] == null? 0 : inventory.WeaponSlots[i].Durability;
        }

    }

    private void UpdateDurabilityCounter(Inventory inventory)
    {
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (inventory.WeaponSlots[i] == null)
            {
                DurabilityTextMeshPro[i].text = "";
            }
            else
            {
                DurabilityTextMeshPro[i].text = inventory.WeaponSlots[i].Durability.ToString();
            }
        }
    }

    private void AutoEquipWeapon(Inventory inventory)
    {
        if (inventory.SelectedWeapon == null)
        {
            for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
            {
                if (inventory.WeaponSlots[i] != null)
                {
                    inventory.SelectedWeaponSlot = i;
                    break;
                }
            }
        }
    }

    private void UpsizeSelectedWeapon(Inventory inventory)
    {
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (i == inventory.SelectedWeaponSlot)
            {
                WeaponImageGroup[i].transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
                WeaponImageGroup[i].transform.rotation = Quaternion.Euler(0,0,15);
                continue;
            }
            WeaponImageGroup[i].transform.localScale = Vector3.one;
            WeaponImageGroup[i].transform.rotation = Quaternion.Euler(0,0,0);
        }
        
    }

    private void UpdateCoolDown(Inventory inventory)
    {
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (inventory.WeaponSlots[i] == null)
                continue;

            if (inventory.WeaponSlots[i].Durability <= 0)
            {
                _imageCopy[i].fillAmount = 1f;
                continue;
            }

            if (inventory.WeaponSlots[i].Template.Cooldown < 1f)
            {
                _imageCopy[i].fillAmount = 0f;
                continue;
            }

            _imageCopy[i].fillAmount =
                inventory.WeaponSlots[i].RemainingCooldown / inventory.WeaponSlots[i].Template.Cooldown;
        }
    }

    private void UpdateWeaponImage(Inventory inventory)
    {
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (inventory.WeaponSlots[i] != _weapon[i])
            {
                _weapon[i] = inventory.WeaponSlots[i];
                if (_weapon[i] == null)
                {
                    _image[i].sprite = blankSprite;
                    _imageCopy[i].sprite = blankSprite;
                }
                else
                {
                    _image[i].sprite = _weapon[i].Template.Icon;
                    _imageCopy[i].sprite = _weapon[i].Template.Icon;
                }
            }
        }
    }

    private void AutoSwitchWeapon(Inventory inventory)
    {
        if (inventory.SelectedWeapon == null)
            return;

        if (inventory.SelectedWeapon.Durability > 0)
            return;

        bool weaponsWithDurability = false;
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (inventory.WeaponSlots[i].Durability > 0)
            {
                weaponsWithDurability = true;
                break;
            }
        }

        if (!weaponsWithDurability)
            return;
        
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (_lastDurability[i] > 0 && inventory.WeaponSlots[i].Durability <= 0)
            {
                while (inventory.WeaponSlots[inventory.SelectedWeaponSlot].Durability <= 0)
                {
                    inventory.SelectedWeaponSlot++;
                    if (inventory.SelectedWeaponSlot >= Inventory.MAX_WEAPON_SLOTS)
                    {
                        inventory.SelectedWeaponSlot -= Inventory.MAX_WEAPON_SLOTS;
                    }
                }
                break;
            }
        }
    }
}