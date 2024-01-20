using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] Text damageText;

    public void SetValue(float amount)
    {
        damageText.text = string.Format("{0:0}",amount);
        //damageText.text = $"{amount}";
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
