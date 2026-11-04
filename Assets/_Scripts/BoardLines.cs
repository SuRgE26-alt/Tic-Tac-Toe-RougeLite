using NUnit.Framework;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using System.Collections.Generic;

public static class BoardLines
{
    public static readonly int[][][] SeamLinesByLastBoard = BuildSeamLines();

    public static int ToGlobal(int board, int cell)
    {
        int row = (board / 3) * 3 + (cell / 3);
        int col = (board % 3) * 3 + (cell % 3);
        return row * 9 + col;
    }

    public static int BoardOf(int globalIndex)
    {
        int row = globalIndex / 9;
        int col = globalIndex % 9;
        return (row / 3) * 3 + (col / 3);
    }

    private static int[][][] BuildSeamLines()
    {
        List<int[]>[] buckets = new List<int[]>[9];
        for(int i = 0; i < 9; i++)
        {
            buckets[i] = new List<int[]>();
        }

        int[][] directions = 
        {   
            new[] { 0, 1 }, 
            new[] { 1, 0 }, 
            new[] { 1, 1 }, 
            new[] { 1, -1 } 
        };

        for(int row = 0; row < 9; row++)
        {
            for(int col = 0; col < 9; col++)
            {
                foreach (int[] d in directions)
                {
                    int endRow = row + 2 * d[0];
                    int endCol = col + 2 * d[1];

                    if (endRow > 8 || endCol < 0 || endCol > 8) continue;

                    int a = row * 9 + col;
                    int b = (row + d[0]) * 9 + (col + d[1]);
                    int c = endRow * 9 + endCol;

                    int boardA = BoardOf(a);
                    int boardB = BoardOf(b);
                    int boardC = BoardOf(c);

                    if ((boardA == boardB && boardB == boardC)) continue;
                    
                    int lastBoard = Mathf.Max(boardA, Mathf.Max(boardB, boardC));
                    buckets[lastBoard].Add(new[] { a, b, c });
                }
            }
        }

        int[][][] result = new int[9][][];
        for (int i = 0; i < 9; i++)
        {
            result[i] = buckets[i].ToArray();
        }
        return result;
    }
}
