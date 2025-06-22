using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CongratScript : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public ParticleSystem SparksParticles;
    

    public String text = hello world;
    
    private List<string> TextToDisplay;

    int CongratScript;
    private float RotatingSpeed;
    private float TimeToNextText;

    private int CurrentText;
    
    // Start is called before the first frame update
    void Start()
    {
        CongratScript = 1;
        TimeToNextText = 0.0f;
        CurrentText = 0;
        
        RotatingSpeed = 1.0f;
        TextToDisplay = new List<string>
        {
            "Congratulation",
            "All Errors Fixed"
        };

        Text.text = TextToDisplay[0];
        
        SparksParticles.Play();
    }

    // Update is called once per frame
    void Update()
    {
        TimeToNextText += Time.deltaTime;

        if (TimeToNextText > 1.5f)
        {
            TimeToNextText = 0.0f;
            
            CurrentText++;
            if (CurrentText >= TextToDisplay.Count)
            {
                CurrentText = 0;

            }

            Text.text = TextToDisplay[CurrentText];
        }
    }
}