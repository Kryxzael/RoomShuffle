using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the trail behind the player
/// </summary>
public class TrailController : MonoBehaviour
{
	private SpriteRenderer[] _renderers;
	private Mimicker[] _mimickers;

	private void Awake()
	{
		_renderers = GetComponentsInChildren<SpriteRenderer>();
		_mimickers = GetComponentsInChildren<Mimicker>();
	}

	private void Start()
	{
		DisableTrail();
	}

	/// <summary>
	/// Enables the trail with the provided delay and color
	/// </summary>
	/// <param name="color"></param>
	/// <param name="minDuration"></param>
	/// <param name="maxDuration"></param>
	public void SetTrail(Color color, float minDuration, float maxDuration)
	{
		foreach (SpriteRenderer i in _renderers)
		{
			i.gameObject.SetActive(true);
			i.color = new Color(color.r, color.g, color.b, i.color.a);
		}

		for (int i = 0; i < _mimickers.Length; i++)
			_mimickers[i].Delay = Mathf.Lerp(minDuration, maxDuration, (float)i / _mimickers.Length);
	}

	/// <summary>
	/// Disables the trail
	/// </summary>
	public void DisableTrail()
	{
		foreach (var i in _renderers)
			i.gameObject.SetActive(false);
	}
}
