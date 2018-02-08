namespace VolleyManagement.UnitTests.Admin.Comparers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using UI.Areas.Admin.Models;

    /// <summary>
    /// Compares message instances
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MessageViewModelComparer : IComparer<MessageViewModel>, IComparer
    {
        public int Compare(MessageViewModel x, MessageViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        public int Compare(object x, object y)
        {
            MessageViewModel firstMessage = x as MessageViewModel;
            MessageViewModel secondMessage = y as MessageViewModel;

            if (firstMessage == null)
            {
                return -1;
            }
            else if (secondMessage == null)
            {
                return 1;
            }

            return Compare(firstMessage, secondMessage);
        }

        private bool AreEqual(MessageViewModel x, MessageViewModel y)
        {
            return x.Id == y.Id &&
                   x.Message == y.Message;
        }
    }
}