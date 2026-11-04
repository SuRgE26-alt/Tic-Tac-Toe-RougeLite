using UnityEngine;

public class SeamLinesTest : MonoBehaviour
{
    void Start()
    {
        bool allPassed = true;

        // 1. ToGlobal: your example (board 1 cells 4 and 5, board 2 cell 3).
        allPassed &= Check("ToGlobal(0, 4) == 10", BoardLines.ToGlobal(0, 4) == 10);
        allPassed &= Check("ToGlobal(0, 5) == 11", BoardLines.ToGlobal(0, 5) == 11);
        allPassed &= Check("ToGlobal(1, 3) == 12", BoardLines.ToGlobal(1, 3) == 12);

        // 2. BoardOf is the reverse mapping.
        allPassed &= Check("BoardOf(12) == 1", BoardLines.BoardOf(12) == 1);
        allPassed &= Check("BoardOf(80) == 8", BoardLines.BoardOf(80) == 8);

        // 3. Counts per bucket and in total (must be 152).
        int total = 0;
        for (int b = 0; b < 9; b++)
        {
            int count = BoardLines.SeamLinesByLastBoard[b].Length;
            total += count;
            Debug.Log($"Seam lines completing on board {b + 1}: {count}");
        }
        allPassed &= Check("Total seam lines == 152", total == 152);
        allPassed &= Check("Board 1 bucket is empty", BoardLines.SeamLinesByLastBoard[0].Length == 0);

        // 4. The example line {10, 11, 12} must sit in board 2's bucket (index 1).
        allPassed &= Check("Bucket for board 2 contains {10, 11, 12}", ContainsLine(BoardLines.SeamLinesByLastBoard[1], 10, 11, 12));

        // 5. Every line in a bucket really ends on that board and really crosses a seam.
        bool bucketsValid = true;
        for (int b = 0; b < 9; b++)
        {
            foreach (int[] line in BoardLines.SeamLinesByLastBoard[b])
            {
                int boardA = BoardLines.BoardOf(line[0]);
                int boardB = BoardLines.BoardOf(line[1]);
                int boardC = BoardLines.BoardOf(line[2]);
                int highest = Mathf.Max(boardA, Mathf.Max(boardB, boardC));
                bool crossesSeam = !(boardA == boardB && boardB == boardC);

                if (highest != b || !crossesSeam) bucketsValid = false;
            }
        }
        allPassed &= Check("Every line is in the right bucket and crosses a seam", bucketsValid);

        Debug.Log(allPassed ? "SeamLinesTest: ALL PASSED" : "SeamLinesTest: SOMETHING FAILED, see errors above");
    }

    static bool ContainsLine(int[][] lines, int a, int b, int c)
    {
        foreach (int[] line in lines)
        {
            if (line[0] == a && line[1] == b && line[2] == c) return true;
        }
        return false;
    }

    static bool Check(string name, bool passed)
    {
        if (passed) Debug.Log($"PASS: {name}");
        else Debug.LogError($"FAIL: {name}");
        return passed;
    }

}
