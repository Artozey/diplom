using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

/* Программное средство решения однородной минимаксной задачи при условии наличия неоднородностей */

namespace Genetic_algorithm
{
    public class ReverseComparer : IComparer
    {
        // Call CaseInsensitiveComparer.Compare with the parameters reversed.
        public int Compare(Object x, Object y)
        {
            return (new CaseInsensitiveComparer()).Compare(y, x);
        }
    }

    public class AllSort
    {
        public int[][] Sort1Tasks(int quantityTasks, int quantityProcessors, ref int[][] ArrayOfTasks) //сортировка по убыванию
        {
            int[] someArray = new int[quantityTasks]; //хранит нагрузку задания 
            int[] someArrayKey = new int[quantityTasks];
            int[][] ArrayOfTasksCopy = new int[quantityTasks][];
            for (int i = 0; i < quantityTasks; i++)
            {
                ArrayOfTasksCopy[i] = new int[quantityProcessors];
            }
            IComparer revComparer = new ReverseComparer();
            for (int i = 0; i < quantityTasks; i++)
            {
                for (int j = 0; j < quantityProcessors; j++)
                {
                    ArrayOfTasksCopy[i][j] = ArrayOfTasks[i][j];
                }
            }
            for (int i = 0; i < quantityTasks; i++)
            {
                for (int j = 0; j < quantityProcessors; j++)
                {
                    if (ArrayOfTasks[i][j] >= 0)
                    {
                        someArray[i] = ArrayOfTasks[i][j];
                        someArrayKey[i] = i;
                        break;
                    }
                }
            }
            Array.Sort(someArray, someArrayKey, revComparer);
            for (int i = 0; i < quantityTasks; i++)
            {
                for (int j = 0; j < quantityProcessors; j++)
                {
                    ArrayOfTasks[i][j] = ArrayOfTasksCopy[someArrayKey[i]][j];
                }
            }
            return ArrayOfTasks;
        }
        public int[][] Sort2Tasks(int quantityTasks, int quantityProcessors, ref int[][] ArrayOfTasks) //сортировка по убыванию количества бесконечностей
        {
            int[] ArrayOfinfinity = new int[quantityTasks]; //массив недостижимостей
            int[] someArray = new int[quantityTasks]; //хранит нагрузку задания 
            int[] someArrayKey = new int[quantityTasks];
            IComparer revComparer = new ReverseComparer();
            int[][] ArrayOfTasksCopy = new int[quantityTasks][]; //копия массива заданий
            for (int i = 0; i < quantityTasks; i++)//создание массива
            {
                ArrayOfTasksCopy[i] = new int[quantityProcessors];
            }
            for (int j = 0; j < quantityProcessors; j++) // инициализация
            {
                ArrayOfinfinity[j] = 0;
            }
            for (int i = 0; i < quantityTasks; i++) //копирование
            {
                for (int j = 0; j < quantityProcessors; j++)
                {
                    ArrayOfTasksCopy[i][j] = ArrayOfTasks[i][j];
                }
            }
            for (int i = 0; i < quantityTasks; i++)
            {
                for (int j = 0; j < quantityProcessors; j++)
                {
                    if (ArrayOfTasks[i][j] >= 0)
                    {
                        someArray[i] = ArrayOfTasks[i][j]; // храним значение однородной загрузки
                        someArrayKey[i] = i;
                    }
                    else
                    {
                        ArrayOfinfinity[i]++; //считаем кол-во бесконечностей 
                    }
                }
            }
            Array.Sort(ArrayOfinfinity, someArrayKey, revComparer);
            for (int i = 0; i < quantityTasks; i++)
            {
                for (int j = 0; j < quantityProcessors; j++)
                {
                    ArrayOfTasks[i][j] = ArrayOfTasksCopy[someArrayKey[i]][j];
                }
            }
            return ArrayOfTasks;
        }
        public int[][] Sort3Tasks(int quantityTasks, int quantityProcessors, ref int[][] ArrayOfTasks)
        {
            Sort1Tasks(quantityTasks, quantityProcessors, ref ArrayOfTasks);
            int[] ArrayOfinfinity = new int[quantityTasks]; //массив недостижимостей
            int[] someArray = new int[quantityTasks]; //хранит нагрузку задания 
            int[] someArrayKey = new int[quantityTasks];
            IComparer revComparer = new ReverseComparer();
            for (int j = 0; j < quantityProcessors; j++) // инициализация
            {
                ArrayOfinfinity[j] = 0;
            }

            for (int i = 0; i < quantityTasks; i++)
            {
                for (int j = 0; j < quantityProcessors; j++)
                {
                    if (ArrayOfTasks[i][j] >= 0)
                    {
                        someArray[i] = ArrayOfTasks[i][j]; // храним значение однородной загрузки
                        someArrayKey[i] = i;
                    }
                    else
                    {
                        ArrayOfinfinity[i]++; //считаем кол-во бесконечностей 
                    }
                }
            }
            int temp = someArray[0];
            int tempi = 0;
            int len = 0;
            for (int i = 1; i < quantityTasks; i++)
            {
                if (temp == someArray[i])
                {
                    len++;
                }
                else
                {
                    Array.Sort(ArrayOfinfinity, ArrayOfTasks, tempi, len + 1, revComparer);
                    temp = someArray[i];
                    tempi = i;
                    len = 0;
                }
            }
            Array.Sort(ArrayOfinfinity, ArrayOfTasks, tempi, len + 1, revComparer);

            return ArrayOfTasks;

        }
    }
    public class CriticalPathMethod
    {
        public int distributionByProcessors(bool output, int quantityTasks, int quantityProcessors, ref int[][] ArrayOfTasks, ref int[][] ArrayOfPopulation, int elite) // распределение задач по процессорам.
        {
            int[] result = new int[quantityProcessors];
            int numderMin = 65635;
            int indexMin = -1;
            for (int j = 0; j < quantityProcessors; j++)
            {
                result[j] = 0;
            }
            //если эл-т матрицы задания не бесконечность,
            //то складываем к предыдущему результату, находим минимальную сумму 
            //в этом массиве, запоминаем индекс этого эл-та,
            //возвращаем матрицу суммы в исходное состояние 
            // прибавляем эл-т чей индекс сохранили
            using (StreamWriter sw = File.AppendText(Generation.Path))
            {

                for (int i = 0; i < quantityTasks; i++)
                {
                    numderMin = 65635;
                    for (int j = 0; j < quantityProcessors; j++)
                    {
                        if (ArrayOfTasks[i][j] >= 0)
                        {
                            result[j] += ArrayOfTasks[i][j];
                            if (numderMin > result[j])
                            {
                                numderMin = result[j];
                                indexMin = j;
                            }
                        }
                    }
                    for (int j = 0; j < quantityProcessors; j++)
                    {
                        if (ArrayOfTasks[i][j] >= 0)
                        {
                            result[j] -= ArrayOfTasks[i][j];
                        }
                    }
                    result[indexMin] += ArrayOfTasks[i][indexMin];
                    ArrayOfPopulation[elite][i] = indexMin * (255 / quantityProcessors) + ((255 / quantityProcessors) / 2); // формируем элитную особь
                    if (output)
                    {
                        for (int j = 0; j < quantityProcessors; j++)
                        {
                            sw.Write(result[j] + "  ");
                        }
                        sw.WriteLine();
                    }
                }
            }
            return result.Max();
        }
    }

