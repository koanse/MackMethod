using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace MackMethod
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
    public class MackMethod
    {
        float[,] elements;
        Mark[,] marks;
        int n, clmC, clmD, rowCur;
        float difMin;
        List<int> arrClmA, arrClmB;
        public string iterationResult;
        public MackMethod(float[,] elements)
        {
            this.n = elements.GetLength(0);
            this.elements = elements;
            this.marks = new Mark[n, n];
            this.arrClmA = new List<int>();
            this.arrClmB = new List<int>();
            ClearMarks();
            MarkMinElements();
            FormClmArrays();
        }
        public bool DoIteration()
        {
            iterationResult = "Номера столбцов во множестве A: ";
            foreach (int j in arrClmA)
                iterationResult += j.ToString() + " ";
            iterationResult += "\r\nНомера столбцов во множестве A': ";
            foreach (int j in arrClmB)
                iterationResult += j.ToString() + " ";

            iterationResult += "\r\nМатрица\r\ni / j\t";
            for (int j = 0; j < n; j++)
                iterationResult += j.ToString() + "\t";
            iterationResult += "\r\n";
            for (int i = 0; i < n; i++)
            {
                iterationResult += i.ToString() + "\t";
                for (int j = 0; j < n; j++)
                    switch (marks[i, j])
                    {
                        case Mark.Line:
                            iterationResult += "__" + elements[i, j].ToString() + "__\t";
                            break;
                        case Mark.Points:
                            iterationResult += ".." + elements[i, j].ToString() + "..\t";
                            break;
                        default:
                            iterationResult += "  " + elements[i, j].ToString() + "  \t";
                            break;
                    }
                iterationResult += "\r\n";
            }
            
            GetMinDif(out rowCur, out clmC, out clmD, out difMin);
            if (rowCur == -1)
                return false;
            iterationResult += "Минимальная неотрицательная разность между " +
                "находящимися в одной строке подчеркнутыми элементами " +
                "столбцов множества A и элементами столбцов множества A' " +
                "равна " + difMin.ToString() + " и достигается в строке " + rowCur.ToString() + 
                " для столбца номер " + clmD.ToString() + " из множества A " +
                "(обозначим его C) и столбца " + clmC.ToString() +
                " (из A')\r\n";

            iterationResult += "Увеличиваем элементы матрицы на число " +
                difMin.ToString() + "\r\n";
            IncreaseArrClmA(difMin);
            iterationResult += "Отмечаем элемент матрицы (" + rowCur.ToString() +
                ", " + clmC.ToString() + ") точками\r\n";
            marks[rowCur, clmC] = Mark.Points;
            
            int count = 0;
            for (int i = 0; i < n; i++)
                if (marks[i, clmC] == Mark.Line)
                    count++;
            
            iterationResult += "В столбце С находится " +
                count.ToString() + " подчеркнутый(ых) элемент(а)\r\n";
            if (count >= 1)
            {
                iterationResult += "Переводим столбец C (номер " +
                    clmC.ToString() + ") во множество A'\r\n";
                arrClmB.Remove(clmC);
                arrClmA.Add(clmC);
                return true;
            }

            while (true)
            {
                for (int j = 0; j < n; j++)
                    if (marks[rowCur, j] == Mark.Line)
                    {
                        clmD = j;
                        break;
                    }

                marks[rowCur, clmC] = Mark.Line;
                marks[rowCur, clmD] = Mark.NoMark;
                iterationResult += "Столбец номер " +
                    clmD.ToString() + " обозначаем за D, " +
                    "в столбце C подчеркиваем линией элемент (" +
                    rowCur.ToString() + ", " + clmC.ToString() +
                    ") и снимаем подчеркивание в столбце D c элемента (" +
                    rowCur.ToString() + ", " + clmD.ToString() + ")\r\n";

                count = 0;
                int rowNext = -1;
                for (int i = 0; i < n; i++)
                {
                    if (marks[i, clmD] == Mark.Line)
                        count++;
                    if (marks[i, clmD] == Mark.Points)
                        rowNext = i;
                }
                if (count == 0)
                {
                    iterationResult += "Поскольку в столбце D нет подчеркнутых " +
                    "элементов и имеется ровно 1 элемент, отмеченный точками, " +
                    "обозначаем за столбец С столбец номер " +
                    clmC.ToString() + " и выбираем в качестве текущей строку " +
                    rowCur.ToString() + "\r\n";
                    rowCur = rowNext;
                    clmC = clmD;
                }
                else
                {
                    if (count == 1)
                    {
                        bool end = true;
                        for (int j = 0; j < n; j++)
                        {
                            count = 0;
                            for (int i = 0; i < n; i++)
                                if (marks[i, j] == Mark.Line)
                                    count++;
                            if (count == 0)
                                end = false;
                        }
                        if (end)
                            return false;
                    }
                    iterationResult += "Поскольку столбец D содержит другие " +
                        "подчеркнутые элементы, убираем все отметки точками " +
                        "и переформируем множества A и A'";
                    for (int i = 0; i < n; i++)
                        for (int j = 0; j < n; j++)
                            if (marks[i, j] == Mark.Points)
                                marks[i, j] = Mark.NoMark;
                    arrClmA.Clear();
                    arrClmB.Clear();
                    FormClmArrays();
                    break;
                }
            }
            for (int j = 0; j < n; j++)
            {
                count = 0;
                for (int i = 0; i < n; i++)
                    if (marks[i, j] == Mark.Line)
                        count++;
                if (count == 0)
                    return true;
            }
            return false;
        }
        public string GetResult()
        {
            string res = "\r\nОтвет:\r\ni / j\t";
            for (int j = 0; j < n; j++)
                res += j.ToString() + "\t";
            res += "\r\n";
            for (int i = 0; i < n; i++)
            {
                res += i.ToString() + "\t";
                for (int j = 0; j < n; j++)
                    if (marks[i, j] == Mark.Line)
                        res += "x\t";
                    else
                        res += "\t";
                res += "\r\n";
            }
            return res;
        }
        public int[] GetResultIndexes() // возвр. массив номеров столбцов
        {
            int[] arrIndex = new int[n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    if (marks[i, j] == Mark.Line)
                        arrIndex[i] = j;
            return arrIndex;
        }
        void ClearMarks()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    marks[i, j] = Mark.NoMark;
        }
        void MarkMinElements()
        {
            for (int i = 0; i < n; i++)
            {
                int jMin = -1;
                float min = float.MaxValue;
                for (int j = 0; j < n; j++)
                {
                    if (elements[i, j] < min)
                    {
                        jMin = j;
                        min = elements[i, j];
                    }
                }                    
                marks[i, jMin] = Mark.Line;
            }
        }
        void FormClmArrays()
        {
            int clmA = -1;
            for (int j = 0; j < n; j++)
            {
                int count = 0;
                for (int i = 0; i < n; i++)
                    if (marks[i, j] != Mark.NoMark)
                        count++;
                if (count > 1)
                {
                    arrClmA.Add(j);
                    clmA = j;
                    break;
                }
            }
            for (int j = 0; j < n; j++)
                if (j != clmA)
                    arrClmB.Add(j);
        }
        void GetMinDif(out int row, out int clmC, out int clmD, out float difMin)
        {
            row = -1;
            clmC = -1;
            clmD = -1;
            difMin = float.MaxValue;
            for (int i = 0; i < n; i++)
            {
                int count = 0;
                foreach (int j in arrClmA)
                    if (marks[i, j] == Mark.Line)
                        count++;
                if (count == 0)
                    continue;
                float dif = 0;
                int jMinA = -1;
                float min = float.MaxValue;
                foreach (int j in arrClmA)
                {
                    if (elements[i, j] < min)
                    {
                        jMinA = j;
                        min = elements[i, j];
                    }
                }
                dif -= min;

                int jMinB = -1;
                min = float.MaxValue;
                foreach (int j in arrClmB)
                {
                    if (elements[i, j] < min)
                    {
                        jMinB = j;
                        min = elements[i, j];
                    }
                }
                dif += min;
                if (dif >= 0 &&
                    dif < difMin)
                {
                    row = i;
                    difMin = dif;
                    clmC = jMinB;
                    clmD = jMinA;
                }
            }
            return;
        }
        void IncreaseArrClmA(float x)
        {
            foreach (int j in arrClmA)
                for (int i = 0; i < n; i++)
                    elements[i, j] += x;
        }
    }
    public enum Mark
    {
        NoMark, Line, Points
    }
}
