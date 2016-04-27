using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS_alpha1
{
    public static class Engine
    {
        public static int Height;
        public static int Width;

        public static List<oUnit> Units;
        public static List<oNode> Nodes;
        public static List<oLocation> Locations;
        public static int Money;

        public static void Init(int GridHeight, int GridWidth)
        {
            Height = GridHeight;
            Width = GridWidth;
            Money = 0;

            Units = new List<oUnit>();
            Nodes = new List<oNode>();
            Locations = new List<oLocation>();

            MakeGrid();
            MakeLocations();
            MakeUnits();
        }

        public static void AddMoney(int Amount)
        {
            Money += Amount;
        }

        public static oNode FindEmptyNode()
        {
            bool isSearching = true;
            int x = Global.rnd.Next(Height * Width);

            while (isSearching)
            {
                if (Nodes[x].LocationGuid == Guid.Empty)
                {
                    isSearching = false;
                }
                else {
                    x = Global.rnd.Next(Height * Width);
                }
            }

            return Nodes[x];
        }
        public static oLocationForge FindForge()
        {
            List<oLocationForge> list = new List<oLocationForge>();

            foreach (oLocation l in Locations)
            {
                if (l.LocationType == LocationType.IronForge)
                {
                    list.Add((oLocationForge)l);
                }
            }

            if (list.Count > 0)
            {
                return list[Global.rnd.Next(list.Count())];
            }

            return null;
        }
        public static oLocation FindLocationByGuid(Guid LocationGuid)
        {
            foreach(oLocation loc in Locations)
            {
                if (loc.LocationGuid == LocationGuid) { return loc; }
            }
            return null;
        }
        public static oNode FindNodeByCoor(int X, int Y)
        {
            foreach (oNode node in Nodes)
            {
                if (node.X == X && node.Y == Y) { return node; }
            }
            return null;
        }
        public static oNode FindNodeByLocationGuid(Guid LocationGuid)
        {
            foreach(oNode node in Nodes)
            {
                if (node.LocationGuid == LocationGuid) { return node; }
            }
            return null;
        }
        public static oLocationMine FindMine(int X, int Y)
        {
            List<oLocationMine> list = new List<oLocationMine>();

            foreach(oLocation l in Locations)
            {
                if (l.LocationType == LocationType.IronMine)
                {
                    if (((oLocationMine)l).Supply > 0)
                    {
                        oLocationMine m = (oLocationMine)l;
                        m.Distance = DistanceFrom(m.LocationGuid, X, Y);
                        list.Add(m);
                    }
                }
            }

            if (list.Count > 0)
            {
                //random
                //return list[Global.rnd.Next(list.Count())];

                int dist = 9999;
                oLocationMine value = null;
                foreach(oLocationMine m in list)
                {
                    if (m.Distance < dist) { value = m; }
                }
                return value;
            }

            return null;
        }
        public static int DistanceFrom(Guid LocationGuid, int X, int Y)
        {
            oNode loc = FindNodeByLocationGuid(LocationGuid);
            int A;
            int B;
            int C;
            A = Math.Abs(loc.X - X);
            B = Math.Abs(loc.Y - Y);
            C = (int)Math.Sqrt(A ^ 2 + B ^ 2);
            return C;
        }
        public static oPathStep FindRandom()
        {
            int X = Global.rnd.Next(Width);
            int Y = Global.rnd.Next(Height);

            if (NodeIsEmpty(Guid.Empty, X, Y))
            {
                return new oPathStep(X, Y);
            }
            return null;
        }

        public static int Mine(Guid LocationGuid)
        {
            foreach (oLocation loc in Locations)
            {
                if (loc.LocationGuid == LocationGuid && loc.GetType() == typeof(oLocationMine))
                {
                    return ((oLocationMine)loc).Mine();
                }
            }
            return 0;
        }
        public static void Deposit(Guid LocationGuid)
        {
            foreach (oLocation loc in Locations)
            {
                if (loc.LocationGuid == LocationGuid && loc.GetType() == typeof(oLocationForge))
                {
                    ((oLocationForge)loc).Supply += 1;
                }
            }
        }
        public static bool NodeIsEmpty(Guid UnitID, int X, int Y)
        {
            foreach(oUnit u in Units)
            {
                if (u.Guid != UnitID)
                {
                    if (u.X == X && u.Y == Y) { return false; }
                }
            }
            return true;
        }
        /*
        public static List<oStep> FindPath(int X, int Y, oStep Destination)
                {

                }
        */
        public static void Update()
        {
            foreach (oLocation l in Locations)
            {
                if (l.LocationType == LocationType.IronForge)
                {
                    ((oLocationForge)l).Update();
                }
            }

            foreach (oUnit u in Units)
            {
                u.Update();
            }

            CleanUpMine();
        }
        private static void CleanUpMine()
        {
            bool foundEmpty = false;
            foreach (oLocation m in Locations)
            {
                if (m.GetType() == typeof(oLocationMine))
                {
                    if (((oLocationMine)m).Supply == 0)
                    {
                        Locations.Remove(m);
                        foundEmpty = true;
                        break;
                    }
                }
            }
            if (foundEmpty) { CleanUpMine(); }
        }

        private static void MakeGrid()
        {
            for(int x = 0; x < Height * Width; x++)
            {
                oNode n = new oNode(x, Width);
                Nodes.Add(n);
            }
        }
        private static void MakeLocations()
        {
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();
            MakeMine();

            oNode node;
            node = FindEmptyNode();
            if (node != null)
            {
                oLocationForge forge;
                forge = new oLocationForge();
                Locations.Add(forge);
                node.LocationGuid = forge.LocationGuid;
            }
            node = FindEmptyNode();
            if (node != null)
            {
                oLocationForge forge;
                forge = new oLocationForge();
                Locations.Add(forge);
                node.LocationGuid = forge.LocationGuid;
            }
            node = FindEmptyNode();
            if (node != null)
            {
                oLocationForge forge;
                forge = new oLocationForge();
                Locations.Add(forge);
                node.LocationGuid = forge.LocationGuid;
            }
        }
        private static void MakeMine()
        {
            oNode node = null;
            while (node == null)
            {
                node = FindEmptyNode();
                if (node != null)
                {
                    oLocationMine mine;
                    mine = new oLocationMine();
                    Locations.Add(mine);
                    node.LocationGuid = mine.LocationGuid;
                }
            }
        }
        private static void MakeUnits()
        {
            //AddUnit("I", 0.5f, 2);
            //AddUnit("J", 0.25f, 3);
            //AddUnit("K", 0.3f, 3);
            //AddUnit("L", 0.1f, 5);

            //AddUnit("A", 0.5f, 2);
            //AddUnit("B", 0.25f, 3);
            //AddUnit("C", 0.3f, 3);
            //AddUnit("D", 0.1f, 5);

            //AddUnit("E", 0.5f, 2);
            //AddUnit("F", 0.25f, 3);
            //AddUnit("G", 0.3f, 3);
            //AddUnit("H", 0.1f, 5);

            //AddUnit("M", 0.5f, 2);
            //AddUnit("N", 0.25f, 3);
            //AddUnit("O", 0.3f, 3);
            //AddUnit("P", 0.1f, 5);

            //AddUnit("Q", 0.5f, 2);
            //AddUnit("R", 0.25f, 3);
            //AddUnit("S", 0.3f, 3);
            //AddUnit("T", 0.1f, 5);

            //AddUnit("U", 0.5f, 2);
            //AddUnit("V", 0.25f, 3);
            AddUnit("W", 0.3f, 3);
            AddUnit("X", 0.1f, 5);

        }
        private static void AddUnit(string Name, float MineRate, int MineTime)
        {
            oUnit u = new oUnit();
            oNode n = FindEmptyNode();

            u.PosX = n.X;
            u.PosY = n.Y;
            u.Name = Name;
            u.MineRate = MineRate;
            u.MineTime = MineTime;

            Units.Add(u);
        }

    }
}
