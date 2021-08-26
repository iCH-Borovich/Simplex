using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Simplex
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            List<float> constrain = new List<float>();
            List<List<float>> adjacency = new List<List<float>>();
            List<float> tempList = new List<float>();
            List<float> nonnegativeConstraints = new List<float>();
            List<int[]> pointHistory = new List<int[]>();
            string userInput;
            string inputName = "";
            string outputName = "";

            bool inputOn = true;
            Console.Write(">>script ");
            while (inputOn)
            { 
                userInput = Console.ReadLine();
                foreach (var s in userInput.Split(' '))
                {
                    if (inputName == "")
                        inputName = s;
                    else if (outputName == "")
                        outputName = s;
                    else
                        Fail(1);
                }
                inputOn = false;
            }

            if (!inputName.Contains(".txt"))
                inputName += ".txt";
            if(!outputName.Contains(".txt"))
                outputName += ".txt";

            if (!File.Exists(inputName))
            {
                Fail(1);
            }
            
            string[] input = File.ReadAllLines(inputName);
            Console.Clear();
            
            bool isSolved = false;
            float num = 0;
            
            foreach (var s in input[0].Split('\t'))
            {
                if(float.TryParse(s,out num))
                    constrain.Add(Convert.ToSingle(num));
                else
                    Fail(1);
            }

            for (int i = 2; i < input.Length-2; i++)
            {
                foreach (var s in input[i].Split('\t'))
                {
                    if(float.TryParse(s,out num))
                        tempList.Add(Convert.ToSingle(num));
                    else
                        Fail(1);
                }
                adjacency.Add(new List<float>(tempList));
                tempList.Clear();
            }

            foreach (var s in input[input.Length-1].Split('\t'))
            {
                if(float.TryParse(s,out num))
                    nonnegativeConstraints.Add(Convert.ToSingle(num));
                else
                    Fail(1);
            }

            if (constrain.Count != adjacency[0].Count || nonnegativeConstraints.Count != adjacency.Count)
            {
                Fail(1);
            }

            if (AllPositive(constrain))
            {
                for (int i = 0; i < constrain.Count; i++)
                {
                    constrain[i] *= -1;
                }
            }
            else
            {
                Fail(2);
            }
            
            for (int i = 0; i < nonnegativeConstraints.Count; i++)
            {
                if(nonnegativeConstraints[i]<0)
                    Fail(2);
            }
            
            for (int i = 0; i < nonnegativeConstraints.Count; i++)
            {
                List<float> zeroList = new List<float>(nonnegativeConstraints.Count);
                
                for (int s = 0; s < nonnegativeConstraints.Count; s++)
                {
                    zeroList.Add(0);
                }

                zeroList[i] = 1;

                for (int j = 0; j < nonnegativeConstraints.Count; j++)
                {
                    adjacency[i].Add(zeroList[j]);
                }
            }

            for (int i = 0; i < nonnegativeConstraints.Count; i++)
            {
                List<float> zeroList = new List<float>(nonnegativeConstraints.Count);
                
                for (int s = 0; s < nonnegativeConstraints.Count; s++)
                {
                    zeroList.Add(0);
                }
            }
            for (int j = 0; j < nonnegativeConstraints.Count; j++)
            {
                List<float> zeroList = new List<float>(nonnegativeConstraints.Count);
                
                for (int s = 0; s < nonnegativeConstraints.Count; s++)
                {
                    zeroList.Add(0);
                }
                constrain.Add(zeroList[j]);
            }
            
            nonnegativeConstraints.Add(0);
            
            while (!isSolved)
            {
                DrawMatrix(nonnegativeConstraints,adjacency,constrain);
                NotSoSimpleCalculations(nonnegativeConstraints, adjacency, constrain);
            }

            if (isSolved)
            {
                string answer;
                if (MultipleSolution(Answers(nonnegativeConstraints, adjacency, constrain), constrain))
                {
                    answer = "SOLUTION FOUND: multiple solutions\r\nObjective: z (optimal value)\r\n";
                }
                else
                {
                    answer = "SOLUTION FOUND: unique solution\r\nObjective: z (optimal value)\r\n";
                }
                Console.WriteLine("Solved!\n"+outputName+" file is created in program's represetory");
                
                foreach (var s in Answers(nonnegativeConstraints, adjacency, constrain))
                {
                    answer += s + "\t";
                }
                File.WriteAllText(outputName, answer);
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            
            void Fail(int errorID)
            {
                switch (errorID)
                {
                    case 1:
                        File.WriteAllText(outputName, "ERROR: incorrect input data");
                        break;
                    case 2:
                        File.WriteAllText(outputName, "ERROR: incorrect problem statement");
                        break;
                    case 3:
                        File.WriteAllText(outputName, "NO SOLUTION: unbounded problem");
                        break;
                    default:
                        File.WriteAllText(outputName, "Oshibochka");
                        break;
                }
                Console.WriteLine("There are occurred an error, more details in "+outputName);
                System.Environment.Exit(1);
            }
            
            void NotSoSimpleCalculations(List<float> Z, List<List<float>> A, List<float> C)
            {
                if (C.Min() >= 0)
                {
                    isSolved = true;
                    return;
                }
                
                int indexConstrain = C.IndexOf(C.Min());
                List<float> dividedConstrain = new List<float>();
                
                
                for (int j = 0; j < Z.Count-1; j++)
                {
                    if (Z[j] / A[j][indexConstrain] >= 0)
                    {
                        dividedConstrain.Add(Z[j] / A[j][indexConstrain]);
                    }
                    else
                    {
                        dividedConstrain.Add(-1);
                    }
                }

                if (MinPositive(dividedConstrain) == float.MaxValue)
                {
                    Fail(3);
                }
                int indexNonConstrain = dividedConstrain.IndexOf(MinPositive(dividedConstrain));
                
                Console.WriteLine(indexNonConstrain+" "+indexConstrain);
                int[] points = new int[] {indexNonConstrain,indexConstrain};
                pointHistory.Add(points);
                
                float point = A[indexNonConstrain][indexConstrain];
                float firstPoint = point;
                    
                for (int j = 0; j < A[0].Count; j++)
                {
                    A[indexNonConstrain][j] *= (1 / point);
                }
                Z[indexNonConstrain] *= 1 / point;
                for (int i = 0; i < Z.Count-1; i++)
                {
                    point = A[i][indexConstrain];
                    if (i == indexNonConstrain)
                    {
                        continue;
                    }
                    for (int j = 0; j < A[0].Count; j++)
                    {
                        A[i][j] += -1*point*A[indexNonConstrain][j];
                    }
                    Z[i] += -1*point*Z[indexNonConstrain];
                }
                point = C[indexConstrain];
                for (int j = 0; j < A[0].Count; j++)
                {
                    C[j] += -1*point*A[indexNonConstrain][j];
                }
                Z[Z.Count-1] += -1*point*Z[indexNonConstrain];
            }
            
            void DrawMatrix(List<float> Z, List<List<float>> A, List<float> C)
            {
                for (int i = 0; i < A.Count; i++)
                {
                    for (int j = 0; j < A[0].Count; j++)
                    {
                        Console.Write(A[i][j] + "\t");
                    }
                    Console.WriteLine(Z[i]);
                }
    
                for (int i = 0; i < A[0].Count; i++)
                {
                    Console.Write(C[i]+"\t");
                }
                Console.WriteLine(Z[Z.Count-1]);
                Console.WriteLine("____________________________________________________");
            }

            float[] Answers(List<float> Z, List<List<float>> A, List<float> C)
            {
                float[] localList = new float[C.Count];
                for (int i = 0; i < C.Count; i++)
                {
                    localList[i] = 0;
                }
                for (int i = A.Count-1; i >= 0; i--)
                {
                    for (int j = A[0].Count-1; j >= 0; j--)
                    {
                        if (A[i][j] == 1 && j>(A[0].Count/2)-1)
                        {
                            localList[j] = Z[i];
                        }
                        else if(A[i][j] == 1 && isPrimary(i , j))
                        {
                            localList[j] = Z[i];
                            break;
                        }
                    }
                }
                return localList;
            }

            bool AllPositive(List<float> list)
            {
                float localSum = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    if (localSum + list[i] < localSum)
                        return false;
                }
                return true;
            }

            bool MultipleSolution(float[] localList, List<float> C)
            {
                for (int i = 0; i < (C.Count / 2) - 1; i++)
                {
                    if (C[i] == 0 && localList[i] == 0)
                        return true;
                }

                return false;
            }
            
            float MinPositive(List<float> A)
            {
                float localMin = float.MaxValue;
                for (int i = 0; i < A.Count; i++)
                {
                    if (A[i] <= localMin && A[i] >= 0)
                    {
                        localMin = A[i];
                    }
                }
                return localMin;
            }

            bool isPrimary(int a, int b)
            {
                int[] localArray = new int[] {a,b};
                for (int i = 0; i < pointHistory.Count; i++)
                {
                    if (pointHistory[i].SequenceEqual(localArray))
                        return true;
                }
                return false;
            }
        }
    }
    //
}