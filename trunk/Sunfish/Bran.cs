using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sunfish.ValueTypes;
using Sunfish.Developmental;

namespace Sunfish
{
    class Bran
    {
        Map map;
        Queue<TagIndex> queue;

        public Bran()
        {
            queue = new Queue<TagIndex>();
        }

        public void Decompile(Map map)
        {
            this.map = map;
            //TO DO
            //Decompile a map from the cache format into a format usable and optimized for Sunfish
            //Handle deocmpiling of tag classes into different classes
            TagIndex tagindex = map.Index.ScenarioTagId;
            //scan the tag, and strip out strings, tags, and raws
            queue.Enqueue(tagindex);
            Decompile();
        }

        private void Decompile()
        {
            while (queue.Count > 0)
            {
                TagIndex tagindex = queue.Dequeue();
                switch (map.Index.TagEntries[tagindex.Index].Type.ToString())
                {
                    case "unic":
                        break;
                    default:

                        break;
                }
            }
        }
    }
}