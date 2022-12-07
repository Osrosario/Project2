using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Script from the Parent Object")]
    public Stats unitStats;

    private int enemyPointModifier;

    public void AttackUnit(Tile enemytile)
    {
        Stats enemyStats = enemytile.UnitOnTile.GetComponent<Stats>();
        string unitFace = GetComponentInParent<Stats>().GetFace();

        pointModifier(enemyStats, unitFace);
        Battle(enemyStats);
    }

    private void Battle(Stats enemyStats)
    {
        int enemyATK = enemyStats.GetComponentInParent<Stats>().GetATK();
        int enemyTerrainBuff = enemyStats.GetComponentInParent<Stats>().TerrainBuff();
        
        int unitATK = GetComponentInParent<Stats>().GetATK();
        int unitTerrainModifier = unitStats.TerrainBuff();

        Debug.Log($"Enemy ATK: {enemyATK}" +
                  $"\nEnemy Point Modifer: {enemyPointModifier}" +
                  $"\nEnemy Terrain Buff: {enemyTerrainBuff}" +
                  $"\nUnit ATK: {unitATK}"+
                  $"\nUnit Terrain Buff: {unitTerrainModifier}");


        int damage = (unitATK + unitTerrainModifier) - (enemyATK + enemyTerrainBuff + enemyPointModifier);

        Debug.Log($"Damage: {damage}");

        if (damage < 0)
        {
            unitStats.TakeDamage(Mathf.Abs(damage));
            Debug.Log($"This Unit attacked.\nIt received {Mathf.Abs(damage)}");
        }
        else
        {
            enemyStats.TakeDamage(damage);
            Debug.Log($"This Unit attacked.\nIt dealt {damage}");
        }
    }

    private int pointModifier(Stats enemyStats, string unitFace)
    {
        string enemyFace = enemyStats.GetFace();

        if (unitFace == enemyFace)
        {
            enemyPointModifier = enemyStats.pointModfiers["Back"];
        }
        else if ((unitFace == "North" && enemyFace == "South") || 
                 (unitFace == "South" && enemyFace == "North") ||
                 (unitFace == "West" && enemyFace == "East")   || 
                 (unitFace == "East" && enemyFace == "West"))
        {
            enemyPointModifier = enemyStats.pointModfiers["Front"];
        }
        else if ((unitFace == "North" && enemyFace == "West") ||
                 (unitFace == "South" && enemyFace == "East") ||
                 (unitFace == "West" && enemyFace == "South") ||
                 (unitFace == "East" && enemyFace == "North"))
        {
            enemyPointModifier = enemyStats.pointModfiers["Left"];
        }
        else if ((unitFace == "North" && enemyFace == "East") ||
                 (unitFace == "South" && enemyFace == "West") ||
                 (unitFace == "West" && enemyFace == "North") ||
                 (unitFace == "East" && enemyFace == "South"))
        {
            enemyPointModifier = enemyStats.pointModfiers["Right"];
        }

        return enemyPointModifier;
    }
}
