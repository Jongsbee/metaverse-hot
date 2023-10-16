using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEnums
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public enum EnhanceItemEnums
    {
        PlayerAttackUp = 1, 
        PlayerMoveSpeedUp = 2, 
        PlayerRangeUp = 3,
        TowerAttackUp = 101, 
        TowerAttackSpeedUp = 102,
        TowerRangeUp = 103
    }

    // 버프 아이템들
    public enum BuffItemEnums
    {
        BuffPlayerAttack = 1, 
        BuffPlayerMoveSpeed = 2, 
        BuffPlayerRange = 3,
        BuffTowerAttack = 101, 
        BuffTowerAttackSpeed = 102, 
        BuffTowerRange = 103,
        BuffMonsterMoveSpeed = 201, 
        BuffMonsterArmor = 202, 
        BuffMonsterHpRegen = 203
    }

    public enum AccountItemEnums
    {
        AccountPlayerAttack = 1, 
        AccountPlayerMoveSpeed = 2, 
        AccountPlayerRange = 3,



        AccountTowerAttack = 101, 
        AccountTowerAttackSpeed = 102, 
        AccountTowerRange = 103,

        BuffMonsterMoveSpeed = 201,
        BuffMonsterArmor = 202,
        BuffMonsterHpRegen = 203,

        AccountExtraGold = 301, 
        AccountTowerCnt = 302,
        AccountShopDiscount = 303
    }
}
