using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class SlotsManager : MonoBehaviour
{
    [Range(1, 5)]public int slotMachineHeight;
    [Range(1, 7)]public int slotMachineWidth;



    [SerializeField] private GameObject[] slotIcons;
    [SerializeField] private Material winLineMaterial;
    [SerializeField] private GameObject slot;
    [SerializeField] private TMP_Text winsText;
   

    private GameObject slotMachine;
    private GameObject[,] slotMachineGrid;
    private List<GameObject> slotMachineMatchLines;
    private AudioSource slotWinAudio;
    private Vector3 offset = new Vector3(0, 0, -1);

    private int winTotal = 0;

    private void Start()
    {
        slotMachine = GameObject.Find("SlotMachine");
        slotMachineGrid = new GameObject[slotMachineHeight, slotMachineWidth];
        slotMachineMatchLines = new List<GameObject>();
        slotWinAudio = GetComponent<AudioSource>();

        int heightLimit = slotMachineHeight - 1;
        int widthLimit = slotMachineWidth - 1;
        int heightIndex = 0;
        int widthIndex = 0;
        for (int i = heightLimit; i >= -heightLimit; i -= 2)
        {
            for (int j = -widthLimit; j <= widthLimit; j += 2)
            {
                GameObject thisSlot = Instantiate(slot, new Vector3(j, i, 0), Quaternion.identity);
                thisSlot.name = heightIndex + " " + widthIndex;
                thisSlot.transform.parent = slotMachine.transform;
                widthIndex++;
            }
            heightIndex++;
            widthIndex = 0;
        }
    }

    public void Spin()
    {
        foreach (GameObject line in slotMachineMatchLines)
        {
            GameObject.Destroy(line);
        }
        slotMachineMatchLines.Clear();
        for (int i = 0; i < slotMachineHeight; i++)
        {
            for (int j = 0; j < slotMachineWidth; j++)
            {
                GameObject gridPosition = slotMachine.transform.Find(i + " " + j).gameObject;
                if(gridPosition.transform.childCount > 0)
                {
                    GameObject destroyIcon = gridPosition.transform.GetChild(0).gameObject;
                    Destroy(destroyIcon);
                }
                GameObject iconType = slotIcons[Random.Range(0, slotIcons.Length)];
                GameObject thisIcon = Instantiate(iconType, gridPosition.transform.position + offset, Quaternion.identity);

                thisIcon.name = iconType.name;
                thisIcon.transform.parent = gridPosition.transform;
                slotMachineGrid[i, j] = thisIcon;
            }
        }

        CheckForWin();

    }

    void CheckForWin()
    {
        bool win = false;
        /* Vertical Wins */
        for (int i = 0; i < slotMachineWidth; i++)
        {
            int winLength = 1;
            GameObject winStart = slotMachineGrid[0, i];
            GameObject winEnd = null;
            for (int j = 0; j < slotMachineHeight - 1; j++)
            {
                if (slotMachineGrid[j, i].name == slotMachineGrid[j + 1, i].name)
                {
                    winLength++;
                }
                else
                {
                    if (winLength >= 3)
                    {
                        winEnd = slotMachineGrid[j, i];
                        winTotal += 10 * (winLength - 2);
                        DrawStraightWinLine(winStart.transform.position + offset,
                            winEnd.transform.position + offset);
                        win = true;
                    }
                    winStart = slotMachineGrid[j + 1, i];
                    winLength = 1;
                }
            }
            if (winLength >= 3)
            {
                winEnd = slotMachineGrid[slotMachineHeight - 1, i];
                winTotal += 10 * (winLength - 2);
                DrawStraightWinLine(winStart.transform.position + offset,
                    winEnd.transform.position + offset);
                win = true;
            }
        }

        /* Horizontal Wins */
        for (int i = 0; i < slotMachineHeight; i++)
        {
            int winLength = 1;
            GameObject winStart = slotMachineGrid[i, 0];
            GameObject winEnd = null;
            for (int j = 0; j < slotMachineWidth - 1; j++)
            {
                if (slotMachineGrid[i,j].name == slotMachineGrid[i, j + 1].name)
                {
                    winLength++;
                }
                else
                {
                    if(winLength >= 3)
                    {
                        winEnd = slotMachineGrid[i, j];
                        winTotal += 10 * (winLength - 2);
                        DrawStraightWinLine(winStart.transform.position + offset,
                            winEnd.transform.position + offset);
                        win = true;
                    }
                    winStart = slotMachineGrid[i, j + 1];
                    winLength = 1;
                }
            }
            if (winLength >= 3)
            {
                winEnd = slotMachineGrid[i, slotMachineWidth - 1];
                winTotal += 10 * (winLength - 2);
                DrawStraightWinLine(winStart.transform.position + offset,
                    winEnd.transform.position + offset);
                win = true;
            }
        }

        /* Top-Left to Bottom-Right Win */
        for (int i = 0; i <= slotMachineHeight - 3; i++)
        {
            for (int j = 0; j <= slotMachineWidth - 3; j++)
            {
                string slotName = slotMachineGrid[i, j].name;
                bool diagonalWin = true;
                List<Vector3> slotPoints = new List<Vector3>();

                for (int k = 0; k < 3; k++)
                {
                    if (slotMachineGrid[i + k, j + k].name != slotName)
                    {
                        diagonalWin = false;
                        break;
                    }
                    slotPoints.Add(slotMachineGrid[i + k, j + k].transform.position + offset);
                }

                if (diagonalWin)
                {
                    winTotal += 100;
                    DrawDiagonalWinLine(slotPoints);
                    win = true;
                }
            }
        }

        /* Bottom-Left to Top-Right Win */
        for (int i = 2; i < slotMachineHeight; i++)
        {
            for (int j = 0; j <= slotMachineWidth - 3; j++)
            {
                string slotName = slotMachineGrid[i, j].name;
                bool diagonalWin = true;
                List<Vector3> slotPoints = new List<Vector3>();

                for (int k = 0; k < 3; k++)
                {
                    if (slotMachineGrid[i - k, j + k].name != slotName)
                    {
                        diagonalWin = false;
                        break;
                    }
                    slotPoints.Add(slotMachineGrid[i - k, j + k].transform.position + offset);
                }

                if (diagonalWin)
                {
                    winTotal += 100;
                    DrawDiagonalWinLine(slotPoints);
                    win = true;
                }
            }
        }

        /* X in a row */
        List<GameObject> iconPoints = new List<GameObject>();
        List<string> iconNames = new List<string>();

        for (int i = 0;  i < slotMachineHeight; i++)
        {
            iconPoints.Clear();
            GameObject startPoint = slotMachineGrid[i, 0];
            if (iconNames.Contains(startPoint.name))
              continue;
            else
                iconNames.Add(startPoint.name);
            iconPoints.Add(startPoint);
            for(int j = 1; j < slotMachineWidth; j++)
            {
                bool notFound = false;
                for (int k = 0; k < slotMachineHeight; k++)
                {
                    if (slotMachineGrid[k, j].name == startPoint.name)
                    {
                        iconPoints.Add(slotMachineGrid[k, j]);
                        notFound = true;
                        break;
                    }
                }
                if (notFound)
                    break;
            }
            if (iconPoints.Count == slotMachineWidth)
            {
                List<Vector3> scatteredPoints = new List<Vector3>();
                foreach (var point in iconPoints)
                    scatteredPoints.Add(point.transform.position + offset);
                DrawDiagonalWinLine(scatteredPoints);
                winTotal += slotMachineWidth * 10;
                win = true;
            }

        }

        if(win && slotWinAudio != null)
            slotWinAudio.Play();
        winsText.text = winTotal.ToString();
    }

    private void DrawStraightWinLine(Vector3 start, Vector3 end)
    {
        GameObject thisLine = new GameObject();
        thisLine.transform.position = start;
        thisLine.AddComponent<LineRenderer>();
        LineRenderer lr = thisLine.GetComponent<LineRenderer>();
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.material = winLineMaterial;
        lr.startColor = Color.cyan;
        lr.endColor = Color.cyan;
        slotMachineMatchLines.Add(thisLine);
    }

    private void DrawDiagonalWinLine(List<Vector3> points)
    {
        if (points.Count < 2) return;
        GameObject thisLine = new GameObject();
        thisLine.transform.position = points[0];
        thisLine.AddComponent<LineRenderer>();
        LineRenderer lr = thisLine.GetComponent<LineRenderer>();
        lr.positionCount = points.Count;
        lr.startWidth = 0.2f;
        lr.endWidth = 0.2f;
        for (int i = 0;  i < points.Count; i++) 
            lr.SetPosition(i, points[i]);
        lr.material = winLineMaterial;
        lr.startColor = Color.cyan;
        lr.endColor = Color.cyan;
        slotMachineMatchLines.Add(thisLine);
    }
}
