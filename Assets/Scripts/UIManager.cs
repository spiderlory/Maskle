using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private int row;
    [SerializeField] private int columns;

    [SerializeField] private RectTransform panel;
    [SerializeField] private GameObject uiElement;

    public int min;
    public int max;
    public int test;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GridLayoutGroup gridLayoutGroup = panel.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraintCount = row;
        
        GameObject newElement;
        
        for (int iCol = 0; iCol < columns; iCol++)
        {
            for (int jRow = 0; jRow < row; jRow++)
            {
                int iIndex = iCol;
                int jIndex = jRow;
                
                newElement = Instantiate(uiElement, panel.transform);
                newElement.GetComponent<Button>().onClick.AddListener(() => TestPrint(iIndex, jIndex));
            }
        }

        Mask m = new Mask(2);
        m.GenerateMask(min, max, test);
        m.PrintMask();
    }

    void TestPrint(int i, int j)
    {
        print("i:  " + i + ", j: " + j);
    }
}
