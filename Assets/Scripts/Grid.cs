﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// </summary>
public class Grid : MonoBehaviour
{
    private int row, column;

    public GameObject neo;
    public GameObject trinity;
    public GameObject morph;
    public GameObject niobe;
    public GameObject smith;
    public GameObject audio;


    public int[,] currentGrid;
    public int[,] puzzle;
    public static GridSquare currentSquare = null;

    private int mouseType = -1;
    private int mouseFlag = 0;
    private int element;

    public bool playing = false;

    private GridSquare[,] squares;
    public Transform gridTransform;
    public GameObject squarePrefab;

    //public GameObject rowTextPrefab, columnTextPrefab;
    //private Text[,] rowText, columnText;
    public GameObject textPrefab;
    private List<Text> rowText, columnText;

    [HideInInspector]
    public int squareSize = 100;
    [HideInInspector]
    public int fontSize = 50;

    public GameObject pausePanel, winPanel;
    public Text timeText;
    public Text time2;

    private float time = 0;

    public GameObject winss;


    public void LoadPuzzle(int r, int c, int[,] puzzle , int element )
    {
        this.row = r;
        this.column = c;
        this.puzzle = puzzle;
        this.element = element;

        Debug.Log(row + ", " + column);

        //gridTransform .GetComponent<RectTransform >().sizeDelta = 

        squareSize = (int)(1000 / r / 1.5f);
        fontSize = squareSize / 2;
        if (fontSize < 25) fontSize = 25;

        currentGrid = new int[column, row];



        squares = new GridSquare[column, row];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                GameObject obj = Instantiate(squarePrefab, gridTransform );
                obj.name = "(" + i + ", " + j + ")";

                GridSquare square = obj.GetComponent<GridSquare>();
                square.SetPosition(i, j);
                Vector2 pos = new Vector2(squareSize / 2 + squareSize * j, (row - i) * squareSize-squareSize/2);
                square.GetComponent<RectTransform>().anchoredPosition = pos ;
                //square.GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize*0.9f, squareSize*0.9f);
                squares[i, j] = square;

                SetSquare(square, 0);
                square.SetSize(squareSize);
                //square.image.color = (puzzle[i, j] == 0) ? Color.white : Color.black;

            }
        }

        
        columnText = new List<Text>();
        for(int i = 0;i<column;i++)
        {
            List<int> result = ReadPuzzle(this.puzzle, true, i);
            for(int a = 0; a < result.Count; a++)
            {
                GameObject obj = Instantiate(textPrefab, gridTransform);
                
                obj.GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize, squareSize);

                
                Vector2 pos = new Vector2(i * squareSize + squareSize / 2, (row) * squareSize + (result.Count -a)*squareSize - squareSize /2 );
                obj.GetComponent<RectTransform>().anchoredPosition = pos ;

                
                Text t = obj.GetComponentInChildren<Text>();
                t.text = result[a]+"";
                t.fontSize = this.fontSize;

                columnText.Add(t);
            }
        }
        
        rowText = new List<Text>();
        for (int i = 0; i < row; i++)
        {
            List<int> result = ReadPuzzle(this.puzzle, false, i);
            for (int a = 0; a < result.Count; a++)
            {
                GameObject obj = Instantiate(textPrefab, gridTransform);
                
                obj.GetComponent<RectTransform>().sizeDelta = new Vector2(squareSize, squareSize);

                
                Vector2 pos = new Vector2(-(result.Count -a)*squareSize +squareSize /2, (row-i)*squareSize-squareSize/2);
                obj.GetComponent<RectTransform>().anchoredPosition = pos ;

                
                Text t = obj.GetComponentInChildren<Text>();
                t.text = result[a] + "";
                t.fontSize = this.fontSize;

                rowText.Add(t);
            }
        }

        gridTransform.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-squareSize * row / 2, -350);

    }

    void Update()
    {
        if (playing)
        {
            time += Time.deltaTime;
            int hour = (int)time / 3600;
            int minute = (int)(time - hour * 3600) / 60;
            int second = (int)(time - hour * 3600 - minute * 60);
            timeText.text = string.Format("{0:D2}:{1:D2}", minute, second);
            time2.text = string.Format("{0:D2}:{1:D2}", minute, second);

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(gridTransform as RectTransform,Input.mousePosition, null, out pos);
            //Debug.Log(pos);

            float limit = squareSize * row ;

            if (pos.x < 0 || pos.x >  limit|| pos.y < 0 || pos.y > limit) currentSquare = null;
            else
            {
                Vector2Int arr = new Vector2Int((int)(pos.x / squareSize), (int)(row - pos.y / squareSize));
                //Debug.Log(arr);
                currentSquare = squares[arr.y,arr.x];
            }


            if (Input.GetMouseButtonUp(0)|| Input.GetMouseButtonUp(1)|| Input.GetMouseButtonUp(2))
            {
                mouseType = -1;
                mouseFlag = 0;
            }

            
            if (Input.GetMouseButtonDown(0))
            {
                mouseType = 0;
                if (currentSquare == null)
                {
                    mouseFlag = 0;
                }
                else if (currentSquare.state == 0 || currentSquare.state == 3)
                {
                    mouseFlag = 1;
                    SetSquare(currentSquare, 1);
                }
                else
                {
                    SetSquare(currentSquare, 0);
                    mouseFlag = -1;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("button down 1");

                mouseType = 1;
                if (currentSquare == null)
                {
                    mouseFlag = 0;
                }
                else if (currentSquare.state == 0 || currentSquare.state == 3)
                {
                    SetSquare(currentSquare, 2);
                    mouseFlag = 1;
                }
                else
                {
                    SetSquare(currentSquare, 0);
                    mouseFlag = -1;
                }
            }
            else if (Input.GetMouseButtonDown(2))
            {
                mouseType = 2;
                if (currentSquare == null)
                {
                    mouseFlag = 0;
                }
                else if (currentSquare.state == 0)
                {
                    mouseFlag = 1;
                    SetSquare(currentSquare, 3);
                }
                else
                {
                    mouseFlag = -1;
                    SetSquare(currentSquare,0);
                }
            }


            if (currentSquare != null)
            {
                
                
                if (Input.GetMouseButton(0))
                {
                    if(mouseType == 0)
                    {
                        if((currentSquare.state == 0 || currentSquare.state == 3 )&& mouseFlag == 1)
                        {
                            SetSquare(currentSquare, 1);
                        }
                        else if(currentSquare.state == 1 && mouseFlag == -1)
                        {
                            SetSquare(currentSquare, 0);
                        }
                        else if(mouseFlag == 0)
                        {
                            if(currentSquare.state == 0 || currentSquare.state == 3)
                            {
                                mouseFlag = 1;
                                SetSquare(currentSquare, 1);
                            }else if(currentSquare.state == 1)
                            {
                                mouseFlag = -1;
                                SetSquare(currentSquare, 0);
                            }
                        }
                    }
                   
                }
                
                else if (Input.GetMouseButton(1))
                {
                    if (mouseType == 1)
                    {
                        if ((currentSquare.state == 0 || currentSquare.state == 3) && mouseFlag == 1)
                        {
                            SetSquare(currentSquare, 2);
                        }
                        else if (currentSquare.state == 2 && mouseFlag == -1)
                        {
                            SetSquare(currentSquare, 0);
                        }
                        else if (mouseFlag == 0)
                        {
                            if (currentSquare.state == 0 || currentSquare.state == 3)
                            {
                                mouseFlag = 2;
                                SetSquare(currentSquare, 2);
                            }
                            else if (currentSquare.state == 2)
                            {
                                mouseFlag = -1;
                                SetSquare(currentSquare, 0);
                            }
                        }
                    }
                }
                
                else if (Input.GetMouseButton(2))
                {
                    if (mouseType == 2)
                    {
                        if ((currentSquare.state == 0) && mouseFlag == 1)
                        {
                            SetSquare(currentSquare, 3);
                        }
                        else if (currentSquare.state == 3 && mouseFlag == -1)
                        {
                            SetSquare(currentSquare, 0);
                        }
                        else if (mouseFlag == 0)
                        {
                            if (currentSquare.state == 0 )
                            {
                                mouseFlag = 1;
                                SetSquare(currentSquare, 3);
                            }
                            else if (currentSquare.state == 3)
                            {
                                mouseFlag = -1;
                                SetSquare(currentSquare, 0);
                            }
                        }
                    }
                }

            }
        }
    }

    private void SetSquare(GridSquare square, int state)
    {
        //Debug.Log(square.name + " set as " + state);
        square.SetState(state);
        currentGrid[square.x, square.y] = state;

        if (CheckWin())
        {
            playing = false;
            winss.SetActive(true);
            switch (element)
            {
                case 0: neo.SetActive(true);
                    break;
                case 1:
                    trinity.SetActive(true);
                    break;
                case 2:
                    morph.SetActive(true);
                    break;
                case 4:
                    niobe.SetActive(true);
                    break;
                case 5:
                    smith.SetActive(true);
                    break;
            }

            StartCoroutine(winning(2f));
        }
    }

    IEnumerator winning (float time)
    {
        yield return new WaitForSeconds(time);

        winss.SetActive(false);
        winPanel.SetActive(true);
        neo.SetActive(false);
        trinity.SetActive(false);
        morph.SetActive(false);
        niobe.SetActive(false);
        smith.SetActive(false);
    }

    private bool CheckWin()
    {
        /*for(int i = 0; i < column; i++)
        {
            for(int j = 0; j < row; j++)
            {
                if (puzzle[i, j] == 1 && currentGrid[i, j] != 1) return false;
                if (puzzle[i, j] == 0 && currentGrid[i, j] == 1) return false;
            }
        }*/
        for(int i = 0; i < row; i++)
        {
            if (!IntListCompare(ReadPuzzle(puzzle, false, i), ReadPuzzle(currentGrid, false, i))) return false;
            //if (ReadPuzzle(puzzle, false, i).Equals(ReadPuzzle(currentGrid, false, i))) return false;
        }
        for (int i = 0; i < column; i++)
        {
            if (!IntListCompare(ReadPuzzle(puzzle, true, i), ReadPuzzle(currentGrid, true, i))) return false;

        }
        return true;
    }

    public static bool IntListCompare(List<int> a, List<int> b)
    {
        if (a == null || b == null) return false;
        if (a.Count != b.Count) return false;
        for(int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

    public void Win()
    {
        playing = false;
        
        winPanel.SetActive(true);
        pausePanel.SetActive(false);
    }

    public void Pause()
    {
        playing = false;
        pausePanel.SetActive(true);
        audio.SetActive(false);
    }

    public void Resume()
    {
        playing = true;
        pausePanel.SetActive(false);
        audio.SetActive(true);
    }

    public void Restart()
    {
        time = 0;


        for(int i = 0; i < column; i++)
        {
            for(int j = 0; j < row; j++)
            {
                SetSquare(squares[i, j], 0);
            }
        }

        playing = true;
        pausePanel.SetActive(false);
        winPanel.SetActive(false);

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenuB()
    {
        SceneManager.LoadScene("menu2");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="puzzle"></param>
    /// <param name="readColumn">true:一列，false:一行</param>
    /// <param name="index"></param>
    /// <returns></returns>
    public List<int> ReadPuzzle(int[,] puzzle, bool readColumn, int index)
    {
        List<int> result = new List<int>();

        if(readColumn)
        {
            bool flag = false;
            int count = 0;
            for(int r = 0; r < row; r++)
            {
                if (flag)
                {
                    if(puzzle[r,index]==1)
                    {
                        count++;
                    }
                    else
                    {
                        result.Add(count);
                        flag = false;
                        count = 0;
                    }
                }
                else
                {
                    if(puzzle[r,index] == 1)
                    {
                        count = 1;
                        flag = true;
                    }
                }
            }
            if(flag == true)
            {
                result.Add(count);
            }
        }
        else
        {
            bool flag = false;
            int count = 0;
            for (int c = 0; c < column; c++)
            {
                if (flag)
                {
                    if (puzzle[index,c] == 1)
                    {
                        count++;
                    }
                    else
                    {
                        result.Add(count);
                        flag = false;
                        count = 0;
                    }
                }
                else
                {
                    if (puzzle[index,c] == 1)
                    {
                        count = 1;
                        flag = true;
                    }
                }
            }
            if (flag == true)
            {
                result.Add(count);
            }
        }


        if (result.Count == 0) result.Add(0);
        return result;
    }

}
