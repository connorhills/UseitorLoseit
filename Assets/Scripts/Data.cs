using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Data
{
    public double chips;

    public List<int> clickUpgradeLevel;
    public List<int> autoGenUpgradeLevel;
    
    public Data()
    {
        chips = 0;

        clickUpgradeLevel = new int[5].ToList();
        autoGenUpgradeLevel = new int[5].ToList();
    }
}
