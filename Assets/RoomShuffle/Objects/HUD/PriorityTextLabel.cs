using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using TMPro;

using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Controls the on-screen warning label
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class PriorityTextLabel : MonoBehaviour
{
	private IEnumerator _current;
	private TextMeshProUGUI _text;

	private bool _lastKnownHealthNeed = true;
	private bool _lastKnownAmmoNeed = true;
	private int _lastKnownEnemyLevel;

	private void Awake()
	{
		_text = GetComponent<TextMeshProUGUI>();
	}

	private void Update()
	{
		/*
		 * Health
		 */
		bool hpLow = Commons.PlayerHealth.Health <= Commons.PlayerHealth.GetSoftDeathDamage();

		if (!_lastKnownHealthNeed && hpLow)
			Set($"WARNING{Environment.NewLine}Low Health", 3f);

		_lastKnownHealthNeed = hpLow;

		/*
		 * Damage
		 */
		int enemyLevel = Commons.EnemyProgression.DamageLevel;

		if (enemyLevel > _lastKnownEnemyLevel && enemyLevel > Commons.PlayerProgression.DamageLevel)
			Set($"CAUTION{Environment.NewLine}Underleveled", 3f);

		_lastKnownEnemyLevel = enemyLevel;

		/*
		 * Ammo
		 */
		bool needsAmmo = RequiredItemLootTable.InNeedOfWeapons();

		if (!_lastKnownAmmoNeed && needsAmmo)
			Set($"WARNING{Environment.NewLine}Low Ammo", 3f);

		_lastKnownAmmoNeed = needsAmmo;
	}

	public void SetIndefinite(string text, bool noFlash = true)
	{
		Set(text, -1f, noFlash);
	}

	public void Clear()
	{
		if (_current?.MoveNext() == true)
			StopCoroutine(_current);

		_text.text = "";
	}

	public void Set(string text, float duration, bool noFlash = false)
	{
		if (_current?.MoveNext() == true)
			StopCoroutine(_current);

		_current = CoStart(text, duration, noFlash);
		StartCoroutine(_current);
	}

	private IEnumerator CoStart(string text, float duration, bool noFlash)
	{
		const float FLASH_RATE = 0.1f;

		for (float time = 0f; time <= duration || duration == -1; time += 2 * FLASH_RATE)
		{
			_text.text = text;
			yield return new WaitForSecondsRealtime(FLASH_RATE);
			_text.text = noFlash ? text : "";
			yield return new WaitForSecondsRealtime(FLASH_RATE);
		}
	}
}
