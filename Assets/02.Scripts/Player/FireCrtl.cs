using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireCrtl : MonoBehaviour {


	public GameObject bullet;

	public Transform firePos;
	// Use this for initialization

	public ParticleSystem  cartidge;

	public PlaySfx playSfx;

	//이미 firepos 정보를 갖고 있으므로 firepos 의 child 인 muzzle flush 프리펍정보를 가져온다.
	// 따라서 private 으로 설정해도 된다.
	private ParticleSystem  muzzleFlash;

	private AudioSource fireCtrAudio;


	#region == 탄창관련 ====

	public Image magazineImage;

	public Text magazineText;

	public int maxBulllet =10;

	public int remainingBullet =10;

	//재장전시간
	public float reloadTime = 2.0f;

	//재장전중인지 여부 판단
	private bool isReloading = false;


	#endregion

	public enum WeaponType{
		RIFLE =0,
		SHOTGUN
	}

	public WeaponType currentWeapon = WeaponType.RIFLE;

	private Shake shake;

	//변경할 무기 이미지
	public Sprite[] weaponIcons;

	//변경할 무기 이미지 UI
	public Image weaponImage;

	private void Start() {
		muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
		fireCtrAudio = GetComponent<AudioSource>();

		shake = GameObject.Find("CameraRig").GetComponent<Shake>();
	}
	// Update is called once per frame
	void Update () {

		Debug.DrawRay(firePos.position,firePos.forward * 20.0f,Color.red);

		if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return; //UI 버튼 클릭인 경우 총을 쏘지 않는다.
		
		if (!isReloading && Input.GetMouseButtonDown(0))
		{
			Fire();

			if (remainingBullet ==0) //총알이 다 떨지면 재장전
			{
				StartCoroutine(Reloading());
			}
		}
	}

    private IEnumerator Reloading()
    {
        isReloading = true;

		fireCtrAudio.PlayOneShot(playSfx.reload[(int)currentWeapon],1.0f);

		yield return new WaitForSeconds(playSfx.reload[(int)currentWeapon].length +0.3f); //재장전 오디오 길이에 0.3초 더한 만큼 대기


		//탄창 초기화
		isReloading = false;
		magazineImage.fillAmount = 1.0f;
		remainingBullet = maxBulllet;

		UpdateBulletText();
		
    }

    private void Fire()
    {
		--remainingBullet; //총알 하나 제거

		StartCoroutine(shake.ShakeCamera()); //사격 반동 표시

        //Instantiate(bullet, firePos.position, firePos.rotation);
		//풀에서 가져와 사용하는 것으로 바꾼다.
		var bullet = GameManager.instance.GetBullet();
		if (bullet !=null)
		{
			bullet.transform.position = firePos.position;
			bullet.transform.rotation = firePos.rotation;

			bullet.SetActive(true);
		}

		muzzleFlash.Play();
		cartidge.Play(); //안다 오타인거..

		FireSfx();
		
		//탄창정보 업뎃
		magazineImage.fillAmount = (float) remainingBullet / (float)maxBulllet;

		UpdateBulletText();

    }

    private void UpdateBulletText()
    {
        magazineText.text = string.Format("<color=#ff0000>{0}</color>/{1}", remainingBullet.ToString(),maxBulllet.ToString());
    }

    private void FireSfx()
    {
        var sfx = playSfx.fire[(int)currentWeapon];
		fireCtrAudio.PlayOneShot(sfx,1.0f);
    }

	public void OnChangeWeapon()
	{
		currentWeapon =(WeaponType)((int) ++currentWeapon % 2);
		weaponImage.sprite = weaponIcons[(int)currentWeapon];
	}
}

[System.Serializable]
public struct PlaySfx
{
	public AudioClip [] fire;
	public AudioClip [] reload;
}
