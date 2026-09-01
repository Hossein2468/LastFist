using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{
    private PlayerCombat playerCombat;

    // Start is called before the first frame update
    void Start()
    {
        playerCombat = GetComponentInParent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndAttack()
    {
        playerCombat.EndAttack();
    }

    public void OpenComboWindow()
    {
        playerCombat.OpenComboWindow();
    }

    public void CloseComboWindow()
    {
        playerCombat.CloseComboWindow();
    }

    public void DealDamage()
    {
        playerCombat.DealDamage();
    }
}
