namespace PLCsimAdvAPI;

public struct Address
{
    public uint Offset { get; set; }
    public uint Length { get; set; }

    public Address(uint offset, uint length)
    {
        Offset = offset;
        Length = length;
    }

    public override string ToString() => $"Start={Offset}, Offset={Length}";
}