using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarController : MonoBehaviour
{
    public GameObject _calendarPanel;
    public Text _yearNumText;
    public Text _monthNumText;

    public GameObject _item;

    public List<GameObject> _dateItems = new List<GameObject>();
    const int _totalDateNum = 42;

    private DateTime _dateTime;
    public static CalendarController _calendarInstance;

    public Text SelectDate_DateInfo;


    // 
    public string Daily_day;
    public GameObject Date_Title, Title;

    public bool isDateClicked = false;
    public GameObject Write_Plan;

    public string Clickedday;

    void Start()
    {
        Date_Title.SetActive(false);
        Title.SetActive(true);
        Write_Plan.SetActive(false);

        _calendarInstance = this;
        Vector3 startPos = _item.transform.localPosition;
        _dateItems.Clear();
        _dateItems.Add(_item);

        for (int i = 1; i < _totalDateNum; i++)
        {
            GameObject item = GameObject.Instantiate(_item) as GameObject;
            item.name = "Item" + (i + 1).ToString();
            item.transform.SetParent(_item.transform.parent);
            item.transform.localScale = Vector3.one;
            item.transform.localRotation = Quaternion.identity;
            item.transform.localPosition = new Vector3((i % 7) * 36  + startPos.x, startPos.y - (i / 7) * 30, startPos.z);

            _dateItems.Add(item);
        }

        _dateTime = DateTime.Now;

        CreateCalendar();

        ShowCalendar(SelectDate_DateInfo);

    }

    #region CalenderBasic(Create, Prev, Next Button)
    void CreateCalendar()
    {
        DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
        int index = GetDays(firstDay.DayOfWeek);

        int date = 0;
        for (int i = 0; i < _totalDateNum; i++)
        {
            Text label = _dateItems[i].GetComponentInChildren<Text>();
            _dateItems[i].SetActive(false);

            if (i >= index)
            {
                DateTime thatDay = firstDay.AddDays(date);
                if (thatDay.Month == firstDay.Month)
                {
                    _dateItems[i].SetActive(true);

                    label.text = (date + 1).ToString();
                    date++;
                }
            }
        }
        _yearNumText.text = _dateTime.Year.ToString();
        _monthNumText.text = _dateTime.Month.ToString("D2");

        Transform[] childObjects = gameObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in childObjects)
        {
            if (child.gameObject.name == "Daily_text(Clone)")
            {
                child.parent.gameObject.SetActive(child.parent.name.Contains(_dateTime.Year.ToString() + "-" + _dateTime.Month.ToString("D2")));
            }
        }
    }

    int GetDays(DayOfWeek day)
    {
        switch (day)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 0;
        }

        return 0;
    }
    public void YearPrev()
    {
        _dateTime = _dateTime.AddYears(-1);
        CreateCalendar();
    }

    public void YearNext()
    {
        _dateTime = _dateTime.AddYears(1);
        CreateCalendar();
    }

    public void MonthPrev()
    {
        _dateTime = _dateTime.AddMonths(-1);
        CreateCalendar();
    }

    public void MonthNext()
    {
        _dateTime = _dateTime.AddMonths(1);
        CreateCalendar();
    }

    #endregion

    // RegisterPlan_Btn
    public void ShowCalendar(Text target)
    {
        _calendarPanel.SetActive(true);
        _target = target;

        Date_Title.SetActive(false);
        Title.SetActive(true);
        Write_Plan.SetActive(false);
    }

    Text _target;

    public GameObject Daily;
    public GameObject DailyText;
    public GameObject DailyLine;
    public Transform DailyPlan_Parent;
    public InputField InputPlan;
    public void CreateDateObj()
    {
        if(InputPlan.text != "")
        {
            // 일정 object 생성
            GameObject Day_Daily = Instantiate(Daily, DailyPlan_Parent);
            Day_Daily.transform.name = Clickedday;
            // 일정 text 생성
            GameObject DailyPlan = Instantiate(DailyText, Day_Daily.transform);
            DailyPlan.GetComponent<Text>().text = InputPlan.text;

            //일정 생성 완료 표시 
            GameObject Dailyline = Instantiate(DailyLine, Day_Daily.transform);
            
            InputPlan.text = "";
        }

    }

    public void CancelButton()
    {
        _calendarPanel.SetActive(true);

        Date_Title.SetActive(false);
        Title.SetActive(true);
        Write_Plan.SetActive(false);

        InputPlan.text = "";
    }

    public void AddButtonClick()
    {
        isDateClicked = false;

        Date_Title.GetComponent<Text>().text = Clickedday;
        Date_Title.SetActive(true);

        Title.SetActive(false);
        Write_Plan.SetActive(true);
        _calendarPanel.SetActive(false);
    }


    //Item 클릭했을 경우 Text에 표시
    public void OnDateItemClick(string day)
    {
        isDateClicked = true;
        DailyText.SetActive(false);
        Clickedday = _yearNumText.text + "-" + _monthNumText.text + "-" + int.Parse(day).ToString("D2");
        _target.text = Clickedday;

        Transform[] childObjects = gameObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in childObjects)
        {
            if (child.gameObject.name == "Daily_text(Clone)")
            {
                child.gameObject.SetActive(child.parent.name == Clickedday);
            }
        }
    }
}
