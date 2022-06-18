using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected BoundsCheck bndCheck;
    protected Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;

    [SerializeField]
    protected WeaponType _type;

    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Изменяет скрытое поле _type и устанавливает цвет снаряда
    /// как определено в WeaponDefinition
    /// </summary>
    /// <param name="eType"></param>
    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
