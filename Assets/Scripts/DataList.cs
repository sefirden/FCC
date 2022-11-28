using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataList : MonoBehaviour
{
    void Start()
    {
        
    }


}

public class Data
{
    public string Date { get; set; }    
    public string Input { get; set; }
    public string ConvertFrom { get; set; }
    public string Result { get; set; }
    public string ConvertTo { get; set; }
    

    public Data(string D, string I, string Cf, string R, string Ct)//в таком порядке заполняем данные
    {
        Date = D;
        Input = I;
        ConvertFrom = Cf;
        Result = R;
        ConvertTo = Ct;
    }
}
