using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS_alpha1
{

    public enum UnitAction { FindMine, MoveToMine, Mine, FindForge, MoveToForge, DepositAtForge, WanderStart, WanderWalk }

    public class oUnit
    {
        public string Name;
        public Guid Guid;
        public float Health;
        public int Cargo;
        public int Capacity;
        public float MineTime;
        public float MineRate;
        public float Speed;
        public float PosX;
        public float PosY;
        public Guid LocationGuid;
        
        public int X { get { return (int)PosX; } }
        public int Y { get { return (int)PosY; } }

        public UnitAction CurrentAction;

        public List<UnitAction> Actions;

        public oStep _destination;
        public List<oPathStep> _path;
        private float _mineTimer;
        private int _pathBlockedCnt = 0;

        public oUnit()
        {
            Name = "?";
            Guid = Guid.NewGuid();
            Health = 10;
            Cargo = 0;
            Capacity = 4;
            Speed = (float)Global.rnd.NextDouble() + 1.0f;
            CurrentAction = UnitAction.FindMine;
            Actions = new List<UnitAction>();
            MineTime = 2;
            MineRate = 0.5f;

            _destination = null;
            _path = new List<oPathStep>();
            _mineTimer = 0;
        }

        public void Update()
        {
            switch(CurrentAction)
            {
                case UnitAction.FindMine:
                    FindMine();
                    break;
                case UnitAction.MoveToMine:
                    MoveToMine();
                    break;
                case UnitAction.Mine:
                    Mine();
                    break;
                case UnitAction.FindForge:
                    FindForge();
                    break;
                case UnitAction.MoveToForge:
                    MoveToForge();
                    break;
                case UnitAction.DepositAtForge:
                    DepositAtForge();
                    break;
                case UnitAction.WanderStart:
                    WanderStart();
                    break;
                case UnitAction.WanderWalk:
                    WanderWalk();
                    break;
            }
        }

        private void FindMine()
        {
            oLocationMine mine = Engine.FindMine(this.X, this.Y);
            if (mine != null)
            {
                oNode node = Engine.FindNodeByLocationGuid(mine.LocationGuid);
                _destination =  new oStep(node.X, node.Y);

                oPathfinder pathFinder = new oPathfinder(Engine.Nodes, X, Y, _destination);
                pathFinder.UnitGuid = this.Guid;

                _path = pathFinder.FindPath();
                _pathBlockedCnt = 0;

                if (_path.Count < 2)
                {
                    CurrentAction = UnitAction.WanderStart;
                }
                else
                {
                    CurrentAction = UnitAction.MoveToMine;
                }
            }
            else
            {
                //?? boom? fuck!!

            }
        }
        private void MoveToMine()
        {
            if (_path.Count > 0)
            {
                oPathStep _next = _path[_path.Count - 1];
                if (Engine.NodeIsEmpty(Guid, _next.X, _next.Y))
                {
                    _path.RemoveAt(_path.Count - 1);
                    PosX = _next.X;
                    PosY = _next.Y;
                }
                else
                {
                    _pathBlockedCnt += 1;
                    if (_pathBlockedCnt > 5)
                    {
                        CurrentAction = UnitAction.WanderStart;
                    }
                }
            }
            else
            {
                LocationGuid = Engine.FindNodeByCoor(X, Y).LocationGuid;
                _mineTimer = 0;
                CurrentAction = UnitAction.Mine;
            }
        }
        private void Mine()
        {
            _mineTimer += MineRate;
            if (_mineTimer >= MineTime)
            {
                int value = 0;
                value = Engine.Mine(LocationGuid);

                Cargo += value;
                if (Cargo == Capacity || value == 0)
                {
                    LocationGuid = Guid.Empty;
                    CurrentAction = UnitAction.FindForge;
                }
                else
                {
                    _mineTimer = 0;
                }
            }
        }
        private void FindForge()
        {
            oLocationForge forge = Engine.FindForge();
            if (forge != null)
            {
                oNode node = Engine.FindNodeByLocationGuid(forge.LocationGuid);
                _destination = new oStep(node.X, node.Y);

                oPathfinder pathFinder = new oPathfinder(Engine.Nodes, X, Y, _destination);
                pathFinder.UnitGuid = this.Guid;

                _path = pathFinder.FindPath();

                CurrentAction = UnitAction.MoveToForge;
            }
            else
            {
                //?? boom? fuck!!

            }

        }
        private void MoveToForge()
        {
            if (_path.Count > 0)
            {
                oPathStep _next = _path[_path.Count - 1];
                if (Engine.NodeIsEmpty(Guid, _next.X, _next.Y))
                {
                    _path.RemoveAt(_path.Count - 1);
                    PosX = _next.X;
                    PosY = _next.Y;
                }
            }
            else
            {
                LocationGuid = Engine.FindNodeByCoor(X, Y).LocationGuid;
                CurrentAction = UnitAction.DepositAtForge;
            }
        }
        private void DepositAtForge()
        {
            if (Cargo > 0)
            {
                oLocation l = Engine.FindLocationByGuid(LocationGuid);
                if (l != null && l.GetType() == typeof(oLocationForge))
                {
                    Engine.Deposit(LocationGuid);
                    //oLocationForge loc = (oLocationForge)l;
                    //loc.Supply += 1;
                    Cargo -= 1;
                }
                else
                {
                    CurrentAction = UnitAction.FindForge;
                }
            }
            else
            {
                CurrentAction = UnitAction.FindMine;
            }
        }
        private void WanderStart()
        {
            oPathStep node = Engine.FindRandom();
            if (node != null)
            {
                _destination = new oStep(node.X, node.Y);

                oPathfinder pathFinder = new oPathfinder(Engine.Nodes, X, Y, _destination);
                pathFinder.UnitGuid = this.Guid;

                _path = pathFinder.FindPath();
                _pathBlockedCnt = 0;

                CurrentAction = UnitAction.MoveToMine;
            }
            else
            {
                //?? boom? fuck!!

            }
        }
        private void WanderWalk()
        {
            if (_path.Count > 0)
            {
                oPathStep _next = _path[_path.Count - 1];
                if (Engine.NodeIsEmpty(Guid, _next.X, _next.Y))
                {
                    _path.RemoveAt(_path.Count - 1);
                    PosX = _next.X;
                    PosY = _next.Y;
                }
                else
                {
                    _pathBlockedCnt += 1;
                    if (_pathBlockedCnt > 5)
                    {
                        CurrentAction = UnitAction.WanderStart;
                    }
                }
            }
            else
            {
                LocationGuid = Engine.FindNodeByCoor(X, Y).LocationGuid;
                _mineTimer = 0;
                CurrentAction = UnitAction.FindMine;
            }
        }
    }
}
