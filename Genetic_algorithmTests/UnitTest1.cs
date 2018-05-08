using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Genetic_algorithm;

namespace Genetic_algorithmTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMyt()
        {
            //arrange
            bool output = false;
            int[] arr = { 6, 135, 137, 17, 210, 93, 109, 187, 194, 116 }; //137 = 1000 1001
            int[] resArr = { 6, 135, 129, 17, 210, 93, 109, 187, 194, 116 }; //129 = 1000 0001
            int mytGen = 2;
            int gen = 4;// бит
            int quantityProcessors = 3;
            int[][] ArrayOfTasks = new int[10][];
            for(int i=0; i<10; i++)
            ArrayOfTasks[i] = new int[4];
            ArrayOfTasks[0][0] = 26;
            ArrayOfTasks[0][1] = 26;
            ArrayOfTasks[0][2] = 26;
            ArrayOfTasks[0][3] = -1;
            ArrayOfTasks[1][0] = 30;
            ArrayOfTasks[1][1] = 30;
            ArrayOfTasks[1][2] = 30;
            ArrayOfTasks[1][3] = -1;
            ArrayOfTasks[2][0] = 25;
            ArrayOfTasks[2][1] = 25;
            ArrayOfTasks[2][2] = 25;
            ArrayOfTasks[2][3] = 25;
            ArrayOfTasks[3][0] = 25;
            ArrayOfTasks[3][1] = -1;
            ArrayOfTasks[3][2] = -1;
            ArrayOfTasks[3][3] = -1;
            ArrayOfTasks[4][0] = -1;
            ArrayOfTasks[4][1] = -1;
            ArrayOfTasks[4][2] = -1;
            ArrayOfTasks[4][3] = 25;
            ArrayOfTasks[5][0] = -1;
            ArrayOfTasks[5][1] = 28;
            ArrayOfTasks[5][2] = 28;
            ArrayOfTasks[5][3] = 28;
            ArrayOfTasks[6][0] = -1;
            ArrayOfTasks[6][1] = 29;
            ArrayOfTasks[6][2] = 29;
            ArrayOfTasks[6][3] = -1;
            ArrayOfTasks[7][0] = -1;
            ArrayOfTasks[7][1] = 27;
            ArrayOfTasks[7][2] = 27;
            ArrayOfTasks[7][3] = 27;
            ArrayOfTasks[8][0] = 26;
            ArrayOfTasks[8][1] = 26;
            ArrayOfTasks[8][2] = 26;
            ArrayOfTasks[8][3] = 26;
            ArrayOfTasks[9][0] = 28;
            ArrayOfTasks[9][1] = 28;
            ArrayOfTasks[9][2] = -1;
            ArrayOfTasks[9][3] = 28;
            //act 
            MutationClass m = new MutationClass();
            m.Mut(output, ref arr,  mytGen, gen, quantityProcessors, ref ArrayOfTasks);
            //assert
            
            Assert.AreEqual(arr[2], resArr[2]);
        } 
    }
}
