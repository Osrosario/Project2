using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Script from the Parent Object")]
    public Stats unitStats;

    [Header("Attack Range")]
    public float RayCastDistance;

    private GameObject pointOfATK;
    private List<string> pointList = new List<string> { "Weak", "Neutral", "Strong" };

    public bool CanAttack()
    {
        RaycastHit hit;

        bool unitInRange = Physics.Raycast(transform.position, transform.forward, out hit, RayCastDistance);

        if (unitInRange)
        {
            if (pointList.Contains(hit.collider.tag))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void AttackUnit()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, RayCastDistance))
        {
            GameObject hitPoint = hit.collider.gameObject;

            if (hitPoint != null)
            {
                pointOfATK = hitPoint;
                Battle();
            }
        }
    }

    private void Battle()
    {
        Stats enemyStats = pointOfATK.GetComponentInParent<Stats>();
        int enemyATK = pointOfATK.GetComponentInParent<Stats>().GetATK();
        int enemyPointModifier = pointOfATK.GetComponent<AttackPoint>().GetModifer();
        int enemyTerrainBuff = pointOfATK.GetComponentInParent<Stats>().TerrainBuff();
        
        int unitATK = GetComponentInParent<Stats>().GetATK();
        int unitTerrainModifier = unitStats.TerrainBuff();

        Debug.Log($"Enemy ATK: {enemyATK}" +
                  $"\nEnemy Point Modifer: {enemyPointModifier}" +
                  $"\nEnemy Terrain Buff: {enemyTerrainBuff}" +
                  $"\nUnit ATK: {unitATK}");


        int damage = (unitATK + unitTerrainModifier) - (enemyATK + enemyTerrainBuff + enemyPointModifier);

        Debug.Log($"Damage: {damage}");

        if (damage < 0)
        {
            unitStats.SetHP(Mathf.Abs(damage));
            Debug.Log($"This Unit attacked.\nIt received {Mathf.Abs(damage)}");
        }
        else
        {
            enemyStats.SetHP(damage);
            Debug.Log($"This Unit attacked.\nIt dealt {damage}");
        }
    }

    private void OnDrawGizmos()
    {
        RaycastHit hit;

        bool isHit = Physics.Raycast(transform.position, transform.forward, out hit, RayCastDistance);

        if (isHit)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            //Debug.Log(hit.collider.tag);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, transform.forward * RayCastDistance);
        }
    }
}
