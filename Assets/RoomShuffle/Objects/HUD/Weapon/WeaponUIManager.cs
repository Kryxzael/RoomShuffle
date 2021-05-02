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
        
        //Set selected slot to zero
        Commons.Inventory.SelectedWeaponSlot = 0;
    }
    
    void Update()
    {

        Inventory inventory = Commons.Inventory;

        UpdateWeaponImage(inventory);
        UpdateCoolDown(inventory);
        UpsizeSelectedWeapon(inventory);
        //We have disabled Auto equip weapon
        //AutoEquipWeapon(inventory);
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
            var weapon = inventory.WeaponSlots[i];

            if (weapon == null)
            {
                _durabilityTextMeshPro[i].text = "";
            }
            else
            {
                _durabilityTextMeshPro[i].text = weapon.Durability.ToString(System.Globalization.CultureInfo.InvariantCulture);

                if (weapon.Durability <= 0)
                    _durabilityTextMeshPro[i].color = Color.red;

                else if (weapon.Durability <= weapon.MaxDurability / 4f)
                    _durabilityTextMeshPro[i].color = new Color(1f, 0.25f, 0f);

                else if (weapon.Durability <= weapon.MaxDurability / 2f)
                    _durabilityTextMeshPro[i].color = new Color(1f, 1f, 0f);

                else if (weapon.Durability <= weapon.MaxDurability / 1.5f)
                    _durabilityTextMeshPro[i].color = new Color(1f, 1f, 0.5f);


                else
                    _durabilityTextMeshPro[i].color = Color.white;
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
        //the minimum cooldwon timer for the weapon for the iconslot to show a filling up animation
        const float MINIMUM_COOLDOWN_TIME = 0.5f;
        
        for (int i = 0; i < Inventory.MAX_WEAPON_SLOTS; i++)
        {
            if (inventory.WeaponSlots[i] == null)
                continue;

            if (inventory.WeaponSlots[i].Durability <= 0)
            {
                _imageCopy[i].fillAmount = 1f;
                continue;
            }

            if (inventory.WeaponSlots[i].Template.Cooldown < MINIMUM_COOLDOWN_TIME)
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
}