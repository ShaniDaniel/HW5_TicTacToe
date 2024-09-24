using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TicTacToe : MonoBehaviour
{
    public Button[,] buttons = new Button[3, 3];  
    private string[,] board = new string[3, 3];   
    private bool isXTurn = true;  
    public TextMeshProUGUI statusText;

    [SerializeField]
    public bool playAgainstComputer = false;

    void Start()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                SetButton(row, col, GameObject.Find($"Button{row}_{col}").GetComponent<Button>());
            }
        }

        ResetBoard();
    }

    public void SetButton(int row, int col, Button button)
    {
        buttons[row, col] = button;
        button.onClick.AddListener(() => OnButtonClick(row, col));
    }

    public void OnButtonClick(int row, int col)
    {
        if (board[row, col] == null)  
        {
            board[row, col] = isXTurn ? "X" : "O"; 
            buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = board[row, col];  
            isXTurn = !isXTurn;  
            if (playAgainstComputer is true)
            {
                statusText.text = isXTurn ? "X's turn" : "Computer's turn";
                if (!isXTurn) // if it's O's turn (computer)
                {
                    Invoke("ComputerTurn", 0.5f); // delay for 0.5 second
                }
            }
            else
            {
                statusText.text = isXTurn ? "X's turn" : "O's turn";
            }
            CheckWinner();
        }
    }

    void CheckWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            // Check rows
            if (board[i, 0] != null && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
            {
                EndGame(board[i, 0]);
                return;
            }
            // Check columns
            if (board[0, i] != null && board[0, i] == board[1, i] && board[1, i] == board[2, i])
            {
                EndGame(board[0, i]);
                return;
            }
        }

        // Check diagonals
        if (board[0, 0] != null && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            EndGame(board[0, 0]);
            return;
        }
        if (board[0, 2] != null && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            EndGame(board[0, 2]);
            return;
        }

        // Check for draw
        if (IsBoardFull())
        {
            statusText.text = "It's a Draw!";
        }
    }

    bool IsBoardFull()
    {
        foreach (var cell in board)
        {
            if (cell == null)
            {
                return false;
            }
        }
        return true;
    }

    void EndGame(string winner)
    {
        if (playAgainstComputer is true)
        {
            if (winner == "O")
            {
                statusText.text = "Computer Wins!";
                DisableButtons();
                return;
            }
        }
        statusText.text = winner + " Wins!";
        DisableButtons();
    }

    void DisableButtons()
    {
        foreach (var button in buttons)
        {
            button.interactable = false;
        }
    }

    public void ResetBoard()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                board[row, col] = null;  
                buttons[row, col].GetComponentInChildren<TextMeshProUGUI>().text = ""; 
                buttons[row, col].interactable = true;
            }
        }
        statusText.text = "X's turn";
        isXTurn = true;
    }

    void ComputerTurn()
    {
        List<int[]> availableMoves = new List<int[]>();

        // Collect all available moves
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (board[row, col] == null)
                {
                    int[] move = new int[2];
                    move[0] = row;
                    move[1] = col;
                    availableMoves.Add(move);
                }
            }
        }

        if (availableMoves.Count > 0)
        {
            int[] move = availableMoves[Random.Range(0, availableMoves.Count)];
            OnButtonClick(move[0], move[1]); // simulate button click
        }
    }
}
