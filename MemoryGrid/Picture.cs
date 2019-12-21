using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryGrid
{

    public class Picture
    {
        public string ImgPath { get; set; }
        public int IndexRandom1 { get; set; }
        public int IndexRandom2 { get; set; }
        public int IndexAnswer1 { get; set; }
        public int IndexAnswer2 { get; set; }
        public bool CorrectAnswer { get; set; }
        public Picture()
        {
            ImgPath = "";
            IndexRandom1 = -1;
            IndexRandom2 = -1;
            IndexAnswer1 = 0;
            IndexAnswer2 = 0;
            CorrectAnswer = false;
        }
    }
}
