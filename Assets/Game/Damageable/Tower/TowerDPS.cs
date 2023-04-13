using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

[CreateAssetMenu(menuName = "TowerDPS")]
public class TowerDPS : ScriptableObject
{
    public List<IDPSObject> dpsObjects = new();
    public List<IDamageObject> damageObjects = new();

    public void Register(IDPSObject obj)
    {
        if (dpsObjects.Contains(obj)) return;
        dpsObjects.Add(obj);
    }
    public void Register(IDamageObject obj)
    {
        if (damageObjects.Contains(obj)) return;
        damageObjects.Add(obj);
    }
    public void Unregister(IDPSObject obj)
    {
        if (!dpsObjects.Contains(obj)) return;
        dpsObjects.Remove(obj);
    }
    public void Unregister(IDamageObject obj)
    {
        if (!damageObjects.Contains(obj)) return;
        damageObjects.Remove(obj);
    }

    public int CalculateDamage(int seconds)
    {
        int damage = 0;
        for (int i = 0; i < dpsObjects.Count; i++)
        {
            Debug.Log(dpsObjects[i]);
            Debug.Log(dpsObjects[i].GetDPS());
            damage += Mathf.CeilToInt(dpsObjects[i].GetDPS() * seconds);
        }
        for (int i = 0; i < damageObjects.Count; i++)
        {
            Debug.Log(damageObjects[i]);
            damage += damageObjects[i].GetDamage();
        }
        return damage;
    }

    public void Reset()
    {
        damageObjects.Clear();
        dpsObjects.Clear();
    }
}
