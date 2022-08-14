using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playercontroller : MonoBehaviour
{
    CharacterController controller;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float health;
    [SerializeField]
    public Text healthTxt;

    private float currentHealth;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = health;
    }


    void Update()
    {
        healthTxt.text = "Health :- "+currentHealth.ToString();
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        controller.Move(new Vector3(horizontal,0f, vertical) * -speed * Time.deltaTime );
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
