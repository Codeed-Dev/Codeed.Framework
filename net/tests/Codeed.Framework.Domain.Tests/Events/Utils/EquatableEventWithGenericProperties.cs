using System;
using System.Collections.Generic;

namespace Codeed.Framework.Domain.Tests.Events.Utils
{
    internal class EquatableEventWithGenericProperties : Event
    {
        protected override bool Equatable => true;

        public EquatableEventWithGenericProperties(string text, double number, DateTimeOffset date)
        {
            Text = text;
            Number = number;
            Date = date;
        }

        public string Text { get; }
        public double Number { get; }
        public DateTimeOffset Date { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Text;
            yield return Number;
            yield return Date;
        }
    }
}
