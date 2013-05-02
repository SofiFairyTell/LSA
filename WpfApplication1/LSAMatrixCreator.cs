using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace WpfApplication1
{
    class LSAMatrixCreator
    {
        List<string> stopWords = new List<string>{""," ",",",".","!","?","-",":",";","\t","\r","\n"};
        
        double[,] defaultA = new double[,] 
            {
                {1,0,0,1,0,1,0,1,0},
                {0,0,0,1,0,0,0,1,0},
                {0,0,0,1,0,0,0,1,0},
                {0,0,1,0,1,0,0,0,1},
                {0,0,1,0,1,0,0,0,1},
                {1,0,0,1,0,1,0,1,0},
                {1,0,0,0,0,0,0,1,0},
                {0,0,1,0,1,0,0,0,1},
                {0,1,0,0,0,0,1,0,0},
                {0,0,1,0,0,0,1,0,0},
                {0,1,0,0,0,1,0,0,0},
                {0,1,0,0,0,0,1,0,0},
                {0,0,1,0,1,0,0,0,0}
            };
        double[,] currentMatrix;

        /// <summary>
        /// Добавляет к текущему словарю стоп-символов дополнительные слова
        /// </summary>
        /// <param name="pathToDictionary">Путь к текстовому словарю(UTF-8)</param>
        void GetStopSymbols(string pathToDictionary = @"D:\stop_symbols.txt")
        {
            StreamReader file = File.OpenText(@pathToDictionary);
           
            string line = file.ReadToEnd();
            string[] currentWords = line.Split(new Char[] { ' ', ',', '.', ':', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in currentWords)
            {
                stopWords.Add(word);
            }
        }

        public double[,] GetReadyMatrix(string pathToMatrix = @"D:\matrix.txt")
        {
            List<double> matrixRow = new List<double>();
            List<double[]> matrix = new List<double[]>();

            StreamReader file = File.OpenText(@pathToMatrix);
            while (!file.EndOfStream)
            {
                string line = file.ReadLine();
                string[] currentLineWords = line.Split(new Char[] { ' ', ',', '.', ':', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                try
                {
                    foreach (var word in currentLineWords)
                    {
                        matrixRow.Add(double.Parse(word));
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                
                matrix.Add(matrixRow.ToArray());
                matrixRow.Clear();
            }
            var result = matrix.ToArray();

            return doubleToDoubleConverter(result);
        }
        /// <summary>
        /// Переводи данные из double[][] в double[,]
        /// </summary>
        /// <param name="result">Матрица для перекодирвки</param>
        private double[,] doubleToDoubleConverter(double[][] result)
        {
            int size = result.GetLength(0);
            int size2 = result[0].GetLength(0);

            double[,] resultMatrix = new double[size, size2];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size2; j++)
                {
                    resultMatrix[i, j] = result[i][j];
                }
            }
            return resultMatrix;
        }
    }
}
