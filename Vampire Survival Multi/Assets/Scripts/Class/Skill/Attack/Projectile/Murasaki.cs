using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(menuName = "Skill/Class/Active/Test", fileName = "Murasaki")]
public class Murasaki : Skill
{
    // 스킬 세부 사항
    [SerializeField] private float speed;
    [SerializeField] private float damageValue;
    [SerializeField] private float damageRate;

    [Header("발사체")]
    [SerializeField] private GameObject projectilePrefab;

    public override void UseSkill(Player caster)
    {
        string prefabName = SKILL_OBJECT_DIRECTORY + projectilePrefab.name;
        GameObject projectileObj = PhotonNetwork.Instantiate(prefabName, CasterData.Position, Quaternion.identity);

        // 투사체 생성 위치 설정
        projectileObj.transform.position = CasterData.Position;

        // 이동 방향 설정
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 casterPos = CasterData.Position;
        Vector2 targetPos = casterPos + (mousePos - casterPos).normalized * Distance;

        float damage = damageValue + CasterData.STR * damageRate;

        // 투사체 발사
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        projectile.ThrowProjectile(targetPos, speed, true, (monster) =>
        {
            caster.OnSkillAttack(monster, damage);
        });
    }
}