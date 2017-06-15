namespace VolleyManagement.Crosscutting.IOC
{
    public interface IIOCRegistrationModule
    {
        void RegisterDependencies(IOCContainer container);
    }
}
