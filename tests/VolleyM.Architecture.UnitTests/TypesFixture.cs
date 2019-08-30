using NetArchTest.Rules;

namespace VolleyM.Architecture.UnitTests
{
    public class TypesFixture
    {
        internal static Types AllProjectTypes()
        {
            return Types.InAssemblies(AssembliesFixture.AllAssemblies);
        }
    }
}