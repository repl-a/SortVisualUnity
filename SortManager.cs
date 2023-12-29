using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SortManager : MonoBehaviour
{
    public GameObject elementPrefab;
    public GameObject[] elementObjects;
    private int[] array;
    public TMP_Dropdown algorithmDropdown; 
    public Slider arraySizeSlider; 
    public Slider speedSlider; 
    public Button newArrayButton;
    private int arraySize;
    private int sortingSpeed;
    bool isSorting = false;


    void Start()
    {
        arraySize = Mathf.RoundToInt(arraySizeSlider.value);
        sortingSpeed = Mathf.RoundToInt(speedSlider.value);
        InitializeArray();
        CreateVisualElements();
    }

    public void InitializeArray()
    {
        isSorting = false;
        StopAllCoroutines();
        if (elementObjects != null)
        {
            foreach (var obj in elementObjects)
            {
                if (obj != null) Destroy(obj);
            }
        }

        arraySize = Mathf.RoundToInt(arraySizeSlider.value);
        array = new int[arraySize];
        elementObjects = new GameObject[arraySize];

        for (int i = 0; i < arraySize; i++)
        {
            elementObjects[i] = elementPrefab;
        }

        for (int i = 0; i < arraySize; i++)
        {
            array[i] = i + 1;
        }
        ShuffleArray(array);
        CreateVisualElements();
    }

    void ShuffleArray(int[] arr)
    {
        System.Random rand = new System.Random();
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            SwapElements(i, j, false);
        }
    }

    void CreateVisualElements()
    {
        float startX = -arraySize / 2f;
        for (int i = 0; i < arraySize; i++)
        {
            GameObject element = Instantiate(elementPrefab, new Vector3(startX + i, 0, 0), Quaternion.identity);
            element.transform.localScale = new Vector3(1, array[i], 1);
            element.GetComponent<Renderer>().material.color = GetColor(i, arraySize);
            elementObjects[i] = element;

        }
    }

    Color GetColor(int index, int total)
    {
        return Color.HSVToRGB((float)index / total, 1, 1);
    }

    void SwapElements(int index1, int index2, bool swapColors)
    {
        int temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;

        Vector3 tempScale = elementObjects[index1].transform.localScale;
        elementObjects[index1].transform.localScale = elementObjects[index2].transform.localScale;
        elementObjects[index2].transform.localScale = tempScale;

        if (swapColors)
        {
            Color tempColor = elementObjects[index1].GetComponent<Renderer>().material.color;
            elementObjects[index1].GetComponent<Renderer>().material.color = elementObjects[index2].GetComponent<Renderer>().material.color;
            elementObjects[index2].GetComponent<Renderer>().material.color = tempColor;
        }
    }

    void HighlightElement(int index, bool highlight)
    {
        if (index < 0 || index >= arraySize) return;
        var renderer = elementObjects[index].GetComponent<Renderer>();
        renderer.material.color = highlight ? Color.black : GetColor(index, arraySize);
    }

    void DehighlightAllElements()
    {
        for (int i = 0; i < arraySize; i++)
        {
            HighlightElement(i, false);
        }
    }

    public void StartSorting()
    {
        if (!isSorting && elementObjects.Length != 0) {
            
            sortingSpeed = Mathf.RoundToInt(speedSlider.value);
            string selectedAlgorithm = algorithmDropdown.options[algorithmDropdown.value].text;
            print(selectedAlgorithm);
            switch (selectedAlgorithm)
            {
                case "Merge":
                    StartCoroutine(MergeSort(0, array.Length - 1));
                    break;
                case "Quick":
                    StartCoroutine(QuickSort(0, array.Length - 1));
                    break;
                case "Quick3":
                    StartCoroutine(Quick3Sort(0, array.Length - 1));
                    break;
                case "Heap":
                    StartCoroutine(HeapSort());
                    break;
                case "Shell":
                    StartCoroutine(ShellSort());
                    break;
                case "Bubble":
                    StartCoroutine(BubbleSort());
                    break;
                case "Selection":
                    StartCoroutine(SelectionSort());
                    break;
                case "Insertion":
                    StartCoroutine(InsertionSort());
                    break;
            }
        }
        isSorting = true;
    }
    //------------------------------------------- BUBBLE SORT
    IEnumerator BubbleSort()
    {
        for (int i = 0; i < arraySize - 1; i++)
        {
            for (int j = 0; j < arraySize - i - 1; j++)
            {
                HighlightElement(j, true); 
                if (array[j] > array[j + 1])
                {
                    SwapElements(j, j + 1, true);
                    yield return new WaitForSeconds(1f / sortingSpeed);
                }
                DehighlightAllElements();
            }
        }
    }

    //------------------------------------------- QUICK SORT
    IEnumerator QuickSort(int low, int high)
    {
        if (low < high)
        {
            int pivot = array[high];
            int i = (low - 1);

            for (int j = low; j < high; j++)
            {
                HighlightElement(j, true);
                if (array[j] < pivot)
                {
                    i++;
                    SwapElements(i, j, true);
                    yield return new WaitForSeconds(1f / sortingSpeed);
                }
                HighlightElement(j, false); 
            }

            SwapElements(i + 1, high, true);
            yield return new WaitForSeconds(1f / sortingSpeed);

            DehighlightAllElements();

            int pi = i + 1;

            yield return StartCoroutine(QuickSort(low, pi - 1));
            yield return StartCoroutine(QuickSort(pi + 1, high));
        }
    }

    //------------------------------------------- MERGE SORT
    IEnumerator MergeSort(int left, int right)
    {
        if (left < right)
        {
            int middle = left + (right - left) / 2;

            yield return StartCoroutine(MergeSort(left, middle));
            yield return StartCoroutine(MergeSort(middle + 1, right));

            yield return StartCoroutine(Merge(left, middle, right));
        }
    }
    IEnumerator Merge(int left, int middle, int right)
    {
        int[] leftArray = new int[middle - left + 1];
        int[] rightArray = new int[right - middle];

        Array.Copy(array, left, leftArray, 0, middle - left + 1);
        Array.Copy(array, middle + 1, rightArray, 0, right - middle);

        int i = 0, j = 0;
        int k = left;
        while (i < leftArray.Length && j < rightArray.Length)
        {
            HighlightElement(k, true);
            if (leftArray[i] <= rightArray[j])
            {
                array[k] = leftArray[i];
                i++;
            }
            else
            {
                array[k] = rightArray[j];
                j++;
            }
            UpdateElement(k, array[k]);
            yield return new WaitForSeconds(1f / sortingSpeed);
            HighlightElement(k, false);
            k++;
        }

        while (i < leftArray.Length)
        {
            array[k] = leftArray[i];
            UpdateElement(k, array[k]);
            HighlightElement(k, true);
            yield return new WaitForSeconds(1f / sortingSpeed);
            HighlightElement(k, false);
            i++;
            k++;
        }

        while (j < rightArray.Length)
        {
            array[k] = rightArray[j];
            UpdateElement(k, array[k]);
            HighlightElement(k, true);
            yield return new WaitForSeconds(1f / sortingSpeed);
            HighlightElement(k, false);
            j++;
            k++;
        }
    }
    void UpdateElement(int index, int value)
    {
        array[index] = value;
        if (index >= 0 && index < elementObjects.Length)
        {
            elementObjects[index].transform.localScale = new Vector3(1, value, 1);
            elementObjects[index].GetComponent<Renderer>().material.color = GetColor(index, arraySize);
        }
    }

    //------------------------------------------- QUCIK3 SORT
    IEnumerator Quick3Sort(int low, int high)
    {
        if (low < high)
        {
            int lt = low, gt = high;
            int pivot = array[low];
            int i = low;

            while (i <= gt)
            {
                HighlightElement(i, true); 
                if (array[i] < pivot)
                {
                    SwapElements(lt++, i++, true);
                }
                else if (array[i] > pivot)
                {
                    SwapElements(i, gt--, true);
                }
                else
                {
                    i++;
                }
                yield return new WaitForSeconds(1f / sortingSpeed);
                HighlightElement(i - 1, false); 
            }

            DehighlightAllElements();
            yield return StartCoroutine(Quick3Sort(low, lt - 1));
            yield return StartCoroutine(Quick3Sort(gt + 1, high));
        }
    }

    //------------------------------------------- HEAP SORT
    IEnumerator HeapSort()
    {
        int n = array.Length;

        for (int i = n / 2 - 1; i >= 0; i--)
        {
            yield return StartCoroutine(Heapify(n, i));
        }
        for (int i = n - 1; i > 0; i--)
        {
            HighlightElement(0, true);
            HighlightElement(i, true);
            SwapElements(0, i, true);
            yield return new WaitForSeconds(1f / sortingSpeed);
            DehighlightAllElements();

            yield return StartCoroutine(Heapify(i, 0));
        }
    }
    IEnumerator Heapify(int n, int i)
    {
        int largest = i; 
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        if (left < n)
        {
            HighlightElement(left, true);
            if (array[left] > array[largest])
            {
                largest = left;
            }
        }

        if (right < n)
        {
            HighlightElement(right, true);
            if (array[right] > array[largest])
            {
                largest = right;
            }
        }

        if (largest != i)
        {
            SwapElements(i, largest, true);
            yield return new WaitForSeconds(1f / sortingSpeed);

            yield return StartCoroutine(Heapify(n, largest));
        }

        if (left < n) HighlightElement(left, false);
        if (right < n) HighlightElement(right, false);
    }

    //------------------------------------------- SHELL SORT
    IEnumerator ShellSort()
    {
        int n = array.Length;

        for (int gap = n / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < n; i += 1)
            {
                int temp = array[i];
                int j;
                HighlightElement(i, true); 

                for (j = i; j >= gap && array[j - gap] > temp; j -= gap)
                {
                    HighlightElement(j - gap, true); 
                    array[j] = array[j - gap];
                    UpdateElement(j, array[j]);
                    yield return new WaitForSeconds(1f / sortingSpeed);
                    HighlightElement(j - gap, false); 
                }

                array[j] = temp;
                UpdateElement(j, temp);
                HighlightElement(i, false); 
                yield return new WaitForSeconds(1f / sortingSpeed);
            }
        }
    }

    //------------------------------------------- SELECTION SORT
    IEnumerator SelectionSort()
    {
        int n = array.Length;

        for (int i = 0; i < n - 1; i++)
        {
            int minIdx = i;
            HighlightElement(minIdx, true);

            for (int j = i + 1; j < n; j++)
            {
                HighlightElement(j, true); 
                if (array[j] < array[minIdx])
                {
                    HighlightElement(minIdx, false); 
                    minIdx = j;
                    HighlightElement(minIdx, true); 
                }
                yield return new WaitForSeconds(1f / sortingSpeed);
                HighlightElement(j, false); 
            }

            SwapElements(i, minIdx, true); 
            DehighlightAllElements(); 
            yield return new WaitForSeconds(1f / sortingSpeed);
        }

        DehighlightAllElements();
    }

    //------------------------------------------- INSERTION SORT
    IEnumerator InsertionSort()
    {
        int n = array.Length;

        for (int i = 1; i < n; ++i)
        {
            int key = array[i];
            int j = i - 1;
            while (j >= 0 && array[j] > key)
            {
                HighlightElement(j, true); 
                array[j + 1] = array[j];
                UpdateElement(j + 1, array[j + 1]);
                yield return new WaitForSeconds(1f / sortingSpeed);
                HighlightElement(j, false); 
                j = j - 1;
            }
            array[j + 1] = key;
            UpdateElement(j + 1, key);

            if (i != j + 1)
            {
                HighlightElement(j + 1, true);
                yield return new WaitForSeconds(1f / sortingSpeed);
                HighlightElement(j + 1, false);
            }
            DehighlightAllElements();
        }

        DehighlightAllElements();
    }

}

