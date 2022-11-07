using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3.0f;
    //ID for Powerups
    // 0 = Triple Shot, 1 = Speed, 2 = Shields, 3 = Ammo, 4 = Health
    [SerializeField]
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime); 

        if(transform.position.y < -7.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
           
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if(player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AmmoPowerup();
                        break;
                    case 4:
                        player.HealthPowerup();
                        break;
               
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
           
            Destroy(this.gameObject);
        }
    }   
}
