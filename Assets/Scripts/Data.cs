using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Data
{
    public double chips;

    public List<int> clickUpgradeLevel;
    public List<int> autoGenUpgradeLevel;
    public List<int> critChanceUpgradeLevel;
    
    public Data()
    {
        chips = 0;

        clickUpgradeLevel = new int[8].ToList();
        autoGenUpgradeLevel = new int[11].ToList();
        critChanceUpgradeLevel = new int[9].ToList();
    }
}
