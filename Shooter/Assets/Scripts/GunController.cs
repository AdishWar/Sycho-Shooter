using UnityEngine;

public class GunController : MonoBehaviour {

    public Transform weaponHold;
    public Gun startingGun;
    Gun equippedGun;

	// Use this for initialization
	void Start () {
		if (startingGun != null) {
            equipGun(startingGun);
        }
	}
	
	public void equipGun(Gun gunToEquip) {
        if (equippedGun != null) {
            Destroy(equippedGun.gameObject);
        }

        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }

    public void shoot() {
    	if(equippedGun != null) {
    		equippedGun.shoot();
    	}
    }
}