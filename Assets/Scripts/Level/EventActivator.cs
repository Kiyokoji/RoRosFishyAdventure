using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventActivator : MonoBehaviour
{
    public UnityEvent ActionTriggerEnter;
    public UnityEvent ActionTriggerExit;
    public UnityEvent ActionTriggerStay;

    public float triggerStayTime = 0f;
    private float m_timeStayed = 0f;
    private bool triggered = false;

    public int triggerCounterEnter = 0;
    public int triggerCounterExit = 0;
    public int triggerCounterStay = 0;

    private int m_triggerCountedEnter = 0;
    private int m_triggerCountedExit = 0;
    private int m_triggerCountedStay = 0;

    public void Update()
    {
        if (triggered)
        {
            m_timeStayed += Time.deltaTime;

            if (m_timeStayed > triggerStayTime && triggerCounterStay == 0 || m_triggerCountedStay < triggerCounterStay)
            {
                ActionTriggerStay.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggered = true;
        m_timeStayed = 0f;

        if (triggerCounterEnter == 0 || m_triggerCountedEnter < triggerCounterEnter)
        {
            ActionTriggerEnter.Invoke();
            m_triggerCountedEnter++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggered = false;
        m_timeStayed = 0f;

        if (triggerCounterExit == 0 || m_triggerCountedExit < triggerCounterExit)
        {
            ActionTriggerExit.Invoke();
            m_triggerCountedExit++;
        }

        m_triggerCountedStay++;
    }
}
