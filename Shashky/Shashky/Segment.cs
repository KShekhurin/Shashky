using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shashky
{
    class Segment
    {
        public TypeOfSegment type = TypeOfSegment.Null;

        public bool mouseOn = false;

        public bool isTarget = false;

        public int eatX = -1;
        public int eatY = -1;

        public Segment()
        {

        }
    }
}
