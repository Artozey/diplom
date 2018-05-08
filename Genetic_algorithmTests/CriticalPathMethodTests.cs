using Microsoft.VisualStudio.TestTools.UnitTesting;
using Genetic_algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_algorithm.Tests
{
    [TestClass()]
    public class CriticalPathMethodTests
    {
        [TestMethod()]
        public void distributionByProcessorsTest()
        {
            int quantityTasks = 10;
            int quantityProcessors = 4;
            int[][] ArrayOfPopulation = new int[1][];
            for (int i = 0; i < 1; i++)
                ArrayOfPopulation[i] = new int[10];
            int[][] ArrayOfTasks = new int[10][];
            for (int i = 0; i < 10; i++)
                ArrayOfTasks[i] = new int[4];
            ArrayOfTasks[0][0] = -1;
            ArrayOfTasks[0][1] = 29;
            ArrayOfTasks[0][2] = -1;
            ArrayOfTasks[0][3] = 29;
            ArrayOfTasks[1][0] = 29;
            ArrayOfTasks[1][1] = 29;
            ArrayOfTasks[1][2] = 29;
            ArrayOfTasks[1][3] = 29;
            ArrayOfTasks[2][0] = -1;
            ArrayOfTasks[2][1] = 29;
            ArrayOfTasks[2][2] = 29;
            ArrayOfTasks[2][3] = -1;
            ArrayOfTasks[3][0] = 25;
            ArrayOfTasks[3][1] = 25;
            ArrayOfTasks[3][2] = -1;
            ArrayOfTasks[3][3] = 25;
            ArrayOfTasks[4][0] = 25;
            ArrayOfTasks[4][1] = -1;
            ArrayOfTasks[4][2] = 25;
            ArrayOfTasks[4][3] = 25;
            ArrayOfTasks[5][0] = 25;
            ArrayOfTasks[5][1] = -1;
            ArrayOfTasks[5][2] = -1;
            ArrayOfTasks[5][3] = 25;
            ArrayOfTasks[6][0] = -1;
            ArrayOfTasks[6][1] = 30;
            ArrayOfTasks[6][2] = 30;
            ArrayOfTasks[6][3] = 30;
            ArrayOfTasks[7][0] = -1;
            ArrayOfTasks[7][1] = 25;
            ArrayOfTasks[7][2] = 25;
            ArrayOfTasks[7][3] = -1;
            ArrayOfTasks[8][0] = -1;
            ArrayOfTasks[8][1] = 26;
            ArrayOfTasks[8][2] = 26;
            ArrayOfTasks[8][3] = 26;
            ArrayOfTasks[9][0] = 29;
            ArrayOfTasks[9][1] = -1;
            ArrayOfTasks[9][2] = -1;
            ArrayOfTasks[9][3] = 29;

            CriticalPathMethod c = new CriticalPathMethod();
            int res = c.distributionByProcessors(false, quantityTasks, quantityProcessors, ref ArrayOfTasks, ref ArrayOfPopulation, 0);

            Assert.AreEqual(res, 83);
        }
    }
}