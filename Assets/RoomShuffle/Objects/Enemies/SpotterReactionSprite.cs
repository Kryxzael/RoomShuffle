using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpotPlayer))]
public class SpotterReactionSprite : MonoBehaviour
{
    public Sprite PuzzeledSprite;

    public Sprite SpottedSprite;
    
    public Sprite ChasingSprite;
    
    public Sprite BlindChasingSprite;
    
    public SpriteRenderer ReactionSpriteRenderer;

    private SpotterPlayerRelationship _lastState;
    private SpotPlayer _spotPlayer;
    void Start()
    {
        _spotPlayer = GetComponent<SpotPlayer>();
        _lastState = _spotPlayer.State;
        ReactionSpriteRenderer.transform.localPosition = Vector3.zero + (Vector3.up * 1.4f);
    }
    
    void Update()
    {
        if (_lastState == _spotPlayer.State)
            return;

        _lastState = _spotPlayer.State;

        ReactionSpriteRenderer.sprite = GetReactionSprite(_spotPlayer.State);
    }
    
    /// <summary>
    /// Return the sprite that suits the spotter's state
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private Sprite GetReactionSprite(SpotterPlayerRelationship state)
    {
        switch (state)
        {
            case SpotterPlayerRelationship.Spotted:
                return SpottedSprite;
            case SpotterPlayerRelationship.Puzzled:
                return PuzzeledSprite;
            case SpotterPlayerRelationship.Chasing:
                return ChasingSprite;
            case SpotterPlayerRelationship.BlindChasing:
                return BlindChasingSprite;
            default:
                return null;
        }
    }
}
