using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public bool isLeftHandSlot;
    public bool isRightHandSlot;
    
    public GameObject currentWeaponModel;

	public bool LoadWeaponModel(WeaponItem weaponItem)
	{
		UnloadWeaponAndDestroy();

		if (weaponItem == null)
		{
			UnloadWeapon();
			return false;
		}

		GameObject model = Instantiate(weaponItem.model) as GameObject;
		if (model != null)
		{
			if (parentOverride != null)
			{
				model.transform.parent = parentOverride;
			}
			else
			{
				model.transform.parent = transform;
			}
			model.transform.localPosition = Vector3.zero;
			model.transform.localRotation = Quaternion.identity;
			model.transform.localScale = Vector3.one;
		}

		currentWeaponModel = model;
		return true;
	}

	public void UnloadWeapon()
	{
		if (currentWeaponModel != null)
		{
			currentWeaponModel.SetActive(false);
		}
	}

	public void UnloadWeaponAndDestroy()
	{
		if (currentWeaponModel != null)
		{
			Destroy(currentWeaponModel);
		}
	}
}
