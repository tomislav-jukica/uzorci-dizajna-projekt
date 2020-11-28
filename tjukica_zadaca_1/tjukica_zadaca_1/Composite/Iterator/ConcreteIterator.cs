using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.Composite.Iterator
{
    class ConcreteIterator : Iterator
    {
        private List<TvrtkaComponent> collection;
        private OrgJedinica pocetna;
        private int position = 0;
        
        public ConcreteIterator(OrgJedinica orgJedinica)
        {
            this.pocetna = orgJedinica;
            this.collection = new List<TvrtkaComponent>(orgJedinica.getChildrenComponents());
        }

        public override TvrtkaComponent Current()
        {
            return this.collection[position];
        }

        public override bool IsEnd()
        {
            return (position == collection.Count) ? true : false;
            
        }

        public override int Key()
        {
            return this.position;
        }

        public override bool MoveNext()
        {
            int updatedPosition = ++this.position;
            if (updatedPosition >= 0 && updatedPosition < this.collection.Count)
            {
                this.position = updatedPosition;
                return true;
            } else
            {
                return false;
            }
        }

        public override bool MovePrevious()
        {
            int updatedPosition = this.position--;
            if (updatedPosition >= 0 && updatedPosition < this.collection.Count)
            {
                this.position = updatedPosition;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Reset()
        {
            this.position = 0;
        }

        public override List<TvrtkaComponent> DFS()
        {

            for (int i = 0; i < collection.Count; i++)
            {
                if (collection[i].getChildrenComponents().Count > 0)
                {
                    collection.InsertRange(i + 1, collection[i].getChildrenComponents());
                }
            }
            collection.Insert(0, pocetna);
            return this.collection;
        }

        
    }
}
