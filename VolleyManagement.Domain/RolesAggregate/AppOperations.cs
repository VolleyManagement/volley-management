namespace VolleyManagement.Domain.RolesAggregate
{
    using System;
    using System.Collections.Generic;

    public static class AppOperations
    {
        #region Constants

        private const byte TOURNAMENT_AREA = 0x00;

        #endregion

        public static readonly TournamentOperations Tournaments = new TournamentOperations(TOURNAMENT_AREA);

        public class TournamentOperations : AreaOperations
        {
            public TournamentOperations(byte areaId) : base(areaId)
            {
                //// NOTE: It is very important to not change the order of operations if DB has entries with existing 
                //// bindings roles to operations
                AreaOperationsInitializer.InitArea(this)
                                         .WithOperation(x => Create = x)
                                         .WithOperation(x => Edit = x)
                                         .WithOperation(x => Delete = x);
            }

            public AppAreaOperation Create { get; set; }

            public AppAreaOperation Edit { get; set; }

            public AppAreaOperation Delete { get; set; }
        }

        public class AreaOperations
        {
            public byte Area { get; set; }
            public AreaOperations(byte areaId)
            {
                Area = areaId;
            }
        }

        internal class AreaOperationsInitializer 
        {
            private byte _areaId;
            private byte _operationsCounter;

            internal static AreaOperationsInitializer InitArea<T>(T instance) where T: AreaOperations
            {
                return new AreaOperationsInitializer() { _areaId = instance.Area };
            }

            internal AreaOperationsInitializer WithOperation(Action<short> propertySetter)
            {
                propertySetter(GetAreaOperationId(_areaId, ++_operationsCounter));
                return this;
            }

            private short GetAreaOperationId(byte areaId, byte operationId)
            {
                return BitConverter.ToInt16(new byte[] { areaId, operationId }, 0);
            }  
        }
    }
}
