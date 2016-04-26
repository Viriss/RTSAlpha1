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
        public static oLocationMine FindMine()
        {
            List<oLocationMine> list = new List<oLocationMine>();

            foreach(oLocation l in Locations)
            {
                if (l.LocationType == LocationType.IronMine)
                {
                    if (((oLocationMine)l).Supply > 0)
                    {
                        list.Add((oLocationMine)l);
                    }
                }
            }

            if (list.Count > 0)
            {
                return list[Global.rnd.Next(list.Count())];
            }

            return null;
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
            oNode node;

            node = FindEmptyNode();
            if (node != null)
            {
                oLocationMine mine;
                mine = new oLocationMine();
                Locations.Add(mine);
                node.LocationGuid = mine.LocationGuid;
            }

            node = FindEmptyNode();
            if (node != null)
            {
                oLocationMine mine;
                mine = new oLocationMine();
                Locations.Add(mine);
                node.LocationGuid = mine.LocationGuid;
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
        private static void MakeUnits()
        {
            oUnit u = new oUnit();
            oNode n = FindEmptyNode();

            u.PosX = n.X;
            u.PosY = n.Y;
            u.Name = "1";

            Units.Add(u);
        }

    }
}
