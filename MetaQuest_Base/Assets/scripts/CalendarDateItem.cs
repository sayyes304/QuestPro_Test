using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CalendarDateItem : MonoBehaviour {
    //public Text DateText;

    public GameObject DailyText;
    public GameObject DailyLine;

    public Transform DailyPlan_Parent;
    
    public void OnDateItemClick()
    {
        // CalendarController._calendarInstance.OnDateItemClick(DateText.text);
        // DateText.gameObject.SetActive(true);
        // gameObject.GetComponent<Button>(). = ;
        CalendarController._calendarInstance.OnDateItemClick(gameObject.GetComponentInChildren<Text>().text);

        print("name : " + gameObject.GetComponent<RectTransform>().anchoredPosition + " name ? : " + gameObject.name + " day : " + CalendarController._calendarInstance.Daily_day);

        // 일정 text 생성
        GameObject DailyPlan = Instantiate(DailyText, DailyPlan_Parent);
        DailyPlan.transform.name = CalendarController._calendarInstance.Daily_day;

        
        GameObject Daily = Instantiate(DailyLine, DailyPlan.transform);

        Daily.transform.position = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);

    }
}
