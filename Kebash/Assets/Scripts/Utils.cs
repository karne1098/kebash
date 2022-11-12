using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Utils
{
  public static Vector3 V2ToV3(Vector2 v, float yValue = 0)
  {
    return new Vector3(v.x, yValue, v.y);
  }

  public static Vector2 V3ToV2(Vector3 v)
  {
    return new Vector2(v.x, v.z);
  }

  public static int PlayerLayer      = LayerMask.NameToLayer("Player");
  public static int ChargerLayer     = LayerMask.NameToLayer("Charger");
  public static int DamagerLayer     = LayerMask.NameToLayer("ChargeDamagerCollider");
  public static int FoodLayer        = LayerMask.NameToLayer("Food");
  public static int FallTriggerLayer = LayerMask.NameToLayer("FallTrigger");
  public static int PickerLayer      = LayerMask.NameToLayer("FoodPickerCollider");
}
