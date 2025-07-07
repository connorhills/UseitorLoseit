using System.Collections;
using System.Collections.Generic;

public class Data
{
    public double chips;

    public List<double> clickUpgradeLevel;
    
    public Data()
    {
        chips = 0;

        clickUpgradeLevel = MethodManager.CreateList<double>(capacity: 5);
    }
}
