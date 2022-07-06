using System.Collections.Generic;

namespace Codeed.Framework.Domain.Tests.Events.Utils
{
    internal class EquatableEventWithoutProperties : Event
    {
        protected override bool Equatable => true;
    }
}
