using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.Composite
{
    public abstract class TvrtkaComponent
    {
        public int id;
        public TvrtkaComponent nadredeni;
        public TvrtkaComponent(int componentId, TvrtkaComponent nadred)
        {
            id = componentId;
            nadredeni = nadred;
        }
        public abstract TvrtkaComponent getParentComponent();
        public abstract TvrtkaComponent getComponent(int componentId);
        public abstract string getComponentName();

    }
}
