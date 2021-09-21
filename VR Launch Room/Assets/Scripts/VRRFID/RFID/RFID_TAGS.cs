public enum RFID_Tag
{
    TBN,
    A02,
    Inline
}

public class RFID_Transponder
{
    private string type;
    private string manufacturer;
    private string chipType;

    public string Type
    {
        get => type;
        set => type = value;
    }

    public string Manufacturer
    {
        get => manufacturer;
        set => manufacturer = value;
    }

    public string ChipType
    {
        get => chipType;
        set => chipType = value;
    }
}