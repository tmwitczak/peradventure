using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SmokeType
{
    Up,
    Down,
    Right,
    Left
}

public class SmokeBehaviour : MonoBehaviour
{
    [SerializeField] GameObject SmokeLeft;
    [SerializeField] GameObject SmokeRight;
    [SerializeField] GameObject SmokeUp;
    [SerializeField] GameObject SmokeDown;

    private Vector3 lSmokeStartPos;
    private Vector3 rSmokeStartPos;
    private Vector3 uSmokeStartPos;
    private Vector3 dSmokeStartPos;

    private Vector3 lSmokeEndPos;
    private Vector3 rSmokeEndPos;
    private Vector3 uSmokeEndPos;
    private Vector3 dSmokeEndPos;

    private float smokeTimer = 0.0f;
    private float smokeCooldown = 2.0f;
    private float moveAmount = 10.0f;
    private bool[] moved;
    private bool[] startMovement;
    private float[] startTime;

    // Start is called before the first frame update
    void Start()
    {
        lSmokeStartPos = SmokeLeft.transform.position;
        rSmokeStartPos = SmokeRight.transform.position;
        uSmokeStartPos = SmokeUp.transform.position;
        dSmokeStartPos = SmokeDown.transform.position;

        lSmokeEndPos = SmokeLeft.transform.position + new Vector3(moveAmount, 0.0f, 0.0f);
        rSmokeEndPos = SmokeRight.transform.position + new Vector3(-moveAmount, 0.0f, 0.0f);
        uSmokeEndPos = SmokeUp.transform.position + new Vector3(0.0f, -moveAmount, 0.0f);
        dSmokeEndPos = SmokeDown.transform.position + new Vector3(0.0f, moveAmount, 0.0f);

        moved = new bool[4];
        startMovement = new bool[4];
        startTime = new float[4];

        for (int i = 0; i < 4; i++)
        {
            startTime[i] = 0.0f;
            moved[i] = false;
            startMovement[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        smokeTimer += Time.deltaTime;
        if (smokeTimer >= smokeCooldown)
        {
            MoveSmoke((int)Random.Range(0, 4));
            smokeTimer = 0.0f;
            smokeCooldown = Random.Range(5, 10);
        }

        if(!moved[0] && startMovement[0])
        {
            float distanceCovered = (Time.time - startTime[0]);
            float distanceSmoothing = distanceCovered / moveAmount;
            SmokeLeft.transform.position = Vector3.Lerp(lSmokeStartPos, lSmokeEndPos, distanceSmoothing);
            if(SmokeLeft.transform.position.x == lSmokeEndPos.x)
            {
                startMovement[0] = false;
                moved[0] = true;
            }
        }

        if(!moved[1] && startMovement[1])
        {
            float distanceCovered = (Time.time - startTime[1]);
            float distanceSmoothing = distanceCovered / moveAmount;
            SmokeRight.transform.position = Vector3.Lerp(rSmokeStartPos, rSmokeEndPos, distanceSmoothing);
            if (SmokeRight.transform.position.x == rSmokeEndPos.x)
            {
                startMovement[1] = false;
                moved[1] = true;
            }
        }

        if(!moved[2] && startMovement[2])
        {
            float distanceCovered = (Time.time - startTime[2]);
            float distanceSmoothing = distanceCovered / moveAmount;
            SmokeUp.transform.position = Vector3.Lerp(uSmokeStartPos, uSmokeEndPos, distanceSmoothing);
            if (SmokeUp.transform.position.y == uSmokeEndPos.y)
            {
                startMovement[2] = false;
                moved[2] = true;
            }
        }
        
        if(!moved[3] && startMovement[3])
        {
            float distanceCovered = (Time.time - startTime[3]);
            float distanceSmoothing = distanceCovered / moveAmount;
            SmokeDown.transform.position = Vector3.Lerp(dSmokeStartPos, dSmokeEndPos, distanceSmoothing);
            if (SmokeDown.transform.position.y == dSmokeEndPos.y)
            {
                startMovement[3] = false;
                moved[3] = true;
            }
        }
    }

    void MoveSmoke(int i)
    {
        switch(i)
        {
            case 0:
                if(!moved[i])
                {
                    startTime[i] = Time.time;
                    startMovement[i] = true;
                }
                break;
            case 1:
                if (!moved[i])
                {
                    startTime[i] = Time.time;
                    startMovement[i] = true;
                }
                break;
            case 2:
                if (!moved[i])
                {
                    startTime[i] = Time.time;
                    startMovement[i] = true;
                }
                break;
            case 3:
                if (!moved[i])
                {
                    startTime[i] = Time.time;
                    startMovement[i] = true;
                }
                break;
        }
    }

    private void ResetSmoke(SmokeType type)
    {
        switch(type)
        {
            case SmokeType.Up:
                SmokeUp.transform.position = uSmokeStartPos;
                break;
            case SmokeType.Down:
                SmokeDown.transform.position = dSmokeStartPos;
                break;
            case SmokeType.Right:
                SmokeRight.transform.position = rSmokeStartPos;
                break;
            case SmokeType.Left:
                SmokeLeft.transform.position = lSmokeStartPos;
                break;
        }
    }
}
