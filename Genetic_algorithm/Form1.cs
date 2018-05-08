using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Genetic_algorithm
{
    public partial class Form1 : Form
    {
        string result = "";
        public string setw(string st, int wide)
        {
            switch (st.Length)
            {
                case 1: wide += 5; break;
                case 2: wide += 4; break;
                case 3: wide += 3; break;
                case 4: wide += 2; break;
                case 5: wide += 1; break;
            }
            if (st.Contains("-")) wide += 1;
            int length = wide - st.Length;
            for (int i = 0; i < length; i++)
            {
                st += ' ';
            }
            return st;
        }
        public string setwf(string st, int wide)
        {
            int length = wide - st.Length;
            for (int i = 0; i < length; i++)
            {
                st += ' ';
            }
            return st;
        }
        public void PrintArray(ref int[][] Array, int maxI, int maxJ)
        {

            using (StreamWriter sw = File.AppendText(Generation.Path))
            {
                for (int i = 0; i < maxI; i++)
                {
                    //textBox6.Text += i.ToString() + ")  ";
                    sw.Write(i.ToString() + ")  ");
                    for (int j = 0; j < maxJ; j++)
                    {
                        // textBox6.Text += setw(Array[i][j].ToString(), 5);
                        sw.Write(setwf(Array[i][j].ToString(), 5));
                    }
                    //textBox6.Text += Environment.NewLine;
                    sw.WriteLine();
                }
            }
        }
        public void PrintArray1(ref int[][] Array, int maxI, int maxJ, int quantityProcessors, ref int[] ArrayVitality)
        {
            double d = 256.0 / quantityProcessors;
            using (StreamWriter sw = File.AppendText(Generation.Path))
            {
                for (int i = 0; i < maxI; i++)
                {
                    //textBox6.Text += i.ToString() + ")  ";
                    sw.Write(i.ToString() + ")  ");
                    for (int j = 0; j < maxJ; j++)
                    {
                        //textBox6.Text += setw(Math.Floor((Array[i][j] / Math.Floor(d + 1))).ToString(), 5);
                        sw.Write(setwf(Math.Floor((Array[i][j] / Math.Floor(d + Generation.q))).ToString(), 5));
                    }
                    //textBox6.Text += "  =  " + ArrayVitality[i] + Environment.NewLine;
                    sw.WriteLine("  =  " + ArrayVitality[i]);
                }
            }
        }
        
        public bool Revolution(ref Generation g)
        {
            int[] someArr = new int[g.quantityTasks];
            int some = 0;
            int min = g.ArrayVitality.Min();
            int minIndex = Array.IndexOf(g.ArrayVitality, g.ArrayVitality.Min());
            if (g.ArrayVitality[0] > min)
            {
                using (StreamWriter sw = File.AppendText(Generation.Path))
                {
                    sw.WriteLine("Произошла революция! Особь: " + minIndex + " c " + min + " свергает 0 c " + g.ArrayVitality[0]);
                }
                for (int i = 0; i < g.quantityTasks; i++)
                {
                    someArr[i] = g.ArrayOfPopulation[0][i];
                    g.ArrayOfPopulation[0][i] = g.ArrayOfPopulation[minIndex][i];
                    g.ArrayOfPopulation[minIndex][i] = someArr[i];
                }
                some = g.ArrayVitality[0];
                g.ArrayVitality[0] = min;
                g.ArrayVitality[minIndex] = some;
                return true;
            }
            return false;
        }
       
        public int SortArr(ref Generation g, bool mut_output, int numberSort, GoldbergModel gm)
        {
            int oldMin = 0;
            int repeat = 1;//для людей
            AllSort sortTasks = new AllSort();
            CriticalPathMethod cmp = new CriticalPathMethod();
            int[][] ar;
            using (StreamWriter sw = File.AppendText(Generation.Path))
            {
                switch (numberSort)
                {
                    case 1:
                        sw.WriteLine("Отсортированная матрица заданий  (модификация 1)");
                        ar = sortTasks.Sort1Tasks(g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks); 
                        break;
                    case 2:
                        sw.WriteLine("Отсортированная матрица заданий  (модификация 2)");
                        ar = sortTasks.Sort2Tasks(g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks); 
                        break;
                    case 3:
                        sw.WriteLine("Отсортированная матрица заданий  (модификация 3)");
                        ar = sortTasks.Sort3Tasks(g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks); 
                        break;
                }
                
            }
            PrintArray(ref g.ArrayOfTasks, g.quantityTasks, g.quantityProcessors);
            using (StreamWriter sw = File.AppendText(Generation.Path))
            {
                sw.WriteLine("Распределение задач по процессорам");
            }
            g.FillArrayOfPopulation(); //заполнение до распределения по процессорам т.к так при распределении формируется эоитная особь
           
            int z = cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0);//тут записывается элитная особь 0
            using (StreamWriter sw = File.AppendText(Generation.Path))
            {
                sw.WriteLine("Максимальная загрузка " + z);
            }
            using (StreamWriter sw = File.AppendText(Generation.Path))
            {
                sw.WriteLine("Генетическим алгоритмом, где этита(особь 0) получена методом критического пути с модификацией 1");
                sw.WriteLine("Матрица особей");
            }
            gm.vitality(g.quantityProcessors, g.quantityTasks, ref g.ArrayOfTasks, ref g.ArrayOfPopulation,
                ref g.ArrayVitality, g.populationSize); //ф-я выживаемости
            PrintArray(ref g.ArrayOfPopulation, g.populationSize, g.quantityTasks);// вывод матрицы 255 особей
            PrintArray1(ref g.ArrayOfPopulation, g.populationSize, g.quantityTasks, g.quantityProcessors, ref g.ArrayVitality); //вывод матрицы с ф-й выживаемости
            if(Revolution(ref g))
            {
                PrintArray1(ref g.ArrayOfPopulation, g.populationSize, g.quantityTasks, g.quantityProcessors, ref g.ArrayVitality); //вывод матрицы с ф-й выживаемости
            }
            using (StreamWriter sw = File.AppendText(Generation.Path))
            {
                sw.WriteLine("Результаты поколений");
            }
            while (repeat != (Convert.ToInt32(textBox7.Text)))
            {
                gm.Cross(mut_output, 1, g.quantityTasks, g.populationSize, ref g.ArrayOfPopulation,
                    g.probabilityСrossing, g.quantityProcessors, g.probabilityMutation, ref g.ArrayOfTasks, ref g.ArrayVitality); //элита в скрещивании не участвует. mut_output
                if (oldMin == g.ArrayVitality.Min())
                {
                    repeat++;
                }
                else
                {
                    repeat = 1;
                }
                oldMin = g.ArrayVitality.Min();
                using (StreamWriter sw = File.AppendText(Generation.Path))
                {
                    for (int i = 0; i < g.populationSize; i++)
                    {
                        sw.Write(setwf(g.ArrayVitality[i].ToString(), 5));
                    }
                    sw.WriteLine(" = " + g.ArrayVitality.Min());
                }
            }           
            return g.ArrayVitality.Min();
        }

        public int Genetic(ref Generation g, bool mut_output, GoldbergModel gm)
        {
            int oldMin = 0;
            int repeat = 1;//для людей
            g.FillArrayOfPopulation();
            gm.vitality(g.quantityProcessors, g.quantityTasks, ref g.ArrayOfTasks, ref g.ArrayOfPopulation,
                ref g.ArrayVitality, g.populationSize); //ф-я выживаемости
            //using (StreamWriter sw = File.AppendText(Generation.Path))
            //{
            //    sw.WriteLine("Распределение по процессорам в общем диапазоне от 0..255");
            //}  
            //PrintArray(ref g.ArrayOfPopulation, g.populationSize, g.quantityTasks);// вывод матрицы 255 особей
            //using (StreamWriter sw = File.AppendText(Generation.Path))
            //{
            //    sw.WriteLine("Определение процессора, подсчет максимальной загрузки");
            //}
            //PrintArray1(ref g.ArrayOfPopulation, g.populationSize, g.quantityTasks, g.quantityProcessors, ref g.ArrayVitality); //вывод матрицы с ф-й выживаемости
            //using (StreamWriter sw = File.AppendText(Generation.Path))
            //{
            //    sw.WriteLine("Результаты поколений");
            //}
                while (repeat != (Convert.ToInt32(textBox7.Text)))
                {
                gm.Cross(mut_output, 0, g.quantityTasks, g.populationSize, ref g.ArrayOfPopulation,
                g.probabilityСrossing, g.quantityProcessors, g.probabilityMutation, ref g.ArrayOfTasks, ref g.ArrayVitality);//mut_output
                if (oldMin == g.ArrayVitality.Min())
                    {
                        repeat++;
                    }
                    else
                    {
                        repeat = 1;
                    }
                    oldMin = g.ArrayVitality.Min();
                //    using (StreamWriter sw = File.AppendText(Generation.Path))
                //    {
                //        for (int i = 0; i < g.populationSize; i++)
                //        {
                //       sw.Write(setwf(g.ArrayVitality[i].ToString(), 5));
                //        }
                //    sw.WriteLine(" = " + g.ArrayVitality.Min());
                //    }
                //PrintArray1(ref g.ArrayOfPopulation, g.quantityTasks, g.populationSize, g.quantityProcessors, ref g.ArrayVitality);
                }
            return g.ArrayVitality.Min();
        }

        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            if(!backgroundWorker1.IsBusy && !backgroundWorker2.IsBusy)
            {
                progressBar1.Maximum = Convert.ToInt32(textBox8.Text);
                backgroundWorker1.RunWorkerAsync();
                button1.Text = "Прервать";
            }
            else if (backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox6.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy && !backgroundWorker2.IsBusy)
            {
                backgroundWorker2.RunWorkerAsync();
                progressBar1.Maximum = Convert.ToInt32(textBox11.Text);
                button3.Text = "Прервать";
            }
            else if (backgroundWorker2.IsBusy)
                backgroundWorker2.CancelAsync();
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //исследование 1
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            Generation g = new Generation(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text),
Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text));
            float res = 0;
            float res_sort1 = 0;
            float res_sort2 = 0;
            float res_sort3 = 0;
            float res_sort1_genetic = 0;
            float res_sort2_genetic = 0;
            float res_sort3_genetic = 0;
            float res_genetic = 0;
            float res_genetic2 = 0;
            float count_arr = Convert.ToInt32(textBox8.Text);
            bool mut_output = false; //отвечает за вывод мутации и скрещивания
            if (radioButton1.Checked)
            {
                mut_output = true;
            }
            else
            {
                mut_output = false;
            }

            AllSort sortTasks = new AllSort();
            CriticalPathMethod cmp = new CriticalPathMethod();
            GoldbergModel gm = new GoldbergModel();
            //int iter = 100 / (int)count_arr;

            if (!File.Exists("file.txt")) // для очистки файла
            { using (StreamWriter sw = File.CreateText(Generation.Path)) { } }
            else
            { using (File.Create(Generation.Path)) { } }

            for (int i = 1; i <= count_arr; i++) // рассчет для заданного кол-ва матриц
            {
                if (backgroundWorker1.CancellationPending) //если поток приостановлен
                {
                    e.Cancel = true;
                }
                else // выполнение хода исследования
                {
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        sw.WriteLine("Начальная матрица заданий");
                    }
                    g.FillArrayOfTasks(); // заполняем матрицу произвольными значениями
                    PrintArray(ref g.ArrayOfTasks, g.quantityTasks, g.quantityProcessors);
                    //одну и ту же матрицу распледеляем разлисными способами:
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        sw.WriteLine("Распределение заданий методом критического пути без элиты.");
                    }
                    res += cmp.distributionByProcessors(true, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); // 1) обычным методом критического пути
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        sw.WriteLine("Максимальная загрузка " + res);
                        sw.WriteLine("Генетическим алгоритмом без элиты");
                    }
                    res_genetic += Genetic(ref g, mut_output, gm); //2) генетическим алгоритмом
                    sortTasks.Sort1Tasks(g.quantityProcessors, g.quantityProcessors, ref g.ArrayOfTasks);
                    res_sort1 += cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); //3) методом критического пути с модификацией 1
                    res_sort1_genetic += SortArr(ref g, mut_output, 1, gm); // 4) генетическим, где этита(особь 0) получена методом критического пути с модификацией 1
                                                                            //res_sort1_genetic += Genetic(ref g, mut_output, gm); // генетическим алгоритмом где нач. матрица получена модификаей 1, без элиты
                    sortTasks.Sort2Tasks(g.quantityProcessors, g.quantityProcessors, ref g.ArrayOfTasks);
                    res_sort2 += cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); //5) методом критического пути с модификацией 2
                    res_sort2_genetic += SortArr(ref g, mut_output, 2, gm); //6) генетическим, где этита(особь 0) получена методом критического пути с модификацией 2
                                                                            //res_sort2_genetic += Genetic(ref g, mut_output, gm); // генетическим алгоритмом где нач. матрица получена модификаей 1, без элиты
                    sortTasks.Sort3Tasks(g.quantityProcessors, g.quantityProcessors, ref g.ArrayOfTasks);
                    res_sort3 += cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); //7) методом критического пути с модификацией 3
                    res_sort3_genetic += SortArr(ref g, mut_output, 3, gm); // 8) генетическим, где этита(особь 0) получена методом критического пути с модификацией 3
                                                                            //res_sort3_genetic += Genetic(ref g, mut_output, gm); // генетическим алгоритмом где нач. матрица получена модификаей 1, без элиты

                    backgroundWorker1.ReportProgress(i);
                }
            }
            using (StreamWriter sw = File.AppendText(Generation.Path))
            {
                result += "Обычное распределение = " + (res / count_arr) + Environment.NewLine;
                sw.WriteLine("Обычное распределение = " + (res / count_arr));
                result += "Генетический алгоритм без элиты = " + (res_genetic / count_arr) + Environment.NewLine;
                sw.WriteLine("Генетический алгоритм без элиты = " + (res_genetic / count_arr));
                //result += "Генетический алгоритм - нач. поколение элита = " + (res_genetic2 / count_arr) + Environment.NewLine;
                //sw.WriteLine("Генетический алгоритм - нач. поколение элита = " + (res_genetic2 / count_arr));
                result += "Модификации: " + Environment.NewLine;
                result += "Сортировка 1 = " + (res_sort1 / count_arr) + Environment.NewLine;
                result += "Сортировка 2 = " + (res_sort2 / count_arr) + Environment.NewLine;
                result += "Сортировка 3 = " + (res_sort3 / count_arr) + Environment.NewLine;
                sw.WriteLine("Модификации");
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой1 = " + (res_sort1 / count_arr));
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой2 = " + (res_sort2 / count_arr));
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой3 = " + (res_sort3 / count_arr));
                result += "Генетический алгоритм с элитой" + Environment.NewLine;
                result += "Сортировка 1 = " + (res_sort1_genetic / count_arr) + Environment.NewLine;
                result += "Сортировка 2 = " + (res_sort2_genetic / count_arr) + Environment.NewLine;
                result += "Сортировка 3 = " + (res_sort3_genetic / count_arr) + Environment.NewLine;
                sw.WriteLine("Генетический алгоритм с элитой");
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой1 = " + (res_sort1_genetic / count_arr));
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой2 = " + (res_sort2_genetic / count_arr));
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой3 = " + (res_sort3_genetic / count_arr));
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if(progressBar1.Value == progressBar1.Maximum)
            {
                textBox6.Text += result;
                button1.Text = "Исследование 1";
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Процесс прерван!");
            }
            else
            {
                MessageBox.Show("Успешно!");
            }
        }
        //исследование 2
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
 
            Generation g = new Generation(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text),
            Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), 
            Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text));
            float res = 0;
            float res_genetic = 0;
            float temp = 0;
            float num_less = 0;
            float num_more = 0;
            float num_equal = 0;
            float count_arr = Convert.ToInt32(textBox8.Text);
            bool mut_output = false; //отвечает за вывод мутации и скрещивания

            if (radioButton1.Checked)
            {
                mut_output = true;
            }
            else
            {
                mut_output = false;
            }
            AllSort sortTasks = new AllSort();
            CriticalPathMethod cmp = new CriticalPathMethod();
            GoldbergModel gm = new GoldbergModel();
            if (!File.Exists("file.txt")) // для очистки файла
            { using (StreamWriter sw = File.CreateText(Generation.Path)) { } }
            else
            { using (File.Create(Generation.Path)) { } }

            /* Формирование начальной матрицы */

            using (StreamWriter sw = File.AppendText(Generation.Path))
            {
                sw.WriteLine("Начальная матрица заданий");
            }
            for (int h = 1; h <= Convert.ToInt32(textBox11.Text); h++) //основной цикл прогона матриц !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            {
                if (backgroundWorker2.CancellationPending)
                {
                    e.Cancel = true;
                }
                else
                {

                    g.FillArrayOfTasks(); // заполняем матрицу произвольными значениями
                                          //PrintArray(ref g.ArrayOfTasks, g.quantityTasks, g.quantityProcessors);
                                          //одну и ту же матрицу распледеляем разлиными способами:

                    num_equal = 0; num_less = 0; num_more = 0; res_genetic = 0;
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        sw.WriteLine("Распределение заданий методом критического пути без элиты.");
                        sw.WriteLine("Осталось запусков:  " + h);
                    }
                    res = cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); // 1) обычным методом критического пути

                    for (int i = 0; i < count_arr; i++) // рассчет для заданного кол-ва матриц
                    {
                        temp = Genetic(ref g, mut_output, gm); //2) генетическим алгоритмом
                        res_genetic += temp;
                        if (res == temp)
                        {
                            num_equal++;
                        }
                        else if (res > temp)
                        {
                            num_less++;
                        }
                        else
                        {
                            num_more++;
                        }
                    }
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        // textBox6.Text += "Без модификаций" + Environment.NewLine;
                        // textBox6.Text += "Методом критического пути " + res + Environment.NewLine;
                        //  textBox6.Text += "Среднее значение генетическим алгоритмом: " + (res_genetic / count_arr) + Environment.NewLine;
                        //  textBox6.Text += "Равных значений: " + num_equal + ", больше: " + num_more + ", меньше: " + num_less + Environment.NewLine;
                        sw.WriteLine("Без модификаций");
                        sw.WriteLine("Методом критического пути " + res);
                        sw.WriteLine("Среднее значение генетическим алгоритмом: " + (res_genetic / count_arr));
                        sw.WriteLine("Равных значений: " + num_equal + ", больше: " + num_more + ", меньше: " + num_less);
                    }
                    num_equal = 0; num_less = 0; num_more = 0; res_genetic = 0;
                    sortTasks.Sort1Tasks(g.quantityProcessors, g.quantityProcessors, ref g.ArrayOfTasks);
                    res = cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); //3) методом критического пути с модификацией 1
                    for (int i = 0; i < count_arr; i++) // рассчет для заданного кол-ва матриц
                    {
                        temp = Genetic(ref g, mut_output, gm); // генетическим алгоритмом где нач. матрица получена модификаей 1, без элиты
                        res_genetic += temp;
                        if (res == temp) //подсчитываем сколько из результатов методом МКП больше генетического, сколько меньше, сколько равно.
                        {
                            num_equal++;
                        }
                        else if (res > temp)
                        {
                            num_less++;
                        }
                        else
                        {
                            num_more++;
                        }

                    }
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        result += "Модификация 1" + Environment.NewLine;
                        result += "Методом критического пути " + res + Environment.NewLine;
                        result += "Среднее значение генетическим алгоритмом: " + (res_genetic / count_arr) + Environment.NewLine;
                        result += "Равных значений: " + num_equal + ", больше: " + num_more + ", меньше: " + num_less + Environment.NewLine;
                        sw.WriteLine("Модификация 1");
                        sw.WriteLine("Методом критического пути " + res);
                        sw.WriteLine("Среднее значение генетическим алгоритмом: " + (res_genetic / count_arr));
                        sw.WriteLine("Равных значений: " + num_equal + ", больше: " + num_more + ", меньше: " + num_less);
                    }
                    num_equal = 0; num_less = 0; num_more = 0; res_genetic = 0;

                    sortTasks.Sort2Tasks(g.quantityProcessors, g.quantityProcessors, ref g.ArrayOfTasks);
                    res = cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); //5) методом критического пути с модификацией 2
                                                                                                                                                      //res_sort2_genetic += SortArr(ref g, mut_output, 2, gm); //6) генетическим, где этита(особь 0) получена методом критического пути с модификацией 2
                    for (int i = 0; i < count_arr; i++) // рассчет для заданного кол-ва матриц
                    {
                        temp = Genetic(ref g, mut_output, gm); // генетическим алгоритмом где нач. матрица получена модификаей 1, без элиты
                        res_genetic += temp;
                        if (res == temp)
                        {
                            num_equal++;
                        }
                        else if (res > temp)
                        {
                            num_less++;
                        }
                        else
                        {
                            num_more++;
                        }
                    }
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        result += "Модификация 2" + Environment.NewLine;
                        result += "Методом критического пути " + res + Environment.NewLine;
                        result += "Среднее значение генетическим алгоритмом: " + (res_genetic / count_arr) + Environment.NewLine;
                        result += "Равных значений: " + num_equal + ", больше: " + num_more + ", меньше: " + num_less + Environment.NewLine;
                        sw.WriteLine("Модификация 2");
                        sw.WriteLine("Методом критического пути " + res);
                        sw.WriteLine("Среднее значение генетическим алгоритмом: " + (res_genetic / count_arr));
                        sw.WriteLine("Равных значений: " + num_equal + ", больше: " + num_more + ", меньше: " + num_less);
                    }
                    num_equal = 0; num_less = 0; num_more = 0; res_genetic = 0;
                    sortTasks.Sort3Tasks(g.quantityProcessors, g.quantityProcessors, ref g.ArrayOfTasks);
                    res = cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); //7) методом критического пути с модификацией 3
                    for (int i = 0; i < count_arr; i++) // рассчет для заданного кол-ва матриц
                    {
                        temp = Genetic(ref g, mut_output, gm); // генетическим алгоритмом где нач. матрица получена модификаей 1, без элиты
                        res_genetic += temp;
                        if (res == temp)
                        {
                            num_equal++;
                        }
                        else if (res > temp)
                        {
                            num_less++;
                        }
                        else
                        {
                            num_more++;
                        }
                    }
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        result += "Модификация 3" + Environment.NewLine;
                        result += "Методом критического пути " + res + Environment.NewLine;
                        result += "Среднее значение генетическим алгоритмом: " + (res_genetic / count_arr) + Environment.NewLine;
                        result += "Равных значений: " + num_equal + ", больше: " + num_more + ", меньше: " + num_less + Environment.NewLine;
                        sw.WriteLine("Модификация 3");
                        sw.WriteLine("Методом критического пути " + res);
                        sw.WriteLine("Среднее значение генетическим алгоритмом: " + (res_genetic / count_arr));
                        sw.WriteLine("Равных значений: " + num_equal + ", больше: " + num_more + ", меньше: " + num_less);
                    }
                    backgroundWorker2.ReportProgress(h);
                }
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (progressBar1.Value == progressBar1.Maximum)
            {
                textBox6.Text = result;
                button3.Text = "Исследование 2";
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Процесс прерван!");
            }
            else
            {
                MessageBox.Show("Успешно!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy && !backgroundWorker2.IsBusy && !backgroundWorker3.IsBusy)
            {
                backgroundWorker3.RunWorkerAsync();
                progressBar1.Maximum = Convert.ToInt32(textBox8.Text);
                button4.Text = "Прервать";
            }
            else if (backgroundWorker3.IsBusy)
                backgroundWorker3.CancelAsync();
        }
        int countClickRadioButton = 0; //для многоразового изменения радиокнопки
        private void radioButton1_Click(object sender, EventArgs e)
        {
            if (countClickRadioButton % 2==0)
            {
                 radioButton1.Checked = true;
            }
            else
            {
                radioButton1.Checked = false;
            }
            countClickRadioButton++;
        }
        //исследование 3
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            Generation g = new Generation(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text),
 Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text));
            float res = 0;
            float res_sort1 = 0;
            float res_sort2 = 0;
            float res_sort3 = 0;
            float res_sort1_genetic = 0;
            float res_sort2_genetic = 0;
            float res_sort3_genetic = 0;
            float res_genetic = 0;
            float res_genetic2 = 0;
            float count_arr = Convert.ToInt32(textBox8.Text);
            bool mut_output = false; //отвечает за вывод мутации и скрещивания
            if (radioButton1.Checked)
            {
                mut_output = true;
            }
            else
            {
                mut_output = false;
            }

            AllSort sortTasks = new AllSort();
            CriticalPathMethod cmp = new CriticalPathMethod();
            GoldbergModel gm = new GoldbergModel();
            //int iter = 100 / (int)count_arr;

            if (!File.Exists("file.txt")) // для очистки файла
            { using (StreamWriter sw = File.CreateText(Generation.Path)) { } }
            else
            { using (File.Create(Generation.Path)) { } }

            for (int i = 1; i <= count_arr; i++) // рассчет для заданного кол-ва матриц
            {
                if (backgroundWorker3.CancellationPending) //если поток приостановлен
                {
                    e.Cancel = true;
                }
                else // выполнение хода исследования
                {
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        sw.WriteLine("Начальная матрица заданий");
                    }
                    g.FillArrayOfTasks(); // заполняем матрицу произвольными значениями
                    PrintArray(ref g.ArrayOfTasks, g.quantityTasks, g.quantityProcessors);
                    //одну и ту же матрицу распледеляем разлисными способами:
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        sw.WriteLine("Распределение заданий методом критического пути без элиты.");
                    }
                    res += cmp.distributionByProcessors(true, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); // 1) обычным методом критического пути
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        sw.WriteLine("Максимальная загрузка " + res);
                        sw.WriteLine("Генетическим алгоритмом без элиты");
                    }
                    res_genetic += Genetic(ref g, mut_output, gm); //2) генетическим алгоритмом
                    sortTasks.Sort1Tasks(g.quantityProcessors, g.quantityProcessors, ref g.ArrayOfTasks);
                    res_sort1 += cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); //3) методом критического пути с модификацией 1
                    res_sort1_genetic += SortArr(ref g, mut_output, 1, gm); // 4) генетическим, где этита(особь 0) получена методом критического пути с модификацией 1
                                                                            //res_sort1_genetic += Genetic(ref g, mut_output, gm); // генетическим алгоритмом где нач. матрица получена модификаей 1, без элиты
                    sortTasks.Sort2Tasks(g.quantityProcessors, g.quantityProcessors, ref g.ArrayOfTasks);
                    res_sort2 += cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); //5) методом критического пути с модификацией 2
                    res_sort2_genetic += SortArr(ref g, mut_output, 2, gm); //6) генетическим, где этита(особь 0) получена методом критического пути с модификацией 2
                                                                            //res_sort2_genetic += Genetic(ref g, mut_output, gm); // генетическим алгоритмом где нач. матрица получена модификаей 1, без элиты
                    sortTasks.Sort3Tasks(g.quantityProcessors, g.quantityProcessors, ref g.ArrayOfTasks);
                    res_sort3 += cmp.distributionByProcessors(false, g.quantityTasks, g.quantityProcessors, ref g.ArrayOfTasks, ref g.ArrayOfPopulation, 0); //7) методом критического пути с модификацией 3
                    res_sort3_genetic += SortArr(ref g, mut_output, 3, gm); // 8) генетическим, где этита(особь 0) получена методом критического пути с модификацией 3
                                                                            //res_sort3_genetic += Genetic(ref g, mut_output, gm); // генетическим алгоритмом где нач. матрица получена модификаей 1, без элиты

                    backgroundWorker3.ReportProgress(i);
                }
            }
            using (StreamWriter sw = File.AppendText(Generation.Path))
            {
                result += "Обычное распределение = " + (res / count_arr) + Environment.NewLine;
                sw.WriteLine("Обычное распределение = " + (res / count_arr));
                result += "Генетический алгоритм без элиты = " + (res_genetic / count_arr) + Environment.NewLine;
                sw.WriteLine("Генетический алгоритм без элиты = " + (res_genetic / count_arr));
                //result += "Генетический алгоритм - нач. поколение элита = " + (res_genetic2 / count_arr) + Environment.NewLine;
                //sw.WriteLine("Генетический алгоритм - нач. поколение элита = " + (res_genetic2 / count_arr));
                result += "Модификации: " + Environment.NewLine;
                result += "Сортировка 1 = " + (res_sort1 / count_arr) + Environment.NewLine;
                result += "Сортировка 2 = " + (res_sort2 / count_arr) + Environment.NewLine;
                result += "Сортировка 3 = " + (res_sort3 / count_arr) + Environment.NewLine;
                sw.WriteLine("Модификации");
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой1 = " + (res_sort1 / count_arr));
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой2 = " + (res_sort2 / count_arr));
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой3 = " + (res_sort3 / count_arr));
                result += "Генетический алгоритм с элитой" + Environment.NewLine;
                result += "Сортировка 1 = " + (res_sort1_genetic / count_arr) + Environment.NewLine;
                result += "Сортировка 2 = " + (res_sort2_genetic / count_arr) + Environment.NewLine;
                result += "Сортировка 3 = " + (res_sort3_genetic / count_arr) + Environment.NewLine;
                sw.WriteLine("Генетический алгоритм с элитой");
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой1 = " + (res_sort1_genetic / count_arr));
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой2 = " + (res_sort2_genetic / count_arr));
                sw.WriteLine("Среднее максимальное значение для " + count_arr + " матриц сортировкой3 = " + (res_sort3_genetic / count_arr));
            }
        }

        private void backgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            if (progressBar1.Value == progressBar1.Maximum)
            {
                textBox6.Text += result;
                button4.Text = "Исследование 3";
            }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Процесс прерван!");
            }
            else
            {
                MessageBox.Show("Успешно!");
            }
        }
    }
}
