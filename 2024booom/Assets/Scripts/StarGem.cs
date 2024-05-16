using System.Collections.Generic;
using UnityEngine;

//����
public class StarGem : MonoBehaviour
{
    [SerializeField] AudioClip pickupSFX;
    [SerializeField] ParticleSystem pickupVFX;

    new Collider2D collider;
    new MeshRenderer renderer;
    AudioSource audioSource;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
        renderer = GetComponentInChildren<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //�����봥�������ǲ������
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.jumpCount = player.JumpCount + 1;
            player.starGems.Add(this);

            audioSource.PlayOneShot(pickupSFX);
            Instantiate(pickupVFX, transform.position, Quaternion.identity);

            collider.enabled = false;
            renderer.enabled = false;

            //��ʱ�ָ�
            //Invoke(nameof(Reset), 3f);
        }
    }

    public void Reset()
    {
        collider.enabled = true;
        renderer.enabled = true;
    }
}