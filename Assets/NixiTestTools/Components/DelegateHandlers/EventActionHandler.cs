using NixiTestTools.Extensions;
using NixiTestTools.Components.Dtos;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NixiTestTools.Components.DelegateHandlers
{
    // TODO : Tests
    // TODO : Refacto
    // TODO : Comments
    internal sealed class EventActionHandler<TMock>
        where TMock : class
    {
        private readonly List<EventActionWithTimes<TMock>> eventActionWithTimes = new List<EventActionWithTimes<TMock>>();
        private readonly Mock<TMock> mock;

        internal EventActionHandler(Mock<TMock> mock)
        {
            this.mock = mock;
        }

        internal void AddExpressionEvent(Action<TMock> mockEventAction, bool isAdding, int nbTimes = 1)
        {
            ISetup setupFound = GetSetupEventIfExists(mockEventAction, isAdding);

            if (setupFound != null)
            {
                EventActionWithTimes<TMock> eventActionFound = GetRegisteredEventAction(setupFound);
                eventActionFound.NbTimes += nbTimes;
            }
            else
            {
                SetupEvent(mockEventAction, isAdding);

                eventActionWithTimes.Add(new EventActionWithTimes<TMock>
                {
                    EventAction = mockEventAction,
                    SetupRegistered = mock.Setups.Last(),
                    IsAdding = isAdding,
                    NbTimes = nbTimes
                });
            }
        }

        private ISetup GetSetupEventIfExists(Action<TMock> mockEventAction, bool isAdding)
        {
            if (isAdding)
            {
                return mock.GetSetupAddIfExists(mockEventAction);
            }
            else
            {
                return mock.GetSetupRemoveIfExists(mockEventAction);
            }
        }

        private void SetupEvent(Action<TMock> mockEventAction, bool isAdding)
        {
            if (isAdding)
            {
                mock.SetupAdd(mockEventAction);
            }
            else
            {
                mock.SetupRemove(mockEventAction);
            }
        }

        private EventActionWithTimes<TMock> GetRegisteredEventAction(ISetup setup)
        {
            return eventActionWithTimes.SingleOrDefault(x => x.SetupRegistered.ToString() == setup.ToString());
        }

        internal void VerifyAll()
        {
            foreach (EventActionWithTimes<TMock> eventActionAdd in eventActionWithTimes.Where(x => x.IsAdding))
            {
                mock.VerifyAdd(eventActionAdd.EventAction, eventActionAdd.Times);
            }

            foreach (EventActionWithTimes<TMock> eventActionAdd in eventActionWithTimes.Where(x => !x.IsAdding))
            {
                mock.VerifyRemove(eventActionAdd.EventAction, eventActionAdd.Times);
            }
        }
    }
}
