using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using UnityEngine.Assertions.Must;

public class DatePickerMin : MonoBehaviour
{
    public TMP_Text dateText;
    public GameObject calendarPanel;
    public Button[] dateButtons;
    public Button prevMonthButton;
    public Button nextMonthButton;
    public TMP_InputField yearInputField;
    public TMP_Text dateMonth;

    public DateTime currentDate;

    private UI ui; //скрипт уи


    private void Awake()
    {
        ui = FindObjectOfType<UI>();
        // Инициализация текущей даты и отображение ее в UI
        //currentDate = ui.minDate;
        //dateText.text = currentDate.ToShortDateString();

        // Назначение обработчика события для кнопки открытия календаря
        Button openCalendarButton = GetComponent<Button>();
        openCalendarButton.onClick.AddListener(OpenCalendar);

        // Назначение обработчиков событий для кнопок выбора даты в календаре
        foreach (Button dateButton in dateButtons)
        {
            dateButton.onClick.AddListener(() => SelectDate(dateButton.GetComponent<SelectedDay>().currentDate));
        }

        // Назначение обработчиков событий для кнопок переключения месяца
        prevMonthButton.onClick.AddListener(PrevMonth);
        nextMonthButton.onClick.AddListener(NextMonth);

        // Назначение обработчика события для поля ввода года
        yearInputField.onEndEdit.AddListener(UpdateYear);
    }

    public void OpenCalendar()
    {
        // Отображение календаря и заполнение его данными
        if (calendarPanel.activeSelf)
        {
            calendarPanel.SetActive(false);
        }
        else
        {
            calendarPanel.SetActive(true);
            currentDate = ui.minDate;
            FillCalendar();
        }
    }

    private void FillCalendar()
    {
        // Получение первого числа текущего месяца
        DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

        // Получение первого дня недели текущего месяца
        DayOfWeek firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        DateTime firstVisibleDate = firstDayOfMonth.AddDays(-(int)(firstDayOfMonth.DayOfWeek - firstDayOfWeek + 7) % 7);


        // Отображение всех дат текущего месяца в календаре

        for (int i = 0; i < dateButtons.Length; i++)
        {
            DateTime date = firstVisibleDate.AddDays(i);

            // Пропуск дат, которые не относятся к текущему месяцу
            if (date.Month != currentDate.Month)
            {
                dateButtons[i].GetComponentInChildren<TMP_Text>().text = "";
                dateButtons[i].interactable = false;
                continue;
            }

            string dateString = date.ToString("dd", CultureInfo.CurrentCulture);
            dateButtons[i].GetComponentInChildren<TMP_Text>().text = dateString;
            dateButtons[i].GetComponent<SelectedDay>().currentDate = date;
            dateButtons[i].interactable = true;

            // Выделение кнопки, если она соответствует текущей дате
            if (date.Date == currentDate.Date)
            {
                dateButtons[i].GetComponent<Image>().enabled = true; 
            }
            else
            {
                dateButtons[i].GetComponent<Image>().enabled = false;
            }

        }

        // Установка значения года в поле ввода года
        yearInputField.text = currentDate.Year.ToString();
    }

    private void PrevMonth()
    {
        // Переключение на предыдущий месяц
        currentDate = currentDate.AddMonths(-1);
        ui.minDate = currentDate;
        dateText.text = currentDate.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
        FillCalendar();
    }

    private void NextMonth()
    {
        // Переключение на следующий месяц
        currentDate = currentDate.AddMonths(1);
        ui.minDate = currentDate;
        dateText.text = currentDate.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
        FillCalendar();
    }

    private void SelectDate(DateTime selectedDate)
    {
        // Закрытие календаря и обновление отображаемой даты
        calendarPanel.SetActive(false);
        currentDate = selectedDate;
        ui.minDate = selectedDate;
        dateText.text = selectedDate.ToString("dd MMM yyyy", CultureInfo.CurrentCulture);
    }

    private void UpdateYear(string yearString)
    {
        int year = currentDate.Year;
        if (int.TryParse(yearString, out year))
        {
            // Обновление текущей даты с новым годом
            currentDate = new DateTime(year, currentDate.Month, currentDate.Day);
            FillCalendar();
        }
        else
        {
            // Вывод сообщения об ошибке при некорректном вводе года
            Debug.LogWarning("Invalid year input");
        }
    }

}
