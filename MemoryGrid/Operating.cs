using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MemoryGrid
{
    public static class Operating
    {
        public static List<Picture> myGallery;

        static Operating()
        {
            myGallery = new List<Picture>();
        }

        public static string GettingImgPaths()
        {
            string dirPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory())) + @"\Fruits";
            string supportedExtensions = "*.png";
            myGallery = new List<Picture>();
            foreach (string imageFile in Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories).Where(s => supportedExtensions.Contains(System.IO.Path.GetExtension(s).ToLower())))
            {
                Picture temp = new Picture();
                temp.ImgPath = imageFile;
                myGallery.Add(temp);
            }
            return myGallery[0].ImgPath;

        }

        public static void FillingMatrix()
        {
            int min = 1; //first cell index
            int max = 26; //last sell index minus one
            int randomTemp = 0;
            List<int> randomIndexList = new List<int>();
            Random random = new Random();

            for (int i = 0; i < myGallery.Count; i++)
            {
                if (i == myGallery.Count - 1)// recheck here
                {
                    List<int> myList2 = Enumerable.Range(min, max - min).ToList();
                    List<int> remaining = myList2.Except(randomIndexList).ToList();
                    myGallery[i].IndexRandom1 = remaining[0];
                }
                else
                {
                    do
                    {
                        randomTemp = random.Next(min, max);
                        if (myGallery[i].IndexRandom1 < 0 && randomIndexList.IndexOf(randomTemp) == -1)
                        {
                            myGallery[i].IndexRandom1 = randomTemp;
                            randomIndexList.Add(randomTemp);
                        }
                        if (myGallery[i].IndexRandom2 < 0 && randomIndexList.IndexOf(randomTemp) == -1)
                        {
                            myGallery[i].IndexRandom2 = randomTemp;
                            randomIndexList.Add(randomTemp);
                        }
                    }
                    while (myGallery[i].IndexRandom1 < 0 || myGallery[i].IndexRandom2 < 0);
                }
            }

        }

    }
}
