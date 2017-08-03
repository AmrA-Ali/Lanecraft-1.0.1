public class CheckThereAreMaps : Checker {
    public override int Check()
    {
        return CountMaps.ThereAreMaps ? 1 : 0;
    }
}
