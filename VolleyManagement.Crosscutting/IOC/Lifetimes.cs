namespace VolleyManagement.Crosscutting.IOC
{
    /// <summary>
    /// Contains type of lifetimes for types which should be resolved by IOC
    /// </summary>
    public enum Lifetimes
    {
        Singleton,
        Transient,
        Scoped
    }
}