    public class MutationClass
    {
        Random Rand = new Random();
        public void Mut(bool output, ref int[] arr, int mytGen, int gen, int quantityProcessors, ref int[][] ArrayOfTasks)
        {
            using (StreamWriter sw = File.AppendText(Generation.Path))
            {

                int v = 0;
                char[] chMas = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
                string binGen = "";
                string st = "";
                int k = 0;
                double d = 256.0 / quantityProcessors;
                v = 0; binGen = ""; st = ""; k = 0;

                for (int i = 0; i < 8; i++)
                {
                    chMas[i] = '0';
                }
                if (output)
                {
                    sw.WriteLine("Мутирует ген " + mytGen);
                }
                v = arr[mytGen]; //получаем значение гена
                binGen = Convert.ToString(v, 2); //возвращает Строковое представление значения параметра v в системе счисления с основанием 2.
                k = binGen.Length - 1;
                for (int i = 7; i >= 0; i--)
                {
                    if (k >= 0)
                        chMas[i] = binGen[k--];
                }
                binGen = "";
                for (int i = 0; i < 8; i++)
                {
                    binGen += chMas[i];
                }
                if (binGen[gen] == '0')
                {
                    st = binGen;
                    binGen = "";
                    for (int i = 0; i < 8; i++)
                    {
                        if (i != gen)
                            binGen += st[i];
                        else
                            binGen += '1';
                    }
                }
                else
                {
                    st = binGen;
                    binGen = "";
                    for (int i = 0; i < 8; i++)
                    {
                        if (i != gen)
                            binGen += st[i];
                        else
                            binGen += '0';
                    }
                }
                arr[mytGen] = Convert.ToInt32(binGen, 2);
                if (output)
                {
                    sw.WriteLine("Мутация: " + ArrayOfTasks[mytGen][(int)(Math.Floor(arr[mytGen] / Math.Floor(d + Generation.q)))]);//!!!!!!!!
                }
            }
        }
        public void Mutation(ref int[] arr, bool output, int quantityProcessors, int quantityTasks, int probabilityMutation, ref int[][] ArrayOfTasks) //оболочка для мутации
        {
            int repeat = 0;
            int mytGen = 0;
            int gen = 0;
            double d = 256.0 / quantityProcessors;
            mytGen = Rand.Next(0, quantityTasks - 1); //выбираем ген для мутации
            gen = Rand.Next(0, 7); //изменяемая часть гена 0000 0000
            int[] copyArr = new int[quantityTasks];
            for (int i = 0; i < quantityTasks; i++)
            {
                copyArr[i] = arr[i];
            }
            if (Rand.Next(0, 99) < probabilityMutation)
            {
                do
                {
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        repeat++;
                        if (repeat == 4) //если 3 раза получается -1 не мутирует
                        {
                            for (int i = 0; i < quantityTasks; i++)
                            {
                                arr[i] = copyArr[i];
                            }
                            if (output)
                            {
                                sw.WriteLine("Не мутирует.");
                            }
                            break;
                        }
                    }

                    Mut(output, ref arr, mytGen, gen, quantityProcessors, ref ArrayOfTasks); //чистая мутация

                } while (ArrayOfTasks[mytGen][(int)(Math.Floor(arr[mytGen] / Math.Floor(d + Generation.q)))] == -1);
            }
        }
    }
    public class CrossoverClass
    {
        Random rand = new Random();

        public void Crossover(bool output, int quantityTasks, ref int[] child1, ref int[] child2, ref int[][] arrParent, int parent1, int parent2)//оператор кроссовера
        {
            int breakPoint = 0; //точка разбиения
            breakPoint = rand.Next(1, quantityTasks - 2);
            if (output)
            {
                using (StreamWriter sw = File.AppendText(Generation.Path))
                {
                    sw.WriteLine("Точка разбиения: " + breakPoint);
                }
            }
            for (int j = 0; j < quantityTasks; j++)
            {
                if (j < breakPoint)// если данный номер гена < точки разбиения
                {
                    child1[j] = arrParent[parent1][j];
                    child2[j] = arrParent[parent2][j];
                }
                else
                {
                    child1[j] = arrParent[parent2][j];
                    child2[j] = arrParent[parent1][j];
                }
            }
        }

    }
    public class GoldbergModel
    {
        Random rand = new Random();
        MutationClass mytt = new MutationClass();
        CrossoverClass cros = new CrossoverClass();

        public bool Revolution(int quantityTasks, ref int[] ArrayVitality, ref int[][] ArrayOfPopulation)
        {//ref Generation g
            int[] someArr = new int[quantityTasks];
            int some = 0;
            int min = ArrayVitality.Min();
            int minIndex = Array.IndexOf(ArrayVitality, ArrayVitality.Min());
            if (ArrayVitality[0] > min)
            {
                using (StreamWriter sw = File.AppendText(Generation.Path))
                {
                    sw.WriteLine("Произошла революция! Особь: " + minIndex + " c " + min + " свергает 0 c " + ArrayVitality[0]);
                }
                for (int i = 0; i < quantityTasks; i++)
                {
                    someArr[i] = ArrayOfPopulation[0][i];
                    ArrayOfPopulation[0][i] = ArrayOfPopulation[minIndex][i];
                    ArrayOfPopulation[minIndex][i] = someArr[i];
                }
                some = ArrayVitality[0];
                ArrayVitality[0] = min;
                ArrayVitality[minIndex] = some;
                return true;
            }
            return false;
        }
        public int vitalityForIndivid(ref int[] ArrayIndivid, int quantityProcessors, int quantityTasks, ref int[][] ArrayOfTasks) // считает ф-ю выживаемости для особи
        {
            int[] SomeArray = new int[quantityProcessors];

            double d = 256.0 / quantityProcessors;
            for (int k = 0; k < quantityProcessors; k++)
            {
                SomeArray[k] = 0;
            }
            for (int j = 0; j < quantityTasks; j++)
            {
                SomeArray[(int)(Math.Floor(ArrayIndivid[j] / Math.Floor(d + Generation.q)))] += ArrayOfTasks[j][(int)(Math.Floor(ArrayIndivid[j] / Math.Floor(d + Generation.q)))];
            }
            return SomeArray.Max();
        }

        public void vitality(int quantityProcessors, int quantityTasks, ref int[][] ArrayOfTasks, ref int[][] ArrayOfPopulation, ref int[] ArrayVitality,
            int populationSize) // считает ф-ю выживаемости. 
        {
            int[] SomeArray = new int[quantityProcessors];
            int[] Arr = new int[quantityTasks];
            double d = 256.0 / quantityProcessors;
            for (int i = 0; i < populationSize; i++) //потому, что 0 - это элита.
            {
                for (int k = 0; k < quantityTasks; k++)
                {
                    Arr[k] = ArrayOfPopulation[i][k];
                }
                ArrayVitality[i] = vitalityForIndivid(ref Arr,  quantityProcessors,  quantityTasks, ref ArrayOfTasks);
            }
        }
        
        public int[][] Cross(bool output, int if_elite_exists, int quantityTasks, int populationSize, 
            ref int[][] ArrayOfPopulation, int probabilityСrossing, int quantityProcessors,
            int probabilityMutation, ref int[][] ArrayOfTasks, ref int[] ArrayVitality)//оболочка для скрещивания
        {

            int parent1 = 0, parent2 = 0;
            int[] child1 = new int[quantityTasks];
            int[] child2 = new int[quantityTasks];
            int[][] arr = new int[populationSize][];
            for (int i = 0; i < populationSize; i++)
            {
                arr[i] = new int[quantityTasks];
            }
            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < quantityTasks; j++)
                {
                    arr[i][j] = ArrayOfPopulation[i][j];
                }
            }
            //модель Голдберга
            for (int i = if_elite_exists; i < populationSize; i++) //цикл по особям. if_elite_exists = 1,если элита существует и 0 - если нет.
            {
                if (rand.Next(0, 99) < probabilityСrossing)
                {
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        parent1 = i;
                        parent2 = rand.Next(0, populationSize - 1);
                        while (parent1 == parent2) //пока не найдем разных родителей
                        {
                            parent2 = rand.Next(0, populationSize - 1);
                        }

                        if (output)
                        {
                            sw.Write("Выбраны родители: " + parent1 + ") ");
                            for (int j = 0; j < quantityTasks; j++)
                            {
                                sw.Write(arr[parent1][j] + " ");
                                child1[j] = arr[parent1][j];// временно используем массив ребенка для подсчета ф-и выживаемости родителя 
                            }
                            sw.Write(" = " + vitalityForIndivid(ref child1, quantityProcessors, quantityTasks, ref ArrayOfTasks)); //считаем ф-ю выживаемости родителя1
                            sw.WriteLine();
                            sw.Write(parent2 + ") ");
                            for (int j = 0; j < quantityTasks; j++)
                            {
                                sw.Write(arr[parent2][j] + " ");
                                child2[j] = arr[parent2][j];// временно используем массив ребенка для подсчета ф-и выживаемости родителя
                            }
                            sw.Write(" = " + vitalityForIndivid(ref child2, quantityProcessors, quantityTasks, ref ArrayOfTasks)); //считаем ф-ю выживаемости родителя2
                            sw.WriteLine();

                        }
                    }
                    cros.Crossover(output, quantityTasks, ref child1, ref child2, ref arr, parent1, parent2);
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        if (output)
                        {
                            sw.Write("Ребенок 1: ");
                            for (int j = 0; j < quantityTasks; j++)
                            {
                                sw.Write(child1[j] + " ");
                            }
                            sw.Write(" = " + vitalityForIndivid(ref child1, quantityProcessors, quantityTasks, ref ArrayOfTasks));
                            sw.WriteLine();
                            sw.Write("Ребенок 2: ");
                            for (int j = 0; j < quantityTasks; j++)
                            {
                                sw.Write(child2[j] + " ");
                            }
                            sw.Write(" = " + vitalityForIndivid(ref child2, quantityProcessors, quantityTasks, ref ArrayOfTasks));
                            sw.WriteLine();
                            sw.WriteLine("Мутирует ребенок1");
                        }
                    }
                    mytt.Mutation(ref child1, output, quantityProcessors, quantityTasks, probabilityMutation, ref ArrayOfTasks); //мутация 
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        if (output)
                        {
                            sw.WriteLine("Мутирует ребенок2");
                        }
                    }
                    mytt.Mutation(ref child2, output, quantityProcessors, quantityTasks, probabilityMutation, ref ArrayOfTasks); //мутация 
                    using (StreamWriter sw = File.AppendText(Generation.Path))
                    {
                        if (output)
                        {
                            sw.WriteLine("После мутации");
                            sw.Write("Ребенок 1: ");
                            for (int j = 0; j < quantityTasks; j++)
                            {
                                sw.Write(child1[j] + " ");
                            }
                            sw.Write(" = " + vitalityForIndivid(ref child1, quantityProcessors, quantityTasks, ref ArrayOfTasks));
                            sw.WriteLine();
                            sw.Write("Ребенок 2: ");
                            for (int j = 0; j < quantityTasks; j++)
                            {
                                sw.Write(child2[j] + " ");
                            }
                            sw.Write(" = " + vitalityForIndivid(ref child2, quantityProcessors, quantityTasks, ref ArrayOfTasks));
                            sw.WriteLine();
                        }
                        if (vitalityForIndivid(ref child1, quantityProcessors, quantityTasks, ref ArrayOfTasks) < vitalityForIndivid(ref child2, quantityProcessors, quantityTasks, ref ArrayOfTasks)) //определяем лучшего потомка
                        {
                            if (vitalityForIndivid(ref child1, quantityProcessors, quantityTasks, ref ArrayOfTasks) < ArrayVitality[i])
                            {
                                if (output)
                                {
                                    sw.Write("Выжил потомок1: ");
                                }
                                for (int k = 0; k < quantityTasks; k++)  //!!! записывать, только если он лучше родителя. 
                                {
                                    arr[parent1][k] = child1[k];
                                    if (output)
                                    {
                                        sw.Write(child1[k] + " ");
                                    }
                                }
                                if (output)
                                {
                                    sw.WriteLine(" = " + vitalityForIndivid(ref child1, quantityProcessors, quantityTasks, ref ArrayOfTasks));
                                }
                                ArrayVitality[i] = vitalityForIndivid(ref child1, quantityProcessors, quantityTasks, ref ArrayOfTasks);
                            }
                        }
                        else
                        {
                            if (vitalityForIndivid(ref child2, quantityProcessors, quantityTasks, ref ArrayOfTasks) < ArrayVitality[i])
                            {
                                if (output)
                                {
                                    sw.Write("Выжил потомок2: ");
                                }
                                for (int k = 0; k < quantityTasks; k++)
                                {
                                    arr[parent1][k] = child2[k];
                                    if (output)
                                    {
                                        sw.Write(child2[k] + " ");
                                    }
                                }
                                if (output)
                                {
                                    sw.WriteLine(" = " + vitalityForIndivid(ref child2, quantityProcessors, quantityTasks, ref ArrayOfTasks));
                                }
                                ArrayVitality[i] = vitalityForIndivid(ref child2, quantityProcessors, quantityTasks, ref ArrayOfTasks);
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < quantityTasks; j++)
                {
                    ArrayOfPopulation[i][j] = arr[i][j];
                }
            }
            if (if_elite_exists == 1)
            {
                Revolution(quantityTasks, ref ArrayVitality, ref ArrayOfPopulation);
            }
            return ArrayOfPopulation;
        }
    }
    public class Generation
    {
        public int quantityTasks; //количество заданий
        public int quantityProcessors; //количество процессоров
        public int populationSize; //размер популяции
        public int probabilityСrossing; //вероятность скрещивания
        public int probabilityMutation; //верочтность мутации
        public int[][] ArrayOfTasks = new int[0][];//массив сожержащий задания
        public int[][] ArrayOfPopulation = new int[0][]; //массив содержащий всю популяцию
        public int[] ArrayVitality = new int[0]; //массив содержащий ф-и выживания всего поколения.
        public int startRangeValues;
        public int endRangeValues;
        Random Rand = new Random();
        
        static public string Path = "file.txt";
        static public double q;

        public Generation(int t, int p, int s, int c, int m, int b, int e) 
        {
            quantityTasks = t;
            quantityProcessors = p;
            populationSize = s;
            probabilityСrossing = c;
            probabilityMutation = m;
            startRangeValues = b;
            endRangeValues = e;
           if (p % 2 == 0) //подбираем коэфициент распределения.
            {
                q = 0.5; 
            }
            else
            {
                q = 1;
            }

            Array.Resize(ref ArrayOfTasks, t);
            for (int i = 0; i < t; i++)
            {
                ArrayOfTasks[i] = new int[p];
            }
            Array.Resize(ref ArrayOfPopulation, s);
            for (int i = 0; i < s; i++)
            {
                ArrayOfPopulation[i] = new int[t];
            }
            Array.Resize(ref ArrayVitality, s);
        }
        /*FillArrayOfPopulation() - Генерирует случайный номер процессора для задания, 
         при этом проверяет чтобы задание могло выполниться на этом процессоре
         (для этого сгенерированной задание помещается а матрицу заданий и если оно не равно -1, 
         то записывается, как ген особи, иначе цикл повторяется до тех пор пока не особи не сгенерируются)*/
        public void FillArrayOfPopulation() //принимает количество процессоров, размер популяции, кол-во заданий.
        {
            double d = 256.0 / quantityProcessors;
            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < quantityTasks; j++)
                {
                    while (true)
                    {
                        ArrayOfPopulation[i][j] = Rand.Next(0, 255); //представляем гены особи от 0 до 255;
                        if (ArrayOfTasks[j][(int)(Math.Floor(ArrayOfPopulation[i][j] / Math.Floor(d + q)))] != -1) 
                        {
                            break;
                        }
                     }
                }
            }
        }

        /*Формирует двумерную матрицу, где строка - это номер задания, столбец - номер строки, 
         на пересечении в ячейке стоит нагрузка задания, нагрузка формируется случайным образом в 
         заданном диапазоне значений, при этом с некоторой вероятностью расставляются неопределенности(-1), 
         но так чтобы хоть на одном процессоре задание выполнялось.
         */
        public void FillArrayOfTasks()
        {
            int rnd;
            for (int i = 0; i < quantityTasks; i++)
            {
                rnd = Rand.Next(startRangeValues, endRangeValues+1);
                for (int j = 0; j < quantityProcessors; j++)
                {
                    ArrayOfTasks[i][j] = rnd; //диапазон значений матрицы заданий
                    if (Rand.Next(0, 100) < 50) //задаем случайную невыполнимость задачи на процессоре
                    {
                        ArrayOfTasks[i][j] = -1;
                    }
                }
                ArrayOfTasks[i][Rand.Next(0, quantityProcessors)] = rnd; //обязательно, хоть один процессор будет выполнять задачу
            }
        }
        
        
       
        
        
    }
}

//Классы: сортировка, мутация, скрещивание, генетический, метод критического пути.
//создать метод с подстраиваемой ф-ей вычисления процессора.