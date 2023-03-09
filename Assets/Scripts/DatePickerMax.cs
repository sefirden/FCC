using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using UnityEngine.Assertions.Must;

public class DatePickerMax : MonoBehaviour
{
    public TMP_Text dateText;
    public GameObject calendarPanel;
    public Button[] dateButtons;
    public TMP_Text[] dayOfWeek;
    public Button prevMonthButton;
    public Button nextMonthButton;
    public TMP_InputField yearInputField;
    public TMP_Text dateMonth;

    public DateTime currentDate;

    private UI ui; //������ ��


    private void Awake()
    {
        ui = FindObjectOfType<UI>();

        // ���������� ����������� ������� ��� ������ �������� ���������
        Button openCalendarButton = GetComponent<Button>();
        openCalendarButton.onClick.AddListener(OpenCalendar);

        // ���������� ������������ ������� ��� ������ ������ ���� � ���������
        foreach (Button dateButton in dateButtons)
        {
            dateButton.onClick.AddListener(() => SelectDate(dateButton.GetComponent<SelectedDay>().currentDate));
        }

        // ���������� ������������ ������� ��� ������ ������������ ������
        prevMonthButton.onClick.AddListener(PrevMonth);
        nextMonthButton.onClick.AddListener(NextMonth);

        // ���������� ����������� ������� ��� ���� ����� ����
        yearInputField.onEndEdit.AddListener(UpdateYear);
    }

    public void OpenCalendar()
    {
        // ����������� ��������� � ���������� ��� �������
        if (calendarPanel.activeSelf)
        {
            calendarPanel.SetActive(false);
        }
        else
        {
            calendarPanel.SetActive(true);
            currentDate = ui.maxDate;
            FillWeekDays();
            FillCalendar();
        }
    }

    private void FillCalendar()
    {
        // ��������� ������� ����� �������� ������
        DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

        // ��������� ������� ��� ������ �������� ������
        DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        DateTime firstVisibleDate = firstDayOfMonth.AddDays(-(int)(firstDayOfMonth.DayOfWeek - firstDayOfWeek + 7) % 7);


        // ����������� ���� ��� �������� ������ � ���������

        for (int i = 0; i < dateButtons.Length; i++)
        {
            DateTime date = firstVisibleDate.AddDays(i);

            // ������� ���, ������� �� ��������� � �������� ������
            if (date.Month != currentDate.Month)
            {
                dateButtons[i].GetComponentInChildren<TMP_Text>().text = "";
                dateButtons[i].interactable = false;
                dateButtons[i].GetComponent<Image>().color = Color.clear;
                continue;
            }

            string dateString = date.ToString("dd", CultureInfo.CurrentCulture);
            dateButtons[i].GetComponentInChildren<TMP_Text>().text = dateString;
            dateButtons[i].GetComponent<SelectedDay>().currentDate = date;
            dateButtons[i].interactable = true;

            // ��������� ������, ���� ��� ������������� ������� ����
            if (date.Date == currentDate.Date)
            {
                dateButtons[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                dateButtons[i].GetComponent<Image>().color = Color.clear;
            }

        }

        // ��������� �������� ���� � ���� ����� ����
        yearInputField.text = currentDate.Year.ToString();

        // ����������� �������� ������
        dateMonth.text = currentDate.ToString("MMMM", CultureInfo.CurrentCulture);
    }

    private void FillWeekDays()
    {
        DateTimeFormatInfo dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;

        for (int i = 0; i < dayOfWeek.Length; i++)
        {
            DayOfWeek day = (DayOfWeek)(((int)dateTimeFormat.FirstDayOfWeek + i) % 7);
            string dayAbbreviation = dateTimeFormat.GetAbbreviatedDayName(day);
            dayOfWeek[i].text = dayAbbreviation;

            if (day == DayOfWeek.Sunday)
            {
                dayOfWeek[i].color = new Color(0.9568627f, 0.2627451f, 0.2117647f);
            }
        }
    }

    private void PrevMonth()
    {
        // ������������ �� ���������� �����
        currentDate = currentDate.AddMonths(-1);
        ui.maxDate = currentDate;
        dateText.text = currentDate.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
        FillCalendar();
    }

    private void NextMonth()
    {
        // ������������ �� ��������� �����
        currentDate = currentDate.AddMonths(1);
        ui.maxDate = currentDate;
        dateText.text = currentDate.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
        FillCalendar();
    }

    private void SelectDate(DateTime selectedDate)
    {
        // �������� ��������� � ���������� ������������ ����
        calendarPanel.SetActive(false);
        currentDate = selectedDate;
        ui.maxDate = selectedDate;
        dateText.text = selectedDate.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
    }

    private void UpdateYear(string yearString)
    {
        int year = currentDate.Year;
        if (int.TryParse(yearString, out year))
        {
            // ���������� ������� ���� � ����� �����
            try
            {
                currentDate = new DateTime(year, currentDate.Month, currentDate.Day);
                ui.maxDate = currentDate;
                dateText.text = currentDate.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
            }
            catch
            {
                currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);
                ui.maxDate = currentDate;
                dateText.text = currentDate.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
            }
            FillCalendar();
        }
        else
        {
            // ����� ��������� �� ������ ��� ������������ ����� ����
            //StartCoroutine(ui.ToastShow("valid_data")); 
        }
    }

}
