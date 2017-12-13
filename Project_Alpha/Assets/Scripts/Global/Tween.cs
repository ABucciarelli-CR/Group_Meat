using UnityEngine;
using System.Collections;

public class Tween : MonoBehaviour
{

	public float time = 5;
	private float start;
	private float end;
	private float startTime;
    public AnimationCurve curve;

    public enum Tweens
    {
        LINEAR,
        EASE_IN,
        CURVE
    }

    public Tweens tweenType;

	// Use this for initialization
	void Start ()
    {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {}

    public void Move(float lastMove)
    {
        float move;
        
        start = transform.position.x;
        end = start + Mathf.Sign(lastMove) * 2;

        Debug.Log("Start = " + start);
        Debug.Log("End = " + end);

        /*
        switch (tweenType)
        {
            case Tweens.LINEAR:
                move = Linear(DeltaTime());
                break;

            case Tweens.EASE_IN:
                move = EaseIn(DeltaTime());
                break;

            case Tweens.CURVE:
                move = Curve(DeltaTime());
                break;

            default:
                move = 0;
                break;
        }*/
        switch (tweenType)
        {
            case Tweens.LINEAR:
                move = Linear(3);
                break;

            case Tweens.EASE_IN:
                move = EaseIn(Time.deltaTime);
                break;

            case Tweens.CURVE:
                move = Curve(Time.deltaTime);
                break;

            default:
                move = 0;
                break;
        }

        transform.position = new Vector3(move, transform.position.y, transform.position.z);

        //Bounce();
    }

    /*
    float DeltaTime()
    {
        float timeDelta = Time.time - startTime;

        if (timeDelta < time)
        {
            return timeDelta / time;
        }
        else
        {
            return 1;
        }
    }

	void Bounce()
    {
        if (DeltaTime() == 1)
        { 
			float temp = end;

			end = start;
			start = temp;
			startTime = Time.time;
		}
	}*/

	float Linear(float delta)
    {
        return Mathf.Lerp(start, end, delta);
	}

    float EaseIn(float delta)
    {
        return Mathf.Lerp(start, end, delta * delta);
    }

    float Curve(float delta)
    {
        return (end - start) * curve.Evaluate(delta) + start;
    }
}
