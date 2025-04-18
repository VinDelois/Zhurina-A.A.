using System;
using System.Collections.Generic;
using System.Linq;
namespace AttendanceApp
{
    public class AttendanceData
    {
        private List<int> attendanceList;
        public AttendanceData()
        {
            attendanceList = new List<int>(31); // Создаем список на 31 день
            for (int i = 0; i < 31; i++)
            {
                attendanceList.Add(0); // Инициализируем нулями
            }
        }
        public void SetAttendance(int day, int attendance)
        {
            if (day < 1 || day > 31) throw new ArgumentOutOfRangeException(nameof(day), "День должен быть от 1 до 31.");
            attendanceList[day - 1] = attendance;
        }
        public int GetAttendance(int day)
        {
            if (day < 1 || day > 31) throw new ArgumentOutOfRangeException(nameof(day), "День должен быть от 1 до 31.");
            return attendanceList[day - 1];
        }
        public (int minDay, int maxDay) GetMinMaxDays()
        {
            int minAttendance = attendanceList.Min();
            int maxAttendance = attendanceList.Max();
            int minDay = attendanceList.IndexOf(minAttendance) + 1; // +1 для соответствия дню месяца
            int maxDay = attendanceList.IndexOf(maxAttendance) + 1;
            return (minDay, maxDay);
        }
        public List<int> GetAttendanceList()
        {
            return attendanceList;
        }
    }
}
