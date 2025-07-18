using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlotsManager : MonoBehaviour
{
    [Range(1, 20)]public int slotMachineHeight;
    [Range(1, 25)]public int slotMachineWidth;

    [Header("Grid Layout")]
    [SerializeField] private float slotCellSize = 175f;    // Size of each slot
    [SerializeField] private float slotPadding = 0f;       // Space between slots
    [SerializeField] private Vector2 slotOffset = Vector2.zero;
    [Range(0.5f, 2f)]
    [SerializeField] private float gridScale = 1f;

    [Header("UI Variables")]
    [SerializeField] private GameObject slot;
    [SerializeField] private GameObject[] slotIcons;
    [SerializeField] private GameObject slotMachinePanel;
    [SerializeField] private Button playSlotsButton;
    [SerializeField] private Button quitSlotsButton;
    [SerializeField] private Material winLineMaterial;
    [SerializeField] private GameObject winLinePrefab;
    [SerializeField] private TMP_Text winsText;

    [SerializeField] private GameObject slotMachine;
    [SerializeField] private RectTransform slotGrid;
    [SerializeField] private double slotCost = 100.0;

    private GameObject[,] slotMachineGrid;
    private List<GameObject> slotMachineMatchLines;
    private AudioSource slotWinAudio;

    private void Start()
    {
        slotMachineGrid = new GameObject[slotMachineHeight, slotMachineWidth];
        slotMachineMatchLines = new List<GameObject>();
        slotWinAudio = GetComponent<AudioSource>();

        InitializeSlotMachineGrid();

        if (slotMachinePanel != null)
            playSlotsButton.onClick.AddListener(OpenSlots);
        if (quitSlotsButton != null)
            quitSlotsButton.onClick.AddListener(QuitSlots);

    }

    private void InitializeSlotMachineGrid()
    {
        // Calculate real cell and padding sizes after scaling
        float cellSize = slotCellSize * gridScale;
        float cellPadding = slotPadding * gridScale;
        
        // The distance from the center of one cell to the center of the next
        float cellSpacing = cellSize + cellPadding;
        
        // Calculate total width/height based on the number of cells and padding between them
        float totalWidth = (slotMachineWidth * cellSize) + ((slotMachineWidth - 1) * cellPadding);
        float totalHeight = (slotMachineHeight * cellSize) + ((slotMachineHeight - 1) * cellPadding);
        
        // Calculate offset 
        float startX = -(totalWidth / 2) + (cellSize / 2) + slotOffset.x;
        float startY = (totalHeight / 2) - (cellSize / 2) + slotOffset.y;
        
        Debug.Log($"Grid dimensions: {slotMachineWidth}x{slotMachineHeight}, Cell Size: {cellSize}, Padding: {cellPadding}");
        
        for (int i = 0; i < slotMachineHeight; i++)
        {
            for (int j = 0; j < slotMachineWidth; j++)
            {
                GameObject thisSlot = Instantiate(slot, slotMachine.transform);
                thisSlot.name = $"{i} {j}";

                RectTransform rt = thisSlot.GetComponent<RectTransform>();
                if (rt != null)
                {
                    // Set anchor and pivot to center of parent 
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    
                    // Position slot using calculated offsets and spacing
                    rt.anchoredPosition = new Vector2(
                        startX + (j * cellSpacing), 
                        startY - (i * cellSpacing)
                    );
                    
                    // Set visual size 
                    rt.sizeDelta = new Vector2(cellSize, cellSize);
                    rt.localScale = Vector3.one;
                    
                    Debug.Log($"Slot {i},{j} positioned at {rt.anchoredPosition} with size: {rt.sizeDelta}");
                }

                slotMachineGrid[i, j] = thisSlot;
            }
        }
    }


    public void OpenSlots()
    {
        if (slotMachinePanel != null)
            slotMachinePanel.SetActive(true);
    }

    public void QuitSlots()
    {
        if (slotMachinePanel !=null) 
            slotMachinePanel.SetActive(false);
    }

    public void Spin()
    {
        if (GameManager.instance.data.chips < slotCost)
        {
            Debug.Log("Not enoguh chips to spin!");
            return;
        }

        GameManager.instance.data.chips -= slotCost;

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
                if (gridPosition.transform.childCount > 0)
                {
                    GameObject destroyIcon = gridPosition.transform.GetChild(0).gameObject;
                    Destroy(destroyIcon);
                }
                GameObject iconType = slotIcons[Random.Range(0, slotIcons.Length)];
                GameObject thisIcon = Instantiate(iconType);

                thisIcon.transform.SetParent(gridPosition.transform, false);

                RectTransform iconRect = thisIcon.GetComponent<RectTransform>();
                if (iconRect != null)
                {
                    iconRect.anchoredPosition = Vector2.zero;
                    iconRect.localScale = Vector3.one;
                }

                slotMachineGrid[i, j] = thisIcon;
            }
        }

        CheckForWin();

    }

    void CheckForWin()
    {
        bool win = false;
        double currentSpinWinnings = 0;

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
                        currentSpinWinnings += GetLinePayout(winLength);
                        DrawUILineFromSlots(winStart, winEnd);
                        win = true;
                    }
                    winStart = slotMachineGrid[j + 1, i];
                    winLength = 1;
                }
            }
            if (winLength >= 3)
            {
                winEnd = slotMachineGrid[slotMachineHeight - 1, i];
                currentSpinWinnings += GetLinePayout(winLength);
                DrawUILineFromSlots(winStart, winEnd);
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
                        currentSpinWinnings += GetLinePayout(winLength);
                        DrawUILineFromSlots(winStart, winEnd);
                        win = true;
                    }
                    winStart = slotMachineGrid[i, j + 1];
                    winLength = 1;
                }
            }
            if (winLength >= 3)
            {
                winEnd = slotMachineGrid[i, slotMachineWidth - 1];
                currentSpinWinnings += GetLinePayout(winLength);
                DrawUILineFromSlots(winStart, winEnd);
                win = true;
            }
        }

        /* Top-Left to Bottom-Right Win */
        for (int i = 0; i <= slotMachineHeight - 3; i++)
        {
            for (int j = 0; j <= slotMachineWidth - 3; j++)
            {
                string slotName = slotMachineGrid[i, j].name;
                int winLength = 1;

                while (i + winLength < slotMachineHeight &&
                       j + winLength < slotMachineWidth &&
                       slotMachineGrid[i + winLength, j + winLength].name == slotName)
                {
                    winLength++;
                }

                if (winLength >= 3)
                {
                    currentSpinWinnings += GetLinePayout(winLength) * 3;
                    DrawUILineFromSlots(slotMachineGrid[i, j], slotMachineGrid[i + winLength - 1, j + winLength - 1]);
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
                int winLength = 1;

                while (i - winLength >= 0 &&
                       j + winLength < slotMachineWidth &&
                       slotMachineGrid[i - winLength, j + winLength].name == slotName)
                {
                    winLength++;
                }

                if (winLength >= 3)
                {
                    currentSpinWinnings += GetLinePayout(winLength) * 3;
                    DrawUILineFromSlots(slotMachineGrid[i, j], slotMachineGrid[i - winLength + 1, j + winLength - 1]);
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
                DrawUILineFromSlots(iconPoints[0], iconPoints[iconPoints.Count - 1]);
                currentSpinWinnings += slotMachineWidth * 1000;
                win = true;
            }

        }

        if (currentSpinWinnings > 0)
        {
            GameManager.instance.data.chips += currentSpinWinnings;
        }

        if(win && slotWinAudio != null)
            slotWinAudio.Play();

        winsText.text = currentSpinWinnings > 0 ? $"{currentSpinWinnings}" : "Loss";


    }
    private int GetLinePayout(int winLength)
    {
        if (slotMachineHeight <= 1)
            return winLength * 100;
        switch (winLength)
        {
            case 3: return 30;
            case 4: return 60;
            case 5: return 100;
            case 6: return 1000;
            default:
                return 10 * (winLength - 2);
        }
    }

    // Drawas a UI line between two slot objects
    private void DrawUILineFromSlots(GameObject startSlot, GameObject endSlot)
    {
        // Try to use the parent container of each slot if it exists 
        GameObject startContainer = startSlot.transform.parent ? startSlot.transform.parent.gameObject : startSlot;
        GameObject endContainer = endSlot.transform.parent ? endSlot.transform.parent.gameObject : endSlot;

        Debug.Log($"Drawing line from {startContainer.name} to {endContainer.name}");

        // Get world-space positions of both containers
        Vector3 startWorldPos = startContainer.transform.position;
        Vector3 endWorldPos = endContainer.transform.position;
        
        // Used to convert world-space positions to local canvas positions
        Vector2 startCanvasPos, endCanvasPos;
        Canvas canvas = slotGrid.GetComponentInParent<Canvas>();
        Camera canvasCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
        
        // Convert world position -> screen point -> local canvas position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            slotGrid, 
            RectTransformUtility.WorldToScreenPoint(canvasCamera, startWorldPos), 
            canvasCamera, 
            out startCanvasPos);
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            slotGrid, 
            RectTransformUtility.WorldToScreenPoint(canvasCamera, endWorldPos), 
            canvasCamera, 
            out endCanvasPos);
        
        Debug.Log($"Start Canvas Pos: {startCanvasPos}, End Canvas Pos: {endCanvasPos}");
        
        // Measure distance and direction between two points
        float distance = Vector2.Distance(startCanvasPos, endCanvasPos);
        Vector2 direction = (endCanvasPos - startCanvasPos).normalized;
        Vector2 midpoint = startCanvasPos + (endCanvasPos - startCanvasPos) * 0.5f;
        
        GameObject lineGO = Instantiate(winLinePrefab, slotGrid);
        RectTransform rt = lineGO.GetComponent<RectTransform>();
        
        Image lineImage = lineGO.GetComponent<Image>();
        if (lineImage == null)
        {
            lineImage = lineGO.AddComponent<Image>();
            lineImage.color = Color.red; // fallback color
        }
        
        if (winLineMaterial != null)
        {
            lineImage.material = winLineMaterial;
        }
        
        // Set line properties
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(distance, 16); // width = distance between slots, height = line thickness
        rt.anchoredPosition = midpoint;
        
        // Calculate the angle and apply it to the line
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rt.localRotation = Quaternion.Euler(0, 0, angle);
        
        // Makes sure line renders on top
        Canvas.ForceUpdateCanvases();
        rt.SetAsLastSibling();
        
        slotMachineMatchLines.Add(lineGO);
        
        Debug.Log($"Line created with size: {rt.sizeDelta}, position: {rt.anchoredPosition}, angle: {angle}");
    }
}
