using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour {

	// Use this for initialization

	public GameObject expEffect;

	public Mesh[] effectedMeshes;

	public Texture[] textures;

	public AudioClip expSfx;

	private int hitCount  = 0;
	private MeshFilter meshFilter;

	//private Rigidbody rb;

	private AudioSource barrelCrlAudio;


	private float expRadius = 10.0f;

	private Shake shake;

	void Start () {
		//rb = GetComponent<Rigidbody>();
		meshFilter = GetComponent<MeshFilter>();

		barrelCrlAudio = GetComponent<AudioSource>();
		GetComponent<MeshRenderer>().material.mainTexture = textures[UnityEngine.Random.Range(0,textures.Length)];

		shake= GameObject.Find("CameraRig").GetComponent<Shake>();
	}
	
	// Update is called once per frame
	private void OnCollisionEnter(Collision other) {
		
		if ( other.collider.CompareTag("BULLET"))
		{
			if(++hitCount ==3)
			{
				ExpBarrel();
			}
		}
	
	}

    private void ExpBarrel()
    {

       var effect =Instantiate(expEffect,transform.position, Quaternion.identity);
		//rb.mass = 2.0f; //문게를 줄이고
		//rb.AddForce(Vector3.up * 1000.0f);
		Destroy(effect,2.0f);

		IndirectDamage(transform.position);

		int rnd = UnityEngine.Random.Range(0,effectedMeshes.Length);

		meshFilter.sharedMesh = effectedMeshes[rnd];
		//콜라이더 메쉬도 변경해 주자.
		GetComponent<MeshCollider>().sharedMesh = effectedMeshes[rnd];

		barrelCrlAudio.PlayOneShot(expSfx, 1.0f);

		StartCoroutine(shake.ShakeCamera(0.2f,0.4f,0.8f));

    }

    private void IndirectDamage(Vector3 position)
    {
        Collider[] cols = Physics.OverlapSphere(position,expRadius,1<<11);

		foreach(var col in cols)
		{
			var rbody = col.GetComponent<Rigidbody>();
			rbody.mass = 1.0f;
			rbody.AddExplosionForce(1200.0f, position, expRadius, 1000.0f);
		}
    }
}
