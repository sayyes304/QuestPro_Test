using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalendarDateItem : MonoBehaviour {
    //public Text DateText;
    
    public void OnDateItemClick()
    {
        CalendarController._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);
        print("name : " + gameObject.GetComponent<RectTransform>().anchoredPosition + " name ? : " + gameObject.name );



        CalendarController._calendarInstance.DailyLine.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 10, transform.localPosition.z); ; ;

    }

}
