using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;
using HTF2020.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class WalkManager
    {        
        private const int MIN_DIRECTION = 11;
        private const int MAX_DIRECTION = 14;
        private const int OPPOSITE_DIRECTION = 2;
        private const int MINIMUM_HITPOINTS = 40;

        public TurnAction CurrentDirection { get; set; }
        public PlayTurnRequest Request { get; set; }

        public WalkManager(PlayTurnRequest request, TurnAction currentDirection)
        {
            CurrentDirection = currentDirection;
            Request = request;
        }

        public Turn Walk()
        {
            TurnAction direction = NextDirectionCounterClockwise(CurrentDirection);

            if (Request.PossibleActions.Contains(TurnAction.Loot))
            {
                return new Turn(TurnAction.Loot);
            }
            else if (Request.PossibleActions.Contains(TurnAction.Attack))
            {
                if (Request.PossibleActions.Contains(TurnAction.DrinkPotion) && Request.IsCombat && Request.PartyMember.CurrentHealthPoints < MINIMUM_HITPOINTS)
                {
                    return new Turn(TurnAction.DrinkPotion);
                }
                return new Turn(TurnAction.Attack);
            }
            else if (Request.PossibleActions.Contains(direction))
            {
                CurrentDirection = direction;
            }
            else if (!Request.PossibleActions.Contains(CurrentDirection))
            {
                CurrentDirection = GetNextPossibleDirection(direction);
            }
            return new Turn(CurrentDirection);
        }

        public TurnAction NextDirectionCounterClockwise(TurnAction currentDirection)
        {
            int nextDirection = (int)currentDirection - 1;
            if (nextDirection < MIN_DIRECTION)
            {
                return (TurnAction)MAX_DIRECTION;
            }
            return (TurnAction)nextDirection;
        }

        public TurnAction GetNextPossibleDirection(TurnAction direction)
        {
            bool canMove = false;
            TurnAction nextDirection = NextDirectionCounterClockwise(direction);

            while (!canMove)
            {
                if (TotalDirections() == 1)
                {
                    canMove = true;
                }
                else 
                { 
                    if (Request.PossibleActions.Contains(nextDirection) && nextDirection != CurrentDirection - OPPOSITE_DIRECTION || nextDirection != CurrentDirection + OPPOSITE_DIRECTION)
                    {
                     canMove = true;
                    }               
                    nextDirection = NextDirectionCounterClockwise(nextDirection);
                }
            }
            return nextDirection;
        }

        public int TotalDirections()
        {
            int total = 0;
            foreach (var direction in Request.PossibleActions)
            {
                if ((int)direction >= MIN_DIRECTION && (int)direction <= MAX_DIRECTION)
                {
                    total++;
                }
            }
            return total;
        }     
    }
}
