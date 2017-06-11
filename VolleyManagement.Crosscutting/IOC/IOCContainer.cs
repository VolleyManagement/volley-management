namespace VolleyManagement.Crosscutting.IOC
{
    public class IOCContainer
    {
        public IOCContainer RegisterSingleton<TContract, TImpl>()
        {
            return this;
        }

        public IOCContainer RegisterScoped<TContract, TImpl>()
        {
            return this;
        }


    }
}
