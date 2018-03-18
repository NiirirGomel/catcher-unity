using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour {

    [SerializeField] CandyController gameCandy;
    
    public void TakeDamage(int damage) {
        gameCandy.SetCandy(damage > gameCandy.candyCount ? 0 : gameCandy.candyCount - damage);
    }
}
