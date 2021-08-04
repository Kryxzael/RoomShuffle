using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.RoomShuffle.Objects.HUD
{
    /// <summary>
    /// Controls the hiding of the HUD
    /// </summary>
    public class DemoHUDManager : MonoBehaviour
    {
        public GameObject AttackLevelUI;
        public GameObject HealthUI;
        public GameObject WeaponUI;
        public GameObject ItemUI;
        public GameObject WeaponStatUI;
        public GameObject RoomNumberUI;
        public GameObject CurrencyUI;
        public GameObject GeneralKeysUI;
        public GameObject PuzzleKeysUI;
        public GameObject EscapeToExitUI;
        public GameObject RoomClassUI;
        public GameObject RoomEffectUI;

        /// <summary>
        /// Hides and shows UI elements in correspondence with the provided UIHideFlags
        /// </summary>
        /// <param name="flags"></param>
        public void SetUIFlags(DemoParameterBuilder.DemoRoomConfig.UIHideFlags flags)
        {
            AttackLevelUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.AttackLevel));
            HealthUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.Health));
            WeaponUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.Weapons));
            ItemUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.Item));
            WeaponStatUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.WeaponStats));
            RoomNumberUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.RoomNumber));
            CurrencyUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.Currency));
            GeneralKeysUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.GeneralKeys));
            PuzzleKeysUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.PuzzleKeys));
            EscapeToExitUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.EscapeToExit));
            RoomClassUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.RoomClass));
            RoomEffectUI.SetActive(!flags.HasFlag(DemoParameterBuilder.DemoRoomConfig.UIHideFlags.RoomEffect));
        }
    }
}
