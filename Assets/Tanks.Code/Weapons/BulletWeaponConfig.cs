﻿namespace Tanks.Weapons {
    using UnityEngine;

    [CreateAssetMenu(fileName = "BulletWeaponConfig", menuName = "Tanks/BulletWeaponConfig", order = 0)]
    public class BulletWeaponConfig : ScriptableObject {
        public BulletConfig bulletConfig;
        public float bulletSpeed;
        public float reloadTime;
    }
}