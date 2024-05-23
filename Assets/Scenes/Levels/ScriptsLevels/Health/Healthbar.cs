using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Healthclass playerhealth;
    [SerializeField] private Image totalHealth;
    [SerializeField] private Image currenthealth;

    void Start()
    {
        totalHealth.fillAmount = playerhealth.CurrentHealth / 10f;
        Debug.Log("StartTotal" + totalHealth.fillAmount.ToString());
        Debug.Log("StartCurrent" + currenthealth.fillAmount.ToString());
        Debug.Log("Start PlayerHealth "+ playerhealth.CurrentHealth.ToString());
    }

    void Update()
    {
        currenthealth.fillAmount = playerhealth.CurrentHealth / 10f;


    }
}