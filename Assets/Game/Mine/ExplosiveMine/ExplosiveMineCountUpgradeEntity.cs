using UnityEngine;
using FateGames.Core;
[CreateAssetMenu(menuName = "Upgrades/Preparation/Base/Mine/Explosive/Explosive Mine Count Upgrade")]
public class ExplosiveMineCountUpgradeEntity : PreparationUpgradeEntity
{
    [SerializeField] private SoundEntity sound;
    [SerializeField] private SoundManager soundManager;
    public override int Cost => Level * 10;
    public override int Level { get => saveData.Value.ExplosiveMineCount; protected set => saveData.Value.ExplosiveMineCount = value; }
    public override void Initialize()
    {
        ExplosiveMine[] mines = FindObjectsOfType<ExplosiveMine>(true);
        int count = Mathf.Clamp(Level, 0, mines.Length);
        for (int i = 0; i < count; i++)
        {
            mines[i].Activate();
        }
    }
    public override void Upgrade()
    {
        base.Upgrade();
        Initialize();
        soundManager.PlaySound(sound);
    }
}
public partial class SaveData
{
    public int ExplosiveMineCount = 0;
}