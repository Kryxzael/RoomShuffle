using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;
using Debug = UnityEngine.Debug;

public class WeaponUIManager : MonoBehaviour

{
    [Tooltip("The UI images that contains the weapon icon")]
    public GameObject[] WeaponImage = new GameObject[Inventory.MAX_WEAPON_SLOTS];
    
    [Tooltip("The copy if the UI images that contains the weapon icon")]
    public GameObject[] WeaponImageCopy = new GameObject[Inventory.MAX_WEAPON_SLOTS];
    
    [Tooltip("The object containing the weapon image and weapon image copy")]
    public GameObject[] WeaponImageGroup = new GameObject[Inventory.MAX_WEAPON_SLOTS];
    
    [Tooltip("The TextMeshPro objects describing durability per weapon")]
    public GameObject[] DurabilityText = new GameObject[Inventory.MAX_WEAPON_SLOTS];
    
    /*
     * Private variables used for faster caching 
     */
    public Sprite blankSprite;
    private WeaponInstance[] _weapon = new WeaponInstance[Inventory.MAX_WEAPON_SLOTS];
    private Image[] _image = new Image[Inventory.MAX_WEAPON_SLOTS];
    private Image[] _imageCopy = new Image[Inventory.MAX_WEAPON_SLOTS];
    private int[] _lastDurability = new int[Inventory.MAX_WEAPON_SLOTS];
    private TextMeshProUGUI[] _durabilityTextMeshPro = new TextMeshProUGUI[Inventory.MAX_WEAPON_SLOTS];

    void Start()
    {
        Commons.Inventory.SelectedWeaponSlot = 1;
        
        //Initialize private arrays
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            _image[i] = WeaponImage[i].GetComponent<Image>();
            _imageCopy[i] = WeaponImageCopy[i].GetComponent<Image>();
            _durabilityTextMeshPro[i] = DurabilityText[i].GetComponent<TextMeshProUGUI>();
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
        
        //Update array keeping track of last durability
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            _lastDurability[i] = inventory.WeaponSlots[i] == null? 0 : inventory.WeaponSlots[i].Durability;
        }

    }

    /// <summary>
    /// Updates the textMeshPro object to show durability for each weapon
    /// </summary>
    /// <param name="inventory"></param>
    private void UpdateDurabilityCounter(Inventory inventory)
    {
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (inventory.WeaponSlots[i] == null)
            {
                _durabilityTextMeshPro[i].text = "";
            }
            else
            {
                _durabilityTextMeshPro[i].text = inventory.WeaponSlots[i].Durability.ToString();
            }
        }
    }

    /// <summary>
    /// Automatically equips a weapon if your selected weapon is null
    /// </summary>
    /// <param name="inventory"></param>
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

    /// <summary>
    /// Makes the selected weapon bigger and skewed 
    /// </summary>
    /// <param name="inventory"></param>
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

    /// <summary>
    /// Updates the cooldown cake diagram to represent the actual cooldown of all weapons
    /// </summary>
    /// <param name="inventory"></param>
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

    /// <summary>
    /// Sets the image in ui the icon/sprite from the weapon
    /// </summary>
    /// <param name="inventory"></param>
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

    /// <summary>
    /// Automatically switches weapon to the next slot if the selcted weapon ammo has just been emptied
    /// </summary>
    /// <param name="inventory"></param>
    private void AutoSwitchWeapon(Inventory inventory)
    {
        if (inventory.SelectedWeapon == null)
            return;

        if (inventory.SelectedWeapon.Durability > 0)
            return;

        bool weaponsWithDurability = false;
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (inventory.WeaponSlots[i]?.Durability > 0)
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