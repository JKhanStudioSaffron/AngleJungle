using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Purpose: Manages level creation, background setup, and player positioning.
/// Responsibilities:
/// - Instantiates the appropriate level prefab based on the `LevelIndex`.
/// - Sets up the background and determines whether the monster should be active.
/// - Positions the player at the correct spawn point for the level.
/// Usage:
/// - Attach this script to an empty GameObject in the scene.
/// - Assign `LevelPrefabs`, `BackgroundSprites`, and `PlayerPositionSetup` in the Inspector.
/// - Ensure `LevelIndex` and `TotalLevels` are set before level creation.
/// </summary>

public class LevelManager : MonoBehaviour
{
    public static int LevelIndex;
    public static int TotalLevels;
    public Transform Player;
    public SpriteRenderer Background, Monster;
    public List<GameObject> LevelPrefabs;
    public List<LevelsBgSetup> BackgroundSprites;
    public List<PlayerPosition> PlayerPositionSetup;


    private void Awake()
    {
        CreateLevel();
    }

    void CreateLevel()
    {
        Instantiate(LevelPrefabs[LevelIndex - 1]);
        SetUpBg();
        ChangePlayerPos();
    }

    void ChangePlayerPos()
    {
        //will prioritise separate indices first before checking the range
        Transform playerPos = PlayerPositionSetup.FirstOrDefault(bg => bg.LevelIndices?.OtherLevelIndices?.Contains(LevelIndex) == true)
        ?.PlayerPositionInLevel;

        if (playerPos == null)
            playerPos = PlayerPositionSetup.Find(index => (index.LevelIndices.startLevelIndex <= LevelIndex && index.LevelIndices.endingLevelIndex >= LevelIndex)).PlayerPositionInLevel;

        Player.position = playerPos.position;
    }

    void SetUpBg()
    {
        if(LevelIndex > 7)
            Monster.gameObject.SetActive(true);
        else
            Monster.gameObject.SetActive(false);

        //will prioritise separate indices first before checking the range
        Sprite bgSprite = BackgroundSprites
        .FirstOrDefault(bg => bg.LevelIndices?.OtherLevelIndices?.Contains(LevelIndex) == true)
        ?.BgSprite;

        if(bgSprite == null)
            bgSprite = BackgroundSprites.Find(index => (index.LevelIndices.startLevelIndex <= LevelIndex && index.LevelIndices.endingLevelIndex >= LevelIndex)).BgSprite;

        Background.sprite = bgSprite;
    }
}
