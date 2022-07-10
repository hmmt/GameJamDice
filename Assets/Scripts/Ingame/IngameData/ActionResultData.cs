public struct ActionResultData
{
    public UnitStatusData unit;
    public int value;

    public ActionResultData(UnitStatusData unit, int value)
    {
        this.unit = unit;
        this.value = value;
    }
}
