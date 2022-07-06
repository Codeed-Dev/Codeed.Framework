using System;

namespace Codeed.Framework.Domain.Tests.Events.Utils
{
    internal class NotEquatableEventWithGenericProperties : Event
    {
        protected override bool Equatable => false;

        public NotEquatableEventWithGenericProperties(string text, double number, DateTimeOffset date)
        {
            Text = text;
            Number = number;
            Date = date;
        }

        public string Text { get; }
        public double Number { get; }
        public DateTimeOffset Date { get; }
    }
}
