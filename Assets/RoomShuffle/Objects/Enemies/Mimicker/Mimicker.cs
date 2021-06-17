using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEditor.Timeline.Actions;

using UnityEngine;

[RequireComponent(typeof(SpriteAnimator))]
public class Mimicker : MonoBehaviour
{
	private const float KEYFRAME_INTERVAL_SECONDS = 0.05f;

	/* *** */

	private Flippable _playerFlippable;
	private SpriteAnimator _playerSpriteAnimator;
	private SpriteAnimation _lastKnownPlayerAnimation;

	/* *** */

	public Queue<PositionSnapshot> KeyFrames = new Queue<PositionSnapshot>();
	public float Delay = 1f;
	public bool ActivatesAdrenaline;

	private void OnEnable()
	{
		StartCoroutine(PerformMimic());
	}

	private IEnumerator PerformMimic()
	{
		var animator = GetComponent<SpriteAnimator>();
		var flippable = GetComponent<Flippable>();

		if (ActivatesAdrenaline)
			Commons.SoundtrackPlayer.AddAdrenalineTrigger(this);

		/*
		 * Get player info and begin pulse
		 */

		var player = this.GetPlayer();

		if (!player)
			throw new InvalidOperationException("Cannot create mimicker when there is no player");

		_playerFlippable = player.GetComponent<Flippable>();
		_playerSpriteAnimator = player.GetComponent<SpriteAnimator>();

		InvokeRepeating(nameof(CreateKeyFrame), time: 0f, repeatRate: KEYFRAME_INTERVAL_SECONDS);

		/*
		 * Loop
		 */
		while (true)
		{
			var keyframe = FindPointOnTimeline(DateTime.Now.AddSeconds(-Delay));


			if (keyframe == null)
			{
				yield return new WaitForFixedUpdate();
				continue;
			}

			var (position, snapshot) = keyframe.Value;

			transform.position = position;
			flippable.Direction = snapshot.Direction;
			animator.Animation = snapshot.Animation;

			yield return new WaitForFixedUpdate();
		}
	}

	/// <summary>
	/// Gets the position and snapshot at the provided real-time
	/// </summary>
	/// <param name="time"></param>
	/// <returns></returns>
	private (Vector2 position, PositionSnapshot snapshot)? FindPointOnTimeline(DateTime time)
	{
		if (!KeyFrames.Any())
			return null;

		PositionSnapshot last = default;
		foreach (var i in KeyFrames)
		{
			if (i.Time > time)
			{
				if (last.Time == default)
					return null;

				float t;

				t = Mathf.InverseLerp(
					a: (float)(last.Time - DateTime.Today).TotalSeconds, 
					b: (float)(i.Time - DateTime.Today).TotalSeconds, 
					value: (float)(time - DateTime.Today).TotalSeconds
				);

				return (Vector2.Lerp(last.Position, i.Position, t), last);
			}

			last = i;
		}

		return (last.Position, last);
	}

	private void Update()
	{
		//Create keyframe if the player's sprite changes
		if (_playerSpriteAnimator && _playerSpriteAnimator.Animation != _lastKnownPlayerAnimation)
		{
			CreateKeyFrame();
			_lastKnownPlayerAnimation = _playerSpriteAnimator.Animation;
		}
			

	}

	/// <summary>
	/// Creates a new keyframe of the player
	/// </summary>
	private void CreateKeyFrame()
	{
		if (_playerFlippable && _playerSpriteAnimator)
			KeyFrames.Enqueue(PositionSnapshot.FromObjects(_playerFlippable, _playerSpriteAnimator));

		if (KeyFrames.Count > 500)
			KeyFrames.Dequeue();
	}

	private void OnDestroy()
	{
		if (Commons.SoundtrackPlayer)
			Commons.SoundtrackPlayer.RemoveTrigger(this);
	}
}