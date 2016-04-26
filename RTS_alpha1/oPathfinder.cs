using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS_alpha1
{
    public enum SearchDirection { Up, Down, Left, Right, NW, NE, SW, SE }

    public class oPathfinder
    {
        public List<oNode> Nodes;
        public int X;
        public int Y;
        public oStep Destination;

        private List<oPathStep> _steps;
        private List<oPathStep> _lookAtNext;
        private List<oPathStep> _temp;
        private int _dist;
        private bool foundDestination = false;

        public oPathfinder(List<oNode> Nodes, int X, int Y, oStep Destination)
        {
            this.Nodes = Nodes;
            this.X = X;
            this.Y = Y;
            this.Destination = Destination;

            _steps = new List<oPathStep>();
            _lookAtNext = new List<oPathStep>();
            _temp = new List<oPathStep>();
        }

        public List<oPathStep> FindPath()
        {
            List<oPathStep> result = new List<oPathStep>();

            _dist = 0;
            _lookAtNext.Add(new oPathStep(X, Y));
            StepLookAt();



            //return _steps;
            return BuildRouteFromPath();
        }

        public void StepLookAt()
        {
            if (foundDestination) { return; }

            oPathStep next;

            foreach(oPathStep p in _lookAtNext)
            {
                p.Distance = _dist;
                _steps.Add(p);

                next = GoDirection(p, SearchDirection.Down);
                if (next != null) { _temp.Add(next); }

                next = GoDirection(p, SearchDirection.Up);
                if (next != null) { _temp.Add(next); }

                next = GoDirection(p, SearchDirection.Left);
                if (next != null) { _temp.Add(next); }

                next = GoDirection(p, SearchDirection.Right);
                if (next != null) { _temp.Add(next); }

                next = GoDirection(p, SearchDirection.NW);
                if (next != null) { _temp.Add(next); }

                next = GoDirection(p, SearchDirection.NE);
                if (next != null) { _temp.Add(next); }

                next = GoDirection(p, SearchDirection.SW);
                if (next != null) { _temp.Add(next); }

                next = GoDirection(p, SearchDirection.SE);
                if (next != null) { _temp.Add(next); }
            }

            _dist += 1;

            _lookAtNext.Clear();
            _lookAtNext.AddRange(_temp);
            _temp.Clear();

            foreach (oPathStep ps in _steps)
            {
                if (ps.X == Destination.X && ps.Y == Destination.Y)
                {
                    foundDestination = true;
                    return;
                }
            }

            if (_lookAtNext.Count > 0) { StepLookAt(); }
        }
        private List<oPathStep> BuildRouteFromPath()
        {
            int StartDist = -1;
            List<oPathStep> _result = new List<oPathStep>();
            oPathStep _next = null;
            oPathStep _prev = null;

            foreach(oPathStep s in _steps)
            {
                if (s.X == Destination.X && s.Y == Destination.Y)
                {
                    _result.Add(s);
                    _prev = s;
                    StartDist = s.Distance - 1;
                    break;
                }
            }

            while (StartDist > 0)
            {
                _next = FindNextStepInPath(StartDist, _prev);

                if (_next == null)
                {
                    break;
                }
                else
                {
                    _prev = _next;
                    _result.Add(_next);
                    StartDist -= 1;
                }
            }

            return _result;
        }
        private oPathStep FindNextStepInPath(int StepNumber, oPathStep FromStep) 
        {
            foreach(oPathStep s in _steps)
            {
                if (s.Distance == StepNumber && isNextToNode(FromStep, s))
                {
                    return s;
                }
            }
            return null;
        }
        private bool isNextToNode(oPathStep From, oPathStep To)
        {
            if (From.X <= To.X + 1 && From.X >= To.X - 1)
            {
                if (From.Y <= To.Y + 1 && From.Y >= To.Y - 1)
                {
                    return true;
                }
            }
            return false;
        }
        private oPathStep GoDirection(oPathStep FromStep, SearchDirection Direction)
        {
            int X = FromStep.X;
            int Y = FromStep.Y;
            oNode n;

            switch (Direction)
            {
                case SearchDirection.Down:
                    Y -= 1;
                    break;
                case SearchDirection.Left:
                    X += 1;
                    break;
                case SearchDirection.Right:
                    X -= 1;
                    break;
                case SearchDirection.Up:
                    Y += 1;
                    break;
                case SearchDirection.NW:
                    Y += 1;
                    X += 1;
                    break;
                case SearchDirection.NE:
                    Y += 1;
                    X -= 1;
                    break;
                case SearchDirection.SW:
                    Y -= 1;
                    X += 1;
                    break;
                case SearchDirection.SE:
                    Y -= 1;
                    X -= 1;
                    break;
            }

            if (X < 0) { return null; }
            if (X == Engine.Width) { return null; }
            if (Y < 0) { return null; }
            if (Y == Engine.Height) { return null; }

            foreach (oPathStep ps in _steps)
            {
                if (ps.X == X && ps.Y == Y) { return null; }
            }
            foreach (oPathStep ps in _lookAtNext)
            {
                if (ps.X == X && ps.Y == Y) { return null; }
            }
            foreach(oPathStep ps in _temp)
            {
                if (ps.X == X && ps.Y == Y) { return null; }
            }

            //n = Nodes[X + (Y * Engine.Width)];

            //if (n.LocationGuid == Guid.Empty)
            //{
                return new oPathStep(X, Y);
            //}

            //return null;
        }
    }
}
