using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfApplication1
{
    class MyData
    {
        public string matrixA
        {
            get
            {
                string text = string.Empty;
                int m = A.GetLength(0);
                int n = A.GetLength(1);
                for (int i = 0; i < m; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        text += " " + A[i, j].ToString();
                    }
                    text += "\r\n";
                }
                return text;
            }
        }
        public string matrixU { get { return ShowMatrix(U); } }
        public string matrixVT { get { return ShowMatrix(VT); } }
        public string matrixW
        {
            get
            {
                string w = string.Empty;
                foreach (var item in W)
                {
                    w += " "+item.ToString("F", CultureInfo.CurrentCulture);
                }
                return w;
            }
        }
        //A = U * W * V^T
        double[,] A;
        double[,] U;
        double[,] VT;
        double[] W;
        //Размер матрицы А
        readonly int m;
        readonly int n;
        //Возвращаемые аргументы(возвращаем целиком матрицу U и V^T)
        const int uNeeded = 1;
        const int vtNeeded = 1;
        //Производительность(от 0 до 2)- за счет выделения памяти
        const int additionalMemory = 2;

        //Наши поля для вывода
        public KeyValuePair<double, double>[] allTextsCoords
        {
            get
            {            //get 2 row and
                KeyValuePair<double, double>[] allTextsCoords = new KeyValuePair<double, double>[m];
                for (int j = 0; j < VT.GetLength(1); j++)
                {
                    allTextsCoords[j] = new KeyValuePair<double, double>(VT[0, j], VT[1, j]);
                }
                return allTextsCoords;
            }
        }
        public KeyValuePair<double, double>[] allWordsCoords
        {
            get
            {
                //get 2 column
                KeyValuePair<double, double>[] allWordsCoords = new KeyValuePair<double, double>[m];
                for (int i = 0; i < U.GetLength(0); i++)
                {
                    allWordsCoords[i] = new KeyValuePair<double, double>(U[i, 0], U[i, 1]);
                }
                return allWordsCoords;
            }
        }

        private void InitializeDefault(double[,] A,out double[,] U, out double[,] VT, out double[] W)
        {
            Console.WriteLine("Current Matrix");
            ShowMatrix(A);

            Console.WriteLine("Do singular transformation");

            if (alglib.rmatrixsvd(A, m, n, uNeeded, vtNeeded, additionalMemory, out W, out U, out VT))
            {
                Console.WriteLine("Matrix W");
                foreach (var item in W)
                {
                    Console.Write(" " + item);
                }

                Console.WriteLine("\r\nMatrix U");
                ShowMatrix(U);

                Console.WriteLine("Matrix VT");
                ShowMatrix(VT);
            }
        }

        public KeyValuePair<double, double>[] TextsCoords(double[,] VT)
        {
            //get 2 row and
            KeyValuePair<double, double>[] allTextsCoords = new KeyValuePair<double, double>[m];
            for (int j = 0; j < VT.GetLength(1); j++)
            {
                allTextsCoords[j] = new KeyValuePair<double, double>(VT[0, j], VT[1, j]);
            }
            return allTextsCoords;
        }

        public KeyValuePair<double, double>[] WordsCoords(double[,] U)
        {
            //get 2 column
            KeyValuePair<double, double>[] allWordsCoords = new KeyValuePair<double, double>[m];
            for (int i = 0; i < U.GetLength(0); i++)
            {
                allWordsCoords[i] = new KeyValuePair<double, double>(U[i, 0], U[i, 1]);
            }
            return allWordsCoords;
        }

        private string ShowMatrix(double[,] A)
        {
            string text=string.Empty;
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    text+=" "+ A[i, j].ToString("F", CultureInfo.CurrentCulture);
                }
                text += "\r\n";
            }
            return text;
        }
   
        public MyData()
        {
            LSAMatrixCreator creator = new LSAMatrixCreator();
            string temp = System.AppDomain.CurrentDomain.BaseDirectory+"ReadyMatrix\\matrix.txt";
            if (File.Exists(temp))
            {
                A = creator.GetReadyMatrix(temp);
            }
            else
            {
                A = creator.GetReadyMatrix();
            }

            m = A.GetLength(0);
            n = A.GetLength(1);
            InitializeDefault(A, out U, out VT, out W);
        }

    }
}
