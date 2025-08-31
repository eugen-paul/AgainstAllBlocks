
using System.Collections.Generic;
using System.Collections.Immutable;

public class UpgradeInfo
{
    public UpgradeType Type { private set; get; }

    public int MaxLevel { private set; get; }

    public string Description { private set; get; }

    public IList<string> LevelDescription { private set; get; }

    public IList<int> Cost { private set; get; }

    public IList<string> Textures { private set; get; }

    public UpgradeInfo(UpgradeType Type, int MaxLevel, string Description, IList<string> LevelDescription, IList<int> Cost, IList<string> Textures)
    {
        this.Type = Type;
        this.MaxLevel = MaxLevel;
        this.Description = Description;
        this.Cost = Cost;
        this.LevelDescription = LevelDescription.ToImmutableList();
        this.Textures = Textures;
    }
}

public class UpgradeItemInfo
{
    private UpgradeItemInfo() { }

    public static readonly IDictionary<UpgradeType, UpgradeInfo> UpgradeItemInfos = new Dictionary<UpgradeType, UpgradeInfo>
    {
        {UpgradeType.EMPTY, new(UpgradeType.EMPTY,
                                0,
                                "UPGRADE_EMPTY",
                                new List<string>(){"UPGRADE_EMPTY_0",}.ToImmutableList(),
                                new List<int>(){0,}.ToImmutableList(),
                                new List<string>(){"res://assets/textures/gui/upgrades/EMPTY-0.png",}.ToImmutableList())},
        {UpgradeType.EXTRA_LIFE, new(UpgradeType.EXTRA_LIFE,
                                3,
                                "UPGRADE_EXTRA_LIFE",
                                new List<string>(){"UPGRADE_EXTRA_LIFE_0",
                                                   "UPGRADE_EXTRA_LIFE_1",
                                                   "UPGRADE_EXTRA_LIFE_2",
                                                   "UPGRADE_EXTRA_LIFE_3",}.ToImmutableList(),
                                new List<int>(){0, 1, 2, 3,}.ToImmutableList(),
                                new List<string>(){ "res://assets/textures/gui/upgrades/EXTRA_LIFE-0.png",
                                                    "res://assets/textures/gui/upgrades/EXTRA_LIFE-1.png",
                                                    "res://assets/textures/gui/upgrades/EXTRA_LIFE-2.png",
                                                    "res://assets/textures/gui/upgrades/EXTRA_LIFE-3.png",}.ToImmutableList())},
        {UpgradeType.PADDLE_SPEED, new(UpgradeType.PADDLE_SPEED,
                                3,
                                "UPGRADE_PADDLE_SPEED",
                                new List<string>(){"UPGRADE_PADDLE_SPEED_0",
                                                   "UPGRADE_PADDLE_SPEED_1",
                                                   "UPGRADE_PADDLE_SPEED_2",
                                                   "UPGRADE_PADDLE_SPEED_3",}.ToImmutableList(),
                                new List<int>(){0, 1, 2, 3,}.ToImmutableList(),
                                new List<string>(){"res://assets/textures/gui/upgrades/PADDLE_SPEED-0.png",
                                                   "res://assets/textures/gui/upgrades/PADDLE_SPEED-1.png",
                                                   "res://assets/textures/gui/upgrades/PADDLE_SPEED-2.png",
                                                   "res://assets/textures/gui/upgrades/PADDLE_SPEED-3.png",}.ToImmutableList())},
        {UpgradeType.BOMB_POWER, new(UpgradeType.BOMB_POWER,
                                3,
                                "UPGRADE_BOMB_POWER",
                                new List<string>(){"UPGRADE_BOMB_POWER_0",
                                                   "UPGRADE_BOMB_POWER_1",
                                                   "UPGRADE_BOMB_POWER_2",
                                                   "UPGRADE_BOMB_POWER_3",}.ToImmutableList(),
                                new List<int>(){0, 1, 2, 3,}.ToImmutableList(),
                                new List<string>(){"res://assets/textures/gui/upgrades/BOMB_POWER-0.png",
                                                   "res://assets/textures/gui/upgrades/BOMB_POWER-1.png",
                                                   "res://assets/textures/gui/upgrades/BOMB_POWER-2.png",
                                                   "res://assets/textures/gui/upgrades/BOMB_POWER-3.png",}.ToImmutableList())},
        {UpgradeType.BOMB_SCORE, new(UpgradeType.BOMB_SCORE,
                                3,
                                "UPGRADE_BOMB_SCORE",
                                new List<string>(){"UPGRADE_BOMB_SCORE_0",
                                                   "UPGRADE_BOMB_SCORE_1",
                                                   "UPGRADE_BOMB_SCORE_2",
                                                   "UPGRADE_BOMB_SCORE_3",}.ToImmutableList(),
                                new List<int>(){0, 1, 2, 3,}.ToImmutableList(),
                                new List<string>(){"res://assets/textures/gui/upgrades/BOMB_SCORE-0.png",
                                                   "res://assets/textures/gui/upgrades/BOMB_SCORE-1.png",
                                                   "res://assets/textures/gui/upgrades/BOMB_SCORE-2.png",
                                                   "res://assets/textures/gui/upgrades/BOMB_SCORE-3.png",}.ToImmutableList())},
    }.ToImmutableDictionary();

    public static readonly IList<int> SlotsCost = new List<int>() { 5, 20, 40, 70, }.ToImmutableList();
}