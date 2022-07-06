using Codeed.Framework.Domain.Tests.Events.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Codeed.Framework.Domain.Tests
{
    public class EventTests
    {
        [Fact]
        public void should_validate_if_not_equatable_events_without_properties_are_not_equals()
        {
            var event1 = new NotEquatableEventWithoutProperties();
            var event2 = new NotEquatableEventWithoutProperties();

            Assert.NotEqual(event1, event2);
            Assert.False(event1.Equals(event2));
        }
        

        [Fact]
        public void should_validate_if_equatable_events_without_properties_are_equals()
        {
            var event1 = new EquatableEventWithoutProperties();
            var event2 = new EquatableEventWithoutProperties();

            Assert.Equal(event1, event2);
            Assert.True(event1.Equals(event2));
        }

        [Fact]
        public void should_validate_if_not_equatable_events_with_generic_properties_are_not_equals()
        {
            var date = DateTimeOffset.Now;
            var event1 = new NotEquatableEventWithGenericProperties("teste", 0, date);
            var event2 = new NotEquatableEventWithGenericProperties("teste", 0, date);

            Assert.NotEqual(event1, event2);
            Assert.False(event1.Equals(event2));
        }

        [Fact]
        public void should_validate_i_equatable_events_with_generic_properties_are_equals()
        {
            var date = DateTimeOffset.Now;
            var event1 = new EquatableEventWithGenericProperties("teste", 0, date);
            var event2 = new EquatableEventWithGenericProperties("teste", 0, date);

            Assert.Equal(event1, event2);
            Assert.True(event1.Equals(event2));
        }

        [Fact]
        public void should_validate_equals_events_using_distinct()
        {
            var notEquatableEvent1 = new NotEquatableEventWithoutProperties();
            var notEquatableEvent2 = new NotEquatableEventWithoutProperties();

            var equatableEvent1 = new EquatableEventWithoutProperties();
            var equatableEvent2 = new EquatableEventWithoutProperties();

            var events = new List<Event>()
            {
                notEquatableEvent1,
                notEquatableEvent2,
                equatableEvent1,
                equatableEvent2
            };

            var distinctList = events.Distinct().ToList();

            Assert.Collection(distinctList,
                (event1) => Assert.Same(event1, notEquatableEvent1),
                (event2) => Assert.Same(event2, notEquatableEvent2),
                (event3) => Assert.Same(event3, equatableEvent1));
        }
    }
}