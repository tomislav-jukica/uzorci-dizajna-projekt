using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.Composite.Iterator
{
    public abstract class Iterator
    {        
        public abstract int Key(); //Return the key of the current element       
        public abstract TvrtkaComponent Current(); //Return the current element        
        public abstract bool MoveNext(); //Move to the next element
        public abstract bool MovePrevious(); //Move to the previous element
        public abstract void Reset(); //Rewinds the Iterator to the first element
        public abstract bool IsEnd(); //Check if you have reached the end
        public abstract List<TvrtkaComponent> DFS(); //Depth-first search
        public abstract List<TvrtkaComponent> DFS(List<TvrtkaComponent> lista);
    }
}
