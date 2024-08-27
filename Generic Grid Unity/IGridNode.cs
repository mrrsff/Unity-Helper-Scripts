using System.Collections.Generic;

namespace AlphaGame.Core.GridSystem
{
    public interface IGridNode
    {
        public List<IGridNode> GetNeighbours();

        public void SetGrid(Grid<IGridNode> grid);
    }
}
