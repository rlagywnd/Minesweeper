using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

[System.Serializable]
public struct Array2D<T>
{
    public T[] array;
    public int Length { 
        get 
        { 
            return array.Length;
        }
    }
    public T this[int index]
    {
        get
        { 
            return array[index];
        }
        set
        { 
            array[index] = value;
        }
    }
}
