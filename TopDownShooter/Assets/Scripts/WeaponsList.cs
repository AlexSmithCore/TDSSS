using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsList : MonoBehaviour {

	public WeaponList wl;

}

[System.Serializable]
public class WeaponList{
	public MeleeList[] ml;

	public SWeaponList[] swl;

	public MWeaponList[] mwl;

}

[System.Serializable]
public class MeleeList{
	public string wName;
	public Sprite wSprite;
}

[System.Serializable]
public class SWeaponList{
	public string wName;
	public Sprite wSprite;
}

[System.Serializable]
public class MWeaponList{
	public string wName;
	public Sprite wSprite;
}
