using Photon.Pun;

public class BossMonster : Monster
{
    protected override int GetMonsterExp()
    {
        // 보스 몹 경험치 지급
        int level = GameData.Instance.Level;
        int requireExp = LevelResource.Instance.GetRequireExp(level);

        return (int)(requireExp * 1.1f);
    }
}