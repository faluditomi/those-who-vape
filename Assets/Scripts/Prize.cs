using UnityEngine;

public class Prize
{
    public VapeController.VapeType vapeType;
    public int quantity;

    public Prize(VapeController.VapeType vapeType, int quantity)
    {
        this.vapeType = vapeType;
        this.quantity = quantity;
    }

    public void Double()
    {
        quantity *= 2;
    }
}
