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
    public class GoldbergModelTests
    {
        [TestMethod()]
        public void vitalityForIndividTest()
        {
            int[] ArrayIndivid = new int[] {65, 206, 128, 52, 7, 49, 175, 109, 172, 253 }; //85
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

            int quantityProcessors = 4;
            int quantityTasks = 10;
            GoldbergModel g = new GoldbergModel();
            int res = g.vitalityForIndivid(ref ArrayIndivid, quantityProcessors, quantityTasks, ref ArrayOfTasks);
            Assert.AreEqual(res, 85);
        }

        [TestMethod()]
        public void CrossTest()
        {
            
            int[] child1 = new int[10] { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' };
            int[] child2 = new int[10] { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' };
            int parent1 = 0;
            int parent2 = 1;
            int quantityTasks = 10;

            int[][] arrParent = new int[2][];
            for (int i = 0; i < 2; i++)
                arrParent[i] = new int[10];
            arrParent[0][0] = 122; arrParent[1][0] = 59;
            arrParent[0][1] = 36; arrParent[1][1] = 215;
            arrParent[0][2] = 23; arrParent[1][2] = 111;
            arrParent[0][3] = 250; arrParent[1][3] = 1;
            arrParent[0][4] = 78; arrParent[1][4] = 199;
            arrParent[0][5] = 66; arrParent[1][5] = 145;
            arrParent[0][6] = 230; arrParent[1][6] = 144;
            arrParent[0][7] = 144; arrParent[1][7] = 165;
            arrParent[0][8] = 156; arrParent[1][8] = 32;
            arrParent[0][9] = 133; arrParent[1][9] = 99;


            CrossoverClass c = new CrossoverClass();
            c.Crossover(false, quantityTasks, ref child1, ref child2, ref arrParent, parent1, parent2);

            Assert.AreEqual(arrParent[0][0], child1[0]);
            Assert.AreEqual(arrParent[0][9], child2[9]);
            Assert.AreEqual(arrParent[1][0], child2[0]);
            Assert.AreEqual(arrParent[1][9], child1[9]);
        }
    }
}